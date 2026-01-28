using DomainModel.Model;
using DomainService.Interface;

namespace DomainService
{
    public class LoyalCheckDomainService : ILoyalCheckDomainService
    {
        public bool IsLoyal(List<UserDailyActivity> dailyActivity)
        {
            if (dailyActivity.Count() < 3)
                return false;

            var sort = dailyActivity.OrderBy(x => x.Date).ToList();

            // 往後找是否有連續的第二天與第三天，迴圈停止條件是序列總數減2，因為最後兩個元素沒辦法湊齊往後的三天
            for(int i = 0; i < sort.Count - 2; i++)
            {
                var day1 = sort[i];
                var day2 = sort.FirstOrDefault(x => x.Date == day1.Date.AddDays(1));
                var day3 = sort.FirstOrDefault(x => x.Date == day1.Date.AddDays(2));

                if(day2 == null || day3 == null)
                    continue;
                
                var totalPages = new HashSet<string>();
                foreach (var page in day1.VisitedPages)
                    totalPages.Add(page);
                foreach (var page in day2.VisitedPages)
                    totalPages.Add(page);
                foreach (var page in day3.VisitedPages)
                    totalPages.Add(page);

                if(totalPages.Count > 4)
                    return true;
            }

            return false;
        }
    }
}