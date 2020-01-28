using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpool
{
    public enum CarTypes { Hatchback = 1, Sedan, SUV, Crossover, Convertible }

    public class Car
    {
        public string Name { get; set; }
        public int SeatCapacity { get; set; }
        public CarTypes TypeName { get; set; }
    }

}


