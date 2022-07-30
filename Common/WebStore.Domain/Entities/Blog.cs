using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.Entities;

[Index(nameof(Title), IsUnique = true)]
public class Blog : Entity
{
    [Required]
    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public int? Stars { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}
