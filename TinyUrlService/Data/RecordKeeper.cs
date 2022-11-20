using System.Runtime.ConstrainedExecution;

namespace TinyUrlService.Data
{
    /*
     * array?
     * dictionary?
     * tree?
     * if it's not a real DB does it matter very much?
     * Should be relational, optimized for update
     */
    // maybe make a more elaborate hashing function that uses [0-9, a-z, A-Z]


    /// <summary>
    /// This is going to be our 'in memory DB'
    /// </summary>
    public sealed class RecordKeeper
    {
        const string baseUrl = Constants.Constants.baseUrl;
        ulong urlId = 1;
        readonly Queue<ulong> reusableIds = new Queue<ulong>();
        private static RecordKeeper? instance;

        //          Id,   Object
        private Dictionary<string, UrlEntity> records;
        private RecordKeeper()
        {
            records = new Dictionary<string, UrlEntity>();
        }

        /// <summary>
        /// Assumption is that this is single thread
        /// This has to be a singleton to try to emulate a database
        /// </summary>
        public static RecordKeeper Instance
        {
            get
            {
                instance ??= new RecordKeeper();
                return instance;
            }
        }

        /// <summary>
        /// Puts a new entry into the database
        /// </summary>
        /// <param name="entry"></param>
        /// <returns>The short version of the UrlEntry</returns>
        public string Add(UrlEntity entry)
        {
            // Custom URL
            if (entry.shortUrl != null && !records.ContainsKey(entry.shortUrl))
            {
                var stringId = entry.shortUrl;
                entry.shortUrl = baseUrl+ stringId;
                records.Add(stringId, entry);
                return entry.shortUrl;
            }

            ulong id;
            if (reusableIds.Count > 0)
            {
                id = reusableIds.Dequeue();
            }
            else
            {
                id = urlId + 1;
            }
            entry.id = id;
            entry.shortUrl = baseUrl + id;

            urlId++;
            records.Add(entry.id.ToString(), entry);
            return entry.shortUrl;
        }

        /// <summary>
        /// Removes the UrlEntry from the database
        /// </summary>
        /// <param name="shortUrl"></param>
        /// <returns>True if the operation succeeded, Else False</returns>
        public bool Delete(string shortUrl)
        {
            var shortUrlId = UrlTrim(shortUrl);
            if (records.ContainsKey(shortUrlId.ToString()))
            {
                var e = records[shortUrlId.ToString()];
                reusableIds.Enqueue(ulong.Parse(shortUrlId));
                records.Remove(shortUrlId.ToString());
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shortUrl">the short url that points to the original</param>
        /// <returns>The long Url that the short url maps to</returns>
        public string Read(string shortUrl)
        {
            var shortUrlId = UrlTrim(shortUrl);
            if (records.TryGetValue(shortUrlId.ToString(), out UrlEntity record))
            {
                record.clicks++;
                return record.longUrl;

            }
            // Custom Url

            return "";
        }

        /// <summary>
        /// Gets the total number of times a link was resolved.
        /// </summary>
        /// <param name="shortUrl"></param>
        /// <returns>The number of times a link was resolved.</returns>
        public ulong GetClicks(string shortUrl)
        {
            var shortUrlId = UrlTrim(shortUrl);
            if (records.ContainsKey(shortUrlId))
            {
                return records[shortUrlId].clicks;
            }
            return 0;
        }

        /// <summary>
        /// Update the count of times a link was resolved
        /// </summary>
        /// <param name="shortUrl"></param>
        internal void Bump(string shortUrl)
        {
            // Edge case where the cash contains the entry but the db does not.
            if (!records.ContainsKey(UrlTrim(shortUrl))) { return; }

            records[UrlTrim(shortUrl)].clicks++;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shortUrl"></param>
        /// <returns>The ID of the entry</returns>
        private string UrlTrim(string shortUrl)
        {
            // bad data
            if (shortUrl.Length < Constants.Constants.baseUrl.Length + 1)
            {
                return "0";
            }

            return shortUrl.Substring(Constants.Constants.baseUrl.Length);
        }
    }
}
