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
                     var c = System.Web.HttpContext.Current;

                    var data = File.ReadAllBytes(c.Server.MapPath("~/Content/logo.png"));

                    using(var mem = new MemoryStream(data))
                    {
                        var metaData = new RavenJObject();
                        metaData["Format"] = "PNG";

                        DataDocumentStore.DocumentStore.DatabaseCommands.
                            PutAttachment("videos/2", null, mem,
                            new RavenJObject { { "Description", "Kids play in the garden" } });

                        db.Store(item);
                    }

                   
                    db.SaveChanges();
                }

              

                return HttpStatusCode.OK;
            };


            Get["/cms/{id}"] = parameters =>
                                   {

                                       using (var db = DataDocumentStore.DocumentStore.OpenSession())
                                       {
                                           
                                           var attachment = DataDocumentStore.
                                               DocumentStore.DatabaseCommands.GetAttachment("videos/2");

                                           
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