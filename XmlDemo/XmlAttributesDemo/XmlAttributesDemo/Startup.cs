using System.Xml.Serialization;
using XmlAttributesDemo.Models;
using System.IO;
using System.Xml;
using XmlAttributesDemo.XmlAttributesDemo.XmlAttributesDemo.Models;

namespace XmlAttributesDemo
{
    public class Startup
    {
        public static void Main(string[] args)
        {
            // var libraries = GetLibraries();
            #region Deserializer
            var xmlString = File.ReadAllText(@"./../../../users.xml");

            var ser = new XmlSerializer(typeof(UserDto[]), new XmlRootAttribute("users"));

            var deserializeUsers = (UserDto[])ser.Deserialize(new StringReader(xmlString));

            foreach (var userDto in deserializeUsers) // имаме обекти които можем да импортнем в базата.
            {
                System.Console.WriteLine($"{userDto.FirstName} {userDto.LastName} - {userDto.Age}");
            }
            var namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });
            #endregion
        }

        private static LibraryDto[] GetLibraries()
        {
            LibraryDto firstLibrary = new LibraryDto
            {
                LibraryName = "Jo Bowl",
                Sections = new SectionDto()
                {
                    Name = "Horror",
                    Books = new BookDto[] 
                    {
                        new BookDto() {
                            Name = "It",
                            Author = "Stephen King", Description = "Here you can put description about the book"
                        },
                        new BookDto() {
                            Name = "Frankenstein",
                            Author = "Mary Shelley", Description = "Here you can put description about the book"
                        }
                    }
                },
                CardPrice = 20.30m
            };

            LibraryDto secondLibrary = new LibraryDto
            {
                LibraryName = "Kevin Sanchez",
                Sections = new SectionDto()
                {
                    Name = "Comedy",
                    Books = new BookDto[]
                    {
                        new BookDto()
                        {
                            Name = "The Diary of a Nobody",
                            Author = "George Grossmith and Weeden Grossmith",
                            Description = "Here you can put description about the book"
                        },
                        new BookDto()
                        {
                            Name = "Queen Lucia",
                            Author = "E F Benson",
                            Description = "Here you can put description about the book"
                        } 
                    }
                },
                CardPrice = 43.35m
            };

            return new LibraryDto[] { firstLibrary, secondLibrary };
        }
    }
}
