using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Changelog.Data
{
    public class Change
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(ChangeType))]
        public int ChangeTypeId { get; set; }

        public ChangeType ChangeType { get; set; }

        [Required]
        [ForeignKey(nameof(Release))]
        public int ReleaseId { get; set; }

        public Release Release { get; set; }

        [Required]
        public string Title { get; set; }

        public string Markdown { get; set; }
    }
}
