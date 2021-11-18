using System.ComponentModel.DataAnnotations;

namespace Changelog.Data
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Can be a CSS color name or a hex color (with included hashtag before the hex)
        /// </summary>
        [Required]
        public string BackgroundColor { get; set; }

        /// <summary>
        /// Can be a CSS color name or a hex color (with included hashtag before the hex)
        /// </summary>
        [Required]
        public string TextColor { get; set; }

        public IList<Change> Changes { get; set; }
    }
}
