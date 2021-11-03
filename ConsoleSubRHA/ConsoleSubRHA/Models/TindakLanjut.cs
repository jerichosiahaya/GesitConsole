using System;
using System.Collections.Generic;

#nullable disable

namespace GesitConsole.Models
{
    public partial class TindakLanjut
    {
        public TindakLanjut()
        {
            TindakLanjutEvidences = new HashSet<TindakLanjutEvidence>();
        }

        public int Id { get; set; }
        public int? SubRhaId { get; set; }
        public string Notes { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
        public string FilePath { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual SubRha SubRha { get; set; }
        public virtual ICollection<TindakLanjutEvidence> TindakLanjutEvidences { get; set; }
    }
}
