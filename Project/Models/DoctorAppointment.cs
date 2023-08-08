using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class DoctorAppointment
{
    public int Id { get; set; }

    public string? Department { get; set; }

    public string? Doctor { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime Date { get; set; }

    public DateTime Time { get; set; }

    public string? Address { get; set; }

    public string? Gender { get; set; }

    public string? Testtype { get; set; }

    public string? Lab { get; set; }

    public string AppointmentType { get; set; } = null!;

    public string? City { get; set; }
}
