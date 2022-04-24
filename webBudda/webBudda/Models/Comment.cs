namespace webBudda.Models
{
    public class Comment
    {
        public string Id { get; set; }
        public string blogID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
        public string Created { get; set; }
        public int Like { get; set; }
        public Boolean UserLike { get; set; }
        public Boolean onfeed { get; set; }
    }
        public class LikeComment
    {
        public string Id { get; set; }
        public string IdComment { get; set; }
        public string Email { get; set; }
    }
}
