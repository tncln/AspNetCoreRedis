using Microsoft.AspNetCore.Mvc;
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
            //Deger varsa olmaya çalışır yoksa değeri cache e set eder. 
            if (!_memoryCache.TryGetValue("newsDate",out string newscache))
            {
                _memoryCache.Set<string>("newsDate", DateTime.Now.ToString());
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
