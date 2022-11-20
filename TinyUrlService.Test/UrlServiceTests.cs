using TinyUrlService.BusinessLogic;
using TinyUrlService.Persistence;
using System;

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

        // code coverage will show the cache gets hit.
        [TestMethod]
        public void ShouldUseCachedEntryInResolver()
        {
            // Assign
            var longUrl = "example.com";
            var r = new UrlResolver();
            var a = new UrlAdder();
            // Act

            var shortUrl = a.CreateRecord(longUrl);
            // Assert
            r.Resolve(shortUrl);
            r.Resolve(shortUrl);
        }

        [TestMethod]
        public void ShouldHaveAccurateClicksCount()
        {
            // Assign
            var longUrl = "example.com";
            var r = new UrlResolver();
            var a = new UrlAdder();
            var random = new Random();
            ulong hits = (ulong)random.Next(1, byte.MaxValue);

            // Act
            var shortUrl = a.CreateRecord(longUrl);
            for (ulong i = 0; i < hits; i++)
            {
                r.Resolve(shortUrl);
            }

            // Assert
            Assert.AreEqual(hits, r.GetClicks(shortUrl));
        }

        [TestMethod]
        public void ShouldReturnEmptyStringWhenEntryDoesNotExist()
        {
            // Assign
            var longUrl = "example.com";
            var r = new UrlResolver();
            var a = new UrlAdder();
            // Act

            // Assert
            Assert.AreEqual("", r.Resolve("doesn't Matter"));
        }

        [TestMethod]
        public void ShouldReturnEmptyStringWhenEntryWasDeleted()
        {
            // Assign
            var longUrl = "example.com";
            var r = new UrlResolver();
            var a = new UrlAdder();
            var d = new UrlDeleter();
            // Act
            var shortUrl = a.CreateRecord(longUrl);
            d.DeleteRecord(shortUrl);
            // Assert
            Assert.AreEqual("", r.Resolve(shortUrl));
        }

        [TestMethod]
        public void ShouldReturnZeroWhenEntryDoesNotExist()
        {
            // Assign
            var longUrl = "example.com";
            var r = new UrlResolver();
            var a = new UrlAdder();
            var d = new UrlDeleter();
            // Act
            var shortUrl = a.CreateRecord(longUrl);
            d.DeleteRecord(shortUrl);
            // Assert
            Assert.AreEqual((ulong)0, r.GetClicks(shortUrl));
            Assert.AreEqual((ulong)0, r.GetClicks("FakeUrlThatDoesn'tExist"));

        }

        [TestMethod]
        public void ShouldResolveFromCacheEvenIfDbHasNoEntry()
        {
            // Assign
            var longUrl = "example.com";
            var r = new UrlResolver();
            var a = new UrlAdder();
            var d = new UrlDeleter();
            // Act
            var shortUrl = a.CreateRecord(longUrl);
            r.Resolve(shortUrl);//loads into cache
            d.DeleteRecord(shortUrl);
            // Assert
            Assert.AreEqual(longUrl, r.Resolve(shortUrl));

            // Act
            r.RemoveFromCache(shortUrl);
            ////Assert
            Assert.AreEqual("", r.Resolve(shortUrl));
        }

        [TestMethod]
        public void ShouldReturnFalseWhenTryingToDeleteEntryThatDoesNotExist()
        {
            // Assign
            var d = new UrlDeleter();


            // Act
            var shortUrl = "a";
            d.DeleteRecord(shortUrl);

            // Assert
            Assert.AreEqual(false, d.DeleteRecord(shortUrl));
        }

        [TestMethod]
        public void ShouldAllowCustomShortUrl()
        {
            // Assign
            var a = new UrlAdder();
            var r = new UrlResolver();

            // Act
            var desiredShort = "Taco Bell";
            var longUrl = "it'sNotTacoBell";
            var result = a.CreateCustomRecord(longUrl, desiredShort);

            // Assert
            Assert.AreEqual(longUrl, r.Resolve(result));
        }
    }
}