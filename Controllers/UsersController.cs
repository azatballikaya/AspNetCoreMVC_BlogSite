using System.Security.Claims;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlogApp.Controllers{
    public class UsersController:Controller{
        
       private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository=userRepository;
        }
      public IActionResult Login(){
        if(User.Identity.IsAuthenticated)
        return RedirectToAction("Index","Posts");
        return View();
      }
      [HttpPost]
      public async Task<IActionResult> Login(LoginViewModel model){
        if(ModelState.IsValid){
        var user=   _userRepository.Users.FirstOrDefault(x=>x.Email==model.Email && x.Password==model.Password);
        if(user!=null){
          var userClaims=new List<Claim>();
          userClaims.Add(new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()));
          userClaims.Add(new Claim(ClaimTypes.Name,user.UserName.ToString() ?? ""));
          userClaims.Add(new Claim(ClaimTypes.GivenName,user.Name.ToString() ?? ""));
          userClaims.Add(new Claim(ClaimTypes.UserData,user.Image.ToString() ?? ""));
          if(user.Email=="info@abc.com"){
            userClaims.Add(new Claim(ClaimTypes.Role,"admin"));
          }
          var claimsIdentity=new ClaimsIdentity(userClaims,CookieAuthenticationDefaults.AuthenticationScheme);
          var authProperties=new AuthenticationProperties {
            IsPersistent=true
          };
          await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
          await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(claimsIdentity),authProperties);
          return RedirectToAction("Index","Posts");
        }
        else{
          ModelState.AddModelError("","Kullanıcı adı veya şifre yanlış");
        }
        }
        
        return View(model);
      }
      [HttpGet]
      public IActionResult Register(){
        return View();
      }
      [HttpPost]
      public async Task<IActionResult> Register(RegisterViewModel model){
        if(ModelState.IsValid){
          var user=_userRepository.Users.FirstOrDefaultAsync(x=>x.Email==model.Email || x.UserName==model.Email);
          if(user!=null){
            _userRepository.CreateUser(new Entity.User{
              UserName=model.UserName,
              Name=model.Name,
              Email=model.Email,
              Password=model.Password,
              Image="avatar.jpg"
            });
            return RedirectToAction("Login");
          }
          else{
            ModelState.AddModelError("","UserName veya Email kullanımda.");
            return View(model);
          }
          

        }
        else{
          return View(model);
        }
      }

      public async Task<IActionResult> Logout(){
          await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
          return RedirectToAction("Login");
      }
      public IActionResult Profile(string username){
        if(string.IsNullOrEmpty(username))
        return NotFound();
        return View(_userRepository.Users.Include(y=>y.Posts).Include(z=>z.Comments).ThenInclude(z=>z.Post).FirstOrDefault(x=>x.UserName==username));
      }
    }
}