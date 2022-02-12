using FluentValidation;

namespace Changelog.Data;

public class ProjectValidator : AbstractValidator<Project>
{
    private readonly ProjectService _projectService;

    public ProjectValidator(ProjectService projectService)
    {
        _projectService = projectService;

        RuleFor(p => p.UrlSlug)
            .Must(SlugUnique)
            .WithMessage("Slug already in use");
    }

    private bool SlugUnique(Project project, string slug)
    {
        return !_projectService.IsSlugUsed(slug, project.Id);
    }
}
