namespace Changelog.Data;

using Changelog.Data.Database;
using Microsoft.EntityFrameworkCore;

public class ReleaseService
{
    private readonly AppDbContext _context;

    public ReleaseService(AppDbContext context)
    {
        _context = context;
    }

    public Release GetReleaseById(int id)
    {
        return _context.Releases
            .Where(r => r.Id == id)
            .Include(r => r.Changes)
            .ThenInclude(c => c.Category)
            .FirstOrDefault();
    }

    public Release GetNewestRelease(bool includeHidden = false)
    {
        return _context.Releases
            .Where(r => r.Hidden == false || r.Hidden == includeHidden) // If includeHidden==true, we want both hidden and non-hidden releases
            .Where(r => r.Project.Hidden == false || r.Project.Hidden == includeHidden) // If includeHidden==true, we want both hidden and non-hidden projects
            .OrderByDescending(r => r.ReleaseYear)
                .ThenByDescending(r => r.ReleaseMonth)
                .ThenByDescending(r => r.ReleaseDay)
                .ThenByDescending(r => r.Major)
                .ThenByDescending(r => r.Minor)
                .ThenByDescending(r => r.Patch)
            .Include(r => r.Changes)
            .ThenInclude(c => c.Category)
            .FirstOrDefault();
    }

    public IList<Release> GetNewestReleases(int releaseLimit = 5, bool includeHidden = false)
    {
        return _context.Releases
            .Where(r => r.Hidden == false || r.Hidden == includeHidden) // If includeHidden==true, we want both hidden and non-hidden releases
            .Where(r => r.Project.Hidden == false || r.Project.Hidden == includeHidden) // If includeHidden==true, we want both hidden and non-hidden projects
            .OrderByDescending(r => r.ReleaseYear)
                .ThenByDescending(r => r.ReleaseMonth)
                .ThenByDescending(r => r.ReleaseDay)
                .ThenByDescending(r => r.Major)
                .ThenByDescending(r => r.Minor)
                .ThenByDescending(r => r.Patch)
            .Include(r => r.Changes)
            .ThenInclude(c => c.Category)
            .Take(releaseLimit)
            .ToList();
    }

    public Release CreateRelease(Release release)
    {
        if (release.Id != default)
        {
            throw new ArgumentException("Do not set the ID field", nameof(release));
        }

        Release result = _context.Releases.Add(release).Entity;
        _context.SaveChanges();

        return result;
    }

    public Release UpdateRelease(int id, Release release)
    {
        Release originalRelease = _context.Releases.Find(id);

        if (originalRelease == null)
        {
            throw new ArgumentOutOfRangeException(nameof(id));
        }

        if (originalRelease.Id != id)
        {
            throw new ArgumentException("Id missmatch in arguments", nameof(release));
        }

        Release result = _context.Releases.Update(release).Entity;
        _context.SaveChanges();

        return result;
    }

    public void DeleteReleaseById(int id)
    {
        Release release = _context.Releases.Find(id);

        if (release == null)
        {
            throw new ArgumentOutOfRangeException(nameof(id));
        }

        _context.Releases.Remove(release);

        _context.SaveChanges();
    }
}
