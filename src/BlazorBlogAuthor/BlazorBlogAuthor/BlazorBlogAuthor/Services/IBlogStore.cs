using BlazorBlogAuthor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorBlogAuthor.Services
{
    public interface IBlogStore
    {
        Task DeletePostAsync(Post post);
        Task<IEnumerable<Post>> GetPostsAsync();
        Task WritePostAsync(Post post);
    }
}