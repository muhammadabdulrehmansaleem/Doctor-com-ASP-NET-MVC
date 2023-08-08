using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Chat
{
    public int MessageId { get; set; }

    public int PatientId { get; set; }

    public int DoctorId { get; set; }

    public string MessageContent { get; set; } = null!;

    public DateTime MessageTimestamp { get; set; }
}
