using System.Text.Json.Serialization;   // <— add this
namespace ClientDashboardAPI.Models
{
    public class PhoneNumber
    {
        public int Id { get; set; }
        public string Label { get; set; } = "Mobile";
        public string Number { get; set; } = "";
        public int ClientId { get; set; }

        [JsonIgnore]                   // <— prevent JSON cycle
        public Client Client { get; set; }
    }
}
