using IDistributedCacheRedis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDistributedCacheRedis.Controllers
{
    public class ProductsController : Controller
    {
        private IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            //_distributedCache.SetString("name", "Adem",cacheEntryOptions);

            Product product = new Product { Id=1, Name="Kalem" , Price=100 };
            string jsonProduct = JsonConvert.SerializeObject(product);

            //BinarySerialize İşlemi 
            Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);
            _distributedCache.Set("productBinary:1", byteProduct);

            //JsonConvert ile ekleme
            await _distributedCache.SetStringAsync("product:1", jsonProduct,cacheEntryOptions);

            return View();
        }
        public IActionResult Show()
        {
            //string name = _distributedCache.GetString("name");

            //Binary Deserialize
            Byte[] byteProduct = _distributedCache.Get("productBinary:1");
            string jsonproduct = Encoding.UTF8.GetString(byteProduct);

            //JsonConvert Deserialize
             jsonproduct = _distributedCache.GetString("product:1");
            Product p = JsonConvert.DeserializeObject<Product>(jsonproduct);  
            return View();
        }
        public IActionResult Remove()
        {
            _distributedCache.Remove("name");
            return View();
        }
        public IActionResult ImageCache()
        {
            //image byte dönüştürme işlemi
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/agac1.jpg");
            byte[] imageByte = System.IO.File.ReadAllBytes(path);

            _distributedCache.Set("resim", imageByte);
            return View();
        }
        public IActionResult ImageUrl()
        {
            byte[] resimByte = _distributedCache.Get("resim");

            return File(resimByte,"image/jpg");
        }
    }
}
