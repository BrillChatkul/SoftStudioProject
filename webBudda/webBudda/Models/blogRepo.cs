namespace webBudda.Models
{
    public class blogRepo
    {
        private List<blog> _blogList;
        public blogRepo()
        {
            _blogList = new List<blog>();
            _blogList.Add(new blog { Id = 1, authen = "Unknown", title = "การนอนเป็นเรื่องเสียเวลา", typep = "โลกธรรมะ", content = "อะไรก็ไม่รู้เหมือนกัน แค่อยากเขียน", Created = DateTime.Now.ToString() });
            _blogList.Add(new blog { Id = 2, authen = "Minori", title = "การนอนเป็นเรื่องปกติของชีวิต", typep = "โลกธรรมะ", content = "อะไรก็ไม่รู้เหมือนกัน แต่ต้องนอน", Created = DateTime.Now.ToString() });
            _blogList.Add(new blog { Id = 3, authen = "Tomie", title = "ตื่นมาพักผ่อนบ้าง", typep = "ท่องเที่ยวธรรมะ", content = "อะไรก็ไม่รู้เหมือนกัน แต่ตื่นมาบ้างก็ดี", Created = DateTime.Now.ToString() });
            _blogList.Add(new blog
            {
                Id = 4,
                authen = "Doctor Daeng",
                title = "ธรรมะแท้ไม่มีคำปลอบใจ",
                typep = "โลกธรรมะ",
                content = "เพราะฉะนั้นกูไม่มีคำอวยพรวันปีใหม่ให้พวกมึงไอ้พวกชาตินรก มึงอยากรวยมึงก็ต้องทำหาแดกวางแผนการเงิน มึงอยากมีชีวิตที่ดีมึงก็ต้องเป็นคนที่ดีคบมิตรที่ดีวางแผนชีวิต ไม่มีใครอวยพรดลบันดาลให้มึงดีหรือชั่วได้ถ้ามึงไม่ลงมือทำ ชีวิตมึงจะเป็นยังไงก็อยู่ที่การกระทำของมึงเอง",
                Created = DateTime.Now.ToString(),
                CommentList = new List<Comment>{ new Comment {Name = "A", Content="Bra bra bra", Created=DateTime.Now.ToString(), },
                                                 new Comment {Name = "B", Content="Ara Ara Ara", Created=DateTime.Now.ToString() }
                }
            });

        }
        public List<blog> GetBlogList() { return _blogList.ToList(); }
        public void setBlog(blog blog)
        {
            _blogList.Add(blog);
        }
    }
}
