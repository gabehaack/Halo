using System;
using System.Collections.Generic;

namespace HaloApp.ApiClient.Models
{
    public class Map
    {
        public string name { get; set; }
        public string description { get; set; }
        public List<string> supportedGameModes { get; set; }
        public string imageUrl { get; set; }
        public Guid id { get; set; }

        public Domain.Models.Map ToDomainModel()
        {
            return new Domain.Models.Map
            {
                Id = id,
                ImageUrl = imageUrl,
                Name = name,
            };
        }
    }
}
