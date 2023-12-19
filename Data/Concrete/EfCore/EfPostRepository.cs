using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore{
    public class EfPostRepository:IPostRepository{
        private BlogContext _context;
        public EfPostRepository(BlogContext context)
        {
            _context=context;
        }
        public IQueryable<Post> Posts =>_context.Posts;

        public void CreatePost(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }
        public void EditPost(Post post){
            var updated=_context.Posts.Include(x=>x.Tags).FirstOrDefault(x=>x.PostId==post.PostId);
            if(updated!=null){
                updated.Title=post.Title;
                updated.Description=post.Description;
                updated.Content=post.Content;
                updated.Url=post.Url;
                updated.IsActive=post.IsActive;
                updated.Tags=post.Tags;

            }
            _context.SaveChanges();
        }
    }
}