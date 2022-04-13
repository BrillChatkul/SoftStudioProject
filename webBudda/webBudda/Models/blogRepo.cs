namespace webBudda.Models
{
    public class blogRepo
    {
        private List<blog> _blogList;
        public blogRepo()
        {
            _blogList = new List<blog>();
            _blogList.Add(new blog { Id = 1, authen = "Unknown", title = "การนอนเป็นเรื่องเสียเวลา", typep = "เรื่องเล่า" });
            _blogList.Add(new blog { Id = 2, authen = "Minori", title = "การนอนเป็นเรื่องปกติของชีวิต", typep = "เรื่องเล่า" });
            _blogList.Add(new blog { Id = 3, authen = "Tomie", title = "ตื่นมาพักผ่อนบ้าง", typep = "ท่องเที่ยว" });
            _blogList.Add(new blog { Id = 4, authen = "Doctor Daeng", title = "ธรรมะแท้ไม่มีคำปลอบใจ", typep = "เรื่องเล่า" });

        }
        public List<blog> GetBlogList() { return _blogList.ToList(); }
        public void setBlog(blog blog)
        {
            _blogList.Add(blog);
        }
    }
}
