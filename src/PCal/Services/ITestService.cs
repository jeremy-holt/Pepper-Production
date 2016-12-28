using Raven.Client;

namespace PCal.Services
{
    public interface ITestService
    {
        IAsyncDocumentSession Session { get; }
        IDocumentStore DocumentStore();
    }
}