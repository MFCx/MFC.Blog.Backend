using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MFC.Blog.DataAccess.Concrete.EntityFrameworkCore.Context;
using MFC.Blog.DataAccess.Interfaces;
using MFC.Blog.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace MFC.Blog.DataAccess.Concrete.EntityFrameworkCore.Repositories
{
    public class EfBlogRepository : EfGenericRepository<Entities.Concrete.Blog>, IBlogDal
    {
        private readonly MFCBlogContext _context;

        public EfBlogRepository(MFCBlogContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Entities.Concrete.Blog>> GetAllByCategoryIdAsync(int categoryId)
        {

            return await _context.Blogs.Join(_context.CategoryBlogs, b => b.Id, cb => cb.BlogId, (blog, categoryBlog) => new
            {
                blog,
                categoryBlog
            }).Where(I => I.categoryBlog.CategoryId == categoryId).Select(I => new Entities.Concrete.Blog
            {
                AppUser = I.blog.AppUser,
                AppUserId = I.blog.AppUserId,
                CategoryBlogs = I.blog.CategoryBlogs,
                Comments = I.blog.Comments,
                Description = I.blog.Description,
                Id = I.blog.Id,
                ImagePath = I.blog.ImagePath,
                PostedTime = I.blog.PostedTime,
                ShortDescription = I.blog.ShortDescription,
                Title = I.blog.Title
            }).ToListAsync();
        }

     

        public async Task<List<Category>> GetCategoriesAsync(int blogId)
        {

            return await _context.Categories.Join(_context.CategoryBlogs, c => c.Id, cb => cb.CategoryId, (category, categoryBlog) => new
            {
                category,
                categoryBlog
            }).Where(I => I.categoryBlog.BlogId == blogId).Select(I => new Category
            {
                Id = I.category.Id,
                Name = I.category.Name
            }).ToListAsync();
        }

        public async Task<List<Entities.Concrete.Blog>> GetLastFiveAsync()
        {

            return await _context.Blogs.OrderByDescending(I => I.PostedTime).Take(5).ToListAsync();
        }
    }
}
