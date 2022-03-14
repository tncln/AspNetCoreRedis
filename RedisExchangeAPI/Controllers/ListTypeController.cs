using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(1);
        }
        public IActionResult Index()
        {
            List<string> namesList = new List<string>();
            if (db.KeyExists("list"))
            {
                db.ListRange("List").ToList().ForEach(x=> {
                    namesList.Add(x.ToString());
                });
            }
            return View();
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
            db.ListRightPush("list", name);
            return View();
        }
    }
}
