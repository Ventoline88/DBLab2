using System;
using System.Collections.Generic;

namespace DBLab2.Models;

public partial class StoreInventory
{
    public int StoreId { get; set; }

    public string Isbn13 { get; set; } = null!;

    public int Amount { get; set; }

    public virtual Book Isbn13Navigation { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}
