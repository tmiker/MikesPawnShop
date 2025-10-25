﻿using Products.Write.Domain.Enumerations;
using Products.Write.Domain.ValueObjects;
using System.Text.Json;

namespace Products.Write.Domain.Snapshots
{
    public class ProductSnapshot
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public string? Category { get; init; }
        public string? Description { get; init; }
        public decimal Price { get; init; }
        public string? Currency { get; init; }
        public string? Status { get; init; }
        public List<ImageData>? Images { get; init; }
        public List<DocumentData>? Documents { get; init; }
        public int Version { get; init; }
        public DateTime DateCreated { get; init; }
        public DateTime DateUpdated { get; init; }

        public string ToJson()
        {
            JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };
            return JsonSerializer.Serialize(this, options);
        }
    }
}
