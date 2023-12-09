/*
GS.Terminal.SmartBoard.Logic.Core.InjectionEntranceService.cs

Provides the essential methods for Cirno.ChinaGS.* complex.
This class should be put under namspace GS.Terminal.SmartBoard.Logic.Core
and compiled using dnSpy.

By konata233 on Dec. 09 2023
Fuck GS!!
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using GalaSoft.MvvmLight.Threading;
using GS.Terminal.SmartBoard.LocalDB;
using GS.Terminal.SmartBoard.Logic.Garitures;
using GS.Terminal.SmartBoard.Logic.Models;
using GS.Terminal.SmartBoard.Logic.TimeLineAction;
using IVisualBlock;
using SmartBoardViewModels.Models.VisualBlock;

namespace GS.Terminal.SmartBoard.Logic.Core
{
	// Token: 0x02000174 RID: 372
	public class InjectionEntranceService
	{
		// Token: 0x0600074F RID: 1871 RVA: 0x0000221F File Offset: 0x0000041F
		public InjectionEntranceService()
		{
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x0001F9A0 File Offset: 0x0001DBA0
		public void ShadowLantern(string msg, DateTime start, DateTime end)
		{
			Utilites.CreateTimeLineTask(start, end, 1, true, delegate(string s, string id)
			{
				BannerMessageControl.AddBannerMsg(msg);
			}, null, null, delegate(string s, string id)
			{
				BannerMessageControl.RemoveBannerMsg(msg);
			}, null, null, "Cirno::SLantern::" + Guid.NewGuid().ToString());
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0001FA00 File Offset: 0x0001DC00
		public void ShadowLanternEx(string msg, double min)
		{
			DateTime now = DateTime.Now;
			DateTime endTime = DateTime.Now.AddSeconds(min);
			Utilites.CreateTimeLineTask(now, endTime, 1, true, delegate(string s, string id)
			{
				BannerMessageControl.AddBannerMsg(msg);
			}, null, null, delegate(string s, string id)
			{
				BannerMessageControl.RemoveBannerMsg(msg);
			}, null, null, "Cirno::SLantern::" + Guid.NewGuid().ToString());
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0001FA70 File Offset: 0x0001DC70
		public void IntegritedMedia(string webpath, DateTime start, DateTime end)
		{
			string text = "Cirno::Integrited::" + Guid.NewGuid().ToString();
			LocalTaskPlan localTaskPlan = new LocalTaskPlan(Program.LocalObjectSpace().Session)
			{
				ExecuteType = "POSTER",
				ExecStartDateTime = start,
				ExecEndDateTime = end,
				InfoID = Guid.NewGuid().ToString(),
				InfoLevel = 100,
				AllowParallel = true,
				InfoName = text,
				Target = webpath
			};
			ITimeLineActions @object = ActionFactory.FindAction("POSTER", localTaskPlan);
			LogicResources.TimeLineTasks.Add(new TimeLineTask
			{
				StartTime = localTaskPlan.ExecStartDateTime,
				EndTime = localTaskPlan.ExecEndDateTime,
				Lvl = localTaskPlan.InfoLevel,
				OnCreated = new Action<string, string>(@object.OnCreated),
				OnPause = new Action<string, string>(@object.OnPause),
				OnRestart = new Action<string, string>(@object.OnRestart),
				OnStart = new Action<string, string>(@object.OnStart),
				OnStateChanged = new Action<string, string>(@object.OnTaskStateChanged),
				OnStop = new Action<string, string>(@object.OnStop),
				SystemID = localTaskPlan.InfoID
			}.AddToService(text, true));
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0001FBC0 File Offset: 0x0001DDC0
		public void IntegritedMediaEx(string webpath, double min)
		{
			this.IntegritedMedia(webpath, DateTime.Now, DateTime.Now.AddMinutes(min));
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0001FBE8 File Offset: 0x0001DDE8
		public void FullScrMedia(string webpath, string mediaType, DateTime start, DateTime end)
		{
			string text = "Cirno::Integrited::" + Guid.NewGuid().ToString();
			mediaType = mediaType.ToUpper();
			if (mediaType != "VIDEO" || mediaType != "IMAGE")
			{
				mediaType = "IMAGE";
			}
			LocalTaskPlan localTaskPlan = new LocalTaskPlan(Program.LocalObjectSpace().Session)
			{
				ExecuteType = "PLAYMEDIA." + mediaType,
				ExecStartDateTime = start,
				ExecEndDateTime = end,
				InfoID = Guid.NewGuid().ToString(),
				InfoLevel = 3,
				InfoName = text,
				Target = webpath
			};
			ITimeLineActions @object = ActionFactory.FindAction("PLAYMEDIA", localTaskPlan);
			LogicResources.TimeLineTasks.Add(new TimeLineTask
			{
				StartTime = localTaskPlan.ExecStartDateTime,
				EndTime = localTaskPlan.ExecEndDateTime,
				Lvl = localTaskPlan.InfoLevel,
				OnCreated = new Action<string, string>(@object.OnCreated),
				OnPause = new Action<string, string>(@object.OnPause),
				OnRestart = new Action<string, string>(@object.OnRestart),
				OnStart = new Action<string, string>(@object.OnStart),
				OnStateChanged = new Action<string, string>(@object.OnTaskStateChanged),
				OnStop = new Action<string, string>(@object.OnStop),
				SystemID = localTaskPlan.InfoID
			}.AddToService(text, true));
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0001FD60 File Offset: 0x0001DF60
		public void FullScrMediaEx(string webpath, string mediaType, double min)
		{
			this.FullScrMedia(webpath, mediaType, DateTime.Now, DateTime.Now.AddMinutes(min));
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x00005CC5 File Offset: 0x00003EC5
		public void BannerMsgCtrlAdd(string msg)
		{
			BannerMessageControl.AddBannerMsg(msg);
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x00005CCD File Offset: 0x00003ECD
		public void BannerMsgCtrlRemove(string msg)
		{
			BannerMessageControl.RemoveBannerMsg(msg);
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0001FD88 File Offset: 0x0001DF88
		public void ShadowLanternLTP(string msg, DateTime start, DateTime end)
		{
			string text = "Cirno::SLantern::" + Guid.NewGuid().ToString();
			LocalTaskPlan localTaskPlan = new LocalTaskPlan(Program.LocalObjectSpace().Session)
			{
				ExecuteType = "SHADOWLANTERN",
				ExecStartDateTime = start,
				ExecEndDateTime = end,
				InfoID = Guid.NewGuid().ToString(),
				InfoLevel = 1,
				AllowParallel = true,
				InfoName = text,
				Target = msg
			};
			ITimeLineActions @object = ActionFactory.FindAction("SHADOWLANTERN", localTaskPlan);
			LogicResources.TimeLineTasks.Add(new TimeLineTask
			{
				StartTime = localTaskPlan.ExecStartDateTime,
				EndTime = localTaskPlan.ExecEndDateTime,
				Lvl = localTaskPlan.InfoLevel,
				OnCreated = new Action<string, string>(@object.OnCreated),
				OnPause = new Action<string, string>(@object.OnPause),
				OnRestart = new Action<string, string>(@object.OnRestart),
				OnStart = new Action<string, string>(@object.OnStart),
				OnStateChanged = new Action<string, string>(@object.OnTaskStateChanged),
				OnStop = new Action<string, string>(@object.OnStop),
				SystemID = localTaskPlan.InfoID
			}.AddToService(text, true));
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x00005CD5 File Offset: 0x00003ED5
		public void ResetWebPath(string webpath)
		{
			Program.WebPath = webpath;
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x00005CDD File Offset: 0x00003EDD
		public void WriteLog(string msg)
		{
			LogicCore.WriteLog(new object[]
			{
				msg
			});
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x00005CEE File Offset: 0x00003EEE
		public void ShowPrompt(string msg)
		{
			Utilites.ShowPrompt(msg);
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x00005CF6 File Offset: 0x00003EF6
		public Bitmap GetCurrentVideoFrame()
		{
			return FaceRecognizationService.GetCurrentVideoFrame();
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x0001FED8 File Offset: 0x0001E0D8
		public void AddMultiMediaVisualTemplate(string media_json_filename)
		{
			int count = Utilites.ViewModelLocator.MainPage.TemplateList.Count;
			IBlockService firstOrDefaultService = Program.AddonContext.GetFirstOrDefaultService<IBlockService>("GS.Terminal.VisualBlock");
			BlockTemplate blockTemplate = new BlockTemplate();
			blockTemplate.TemplateName = "ClassTemplateStyle";
			blockTemplate.DisplayName = "校园风采";
			blockTemplate.Index = 2;
			blockTemplate.TemplateType = BlockTemplateType.Theme;
			BaseBlock block = firstOrDefaultService.GetBlock("ClassMultiMedia");
			block.Init(Program.AddonContext);
			IUpdate update = (IUpdate)block;
			if (!File.Exists(media_json_filename))
			{
				media_json_filename = Path.Combine(Program.CachePath, "BlockCache", media_json_filename);
			}
			update.LoadLocalData(media_json_filename);
			blockTemplate.Blocks = new List<VisualBlockItem>();
			blockTemplate.Blocks.Add(new VisualBlockItem
			{
				Id = Guid.NewGuid(),
				BlockComponent = "班级风采",
				BlockTypeName = ((IBlock)block).TypeName,
				DataSource = "Services/SmartBoard/BlockClassMultiMedia/json",
				Height = 10,
				NavTemplateName = "",
				Width = 20,
				X = 1,
				Y = 1,
				DataContext = (BaseBlock)update
			});
			blockTemplate.Previous = Utilites.ViewModelLocator.MainPage.TemplateList[1];
			blockTemplate.Next = Utilites.ViewModelLocator.MainPage.TemplateList[3];
			DispatcherHelper.RunAsync(delegate
			{
				Utilites.ViewModelLocator.MainPage.TemplateList[2] = blockTemplate;
			});
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x00020070 File Offset: 0x0001E270
		public void RemoveVisualTemplate(string template_name, bool ignore_first = true)
		{
			BlockTemplate blockTemplate = Utilites.ViewModelLocator.MainPage.TemplateList.FirstOrDefault((BlockTemplate ss) => ss.TemplateName == template_name);
			if (blockTemplate != Utilites.ViewModelLocator.MainPage.TemplateList.First<BlockTemplate>() || !ignore_first)
			{
				Utilites.ViewModelLocator.MainPage.RemoveTemplate(blockTemplate);
			}
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x00005CFD File Offset: 0x00003EFD
		public Guid AddPosterTemplate(string imgUri)
		{
			return MainWindowCore.AddPosterTemplate(imgUri);
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x00005D05 File Offset: 0x00003F05
		public void RemovePosterTemplate(Guid guid)
		{
			MainWindowCore.RemotePosterTemplate(guid);
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x00005D0D File Offset: 0x00003F0D
		public string GetCachePath()
		{
			return Program.CachePath;
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x000200D8 File Offset: 0x0001E2D8
		public void ClearAllPosterTemplate()
		{
			foreach (BlockTemplate blockTemplate in Utilites.ViewModelLocator.MainPage.TemplateList.ToList<BlockTemplate>())
			{
				if (blockTemplate.TemplateType == BlockTemplateType.Media)
				{
					Utilites.ViewModelLocator.MainPage.RemoveTemplate(blockTemplate);
				}
			}
		}
	}
}
