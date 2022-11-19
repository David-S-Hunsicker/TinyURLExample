using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyUrlService.Data
{
    public class UrlEntity
    {
        public string userID; // Who created the URL and would have the right to change it.
        public ulong id; // Unique ID
        public string longUrl;// does NOT have to be unique
        public string shortUrl;// Unique
        public ulong clicks; // Metrics tracking
        public readonly DateTime created;// Maybe they should not live forever

        public UrlEntity() { }

        public UrlEntity(string userID, ulong id, string longUrl, string shortUrl, DateTime created)
        {
            this.userID = userID;
            this.longUrl = longUrl;
            this.shortUrl = shortUrl;
            this.clicks = 0;
            this.created = created;
        }
    }
}
