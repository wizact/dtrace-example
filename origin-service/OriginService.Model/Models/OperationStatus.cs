using Newtonsoft.Json;

namespace OriginService.Model.Models
{
    public class OperationStatus
    {
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string Message { get; }

        public OperationStatus(string message)
        {
            Message = message;
        }

    }
}