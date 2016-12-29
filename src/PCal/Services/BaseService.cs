using Raven.Client;

namespace PCal.Services
{
    public class BaseService
    {
        protected IAsyncDocumentSession Session { get; }

        protected BaseService(IAsyncDocumentSession session)
        {
            Session = session;
        }
    }
}
