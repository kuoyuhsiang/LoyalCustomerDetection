using DomainModel.Model;
using DomainService.Interface;

namespace DomainService
{
    public class LoyalCheckDomainService : ILoyalCheckDomainService
    {
        public bool IsLoyal(List<UserDailyActivity> dailyActivity)
        {
            // 用戶造訪日期不足 3 天就不符合標準
            if (dailyActivity.Count() < 3)
                return false;

            var sort = dailyActivity.OrderBy(x => x.Date).ToList();

            // 往後找是否有連續的第二天與第三天，迴圈停止條件是序列總數減2，因為最後兩個元素沒辦法湊齊往後的三天
            for (int i = 0; i < sort.Count - 2; i++)
            {
                var day1 = sort[i];
                var day2 = sort.FirstOrDefault(x => x.Date == day1.Date.AddDays(1));
                var day3 = sort.FirstOrDefault(x => x.Date == day1.Date.AddDays(2));

                // 用戶連續使用 3 天，並且造訪不同頁面超過 4 個，就符合標準
                if (day2 != null && day3 != null)
                {
                    var totalPages = new HashSet<string>(day1.VisitedPages);
                    totalPages.UnionWith(day2.VisitedPages);
                    totalPages.UnionWith(day3.VisitedPages);

                    if (totalPages.Count > 4)
                        return true;
                }
            }

            return false;
        }
    }
}