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
            var logs = _logRepository.ReadLogs(sourcePath);

            // 將每一個用戶獨立分組
            var usersGroup = logs.GroupBy(x => x.UserName);

            var loyalCustomer = new List<string>();

            // 目的 : 我想知道「每一個用戶，每一天都去過哪些頁面」
            // step1: 遍歷每個用戶 
            // step2: 將用戶使用過的日期再分組 
            // step3: 將每組的日期跟用戶造訪過哪些網站存在新的物件
            // step4: 儲存符合忠誠條件的用戶
            foreach (var user in usersGroup)
            {
                var dailyActivity = user.GroupBy(log => log.TimeStamp.Date)
                                        .Select(dateGroup => new UserDailyActivity
                                        {
                                            Date = dateGroup.Key,
                                            VisitedPages = dateGroup.Select(x => x.Page).ToHashSet()
                                        }).ToList();


                if (_loyalCheckDomainService.IsLoyal(dailyActivity))
                    loyalCustomer.Add(user.Key);
            }

            return loyalCustomer;
        }
    }
}