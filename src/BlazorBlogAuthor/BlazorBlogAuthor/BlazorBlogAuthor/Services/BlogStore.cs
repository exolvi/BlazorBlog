using BlazorBlogAuthor.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
using Newtonsoft.Json;

namespace BlazorBlogAuthor.Services
{
    public class BlogStore : IBlogStore
    {
        const string DRAFTS = "drafts";
        const string PUBLISHED = "published";

        public BlogStore()
        {
            InitContainers();
        }

        private void InitContainers()
        {
            var service = new BlobServiceClient(AppConstants.AzureStorageConnectionString);
            var containers = service.GetBlobContainers();
            var draftsExists = false;
            var publishedExists = false;
            foreach (var c in containers)
            {
                if (c.Name == DRAFTS)
                {
                    draftsExists = true;
                }
                if (c.Name == PUBLISHED)
                {
                    publishedExists = true;
                }

                if (draftsExists && publishedExists)
                {
                    break;
                }
            }

            if (!draftsExists)
            {
                service.CreateBlobContainer(DRAFTS);
            }

            if (!publishedExists)
            {
                service.CreateBlobContainer(PUBLISHED);
            }
        }

        public async Task<IEnumerable<Post>> GetPostsAsync()
        {
            var service = new BlobServiceClient(AppConstants.AzureStorageConnectionString);
            var draftContainer = service.GetBlobContainerClient(DRAFTS);
            var pubContainer = service.GetBlobContainerClient(PUBLISHED);

            var posts = new List<Post>();

            var draftBlobs = draftContainer.GetBlobsAsync(prefix: "post");
            await foreach (var b in draftBlobs)
            {
                var bc = draftContainer.GetBlobClient(b.Name);
                var ms = new MemoryStream();
                _ = await bc.DownloadToAsync(ms);
                var sr = new StreamReader(ms);
                ms.Seek(0, SeekOrigin.Begin);
                var sBlob = sr.ReadToEnd();
                var post = JsonConvert.DeserializeObject<Post>(sBlob);
                posts.Add(post);
            }

            var pubBlobs = pubContainer.GetBlobsAsync(prefix: "post");
            await foreach (var b in pubBlobs)
            {
                var bc = pubContainer.GetBlobClient(b.Name);
                var ms = new MemoryStream();
                _ = await bc.DownloadToAsync(ms);
                var sr = new StreamReader(ms);
                var sBlob = sr.ReadToEnd();
                var post = JsonConvert.DeserializeObject<Post>(sBlob);
                posts.Add(post);
            }

            return posts;
        }

        public async Task WritePostAsync(Post post)
        {
            var service = new BlobServiceClient(AppConstants.AzureStorageConnectionString);
            var container = service.GetBlobContainerClient(post.IsPublished ? PUBLISHED : DRAFTS);
            var bc = container.GetBlobClient($"post-{post.Id}");
            var sBlob = JsonConvert.SerializeObject(post);
            using var ms = new MemoryStream();
            using var sw = new StreamWriter(ms);
            sw.Write(sBlob);
            sw.Flush();
            ms.Seek(0, SeekOrigin.Begin);

            await bc.UploadAsync(ms, overwrite: true);
        }

        public async Task DeletePostAsync(Post post)
        {
            var service = new BlobServiceClient(AppConstants.AzureStorageConnectionString);
            var container = service.GetBlobContainerClient(post.IsPublished ? PUBLISHED : DRAFTS);
            var bc = container.GetBlobClient($"post-{post.Id}");
            await bc.DeleteIfExistsAsync();
        }
    }
}
