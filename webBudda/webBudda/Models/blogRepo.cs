namespace webBudda.Models
{
    public class blogRepo
    {
        private List<blog> _blogList;
        public blogRepo()
        {
            _blogList = new List<blog>();
            _blogList.Add(new blog { Id = 1, Authen = "Unknown", Title = "การนอนเป็นเรื่องเสียเวลา", Type = "เรื่องเล่า" });
            _blogList.Add(new blog { Id = 2, Authen = "Minori", Title = "การนอนเป็นเรื่องปกติของชีวิต", Type = "เรื่องเล่า" });
            _blogList.Add(new blog { Id = 3, Authen = "Tomie", Title = "ตื่นมาพักผ่อนบ้าง", Type = "ท่องเที่ยว" });

        }
        public List<blog> GetBlogList() { return _blogList.ToList(); }
        public void setBlog(blog blog)
        {
            _blogList.Add(blog);
        }
    }
}
