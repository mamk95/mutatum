namespace Changelog.Data;

using System.ComponentModel.DataAnnotations;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the background color of the category.
    /// Can be a CSS color name or a hex color (with included hashtag before the hex).
    /// </summary>
    [Required]
    public string BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the text color of the category.
    /// Can be a CSS color name or a hex color (with included hashtag before the hex).
    /// </summary>
    [Required]
    public string TextColor { get; set; }

    public IList<Change> Changes { get; set; }
}
