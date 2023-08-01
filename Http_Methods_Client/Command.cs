using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Http_Methods_Client
{
    public class Command
    {
        public Command(HttpMethods method, Car car)
        {
            this.method = method;
            this.car = car;
        }

        public HttpMethods method { get; set; }
        public Car car { get; set; }
    }
}
