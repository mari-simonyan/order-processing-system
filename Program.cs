using System;

interface INotifier
{
    void Send(string message);
}

class EmailNotifier : INotifier
{
    public void Send(string message)
    {
        Console.WriteLine("Email: " + message);
    }
}

class SmsNotifier : INotifier
{
    public void Send(string message)
    {
        Console.WriteLine("SMS: " + message);
    }
}

interface IOrderRepository
{
    void Save(Order order);
}

class OrderRepository : IOrderRepository
{
    public void Save(Order order)
    {
        Console.WriteLine($"Order {order.Id} saved for {order.CustomerName}");
    }
}

class Order
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string CustomerName { get; set; }

    public void Create()
    {
        Console.WriteLine($"Order {Id} created for {CustomerName}, amount: {Amount:C}");
    }
}

class OrderService
{
    private readonly INotifier notifier;
    private readonly IOrderRepository repository;

    public OrderService(INotifier notifier, IOrderRepository repository)
    {
        this.notifier = notifier;
        this.repository = repository;
    }

    public Order ProcessOrder(int id, decimal amount, string customerName)
    {
        Order order = new Order
        {
            Id = id,
            Amount = amount,
            CustomerName = customerName
        };

        order.Create();
        repository.Save(order);
        notifier.Send($"Order {order.Id} processed for {order.CustomerName}");

        return order;
    }
}

class Program
{
    static void Main()
    {
        OrderService service =
            new OrderService(
                new EmailNotifier(),
                new OrderRepository()
            );

        Order order = service.ProcessOrder(1, 250.75m, "Ani");
        Console.WriteLine($"Finalized Order: {order.Id}, Customer: {order.CustomerName}, Amount: {order.Amount:C}");
    }
}
