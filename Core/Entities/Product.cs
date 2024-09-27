using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Product :BaseEntity
    {

        [JsonPropertyName("name")]

        public required string Name { get; set; }
        [JsonPropertyName("description")]

        public required string Description { get; set; }
        [JsonPropertyName("price")]

        public decimal Price { get; set; }
        [JsonPropertyName("pictureUrl")]

        public required string PictureUrl { get; set; }
        [JsonPropertyName("type")]

        public required string Type { get; set; }
        [JsonPropertyName("brand")]

        public required string Brand { get; set; }
        [JsonPropertyName("quantityInStock")]

        public int QuantityInStock { get; set; }
    }
}
