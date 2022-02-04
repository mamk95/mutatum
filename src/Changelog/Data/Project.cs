using System.ComponentModel.DataAnnotations;

namespace Changelog.Data
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool Hidden { get; set; }

        public int SortOrder { get; set; } = 99;

        public ICollection<Release> Releases { get; set; }
    }
}
