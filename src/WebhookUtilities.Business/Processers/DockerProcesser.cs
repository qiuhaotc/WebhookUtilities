using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WebhookUtilities.Common;
using WebhookUtilities.Common.Docker;

namespace WebhookUtilities.Business
{
    public class DockerProcesser
    {
        const string Context = "Docker Continuous integration via WebhookUtilities";

        public ILogger Logger { get; }

        public DockerProcesser(ILogger logger)
        {
            Logger = logger;
        }

        internal async Task<DockerResponse> Process(WebhookRequest<DockerRequestWrapper, DockerResponse> dockerWebhookRequest, WebhookConfiguration webhookConfiguration)
        {
            try
            {
                if (dockerWebhookRequest.Token == webhookConfiguration.Token && !string.IsNullOrEmpty(dockerWebhookRequest.Request.ScriptName))
                {
                    var repoScript = Directory.GetFiles(webhookConfiguration.ScriptsFolder, "*", SearchOption.AllDirectories).Select(u => new FileInfo(u)).FirstOrDefault(u => u.Name.Contains(dockerWebhookRequest.Request.ScriptName));

                    if (repoScript != null)
                    {
                        return await RunScript(repoScript);
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

        async Task<DockerResponse> RunScript(FileInfo repoScript)
        {
            return await RunCommand(GetProcessStartInfo(repoScript));
        }

        ProcessStartInfo GetProcessStartInfo(FileInfo scriptFileInfo)
        {
            var argsPrepend = string.Empty;
            var shellName = "/bin/bash";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                shellName = "cmd";
                argsPrepend = "/c ";
            }

            return new ProcessStartInfo
            {
                FileName = shellName,
                Arguments = argsPrepend + scriptFileInfo.Name,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                WorkingDirectory = scriptFileInfo.DirectoryName
            };
        }

        async Task<DockerResponse> RunCommand(ProcessStartInfo processInfo)
        {
            return await Task.Run(() =>
            {
                Logger.LogDebug($"Run Script {processInfo.Arguments.Substring(processInfo.FileName == "cmd" ? 3 : 0)}");

                var process = new Process()
                {
                    StartInfo = processInfo,
                };

                process.OutputDataReceived += (sender, e) => { if (e.Data != null) Logger.LogDebug(e.Data); };
                process.ErrorDataReceived += (sender, e) => { if (e.Data != null) Logger.LogWarning(e.Data); };

                process.Start();

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    Logger.LogInformation($"Run Script {processInfo.Arguments.Substring(processInfo.FileName == "cmd" ? 3 : 0)} Successful");

                    return new DockerResponse
                    {
                        State = "success",
                        Description = "Finished with no error",
                        Context = Context
                    };
                }

                Logger.LogWarning($"Run Script {processInfo.Arguments.Substring(processInfo.FileName == "cmd" ? 3 : 0)} Failed With Exit Code: {process.ExitCode}");

                return new DockerResponse
                {
                    State = "failed",
                    Description = "Failed run script, exit code: " + process.ExitCode,
                    Context = Context
                };
            });
        }
    }
}
