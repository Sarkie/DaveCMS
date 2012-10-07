﻿using Raven.Client;
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

        private static void Init()
        {
            _documentStore = new EmbeddableDocumentStore { ConnectionStringName = "RavenDB" };
            _documentStore.Conventions.IdentityPartsSeparator = "-";
            _documentStore.Initialize();
        }
    }
}