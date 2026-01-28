using DomainModel.Model;

namespace DomainService.Interface
{
    public interface ILoyalCheckDomainService
    {
        /// <summary>
        /// 檢查用戶是否為符合忠誠標準
        /// </summary>
        /// <param name="dailyActivity"></param>
        /// <returns></returns>
        bool IsLoyal(List<UserDailyActivity> dailyActivity);
    }
}