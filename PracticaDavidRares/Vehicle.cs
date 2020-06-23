using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaDavidRares
{
    public class Vehicle
    {
        private int id;
        private string vehicleTitle;
        private string type;

        public Vehicle()
        {
            id = 0;
            VehicleTitle = "";
            Type = "Car";
        }

        public Vehicle(int i, string vT, string t)
        {
            id = i;
            VehicleTitle = vT;
            Type = t;
        }

        public string Type { get => type; set => type = value; }
        public string VehicleTitle { get => vehicleTitle; set => vehicleTitle = value; }
        public int Id { get => id; set => id = value; }
    }
}
