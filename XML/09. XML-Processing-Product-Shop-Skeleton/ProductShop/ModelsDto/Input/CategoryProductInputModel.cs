using System.Xml.Serialization;

namespace ProductShop.ModelsDto.Input
{
    [XmlType("CategoryProduct")]
    public class CategoryProductInputModel
    {
        public int CategoryId { get; set; }
        public int ProductId { get; set; }
    }
}
//    < CategoryProduct >
//        < CategoryId > 4 </ CategoryId >
//        < ProductId > 1 </ ProductId >
//    </ CategoryProduct >