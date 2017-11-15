using BaseLib.Utility;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DTT.UI;
using DTT.UI.Elements;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace DTT
{
	public partial class DTT
	{
		public static DTT Instance;

		public static string SavePath => Main.SavePath + "\\DTT";
		public static string Guilds => SavePath + "\\Cache\\Guilds\\";
		public static string Users => SavePath + "\\Cache\\Users\\";
		public static string Emojis => SavePath + "\\Cache\\Emojis\\";
		public static string ConfigPath => SavePath + "\\Config.json";

		public Config config;

		#region Discord
		public DiscordClient currentClient;
		public DiscordGuild currentGuild;
		public DiscordChannel currentChannel;

		public static List<DiscordMessage> log = new List<DiscordMessage>();
		#endregion

		#region UI
		public SelectUI SelectUI;
		public UserInterface ISelectUI;

		public static Texture2D defaultIcon;

		public static Effect circleShader;
		public static Effect activityShader;
		#endregion
	}

	public partial class DTT
	{
		public void Start()
		{
			try
			{
				if (string.IsNullOrWhiteSpace(config.token)) throw new Exception($"Token wasn't set!\nSet it in \"{ConfigPath}\" and reload the mod.");

				currentClient = new DiscordClient(new DiscordConfiguration
				{
					Token = config.token,
					TokenType = TokenType.User,
					AutoReconnect = true
				});

				currentClient.Ready += Ready;
				currentClient.MessageCreated += Create;
				currentClient.MessageDeleted += Delete;
				currentClient.MessageUpdated += Update;
				currentClient.ClientErrored += Error;
				BackgroundWorker bg = new BackgroundWorker();
				bg.DoWork += async delegate { await currentClient.ConnectAsync(); };
				bg.RunWorkerAsync();
			}
			catch (Exception ex)
			{
				typeof(ModLoader).InvokeMethod<object>("SetModActive", new object[] { File, false });
				throw new Exception(ex.ToString());
			}
		}

		private Task Error(ClientErrorEventArgs e)
		{
			ErrorLogger.Log(e.Exception);

			BackgroundWorker bg = new BackgroundWorker();
			bg.DoWork += async delegate { await currentClient.ReconnectAsync(); };
			bg.RunWorkerAsync();

			return Task.Delay(0);
		}

		private Task Create(MessageCreateEventArgs e)
		{
			if (e.Guild == currentGuild && e.Channel == currentChannel)
			{
				UIMessage message = new UIMessage(e.Message);
				message.Width.Set(0f, 1f);
				message.Height.Set(40, 0);
				string path = $"{Users}{e.Author.Id}.png";
				Utility.DownloadImage(path, e.Author.AvatarUrl, texture => message.avatar = texture);
				message.RecalculateMessage();
				SelectUI.gridMessages.Add(message);
				SelectUI.gridMessages.RecalculateChildren();
			}

			return Task.Delay(0);
		}

		private Task Delete(MessageDeleteEventArgs e)
		{
			if (e.Guild == currentGuild && e.Channel == currentChannel)
			{
				log.Remove(e.Message);
			}

			return Task.Delay(0);
		}

		private Task Update(MessageUpdateEventArgs e)
		{
			if (e.Guild == currentGuild && e.Channel == currentChannel)
			{
				int index = log.FindIndex(x => x.Id == e.Message.Id);
				if (index != -1) log[index] = e.Message;
			}

			return Task.Delay(0);
		}

		private Task Ready(ReadyEventArgs e)
		{
			ErrorLogger.Log($"User [{currentClient.CurrentUser.Username}] is ready");

			currentGuild = config.defaultGuildID.HasValue && currentClient.Guilds.Any(x => x.Value.Id == config.defaultGuildID) ? currentClient.Guilds.First(x => x.Value.Id == config.defaultGuildID).Value : currentClient.Guilds.ElementAt(0).Value;
			currentChannel = config.defaultChannelID.HasValue && currentGuild.Channels.Any(x => x.Id == config.defaultChannelID.Value) ? currentGuild.Channels.First(x => x.Id == config.defaultChannelID) : currentGuild.GetDefaultChannel();

			SelectUI.avatarUser.user = currentClient.CurrentUser;

			string path = $"{Users}{currentClient.CurrentUser.Id}.png";
			Utility.DownloadImage(path, currentClient.CurrentUser.AvatarUrl, texture =>
			{
				SelectUI.avatarUser.texture = texture;
				SelectUI.avatarUser.RecalculateChildren();
			});

			return Task.Delay(0);
		}
	}

	public partial class DTT : Mod
	{
		public DTT()
		{
			Properties = new ModProperties
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}

		public override void Load()
		{
			ErrorLogger.ClearLog();

			Instance = this;

			circleShader = GetEffect("Effects/CircleShader");
			activityShader = GetEffect("Effects/ActivityShader");

			defaultIcon = ModLoader.GetTexture("DTT/Textures/DefaultAvatar");

			Directory.CreateDirectory(SavePath);
			Directory.CreateDirectory(Guilds);
			Directory.CreateDirectory(Users);
			Directory.CreateDirectory(Emojis);

			config = ConfigPath.Load();

			Main.instance.Exiting += OnExit;

			Start();

			if (!Main.dedServ)
			{
				SelectUI = new SelectUI();
				SelectUI.Activate();
				ISelectUI = new UserInterface();
				ISelectUI.SetState(SelectUI);
			}
		}

		public override void Unload()
		{
			Main.instance.Exiting -= OnExit;

			Utility.cache.Clear();

			Guilds.CleanDir();
			Users.CleanDir();
			Emojis.CleanDir();

			Instance = null;

			GC.Collect();
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int InventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
			if (InventoryIndex != -1)
			{
				layers.Insert(InventoryIndex, new LegacyGameInterfaceLayer(
					"DTT: Select",
					delegate
					{
						ISelectUI.Update(Main._drawInterfaceGameTime);
						SelectUI.Draw(Main.spriteBatch);

						return true;
					}, InterfaceScaleType.UI));
			}
		}

		private void OnExit(object sender, EventArgs e)
		{
			Guilds.CleanDir();
			Users.CleanDir();
			Emojis.CleanDir();

			config.Save();
		}
	}
}