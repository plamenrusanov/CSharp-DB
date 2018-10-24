using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.App.Dto.Export
{
    [XmlType("sold-products")]
    public class SoldProductsDto
    {
        [XmlAttribute("count")]
        public int Count { get; set; }

        [XmlElement("product")]
        public SoldProductDto[] Product { get; set; }
    }
}
