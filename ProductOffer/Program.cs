using ProductOffer.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ProductOffer
{
    public class Combo
    {
        public int Id { get; set; }
        public List<Product> Products { get; set; }
        public int Price { get; set; }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Init(out List<Product> productsList, out List<Combo> combosList);

            int[] maxCanBuy = CalMaxCanBuy(productsList, combosList);
            int[] currentBuy = new int[combosList.Count];
            int[] bestBuy = new int[combosList.Count];
            int bestBuyPrice = productsList.Sum(x => x.Price * x.Quantity);

            //maxCanBuy.ToList().ForEach(x => Console.Write($"{x} "));
            //Console.WriteLine();

            //currentBuy.ToList().ForEach(x => Console.Write($"{x} "));
            //Console.WriteLine();

            //bestBuy.ToList().ForEach(x => Console.Write($"{x} "));
            //Console.WriteLine();

            Console.WriteLine(bestBuyPrice);

            TryBuy(0, productsList, combosList, maxCanBuy, currentBuy, bestBuy, ref bestBuyPrice);

            PrintResult(bestBuyPrice, bestBuy, combosList, productsList);
        }

        private static void PrintResult(int bestBuyPrice, int[] bestBuy, List<Combo> combosList, List<Product> productsList)
        {
            int i = 0;
            foreach (var item in combosList)
            {
                UpdateProductsQuantity(item, bestBuy[i], productsList);
                i++;
            }
            Console.WriteLine($"Best buy price = {bestBuyPrice}");
            i = 0;
            foreach (var item in combosList)
            {
                if (bestBuy[i] > 0)
                {
                    Console.WriteLine($"{bestBuy[i]} x {item.Price} = {bestBuy[i] * item.Price} =>Combo {item.Id}");
                }
                i++;
            }
            foreach (var item in productsList)
            {
                if (item.Quantity > 0)
                {
                    Console.WriteLine($"{item.Quantity} x {item.Price} = {item.Quantity * item.Price} =>Product {item.Id}");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="productsList">change quantity realtime</param>
        /// <param name="combosList"></param>
        /// <param name="maxCanBuy"></param>
        /// <param name="currentBuy"></param>
        /// <param name="bestBuy"></param>
        /// <param name="bestBuyPrice"></param>
        private static void TryBuy(int v, List<Product> productsList, List<Combo> combosList, int[] maxCanBuy, int[] currentBuy, int[] bestBuy, ref int bestBuyPrice)
        {
            if (v >= combosList.Count)
            {
                int currentBuyPrice = CalBuyPrice(productsList, combosList, currentBuy);
                if (currentBuyPrice < bestBuyPrice)
                {
                    bestBuyPrice = currentBuyPrice;
                    for (int i = 0; i < bestBuy.Length; i++)
                        bestBuy[i] = currentBuy[i];
                }
            }
            else
            {
                for (int i = maxCanBuy[v]; i >= 0; i--)
                {
                    if (CanBuy(combosList[v], i, productsList))
                    {
                        currentBuy[v] = i;
                        UpdateProductsQuantity(combosList[v], i, productsList);
                        if (CalBuyPriceOnlyCombos(combosList, currentBuy) < bestBuyPrice)
                            TryBuy(v + 1, productsList, combosList, maxCanBuy, currentBuy, bestBuy, ref bestBuyPrice);
                        UpdateProductsQuantity(combosList[v], -i, productsList);
                    }
                }
            }            
        }

        private static void UpdateProductsQuantity(Combo combo, int i, List<Product> productsList)
        {
            foreach (var item in combo.Products)
            {
                productsList.First(x => x.Id == item.Id).Quantity -= item.Quantity * i;
            }
        }

        private static bool CanBuy(Combo combo, int i, List<Product> productsList)
        {
            foreach (var item in combo.Products)
            {
                if (item.Quantity * i > productsList.First(x => x.Id == item.Id).Quantity)
                    return false;
            }
            return true;
        }

        private static int CalBuyPriceOnlyCombos(List<Combo> combosList, int[] currentBuy)
        {
            int result = 0;
            for (int i = 0; i < combosList.Count; i++)
            {
                result += (combosList[i].Price * currentBuy[i]);
            }
            return result;
        }

        private static int CalBuyPrice(List<Product> productsList, List<Combo> combosList, int[] currentBuy)
        {
            int result = productsList.Sum(x => x.Price * x.Quantity);
            for (int i = 0; i < combosList.Count; i++)
            {
                result += (combosList[i].Price * currentBuy[i]);
            }
            return result;               
        }

        private static int[] CalMaxCanBuy(List<Product> productsList, List<Combo> combosList)
        {
            int[] result = new int[combosList.Count];

            for (int i = 0; i < combosList.Count; i++)
            {
                result[i] = combosList[i].Products.Min(x => productsList.Find(y => y.Id == x.Id).Quantity / x.Quantity);
            }

            return result;
        }

        private static void Init(out List<Product> productsList, out List<Combo> combosList)
        {
            productsList = new List<Product>
            {
                new Product { Name = "Coke", Price = 10, Id = 1, Quantity = 27 },
                new Product { Name = "Burger", Price = 15, Id = 2, Quantity = 17 },
                new Product { Name = "Chicken", Price = 12, Id = 3, Quantity = 13 },
                new Product { Name = "Beef", Price = 12, Id = 4, Quantity = 19 },
                new Product { Name = "Salad", Price = 12, Id = 5, Quantity = 9 },
                new Product { Name = "Cheef", Price = 12, Id = 6, Quantity = 11 },
                new Product { Name = "Pork", Price = 12, Id = 7, Quantity = 7 }
            };

            combosList = new List<Combo>();
            List<Product> products;

            products = new List<Product>
            {
                new Product { Id = 1, Quantity = 3 },
                new Product { Id = 2, Quantity = 4 },
                new Product { Id = 3, Quantity = 2 },
                new Product { Id = 4, Quantity = 3 },
                new Product { Id = 7, Quantity = 5 }
            };
            combosList.Add(new Combo { Id = 1, Price = 28, Products = products });

            products = new List<Product>
            {
                new Product { Id = 1, Quantity = 3 },
                new Product { Id = 3, Quantity = 6 },
                new Product { Id = 6, Quantity = 3 }
            };
            combosList.Add(new Combo { Id = 2, Price = 28, Products = products });

            products = new List<Product>
            {
                new Product { Id = 1, Quantity = 2 },
                new Product { Id = 2, Quantity = 1 },
                new Product { Id = 5, Quantity = 3 },
                new Product { Id = 7, Quantity = 3 }
            };
            combosList.Add(new Combo { Id = 3, Price = 19, Products = products });

            products = new List<Product>
            {
                new Product { Id = 2, Quantity = 1 },
                new Product { Id = 4, Quantity = 1 },
                new Product { Id = 6, Quantity = 2 },
                new Product { Id = 7, Quantity = 3 }
            };
            combosList.Add(new Combo { Id = 4, Price = 19, Products = products });

            products = new List<Product>
            {
                new Product { Id = 4, Quantity = 1 },
                new Product { Id = 5, Quantity = 1 },
                new Product { Id = 6, Quantity = 1 }
            };
            combosList.Add(new Combo { Id = 5, Price = 25, Products = products });

            products = new List<Product>
            {
                new Product { Id = 3, Quantity = 2 },
                new Product { Id = 6, Quantity = 2 }
            };
            combosList.Add(new Combo { Id = 6, Price = 25, Products = products });

            products = new List<Product>
            {
                new Product { Id = 2, Quantity = 1 },
                new Product { Id = 3, Quantity = 2 },
                new Product { Id = 4, Quantity = 3 }
            };
            combosList.Add(new Combo { Id = 7, Price = 25, Products = products });
        }
    }
}
