using System;
using System.Collections.Generic;   
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace http_methods_server
{
    public class Command
    {
        public Command(Httpmethods method, Car? car)
        {
            this.method = method;
            this.car = car;
        }

        public  Httpmethods method { get; set; }
        public Car? car { get; set; }
    }
}
