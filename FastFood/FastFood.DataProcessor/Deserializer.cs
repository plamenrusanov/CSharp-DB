using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using FastFood.Data;
using FastFood.DataProcessor.Dto.Import;
using FastFood.Models;
using Newtonsoft.Json;
using System.IO;
using System.Globalization;
using FastFood.Models.Enums;

namespace FastFood.DataProcessor
{
    public static class Deserializer
    {
        private const string FailureMessage = "Invalid data format.";
        private const string SuccessMessage = "Record {0} successfully imported.";

        public static string ImportEmployees(FastFoodDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var deserializedEmployees = JsonConvert.DeserializeObject<EmployeeDto[]>(jsonString);
            List<Employee> employees = new List<Employee>();
            foreach (var employeeDto in deserializedEmployees)
            {
                if (!IsValid(employeeDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Position position = GetPosition(context, employeeDto.Position);

                var employee = new Employee()
                {
                    Name = employeeDto.Name,
                    Age = employeeDto.Age,
                    Position = position
                };
                employees.Add(employee);
                sb.AppendLine(string.Format(SuccessMessage, employee.Name));
            }

            context.Employees.AddRange(employees);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        private static Position GetPosition(FastFoodDbContext context, string position)
        {
            var pos = context.Positions.FirstOrDefault(x => x.Name == position);

            if (pos == null)
            {
                pos = new Position()
                {
                    Name = position
                };

                context.Positions.Add(pos);
                context.SaveChanges();
            }

            return pos;
        }

        public static string ImportItems(FastFoodDbContext context, string jsonString)
        {
            var deserealizedItems = JsonConvert.DeserializeObject<ItemDto[]>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<Item> items = new List<Item>();

            foreach (var itemDto in deserealizedItems)
            {
                if (!IsValid(itemDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }
                if (context.Items.Any(x => x.Name == itemDto.Name) || items.Any(x => x.Name == itemDto.Name))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Category category = GetCategory(context, itemDto.Category);

                Item item = new Item
                {
                    Name = itemDto.Name,
                    Price = itemDto.Price,
                    Category = category
                };

                items.Add(item);
                sb.AppendLine(String.Format(SuccessMessage, item.Name));
            }

            context.Items.AddRange(items);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        private static Category GetCategory(FastFoodDbContext context, string category)
        {
            var cat = context.Categories.FirstOrDefault(x => x.Name == category);

            if (cat == null)
            {
                cat = new Category()
                {
                    Name = category
                };

                context.Categories.Add(cat);
                context.SaveChanges();
            }

            return cat;
        }

        public static string ImportOrders(FastFoodDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            List<OrderItem> orderItems = new List<OrderItem>();

            var deserializer = new XmlSerializer(typeof(OrderDto[]), new XmlRootAttribute("Orders"));

            var deserializedOrders = (OrderDto[])deserializer.Deserialize(new StringReader(xmlString));

            foreach (var orderDto in deserializedOrders)
            {
                bool isValidItem = true;
                if (!IsValid(orderDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                foreach (var item in orderDto.OrderItemsDtos)
                {
                    if (!IsValid(item) || (!context.Items.Any(x => x.Name == item.Name)))
                    {
                        isValidItem = false;
                        break;
                    }
                }

                if (!isValidItem)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Employee employee = context.Employees.FirstOrDefault(x => x.Name == orderDto.Employee);

                if (employee == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                DateTime date = DateTime.ParseExact(orderDto.DateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                OrderType orderType = Enum.Parse<OrderType>(orderDto.Type);

                Order order = new Order()
                {
                    Customer = orderDto.Customer,
                    Employee = employee,
                    DateTime = date,
                    Type = orderType,

                };
               
                foreach (var item in orderDto.OrderItemsDtos)
                {
                    var orderItem = new OrderItem()
                    {
                        Item = context.Items.FirstOrDefault(x => x.Name == item.Name),
                        Order = order,
                        Quantity = item.Quantity
                    };
                    orderItems.Add(orderItem);
                }

                sb.AppendLine($"Order for {order.Customer} on {order.DateTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)} added");


            }

            context.OrderItems.AddRange(orderItems);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);

            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResult, true);
        }
    }
}