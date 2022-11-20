// See https://aka.ms/new-console-template for more information
using TinyUrlService.BusinessLogic;
using TinyUrlService.Persistence;

var adder = new UrlAdder();
var deleter = new UrlDeleter();
var resolver = new UrlResolver();
var run = true;

string PostNewUrl(string longUrl)
{
    return adder.CreateRecord(longUrl);
}

void DeleteUrl(string shortUrl, string userId)
{
    resolver.RemoveFromCache(shortUrl);
    deleter.DeleteRecord(shortUrl);
}

string GetUrl(string shortUrl)
{
    return resolver.Resolve(shortUrl);
}

ulong GetTimesClicked(string shortUrl)
{
    return resolver.GetClicks(shortUrl);
}