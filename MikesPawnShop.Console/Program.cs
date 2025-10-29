using Products.Read.API.Infrastructure.Repositories;
using Products.Write.Domain.Aggregates;
using Products.Write.Domain.Enumerations;
using Products.Write.Domain.Snapshots;
using System.Text.Json;

JsonSerializerOptions jsonOptions = new JsonSerializerOptions() { WriteIndented = true };

//// Products.Domain Tests
//Product product = new Product("Product 1", CategoryEnum.Books, "A book on things.", 25.99m, 
//    "USD", "Active", Guid.NewGuid().ToString());
//product.UpdateStatus("InActive", Guid.NewGuid().ToString());
//product.AddImage("Image 1", "A dog", 3, "Image URL", "Thumb URL", Guid.NewGuid().ToString());
//product.AddDocument("Doc 1", "Instructions", 1, "Document URL", Guid.NewGuid().ToString());
//ProductSnapshot snapshot = product.GetSnapshot();
//string json = JsonSerializer.Serialize(snapshot, jsonOptions);
//Console.WriteLine(json);

// Products.Read.API Tests


