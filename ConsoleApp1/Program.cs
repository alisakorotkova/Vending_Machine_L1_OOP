using System;
using System.Collections.Generic;

public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public Product(string name, decimal price, int quantity)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
    }
}

public class VendingMachine
{
    private List<Product> products;
    private decimal currentBalance;
    private Dictionary<decimal, int> coins;
    private decimal[] acceptedCoins = { 1, 2, 5, 10 };
    private const string AdminPassword = "123";
    private decimal totalRevenue;
    
    public VendingMachine()
    {
        products = new List<Product>();
        currentBalance = 0;
        coins = new Dictionary<decimal, int>();
        totalRevenue = 0;
        InitializeCoins();
        InitializeProducts();
    }
    
    private void InitializeCoins()
    {
        foreach (decimal coin in acceptedCoins)
        {
            coins[coin] = 10;
        }
    }
    
    private void InitializeProducts()
    {
        products.Add(new Product("Добрый кола", 80, 5));
        products.Add(new Product("Шоколадка Милка", 60, 3));
        products.Add(new Product("Вода без газа", 20, 15));
        products.Add(new Product("Вода газированная", 30, 10));
        products.Add(new Product("Лимонад", 70, 5));
    }
    
    public void InsertCoin(decimal coinValue)
    {
        if (Array.IndexOf(acceptedCoins, coinValue) != -1)
        {
            currentBalance += coinValue;
            coins[coinValue]++;
            Console.WriteLine("Внесено: " + coinValue + " руб. Текущий баланс: " + currentBalance + " руб.");
        }
        else
        {
            Console.WriteLine("Монета номиналом " + coinValue + " не принимается");
        }
    }
    
    public void DisplayProducts()
    {
        Console.WriteLine("ДОСТУПНЫЕ ДЛЯ ПОКУПКИ ТОВАРЫ");
        for (int i = 0; i < products.Count; i++)
        {
            Console.WriteLine(i + 1 + ". " + products[i].Name + " - " + products[i].Price + " руб. (осталось " + products[i].Quantity + ")");
        }
        Console.WriteLine("Текущий баланс: " + currentBalance + " руб.");
    }
    
    public decimal GetCurrentBalance()
    {
        return currentBalance;
    }
    
    public void ReturnMoney()
    {
        if (currentBalance == 0)
        {
            Console.WriteLine("Нет денег для возврата");
            return;
        }
        
        Console.WriteLine("Возвращаем: " + currentBalance + " руб.");
        currentBalance = 0;
    }
    
    public void PurchaseProduct(int productIndex)
    {
        if (productIndex < 0 || productIndex >= products.Count)
        {
            Console.WriteLine("Неверный номер товара");
            return;
        }
        
        Product product = products[productIndex];
        
        if (product.Quantity <= 0)
        {
            Console.WriteLine("Товар закончился");
            return;
        }
        
        if (currentBalance < product.Price)
        {
            decimal needed = product.Price - currentBalance;
            Console.WriteLine("Недостаточно денег. Не хватает " + needed + " руб.");
            return;
        }
        
        decimal change = currentBalance - product.Price;
        currentBalance = 0;
        product.Quantity--;
        totalRevenue += product.Price;
        
        Console.WriteLine("Вы купили: " + product.Name);
        
        if (change > 0)
        {
            Console.WriteLine("Сдача " + change + " руб.");
            GiveChange(change);
        }
    }
    
    private void GiveChange(decimal amount)
    {
        Console.WriteLine("Выдаем сдачу: " + amount + " руб.");
    }
    
    public bool CheckAdminPassword(string password)
    {
        return password == AdminPassword;
    }
    
    public void AddProductQuantity(int productIndex, int quantity)
    {
        if (productIndex >= 0 && productIndex < products.Count && quantity > 0)
        {
            products[productIndex].Quantity += quantity;
            Console.WriteLine("Товар " + products[productIndex].Name + " пополнен на " + quantity + " шт.");
        }
    }

    public void AddNewProduct(string name, decimal price, int quantity)
    {
        products.Add(new Product(name, price, quantity));
        Console.WriteLine("Новый товар добавлен: " + name + " - " + price + " руб. (" + quantity + " шт.)");
    }

    public void RemoveProduct(int productIndex)
    {
        if (productIndex >= 0 && productIndex < products.Count)
        {
            string productName = products[productIndex].Name;
            products.RemoveAt(productIndex);
            Console.WriteLine("Товар удален: " + productName);
        }
        else
        {
            Console.WriteLine("Неверный номер товара");
        }
    }
    
    public void CollectMoney()
    {
        Console.WriteLine("Собрано денег: " + totalRevenue + " руб.");
        totalRevenue = 0;
        Console.WriteLine("Все деньги изъяты из автомата");
    }
    
    public void ShowAllProductsForAdmin()
    {
        Console.WriteLine("- ВСЕ ТОВАРЫ -");
        for (int i = 0; i < products.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {products[i].Name} - {products[i].Price} руб. (осталось: {products[i].Quantity})");
        }
    }
}

public class Program
{
    public static void Main()
    {
        VendingMachine machine = new VendingMachine();
        
        while (true)
        {
            Console.Clear();
            Console.WriteLine("- ВЕНДИНГОВЫЙ АВТОМАТ -");
            Console.WriteLine("1. Режим покупателя");
            Console.WriteLine("2. Режим администратора");
            Console.WriteLine("3. Выход");
            Console.Write("Выберите режим: ");
            
            string choice = Console.ReadLine();
            
            switch (choice)
            {
                case "1":
                    CustomerMode(machine);
                    break;
                case "2":
                    AdminMode(machine);
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Неверный выбор");
                    break;
            }
        }
    }
    
    public static void CustomerMode(VendingMachine machine)
    {
        while (true)
        {
            Console.Clear();
            machine.DisplayProducts();
            Console.WriteLine("\n- РЕЖИМ ПОКУПАТЕЛЯ -");
            Console.WriteLine("1. Внести монету");
            Console.WriteLine("2. Купить товар");
            Console.WriteLine("3. Вернуть деньги");
            Console.WriteLine("4. Назад в главное меню");
            Console.Write("Выберите действие: ");
            
            string choice = Console.ReadLine();
            
            switch (choice)
            {
                case "1":
                    Console.Write("Введите номинал монеты (1, 2, 5, 10): ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal coin))
                    {
                        machine.InsertCoin(coin);
                    }
                    break;
                    
                case "2":
                    Console.Write("Введите номер товара: ");
                    if (int.TryParse(Console.ReadLine(), out int productNum))
                    {
                        machine.PurchaseProduct(productNum - 1);
                    }
                    break;
                    
                case "3":
                    machine.ReturnMoney();
                    break;
                    
                case "4":
                    return;
                    
                default:
                    Console.WriteLine("Неверный выбор");
                    break;
            }
            
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
    
    public static void AdminMode(VendingMachine machine)
    {
        Console.Write("Введите пароль администратора: ");
        string password = Console.ReadLine();
        
        if (!machine.CheckAdminPassword(password))
        {
            Console.WriteLine("Неверный пароль");
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
            return;
        }
        
        while (true)
        {
            Console.Clear();
            Console.WriteLine("- РЕЖИМ АДМИНИСТРАТОРА -");
            Console.WriteLine("1. Пополнить товар");
            Console.WriteLine("2. Добавить новый товар");
            Console.WriteLine("3. Удалить товар");
            Console.WriteLine("4. Собрать деньги");
            Console.WriteLine("5. Назад в главное меню");
            Console.Write("Выберите действие: ");
            
            string choice = Console.ReadLine();
            
            switch (choice)
            {
                case "1":
                    machine.ShowAllProductsForAdmin();
                    Console.Write("Введите номер товара для пополнения: ");
                    if (int.TryParse(Console.ReadLine(), out int productNum))
                    {
                        Console.Write("Введите количество: ");
                        if (int.TryParse(Console.ReadLine(), out int quantity))
                        {
                            machine.AddProductQuantity(productNum - 1, quantity);
                        } 
                    }
                    break;
                    
                case "2":
                    Console.Write("Введите название нового товара: ");
                    string name = Console.ReadLine();
                    Console.Write("Введите цену товара: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal price))
                    {
                        Console.Write("Введите количество: ");
                        if (int.TryParse(Console.ReadLine(), out int newQuantity))
                        {
                            machine.AddNewProduct(name, price, newQuantity);
                        }
                    }
                    break;
                    
                case "3":
                    machine.ShowAllProductsForAdmin();
                    Console.Write("Введите номер товара для удаления: ");
                    if (int.TryParse(Console.ReadLine(), out int removeProductNum))
                    {
                        machine.RemoveProduct(removeProductNum - 1);
                    }
                    break;
                    
                case "4":
                    machine.CollectMoney();
                    break;
                    
                case "5":
                    return;
                    
                default:
                    Console.WriteLine("Неверный выбор");
                    break;
            }
            
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}