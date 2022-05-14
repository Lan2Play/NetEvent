using System.Collections.Generic;

namespace NetEvent.Server.Models
{
    public class NetEventEmailRenderModel
    {
        public NetEventEmailRenderModel(IDictionary<string, string> templateVariables)
        {
            TemplateVariables = templateVariables;
        }

        public IDictionary<string, string> TemplateVariables { get; }
    }
}
