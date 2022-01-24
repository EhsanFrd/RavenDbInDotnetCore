using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;

namespace RavenDb_Sample.Data
{
    public class RavenDbDocumentStore : IDisposable
    {
        private static readonly Lazy<IDocumentStore> _store = new Lazy<IDocumentStore>(CreateDocumentStore);

        private static IDocumentStore CreateDocumentStore()
        {
            string serverURL = "http://127.0.0.1:8080";
            string databaseName = "sample_db";

            IDocumentStore documentStore = new DocumentStore
            {
                Urls = new[] { serverURL },
                Database = databaseName
            };
            documentStore.Conventions.IdentityPartsSeparator = '-';
            documentStore.Initialize();
            return documentStore;
        }

        public static IDocumentSession OpenSession()
        {
            return _store.Value.OpenSession("sample_db");
        }
        public static IDocumentStore Store
        {
            get { return _store.Value; }
        }
        public void Dispose()
        {
            _store.Value.Dispose();
        }
    }
}