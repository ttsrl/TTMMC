﻿using TTMMC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTMMC.ConfigurationModels;
using static TTMMC.Models.LayoutListenItem;

namespace TTMMC.Models
{
    public interface IMachine 
    {
        int Id { get; }
        string Description { get; }
        string ReferenceName { get; }
        string Address { get; }
        string Port { get; }
        MachineType Type { get; }
        ConnectionProtocol ConnectionProtocol { get; }
        bool HaveImage { get; }
        bool Recording { get; set; }
        MachineStatus Status { get; }
        ModalityLog ModalityLogCheck { get; }
        int ValueModalityLogCheck { get; }
        DataItem ReferenceKeyRead { get; }
        DataItem FinishKeyRead { get; }
        DataItem FinishKeyWrite { get; }

        void Connect();
        MachineStatus GetStatus();
        string GetImageUrl();
        Task<T> ReadAsync<T>(string key);
        Task<string> ReadAsync(string key, Type type);
        T Read<T>(string key);
        string Read(string key, Type type);
        Type GetDataItemType(DataItem data);
        Dictionary<string, List<DataItem>> GetParametersRead();
        Dictionary<string, List<DataItem>> GetParametersWrite();
        List<DataItem> GetParameterRead(string name);
        List<DataItem> GetParameterWrite(string name);
    }

}
