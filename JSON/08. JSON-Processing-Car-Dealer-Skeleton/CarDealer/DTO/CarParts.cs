using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.DTO
{
    public class CarParts
    {
        public CarParts()
        {
            this.Parts = new List<Part>();
        }

        public Car Car { get; set; }

        public List<Part> Parts { get; set; }
    }
}
