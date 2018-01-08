using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate
{
    /// <summary>
    /// A service that can be used with ASP.NET Core dependency injection.
    /// Usage: In ConfigureServices():
    /// <code>
    /// services.Configure&lt;HiP_MicroServiceTemplateConfig&gt;(Configuration.GetSection("Endpoints"));
    /// services.AddSingleton&lt;HiP_MicroServiceTemplateService&gt;();
    /// </code>
    /// </summary>
    public class HiP_MicroServiceTemplateService
    {
        private readonly HiP_MicroServiceTemplateConfig _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HiP_MicroServiceTemplateService(IOptions<HiP_MicroServiceTemplateConfig> config, ILogger<HiP_MicroServiceTemplateService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _config = config.Value;
            _httpContextAccessor = httpContextAccessor;

            if (string.IsNullOrWhiteSpace(config.Value.HiP_MicroServiceTemplateHost))
                logger.LogWarning($"{nameof(HiP_MicroServiceTemplateConfig.HiP_MicroServiceTemplateHost)} is not configured correctly!");
        }

        public FooClient Foo => new FooClient(_config.HiP_MicroServiceTemplateHost)
        {
            Authorization = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
        };
    }
}
