
namespace DomainModel.Model
{
    // 使用 Record 類別是因為我預期 txt 檔的資料我不會修改到，唯讀就好
    public record LogEntry
    {
        /// <summary>
        /// 時間戳，考慮有時區部分，所以我用 DateTimeOffset 型別
        /// ex : 2025-05-18T00:05:01.096061+00:00
        /// </summary>
        public DateTimeOffset TimeStamp {get; init;}

        /// <summary>
        /// 使用頁面
        /// </summary>
        public string Page {get; init;}

        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string UserName {get; init;}
    }
}