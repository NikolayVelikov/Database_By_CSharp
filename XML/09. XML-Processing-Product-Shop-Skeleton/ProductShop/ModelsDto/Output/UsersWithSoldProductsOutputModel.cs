using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProductShop.ModelsDto.Output
{
    [XmlType("User")]
    public class UsersWithSoldProductsOutputModel
    {
        public UsersWithSoldProductsOutputModel()
        {
            this.Products = new List<SoldProduct>();
        }

        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlArray("soldProducts")]
        public List<SoldProduct> Products { get; set; }
    }

    [XmlType("Product")]
    public class SoldProduct
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }
    }
}
