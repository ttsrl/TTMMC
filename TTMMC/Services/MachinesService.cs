using Hylasoft.Opc.Ua;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTMMC.ConfigurationModels;
using TTMMC.Models;

namespace TTMMC.Services
{
    public enum ConnectionProtocol
    {
        Null,
        OPCUA
    }

    public enum MachineType
    {
        Null,
        SD_Systec100,
    }

    public enum MachineStatus
    {
        Offline,
        Online
    }

    public enum ModalityLog
    {
        RefKeyReadGreater0,
        RefKeyReadGreaterOld,
        RefKeyReadGreater0EveryXTimes,
        RefKeyReadGreaterOldEveryXTimes,
    }

    public class MachinesService
    {
        private static List<IMachine> machines = new List<IMachine>();
        public int Count { get => machines.Count(); }
        private readonly Utilities _utils;

        private static bool started = false;
        public static bool Started { get => started; }

        public MachinesService([FromServices] Utilities utils)
        {
            started = true;
            _utils = utils;

            var configMachines = _utils.GetConfigurationElementsList<Machine>("Machines");
            if(configMachines.Count > 0)
            {
                foreach(var m in configMachines)
                {
                    if (m.Protocol == ConnectionProtocol.Null || m.Type == MachineType.Null)
                        continue;
                    if(m.Protocol == ConnectionProtocol.OPCUA)
                    {
                        var client = new OPCMachine(m);
                        client.Connect();
                        machines.Add(client);
                    }
                }
            }
        }

        public IEnumerable<IMachine> GetMachines()
        {
            return machines;
        }

        public static IEnumerable<IMachine> GetStaticMachines()
        {
            return machines;
        }

        public IMachine GetMachineById(int id)
        {
            foreach (var m in machines)
            {
                if (m.Id == id)
                {
                    return m;
                }
            }
            return null;
        }

        public IMachine GetMachineByName(string Name)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                foreach (var m in machines)
                {
                    if (m.ReferenceName == Name)
                    {
                        return m;
                    }
                }
            }
            return null;
        }

    }
}
