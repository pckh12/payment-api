using Newtonsoft.Json;
using Payment.API.Application.Commands;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Payment.API.Test.FunctionalTests
{
    public class PaymentScenarios : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private static string PaymentsUrlBase => "paymentapi";

        public PaymentScenarios(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Add("ACCOUNT_KEY", "6339d07a-430e-4029-a35c-13e815bcfab4");
        }

        [Fact]
        public async Task get_payments_returns_ok()
        {
            var httpResponse = await _client.GetAsync($"{PaymentsUrlBase}/payments");

            httpResponse.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        }

        [Fact]
        public async Task create_payment_returns_created()
        {
            var command = new CreatePaymentCommand { Amount = 5, Date = new DateTime(2020, 3, 1) };
            var content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            var httpResponse = await _client.PostAsync($"{PaymentsUrlBase}/payments", content);

            httpResponse.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.Created, httpResponse.StatusCode);

            //httpResponse.Headers.Location.Query.Replace("?id=", "")
        }

        [Fact]
        public async Task cancel_payment_returns_ok()
        {
            // add payment
            var command = new CreatePaymentCommand { Amount = 5, Date = new DateTime(2020, 3, 1) };
            var content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{PaymentsUrlBase}/payments", content);

            var id = GetIdFromLocationHeader(httpResponse);

            var cancelReason = new StringContent(JsonConvert.SerializeObject(new CancelReasonDTO { Reason = "cancel" }), Encoding.UTF8, "application/json");
            var cancelResponse = await _client.PostAsync($"{PaymentsUrlBase}/payments/{id}/cancel", cancelReason);
            cancelResponse.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, cancelResponse.StatusCode);
        }

        [Fact]
        public async Task cancel_payment_returns_notfound()
        {
            var id = "1234d07a-430e-4029-a35c-13e815bcfab4";

            var cancelReason = new StringContent(JsonConvert.SerializeObject(new CancelReasonDTO { Reason = "cancel" }), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{PaymentsUrlBase}/payments/{id}/cancel", cancelReason);

            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
        }

        [Fact]
        public async Task process_payment_returns_ok()
        {
            // add payment
            var command = new CreatePaymentCommand { Amount = 5, Date = new DateTime(2020, 3, 1) };
            var content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{PaymentsUrlBase}/payments", content);

            var id = GetIdFromLocationHeader(httpResponse);
           
            var cancelResponse = await _client.PostAsync($"{PaymentsUrlBase}/payments/{id}/process", null);
            cancelResponse.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, cancelResponse.StatusCode);
        }

        [Fact]
        public async Task process_payment_returns_notfound()
        {
            var id = "1234d07a-430e-4029-a35c-13e815bcfab4";

            var httpResponse = await _client.PostAsync($"{PaymentsUrlBase}/payments/{id}/process", null);

            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
        }

        private string GetIdFromLocationHeader(HttpResponseMessage httpResponse)
        {
            var id = httpResponse.Headers.Location.Query.Replace("?id=", "");
            return id;
        }

    
    }
}
