using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyUrlService.Data;

namespace TinyUrlService.Persistence
{
    internal class UrlDeleter
    {
        readonly RecordKeeper db;
        public UrlDeleter()
        {
            db = RecordKeeper.Instance;
        }

        public bool DeleteRecord(string shortUrl)
        {
            // ideally would check the request to make sure
            // the requestor is the creater
            return db.Delete(shortUrl);
        }
    }
}
