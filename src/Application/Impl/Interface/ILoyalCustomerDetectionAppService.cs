namespace Application.Interface
{
    public interface ILoyalCustomerDetectionAppService
    {
        /// <summary>
        /// 取得忠誠用戶名單
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <returns></returns>
        List<string> GetLoyalCustomer(string sourcePath);
    }
}