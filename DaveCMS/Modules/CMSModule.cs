using System;
using System.Globalization;
using System.IO;
using System.Linq;
using DaveCMS.Models;
using Nancy;
using Nancy.ModelBinding;
using Raven.Json.Linq;
using Version = DaveCMS.Models.Version;

namespace DaveCMS.Modules
{
    public class CMSModule : NancyModule
    {


        public CMSModule()
        {
            Post["/cms/{key}/"] = parameters =>
            {
                var file = this.Request.Files.FirstOrDefault();

                if (file == null)
                    return HttpStatusCode.BadRequest;

                var key = (string)parameters["key"];

                using (var db = DataDocumentStore.DocumentStore.OpenSession())
                {
                    var cmsItem = db.Query<CMSItem>().SingleOrDefault(x => x.Key == key);

                    if (cmsItem == null)
                    {
                        cmsItem = new CMSItem
                                      {
                                          DateCreated = DateTime.Now,
                                          Key = key
                                      };

                    }
                    var newVersion = new Version
                                               {
                                                   ContentType = file.ContentType,
                                                   DateCreated = DateTime.Now,
                                                   AttachmentLocation = "content/" + key + "/" + cmsItem.Versions.Count + 1
                                               };

                    cmsItem.AddVersion(newVersion);

                    db.Store(cmsItem);

                    DataDocumentStore.DocumentStore.DatabaseCommands.
                      PutAttachment(newVersion.AttachmentLocation,
                      null,
                      file.Value,
                      new RavenJObject());

                    db.SaveChanges();
                }



                return HttpStatusCode.OK;
            };


            Get["/cms/{key}/"] = parameters =>
                                     {
                                         var key = (string)parameters["key"];

                                         using (var db = DataDocumentStore.DocumentStore.OpenSession())
                                         {
                                             var cmsItem = db.Query<CMSItem>().SingleOrDefault(x => x.Key == key);

                                             if (cmsItem == null)
                                                 return HttpStatusCode.Gone;

                                             var version = cmsItem.Versions.Last();

                                             var attachment =
                                                 DataDocumentStore.DocumentStore.DatabaseCommands.GetAttachment(version.AttachmentLocation);

                                             return Response.FromStream(attachment.Data(), version.ContentType);

                                         }
                                     };



            Get["/cms/{key}/{date}/"] = parameters =>
            {
                var key = (string)parameters["key"];
                DateTime date;
                if(!DateTime.TryParseExact(
                    (string)parameters["date"], 
                    "yyyyMMddHmm", 
                    CultureInfo.InvariantCulture, 
                    DateTimeStyles.None,
                    out date))
                {
                     if(!DateTime.TryParseExact(
                    (string)parameters["date"], 
                    "yyyyMMdd", 
                    CultureInfo.InvariantCulture, 
                    DateTimeStyles.None,
                    out date))
                     {
                         return HttpStatusCode.BadRequest;
                     }
                }

                using (var db = DataDocumentStore.DocumentStore.OpenSession())
                {
                    var cmsItem = db.Query<CMSItem>().SingleOrDefault(x => x.Key == key);

                    if (cmsItem == null)
                        return HttpStatusCode.Gone;

                    var version = cmsItem.Versions.FirstOrDefault(x => 
                        x.DateCreated.Year== date.Year &&
                        x.DateCreated.Month == date.Month &&
                        x.DateCreated.Day == date.Day &&
                        x.DateCreated.Hour == date.Hour &&
                        x.DateCreated.Minute == date.Minute);

                    if (version == null)
                        return HttpStatusCode.Gone;

                    var attachment =
                        DataDocumentStore.DocumentStore.DatabaseCommands.GetAttachment(version.AttachmentLocation);

                    return Response.FromStream(attachment.Data(), version.ContentType);

                }

                return HttpStatusCode.Gone;
            };
        }
    }
}