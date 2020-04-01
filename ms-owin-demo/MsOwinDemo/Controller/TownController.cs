using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;

namespace MsOwinDemo.Controller
{
    [RoutePrefix("api/town")]
    public class TownController : ApiController
    {
        IDictionary<string, int> dict = new Dictionary<string, int>();

        public TownController ()
        {
            dict.Add(new KeyValuePair<string, int>("Madrid", 3000000));
            dict.Add(new KeyValuePair<string, int>("Berlin", 200000));
            dict.Add("Amsterdam", 800000); // also possible
        }

        [HttpGet, Route("get")]
        public HttpResponseMessage GetTown()
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent("Amsterdam", Encoding.UTF8, "text/plain")
            };
        }

        [HttpGet, Route("listxml")]
        public HttpResponseMessage GetTownListXml()
        {
            List<string> towns = new List<string>(this.dict.Keys); // read it from the dictionary

            XElement townsXml = new XElement("Towns", towns.OrderBy(t => t).Select(t => new XElement("town", new XAttribute("name", t))));

            return new HttpResponseMessage()
            {
                Content = new StringContent(townsXml.ToString(), Encoding.UTF8, "application/xml")
            };
        }

        [HttpGet, Route("listjson")]
        public HttpResponseMessage GetTownListJson()
        {
            List<string> towns = new List<string>(this.dict.Keys); // read it from the dictionary

            string json = JsonConvert.SerializeObject(towns);

            return new HttpResponseMessage()
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        }

        [HttpPost, Route("find")]
        public HttpResponseMessage FindTown(HttpRequestMessage request)
        {
            // parse the incoming request
            string requestJson = request.Content.ReadAsStringAsync().Result;

            FindRequest objects = JsonConvert.DeserializeObject<FindRequest>(requestJson);

            // serach the dictionary and check whether the town with the name is found in the list
            List<string> towns = new List<string>(this.dict.Keys); // read it from the dictionary

            if (objects.town == null)
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("request was not parsed", Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            if (towns.Contains(objects.town))
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("contains", Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.NotFound
                };
            }
            else
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("not found", Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.NotFound
                };
            }
        }
    }

    internal class FindRequest
    {
        public string town;
    }
}
