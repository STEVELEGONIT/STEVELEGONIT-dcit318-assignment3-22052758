using System;
using System.Collections.Generic;
using System.IO;

namespace InventoryLoggerSystem
{
    // Marker Interface
    public interface IInventoryEntity
    {
        int Id { get; }
    }

    // Inventory Item Class
    public class InventoryItem : IInventoryEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public DateTime DateAdded { get; set; }

        public InventoryItem()
        {
        }

        public InventoryItem(
            int id,
            string name,
            int quantity,
            DateTime dateAdded)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            DateAdded = dateAdded;
        }
    }

    // Generic Inventory Logger
    public class InventoryLogger<T>
        where T : InventoryItem
    {
        private List<T> _log = new List<T>();
        private readonly string _filePath;

        public InventoryLogger(string filePath)
        {
            _filePath = filePath;
        }

        public void Add(T item)
        {
            _log.Add(item);
        }

        public List<T> GetAll()
        {
            return _log;
        }

        public void SaveToFile()
        {
            try
            {
                using (StreamWriter sw =
                    new StreamWriter(_filePath))
                {
                    foreach (T item in _log)
                    {
                        sw.WriteLine(
                            item.Id + "," +
                            item.Name + "," +
                            item.Quantity + "," +
                            item.DateAdded);
                    }
                }

                Console.WriteLine(
                    "Inventory saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "Error saving file: " +
                    ex.Message);
            }
        }

        public void LoadFromFile()
        {
            try
            {
                _log.Clear();

                if (!File.Exists(_filePath))
                {
                    Console.WriteLine(
                        "File does not exist.");
                    return;
                }

                using (StreamReader sr =
                    new StreamReader(_filePath))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] data = line.Split(',');

                        if (data.Length == 4)
                        {
                            T item = (T)Activator.CreateInstance(
                                typeof(T));

                            item.Id = int.Parse(data[0]);
                            item.Name = data[1];
                            item.Quantity = int.Parse(data[2]);
                            item.DateAdded =
                                DateTime.Parse(data[3]);

                            _log.Add(item);
                        }
                    }
                }

                Console.WriteLine(
                    "Inventory loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "Error loading file: " +
                    ex.Message);
            }
        }
    }

    // Inventory Application
    public class InventoryApp
    {
        private InventoryLogger<InventoryItem> _logger;

        public InventoryApp()
        {
            _logger =
                new InventoryLogger<InventoryItem>(
                    "inventory.txt");
        }

        public void SeedSampleData()
        {
            _logger.Add(
                new InventoryItem(
                    1,
                    "Laptop",
                    10,
                    DateTime.Now));

            _logger.Add(
                new InventoryItem(
                    2,
                    "Printer",
                    5,
                    DateTime.Now));

            _logger.Add(
                new InventoryItem(
                    3,
                    "Mouse",
                    20,
                    DateTime.Now));

            _logger.Add(
                new InventoryItem(
                    4,
                    "Keyboard",
                    15,
                    DateTime.Now));

            _logger.Add(
                new InventoryItem(
                    5,
                    "Scanner",
                    8,
                    DateTime.Now));
        }

        public void SaveData()
        {
            _logger.SaveToFile();
        }

        public void LoadData()
        {
            _logger.LoadFromFile();
        }

        public void PrintAllItems()
        {
            Console.WriteLine("\nInventory Items");

            foreach (InventoryItem item in _logger.GetAll())
            {
                Console.WriteLine(
                    "ID: " + item.Id +
                    ", Name: " + item.Name +
                    ", Quantity: " + item.Quantity +
                    ", Date Added: " +
                    item.DateAdded);
            }
        }
    }

    // Program Entry Point
    class Program
    {
        static void Main(string[] args)
        {
            InventoryApp app =
                new InventoryApp();

            // Add sample data
            app.SeedSampleData();

            // Save to file
            app.SaveData();

            Console.WriteLine(
                "\nSimulating a new session...\n");

            // Create new app instance
            app = new InventoryApp();

            // Load saved data
            app.LoadData();

            // Display loaded data
            app.PrintAllItems();

            Console.WriteLine(
                "\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}