﻿using TTMMC.Services;
using System;
using System.Collections.Generic;
using static TTMMC.Models.LayoutListenItem;

namespace TTMMC.ConfigurationModels
{
    public class Machine
    {
        public string Description { get; set; }
        public int Id { get; set; }
        private MachineType type;
        public MachineType Type { get => type; set => type = ((Enum.IsDefined(typeof(MachineType), value) ? value : MachineType.Null)); }
        public string ReferenceName { get; set; }
        private ConnectionProtocol protocol;
        public ConnectionProtocol Protocol { get => protocol; set => protocol = ((Enum.IsDefined(typeof(ConnectionProtocol), value) ? value : ConnectionProtocol.Null)); }
        public string Address { get; set; }
        public string Port { get; set; }
        public string Image { get; set; }
        public ModalityLog ModalityLogCheck { get; set; }
        public int ValueModalityLogCheck { get; set; }
        public string ReferenceKeyRead { get; set; }
        public string FinishKeyRead { get; set; }
        public string FinishKeyWrite { get; set; }
        public Dictionary<string, List<DataItem>> DatasAddressToRead { get; set; }
        public Dictionary<string, List<DataItem>> DatasAddressToWrite { get; set; }
    }

    public class DataItem
    {
        private string _dataType = "";
        private int _scaling = 0;
        public string Description { get; set; }
        public string Address { get; set; }
        public string DataType { get => _dataType.ToLower(); set => _dataType = value?.ToLower(); }
        public int Scaling { get => _scaling; set => _scaling = value; }
    }
}
