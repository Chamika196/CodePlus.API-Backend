using CodePlus.API.Models.Domain;

namespace CodePlus.API.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<BlogImage> Upload(IFormFile file, BlogImage image);

        Task<IEnumerable<BlogImage>> GetAll();
    }
}
