namespace DBLab2.Models;

public partial class Book
{
    public string Isbn13 { get; set; } = null!;

    public string? Title { get; set; }

    public int LanguageId { get; set; }

    public decimal? Price { get; set; }

    public DateOnly? DatePublished { get; set; }

    public int AuthorId { get; set; }

    public int PublisherId { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual Language Language { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Publisher Publisher { get; set; } = null!;

    public virtual ICollection<StoreInventory> StoreInventories { get; set; } = new List<StoreInventory>();
}
