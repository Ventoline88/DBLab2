using System;
using System.Collections.Generic;

namespace DBLab2.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public string DeliveryAddress { get; set; } = null!;

    public int StoreId { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Store Store { get; set; } = null!;
}
