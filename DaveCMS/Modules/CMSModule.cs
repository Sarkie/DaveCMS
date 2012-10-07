using System.IO;
using System.Linq;
using DaveCMS.Models;
using Nancy;
using Nancy.ModelBinding;
using Raven.Json.Linq;

namespace DaveCMS.Modules
{
    public class CMSModule : NancyModule
    {


        public CMSModule()
        {
            Post["/cms/{id}"] = parameters =>
            {
                var item = this.Bind<CMSItem>();

             

                using (var db = DataDocumentStore.DocumentStore.OpenSession())
                {
                    Stream data = new MemoryStream(new byte[] { 1, 2, 3 }); // don't forget to load the data from a file or something!
                    DataDocumentStore.DocumentStore.DatabaseCommands.
                        PutAttachment("videos/2", null, data,
                        new RavenJObject { { "Description", "Kids play in the garden" } });

                    db.Store(item);
                    db.SaveChanges();
                }

              

                return HttpStatusCode.OK;
            };


            Get["/cms/{id}"] = parameters =>
                                   {

                                       using (var db = DataDocumentStore.DocumentStore.OpenSession())
                                       {
                                           
                                           var attachment = DataDocumentStore.DocumentStore.DatabaseCommands.GetAttachment("videos/1");

                                           
                                          // Response.AsImage("/");

                                           return Response.FromStream(attachment.Data(), "image/png");
                                       }


                                       
                                   };

           

            Get["/cms/{id}/{publisheddate}"] = parameters =>
            {
                return HttpStatusCode.OK;
            };
        }
    }
}