using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyUrlService.Data;

namespace TinyUrlService.BusinessLogic
{
    /// <summary>
    /// This 'service' will read from the db and return 
    /// </summary>
    internal class UrlResolver
    {
        Dictionary<string, string> cache = new Dictionary<string, string>();
        RecordKeeper db;
        
        public UrlResolver()
        {
            db = RecordKeeper.Instance;
        }

        /// <summary>
        /// Taps the database to resolve the short URL.
        /// </summary>
        /// <param name="shortUrl">The short URL</param>
        /// <returns>The long version of the URL, else returns empty string</returns>
        public string Resolve(string shortUrl)
        {
            string longUrl;
            if(cache.TryGetValue(shortUrl, out longUrl))
            {
                db.Bump(shortUrl);
                return longUrl;
            }

            return db.Read(shortUrl);
        }
    }
}
