using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore{
    public static class SeedData{
        public static void TestVerileriniDoldur(IApplicationBuilder app){
            var context=app.ApplicationServices.CreateScope().ServiceProvider.GetService<BlogContext>();
            if(context!=null){
                if(context.Database.GetPendingMigrations().Any()){
                    context.Database.Migrate();
                }
                if(!context.Tags.Any()){
                    context.Tags.AddRange(
                        new Entity.Tag{Text="web programlama"},
                        new Entity.Tag{Text="backend"},
                        new Entity.Tag{Text="frontedn"},
                        new Entity.Tag{Text="php"}
                    );
                    context.SaveChanges();
                }
                if(!context.Users.Any()){
                    context.Users.AddRange(
                        new Entity.User{UserName="ahmetcakar"},
                        new Entity.User{UserName="mehmetyilmaz"}
                    );
                    context.SaveChanges();
                }
                if(!context.Posts.Any()){
                    context.Posts.AddRange(
                        new Entity.Post{
                            Title="asp.net core",
                            Content="Asp.net core dersleri",
                            IsActive=true,
                            PublishedOn=DateTime.Now.AddDays(-10),
                            Tags=context.Tags.Take(3).ToList(),
                            Image="1.jpg",
                            UserId=1,
                            
                        },
                        new Entity.Post{
                            Title="Php",
                            Content="Php dersleri",
                            IsActive=true,
                            PublishedOn=DateTime.Now.AddDays(-20),
                            Tags=context.Tags.Take(2).ToList(),
                            Image="2.jpg",
                            UserId=1,
                            
                        },
                        new Entity.Post{
                            Title="Django",
                            Content="Django dersleri",
                            IsActive=true,
                            PublishedOn=DateTime.Now.AddDays(-5),
                            Tags=context.Tags.Take(4).ToList(),
                            Image="3.jpg",
                            UserId=1,
                            
                        }
                    );
                    context.SaveChanges();

                }
            }
        }
    }
}