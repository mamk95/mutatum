using Microsoft.EntityFrameworkCore;

namespace Changelog.Data
{
    public class ProjectService
    {
        private readonly ApplicationDbContext _context;

        public ProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Project> GetProjects()
        {
            return _context.Projects.OrderBy(p => p.SortOrder).ToList();
        }

        public Project GetProjectById(int id)
        {
            return _context.Projects
                .Where(p => p.Id == id)
                .Include(p => p.Releases
                    .OrderBy(r => r.ReleaseYear)
                    .ThenBy(r => r.ReleaseMonth)
                    .ThenBy(r => r.ReleaseDay)
                    .ThenBy(r => r.Major)
                    .ThenBy(r => r.Minor)
                    .ThenBy(r => r.Patch))
                .ThenInclude(r => r.Changes)
                .ThenInclude(c => c.ChangeType)
                .FirstOrDefault();
        }

        public Project AddProject(string name, string description, bool hidden)
        {
            var project = new Project
            {
                Name = name,
                Description = description,
                Hidden = hidden,
            };

            var result = _context.Projects.Add(project).Entity;
            _context.SaveChanges();

            return result;
        }

        public Project UpdateProject(int id, string name, string description, bool hidden)
        {
            var project = _context.Projects.Find(id);

            if (project == null)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            project.Name = name;
            project.Description = description;
            project.Hidden = hidden;

            var result = _context.Projects.Update(project).Entity;
            _context.SaveChanges();

            return result;
        }

        public void DeleteProjectById(int id)
        {
            var project = _context.Projects.Find(id);

            if (project == null)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            _context.Projects.Remove(project);

            _context.SaveChanges();
        }
    }
}
