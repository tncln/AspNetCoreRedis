﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisInMemory.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            //Deger varsa oku maya çalışır yoksa değeri cache e set eder. 
            if (!_memoryCache.TryGetValue("newsDate",out string newscache))
            {
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
                //AbsoluteExpiration = verilen süre sonunda cache ölür
                options.AbsoluteExpiration = DateTime.Now.AddSeconds(30);

                //SlidingExpiration = Verilen süre sonunda eğer ki yenilenmez ise ölür. Kullanılırsa süre her seferinde uzamaya devam eder
                options.SlidingExpiration = TimeSpan.FromSeconds(10);
                _memoryCache.Set<string>("newsDate", DateTime.Now.ToString(),options);
            }
            //Değer varsa view e ekler
            ViewBag.newsDate = newscache;

            return View();
        }
        public IActionResult Show()
        {
            _memoryCache.Remove("newsDate"); //Siler


            //Veriyi almaya çalışır alamaz ise veriyi oluşturup datayı geri döner..
            _memoryCache.GetOrCreate<string>("newsDate", x => {
                return DateTime.Now.ToString();
            });

            ViewBag.news= _memoryCache.Get<string>("newsDate");
            return View();
        }
    }
}
