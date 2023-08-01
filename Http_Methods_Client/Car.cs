using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Http_Methods_Client
{
    public class Car
    {
        public Car(int id, string make, string model, ushort year, string color)
        {
            Id = id;
            Make = make;
            Model = model;
            Year = year;
            Color = color;
        }

        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public ushort Year { get; set; }
        public string Color { get; set; }


        public override string ToString()
        {
            return $"ID:{Id}\nMake:{Make}\nModel:{Model}\nYear:{Year}\nColor:{Color}\n";
        }
    }
}
