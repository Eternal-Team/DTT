using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DTT.UI;
using Microsoft.Xna.Framework.Graphics;
using NamedPipeWrapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace DTT
{
	public class DTT : Mod
	{
		public static DTT Instance;

		public SelectUI SelectUI;
		public UserInterface ISelectUI;

		public ContentManager manager = new ContentManager(Main.instance.Content.ServiceProvider, "C:\\Development\\Terraria\\Mods\\Repository\\DTT\\Shaders");

		public static string SavePath => Main.SavePath + "\\DTT";
		public static string Guilds => SavePath + "\\Cache\\Guilds\\";
		public static string Users => SavePath + "\\Cache\\Users\\";
		public static string PMs => SavePath + "\\Cache\\PMs\\";
		public static string ConfigPath => SavePath + "\\Config.json";

		public NamedPipeServer<Tuple<string, object>> server;
		private Process bot;
		public static Texture2D defaultIcon;

		public DiscordClient currentUser;
		public DiscordGuild currentGuild;
		public DiscordChannel currentChannel;

		//public static Dictionary<ulong, Texture2D> avatars = new Dictionary<ulong, Texture2D>();
		//public static Queue<DiscordUser> AvatarQueue = new Queue<DiscordUser>();

		// create -> add, delete -> remove, update -> change
		public static List<DiscordMessage> log = new List<DiscordMessage>();

		public Config config;

		public DTT()
		{
			Properties = new ModProperties
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}

		public void InitComms()
		{
			currentUser = new DiscordClient(new DiscordConfiguration
			{
				Token = config.token,
				TokenType = TokenType.User
			});
			currentUser.Ready += Ready;
			Task.Run(currentUser.ConnectAsync);

			server = new NamedPipeServer<Tuple<string, object>>("DTTPipe");

			server.ClientConnected += delegate (NamedPipeConnection<Tuple<string, object>, Tuple<string, object>> conn)
			{
				ErrorLogger.Log($"Bot [{conn.Id}] connected");
				conn.PushMessage(new Tuple<string, object>("Notify", "Estabilished a link with Terraria"));
			};
			server.ClientDisconnected += delegate (NamedPipeConnection<Tuple<string, object>, Tuple<string, object>> conn)
			{
				ErrorLogger.Log($"Bot [{conn.Id}] disconnected");
			};

			server.ClientMessage += delegate (NamedPipeConnection<Tuple<string, object>, Tuple<string, object>> conn, Tuple<string, object> tuple)
			{
				switch (tuple.Item1)
				{
					case "DiscordMessageCreate":
					case "DiscordMessageDelete":
					case "DiscordMessageUpdate":
						DiscordMessage message = JsonConvert.DeserializeObject<DiscordMessage>(tuple.Item2.ToString());
						HandleMessage(message, tuple.Item1.Substring(14));
						break;
					case "TokenUpdate":
						config.token = JsonConvert.DeserializeObject<string>(tuple.Item2.ToString());
						break;
				}
			};

			server.Start();
			bot.Start();
		}

		public void HandleMessage(DiscordMessage message, string mode)
		{
			switch (mode)
			{
				case "Create":
					log.Add(message);
					if (log.Count > 50) log.RemoveAt(0);
					break;
				case "Delete":
					log.Remove(message);
					break;
				case "Update":
					int index = log.FindIndex(x => x.Id == message.Id);
					if (index != -1) log[index] = message;
					break;
			}
		}

		private Task Ready(ReadyEventArgs e)
		{
			currentGuild = currentUser.Guilds.ElementAt(0).Value;
			currentChannel = currentGuild.GetDefaultChannel();

			Utility.UpdateCurrent();

			string path = $"{Users}{currentUser.CurrentUser.Id}.png";
			Utility.DownloadImage(path, currentUser.CurrentUser.AvatarUrl, texture =>
			{
				//avatars[currentUser.CurrentUser.Id] = texture;

				SelectUI.avatarUser.texture = texture;
				SelectUI.textUser.SetText(currentUser.CurrentUser.Username);
				SelectUI.avatarUser.RecalculateChildren();
			});

			return Task.Delay(0);
		}

		public static Effect circleShader;
		public override void Load()
		{
			ErrorLogger.ClearLog();

			Instance = this;

			circleShader = GetEffect("Effects/CircleShader");

			Directory.CreateDirectory(SavePath);
			Directory.CreateDirectory(Guilds);
			Directory.CreateDirectory(PMs);
			Directory.CreateDirectory(Users);

			config = System.IO.File.Exists(ConfigPath) ? JsonConvert.DeserializeObject<Config>(System.IO.File.ReadAllText(ConfigPath)) : new Config();

			bot = new Process
			{
				StartInfo =
				{
					FileName = config.botExe,
					UseShellExecute = false,
					Arguments = config.token,
					CreateNoWindow = !config.openWindow
				}
			};

			Main.instance.Exiting += OnExit;
			Main.OnTick += OnTick;

			InitComms();

			defaultIcon = ModLoader.GetTexture("DTT/Textures/DefaultAvatar");

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
			Main.OnTick -= OnTick;

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

		public void OnTick()
		{
			//while (AvatarQueue.Count > 0)
			//{
			//	DiscordUser user = AvatarQueue.Dequeue();
			//	string url = user.GetAvatarUrl(ImageFormat.Png, 256);
			//	string path = $"{Users}{user.Id}.png";

			//	Utility.DownloadImage(path, url, texture => avatars[user.Id] = texture);
			//}
		}

		private void OnExit(object sender, EventArgs e)
		{
			if (Directory.Exists(Guilds))
			{
				DirectoryInfo di = new DirectoryInfo(Guilds);
				foreach (FileInfo file in di.GetFiles()) file.Delete();
			}
			if (Directory.Exists(PMs))
			{
				DirectoryInfo di = new DirectoryInfo(PMs);
				foreach (FileInfo file in di.GetFiles()) file.Delete();
			}
			if (Directory.Exists(Users))
			{
				DirectoryInfo di = new DirectoryInfo(Users);
				foreach (FileInfo file in di.GetFiles()) file.Delete();
			}

			using (StreamWriter writer = System.IO.File.CreateText(ConfigPath)) writer.WriteLine(JsonConvert.SerializeObject(config));

			if (!bot.HasExited) bot.Kill();

			if (server != null)
			{
				for (int i = 0; i < server._connections.Count; i++) server._connections[i].Close();
				server.Stop();
			}
		}
	}
}