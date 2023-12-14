using BlogApp.Entity;
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
                        new Entity.Tag{Text="web programlama",Url="web-programlama",Color=TagColors.warning},
                        new Entity.Tag{Text="backend",Url="backend",Color=TagColors.primary},
                        new Entity.Tag{Text="frontedn",Url="frontend",Color=TagColors.info},
                        new Entity.Tag{Text="php",Url="php",Color=TagColors.success}
                    );
                    context.SaveChanges();
                }
                if(!context.Users.Any()){
                    context.Users.AddRange(
                         new Entity.User{UserName="ahmetcakar", Name="Ahmet Çakar",Email="info@abc.com",Password="123456",Image="p1.jpg"},
                        new Entity.User{UserName="cinarturan", Name="Çınar Turan",Email="info@info.com",Password="123123",Image="p1.jpg"}
                    );
                    context.SaveChanges();
                }
                if(!context.Posts.Any()){
                    context.Posts.AddRange(
                        new Entity.Post{
                            Title="asp.net core",
                            Content="Asp.net core dersleri",
                            Url="aspnet-core",
                            IsActive=true,
                            PublishedOn=DateTime.Now.AddDays(-10),
                            Tags=context.Tags.Take(3).ToList(),
                            PostImage="1.jpg",
                            UserId=1,
                            Comments=new List<Comment>{
                                new Comment{
                                Text="iyi bir kurs",PublishedOn=DateTime.Now.AddDays(-10),UserId=1},
                                new Comment{
                                Text="çok faydalandığım bir kurs",PublishedOn=DateTime.Now.AddDays(-20),UserId=2}
                                
                                }
                            
                        },
                        new Entity.Post{
                            Title="Php",
                            Content="Php dersleri",
                             Url="php",
                            IsActive=true,
                            PublishedOn=DateTime.Now.AddDays(-20),
                            Tags=context.Tags.Take(2).ToList(),
                           PostImage="2.jpg",
                            UserId=1,
                            
                        },
                        new Entity.Post{
                            Title="Django",
                            Content="Django dersleri",
                            Url="django",
                            IsActive=true,
                            PublishedOn=DateTime.Now.AddDays(-5),
                            Tags=context.Tags.Take(4).ToList(),
                           PostImage="3.jpg",
                            UserId=1,
                            
                        },
                        new Entity.Post{
                            Title="React Dersleri",
                            Content="React dersleri",
                            Url="react-dersleri",
                            IsActive=true,
                            PublishedOn=DateTime.Now.AddDays(-10),
                            Tags=context.Tags.Take(4).ToList(),
                           PostImage="3.jpg",
                            UserId=1,
                            
                        },
                        new Entity.Post{
                            Title="Angular",
                            Content="Angular dersleri",
                            Url="angular",
                            IsActive=true,
                            PublishedOn=DateTime.Now.AddDays(-20),
                            Tags=context.Tags.Take(4).ToList(),
                           PostImage="3.jpg",
                            UserId=1,
                            
                        },
                        new Entity.Post{
                            Title="Web Tasarım",
                            Content="Web Tasarım dersleri",
                            Url="web-tasarim",
                            IsActive=true,
                            PublishedOn=DateTime.Now.AddDays(-40),
                            Tags=context.Tags.Take(4).ToList(),
                           PostImage="3.jpg",
                            UserId=1,
                            
                        }

                    );
                    context.SaveChanges();

                }
            }
        }
    }
}