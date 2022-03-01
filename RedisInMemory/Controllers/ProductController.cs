using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RedisInMemory.Models;
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
                //Data Önceliği
                options.Priority = CacheItemPriority.High;

                //cache silindiğinde çalışır datanın memory den neden silindiğinin bilinmesi için kullanılır. 
                options.RegisterPostEvictionCallback((key,value,reason,state)=>{
                    _memoryCache.Set("callback", $"{key}->{value}-> sebep:{reason}");
                });
                _memoryCache.Set<string>("newsDate", DateTime.Now.ToString(),options);
            }
            //Değer varsa view e ekler
            ViewBag.newsDate = newscache;

            Product product = new Product { Id = 1, Name = "macbook", Price = 1200 };
            _memoryCache.Set<Product>("product:1", product);

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
