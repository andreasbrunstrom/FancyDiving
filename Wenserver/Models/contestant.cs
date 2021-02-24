using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/*A model is an object that represents the data in your application.
 *  ASP.NET Web API can automatically serialize your model to JSON, XML, or some other format, 
 *  and then write the serialized data into the body of the HTTP response message. 
 *  As long as a client can read the serialization format, it can deserialize the object. 
 *  Most clients can parse either XML or JSON. Moreover, the client can indicate which format 
 *  it wants by setting the Accept header in the HTTP request message.*/

namespace Webserver.Models
{
    public class contestant
    {
        public string jumpCode { get; set; }
        public int id { get; set; }
        public double points { get; set; }
    }
}