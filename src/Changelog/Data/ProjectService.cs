using Changelog.Data.Database;
using Microsoft.EntityFrameworkCore;

namespace Changelog.Data
{
    public class ProjectService
    {
        private readonly AppDbContext _context;

        public ProjectService(AppDbContext context)
        {
            _context = context;
        }

        public IList<Project> GetProjects()
        {
            return _context.Projects.OrderBy(p => p.SortOrder).ToList();
        }

        public IList<Project> GetNonHiddenProjects()
        {
            return _context.Projects
                .Where(p => p.Hidden == false)
                .OrderBy(p => p.SortOrder)
                .ToList();
        }

        public IList<(Project project, int numberOfReleases)> GetProjectsWithNumberOfReleases(int projectLimit = 3, bool includeHidden = false)
        {
            return _context.Projects
                .Where(p => p.Hidden == false || p.Hidden == includeHidden)
                .OrderByDescending(p => p.Releases.Count)
                .Take(projectLimit)
                .Select(p => new { project = p, numberOfReleases = p.Releases.Count })
                .AsEnumerable()
                .Select(x => (project: x.project, numberOfReleases: x.numberOfReleases))
                .ToList();
        }

        public IList<Project> GetProjectsWithReleases(int releaseLimit = 3, bool includeHidden = false)
        {
            return _context.Projects
                .Where(p => p.Hidden == false || p.Hidden == includeHidden)
                .OrderBy(p => p.SortOrder)
                .Include(p => p.Releases
                    .OrderByDescending(r => r.ReleaseYear)
                    .ThenByDescending(r => r.ReleaseMonth)
                    .ThenByDescending(r => r.ReleaseDay)
                    .ThenByDescending(r => r.Major)
                    .ThenByDescending(r => r.Minor)
                    .ThenByDescending(r => r.Patch)
                    .Take(releaseLimit))
                .ToList();
        }

        public Project GetProjectById(int id)
        {
            return _context.Projects
                .Where(p => p.Id == id)
                .Include(p => p.Releases
                    .OrderByDescending(r => r.ReleaseYear)
                    .ThenByDescending(r => r.ReleaseMonth)
                    .ThenByDescending(r => r.ReleaseDay)
                    .ThenByDescending(r => r.Major)
                    .ThenByDescending(r => r.Minor)
                    .ThenByDescending(r => r.Patch))
                .ThenInclude(r => r.Changes)
                .ThenInclude(c => c.Category)
                .FirstOrDefault();
        }

        public Project AddProject(string name, string description, bool hidden, int sortOrder)
        {
            var project = new Project
            {
                Name = name,
                Description = description,
                Hidden = hidden,
                SortOrder = sortOrder,
            };

            Project result = _context.Projects.Add(project).Entity;
            _context.SaveChanges();

            return result;
        }

        public Project UpdateProject(int id, string name, string description, bool hidden, int sortOrder)
        {
            Project project = _context.Projects.Find(id);

            if (project == null)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            project.Name = name;
            project.Description = description;
            project.Hidden = hidden;
            project.SortOrder = sortOrder;

            Project result = _context.Projects.Update(project).Entity;
            _context.SaveChanges();

            return result;
        }

        public void DeleteProjectById(int id)
        {
            Project project = _context.Projects.Find(id);

            if (project == null)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            _context.Projects.Remove(project);

            _context.SaveChanges();
        }
    }
}
