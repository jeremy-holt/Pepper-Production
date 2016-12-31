using Newtonsoft.Json;

namespace PCal.Responses
{
    public class ErrorResponse
    {
        public string Message { get; set; }

#if DEBUG
        [JsonIgnore]
#endif

        public string StackTrace { get; set; }
    }
}