using Microsoft.EntityFrameworkCore;

namespace Changelog.Data
{
    public class ReleaseService
    {
        private readonly ApplicationDbContext _context;

        public ReleaseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Release GetReleaseById(int id)
        {
            return _context.Releases
                .Where(r => r.Id == id)
                .Include(r => r.Changes)
                .ThenInclude(c => c.ChangeType)
                .FirstOrDefault();
        }

        public Release UpdateRelease(int id, Release release)
        {
            var originalRelease = _context.Releases.Find(id);

            if (originalRelease == null)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            if (originalRelease.Id != id)
            {
                throw new ArgumentException("Id missmatch in arguments", nameof(release));
            }

            var result = _context.Releases.Update(release).Entity;
            _context.SaveChanges();

            return result;
        }

        public void DeleteReleaseById(int id)
        {
            var release = _context.Releases.Find(id);

            if (release == null)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            _context.Releases.Remove(release);

            _context.SaveChanges();
        }
    }
}
