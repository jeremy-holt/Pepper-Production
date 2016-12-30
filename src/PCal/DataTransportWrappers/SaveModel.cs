using PCal.Models;

namespace PCal.DataTransportWrappers
{
    public class SaveModel
    {
        public SaveModel(IEntity entity, string message)
        {
            Entity = entity;
            Message = message;
        }

        public IEntity Entity { get; }
        public string Message { get; }
    }
}