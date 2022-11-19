using TinyUrlService.BusinessLogic;
using TinyUrlService.Data;

namespace TinyUrlService.Persistence
{
    public class UrlAdder
    {
        const string baseUrl = "TinyLittleUrl.com/";
        RecordKeeper db;
        public UrlAdder()
        {
            db = RecordKeeper.Instance;
        }

        /// <summary>
        /// Creates a new short url when given a long url.
        /// </summary>
        /// <param name="longUrl"></param>
        /// <returns>The generated short url</returns>
        public string CreateRecord(string longUrl)
        {
            
            if (UrlValidator.IsValid(longUrl))
            {
                var e = new UrlEntity();
                e.userID = "0";
                e.longUrl = longUrl;
                e.clicks = 0;
                return db.Add(e);
            }
            return "failed to create";
        }
    }
}