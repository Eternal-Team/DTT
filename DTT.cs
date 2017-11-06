﻿using DSharpPlus;
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

		public static string SavePath => Main.SavePath + "\\DTT";
		public static string IconCache => SavePath + "\\Cache";
		public static string ConfigPath => SavePath + "\\Config.json";

		public NamedPipeServer<Tuple<string, object>> server;
		private Process bot;
		public static Texture2D defaultAvatar;

		public DiscordClient currentUser;
		public static Dictionary<DiscordGuild, Texture2D> guilds = new Dictionary<DiscordGuild, Texture2D>();
		public DiscordGuild currentGuild;
		public DiscordChannel currentChannel;

		public static Dictionary<ulong, Texture2D> avatars = new Dictionary<ulong, Texture2D>();
		public static Queue<DiscordUser> AvatarQueue = new Queue<DiscordUser>();

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
						currentUser = new DiscordClient(new DiscordConfiguration
						{
							Token = tuple.Item2.ToString(),
							TokenType = TokenType.User
						});
						currentUser.Ready += Ready;
						Task.Run(currentUser.ConnectAsync);
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

		private Task Ready(ReadyEventArgs e)
		{
			Utility.InitGuilds();

			currentGuild = currentUser.Guilds.ElementAt(0).Value;
			currentChannel = currentGuild.GetDefaultChannel();

			ChangeGuild();

			Utility.DownloadImage($"{IconCache}\\Users\\{currentUser.CurrentUser.Id}.png", currentUser.CurrentUser.AvatarUrl, (a, b) =>
			 {
				 Utility.AvatarFromPath(currentUser.CurrentUser, b.UserState.ToString());

				 SelectUI.avatarUser.texture = avatars[currentUser.CurrentUser.Id];
				 SelectUI.textUser.SetText(currentUser.CurrentUser.Username);
				 SelectUI.avatarUser.RecalculateChildren();
			 });

			return Task.Delay(0);
		}

		public override void Load()
		{
			ErrorLogger.ClearLog();

			Instance = this;

			Directory.CreateDirectory(SavePath);
			Directory.CreateDirectory(IconCache);
			Directory.CreateDirectory(IconCache + "\\Guilds");
			Directory.CreateDirectory(IconCache + "\\Users");

			if (Directory.Exists(IconCache + "\\Guilds"))
			{
				DirectoryInfo di = new DirectoryInfo(IconCache + "\\Guilds");
				foreach (FileInfo file in di.GetFiles()) file.Delete();
			}
			if (Directory.Exists(IconCache + "\\Users"))
			{
				DirectoryInfo di = new DirectoryInfo(IconCache + "\\Users");
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

			Main.instance.Exiting += OnExit;
			Main.OnTick += OnTick;

			InitComms();

			defaultAvatar = ModLoader.GetTexture("DTT/Textures/DefaultAvatar");

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
			while (AvatarQueue.Count > 0)
			{
				DiscordUser user = AvatarQueue.Dequeue();
				string url = user.GetAvatarUrl(ImageFormat.Png, 256);
				string path = $"{IconCache}\\Users\\{user.Id}.png";

				Utility.DownloadImage(path, url, (a, e) =>
				{
					Utility.AvatarFromPath(user, e.UserState.ToString());
				});
			}
		}

		private void OnExit(object sender, EventArgs e)
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