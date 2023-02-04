using System;

namespace util
{
    public class Article
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public DateTime PublishDate { get; set; }
        public Article()
        {
            Title = string.Empty;
            Description = string.Empty;
            Image = string.Empty;
            Link = string.Empty;
        }
    }
}