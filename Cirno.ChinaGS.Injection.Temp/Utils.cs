using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.IO;
using System.Diagnostics;
using System.Net;

using GS.Unitive.Framework.Core;
using GS.Unitive.Framework.Persistent;


namespace Cirno.ChinaGS.Injection.Temp
{

    public class Utils
    {
        public static string GetMachineCurrentStatus(IAddonContext context)
        {
            ///<summary>
            /// 获得机器当前状态
            ///<returns></returns>
            ///</summary>
            dynamic logicService = context.GetFirstOrDefaultService("GS.Terminal.SmartBoard.Logic",
                "GS.Terminal.SmartBoard.Logic.Core.Service");
            string status = logicService.GetState();

            return status;
        }

        public static string GetTerminalCode(IAddonContext context)
        {
            return context.IntercativeData("TerminalCode");
        }

        public static string GetMachineWebPath(IAddonContext context)
        {
            /// <summary>
            /// 获得 webpath（不知道是什么）
            /// </summary>
            IAddonContext logicContext = AddonRuntime.Instance.GetInstalledAddons()
                .FirstOrDefault((IAddon ss) => ss.SymbolicName == "GS.Terminal.SmartBoard.Logic").Context;

            bool success = false;
            string webPath = logicContext.GlobalSetting("WebPath", ref success);
            if (!success)
            {
                context.Logger.Debug("[CirnoInjection] Method Utils.GetMachineWebPath can't fetch attribute WebPath through GlobalSetting - returned empty string.");
            }
            else
            {
                context.Logger.Debug("[CirnoInjection] Method Utils.GetMachineWebPath is called and fetched attribute WebPath! Fuck GS!!");
            }

            return webPath;
        }

        [Obsolete]
        public static string GetMachineMacAddr(IAddonContext context)
        {
            ///<summary>
            /// 未实现
            /// </summary>
            return "";
        }

        public static string GetMachineID(IAddonContext context)
        {
            ///<summary>
            /// 获得当前机器 ID
            ///<returns></returns>
            ///</summary>
            IAddonContext logicContext = AddonRuntime.Instance.GetInstalledAddons()
                .FirstOrDefault((IAddon ss) => ss.SymbolicName == "GS.Terminal.SmartBoard.Logic").Context;

            bool success = false;
            string machineId = logicContext.GlobalSetting("tCode", ref success);
            if (!success)
            {
                context.Logger.Debug("[CirnoInjection] Method Utils.GetMachineID can't fetch attribute tCode through GlobalSetting - returned empty string.");
            }
            else
            {
                context.Logger.Debug("[CirnoInjection] Method Utils.GetMachineID is called and fetched attribute tCode! Fuck GS!!");
            }

            return machineId;
        }

        public static string AddGarnitureControl(IAddonContext context, UserControl control, double left, double top)
        {
            ///<summary>
            /// 添加用户控件
            ///<returns></returns>
            ///</summary>
            dynamic uiService = context.GetFirstOrDefaultService("GS.Terminal.MainShell",
                "GS.Terminal.MainShell.Services.UIService");

            string guid = uiService.AddGarnitureControl(control, top, left);
            return guid;
        }

        public static void CreateTimelineTask(IAddonContext context, DateTime StartTime, DateTime EndTime, 
            int Lvl, bool AllowParallel,
            Action<string, string> OnStart, Action<string, string> OnPause, 
            Action<string, string> OnRestart, Action<string, string> OnStop, 
            Action<string, string> OnTaskStateChanged, 
            Action<string, string> OnTaskCreated, 
            string taskname)
        {
            ///<summary>
            /// 创建时间线任务
            ///<returns></returns>
            ///</summary>
            dynamic timelineService = context.GetFirstOrDefaultService("GS.Terminal.TimeLine", "GS.Terminal.TimeLine.Service");
            timelineService.CreateTimeLineTask(StartTime, EndTime, Lvl, AllowParallel, OnStart, OnPause, OnRestart, OnStop, OnTaskStateChanged, OnTaskCreated, taskname);;
        }

        public static void CreateTimelineTask(IAddonContext context, DateTime StartTime, DateTime EndTime, int Lvl, Action<string, string> OnStart, Action<string, string> OnStop, string taskname)
        {
            ///<summary>
            /// 创建时间线任务
            ///<returns></returns>
            ///</summary>
            CreateTimelineTask(context, StartTime, EndTime, Lvl, true, OnStart, null, null, OnStop, null, null, taskname);
        }

        public static void CreateTask(IAddonContext context, DateTime ExecuteTime, Action<string> TaskAction, AsyncCallback Callback, string taskname)
        {
            ///<summary>
            /// 创建定时任务
            ///<returns></returns>
            ///</summary>
            dynamic timelineService = context.GetFirstOrDefaultService("GS.Terminal.TimeLine", "GS.Terminal.TimeLine.Service");
            timelineService.CreateTask(ExecuteTime, TaskAction, Callback, taskname);
        }

        public static void ShowPopup(IAddonContext context, string msg)
        {
            dynamic uiAddon = context.
                GetFirstOrDefaultService("GS.Terminal.MainShell", "GS.Terminal.MainShell.Services.UIService");

            uiAddon.ShowPrompt(msg, 3);
        }

        public static void CreateCycleTask(IAddonContext context, DateTime StartTime, int interval, int CycleCount, Action<string> TaskAction, AsyncCallback Callback, string taskname)
        {
            ///<summary>
            /// 创建循环任务
            ///<returns></returns>
            ///</summary>
            dynamic timelineService = context.GetFirstOrDefaultService("GS.Terminal.TimeLine", "GS.Terminal.TimeLine.Service");
            timelineService.CreateCycleTask(StartTime, interval, CycleCount, TaskAction, Callback, taskname);
        }

        public static IObjectSpace GetLocalObjectSpace(IAddonContext context)
        {
            ///<summary>
            /// 获得当前数据空间会话（用于添加时间线任务等）
            ///<returns></returns>
            ///</summary>
            IAddon addon = AddonRuntime.Instance.GetInstalledAddons().FirstOrDefault((IAddon ss) => ss.SymbolicName == "GS.Terminal.SmartBoard.Logic");

            IAddonContext context2 = addon.Context;
            ObjectSpaceManager objectSpaceManager = new ObjectSpaceManager(ChannelMode.Multiton);
            IObjectSpace objectSpace = objectSpaceManager.CreateObjectSpace(context2, "sqlite", 0);

            return objectSpace;
        }

        public static void RefreshTask(IAddonContext context)
        {
            dynamic logicService = context.GetFirstOrDefaultService("GS.Terminal.SmartBoard.Logic",
                "GS.Terminal.SmartBoard.Logic.Core.Service");

            logicService.UpdateMedia();
            logicService.UpdateVisual();
        }

        /*
        [Obsolete]
        public static void DisplayShadowLanternOld(IAddonContext context, DateTime StartTime, DateTime EndTime, string msg)
        {
            ///<summary>
            /// 播放跑马灯 此实现方法错误，已经弃用！！
            ///<returns></returns>
            ///</summary>
            IAddonContext logicContext = AddonRuntime.Instance.GetInstalledAddons()
                .FirstOrDefault((IAddon ss) => ss.SymbolicName == "GS.Terminal.SmartBoard.Logic").Context;

            dynamic bannerMsgCtrl = logicContext.LoadClassInstance("BannerMessageControl", "GS.Terminal.SmartBoard.Logic.Garitures");

            CreateTimelineTask(context, StartTime, EndTime, 1,
                new Action<string, string>(delegate (string s, string id)
                {
                    bannerMsgCtrl.AddBannerMsg(msg);
                }),
                new Action<string, string>(delegate (string s, string id) {
                    bannerMsgCtrl.RemoveBannerMsg(msg);
                }),
                Guid.NewGuid().ToString()
            );
        }

        [Obsolete]
        public static Guid DisplayFullScreenMedia(IAddonContext context, 
            DateTime StartTime, DateTime EndTime, string imageUrl)
        {
            ///<summary>
            /// 显示全屏媒体（注意：实现方法过于粗糙，可能出现问题）
            ///<returns></returns>
            ///</summary>
            Guid guid = Guid.Empty;
            //GS.Terminal.SmartBoard.Logic.Core
            IAddon logicAddon = AddonRuntime.Instance.GetInstalledAddons()
                .FirstOrDefault((IAddon ss) => ss.SymbolicName == "GS.Terminal.SmartBoard.Logic");

            dynamic mainWindowCore = logicAddon.Context
                .LoadClassInstance("GS.Terminal.SmartBoard.Logic", "MainWindowCore", "GS.Terminal.SmartBoard.Logic.Core");
            
            if (imageUrl.StartsWith("http://") || imageUrl.StartsWith("https://"))
            {
                guid = mainWindowCore.AddPosterTemplate(imageUrl);
            }
            else
            {
                guid = mainWindowCore.AddPosterTemplate(GetMachineWebPath(context) + "/" + imageUrl);
            }
            
            Session sess = GetLocalObjectSpace(context).Session;
            return guid;
        }

        public static void DisplayShadowLantern(Session objectSpaceSession, 
            DateTime StartTime, DateTime EndTime, string msg)
        {
            /// <summary>
            /// 显示走马灯
            /// </summary>
            if (!objectSpaceSession.InTransaction)
            {
                objectSpaceSession.BeginTransaction();
            }
            LocalTaskPlan taskPlan = LocalTaskPlanBuilder(objectSpaceSession, TaskPlanType.计划播,
                TaskPlanExecuteType.ShadowLantern, StartTime, EndTime, 1,
                "[Cirno](DisplayShadowLantern)" + Guid.NewGuid().ToString(), msg);
            taskPlan.Save();
        }

        public static void DisplayFullScreenMediaImage(Session objectSpaceSession, 
            DateTime StartTime, DateTime EndTime, string contentUrl)
        {
            ///<summary>
            ///显示全屏媒体（图像）
            ///注意！！contentUrl 系文件相对于 webpath 的路径。
            ///<returns></returns>
            ///</summary>
            if (!objectSpaceSession.InTransaction)
            {
                objectSpaceSession.BeginTransaction();
            }
            LocalTaskPlan taskPlan = LocalTaskPlanBuilder(objectSpaceSession, TaskPlanType.持续播,
                TaskPlanExecuteType.PlayMediaImage, StartTime, EndTime, 99,
                "[Cirno](DisplayFullScreenMediaImage)" + Guid.NewGuid().ToString(), contentUrl);
            taskPlan.Save();
        }

        public static void DisplayFullScreenMediaVideo(Session objectSpaceSession,
            DateTime StartTime, DateTime EndTime, string contentUrl)
        {
            ///<summary>
            ///显示全屏媒体（视频）
            ///注意！！contentUrl 系文件相对于 webpath 的路径。
            ///<returns></returns>
            ///</summary>
            if (!objectSpaceSession.InTransaction)
            {
                objectSpaceSession.BeginTransaction();
            }
            LocalTaskPlan taskPlan = LocalTaskPlanBuilder(objectSpaceSession, TaskPlanType.持续播,
                TaskPlanExecuteType.PlayMediaVideo, StartTime, EndTime, 3,
                "[Cirno](DisplayFullScreenMediaVideo)" + Guid.NewGuid().ToString(), contentUrl);
            taskPlan.Save();
        }

        public static void DisplayIntegritedMedia(Session objectSpaceSession,
            DateTime StartTime, DateTime EndTime, string contentUrl)
        {
            ///<summary>
            /// 显示内嵌媒体（注意：实现方法过于粗糙，可能出现问题）
            /// 注意！！url 建议使用 http://* 开头的远程（学校内网）图像 url，否则默认为 webpath！！
            /// 鉴于目前对 webpath 的理解不够，尽量不要使用 webpath！！
            /// 图像之间以 , （半角逗号）分割
            ///<returns></returns>
            ///</summary>
            ///
            if (!objectSpaceSession.InTransaction)
            {
                objectSpaceSession.BeginTransaction();
            }
            LocalTaskPlan taskPlan = LocalTaskPlanBuilder(objectSpaceSession, TaskPlanType.计划播,
                TaskPlanExecuteType.Poster, StartTime, EndTime, 100,
                "[Cirno](DisplayIntegritedMedia)" + Guid.NewGuid().ToString(), contentUrl);
            taskPlan.Save();
        }
        */
        public static void SetTheme(IAddonContext context, string name)
        {
            /// <summary>
            /// 设置主题
            /// </summary>
            dynamic themeAddon = context.
                GetFirstOrDefaultService("GS.Terminal.Theme", "GS.Terminal.Theme.Service");
            themeAddon.SetTheme(name);
        }

        public static void StartProcess(string process, string cache)
        {
            if (!(process.EndsWith(".exe") || process.EndsWith(".bat") || process.EndsWith(".cmd")))
            {
                process = process + ".exe";
            }
            if (!Directory.Exists(Path.Combine(cache, "utils")))
            {
                Directory.CreateDirectory(Path.Combine(cache, "utils"));
            }
            if (!File.Exists(process))
            {
                if (File.Exists(Path.Combine(cache, "utils", process)))
                {
                    process = Path.Combine(cache, "utils", process);
                }
                else
                {
                    return;
                }
            }

            try
            {
                Process.Start(process);
            }
            catch
            {
            }
        }

        public static void WriteJson(string filename, string json_b64, string cache)
        {
            if (!filename.EndsWith(".json"))
            {
                filename = filename + ".json";
            }
            if (!Directory.Exists(Path.Combine(cache, "BlockCache")))
            {
                Directory.CreateDirectory(Path.Combine(cache, "BlockCache"));
            }
            byte[] byteB64 = Convert.FromBase64String(json_b64);
            string content = Encoding.UTF8.GetString(byteB64);

            File.WriteAllText(Path.Combine(cache, "BlockCache", filename), content, Encoding.UTF8);
        }

        public static void DownloadFile(string url, string savename, string cache)
        {
            string savepath = cache;

            if (savename.EndsWith(".png") || savename.EndsWith(".jpg") || savename.EndsWith(".bmp"))
            {
                savepath = Path.Combine(cache, "image", savename);
            }
            else if (savename.EndsWith(".flv") || savename.EndsWith(".mp4"))
            {
                savepath = Path.Combine(cache, "video", savename);
            }
            else
            {
                savepath = Path.Combine(savepath, "utils");
                if (!Directory.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath);
                }
                savepath = Path.Combine(savepath, savename);
            }

            using (WebClient web = new WebClient())
            {
                try
                {
                    web.DownloadFileAsync(new Uri(url), savepath);
                }
                catch (Exception)
                {
                }
            }
            
        }
        /*

        public static LocalTaskPlan LocalTaskPlanBuilder(Session objectSpaceSession, TaskPlanType taskPlanType, TaskPlanExecuteType taskPlanExecType, DateTime start, DateTime end,
            int level, string name, string target)
        {
            return new LocalTaskPlan(objectSpaceSession)
            {
                TaskType = taskPlanType.ToString(),
                ExecuteType = taskPlanExecType.ToString().ToUpper(),
                ExecStartDateTime = start,
                ExecEndDateTime = end,
                InfoLevel = level,
                AllowParallel = true,
                InfoName = name,
                Target = target,
                InfoID = Guid.NewGuid().ToString()
            };
        }

        public enum TaskPlanExecuteType
        {
            PlayMediaVideo = 0x00,
            PlayMediaImage = 0x01,
            Examination = 0x02,
            ShadowLantern = 0x03,
            Poster = 0x04
        }

        public enum TaskPlanType
        {
            计划播 = 0x00,
            立即播 = 0x01,
            持续播 = 0x02 // 傻逼通软
        }*/
    }
}
