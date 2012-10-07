using Nancy;
using Nancy.Testing;
using Xunit;

namespace DaveCMS.Tests.Modules
{
    public class CMSModuleTests
    {
        [Fact]
        public void Should_return_status_ok_when_route_exists()
        {
            // Given
            var bootstrapper = new CustomBootstrapper();
            var browser = new Browser(bootstrapper);

            // When
            var result = browser.Get("/cms/", with =>
            {
                with.HttpRequest();
            });

            // Then
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public void Should_return_status_ok_when_route_exists_()
        {
            // Given
            var bootstrapper = new CustomBootstrapper();
            var browser = new Browser(bootstrapper);

            // When
            var result = browser.Get("/cms/123/123", with =>
            {
                with.HttpRequest();
            });

            // Then
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        } 
    }
}