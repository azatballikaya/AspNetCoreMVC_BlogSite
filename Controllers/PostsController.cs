using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers{
    public class PostsController:Controller{
        
        private IPostRepository _postRepository;
        private ITagRepository _tagRepository;
        private ICommentRepository _commentRepository;
        public PostsController(IPostRepository postRepository,ITagRepository tagRepository,ICommentRepository commentRepository)
        { 
            _postRepository=postRepository;
            _tagRepository=tagRepository;
            _commentRepository=commentRepository;
            
        }
        public async Task<IActionResult> Index(string url){
            var claims=User.Claims;
            var posts=_postRepository.Posts;
          if(!string.IsNullOrEmpty(url)){
            posts=posts.Where(x=>x.Tags.Any(t=>t.Url==url));
          }
            return View(new PostViewModel{
                Posts=await posts.ToListAsync(),
                Tags=_tagRepository.Tags.ToList()
            });
        }
        public async Task<IActionResult> Details(string? url){
           return View(await _postRepository.Posts.Include(x=>x.Tags).Include(y=>y.Comments).ThenInclude(z=>z.User).FirstOrDefaultAsync(x=>x.Url==url) );
        }
        [HttpPost]
        public JsonResult AddComment(int PostId,string UserName,string Text,string Url){
            var entity=new Comment{
                Text=Text,
                PublishedOn=DateTime.Now,
                PostId=PostId,
                User=new User{UserName=UserName,Image="avatar.jpg"}
            };
            _commentRepository.CreateComment(entity);
            return Json(new {
                UserName,
                Text,
                entity.PublishedOn,
                entity.User.Image
            });
        }
    }
}