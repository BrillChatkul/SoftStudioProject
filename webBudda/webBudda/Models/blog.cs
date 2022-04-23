namespace webBudda.Models
{
    public class blog
    {
        public string Id { get; set; }
        public string title { get; set; }
        public string typep { get; set; }
        public string authen { get; set; }
        public string Created { get; set; }
        public string content { get; set; }
        public Boolean topfeed { get; set; }

        public List<Comment> CommentList { get; set; }
        public int Like { get; set; }
        public Boolean UserLike { get; set; }
    }

    public class Likeblog
    {
        public string Id { get; set; }
        public string Idblog { get; set; }
        public string Email { get; set; }
    }
}
