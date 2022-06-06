using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;

namespace Pokedex.Api.Helpers
{
    public static class PollyHelper
    {
        public static IAsyncPolicy<HttpResponseMessage> GetTransientRetryWithCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromMilliseconds(100), 5))
                .WrapAsync(GetCircuitBreakerPolicy());
        }

        /// <summary>
        /// Adds a Circuit breaker policy
        /// In a 15 second duration if there are more than 20 requests out of which more than 70 % requests fail, then do not allow further requests for 30 seconds
        /// </summary>
        /// <returns></returns>
        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .AdvancedCircuitBreakerAsync(.7, TimeSpan.FromSeconds(15), 20, TimeSpan.FromSeconds(30));
        }
    }
}
