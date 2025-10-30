﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Products.Read.API.DTOs
{
    public class ProductDTO
    {
        public int Id { get; init; }
        public Guid AggregateId { get; init; }
        public string? Name { get; init; }
        public string? Category { get; init; }
        public string? Description { get; init; }
        public decimal Price { get; init; }
        public string? Currency { get; init; }
        public string? Status { get; init; }
        public int Version { get; init; }
        public DateTime DateCreated { get; init; }
        public DateTime DateUpdated { get; init; }
        public List<ImageDataDTO>? Images { get; init; }
        public List<DocumentDataDTO>? Documents { get; init; }

        
    }
}
