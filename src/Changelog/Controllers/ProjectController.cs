using Changelog.Data;
using Microsoft.AspNetCore.Mvc;

namespace Changelog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectService _projectService;

        public ProjectController(ProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet("{slug}")]
        public ActionResult<Project> GetProjectBySlug(string slug)
        {
            Project project = _projectService.GetProjectBySlug(slug, includeHidden: false);

            if (project == null)
                return NotFound();
            else
                return Ok(project);
        }
    }
}
