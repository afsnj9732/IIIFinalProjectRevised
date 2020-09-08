using Newtonsoft.Json;
using IIIProject_travel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Caching;
using System.Security.AccessControl;
using System.Web.Script.Serialization;

namespace IIIProject_travel.Controllers
{
    public class OpenDataConnectController : Controller
    {
        public async Task<ActionResult> TravelOpenDataIndex()
        {
            var travelOpenDataSource = await this.GetHotSpotData();
            ViewData.Model = travelOpenDataSource;
            return View();
        }

        private async Task<IEnumerable<Topendata>> GetHotSpotData()
        {
            string cacheName = "Travel_OPData";

            ObjectCache cache = MemoryCache.Default;
            CacheItem cacheContents = cache.GetCacheItem(cacheName);

            if (cacheContents == null)
            {
                return await RetriveHotSpotData(cacheName);
            }
            else
            {
                return cacheContents.Value as IEnumerable<Topendata>;
            }
        }

        private async Task<IEnumerable<Topendata>> RetriveHotSpotData(string cacheName)
        {
            string targetURI = "https://gis.taiwan.net.tw/XMLReleaseALL_public/activity_C_f.json";

            HttpClient client = new HttpClient();
            client.MaxResponseContentBufferSize = Int32.MaxValue;
            var response = await client.GetStringAsync(targetURI);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Topendata_T json = serializer.Deserialize<Topendata_T>(response);
            Topendata[] collection = json.XML_Head.Infos.Info;

            //var collection = JsonConvert.DeserializeObject<IEnumerable<Topendata>>(response);
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now.AddMinutes(30);

            ObjectCache cacheItem = MemoryCache.Default;
            cacheItem.Add(cacheName, collection, policy);

            return collection;
        }
    }
}