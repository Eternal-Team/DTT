using System.IO;
using BaseLib.Elements;
using BaseLib.UI;
using DSharpPlus;
using DSharpPlus.Entities;
using DTT.UI.Elements;
using Microsoft.Xna.Framework;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace DTT.UI
{
	public class SelectUI : BaseUI
	{
		public UIElement screen = new UIElement();

		public UIButton avatarUser = new UIButton(DTT.defaultIcon);
		public UIText textUser = new UIText("");

		public UITextButton buttonGuilds = new UITextButton("Guilds");
		public UITextButton buttonServers = new UITextButton("Channels");

		public UIPanel panelSelect = new UIPanel();
		public UIGrid gridSelect = new UIGrid();
		public UIScrollbar barSelect = new UIScrollbar();

		public override void OnInitialize()
		{
			screen.Width.Precent = 1;
			screen.Height.Precent = 1;
			Append(screen);

			buttonGuilds.Width.Pixels = 100;
			buttonGuilds.Height.Pixels = 50;
			buttonGuilds.Top.Set(-98, 1);
			buttonGuilds.OnClick += ButtonGuilds_OnClick;
			screen.Append(buttonGuilds);

			buttonServers.Width.Pixels = 100;
			buttonServers.Height.Pixels = 50;
			buttonServers.Top.Set(-156, 1);
			buttonServers.OnClick += ButtonServers_OnClick;
			screen.Append(buttonServers);

			avatarUser.Width.Pixels = 40;
			avatarUser.Height.Pixels = 40;
			avatarUser.Top.Set(-40, 1);
			Append(avatarUser);

			textUser.Left.Set(8, 1);
			textUser.VAlign = 0.5f;
			avatarUser.Append(textUser);

			panelSelect.Width.Pixels = 250;
			panelSelect.Height.Pixels = 350;
			panelSelect.Left.Pixels = 100;
			panelSelect.SetPadding(0);
			panelSelect.Top.Set(-350, 1);

			gridSelect.Width.Set(-16, 1);
			gridSelect.Height.Set(-16, 1);
			gridSelect.Left.Pixels = 8;
			gridSelect.Top.Pixels = 8;
			gridSelect.ListPadding = 4f;
			panelSelect.Append(gridSelect);

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

		public bool CanJoin(DiscordChannel channel) => (channel.PermissionsFor(DTT.Instance.currentGuild.CurrentMember) & Permissions.ReadMessageHistory) != 0 && (channel.PermissionsFor(DTT.Instance.currentGuild.CurrentMember) & Permissions.AccessChannels) != 0;

		private void ButtonServers_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			OpenPanel(listeningElement);

			gridSelect.Clear();
			foreach (DiscordChannel channel in DTT.Instance.currentGuild.Channels)
			{
				if (channel.IsCategory && channel.Children.Any(CanJoin))
				{
					UICategory uiCategory = new UICategory(channel);
					uiCategory.Width.Precent = 1;
					uiCategory.Height.Pixels = 40;

					foreach (DiscordChannel child in channel.Children)
					{
						UIChannel uiChild = new UIChannel(child);
						uiChild.Width.Set(-8, 1);
						uiChild.Height.Pixels = 40;
						uiCategory.items.Add(uiChild);
					}

					gridSelect.Add(uiCategory);
				}
				else if (!channel.IsCategory && !channel.ParentId.HasValue)
				{
					UIChannel uiChild = new UIChannel(channel);
					uiChild.Width.Precent = 1;
					uiChild.Height.Pixels = 40;
					gridSelect.Add(uiChild);
				}
			}
		}

		private void ButtonGuilds_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			OpenPanel(listeningElement);

			gridSelect.Clear();
			foreach (DiscordGuild guild in DTT.Instance.currentUser.Guilds.Values)
			{
				UIGuild uiGuild = new UIGuild(guild);
				uiGuild.Width.Precent = 1;
				uiGuild.Height.Pixels = 40;
				uiGuild.color = DTT.Instance.currentGuild.Id == guild.Id ? Color.Lime : Color.White;
				uiGuild.OnClick += (a, b) =>
				{
					DTT.Instance.currentGuild = guild;

					gridSelect.items.ForEach(x => ((UIGuild)x).color = Color.White);
					uiGuild.color = Color.Lime;

					DTT.Instance.currentChannel = guild.GetDefaultChannel();
					DTT.Instance.ChangeGuild();
				};
				string path = $"{DTT.Guilds}{guild.Id}.png";
				Utility.DownloadImage(path, guild.IconUrl, () =>
				{
					using (MemoryStream buffer = new MemoryStream(File.ReadAllBytes(path)))
					{
						Texture2D texture = Texture2D.FromStream(Main.instance.GraphicsDevice, buffer);
						texture.Name = guild.Id.ToString();
						uiGuild.texture = texture;
					}
				});
				gridSelect.Add(uiGuild);
			}
		}
	}
}