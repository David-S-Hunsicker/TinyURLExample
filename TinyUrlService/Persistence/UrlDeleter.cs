using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyUrlService.Data;

namespace TinyUrlService.Persistence
{
    public class UrlDeleter
    {
        readonly RecordKeeper db;
        public UrlDeleter()
        {
            db = RecordKeeper.Instance;
        }

        /// <summary>
        /// Attempts to remove a record from the 'db'
        /// </summary>
        /// <param name="shortUrl">The short url associated with the entry.</param>
        /// <returns>True if the operation was successful, else False.</returns>
        public bool DeleteRecord(string shortUrl)
        {
            // ideally would check the request to make sure
            // the requestor is the creater
            return db.Delete(shortUrl);
        }
    }
}
