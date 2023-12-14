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

    }
}