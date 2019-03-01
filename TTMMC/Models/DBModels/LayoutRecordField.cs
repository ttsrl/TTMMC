using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TTMMC.Models.DBModels
{
    public class LayoutRecordField
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Key { get; set; }
        [StringLength(255)]
        public string Value { get; set; }
    }
}
