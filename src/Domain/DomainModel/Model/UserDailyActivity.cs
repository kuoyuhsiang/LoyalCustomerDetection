namespace DomainModel.Model
{
    public class UserDailyActivity
    {
        /// <summary>
        /// 只取 TimeStamp 的 .Date 部分
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 該用戶這一天點過的「所有不重複頁面」
        /// </summary>
        public HashSet<string> VisitedPages { get; set; }

        public UserDailyActivity()
        {
        }
    }
}