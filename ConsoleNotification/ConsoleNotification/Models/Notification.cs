﻿using System;
using System.Collections.Generic;

#nullable disable

namespace ConsoleNotification.Models
{
    public partial class Notification
    {
        public int Id { get; set; }
        public string ProjectId { get; set; }
        public string ProjectCategory { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectDocument { get; set; }
        public DateTime TargetDate { get; set; }
        public string AssignedBy { get; set; }
        public string AssignedFor { get; set; }
        public int Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
