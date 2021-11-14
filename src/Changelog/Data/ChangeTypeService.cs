namespace Changelog.Data
{
    public class ChangeTypeService
    {
        private readonly ApplicationDbContext _context;

        public ChangeTypeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ChangeType> GetChangeTypes()
        {
            return _context.ChangeTypes.ToList();
        }
    }
}
