using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DotNetMVC_Task.Models;
using DotNetMVC_Task.Data;
using System.Text;

namespace DotNetMVC_Task.Controllers;

public class MainController : Controller
{
    private readonly ILogger<MainController> logger;
    private readonly AppDbContext dbContext;
    private static Random random = new Random();

    public MainController(AppDbContext dbContext, ILogger<MainController> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public IActionResult NewRoute(string token)
    {
        IActionResult result;

        var queryResult = dbContext.Urls.FirstOrDefault(u => u.Token == token)?.LongUrl ?? null;
        if (queryResult == null)
            result = NotFound();
        else
            result = RedirectPermanent(queryResult);

        return result;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new UrlViewModel() { url = "" });
    }

    [HttpPost]
    public IActionResult Index(string url) {
        var req = HttpContext.Request;

        logger.LogDebug($"Пришёл адрес: {url}");
        logger.LogDebug($"Путь: {req.Headers.Referer}");

        var recordInDb = dbContext.Urls.FirstOrDefault(u => u.LongUrl == url);
        string token;
        if (recordInDb == null) {
            token = RandomString(6);
            dbContext.Urls.Add(new Url() { Token = token, LongUrl = url });
            dbContext.SaveChanges();
        } else {
            token = recordInDb.Token;
        }

        string fullPath = new StringBuilder()
            .Append(req.IsHttps ? "https://" : "http://")
            .Append(req.Host.Value)
            .Append($"/{token}")
            .ToString();
        return View(new UrlViewModel() { url = fullPath });
    }


    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
