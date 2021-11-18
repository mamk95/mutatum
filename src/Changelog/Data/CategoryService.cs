using Microsoft.EntityFrameworkCore;

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

        public IList<Category> GetCategoriesWithRelatedChanges()
        {
            return _context.Categories.Include(c => c.Changes).ToList();
        }

        public Category GetCategoryById(int id)
        {
            return _context.Categories.FirstOrDefault(p => p.Id == id);
        }

        public Category AddCategory(string name, string backgroundColor, string textColor)
        {
            var category = new Category
            {
                Name = name,
                BackgroundColor = backgroundColor,
                TextColor = textColor,
            };

            var result = _context.Categories.Add(category).Entity;
            _context.SaveChanges();

            return result;
        }

        public Category UpdateCategory(int id, string name, string backgroundColor, string textColor)
        {
            var category = _context.Categories.Find(id);

            if (category == null)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            category.Name = name;
            category.BackgroundColor = backgroundColor;
            category.TextColor = textColor;

            var result = _context.Categories.Update(category).Entity;
            _context.SaveChanges();

            return result;
        }

        public void DeleteCategoryById(int id)
        {
            var category = _context.Categories.Where(c => c.Id == id).Include(c => c.Changes).FirstOrDefault();

            if (category == null)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            if (category.Changes.Count > 0)
            {
                throw new ArgumentException("Cannot delete category with active changes");
            }

            _context.Categories.Remove(category);

            _context.SaveChanges();
        }
    }
}
