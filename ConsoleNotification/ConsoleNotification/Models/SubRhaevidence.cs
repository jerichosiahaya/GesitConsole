﻿using System;
using System.Collections.Generic;

#nullable disable

namespace ConsoleNotification.Models
{
    public partial class SubRhaevidence
    {
        public int Id { get; set; }
        public int? SubRhaId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
        public string FilePath { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Notes { get; set; }

        public virtual SubRha SubRha { get; set; }
    }
}
