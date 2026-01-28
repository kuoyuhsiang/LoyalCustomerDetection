using Application.Interface;
using DomainModel.Interface;
using DomainModel.Model;
using DomainService.Interface;

namespace Application.Impl
{
    public class LoyalCustomerDetectionAppService : ILoyalCustomerDetectionAppService
    {
        private readonly ILogRepository _logRepository;
        private readonly ILoyalCheckDomainService _loyalCheckDomainService;

        public LoyalCustomerDetectionAppService(ILogRepository logRepository,
                                                ILoyalCheckDomainService loyalCheckDomainService)
        {
            _logRepository = logRepository;
            _loyalCheckDomainService = loyalCheckDomainService;
        }

        public List<string> GetLoyalCustomer(string sourcePath)
        {
            // step 1. 取得 log 的資料
            var logs = _logRepository.ReadLogs(sourcePath);

            // step 2. 我想知道「每一個用戶，每一天都去過哪些頁面」
            // 將 log 中每一個用戶獨立分組 -> 遍歷每個用戶 -> 將用戶使用過的日期再分組 -> 將每組的日期跟用戶造訪過哪些網站存在新的物件
            var usersGroup = logs.GroupBy(log => log.UserName);
            var loyalCustomer = new List<string>();

            foreach (var user in usersGroup)
            {
                var dailyActivity = user.GroupBy(log => log.TimeStamp.Date)
                                        .Select(dateGroup => new UserDailyActivity
                                        {
                                            Date = dateGroup.Key,
                                            VisitedPages = dateGroup.Select(x => x.Page).ToHashSet()
                                        }).ToList();

                // step3. 進一步知道用戶有符合忠誠度標準嗎?
                if (_loyalCheckDomainService.IsLoyal(dailyActivity))
                    loyalCustomer.Add(user.Key);
            }

            // step 4. 回傳忠誠名單
            return loyalCustomer;
        }
    }
}