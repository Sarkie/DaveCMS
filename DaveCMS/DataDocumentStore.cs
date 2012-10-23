using Raven.Client;
using Raven.Client.Embedded;

namespace DaveCMS
{
    public static class DataDocumentStore
    {
        private static IDocumentStore _documentStore;

        public static IDocumentStore DocumentStore
        {
            get
            {
                if (_documentStore == null)
                    Init();

                return _documentStore;
            }
        }

        public static void Init()
        {
            _documentStore = new EmbeddableDocumentStore { ConnectionStringName = "RavenDB" };
            _documentStore.Conventions.IdentityPartsSeparator = "-";
            _documentStore.Initialize();
        }

        public static void Init_For_Testing()
        {
            _documentStore = new EmbeddableDocumentStore { RunInMemory = true};
            _documentStore.Conventions.IdentityPartsSeparator = "-";
            _documentStore.Initialize();
        }
    }
}