# Question1. Loyal Customer Detection

依據傳入的 .txt 檔，來分析 log 紀錄的用戶是否符合忠誠用戶標準。

## 商業邏輯
用戶必須同時滿足兩點代表忠誠客戶：
1.  Visited the website on **3 consecutive days**：用戶必須**連續 3 天**造訪系統。
2.  Accessed **more than 4 distinct pages** during that 3-day period.：在該連續 3 天的區間內，必須造訪的「不重複頁面」必須 **大於 4 個**。

## 技術想法概要 & 時間空間複查度

### 1.採用分層式架構

* **Presentation** 服務啟動的基本配置和註冊(DI)

* **Application** 主控流程

* **DomainService** 處理商業邏輯，複雜的邏輯判斷都在這邊處理

* **DomainModel** 定義物件模組

* **Infrastructure** 資料倉儲，只專注在讀取原始資料就好(Repository)

分層好處
* **SOLID 原則**：
* 1. SRP : 不同層職責區分 
* 2. OCP : 使用方法都抽介面，隨時都可以直接替換，不改動使用端 
* 3. ISP : 介面只有符合檢查商業邏輯的方法，確保使用端不會依賴到其不需要的功能
* 4. DIP : 注入 DI 進行解耦。

### 2. 記憶體資源管理

原先最直覺的做法是可以用 .NET 框架提供的庫方法 File.ReadAllLines，它可以做到把 .txt 檔整包匯進來，回傳一個字串陣列出去，但它是不考慮原始資料大小的作法。但萬一整包檔案可能是高達數 GB 的日誌檔案，是有可能造成記憶不足爆掉的問題，所以我在 `LogRepository` 採用另一個庫方法 StreamReader，它可以做到「逐行讀取」，再用 C# 迭代器方法**`yield return`**，實作延遲執行，所以記憶體中永遠只存在當前處理的一行資料，不會記憶體溢位：

優點
* **記憶體效率**：空間複雜度 $O(1)$
* **資源釋放**：利用 C# 的 **`using var`** 宣告，可以讓檔案控制權在生命週期結束後能自動釋放記憶體


### 3. 商業邏輯演算法優化：從 $O(N^2)$ 到 $O(N \log N)$
在開發 `LoyalCheckDomainService` 時，：

* **初始做法 (我覺得寫起來比較直覺，可以參考 Commit 657b007)**：
  
  先定義第一天的元素後，我再用 LINQ 語法 `FirstOrDefault` 搜尋「隔天」與「後天」的紀錄，這導致了 $O(N^2)$ 的時間複雜度，如果處理長期歷史數據會很吃效能。
  
  因為我在 for 迴圈內使用了 `FirstOrDefault` 來尋找連續日期，代表我先遍歷當下用戶的每一個日期 $(N)$，再對每一個日期的序列遍歷從頭到位去找我要的日期 $(N)$，因此為 $O(N^2)$。整體效能問題在於兩個迴圈產生的 $O(N^2)$，使`OrderBy`排序的 $O(N \log N)$ 成為次要負擔。 


* **優化作法 ( Refactor Commit 30f8a40)**：
  
  利用`OrderBy` 排序後的序列，直接透過 `sort[i+1]` 與 `sort[i+2]` 存取相鄰資料只會產生 $O(1)$，透過移動窗格概念，只需一次遍歷該用戶的所有日期 $O(N)$ 即可完成連續性與頁面數的判定。但此時最大的時間複雜度就會是 `OrderBy`的 $O(N \log N)$。


## 複雜度分析對照表

| 維度 | 優化前 (Naive Approach) | 優化後 (Final Implementation) | 技術 |
| :--- | :--- | :--- | :--- |
| **時間複雜度 (Time)** | $O(N^2)$ | **$O(N \log N)$** | 預排序 + 滑動視窗索引存取 |
| **空間複雜度 (Space)** | $O(M)$ (隨檔案大小線性增長) | **$O(1)$** (維持常數級別) | `yield return` 串流讀取 |

## 執行與測試指令

### 建置.NET 專案
```bash
dotnet build
```

### 執行專案測試
```bash
dotnet test
```