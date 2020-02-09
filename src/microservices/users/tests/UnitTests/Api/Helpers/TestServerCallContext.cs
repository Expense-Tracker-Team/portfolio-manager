namespace UnitTests.Api.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Grpc.Core;

    public class TestServerCallContext : ServerCallContext
    {
        private readonly Metadata requestHeaders;
        private readonly CancellationToken cancellationToken;
        private readonly Metadata responseTrailers;
        private readonly AuthContext authContext;
        private WriteOptions? writeOptions;

        public Metadata? ResponseHeaders { get; private set; }

        private TestServerCallContext(Metadata requestHeaders, CancellationToken cancellationToken)
        {
            this.requestHeaders = requestHeaders;
            this.cancellationToken = cancellationToken;
            this.responseTrailers = new Metadata();
            this.authContext = new AuthContext(string.Empty, new Dictionary<string, List<AuthProperty>>());
        }

        protected override string MethodCore => "MethodName";

        protected override string HostCore => "HostName";

        protected override string PeerCore => "PeerName";

        protected override DateTime DeadlineCore { get; }

        protected override Metadata RequestHeadersCore => this.requestHeaders;

        protected override CancellationToken CancellationTokenCore => this.cancellationToken;

        protected override Metadata ResponseTrailersCore => this.responseTrailers;

        protected override Status StatusCore { get; set; }

        protected override WriteOptions? WriteOptionsCore { get => this.writeOptions; set => this.writeOptions = value; }

        protected override AuthContext AuthContextCore => this.authContext;

        protected override ContextPropagationToken CreatePropagationTokenCore(ContextPropagationOptions options) => throw new NotImplementedException();

        protected override Task WriteResponseHeadersAsyncCore(Metadata responseHeaders)
        {
            if (this.ResponseHeaders != null)
            {
                throw new InvalidOperationException("Response headers have already been written.");
            }

            this.ResponseHeaders = responseHeaders;
            return Task.CompletedTask;
        }

        public static TestServerCallContext Create(Metadata? requestHeaders = null, CancellationToken cancellationToken = default) => new TestServerCallContext(requestHeaders ?? new Metadata(), cancellationToken);
    }
}