
using DomainModel.Model;

namespace DomainModel.Interface
{
    public interface ILogRepository
    {
        /// <summary>
        /// 取得 .txt 全部 log 資訊
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <returns></returns>
        IEnumerable<LogEntry> ReadLogs(string sourcePath); 
    }
}