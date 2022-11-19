using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyUrlService.Data;

namespace TinyUrlService.BusinessLogic
{
    /// <summary>
    /// This 'service' will read from the 'db' and return long url
    /// Service will also resolve requests for click-through metrics.
    /// </summary>
    public class UrlResolver
    {
        Dictionary<string, string> cache = new Dictionary<string, string>();
        RecordKeeper db;

        public UrlResolver()
        {
            db = RecordKeeper.Instance;
        }

        /// <summary>
        /// Taps the database to resolve the short URL unless it sits in the cache
        /// </summary>
        /// <param name="shortUrl">The short URL</param>
        /// <returns>The long version of the URL, else returns empty string</returns>
        public string Resolve(string shortUrl)
        {
            string longUrl;
            if (cache.TryGetValue(shortUrl, out longUrl))
            {
                db.Bump(shortUrl);
                return longUrl;
            }
            else
            {
                var r = db.Read(shortUrl);
                cache.Add(shortUrl, r);
                return r;
            }
        }

        /// <summary>
        /// Caches may hold deleted records so there has to be a way to remove entries
        /// There isn't currently a message broker from the UrlDeleter to UrlResolver but
        /// this would be a more ideal setup.
        /// </summary>
        public void RemoveFromCache(string shortUrl)
        {
            if (cache.ContainsKey(shortUrl))
            {
                _ = cache.Remove(shortUrl);
            }
        }

        /// <summary>
        /// Checks db for resolve metrics
        /// </summary>
        /// <param name="shortUrl">The short url to be checked</param>
        /// <returns>Number of times a link has been resolved.</returns>
        public ulong GetClicks(string shortUrl)
        {
           return db.GetClicks(shortUrl);
        }
    }
}
