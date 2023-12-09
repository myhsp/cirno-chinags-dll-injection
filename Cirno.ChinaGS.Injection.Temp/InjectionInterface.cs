using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GS.Unitive.Framework.Core;
using System.Drawing;

namespace Cirno.ChinaGS.Injection.Temp
{
    public class InjectionInterface
    {
        IAddonContext addonContext;
        dynamic injectionEntrance;
        bool debugging = false;

        public InjectionInterface(IAddonContext context, bool debugging = false)
        {
            this.addonContext = context;
            this.debugging = debugging;

            dynamic entrance = this.addonContext.
                GetFirstOrDefaultService("GS.Terminal.SmartBoard.Logic", 
                "GS.Terminal.SmartBoard.Logic.Core.InjectionEntranceService");

            this.injectionEntrance = entrance;
        }

        public void ShadowLantern(string msg, DateTime start, DateTime end)
        {
            this.injectionEntrance.ShadowLantern(msg, start, end);
        }

        public void ShadowLanternEx(string msg, double min)
        {
            this.ShadowLantern(msg, DateTime.Now, DateTime.Now.AddMinutes(min));
        }

        public void ShadowLanternLTP(string msg, DateTime start, DateTime end)
        {
            this.injectionEntrance.ShadowLanternLTP(msg, start, end);
        }

        public void BannerMsgCtrlAdd(string msg)
        {
            this.injectionEntrance.BannerMsgCtrlAdd(msg);
        }

        public void BannerMsgCtrlRemove(string msg)
        {
            this.injectionEntrance.BannerMsgCtrlRemove(msg);
        }

        public void IntegritedMedia(string webpath, DateTime start, DateTime end)
        {
            /// <summary>
            /// “更多” 选项卡里面的大图
            /// </summary>
            this.injectionEntrance.IntegritedMedia(webpath, start, end);
        }

        public void IntegritedMediaEx(string webpath, double min)
        {
            this.IntegritedMedia(webpath, DateTime.Now, DateTime.Now.AddMinutes(min));
        }

        public void FullScrMedia(string webpath, string mediaType, DateTime start, DateTime end)
        {
            /// <summary>
            /// 推测是视频和小图片
            /// </summary>
            this.injectionEntrance.FullScrMedia(webpath, mediaType, start, end);
        }

        public void FullScrMediaEx(string webpath, string mediaType, double min)
        {
            this.FullScrMedia(webpath, mediaType, DateTime.Now, DateTime.Now.AddMinutes(min));
        }

        public void ResetWebPath(string webpath)
        {
            this.injectionEntrance.ResetWebPath(webpath);
        }

        public void WriteLog(string msg)
        {
            if (!this.debugging)
            {
                return;
            }
            try
            {
                this.injectionEntrance.WriteLog(msg);
            }
            catch (Exception ex)
            {
                this.addonContext.Logger.Error("[CGSI Assembly] Fail to write log!!" + msg, ex);
            }
        }

        public void ShowPrompt(string msg)
        {
            this.injectionEntrance.ShowPrompt(msg);
        }

        public Bitmap GetCurrentVideoFrame()
        {
            return this.injectionEntrance.GetCurrentVideoFrame();
        }

        public string GetScreenCapture(string filename)
        {
            return this.injectionEntrance.GetScreenCapture(filename);
        }

        public Guid AddPosterTemplate(string imageUri)
        {
            return this.injectionEntrance.AddPosterTemplate(imageUri);
        }

        public void RemovePosterTemplate(Guid guid)
        {
            this.injectionEntrance.RemovePosterTemplate(guid);
        }

        public void AddMultiMediaVisualTemplate(string media_json_filename)
        {
            this.injectionEntrance.AddMultiMediaVisualTemplate(media_json_filename);
        }

        [Obsolete]
        public void RemoveVisualTemplate(string template_name, bool ignore_first = true)
        {
            // 不建议使用
            // 已经改为基于链表的替换而非删除。
            this.injectionEntrance.RemoveVisualTemplate(template_name, ignore_first);
        }

        public string GetCachePath()
        {
            return this.injectionEntrance.GetCachePath();
        }

        public void ClearAllPosterTemplate()
        {
            this.injectionEntrance.ClearAllPosterTemplate();
        }
    }
}
