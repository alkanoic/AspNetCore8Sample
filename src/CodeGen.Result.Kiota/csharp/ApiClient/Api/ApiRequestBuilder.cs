// <auto-generated/>
using ApiSdk.Api.MultiTableApi;
using ApiSdk.Api.SampleTableApi;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
namespace ApiSdk.Api {
    /// <summary>
    /// Builds and executes requests for operations under \api
    /// </summary>
    public class ApiRequestBuilder : BaseRequestBuilder {
        /// <summary>The MultiTableApi property</summary>
        public MultiTableApiRequestBuilder MultiTableApi { get =>
            new MultiTableApiRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The SampleTableApi property</summary>
        public SampleTableApiRequestBuilder SampleTableApi { get =>
            new SampleTableApiRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new ApiRequestBuilder and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public ApiRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/api", pathParameters) {
        }
        /// <summary>
        /// Instantiates a new ApiRequestBuilder and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public ApiRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/api", rawUrl) {
        }
    }
}