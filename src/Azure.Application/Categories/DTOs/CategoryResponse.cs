﻿
namespace Azure.Application.Categories.DTOs
{
    public class CategoryResponse
    {
        public int CategoryId { get; set; }
        public required string NameTest { get; set; }
        public string? Description { get; set; }
    }
}
