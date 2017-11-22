using BaseLib.Elements;
using BaseLib.UI;
using BaseLib.Utility;
using DSharpPlus.Entities;
using DTT.UI.Elements;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace DTT.UI
{
	public class SelectUI : BaseUI
	{
		public UIElement screen = new UIElement();

		public UIUser avatarUser = new UIUser(null);

		public UIRoundImage buttonGuilds = new UIRoundImage(DTT.defaultIcon);
		public UIText textServer = new UIText("");

		public UIReverseGrid gridGuilds = new UIReverseGrid();
		public UIScrollbar barGuilds = new UIScrollbar();

		public UIReverseGrid gridChannels = new UIReverseGrid();
		public UIScrollbar barChannels = new UIScrollbar();

		public UIPanel panelMessages = new UIPanel();
		public UIChat gridMessages = new UIChat();
		public UIScrollbarReversed barMessages = new UIScrollbarReversed();
		public UIPanel panelInputMessages = new UIPanel();
		public UIInput inputMessages = new UIInput();

		public override void OnInitialize()
		{
			screen.Width.Precent = 1;
			screen.Height.Precent = 1;
			screen.SetPadding(0);
			Append(screen);

			avatarUser.Height.Pixels = 40;
			avatarUser.Left.Pixels = 8;
			avatarUser.Top.Set(-40, 1);
			screen.Append(avatarUser);

			buttonGuilds.Width.Pixels = 20;
			buttonGuilds.Height.Pixels = 20;
			buttonGuilds.Left.Pixels = 8;
			buttonGuilds.Top.Set(-68, 1);
			buttonGuilds.OnClick += ButtonGuilds_OnClick;
			screen.Append(buttonGuilds);

			textServer.Left.Pixels = 36;
			textServer.Top.Set(-68, 1);
			textServer.OnClick += ButtonChannels_OnClick;
			screen.Append(textServer);

			gridGuilds.Height.Pixels = 212;
			gridGuilds.Width.Pixels = 20;
			gridGuilds.Top.Set(-288, 1);
			gridGuilds.Left.Pixels = 8;
			gridGuilds.ListPadding = 8f;

			barGuilds.SetView(100f, 1000f);
			gridGuilds.SetScrollbar(barGuilds);

			gridChannels.Height.Pixels = 212;
			gridChannels.Width.Pixels = 300;
			gridChannels.Top.Set(-288, 1);
			gridChannels.Left.Pixels = 36;
			gridChannels.ListPadding = 4f;

			barChannels.SetView(100f, 1000f);
			gridChannels.SetScrollbar(barChannels);

			panelMessages.Width.Set(0, 0.7f);
			panelMessages.Height.Set(0, 0.4f);
			panelMessages.VAlign = 0.5f;
			panelMessages.SetPadding(0);
			panelMessages.BackgroundColor = panelColor * 0.6f;
			screen.Append(panelMessages);

			gridMessages.Width.Set(-44, 1);
			gridMessages.Height.Set(-64, 1);
			gridMessages.Top.Pixels = 8;
			gridMessages.Left.Set(36, 0);
			gridMessages.ListPadding = 8;
			panelMessages.Append(gridMessages);

			panelInputMessages.Width.Set(-44, 1);
			panelInputMessages.Height.Pixels = 40;
			panelInputMessages.Left.Pixels = 36;
			panelInputMessages.Top.Set(-48, 1f);
			panelMessages.Append(panelInputMessages);

			inputMessages.Width.Precent = 1;
			inputMessages.Height.Precent = 1;
			inputMessages.OnEnter += async delegate
			{
				if (!string.IsNullOrEmpty(inputMessages.GetText()))
				{
					string message = inputMessages.GetText().FormatMessageOut();
					await DTT.Instance.currentClient.SendMessageAsync(DTT.Instance.currentChannel, message);
					inputMessages.currentString = "";
				}
			};
			panelInputMessages.Append(inputMessages);

			CalculatedStyle dimensions = panelMessages.GetDimensions();
			barMessages.Height.Precent = (dimensions.Height - 16) / dimensions.Height;
			barMessages.Top.Precent = 8f / dimensions.Height;
			barMessages.Left.Set(8, 0);
			barMessages.SetView(100f, 1000f);
			gridMessages.SetScrollbar(barMessages);
			panelMessages.Append(barMessages);
		}

		public override void Load()
		{
			InitializeGuilds();
			InitializeChannels();
		}

		public void InitializeGuilds()
		{
			gridGuilds.Clear();

			foreach (DiscordGuild guild in DTT.Instance.currentClient.Guilds.Values)
			{
				UIGuild uiGuild = new UIGuild(guild);
				uiGuild.Width.Pixels = 20;
				uiGuild.Height.Pixels = 20;
				uiGuild.OnClick += (a, b) =>
				{
					DTT.Instance.currentGuild = guild;
					DTT.Instance.currentChannel = guild.GetDefaultChannel();

					if (Utility.cache.ContainsKey(guild.IconUrl)) buttonGuilds.texture = Utility.cache[guild.IconUrl];
					string name = "#" + DTT.Instance.currentChannel.Name.Replace("_", "-");
					textServer.SetText(name);
					textServer.Width.Pixels = name.Measure().X;
					textServer.Height.Pixels = name.Measure().Y;
					textServer.Recalculate();

					InitializeChannels();
					gridMessages.Clear();
				};
				string path = $"{DTT.Guilds}{guild.Id}.png";
				Utility.DownloadImage(path, guild.IconUrl, texture => uiGuild.texture = texture);
				gridGuilds.Add(uiGuild);
			}
		}

		public void InitializeChannels()
		{
			gridChannels.Clear();

			foreach (DiscordChannel channel in DTT.Instance.currentGuild.Channels)
			{
				if (channel.IsCategory && channel.Children.Any(x => x.CanJoin()))
				{
					UICategory uiCategory = new UICategory(channel);
					uiCategory.Width.Precent = 1;
					uiCategory.Height.Pixels = 20;
					gridChannels.Add(uiCategory);
				}
				else if (!channel.IsCategory && !channel.ParentId.HasValue && channel.CanJoin())
				{
					UIChannel uiChild = new UIChannel(channel);
					uiChild.Width.Precent = 1;
					uiChild.Height.Pixels = 20;
					uiChild.OnClick += (a, b) =>
					{
						DTT.Instance.currentChannel = channel;

						string name = "#" + channel.Name.Replace("_", "-");
						textServer.SetText(name);
						textServer.Width.Pixels = name.Measure().X;
						textServer.Height.Pixels = name.Measure().Y;
						textServer.Recalculate();

						gridMessages.Clear();

						//Task<IReadOnlyList<DiscordMessage>> task = channel.GetMessagesAsync(50);
						//task.ContinueWith(t => DTT.log.AddRange(t.Result.ToList()));
					};
					gridChannels.Add(uiChild);
				}
			}
		}
		
		//private void ButtonPMs_OnClick(UIMouseEvent evt, UIElement listeningElement)
		//{
		//	OpenPanel(listeningElement);

		//	gridSelect.Clear();
		//	foreach (DiscordDmChannel channel in DTT.Instance.currentClient.PrivateChannels)
		//	{
		//		UIDMChannel dmChannel = new UIDMChannel(channel);
		//		dmChannel.Width.Precent = 1;
		//		dmChannel.Height.Pixels = 40;
		//		dmChannel.color = DTT.Instance.currentChannel.Id == channel.Id ? Color.Lime : Color.White;
		//		dmChannel.OnClick += (a, b) =>
		//		{
		//			DTT.Instance.currentChannel = dmChannel.channel;

		//			gridSelect.items.ForEach(x => ((UIDMChannel)x).color = Color.White);
		//			dmChannel.color = Color.Lime;

		//			DTT.log.Clear();
		//			Task<IReadOnlyList<DiscordMessage>> task = channel.GetMessagesAsync(50);
		//			task.ContinueWith(t => DTT.log.AddRange(t.Result.ToList()));
		//		};
		//		string path = $"{DTT.Users}{channel.Recipients[0].Id}.png";
		//		Utility.DownloadImage(path, channel.Recipients[0].GetAvatarUrl(ImageFormat.Png, 128), texture => dmChannel.texture = texture);
		//		gridSelect.Add(dmChannel);
		//	}
		//}

		private void ButtonGuilds_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			if (!screen.HasChild(gridGuilds)) screen.Append(gridGuilds);
			else screen.RemoveChild(gridGuilds);
		}

		private void ButtonChannels_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			if (!screen.HasChild(gridChannels)) screen.Append(gridChannels);
			else screen.RemoveChild(gridChannels);
		}
	}
}