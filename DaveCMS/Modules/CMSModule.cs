using System;
using System.Collections.Generic;
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
        private void SaveCMSItem(string key, HttpFile file, DateTime dateToBePublished)
        {
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
                    DateToBePublished = dateToBePublished,
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
        }

        
        public CMSModule()
        {
            Post["/cms/"] = parameters =>
            {

                var file = this.Request.Files.FirstOrDefault();

                if (file == null)
                    return HttpStatusCode.BadRequest;

                var key = Request.Form.key;

                if (string.IsNullOrEmpty(key))
                    return HttpStatusCode.BadRequest;

                SaveCMSItem(key, file, DateTime.Now);

                return HttpStatusCode.OK;
            };

            Post["/cms/"] = parameters =>
            {

                var file = this.Request.Files.FirstOrDefault();

                if (file == null)
                    return HttpStatusCode.BadRequest;

                var key = (string)parameters["key"];

                if (string.IsNullOrEmpty(key))
                    return HttpStatusCode.BadRequest;

                var date = GetDateTime((string)parameters["date"]);

                if (!date.HasValue)
                    return HttpStatusCode.BadRequest;

                SaveCMSItem(key, file, date.Value);

                return HttpStatusCode.OK;
            };



            Get["/cms/{key}/"] = parameters =>
                                     {
                                         var key = (string)parameters["key"];

                                         if (string.IsNullOrEmpty(key))
                                             return HttpStatusCode.BadRequest;

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

                if (string.IsNullOrEmpty(key))
                    return HttpStatusCode.BadRequest;

                var date = GetDateTime((string) parameters["date"]);

                if(!date.HasValue)
                    return HttpStatusCode.BadRequest;

                using (var db = DataDocumentStore.DocumentStore.OpenSession())
                {
                    var cmsItem = db.Query<CMSItem>().FirstOrDefault(x => x.Key == key);

                    if (cmsItem == null)
                        return HttpStatusCode.Gone;

                    var version = GetVersion(cmsItem.Versions, date.Value);

                    if (version == null)
                        return HttpStatusCode.Gone;

                    var attachment =
                        DataDocumentStore.DocumentStore.DatabaseCommands.GetAttachment(version.AttachmentLocation);

                    return Response.FromStream(attachment.Data(), version.ContentType);

                }

            };
        }

        private static DateTime? GetDateTime(string date)
        {
            DateTime parsedDate;
            if (DateTime.TryParseExact(
               date,
                "yyyyMMddHmm",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out parsedDate))
            {
                return parsedDate;
            }

            if (DateTime.TryParseExact(
           date,
           "yyyyMMdd",
           CultureInfo.InvariantCulture,
           DateTimeStyles.None,
           out parsedDate))
            {
                return parsedDate;
            }

            return null;

        }

        private static Version GetVersion(IList<Version> versions, DateTime date)
        {
            var version = versions.FirstOrDefault(x =>
                       x.DateCreated.Year == date.Year &&
                       x.DateCreated.Month == date.Month &&
                       x.DateCreated.Day == date.Day &&
                       x.DateCreated.Hour == date.Hour &&
                       x.DateCreated.Minute == date.Minute);

            if (version != null)
                return version;

            version = versions.FirstOrDefault(x =>
                      x.DateCreated.Year == date.Year &&
                      x.DateCreated.Month == date.Month &&
                      x.DateCreated.Day == date.Day &&
                      x.DateCreated.Hour == date.Hour);

            if (version != null)
                return version;

            version = versions.FirstOrDefault(x =>
                      x.DateCreated.Year == date.Year &&
                      x.DateCreated.Month == date.Month &&
                      x.DateCreated.Day == date.Day);

            if (version != null)
                return version;

            version = versions.FirstOrDefault(x =>
                      x.DateCreated.Year == date.Year &&
                      x.DateCreated.Month == date.Month);

            if (version != null)
                return version;

            version = versions.FirstOrDefault(x =>
                      x.DateCreated.Year == date.Year);

            if (version != null)
                return version;

            version = versions.FirstOrDefault();

            if (version != null)
                return version;

            return null;

        }
    }
}