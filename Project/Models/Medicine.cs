using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Medicine
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Quantity { get; set; }

    public int Price { get; set; }

    public byte[] Image { get; set; } = null!;
}
