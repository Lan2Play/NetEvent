using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetEvent.Server.Models;
using RazorLight;

namespace NetEvent.Server.Services
{
    public class RazorEmailRenderer : IEmailRenderer
    {
        private readonly ILogger<RazorEmailRenderer> _Logger;
        private readonly RazorLightEngine _Engine;

        public RazorEmailRenderer(ILogger<RazorEmailRenderer> logger)
        {
            _Engine = new RazorLightEngineBuilder()
              .UseMemoryCachingProvider()
              .Build();
            _Logger = logger;
        }

        public async Task<string?> RenderAsync(string templatKey, string template, NetEventEmailRenderModel model, CancellationToken cancellationToken)
        {
            try
            {
                string result = await _Engine.CompileRenderStringAsync(templatKey, template, model);

                return result;
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Template couldn't be rendered.");

                return null;
            }
        }
    }
}
