using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyUrlService.BusinessLogic;
using TinyUrlService.Data;

namespace TinyUrlService.Persistence
{
    internal class UrlAdder
    {
        RecordKeeper db;
        public UrlAdder()
        {
            db = RecordKeeper.Instance;
        }

        string CreateRecord(string longUrl)
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