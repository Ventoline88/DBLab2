using System;
using System.Collections.Generic;

namespace DBLab2.Models;

public partial class Publisher
{
    public int PublisherId { get; set; }

    public string? PublisherName { get; set; }

    public string PublisherAddress { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
