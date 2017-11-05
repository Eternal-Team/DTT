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
using System.Net;
using System.Threading.Tasks;
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

		public string SavePath => Main.SavePath + "\\DTT";
		public string IconCache => SavePath + "\\Cache";
		public string ConfigPath => SavePath + "\\Config.json";

		public static Texture2D arrowHead;

		public Config config;

		public DTT()
		{
			Properties = new ModProperties
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};

			if (!Directory.Exists(IconCache)) Directory.CreateDirectory(IconCache);
			else
			{
				DirectoryInfo di = new DirectoryInfo(IconCache);
				foreach (FileInfo file in di.GetFiles()) file.Delete();
			}

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
		}
		
		public NamedPipeServer<Tuple<string, object>> server;
		private Process bot;
		public DiscordClient discord;
		public DiscordGuild currentGuild;
		public DiscordChannel currentChannel;
		public Dictionary<ulong, Texture2D> guildIcons = new Dictionary<ulong, Texture2D>();

		public void InitComms()
		{
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
					case "DiscordClient":
						discord = new DiscordClient(new DiscordConfiguration
						{
							Token = tuple.Item2.ToString(),
							TokenType = TokenType.User
						});
						discord.Ready += Ready;
						Task.Run(discord.ConnectAsync);
						break;
					case "DiscordMessage":
						DiscordMessage message = JsonConvert.DeserializeObject<DiscordMessage>(tuple.Item2.ToString());
						break;
					case "TokenUpdate":
						config.token = JsonConvert.DeserializeObject<string>(tuple.Item2.ToString());
						break;
				}
			};

			server.Start();
			bot.Start();
		}

		/// <summary>
		/// Relays a message to the bot to update its current guild and channel
		/// </summary>
		public void ChangeGuild()
		{
			server.PushMessage(new Tuple<string, object>("UpdateCurrent", new Tuple<ulong, ulong>(currentGuild.Id, currentChannel.Id)));
		}

		/// <summary>
		/// Downloads icons for all guilds
		/// </summary>
		public void DownloadGuildIcons()
		{
			foreach (KeyValuePair<ulong, DiscordGuild> guild in discord.Guilds)
			{
				string path = $"{IconCache}\\{guild.Key}.png";
				if (!System.IO.File.Exists(path))
				{
					using (WebClient client = new WebClient())
					{
						client.DownloadFile(new Uri(guild.Value.IconUrl), path);
						using (MemoryStream buffer = new MemoryStream(System.IO.File.ReadAllBytes(path)))
						{
							guildIcons[guild.Key] = Texture2D.FromStream(Main.instance.GraphicsDevice, buffer);
							guildIcons[guild.Key].Name = guild.Key.ToString();
						}
					}
				}
			}
		}

		private Task Ready(ReadyEventArgs e)
		{
			DownloadGuildIcons();

			currentGuild = discord.Guilds.ElementAt(0).Value;
			currentChannel = currentGuild.GetDefaultChannel();

			ChangeGuild();

			return Task.Delay(0);
		}

		public override void Load()
		{
			ErrorLogger.ClearLog();

			Instance = this;

			arrowHead = ModLoader.GetTexture("DTT/Textures/Arrow");

			Main.instance.Exiting += Instance_Exiting;

			InitComms();

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
			Main.instance.Exiting -= Instance_Exiting;

			arrowHead = null;

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

		private void Instance_Exiting(object sender, EventArgs e)
		{
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