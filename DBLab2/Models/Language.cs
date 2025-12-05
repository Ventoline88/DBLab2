using System;
using System.Collections.Generic;

namespace DBLab2.Models;

public partial class Language
{
    public int LanguageId { get; set; }

    public string? LanguageName { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
