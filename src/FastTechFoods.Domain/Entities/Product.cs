namespace FastTechFoods.Domain.Entities;

public class Product
{
    public Product(string name, string description, decimal price, bool availability, string? imageUrl, Guid categoryId)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Price = price;
        Availability = availability;
        ImageUrl = imageUrl;
        CategoryId = categoryId;
        CreatedDate = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public bool Availability { get; private set; }
    public string? ImageUrl { get; private set; }
    public Guid CategoryId { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public virtual Category? Category { get; private set; }

    public void Update(string name, string description, decimal price, bool availability, string? imageUrl, Guid categoryId)
    {
        Name = name;
        Description = description;
        Price = price;
        Availability = availability;
        ImageUrl = imageUrl;
        CategoryId = categoryId;
    }

    public void SetAvailability(bool availability)
    {
        Availability = availability;
    }

    public void UpdateCategory(Guid categoryId)
    {
        CategoryId = categoryId;
    }

    public void SetImageUrl(string imageUrl)
    {
        ImageUrl = imageUrl;
    }
}
