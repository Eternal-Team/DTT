using BaseLib.Elements;
using BaseLib.UI;
using BaseLib.Utility;
using DSharpPlus.Entities;
using DTT.UI.Elements;
using System.Linq;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace DTT.UI
{
	public class MainUI : BaseUI
	{
		public UIElement screen = new UIElement();

		public UIUser avatarUser = new UIUser(null);

		public UIRoundImage buttonGuilds = new UIRoundImage(DTT.defaultIcon);
		public UIRoundImage buttonPms = new UIRoundImage(DTT.friendsIcon);
		public UIText textCurrent = new UIText("");

		public UIReverseGrid gridGuilds = new UIReverseGrid();
		public UIScrollbar barGuilds = new UIScrollbar();

		public UIReverseGrid gridPMs = new UIReverseGrid();
		public UIScrollbar barPMs = new UIScrollbar();

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
			buttonGuilds.OnClick += GuildClick;
			buttonGuilds.OnRightClick += SwitchGuildPM;
			screen.Append(buttonGuilds);

			buttonPms.Width.Pixels = 20;
			buttonPms.Height.Pixels = 20;
			buttonPms.Left.Pixels = 8;
			buttonPms.Top.Set(-68, 1);
			buttonPms.OnClick += PMsClick;
			buttonPms.OnRightClick += SwitchGuildPM;

			textCurrent.Left.Pixels = 36;
			textCurrent.Top.Set(-68, 1);
			textCurrent.OnClick += ChannelsClick;
			screen.Append(textCurrent);

			gridGuilds.Height.Pixels = 140;
			gridGuilds.Width.Pixels = 20;
			gridGuilds.Top.Set(-216, 1);
			gridGuilds.Left.Pixels = 8;
			gridGuilds.ListPadding = 8f;

			barGuilds.SetView(100f, 1000f);
			gridGuilds.SetScrollbar(barGuilds);

			gridPMs.Height.Pixels = 140;
			gridPMs.Width.Pixels = 20;
			gridPMs.Top.Set(-216, 1);
			gridPMs.Left.Pixels = 8;
			gridPMs.ListPadding = 8f;

			barPMs.SetView(100f, 1000f);
			gridPMs.SetScrollbar(barPMs);

			gridChannels.Height.Pixels = 212;
			gridChannels.Width.Pixels = 300;
			gridChannels.Top.Set(-288, 1);
			gridChannels.Left.Pixels = 36;
			gridChannels.ListPadding = 4f;

			barChannels.SetView(100f, 1000f);
			gridChannels.SetScrollbar(barChannels);

			panelMessages.Width.Set(0, 0.33f);
			panelMessages.Height.Set(0, 0.4f);
			panelMessages.VAlign = 0.5f;
			panelMessages.SetPadding(0);
			panelMessages.BackgroundColor = panelColor;
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

		public ulong lastIDPM;
		public ulong lastIDChannel;

		public override void Load()
		{
			InitializeGuilds();
			InitializeChannels();
			InitializePMs();
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
					gridMessages.Clear();

					DTT.Instance.currentGuild = guild;
					DTT.Instance.currentChannel = guild.GetDefaultChannel();

					if (Utility.cache.ContainsKey(guild.IconUrl)) buttonGuilds.texture = Utility.cache[guild.IconUrl];
					string name = "#" + DTT.Instance.currentChannel.Name.Replace("_", "-");
					textCurrent.SetText(name);
					textCurrent.Width.Pixels = name.Measure().X;
					textCurrent.Height.Pixels = name.Measure().Y;
					textCurrent.Recalculate();

					Utility.DownloadLog(DTT.Instance.currentChannel);
					InitializeChannels();
				};
				string path = $"{DTT.Guilds}{guild.Id}.png";
				Utility.DownloadImage(path, guild.IconUrl, texture => uiGuild.texture = texture);
				gridGuilds.Add(uiGuild);
			}
		}

		public void InitializePMs()
		{
			gridChannels.Clear();

			foreach (DiscordDmChannel channel in DTT.Instance.currentClient.PrivateChannels)
			{
				UIDMChannel uiDMChannel = new UIDMChannel(channel);
				uiDMChannel.Width.Precent = 1;
				uiDMChannel.Height.Pixels = 20;
				uiDMChannel.OnClick += (a, b) =>
				{
					gridMessages.Clear();

					DTT.Instance.currentChannel = channel;

					string iconURL = channel.IconUrl ?? channel.Recipients[0].AvatarUrl;
					if (Utility.cache.ContainsKey(iconURL)) buttonPms.texture = Utility.cache[iconURL];
					string name = channel.Name ?? channel.Recipients[0].Username;
					textCurrent.SetText(name);
					textCurrent.Width.Pixels = name.Measure().X;
					textCurrent.Height.Pixels = name.Measure().Y;
					textCurrent.Recalculate();

					Utility.DownloadLog(channel);
				};
				string path = $"{DTT.PMs}{channel.Id}.png";
				Utility.DownloadImage(path, channel.IconUrl ?? channel.Recipients[0].AvatarUrl, texture => uiDMChannel.texture = texture);
				gridPMs.Add(uiDMChannel);
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
						textCurrent.SetText(name);
						textCurrent.Width.Pixels = name.Measure().X;
						textCurrent.Height.Pixels = name.Measure().Y;
						textCurrent.Recalculate();

						gridMessages.Clear();

						Utility.DownloadLog(channel);
					};
					gridChannels.Add(uiChild);
				}
			}
		}

		private void SwitchGuildPM(UIMouseEvent evt, UIElement listeningElement)
		{
			gridMessages.Clear();

			if (screen.HasChild(buttonGuilds))
			{
				lastIDChannel = DTT.Instance.currentChannel.Id;

				screen.RemoveChild(buttonGuilds);
				screen.Append(buttonPms);
				textCurrent.SetText("");

				if (DTT.Instance.currentClient.PrivateChannels.Any(x => x.Id == lastIDPM))
				{
					DTT.Instance.currentChannel = DTT.Instance.currentClient.PrivateChannels.First(x => x.Id == lastIDPM);

					string name = DTT.Instance.currentChannel.Name ?? ((DiscordDmChannel)DTT.Instance.currentChannel).Recipients[0].Username;
					name = name.Replace("_", "-");
					textCurrent.SetText(name);
					textCurrent.Width.Pixels = name.Measure().X;
					textCurrent.Height.Pixels = name.Measure().Y;

					Utility.DownloadLog(DTT.Instance.currentChannel);
				}

				textCurrent.Recalculate();

				if (screen.HasChild(gridGuilds)) screen.RemoveChild(gridGuilds);
			}
			else if (screen.HasChild(buttonPms))
			{
				lastIDPM = DTT.Instance.currentChannel.Id;

				screen.RemoveChild(buttonPms);
				screen.Append(buttonGuilds);
				textCurrent.SetText("");

				if (DTT.Instance.currentGuild.Channels.Any(x => x.Id == lastIDChannel))
				{
					DTT.Instance.currentChannel = DTT.Instance.currentGuild.Channels.First(x => x.Id == lastIDChannel);

					string name = "#" + DTT.Instance.currentChannel.Name;
					name = name.Replace("_", "-");
					textCurrent.SetText(name);
					textCurrent.Width.Pixels = name.Measure().X;
					textCurrent.Height.Pixels = name.Measure().Y;

					Utility.DownloadLog(DTT.Instance.currentChannel);
				}

				textCurrent.Recalculate();

				if (screen.HasChild(gridPMs)) screen.RemoveChild(gridPMs);
			}
		}

		private void PMsClick(UIMouseEvent evt, UIElement listeningElement)
		{
			if (!screen.HasChild(gridPMs)) screen.Append(gridPMs);
			else screen.RemoveChild(gridPMs);
		}

		private void GuildClick(UIMouseEvent evt, UIElement listeningElement)
		{
			if (!screen.HasChild(gridGuilds)) screen.Append(gridGuilds);
			else screen.RemoveChild(gridGuilds);
		}

		private void ChannelsClick(UIMouseEvent evt, UIElement listeningElement)
		{
			if (!screen.HasChild(gridChannels)) screen.Append(gridChannels);
			else screen.RemoveChild(gridChannels);
		}
	}
}