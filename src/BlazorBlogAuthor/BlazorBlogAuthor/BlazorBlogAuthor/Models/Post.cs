using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorBlogAuthor.Models
{
    public class Post
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTimeOffset? PublishDate { get; set; }
        public bool IsPublished { get; set; } = false;
    }
}
