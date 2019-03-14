using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTMMC.ConfigurationModels;
using TTMMC.Models.DBModels;

namespace TTMMC.Models.ViewModels
{
    public class DiagnosticModel
    {
        public List<IMachine> Machines { get; set; }
        public List<KeepAliveRequest> KeepAliveRequests { get; set; }
        public Dictionary<string, bool> Services { get; set; }
    }
}
