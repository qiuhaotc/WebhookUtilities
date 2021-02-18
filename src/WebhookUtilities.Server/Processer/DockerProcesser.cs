using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebhookUtilities.Common.Docker;

namespace WebhookUtilities.Server
{
    public static class DockerProcesser
    {
        const string Context = "Continuous integration via WebhookUtilities";

        internal static async Task<DockerResponse> Process(DockerRequest dockerRequest, string token, string scriptName, Common.WebhookConfiguration webhookConfiguration)
        {
            try
            {
                if (token == webhookConfiguration.Token && !string.IsNullOrEmpty(scriptName))
                {
                    var repoScript = Directory.GetFiles(webhookConfiguration.ScriptsFolder, "*", SearchOption.AllDirectories).Select(u => new FileInfo(u)).FirstOrDefault(u => u.Name.Contains(scriptName));

                    if (repoScript != null)
                    {
                        return await Task.FromResult(new DockerResponse
                        {
                            State = "success",
                            Description = "Finished with no error",
                            Context = Context
                        });
                    }
                    else
                    {
                        return new DockerResponse
                        {
                            State = "failed",
                            Description = "Script not exists",
                            Context = Context
                        };
                    }
                }
                else
                {
                    return new DockerResponse
                    {
                        State = "failed",
                        Description = "Wrong token or empty script name",
                        Context = Context
                    };
                }
            }
            catch (Exception ex)
            {
                return new DockerResponse
                {
                    State = "error",
                    Description = "Exception Occur:" + ex.Message,
                    Context = Context
                };
            }
        }
    }
}
