using DomainModel.Interface;
using DomainModel.Model;

namespace Application.Impl
{
    public class LoyalCustomerDetectionAppService
    {
        private readonly ILogRepository _logRepository;

        public LoyalCustomerDetectionAppService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public List<string> GetLoyalCustomer(string sourcePath)
        {
            var logs = _logRepository.ReadLogs(sourcePath);

            // 將每一個用戶獨立分組
            var usersGroup = logs.GroupBy(x => x.UserName);

            var loyalCustomer = new List<string>();

            // 目的 : 我想知道「每一個使用者，每一天都去過哪些頁面」
            // step1: 遍歷每個使用者 step2: 將使用者使用過的日期先分組 step3: 將每組的日期跟使用造訪過哪些網站存在新的物件
            foreach (var user in usersGroup)
            {
                var dailyActivity = user.GroupBy(log => log.TimeStamp.Date)
                                        .Select(dateGroup => new UserDailyActivity
                                        {
                                            Date = dateGroup.Key,
                                            VisitedPages = dateGroup.Select(x => x.Page).ToHashSet()
                                        }).ToList();
                
            }
        }
    }
}