using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace XmlAttributesDemo.XmlAttributesDemo.XmlAttributesDemo.Models
{
    [XmlType("user")]
    public class UserDto
    {
        [XmlAttribute("firstName")]
        public string FirstName { get; set; }

        [XmlAttribute("lastName")]
        public string LastName { get; set; }

        [XmlAttribute("age")]
        public string Age { get; set; }

    }
}
