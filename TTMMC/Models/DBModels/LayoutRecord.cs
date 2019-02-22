using System;
using System.Collections.Generic;

namespace TTMMC.Models.DBModels
{
    public class LayoutRecord
    {
        private DateTime? _timestamp;
        public int Id { get; set; }
        public List<LayoutRecordField> Fields { get; set; }
        public DateTime Timestamp
        {
            get => _timestamp ?? DateTime.Now;
            set => _timestamp = value;
        }
    }
}
