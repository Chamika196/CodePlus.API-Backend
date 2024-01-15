using Azure.Core;
using CodePlus.API.Models.Domain;
using CodePlus.API.Models.DTO;
using CodePlus.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodePlus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ICategoryRepository categoryRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository, ICategoryRepository categoryRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.categoryRepository = categoryRepository;
        }

        //POST://{apiBaseurl}/api/blogposts
        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateBlogPosts([FromBody] CreateBlogPostRequestDto request) 
        {
            //convert Dto to Domain
            var blogPost = new BlogPost
            {
                Author = request.Author,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                Publishedate = request.Publishedate,
                ShortDescription = request.ShortDescription,
                Title = request.Title,
                UrlHandle = request.UrlHandle,
                Categories = new List<Category>()

            };

            foreach (var categoryGuid in request.Categories)
            {
                var existingCategory = await categoryRepository.GetById(categoryGuid);

                if(existingCategory is not null) 
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            blogPost = await blogPostRepository.CreateAsync(blogPost);

            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                IsVisible = blogPost.IsVisible,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                Publishedate = blogPost.Publishedate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Author = blogPost.Author,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                { 
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle= x.UrlHandle
                }).ToList()

            };
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetALlBlogPosts()
        {
            var blogPosts = await blogPostRepository.GetAllAsync(); 

            //convert domain model into DTO
            var response = new List<BlogPostDto>();
            foreach(var blogPost in blogPosts)
            {
                response.Add(new BlogPostDto
                {
                    Id = blogPost.Id,
                    IsVisible = blogPost.IsVisible,
                    Content = blogPost.Content,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    Publishedate = blogPost.Publishedate,
                    ShortDescription = blogPost.ShortDescription,
                    Title = blogPost.Title,
                    UrlHandle = blogPost.UrlHandle,
                    Author = blogPost.Author,
                    Categories = blogPost.Categories.Select(x => new CategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()
                });
            }

            return Ok(response);    
        }

        //GET: {apibaseUrl}/api/blogposts/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostById(Guid id)
        {
            //Get the Blogpost from the repository

            var blogPost = await blogPostRepository.GetByIdAsync(id);

            if(blogPost is null )
            {
                return NotFound();
            }

            //Convert Domain Model to Dto
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                IsVisible = blogPost.IsVisible,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                Publishedate = blogPost.Publishedate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Author = blogPost.Author,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };
              
            return Ok(response);
        }

        //GET: {apibaseUrl}/api/blogposts/{id}
        [HttpGet]
        [Route("{urlHandle}")]
        public async Task<IActionResult> GetBlogPostByUrlHandle([FromRoute] string urlHandle)
        {
            //Get the Blogpost from the repository

            var blogPost = await blogPostRepository.GetByUrlHandleAsync(urlHandle);

            if (blogPost is null)
            {
                return NotFound();
            }

            //Convert Domain Model to Dto
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                IsVisible = blogPost.IsVisible,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                Publishedate = blogPost.Publishedate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Author = blogPost.Author,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };

            return Ok(response);
        }

        //PUT:{apibaseUrl}/api/blogposts/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateBlogPostById([FromRoute] Guid id,UpdateBlogPostRequestDto request )
        {
            //Convert DTO to Domain Model
            var blogPost = new BlogPost
            {
                Id= id,
                Author = request.Author,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                Publishedate = request.Publishedate,
                ShortDescription = request.ShortDescription,
                Title = request.Title,
                UrlHandle = request.UrlHandle,
                Categories = new List<Category>()

            };

            foreach(var categoryGuid in request.Categories)
            {
                var existingCategory = await categoryRepository.GetById(categoryGuid);  

                if(existingCategory!= null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            //call Repository To Update BlogPost Domain Model
            var updateBlogPost = await blogPostRepository.UpdateAsync(blogPost);

            if(updateBlogPost == null) 
            {
                return NotFound();
            }

            //convert Domain model back to Dto
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                IsVisible = blogPost.IsVisible,
                Content= blogPost.Content,
                FeaturedImageUrl= blogPost.FeaturedImageUrl,
                Publishedate = blogPost.Publishedate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Author = blogPost.Author,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task <IActionResult> DeleteBlogPost([FromRoute] Guid id)
        {
            var deleteBlogPost = await blogPostRepository.DeleteAsync(id);
            if(deleteBlogPost == null) 
            {
                return NotFound(); 
            }

            //convert Domain model to Dto
            var response = new BlogPostDto 
            {
                Id = deleteBlogPost.Id,
                IsVisible = deleteBlogPost.IsVisible,
                Content= deleteBlogPost.Content,
                FeaturedImageUrl= deleteBlogPost.FeaturedImageUrl,
                Publishedate = deleteBlogPost.Publishedate,
                ShortDescription = deleteBlogPost.ShortDescription,
                Title = deleteBlogPost.Title,
                UrlHandle = deleteBlogPost.UrlHandle,
                Author = deleteBlogPost.Author,

            };
            return Ok(response);

        }
    }
}
