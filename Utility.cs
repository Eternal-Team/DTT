using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using DSharpPlus.Entities;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace DTT
{
	public static class Utility
	{
		public static void DownloadImage(string path, string url, Action<object, AsyncCompletedEventArgs> action = null)
		{
			if (!File.Exists(path))
			{
				using (WebClient client = new WebClient())
				{
					client.DownloadFileCompleted += (a, b) => action?.Invoke(a, b);
					client.DownloadFileAsync(new Uri(url), path, path);
				}
			}
		}

		/// <summary>
		/// Downloads icons for all guilds
		/// </summary>
		public static void InitGuilds()
		{
			foreach (KeyValuePair<ulong, DiscordGuild> guild in DTT.Instance.currentUser.Guilds)
			{
				string path = $"{DTT.IconCache}\\Guilds\\{guild.Key}.png";

				DownloadImage(path, guild.Value.IconUrl, (a, b) =>
				 {
					 using (MemoryStream buffer = new MemoryStream(File.ReadAllBytes(path)))
					 {
						 Texture2D texture = Texture2D.FromStream(Main.instance.GraphicsDevice, buffer);
						 texture.Name = guild.Key.ToString();

						 DTT.guilds.Add(guild.Value, texture);
					 }
				 });
			}
		}

		public static void AvatarFromPath(DiscordUser user, string path)
		{
			using (MemoryStream buffer = new MemoryStream(File.ReadAllBytes(path)))
			{
				DTT.avatars[user.Id] = Texture2D.FromStream(Main.instance.GraphicsDevice, buffer);
				DTT.avatars[user.Id].Name = user.Username;
			}
		}
	}
}