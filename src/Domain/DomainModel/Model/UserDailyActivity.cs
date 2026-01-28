namespace DomainModel.Model
{
    public class UserDailyActivity
    {
        // 只取 TimeStamp 的 .Date 部分
        public DateTime Date { get; set; }

        // 該用戶這一天點過的「所有不重複頁面」
        public HashSet<string> VisitedPages { get; set; }

        public UserDailyActivity()
        {
        }
    }
}