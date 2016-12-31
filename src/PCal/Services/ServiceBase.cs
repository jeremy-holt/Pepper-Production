using PCal.Models;
using Raven.Client;

namespace PCal.Services
{
    public class ServiceBase
    {
        protected IAsyncDocumentSession Session { get; }

        protected ServiceBase(IAsyncDocumentSession session)
        {
            Session = session;
        }
    }
}