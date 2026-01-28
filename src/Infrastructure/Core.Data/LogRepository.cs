using DomainModel.Interface;
using DomainModel.Model;

namespace Infrastructure.Core.Data
{
    public class LogRepository : ILogRepository
    {
        // 用 IEnumerable 原因 (搭配 yield return): 
        // 1. 假設 .txt 檔案非常大包，用 List 整包灌進記憶體可能會爆掉，那我逐筆讀取比較安全，記憶體當下只處裡一行就好
        // 2. 預期不用對資料修改刪除，所以唯讀就好
        public IEnumerable<LogEntry> ReadLogs(string sourcePath)
        {
            if (!File.Exists(sourcePath))
                throw new FileNotFoundException("檔案不存在");

            // 通常在使用記憶體相關操作，我會考慮加 using ，功能是會在結束生命周期後自動釋放記憶體
            using var reader = new StreamReader(sourcePath);
            var line = "";

            // 逐行遍歷 txt 檔
            while ((line = reader.ReadLine()) != null)
            {
                if(string.IsNullOrWhiteSpace(line))
                    continue;

                // 根據題目格式：[timestamp] [page] [username] ，例如：2025-05-18T00:05:01.096061+00:00 products/storage alice@yy.net
                var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                if(parts.Length == 3 && DateTimeOffset.TryParse(parts[0], out var timestamp))
                {
                    yield return new LogEntry
                    {
                        TimeStamp = timestamp,
                        Page = parts[1],
                        UserName = parts[2]
                    };
                }
            }
        }
    }
}