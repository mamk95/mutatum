using System.ComponentModel.DataAnnotations;

namespace Changelog.Data
{
    public class Project
    {
        [Key]
        public int Id { get; set; } 

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public bool Hidden { get; set; } = false;

        public int? SortOrder { get; set; } = int.MaxValue;

        public IList<Release>? Releases { get; set; }
    }
}
