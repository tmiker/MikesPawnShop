using Products.Write.Application.DTOs;
using Products.Write.Domain.Aggregates;
using Products.Write.Domain.Enumerations;
using Products.Write.Domain.Snapshots;
using System.Text.Json;
using System.Text;
using Products.Write.Application.CQRS.CommandResults;
using System.Net.Http.Json;

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

async Task LoadData()
{
    Console.WriteLine($"Starting data loading ...");
    HttpClient client = new HttpClient();
    string baseUrl = "https://localhost:7213/api/productsManagement";
    int counter = 3;
    List<string> aggregateIdStrings = new List<string>();

    while (counter > 0)
    {
        // add a new product
        string category = GetRandomCategory();
        decimal price = (decimal)(new Random().Next(999, 499999)) / 100;
        AddProductDTO addProductDTO = new AddProductDTO($"Product {counter}", category, $"Description {counter}", price, "USD", "Active");
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, baseUrl);
        request.Content = new StringContent(JsonSerializer.Serialize(addProductDTO), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            AddProductResult? result = await response.Content.ReadFromJsonAsync<AddProductResult>();
            string? aggregateId = result?.ProductId.ToString();
            if (!string.IsNullOrWhiteSpace(aggregateId))
            {
                aggregateIdStrings.Add(aggregateId);
                Console.WriteLine($"AggregateId {aggregateId} added to list!");
            }
        }
        else Console.WriteLine($"AddProduct Request for counter {counter} failed.");

        await Task.Delay(50);
        
        if (aggregateIdStrings.Count > 0)
        {
            try
            {
                // update a status
                for (int i = 0; i < aggregateIdStrings.Count; i++)
                {
                    // client = new HttpClient();
                    string status = i % 2 == 0 ? "InActive" : "Obsolete";
                    UpdateStatusDTO updateStatusDTO = new UpdateStatusDTO(Guid.Parse(aggregateIdStrings[i]), status);
                    string statusUri = $"{baseUrl}/status";
                    HttpRequestMessage statusRequest = new HttpRequestMessage(HttpMethod.Post, statusUri);
                    statusRequest.Content = new StringContent(JsonSerializer.Serialize(updateStatusDTO), Encoding.UTF8, "application/json");
                    HttpResponseMessage statusResponse = await client.SendAsync(statusRequest);
                    if (statusResponse.IsSuccessStatusCode)
                    {
                        UpdateStatusResult? statusResult = await statusResponse.Content.ReadFromJsonAsync<UpdateStatusResult>();
                        Console.WriteLine($"UpdateStatus Request for counter {counter}, Aggregate {i}: {statusResult?.IsSuccess}");
                    }
                    else
                    {
                        string statusError = await statusResponse.Content.ReadAsStringAsync();
                        Console.WriteLine($"UpdateStatus Request for counter {counter}, Aggregate {i} failed. Error: {statusError}");
                    }
                    await Task.Delay(50);
                }

                //// add an image - CHANGED IMPLEMENTATION!!!
                //for (int i = 0; i < aggregateIdStrings.Count; i++)
                //{
                //    AddImageDTO addImageDTO = new AddImageDTO() { ProductId = Guid.Parse(aggregateIdStrings[i]), Name = $"Image {counter}", Caption = $"Caption {counter}", SequenceNumber = counter, ImageUrl = $"ImageURL {counter}", ThumbnailUrl = $"ThumbURL {counter}" };
                //    string imageUri = $"{baseUrl}/image";
                //    HttpRequestMessage imageRequest = new HttpRequestMessage(HttpMethod.Post, imageUri);
                //    imageRequest.Content = new StringContent(JsonSerializer.Serialize(addImageDTO), Encoding.UTF8, "application/json");
                //    HttpResponseMessage imageResponse = await client.SendAsync(imageRequest);
                //    if (imageResponse.IsSuccessStatusCode)
                //    {
                //        AddImageResult? imageResult = await imageResponse.Content.ReadFromJsonAsync<AddImageResult>();
                //        Console.WriteLine($"AddImage Request for counter {counter}, Aggregate {i}: {imageResult?.IsSuccess}");
                //    }
                //    else
                //    {
                //        string imageError = await imageResponse.Content.ReadAsStringAsync();
                //        Console.WriteLine($"AddImage Request for counter {counter}, Aggregate {i} failed. Error: {imageError}");
                //    }
                //    await Task.Delay(50);
                //}

                //// add a document - CHANGED IMPLEMENTATION!!!
                //for (int i = 0; i < aggregateIdStrings.Count; i++)
                //{
                //    AddDocumentDTO addDocDTO = new AddDocumentDTO(Guid.Parse(aggregateIdStrings[i]), $"Document {counter}",$"Title {counter}", counter, $"DocumentURL {counter}");
                //    string docUri = $"{baseUrl}/document";
                //    HttpRequestMessage docRequest = new HttpRequestMessage(HttpMethod.Post, docUri);
                //    docRequest.Content = new StringContent(JsonSerializer.Serialize(addDocDTO), Encoding.UTF8, "application/json");
                //    HttpResponseMessage docResponse = await client.SendAsync(docRequest);
                //    if (docResponse.IsSuccessStatusCode)
                //    {
                //        AddDocumentResult? docResult = await docResponse.Content.ReadFromJsonAsync<AddDocumentResult>();
                //        Console.WriteLine($"AddDocument Request for counter {counter}, Aggregate {i}: {docResult?.IsSuccess}");
                //    }
                //    else
                //    {
                //        string docError = await docResponse.Content.ReadAsStringAsync();
                //        Console.WriteLine($"AddDocument Request for counter {counter}, Aggregate {i} failed. Error: {docError}");
                //    }
                //    await Task.Delay(50);
                //}
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        counter--;
    }
    Console.WriteLine($"Data loading completed.");
}

Console.WriteLine($"Do you want to run the script to load data? Answer Y or N: ");
string? reply = Console.ReadLine();

if (reply == "Y" || reply == "y")
{
    Console.WriteLine($"Your response was {reply}");
    await LoadData();
}
//else
//{
//    Console.WriteLine($"Do you want to process unprocessed Message Records? Answer Y or N: ");
//    reply = Console.ReadLine();
//    if (reply == "Y" || reply == "y")
//    {
//        Console.WriteLine($"Your response was {reply}");
        
//    }
//}

Console.ReadLine();






    string GetRandomCategory()
    {
        int next = new Random().Next(0, 6);
        CategoryEnum category = (CategoryEnum)next;
        return category.ToString();
    }