using System.Net.Http;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Document.Tests
{
    [TestClass]
    [TestCategory(Constants.TestCategory)]
    public class Post_ApplicationJson_ExceptionObject_Tests
    {

        private static HttpClient http = new HttpClient();

        private JObject _doc;

        [TestInitialize]
        public async Task Init()
        {
            var json = await http.GetStringAsync(Constants.OpenApiDocEndpoint).ConfigureAwait(false);
            this._doc = JsonConvert.DeserializeObject<JObject>(json);
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-exception", "post")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBody(string path, string operationType)
        {
            var requestBody = this._doc["paths"][path][operationType]["requestBody"];

            requestBody.Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-exception", "post", "application/json")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentType(string path, string operationType, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-exception", "post", "application/json", "system.InvalidOperationException")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentReferenceSchema(string path, string operationType, string contentType, string reference)
        {
            var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

            var @ref = content[contentType]["schema"]["$ref"];

            @ref.Value<string>().Should().Be($"#/components/schemas/{reference}");
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-exception", "post", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-exception", "post", "200", "application/json")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-exception", "post", "200", "application/json", "microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.ExceptionObjectModel")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema(string path, string operationType, string responseCode, string contentType, string reference)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var @ref = content[contentType]["schema"]["$ref"];

            @ref.Value<string>().Should().Be($"#/components/schemas/{reference}");
        }

        [DataTestMethod]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.ExceptionObjectModel", "object")]
        [DataRow("exception", "object")]
        [DataRow("system.InvalidOperationException", "object")]
        [DataRow("system.StackOverflowException", "object")]
        public void Given_OpenApiDocument_Then_It_Should_Return_ComponentSchema(string @ref, string refType)
        {
            var schemas = this._doc["components"]["schemas"];

            var schema = schemas[@ref];

            schema.Should().NotBeNull();
            schema.Value<string>("type").Should().Be(refType);
        }

        [DataTestMethod]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.ExceptionObjectModel", "exceptionValue", "object")]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.ExceptionObjectModel", "invalidOperationExceptionValue", "object")]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.ExceptionObjectModel", "stackOverflowExceptionValue", "object")]
        public void Given_OpenApiDocument_Then_It_Should_Return_ComponentSchemaProperty(string @ref, string propertyName, string propertyType)
        {
            var properties = this._doc["components"]["schemas"][@ref]["properties"];

            var value = properties[propertyName];

            value.Should().NotBeNull();
            value.Value<string>("type").Should().Be(propertyType);
        }
    }
}
