using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using DSharpPlus;
using DSharpPlus.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Terraria;

namespace DTT
{
	public static class Utility
	{
		public static Dictionary<string, Texture2D> cache = new Dictionary<string, Texture2D>();

		public static void DownloadImage(string path, string url, Action<Texture2D> action = null)
		{
			if (!File.Exists(path))
			{
				using (WebClient client = new WebClient())
				{
					client.DownloadFileAsync(new Uri(url), path, path);
					client.DownloadFileCompleted += (a, b) =>
					{
						Texture2D texture = path.ToTexture();
						cache[url] = texture;
						action?.Invoke(texture);
					};
				}
			}
			else
			{
				if (!cache.ContainsKey(url) && !path.IsFileLocked())
				{
					cache[url] = path.ToTexture();
					action?.Invoke(cache[url]);
				}
				else if (cache.ContainsKey(url)) action?.Invoke(cache[url]);
			}
		}

		public static Texture2D ToTexture(this string path)
		{
			Texture2D texture;
			using (MemoryStream buffer = new MemoryStream(File.ReadAllBytes(path))) texture = Texture2D.FromStream(Main.instance.GraphicsDevice, buffer);
			return texture;
		}

		public static bool CanJoin(this DiscordChannel channel) => (channel.PermissionsFor(DTT.Instance.currentGuild.CurrentMember) & Permissions.ReadMessageHistory) != 0 && (channel.PermissionsFor(DTT.Instance.currentGuild.CurrentMember) & Permissions.AccessChannels) != 0;

		public static void Save(this Config config)
		{
			using (StreamWriter writer = File.CreateText(DTT.ConfigPath)) writer.WriteLine(JsonConvert.SerializeObject(config));
		}

		public static Config Load(this string path) => File.Exists(path) ? JsonConvert.DeserializeObject<Config>(File.ReadAllText(path)) : new Config();

		public static void CleanDir(this string path)
		{
			if (Directory.Exists(path))
			{
				DirectoryInfo di = new DirectoryInfo(path);
				foreach (FileInfo file in di.GetFiles()) file.Delete();
			}
		}

		private static readonly Color colorInvisibleOffline = new Color(116, 127, 141);
		private static readonly Color colorActive = new Color(67, 181, 129);
		private static readonly Color colorIdle = new Color(250, 166, 26);
		private static readonly Color colorDoNotDisturb = new Color(240, 71, 71);

		public static Color PresenceColor(this DiscordPresence presence)
		{
			if (presence != null)
			{
				switch (presence.Status)
				{
					case UserStatus.Online:
						return colorActive;
					case UserStatus.Idle:
						return colorIdle;
					case UserStatus.DoNotDisturb:
						return colorDoNotDisturb;
					default:
						return colorInvisibleOffline;
				}
			}
			return colorInvisibleOffline;
		}

		public static bool IsFileLocked(this string path)
		{
			FileInfo file = new FileInfo(path);
			FileStream stream = null;

			try
			{
				stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
			}
			catch (IOException)
			{
				return true;
			}
			finally
			{
				stream?.Close();
			}

			return false;
		}
	}
}