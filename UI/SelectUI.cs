using BaseLib.Elements;
using BaseLib.UI;
using DSharpPlus;
using DSharpPlus.Entities;
using DTT.UI.Elements;
using Microsoft.Xna.Framework;
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

		public UITextButton buttonGuilds = new UITextButton("Guilds");
		public UITextButton buttonServers = new UITextButton("Channels");
		public UITextButton buttonPMs = new UITextButton("PMs");

		public UIPanel panelSelect = new UIPanel();
		public UIGrid gridSelect = new UIGrid();
		public UIScrollbar barSelect = new UIScrollbar();

		public UIText test = new UIText("");

		public override void OnInitialize()
		{
			screen.Width.Precent = 1;
			screen.Height.Precent = 1;
			Append(screen);

			buttonGuilds.Width.Pixels = 120;
			buttonGuilds.Height.Pixels = 50;
			buttonGuilds.Top.Set(-98, 1);
			buttonGuilds.OnClick += ButtonGuilds_OnClick;
			screen.Append(buttonGuilds);

			buttonServers.Width.Pixels = 120;
			buttonServers.Height.Pixels = 50;
			buttonServers.Top.Set(-156, 1);
			buttonServers.OnClick += ButtonServers_OnClick;
			screen.Append(buttonServers);

			buttonPMs.Width.Pixels = 120;
			buttonPMs.Height.Pixels = 50;
			buttonPMs.Top.Set(-214, 1);
			buttonPMs.OnClick += ButtonPMs_OnClick;
			screen.Append(buttonPMs);

			avatarUser.Width.Pixels = 120;
			avatarUser.Height.Pixels = 40;
			avatarUser.Top.Set(-40, 1);
			Append(avatarUser);

			panelSelect.Width.Pixels = 250;
			panelSelect.Height.Pixels = 350;
			panelSelect.Left.Pixels = 120;
			panelSelect.SetPadding(0);
			panelSelect.Top.Set(-350, 1);

			gridSelect.Width.Set(-16, 1);
			gridSelect.Height.Set(-16, 1);
			gridSelect.Left.Pixels = 8;
			gridSelect.Top.Pixels = 8;
			gridSelect.ListPadding = 4f;
			panelSelect.Append(gridSelect);

			test.HAlign = 1;
			test.VAlign = 1;
			Append(test);

			barSelect.SetView(100f, 1000f);
			gridSelect.SetScrollbar(barSelect);
		}

		private UIElement prevElement;
		public void OpenPanel(UIElement element)
		{
			if (prevElement == element)
			{
				screen.RemoveChild(panelSelect);
				prevElement = null;
			}
			else
			{
				if (!screen.HasChild(panelSelect)) screen.Append(panelSelect);
				prevElement = element;
			}
		}

		private void ButtonServers_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			OpenPanel(listeningElement);

			gridSelect.Clear();
			foreach (DiscordChannel channel in DTT.Instance.currentGuild.Channels)
			{
				if (channel.IsCategory && channel.Children.Any(x => x.CanJoin()))
				{
					UICategory uiCategory = new UICategory(channel);
					uiCategory.Width.Precent = 1;
					uiCategory.Height.Pixels = 40;

					foreach (DiscordChannel child in channel.Children)
					{
						if (child.CanJoin())
						{
							UIChannel uiChild = new UIChannel(child);
							uiChild.Width.Set(-8, 1);
							uiChild.Height.Pixels = 40;
							uiChild.color = DTT.Instance.currentChannel.Id == child.Id ? Color.Lime : Color.White;
							uiChild.OnClick += (a, b) =>
							{
								DTT.Instance.currentChannel = child;

								gridSelect.items.ForEach(x =>
								{
									if (x is UIChannel) ((UIChannel)x).color = Color.White;
									else
									{
										(x as UICategory)?.items.ForEach(y => ((UIChannel)y).color = Color.White);
									}
								});
								uiChild.color = Color.Lime;

								DTT.log.Clear();
								Task<IReadOnlyList<DiscordMessage>> task = child.GetMessagesAsync(50);
								task.ContinueWith(t => DTT.log.AddRange(t.Result.ToList()));
							};
							uiCategory.items.Add(uiChild);
						}
					}

					gridSelect.Add(uiCategory);
				}
				else if (!channel.IsCategory && !channel.ParentId.HasValue && channel.CanJoin())
				{
					UIChannel uiChild = new UIChannel(channel);
					uiChild.Width.Precent = 1;
					uiChild.Height.Pixels = 40;
					uiChild.color = DTT.Instance.currentChannel.Id == channel.Id ? Color.Lime : Color.White;
					uiChild.OnClick += (a, b) =>
					{
						DTT.Instance.currentChannel = uiChild.channel;

						gridSelect.items.ForEach(x =>
						{
							if (x is UIChannel) ((UIChannel)x).color = Color.White;
							else
							{
								(x as UICategory)?.items.ForEach(y => ((UIChannel)y).color = Color.White);
							}
						});
						uiChild.color = Color.Lime;

						DTT.log.Clear();
						Task<IReadOnlyList<DiscordMessage>> task = channel.GetMessagesAsync(50);
						task.ContinueWith(t => DTT.log.AddRange(t.Result.ToList()));
					};
					gridSelect.Add(uiChild);
				}
			}
		}

		private void ButtonPMs_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			OpenPanel(listeningElement);

			gridSelect.Clear();
			foreach (DiscordDmChannel channel in DTT.Instance.currentClient.PrivateChannels)
			{
				UIDMChannel dmChannel = new UIDMChannel(channel);
				dmChannel.Width.Precent = 1;
				dmChannel.Height.Pixels = 40;
				dmChannel.color = DTT.Instance.currentChannel.Id == channel.Id ? Color.Lime : Color.White;
				dmChannel.OnClick += (a, b) =>
				{
					DTT.Instance.currentChannel = dmChannel.channel;

					gridSelect.items.ForEach(x => ((UIDMChannel)x).color = Color.White);
					dmChannel.color = Color.Lime;

					DTT.log.Clear();
					Task<IReadOnlyList<DiscordMessage>> task = channel.GetMessagesAsync(50);
					task.ContinueWith(t => DTT.log.AddRange(t.Result.ToList()));
				};
				string path = $"{DTT.Users}{channel.Recipients[0].Id}.png";
				Utility.DownloadImage(path, channel.Recipients[0].GetAvatarUrl(ImageFormat.Png, 128), texture => dmChannel.texture = texture);
				gridSelect.Add(dmChannel);
			}
		}

		private void ButtonGuilds_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			OpenPanel(listeningElement);

			gridSelect.Clear();
			foreach (DiscordGuild guild in DTT.Instance.currentClient.Guilds.Values)
			{
				UIGuild uiGuild = new UIGuild(guild);
				uiGuild.Width.Precent = 1;
				uiGuild.Height.Pixels = 40;
				uiGuild.color = DTT.Instance.currentGuild.Id == guild.Id ? Color.Lime : Color.White;
				uiGuild.OnClick += (a, b) =>
				{
					DTT.Instance.currentGuild = guild;
					DTT.Instance.currentChannel = guild.GetDefaultChannel();

					gridSelect.items.ForEach(x => ((UIGuild)x).color = Color.White);
					uiGuild.color = Color.Lime;
				};
				string path = $"{DTT.Guilds}{guild.Id}.png";
				Utility.DownloadImage(path, guild.IconUrl, texture => uiGuild.texture = texture);
				gridSelect.Add(uiGuild);
			}
		}

		public override void Update(GameTime gameTime)
		{
			test.SetText("Count: " + DTT.log.Count);
			test.Recalculate();
		}
	}
}