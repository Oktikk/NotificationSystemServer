namespace NotificationSystemServer.Tests
{
    using NotificationSystemServer.Workers;
    using Xunit;

    public class SimpleDummyTest
    {
        [Fact]
        public void RandomString_ReturnsStringOfGivenLength()
        {
            var length = 10;

            var result = NotificationWorker.RandomString(length);

            Assert.Equal(length, result.Length);
        }
    }
}
