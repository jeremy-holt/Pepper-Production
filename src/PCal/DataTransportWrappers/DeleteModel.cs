namespace PCal.DataTransportWrappers
{
    public class DeleteModel
    {
        public DeleteModel(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
    }
}