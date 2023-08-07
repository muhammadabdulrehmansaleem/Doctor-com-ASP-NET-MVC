using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Doctor
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string Specilization { get; set; } = null!;
}
