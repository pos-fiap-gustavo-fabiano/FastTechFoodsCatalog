namespace FastTechFoods.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public bool Availability { get; private set; }
    public string Type { get; private set; } = string.Empty;
    public DateTime CreatedDate { get; private set; }

    protected Product() { }

    public Product(string name, string description, decimal price, bool availability, string type)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Price = price;
        Availability = availability;
        Type = type;
        CreatedDate = DateTime.UtcNow;
    }

    public void Update(string name, string description, decimal price, bool availability, string type)
    {
        Name = name;
        Description = description;
        Price = price;
        Availability = availability;
        Type = type;
    }

    public void SetAvailability(bool availability)
    {
        Availability = availability;
    }
}
