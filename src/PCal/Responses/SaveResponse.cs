using PCal.Models;

namespace PCal.Responses
{
    public class SaveResponse
    {
        public SaveResponse(IEntity entity, string message)
        {
            Entity = entity;
            Message = message;
        }

        public IEntity Entity { get; }
        public string Message { get; }
    }
}