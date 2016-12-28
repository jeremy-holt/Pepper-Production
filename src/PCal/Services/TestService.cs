using Raven.Client;

namespace PCal.Services
{
    public class TestService : ITestService
    {
        public TestService(IAsyncDocumentSession session)
        {
            Session = session;
        }


        public IDocumentStore DocumentStore()
        {
            return Session.Advanced.DocumentStore;
        }

        public IAsyncDocumentSession Session { get; }
    }
}