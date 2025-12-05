using System;
using System.Collections.Generic;

namespace DBLab2.Models;

public partial class Store
{
    public int StoreId { get; set; }

    public string? StoreName { get; set; }

    public string StoreAddress { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<StoreInventory> StoreInventories { get; set; } = new List<StoreInventory>();
}
