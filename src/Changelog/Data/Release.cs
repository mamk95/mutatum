using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Changelog.Data
{
    public class Release
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Project))]
        public int ProjectId { get; set; }

        public Project Project { get; set; }

        public string Title { get; set; }

        public bool Hidden { get; set; }

        [MaxLength(200)]
        public string ShortDescription { get; set; }

        public string LongDescriptionMarkdown { get; set; }

        public int? Major { get; set; }

        public int? Minor { get; set; }

        public int? Patch { get; set; }

        [NotMapped]
        public string VersionAndTitle
        {
            get
            {
                if (Major == null) return Title;
                else if (Title == null) return $"{Major}.{Minor}.{Patch}";
                else return $"{Major}.{Minor}.{Patch} - {Title}";
            }
        }

        [Range(1980, 2099)]
        public int ReleaseYear { get; set; }

        [Range(1, 12)]
        public int ReleaseMonth { get; set; }

        [Range(1, 31)]
        public int ReleaseDay { get; set; }

        [NotMapped]
        public string ReleaseDate => $"{ReleaseYear}/{ReleaseMonth.ToString().PadLeft(2, '0')}/{ReleaseDay.ToString().PadLeft(2, '0')}";

        public IList<Change> Changes { get; set; } = new List<Change>();
    }
}
