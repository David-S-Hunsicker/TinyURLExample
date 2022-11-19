using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyUrlService.Data
{

    /// <summary>
    /// This is going to be our 'in memory DB'
    /// </summary>
    internal sealed class RecordKeeper
    {
        ulong urlId = 0;
        readonly Queue<ulong> reusableIds = new Queue<ulong>();
        private static RecordKeeper instance = null;
        Dictionary<string, UrlEntity> shortRecords;
        private RecordKeeper()
        {
            shortRecords = new Dictionary<string, UrlEntity>();
        }
        /// <summary>
        /// Assumption is that this is single thread
        public static RecordKeeper Instance
        {
            get
            {
                instance ??= new RecordKeeper();
                return instance;
            }
        }

        public string Add(UrlEntity entry)
        {

            if (!shortRecords.ContainsKey(entry.shortUrl))
            {
                ulong id = 0;
                if (reusableIds.Count > 0)
                {
                    id = reusableIds.Dequeue();
                }
                else
                {
                    id = urlId;
                    urlId++;
                }
                entry.id = id;
                entry.shortUrl = "TinyLittleUrl.com/" + id;

                shortRecords.Add(entry.shortUrl, entry);
                return entry.shortUrl;
            }

            return "";
        }

        public bool Delete(string shortUrl)
        {
            if (shortRecords.ContainsKey(shortUrl))
            {
                var e = shortRecords[shortUrl];
                reusableIds.Enqueue(e.id);
                shortRecords.Remove(e.shortUrl);
            }
            else
            {
                return false;
            }
            return true;
        }

        public string Read(string shortUrl)
        {
            if (shortRecords.TryGetValue(shortUrl, out UrlEntity record))
            {
                record.clicks++;
                return record.longUrl;

            }
            return "";
        }

        public ulong GetClicks(string shortUrl)
        {
            if(shortRecords.ContainsKey(shortUrl))
            {
                return shortRecords[shortUrl].clicks;
            }
            return 0;
        }

        internal void Bump(string shortUrl)
        {
            shortRecords[shortUrl].clicks++;
        }
    }
}
