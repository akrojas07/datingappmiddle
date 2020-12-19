using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Infrastructure.StockPhotoAPI.Models
{
    public class PhotoGallery
    {
        [JsonProperty("total_results")]
        public int TotalResults { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }
        
        [JsonProperty("per_page")]
        public int PerPage { get; set; }

        [JsonProperty("photos")]
        public List<Photo> Photos { get; set; }

        [JsonProperty("next_page")]
        public string NextPage { get; set; }
    }
}
