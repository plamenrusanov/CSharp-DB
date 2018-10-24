using AutoMapper;
using ProductShop.Models;
using DataAnnotation = System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using ProductShop.Data;
using System;
using System.Linq;
using System.Text;
using System.Xml;
using ProductShop.App.Dto.Export;
using Newtonsoft.Json;

namespace ProductShop.App
{
    class StartUp
    {
        static void Main(string[] args)
        {
            #region Import
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });

            var mapper = config.CreateMapper();
            var context = new ProductShopContext();

            //ImportProducts(mapper);   
            //ImportCategories(mapper);
            //ImportCategoryProducts();

            //ImportJsonUsers(context);
            //ImportJsonProducts(context);
            //ImportJsonCategories(context);
            //ImportJsonCategoryProducts(context);

            #endregion

            #region Export
            //ProductsInRange();
            //UsersSoldProducts();
            //CategoriesByProducts();
            //UsersAndProducts();

            //ExportJsonUsersAndProducts(context);
            //ExportJsonUsersSoldProducts(context);
            //ExportJsonCategoryByProducts(context);

            var usersProducts = new
            {
                usersCount = context.Users.Where(x => x.ProductsSold.Count >= 1).Count(),
                users = context.Users
                               .Where(x => x.ProductsSold.Count >= 1 && x.ProductsSold.Any(d => d.Buyer != null))
                               .OrderByDescending(c => c.ProductsSold.Count)
                               .ThenBy(v => v.LastName)
                               .Select(b => new
                               {
                                   firstName = b.FirstName,
                                   lastName = b.LastName,
                                   age = b.Age,
                                   soldProducts = new
                                   {
                                       count = b.ProductsSold.Count,
                                       products = b.ProductsSold.Select(n => new
                                       {
                                           name = n.Name,
                                           price = n.Price
                                       }).ToArray()
                                   }

                               }).ToArray()
            };

            var serializedUsers = JsonConvert.SerializeObject(usersProducts, new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });

            File.WriteAllText("./../../../Json/users-and-products.json", serializedUsers);
            #endregion

        }

        private static void ExportJsonCategoryByProducts(ProductShopContext context)
        {
            var categories = context.Categories
                .Where(c => c.CategoryProducts.Count >= 1)
                .OrderByDescending(x => x.CategoryProducts.Count)
                .Select(x => new
                {
                    category = x.Name,
                    productsCount = x.CategoryProducts.Count,
                    //Select(s => s.Product.Price).DefaultIfEmpty(0).Average()
                    averagePrice = x.CategoryProducts.Sum(v => v.Product.Price) / x.CategoryProducts.Count,
                    totalRevenue = x.CategoryProducts.Sum(s => s.Product.Price)
                })
                .ToArray();

            var serializedCategories = JsonConvert.SerializeObject(categories, new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });

            File.WriteAllText("./../../../Json/categories-by-products.json", serializedCategories);
        }

        private static void ExportJsonUsersSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(x => x.ProductsSold.Count >= 1 && x.ProductsSold.Any(d => d.Buyer != null))
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Select(s => new
                {
                    firstName = s.FirstName,
                    lastName = s.LastName,
                    soldProducts = s.ProductsSold
                                    .Where(b => b.Buyer != null)
                                    .Select(c => new
                                    {
                                        name = c.Name,
                                        price = c.Price,
                                        buyerFirstName = c.Buyer.FirstName,
                                        buyerLastName = c.Buyer.LastName
                                    }).ToArray()
                })
                .ToArray();

            var stringJson = JsonConvert.SerializeObject(users,
                                   new JsonSerializerSettings
                                   {
                                       Formatting = Newtonsoft.Json.Formatting.Indented,
                                       NullValueHandling = NullValueHandling.Ignore
                                   });

            File.WriteAllText("./../../../Json/users_sold_products.json", stringJson);
        }

        private static void ExportJsonUsersAndProducts(ProductShopContext context)
        {
            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .OrderBy(x => x.Price)
                .Select(s => new
                {

                    name = s.Name,
                    price = s.Price,
                    seller = s.Sellar.FirstName + " " + s.Sellar.LastName ?? s.Sellar.LastName

                })
                .ToArray();

            var jsonProducts = JsonConvert.SerializeObject(products, Newtonsoft.Json.Formatting.Indented);

            File.WriteAllText("./../../../Json/products_in_range.json", jsonProducts);
        }

        private static void ImportJsonCategoryProducts(ProductShopContext context)
        {
            List<CategoryProduct> categoryProducts = new List<CategoryProduct>();

            for (int i = 201; i <= 400; i++)
            {
                var categoryId = new Random().Next(1, 12);

                var categoryProduct = new CategoryProduct()
                {
                    CategoryId = categoryId,
                    ProductId = i,
                };
                categoryProducts.Add(categoryProduct);
            }

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();
        }

        private static void ImportJsonCategories(ProductShopContext context)
        {
            var jsonString = File.ReadAllText("./../../../Json/categories.json");

            var deserializeCategories = JsonConvert.DeserializeObject<Category[]>(jsonString);

            List<Category> categories = new List<Category>();

            foreach (var category in deserializeCategories)
            {
                if (IsValid(category))
                {
                    categories.Add(category);
                }
            }

            context.Categories.AddRange(categories);
            context.SaveChanges();
        }

        private static void ImportJsonProducts(ProductShopContext context)
        {
            var jsonString = File.ReadAllText("./../../../Json/products.json");

            var deserializeProducts = JsonConvert.DeserializeObject<Product[]>(jsonString);

            List<Product> products = new List<Product>();

            int countproducts = 0;
            foreach (var product in deserializeProducts)
            {
                if (IsValid(product))
                {
                    var sellarId = new Random().Next(1, 56);
                    var bayerId = new Random().Next(57, 113);

                    product.SellarId = sellarId;
                    product.BuyerId = bayerId;

                    if (countproducts++ % 5 == 0)
                    {
                        product.BuyerId = null;

                    }

                    products.Add(product);
                }

            }

            context.Products.AddRange(products);
            context.SaveChanges();
        }

        private static void ImportJsonUsers(ProductShopContext context)
        {
            var jsonString = File.ReadAllText("./../../../Json/users.json", UTF8Encoding.UTF8);

            var deserializeUsers = JsonConvert.DeserializeObject<User[]>(jsonString);

            List<User> users = new List<User>();

            foreach (var user in deserializeUsers)
            {
                if (IsValid(user))
                {
                    users.Add(user);
                }

            }

            context.Users.AddRange(users);
            context.SaveChanges();
        }

        private static void UsersAndProducts()
        {
            var context = new ProductShopContext();
            var users = new UserProductsDto
            {
                Count = context.Users.Count(s => s.ProductsSold.Count >= 1),
                Users = context.Users
                .Where(d => d.ProductsSold.Count >= 1)
                .Select(x => new UserDto
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Age = x.Age ?? 0,
                    SoldProduct = new SoldProductsDto
                    {
                        Count = x.ProductsSold.Count(r => r.Name != null),
                        Product = x.ProductsSold
                        .Where(p => p.Name != null)
                        .Select(p => new SoldProductDto
                        {
                            Name = p.Name,
                            Price = p.Price,
                        }).ToArray(),
                    },
                })
                .OrderByDescending(s => s.SoldProduct.Count)
                .ThenBy(s => s.LastName)
                .ToArray()
            };

            var sb = new StringBuilder();
            var xmlNameSpaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(typeof(Dto.Export.UserProductsDto), new XmlRootAttribute("users"));
            serializer.Serialize(new StringWriter(sb), users, xmlNameSpaces);

            File.WriteAllText("./../../../Xml/users-and-products.xml", sb.ToString());
        }

        private static void CategoriesByProducts()
        {
            var context = new ProductShopContext();
            var categories = context.Categories
               .OrderByDescending(c => c.CategoryProducts.Count)
               .Select(s => new CategoryDto
               {
                   Name = s.Name,
                   ProductsCount = s.CategoryProducts.Count,
                   AveragePrice = s.CategoryProducts
                        .Select(f => f.Product.Price)
                        .DefaultIfEmpty(0)
                        .Average(),
                   TotalRevenue = s.CategoryProducts
                        .Sum(d => d.Product.Price),
               })
               .ToArray();

            var sb = new StringBuilder();
            var xmlNameSpaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(typeof(Dto.Export.CategoryDto[]), new XmlRootAttribute("categories"));
            serializer.Serialize(new StringWriter(sb), categories, xmlNameSpaces);

            File.WriteAllText("./../../../Xml/categories-by-products.xml", sb.ToString());
        }

        //private static void UsersSoldProducts()
        //{
        //    var context = new ProductShopContext();
        //    var users = context.Users
        //        .Where(s => s.ProductsSold.Count > 0)
        //        .Select(p => new Dto.Export.UserDto
        //        {
        //            FirstName = p.FirstName,
        //            LastName = p.LastName,
        //            SoldProduct = p.ProductsSold
        //            .Where(c => c.Name != null)
        //            .Select(x => new SoldProductDto
        //            {
        //                Name = x.Name,
        //                Price = x.Price,
        //            }).ToArray()
        //        })
        //        .OrderBy(q => q.LastName)
        //        .ThenBy(w => w.FirstName)
        //        .ToArray();

        //    var sb = new StringBuilder();
        //    var xmlNameSpaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
        //    var serializer = new XmlSerializer(typeof(Dto.Export.UserDto[]), new XmlRootAttribute("users"));
        //    serializer.Serialize(new StringWriter(sb), users, xmlNameSpaces);

        //    File.WriteAllText("./../../../Xml/users-sold-products.xml", sb.ToString());
        //}

        private static void ProductsInRange()
        {
            var context = new ProductShopContext();

            var products = context.Products
                 .Where(p => p.Price >= 1000 && p.Price <= 2000 && p.Buyer != null)
                 .OrderByDescending(p => p.Price)
                 .Select(p => new Dto.Export.ProductDto
                 {
                     Name = p.Name,
                     Price = p.Price,
                     Buyer = p.Buyer.FirstName + " " + p.Buyer.LastName ?? p.Buyer.LastName
                 })
                 .ToArray();

            var sb = new StringBuilder();
            var xmlNameSpaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(typeof(Dto.Export.ProductDto[]), new XmlRootAttribute("products"));
            serializer.Serialize(new StringWriter(sb), products, xmlNameSpaces);

            File.WriteAllText("./../../../Xml/products-in-range.xml", sb.ToString());
        }

        private static void ImportCategoryProducts()
        {
            List<CategoryProduct> categoryProducts = new List<CategoryProduct>();


            for (int productId = 201; productId <= 400; productId++)
            {
                var categoryId = new Random().Next(1, 11);

                var categoryProduct = new CategoryProduct()
                {
                    ProductId = productId,
                    CategoryId = categoryId
                };
                categoryProducts.Add(categoryProduct);
            }

            var context = new ProductShopContext();
            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();
        }

        private static void ImportCategories(IMapper mapper)
        {
            var xmlString = File.ReadAllText("./../../../Xml/categories.xml");

            var serializer = new XmlSerializer(typeof(Dto.Import.CategoryDto[]), new XmlRootAttribute("categories"));

            var deserializedCategory = (Dto.Import.CategoryDto[])serializer.Deserialize(new StringReader(xmlString));

            List<Category> categories = new List<Category>();

          
            foreach (var categoryDto in deserializedCategory)
            {
                if (!IsValid(categoryDto))
                {
                    continue;
                }

                var category = mapper.Map<Category>(categoryDto);

                categories.Add(category);

            }

            var context = new ProductShopContext();
            context.Categories.AddRange(categories);
            context.SaveChanges();
        }

        private static void ImportProducts(IMapper mapper)
        {
            var xmlString = File.ReadAllText("./../../../Xml/products.xml");

            var serializer = new XmlSerializer(typeof(Dto.Import.ProductDto[]), new XmlRootAttribute("products"));

            var deserializedUsers = (Dto.Import.ProductDto[])serializer.Deserialize(new StringReader(xmlString));

            List<Product> products = new List<Product>();

            int counter = 1;
            foreach (var productDto in deserializedUsers)
            {
                if (!IsValid(productDto))
                {
                    continue;
                }

                var product = mapper.Map<Product>(productDto);
                var buyerId = new Random().Next(1, 30);
                var sellarId = new Random().Next(31, 56);

                product.BuyerId = buyerId;
                product.SellarId = sellarId;

                if (counter++ % 4 == 0)
                {
                    product.BuyerId = null;
                }

                products.Add(product);

            }

            var context = new ProductShopContext();
            context.Products.AddRange(products);
            context.SaveChanges();
        }

        public static bool IsValid(object obj)
        {
            var validationContext = new DataAnnotation.ValidationContext(obj);

            var validationResult = new List<DataAnnotation.ValidationResult>();

            return DataAnnotation.Validator.TryValidateObject(obj, validationContext, validationResult, true);
        }
    }
}
