using Nancy;
using Nancy.Testing;
using Xunit;

namespace DaveCMS
{
    public class HelloModuleTests
    {
        [Fact]
        public void Should_return_status_ok_when_route_exists()
        {
            // Given
            var bootstrapper = new DefaultNancyBootstrapper();
            var browser = new Browser(bootstrapper);

            // When
            var result = browser.Get("/", with =>
            {
                with.HttpRequest();
            });

            // Then
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public void Should_redirect_to_login_with_error_details_incorrect()
        {
            // Given
            var bootstrapper = new DefaultNancyBootstrapper();
            var browser = new Browser(bootstrapper);

            // When
            var response = browser.Post("/login/", (with) =>
            {
                with.HttpRequest();
                with.FormValue("Username", "username");
                with.FormValue("Password", "wrongpassword");
            });

            // Then
            response.ShouldHaveRedirectedTo("/login?error=true&username=username");
        }
    }
}