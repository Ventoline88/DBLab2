using System;
using System.Collections.Generic;

namespace DBLab2.Models;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public string Isbn13 { get; set; } = null!;

    public int Amount { get; set; }

    public virtual Book Isbn13Navigation { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
