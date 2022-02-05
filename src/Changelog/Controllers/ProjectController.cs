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

        [HttpGet("{id}")]
        public ActionResult<Project> GetProjectById(int id)
        {
            Project project = _projectService.GetProjectById(id, includeHidden: false);

            if (project == null)
                return NotFound();
            else
                return Ok(project);
        }
    }
}
