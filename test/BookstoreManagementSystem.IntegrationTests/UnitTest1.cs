using sta;

namespace BookstoreManagementSystem.IntegrationTests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
      Assert.Equal(2, Domo.TestScheduler());
    }
}
