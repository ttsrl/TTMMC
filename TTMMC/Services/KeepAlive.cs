using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TTMMC.Models.DBModels;

namespace TTMMC.Services
{
    public class KeepAlive
    {
        private int _count = 0;
        private string Host = "";
        private readonly DBContext _dB;
        private static readonly HttpClient client = new HttpClient();

        private Timer _timer;
        private int timerTick = 900; //15 minuti

        public int Count { get => _count; set => _count = value; }

        private static bool started = false;
        public static bool Started { get => started; }

        private bool first = false;

        public KeepAlive(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor == null || httpContextAccessor.HttpContext.Request == null)
                return;

            started = true;
            _dB = DBContext.Instance;
            Host = httpContextAccessor.HttpContext.Request.Host.ToUriComponent();

            client.Timeout = TimeSpan.FromSeconds(180); // 3 minuti

            //timer
            _timer = new Timer(Request, null, TimeSpan.Zero, TimeSpan.FromSeconds(timerTick));
        }

        private async void Request(object state)
        {
            if (!first)
            {
                var kar = new KeepAliveRequest
                {
                    DurationRequest = 0,
                    Response = 200,
                    Inizialize = true
                };
                first = true;
            }
            else
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                HttpResponseMessage response1 = await client.GetAsync("http://" + Host);
                HttpResponseMessage response = await client.GetAsync("http://" + Host + "/machine");
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;

                var kar = new KeepAliveRequest
                {
                    DurationRequest = Convert.ToInt32(elapsedMs),
                    Response = (int)response.StatusCode,
                    Inizialize = false
                };
            }
            //_dB.KeepAliveRequests.Add(kar);
            //await _dB.SaveChangesAsync();
            _count++;
        }
    }
}
