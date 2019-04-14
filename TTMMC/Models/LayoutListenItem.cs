using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTMMC.ConfigurationModels;
using TTMMC.Models.DBModels;
using TTMMC.Services;

namespace TTMMC.Models
{
    public class LayoutListenItem
    {
        private Layout _layout;
        private long workCount = 0;
        private bool _isBusy = false;
        private int timerTick = 5;
        private readonly IMachine _machine;
        private bool _roundedValue = false;
        private int _roundedPrecision = 2;
        private int attualXTimes = 1;
        private Timer _timer;
        private readonly DBContext _dB;

        public int TimerTick { get => timerTick; set => timerTick = value; }
        public bool IsBusy { get => _isBusy; }
        public long WorkCount { get => workCount; }
        public Layout Layout { get => _layout; }
        public IMachine Machine { get => _machine; }
        public static int DefaultTimerTick = 5;
        public bool Rounded { get => _roundedValue; set => _roundedValue = value; }
        public int RoundedPrecision { get => _roundedPrecision; set => _roundedPrecision = value; }

        private string _referenceKeyLogOld = "";

        private readonly IHostingEnvironment _environment;

        public LayoutListenItem(Layout layout, IMachine machine, IHostingEnvironment iHostingEnvironment)
        {
            _machine = machine;
            _layout = layout;
            _dB = DBContext.Instance;
            _environment = iHostingEnvironment;
        }

        public async Task Start()
        {
            var ll = await _dB.Layouts.Include(l => l.LayoutActRecords).FirstOrDefaultAsync(l => l.Id == _layout.Id);
            if (ll is Layout)
            {
                _layout = ll;
                if (ll.Status == Status.Waiting)
                {
                    ll.Status = Status.Recording;
                }

                if (_machine.ReferenceKeyRead == null)
                    return;

                attualXTimes = _machine.ValueModalityLogCheck;
                _referenceKeyLogOld = await _machine.ReadAsync(_machine.ReferenceKeyRead.Address, _machine.GetDataItemType(_machine.ReferenceKeyRead));

                //log sets
                var writes = _machine.GetParametersWrite();
                var fields = new List<LayoutRecordField>();
                foreach (var par in writes)
                {
                    var newIt = new Dictionary<string, string>();
                    foreach (var dataIt in par.Value)
                    {
                        var type = _machine.GetDataItemType(dataIt);
                        var val = _machine.Read(dataIt.Address, type) ?? "";
                        if (type != typeof(string) && (val.Contains(",") || val.Contains(".")))
                        {
                            var decimalVal = double.Parse(val);
                            val = (Rounded) ? Math.Round(decimalVal, RoundedPrecision).ToString() : decimalVal.ToString();
                        }
                        newIt.Add(par.Value.IndexOf(dataIt).ToString(), val);
                    }
                    var json = (newIt.Count > 1) ? JsonConvert.SerializeObject(newIt) : ((newIt.Count == 1) ? newIt.ElementAt(0).Value : "");
                    fields.Add(new LayoutRecordField { Key = par.Key, Value = json });
                }

                var record = new LayoutRecord
                {
                    Fields = fields,
                    Timestamp = DateTime.Now
                };

                ll.LayoutSetRecord = record;
                ll.Timestamp = DateTime.Now;
                await _dB.SaveChangesAsync();

                _timer = new Timer(Do, null, TimeSpan.Zero, TimeSpan.FromSeconds(timerTick));
                _isBusy = true;
                _machine.Recording = true;
            }
        }

        private async void Do(object state)
        {
            if (await isChangedReferenceKey())
            {
                var fields = new List<LayoutRecordField>();
                var acts = _machine.GetParametersRead();
                foreach (var act in acts)
                {
                    if (act.Key.Substring(0, 1) != "[" && act.Key.Substring(act.Key.Length - 1, 1) != "]") // se non è una proprietà nascosta
                    {
                        var newIt = new Dictionary<string, string>();
                        foreach (var dataIt in act.Value)
                        {
                            var type = _machine.GetDataItemType(dataIt);
                            var val = _machine.Read(dataIt.Address, type) ?? "";
                            if (type != typeof(string))
                            {
                                if (type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong))
                                {
                                    if (dataIt.Scaling > 0)
                                    {
                                        double floatVal = double.Parse(val);
                                        for (var i = 0; i < dataIt.Scaling; i++)
                                        {
                                            floatVal = floatVal / 10.0;
                                        }
                                        val = floatVal.ToString();
                                    }
                                }
                                if ((val.Contains(",") || val.Contains(".")))
                                {
                                    var decimalVal = double.Parse(val);
                                    val = (Rounded) ? Math.Round(decimalVal, RoundedPrecision).ToString() : decimalVal.ToString();
                                }
                            }
                            newIt.Add(act.Value.IndexOf(dataIt).ToString(), val);
                        }
                        var json = (newIt.Count > 1) ? JsonConvert.SerializeObject(newIt) : ((newIt.Count == 1) ? newIt.ElementAt(0).Value : "0");
                        fields.Add(new LayoutRecordField { Key = act.Key, Value = json });
                    }
                }
                var record = new LayoutRecord
                {
                    Fields = fields
                };
                _layout.LayoutActRecords.Add(record);
                await _dB.SaveChangesAsync();
                workCount += 1;
            }

            //check finish
            if (_machine.FinishKeyRead != null && _machine.FinishKeyWrite != null)
            {
                var finKeyRead = await _machine.ReadAsync(_machine.FinishKeyRead.Address, _machine.GetDataItemType(_machine.FinishKeyRead));
                var finKeyWrite = await _machine.ReadAsync(_machine.FinishKeyWrite.Address, _machine.GetDataItemType(_machine.FinishKeyWrite));
                if (finKeyRead == finKeyWrite)
                {
                    _layout.Status = Status.Finished;
                    _isBusy = false;
                    _machine.Recording = false;
                    _timer?.Change(Timeout.Infinite, 0);
                }
            }
        }

        private async Task<bool> isChangedReferenceKey()
        {
            //prendo la reference key e prendo il primo valore della ref
            var di = _machine.ReferenceKeyRead;
            var diType = _machine.GetDataItemType(di) ?? typeof(int);
            var actV = await _machine.ReadAsync(di.Address, diType);

            if (diType == typeof(string))
            {
                if (actV != _referenceKeyLogOld)
                {
                    _referenceKeyLogOld = actV;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                var doubleActV = double.Parse(actV);
                if (_machine.ModalityLogCheck == ModalityLog.RefKeyReadGreater0)
                {
                    if (doubleActV > 0.0)
                        return true;
                }
                else if (_machine.ModalityLogCheck == ModalityLog.RefKeyReadGreaterOld)
                {
                    var doubleRefKeyLogOld = double.Parse(_referenceKeyLogOld);
                    if (doubleActV > doubleRefKeyLogOld)
                    {
                        _referenceKeyLogOld = doubleActV.ToString();
                        return true;
                    }
                }
                else if (_machine.ModalityLogCheck == ModalityLog.RefKeyReadGreater0EveryXTimes)
                {
                    if (doubleActV > 0.0)
                    {
                        if (attualXTimes >= _machine.ValueModalityLogCheck)
                        {
                            attualXTimes = 1;
                            return true;
                        }
                        else
                        {
                            attualXTimes++;
                        }
                    }
                }
                else if (_machine.ModalityLogCheck == ModalityLog.RefKeyReadGreaterOldEveryXTimes)
                {
                    var doubleRefKeyLogOld = double.Parse(_referenceKeyLogOld);
                    if (doubleActV > doubleRefKeyLogOld)
                    {
                        _referenceKeyLogOld = doubleActV.ToString();
                        if (attualXTimes >= _machine.ValueModalityLogCheck)
                        {
                            attualXTimes = 1;
                            return true;
                        }
                        else
                        {
                            attualXTimes++;
                        }
                    }
                }
            }
            return false;
        }

        public async Task Stop()
        {
            if (_layout.Status == Status.Recording)
            {
                _layout.Status = Status.Stopped;
                await _dB.SaveChangesAsync();
            }
            _isBusy = false;
            _machine.Recording = false;
            _timer?.Change(Timeout.Infinite, 0);
        }

        public void Dispose()
        {
            _layout = null;
            workCount = 0;
            _timer?.Dispose();
        }
    }
}
