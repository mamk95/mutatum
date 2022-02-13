﻿namespace Changelog.Data;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Change
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }

    public Category Category { get; set; }

    [Required]
    [ForeignKey(nameof(Release))]
    public int ReleaseId { get; set; }

    public Release Release { get; set; }

    [Required]
    public string Title { get; set; }

    public string Markdown { get; set; }
}
