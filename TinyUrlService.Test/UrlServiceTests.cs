using TinyUrlService.BusinessLogic;
using TinyUrlService.Persistence;

namespace TinyUrlService.Test
{
    [TestClass]
    public class UrlServiceTests
    {
        [TestMethod]
        public void ShouldResolveAddedEntry()
        {
            // Assign
            var longUrl = "example.com";
            var r = new UrlResolver();
            var a = new UrlAdder();
            // Act

            var shortUrl = a.CreateRecord(longUrl);
            // Assert
            Assert.AreEqual(longUrl, r.Resolve(shortUrl));
        }

        [TestMethod]
        public void ShouldResolveDuplicateEntries()
        {
            // Assign
            var longUrl = "example.com";
            var r = new UrlResolver();
            var a = new UrlAdder();
            // Act

            var shortUrl = a.CreateRecord(longUrl);
            var shortUrl2 = a.CreateRecord(longUrl);
            // Assert
            Assert.AreEqual(r.Resolve(shortUrl2), r.Resolve(shortUrl));

        }
    }
}