namespace PCal.Responses
{
    public class DeleteResponse
    {
        public DeleteResponse(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
    }
}