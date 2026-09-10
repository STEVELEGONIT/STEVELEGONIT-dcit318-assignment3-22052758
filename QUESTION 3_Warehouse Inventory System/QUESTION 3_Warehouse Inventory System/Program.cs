using System;
using System.Collections.Generic;

namespace WarehouseSystem
{
    // Marker Interface
    public interface IInventoryItem
    {
        int Id { get; }
        string Name { get; }
        int Quantity { get; set; }
    }

    // Electronic Item
    public class ElectronicItem : IInventoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Brand { get; set; }
        public int WarrantyMonths { get; set; }

        public ElectronicItem(
            int id,
            string name,
            int quantity,
            string brand,
            int warrantyMonths)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            Brand = brand;
            WarrantyMonths = warrantyMonths;
        }
    }

    // Grocery Item
    public class GroceryItem : IInventoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; set; }

        public GroceryItem(
            int id,
            string name,
            int quantity,
            DateTime expiryDate)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            ExpiryDate = expiryDate;
        }
    }

    // Custom Exceptions
    public class DuplicateItemException : Exception
    {
        public DuplicateItemException(string msg)
            : base(msg) { }
    }

    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(string msg)
            : base(msg) { }
    }

    public class InvalidQuantityException : Exception
    {
        public InvalidQuantityException(string msg)
            : base(msg) { }
    }

    // Generic Repository
    public class InventoryRepository<T>
        where T : IInventoryItem
    {
        private readonly Dictionary<int, T> _items =
            new Dictionary<int, T>();

        public void AddItem(T item)
        {
            if (_items.ContainsKey(item.Id))
                throw new DuplicateItemException("Duplicate ID");

            _items.Add(item.Id, item);
        }

        public T GetItemById(int id)
        {
            if (!_items.ContainsKey(id))
                throw new ItemNotFoundException("Item not found");

            return _items[id];
        }

        public void RemoveItem(int id)
        {
            if (!_items.Remove(id))
                throw new ItemNotFoundException("Item not found");
        }

        public List<T> GetAllItems()
        {
            return new List<T>(_items.Values);
        }

        public void UpdateQuantity(int id, int quantity)
        {
            if (quantity < 0)
                throw new InvalidQuantityException(
                    "Quantity cannot be negative");

            GetItemById(id).Quantity = quantity;
        }
    }

    // Warehouse Manager
    public class WareHouseManager
    {
        private readonly InventoryRepository<ElectronicItem>
            _electronics =
            new InventoryRepository<ElectronicItem>();

        private readonly InventoryRepository<GroceryItem>
            _groceries =
            new InventoryRepository<GroceryItem>();

        public void SeedData()
        {
            _electronics.AddItem(
                new ElectronicItem(
                    1,
                    "Laptop",
                    10,
                    "Dell",
                    24));

            _electronics.AddItem(
                new ElectronicItem(
                    2,
                    "Printer",
                    5,
                    "HP",
                    12));

            _groceries.AddItem(
                new GroceryItem(
                    101,
                    "Rice",
                    50,
                    DateTime.Now.AddMonths(12)));

            _groceries.AddItem(
                new GroceryItem(
                    102,
                    "Milk",
                    20,
                    DateTime.Now.AddMonths(2)));

            _groceries.AddItem(
                new GroceryItem(
                    103,
                    "Sugar",
                    30,
                    DateTime.Now.AddMonths(18)));
        }

        public void PrintAllItems<T>(
            InventoryRepository<T> repo)
            where T : IInventoryItem
        {
            foreach (T item in repo.GetAllItems())
            {
                Console.WriteLine(
                    $"ID: {item.Id}, Name: {item.Name}, Quantity: {item.Quantity}");
            }
        }

        public void IncreaseStock<T>(
            InventoryRepository<T> repo,
            int id,
            int quantity)
            where T : IInventoryItem
        {
            try
            {
                T item = repo.GetItemById(id);

                repo.UpdateQuantity(
                    id,
                    item.Quantity + quantity);

                Console.WriteLine(
                    $"{item.Name} stock increased.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error: {ex.Message}");
            }
        }

        public void RemoveItemById<T>(
            InventoryRepository<T> repo,
            int id)
            where T : IInventoryItem
        {
            try
            {
                T item = repo.GetItemById(id);

                repo.RemoveItem(id);

                Console.WriteLine($"{item.Name} removed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

}

namespace WarehouseSystem
{
    public class Program
    {
        public static void Main()
        {
            var manager = new WareHouseManager();

            // Use local repositories to demonstrate PrintAllItems
            var electronics = new InventoryRepository<ElectronicItem>();
            electronics.AddItem(new ElectronicItem(1, "Laptop", 10, "Dell", 24));
            electronics.AddItem(new ElectronicItem(2, "Printer", 5, "HP", 12));

            var groceries = new InventoryRepository<GroceryItem>();
            groceries.AddItem(new GroceryItem(101, "Rice", 50, DateTime.Now.AddMonths(12)));

            Console.WriteLine("Electronics:");
            manager.PrintAllItems(electronics);

            Console.WriteLine("Groceries:");
            manager.PrintAllItems(groceries);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
