namespace Changelog.Data
{
    public class CategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }
    }
}
