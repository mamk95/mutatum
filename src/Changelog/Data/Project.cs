namespace Changelog.Data;

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

[Index(nameof(UrlSlug), IsUnique = true)]
public class Project
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [StringLength(maximumLength: 16, MinimumLength = 1)]
    [RegularExpression(@"^[a-z0-9\-]*$", ErrorMessage = "Only URL friendly characters allowed in slug (lowercase a-z, 0-9 and dashes).")]
    public string UrlSlug { get; set; }

    public string Description { get; set; }

    public bool Hidden { get; set; }

    public int SortOrder { get; set; } = 99;

    public ICollection<Release> Releases { get; set; }
}
