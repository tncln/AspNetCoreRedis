using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        public SetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(2);
        }
        public IActionResult Index()
        {
            HashSet<string> namesList = new HashSet<string>();
            if (db.KeyExists("list"))
            {
                db.SetMembers("list").ToList().ForEach(x => {
                    namesList.Add(x.ToString());
                });
            }
            return View();
        }
        public IActionResult Add(string name)
        {
            db.KeyExpire("list", DateTime.Now.AddMinutes(5));
            db.SetAdd("list", name);
            return View();
        }

    }
}
