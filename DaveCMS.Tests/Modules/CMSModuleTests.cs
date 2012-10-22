using System;
using System.IO;
using Nancy;
using Nancy.Testing;
using Xunit;

namespace DaveCMS.Tests.Modules
{
    public class CMSModuleTests
    {

        public CMSModuleTests()
        {
            DataDocumentStore.Init_For_Testing();
        }

        [Fact]
        public void Should_be_able_to_save_a_cms_item()
        {
            // Given
           
            var bootstrapper = new CustomBootstrapper();
            var browser = new Browser(bootstrapper);
            var streamReader = new StreamReader(Path.Combine(Environment.CurrentDirectory, "Content", "Logo.png"));
            var multipart = new BrowserContextMultipartFormData(x => 
                x.AddFile("Logo.png", "Logo.png", "image/png", streamReader.BaseStream));
            // When
            var result = browser.Post("/cms/Homepage_Title_Background/",
                                      delegate(BrowserContext with)
                                      {
                                          with.HttpRequest();
                                          with.MultiPartFormData(multipart);

                                      });

            // Then
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public void Should_be_able_to_save_a_cms_item_with_a_published_date()
        {
            // Given

            var bootstrapper = new CustomBootstrapper();
            var browser = new Browser(bootstrapper);
            var streamReader = new StreamReader(Path.Combine(Environment.CurrentDirectory, "Content", "Logo.png"));
            var multipart = new BrowserContextMultipartFormData(x =>
                x.AddFile("Logo.png", "Logo.png", "image/png", streamReader.BaseStream));
            // When
            var result = browser.Post("/cms/Homepage_Title_Background/20121020/",
                                      delegate(BrowserContext with)
                                      {
                                          with.HttpRequest();
                                          with.MultiPartFormData(multipart);

                                      });

            // Then
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public void Should_be_able_to_save_a_cms_item_with_a_published_date_and_time()
        {
            // Given

            var bootstrapper = new CustomBootstrapper();
            var browser = new Browser(bootstrapper);
            var streamReader = new StreamReader(Path.Combine(Environment.CurrentDirectory, "Content", "Logo.png"));
            var multipart = new BrowserContextMultipartFormData(x =>
                x.AddFile("Logo.png", "Logo.png", "image/png", streamReader.BaseStream));
            // When
            var result = browser.Post("/cms/Homepage_Title_Background/201210202245/",
                                      delegate(BrowserContext with)
                                      {
                                          with.HttpRequest();
                                          with.MultiPartFormData(multipart);

                                      });

            // Then
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public void Should_be_able_to_get_a_cms_item_by_key()
        {
            // Given
            Should_be_able_to_save_a_cms_item();

            var bootstrapper = new CustomBootstrapper();
            var browser = new Browser(bootstrapper);

            // When
            var result = browser.Get("/cms/Homepage_Title_Background/", with => with.HttpRequest());

            // Then
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);


        }

        [Fact]
        public void Should_be_able_to_get_a_cms_item_by_key_and_date()
        {
            // Given
            var bootstrapper = new CustomBootstrapper();
            var browser = new Browser(bootstrapper);

            // When
            var result = browser.Get("/cms/Homepage_Title_Background/20121020/", with => with.HttpRequest());

            // Then
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public void Should_be_able_to_get_a_cms_item_by_key_up_until_the_date()
        {
            // Given
            var bootstrapper = new CustomBootstrapper();
            var browser = new Browser(bootstrapper);

            // When
            var result = browser.Get("/cms/Homepage_Title_Background/20121010/", with => with.HttpRequest());

            // Then
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public void Should_be_able_to_get_a_cms_item_by_key_and_date_and_time()
        {
            // Given
            var bootstrapper = new CustomBootstrapper();
            var browser = new Browser(bootstrapper);

            // When
            var result = browser.Get("/cms/Homepage_Title_Background/201210202245/", with => with.HttpRequest());

            // Then
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

    }
}