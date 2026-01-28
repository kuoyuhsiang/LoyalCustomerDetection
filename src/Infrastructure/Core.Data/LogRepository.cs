using DomainModel.Interface;
using DomainModel.Model;

namespace Infrastructure.Core.Data
{
    public class LogRepository : ILogRepository
    {
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