using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProductShop.ModelsDto.Output
{
    [XmlType("User")]
    public class UserOutputModel
    {
        public UserOutputModel()
        {
            this.SoldProducts = new List<SoldProductsOutputModel>();
        }

        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlElement("age")]
        public int? Age { get; set; }

        [XmlArray("SoldProducts")]
        public List<SoldProductsOutputModel> SoldProducts { get; set; }
    }
        
    [XmlType("SoldProducts")]
    public class SoldProductsOutputModel
    {
        public SoldProductsOutputModel()
        {
            this.Products = new List<ProductOutputModel>();
        }

        [XmlElement("count")]
        public int CountProducts { get; set; }

        [XmlArray("products")]
        public List<ProductOutputModel> Products { get; set; }
    }

    [XmlType("Product")]
    public class ProductOutputModel
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }
    }
}
