using Nancy;
using Nancy.Testing;
using Xunit;

namespace DaveCMS.Tests.Modules
{
    public class ExampleSiteTests
    {
        [Fact]
        public void Should_be_able_to_get_root()
        {
            // Given
            var bootstrapper = new CustomBootstrapper();
            var browser = new Browser(bootstrapper);

            // When
            var result = browser.Get("/", with => with.HttpRequest());

            // Then
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        } 
    }
}