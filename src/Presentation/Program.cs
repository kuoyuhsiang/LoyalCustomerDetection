using Application.Impl;
using Application.Interface;
using DomainModel.Interface;
using DomainService;
using DomainService.Interface;
using Infrastructure.Core.Data;
using Microsoft.Extensions.DependencyInjection;

//建立 DI 容器，並將使用到的介面都做註冊，方便測試以及依賴反轉
var services = new ServiceCollection();
services.AddTransient<ILogRepository, LogRepository>();
services.AddTransient<ILoyalCheckDomainService, LoyalCheckDomainService>();
services.AddTransient<ILoyalCustomerDetectionAppService, LoyalCustomerDetectionAppService>();

//建立服務，讓我們有 AppService 的路口可以進入
var serviceProvider = services.BuildServiceProvider();
var appService = serviceProvider.GetRequiredService<ILoyalCustomerDetectionAppService>();

//我假設有一份 .txt 檔案放在虛構的 AppData 資料夾
var filePath = Path.Combine(Directory.GetCurrentDirectory(), "AppData/test.txt");

try
{
    var loyalCumstomerNameList = appService.GetLoyalCustomer(filePath);

    foreach (var name in loyalCumstomerNameList)
    {
        Console.WriteLine($"Name - {name}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

