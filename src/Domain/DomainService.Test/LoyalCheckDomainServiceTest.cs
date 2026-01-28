using DomainModel.Model;
using DomainService.Interface;
using Xunit;

namespace DomainService.Test
{
    public class LoyalCheckDomainServiceTest
    {
        private readonly ILoyalCheckDomainService _loyalCheckDomainService;

        public LoyalCheckDomainServiceTest()
        {
            _loyalCheckDomainService = new LoyalCheckDomainService();
        }

        [Fact]
        // 連續 3 天，且不重複頁面總數 5 個
        public void IsLoyal_ShouldReturnTrue_WhenContinuousThreeDaysAreConsecutiveAndPagesGreaterThanFour()
        {
            // Arrange
            var userActivity = new List<UserDailyActivity>
            {
                new UserDailyActivity
                {
                    Date = new DateTime(2026, 1, 28),
                    VisitedPages = new HashSet<string>{"A", "B"}
                },
                new UserDailyActivity
                {
                    Date = new DateTime(2026, 1, 29),
                    VisitedPages = new HashSet<string>{"C"}
                },
                new UserDailyActivity
                {
                    Date = new DateTime(2026, 1, 30),
                    VisitedPages = new HashSet<string>{"D", "E"}
                },
            };

            // Act
            var result = _loyalCheckDomainService.IsLoyal(userActivity);

            // Assert
            Assert.True(result);
        }

        // 雖然有 3 天且造訪超過 4 個不同頁面 ，但日期是 1/28, 1/29, 1/31。
        [Fact]
        public void IsLoyal_ShouldReturnFalse_WhenDatesAreNotContinuous()
        {
            // Arrange
            var userActivity = new List<UserDailyActivity>
            {
                new UserDailyActivity
                {
                    Date = new DateTime(2026, 1, 28),
                    VisitedPages = new HashSet<string>{"A", "B"}
                },
                new UserDailyActivity
                {
                    Date = new DateTime(2026, 1, 29),
                    VisitedPages = new HashSet<string>{"C", "D"}
                },
                new UserDailyActivity
                {
                    Date = new DateTime(2026, 1, 31),
                    VisitedPages = new HashSet<string>{"E", "F"}
                },
            };

            // Act
            var result = _loyalCheckDomainService.IsLoyal(userActivity);

            // Assert
            Assert.False(result);
        }

        // 雖然連續 3 天，但每天都只看 2 個頁面而已
        [Fact]
        public void IsLoyal_ShouldReturnFalse_WhenPagesCountIsFourOrLess()
        {
            // Arrange
            var userActivity = new List<UserDailyActivity>
            {
                new UserDailyActivity
                {
                    Date = new DateTime(2026, 1, 28),
                    VisitedPages = new HashSet<string>{"A", "B"}
                },
                new UserDailyActivity
                {
                    Date = new DateTime(2026, 1, 29),
                    VisitedPages = new HashSet<string>{"A", "B"}
                },
                new UserDailyActivity
                {
                    Date = new DateTime(2026, 1, 30),
                    VisitedPages = new HashSet<string>{"A", "B"}
                },
            };

            // Act
            var result = _loyalCheckDomainService.IsLoyal(userActivity);

            // Assert
            Assert.False(result);
        }
    }
}