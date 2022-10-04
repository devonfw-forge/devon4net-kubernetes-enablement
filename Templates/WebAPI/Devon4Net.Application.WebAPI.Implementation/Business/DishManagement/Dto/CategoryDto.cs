using Newtonsoft.Json;

namespace Devon4Net.Application.WebAPI.Implementation.Business.DishManagement.Dto
{
    public class CategoryDto
    {
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }
        [JsonProperty(PropertyName = "modificationCounter")]
        public int? ModificationCounter { get; set; }
        [JsonProperty(PropertyName = "revision")]
        public int Revision { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "showOrder")]
        public int? ShowOrder { get; set; }
    }
}
