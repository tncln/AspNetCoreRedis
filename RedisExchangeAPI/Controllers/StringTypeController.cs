using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0);
        }
        public IActionResult Index()
        {
            //String Veri Tabanına Yazma
            db.StringSet("name", "Adem Tunçalın");
            db.StringSet("count", 10);
            return View();
        }
        public IActionResult Show()
        {
            var value= db.StringGet("name");
            if (value.HasValue)
            {
                //Otomatik artırma, count değer her okunduğunda 1 artar. 
                db.StringIncrement("count", 1);
                ViewBag.value = value.ToString();
            }
            return View();
        }
    }
}
