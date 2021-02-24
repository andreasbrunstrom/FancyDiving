using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Webserver.Models;

/*
To keep the example simple, contestants are stored in a fixed array inside the controller class.
Of course, in a real application, you would query a database or use some other external data source.

The controller defines two methods that return products:

The GetAllProducts method returns the entire list of products as an IEnumerable<Product> type.
The  GetProduct method looks up a single product by its ID. 
*/

namespace Webserver.Controllers
{
    public class ContestantController : ApiController
    {
        contestant[] contestants = new contestant[]
        {
            new contestant {jumpCode = "104A", id= 1234, points = 7.6 },
            new contestant {jumpCode = "510A", id = 4321, points = 6.7 },
            new contestant {jumpCode = "203B", id = 4132, points = 4.2 }
        };

       public IEnumerable<contestant> GetAllContestants()
        {
            return contestants;
        }

        public IHttpActionResult GetContestant(int id)
        {
            var contestant = contestants.FirstOrDefault((p) => p.id == id);
            if(contestant == null)
            {
                return NotFound();
            }
            return Ok(contestant);
        }
    }    
}
