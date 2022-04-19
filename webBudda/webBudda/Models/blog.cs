namespace webBudda.Models
{
    public class blog
    {
        public int Id { get; set; }
        public string title { get; set; }
        public string typep { get; set; }
        public string authen { get; set; }
        public string Created { get; set; }
        public string content { get; set; }
        public Boolean topfeed { get; set; }

        public List<Comment> CommentList { get; set; }


    }
}
