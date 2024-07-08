namespace RestApiServer.Core.EndpointService
{
    public interface IEndpointService
    {
        void ConfigureEndpoints(IEndpointRouteBuilder endpoints);
    }
}
