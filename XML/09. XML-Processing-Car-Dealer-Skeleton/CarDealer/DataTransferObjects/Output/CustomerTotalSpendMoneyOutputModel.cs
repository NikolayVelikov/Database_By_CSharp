﻿using System.Xml.Serialization;

namespace CarDealer.DataTransferObjects.Output
{
    [XmlType("customer")]
    public class CustomerTotalSpendMoneyOutputModel
    {
        [XmlAttribute("full-name")]
        public string FullName { get; set; }

        [XmlAttribute("bought-cars")]
        public int BoughtCars { get; set; }

        [XmlAttribute("spent-money")]
        public decimal SpendMoneyParts { get; set; }
    }
}