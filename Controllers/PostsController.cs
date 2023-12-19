using System.Security.Claims;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
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
        public JsonResult AddComment(int PostId,string Text,string Url){
            var userId=User.FindFirstValue( ClaimTypes.NameIdentifier);
            var username=User.FindFirstValue(ClaimTypes.Name);
            var avatar=User.FindFirstValue(ClaimTypes.UserData);
            var entity=new Comment{
                Text=Text,
                PublishedOn=DateTime.Now,
                PostId=PostId,
                UserId=int.Parse(userId ??"")
            };
            _commentRepository.CreateComment(entity);
            return Json(new {
                username,
                Text,
                entity.PublishedOn,
                avatar
            });
        }
        [HttpGet]
        [Authorize]
       public IActionResult Create(){
        return View();
       }
       [HttpPost]
       public async Task<IActionResult> Create(PostCreateViewModel model){
       var userId=int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if(ModelState.IsValid){
            _postRepository.CreatePost(new Post{
                Title=model.Title,
                Description=model.Description,
                Content=model.Content,
                Url=model.Url,
                UserId=userId,
                PublishedOn=DateTime.Now,
                PostImage="~/img/avatar.jpg",
                IsActive=false,
            });
            return RedirectToAction("Index");
        }   
        return View(model);
       }
       [Authorize]
       public async Task<IActionResult> List(){
        var userId=int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
       var role=User.FindFirstValue(ClaimTypes.Role);
        var postList=_postRepository.Posts;
        if(string.IsNullOrEmpty(role)){
            return View(await postList.Where(x=>x.UserId==userId).ToListAsync());
        
        }
        else{
        return View(await postList.ToListAsync());
        }
       }
       [Authorize]
       [HttpGet]
       public IActionResult Edit(int? id){
        if(id==null)
        return NotFound();
        var post=_postRepository.Posts.Include(x=>x.Tags).FirstOrDefault(x=>x.PostId==id);
        if(post==null)
        return NotFound();
        ViewBag.Tags=_tagRepository.Tags.ToList();
        return View(new PostCreateViewModel{
            PostId=post.PostId,
            Title=post.Title,
            Description=post.Description,
            Content=post.Content,
            Url=post.Url,
            IsActive=post.IsActive,
            Tags=post.Tags
        });
       }
       [Authorize]
       [HttpPost]
       public IActionResult Edit(PostCreateViewModel model,int[] tagIds){
        if(ModelState.IsValid){
            var entityToUpdate=new Post{
                PostId=model.PostId,
                Title=model.Title,
                Description=model.Description,
                Content=model.Content,
                Url=model.Url
            };
            foreach(var item in tagIds){
                var tag=_tagRepository.Tags.FirstOrDefault(x=>x.TagId==item);
                entityToUpdate.Tags.Add(tag);
            }
            if(User.FindFirstValue(ClaimTypes.Role)=="admin"){
                entityToUpdate.IsActive=model.IsActive;
            }
            _postRepository.EditPost(entityToUpdate);
            return RedirectToAction("List","Posts");
        }
        return View(model);
       }
    }
}