using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using FastFood.Data;
using FastFood.DataProcessor.Dto.Export;
using Newtonsoft.Json;
using System.Xml;

namespace FastFood.DataProcessor
{
	public class Serializer
	{
		public static string ExportOrdersByEmployee(FastFoodDbContext context, string employeeName, string orderType)
		{
            var employee = context.Employees.ToArray()
                .Where(x => x.Name == employeeName)
                .Select(c => new
                {
                    Name = c.Name,
                    Orders = c.Orders
                              .Where(x => x.Type.ToString() == orderType)
                              .Select(s => new
                              {
                                  Customer = s.Customer,
                                  Items = s.OrderItems
                                           .Select(d => new
                                           {
                                               Name = d.Item.Name,
                                               Price = d.Item.Price,
                                               Quantity = d.Quantity
                                           }).ToArray(),
                                  TotalPrice = s.TotalPrice
                              })
                              .OrderByDescending(z => z.TotalPrice)
                              .ThenByDescending(a => a.Items.Length)
                              .ToArray(),
                    TotalMade = c.Orders
                                 .Where(x => x.Type.ToString() == orderType)
                                 .Sum(x => x.TotalPrice)
                }).FirstOrDefault();

            var jsonString = JsonConvert.SerializeObject(employee, Newtonsoft.Json.Formatting.Indented);

            return jsonString;
		}

		public static string ExportCategoryStatistics(FastFoodDbContext context, string categoriesString)
		{
            var categoryArray = categoriesString.Split(",");
            StringBuilder sb = new StringBuilder();
            var categories = context.Categories
                .Where(x => categoryArray.Any(c => c == x.Name))
                .Select(s => new CategoryDto
                {
                    Name = s.Name,
                    MostPopularItem = s.Items.Select(z => new MostPopularItemDto
                                              {
                                                   Name = z.Name,
                                                   TimesSold = z.OrderItems.Sum(b => b.Quantity),
                                                   TotalMade = z.OrderItems.Sum(a => a.Item.Price * a.Quantity)
                                              })
                                              .OrderByDescending(x => x.TotalMade)
                                              .ThenByDescending(x => x.TimesSold)
                                              .FirstOrDefault()
                })
                .OrderByDescending(x => x.MostPopularItem.TotalMade)
                .ThenByDescending(x => x.MostPopularItem.TimesSold)
                .ToArray();

            var serializer = new XmlSerializer(typeof(CategoryDto[]), new XmlRootAttribute("Categories"));
            var xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize( new StringWriter(sb),  categories, xmlNamespaces);

            return sb.ToString().Trim();
		}
	}
}