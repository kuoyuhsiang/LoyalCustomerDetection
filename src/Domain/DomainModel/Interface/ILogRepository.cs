
using DomainModel.Model;

namespace DomainModel.Interface
{
    public interface ILogRepository
    {
        /// <summary>
        /// 取得 .txt 全部 log 資訊
        /// 用 IEnumerable 型別原因: 
        /// 1. 假設 .txt 檔案非常大包，用 List 整包灌進記憶體可能會爆掉，那我逐筆讀取比較妥當，記憶體當下只處裡一行就好
        /// 2. 我預期不用對資料修改刪除，所以唯讀就好
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <returns></returns>
        IEnumerable<LogEntry> ReadLogs(string sourcePath); 
    }
}