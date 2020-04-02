using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace SelfHost
{
    public class SomeDto
    {
        public string Message;        
    }


    [RoutePrefix("/api/values")]
    public class ValuesController : ApiController
    {
        [HttpGet]
        // , Route("getnums")
        public IEnumerable<string> Get()
        {
            return new string[] { "88.88", "99.99" };
        }

        [HttpGet, Route("{id}")]
        public string GetItem(string id)
        {
            Console.WriteLine($"id = {id}");            
            return "Http get [" + id + "], return this message.";
        }

        //[HttpGet, Route("read")]
        //public string Read()
        //{
        //    return "Here is the readout.";
        //}

        [HttpPost, Route("{id}")]
        public IHttpActionResult Post([FromBody] SomeDto instr, string id)
        {
            Console.WriteLine("Http Post: " + instr.Message);
            return StatusCode(HttpStatusCode.OK);
        }

    }
}
    