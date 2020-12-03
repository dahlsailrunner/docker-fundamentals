using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using Serilog;

namespace Globomantics.Core.Services
{
    public static class PollyHelpers
    {
        public static IHttpClientBuilder AddResiliencePolicies(this IHttpClientBuilder httpClientBuilder)
        {
            var timesToRetry = 10;
            var secondsToWaitForSingleResponse = 10;
            // this will handle transient errors (500 errros, 408 and httprequestexceptions) by delaying
            // and then retrying based on the array below

            var jitterer = new Random();
            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(
                    retryCount: timesToRetry,
                    sleepDurationProvider: retryAttempt =>
                        (TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) // exponential back-off: 2, 4, 8 etc
                         + TimeSpan.FromMilliseconds(jitterer.Next(0, 1000))),
                    onRetry: (result, timeSpan, retryAttempt, ctx) =>
                    {
                        Log
                            .ForContext("RequestUri", result.Result.RequestMessage.RequestUri)
                            .ForContext("ResponseCode", result.Result.StatusCode)
                            .Information(result.Exception, $"Retrying HTTP call -- ({retryAttempt} of {timesToRetry})");
                    }); // plus some jitter: up to 1 second);

            // This will kill / fail an api call if it takes longer than specified seconds to receive response
            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(secondsToWaitForSingleResponse);

            return httpClientBuilder
                .AddPolicyHandler(retryPolicy)
                .AddPolicyHandler(timeoutPolicy);
        }
    }
}
