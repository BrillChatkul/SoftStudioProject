namespace webBudda.Models
{
    public class EventsLI
    {
        private List<EventsModel> EV;

        public EventsLI()
        {
            EV = new List<EventsModel>() 
            {
         
                new EventsModel {
                                    EventId = 2,
                                    Subject = "วันวิสาขบูชา",
                                    Description = "Mindfire Solutions",
                                    Start = "15-May-2022",
                                    End = "NULL",
                                    ThemeColor = "blue",
                                    isFullDay = true
                },
                new EventsModel {
                                    EventId = 3,
                                    Subject = "วันมาฆบูชา",
                                    Description = "Abc Solutions",
                                    Start = "16-Feb-2022",
                                    End = "NULL",
                                    ThemeColor = "blue",
                                    isFullDay = true
                },
                new EventsModel {
                                    EventId = 4,
                                    Subject = "วันอัฏฐมีบูชา",
                                    Description = "วันถวายพระเพลิงพระพุทธเจ้า",
                                    Start = "22-May-2022",
                                    End = "NULL",
                                    ThemeColor = "blue",
                                    isFullDay = true
                },
                new EventsModel {
                                    EventId = 5,
                                    Subject = "วันอาสาฬหบูชา",
                                    Description = "...........",
                                    Start = "13-Jul-2022",
                                    End = "NULL",
                                    ThemeColor = "blue",
                                    isFullDay = true
                },
                new EventsModel {
                                    EventId = 6,
                                    Subject = "วันเข้าพรรษา",
                                    Description = "..........",
                                    Start = "14-Jul-2022",
                                    End = "NULL",
                                    ThemeColor = "blue",
                                    isFullDay = true
                },
                new EventsModel {
                                    EventId = 7,
                                    Subject = "วันออกพรรษา",
                                    Description = "..........",
                                    Start = "9-Oct-2022",
                                    End = "NULL",
                                    ThemeColor = "blue",
                                    isFullDay = true
                },
   
            };
            
        }
        public List<EventsModel> GetEvent()
    {
            return EV.ToList();
    }

    }



}
