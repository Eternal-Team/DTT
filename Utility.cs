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
		public static void DownloadImage(string path, string url, Action action = null)
		{
			if (!File.Exists(path))
			{
				using (WebClient client = new WebClient())
				{
					client.DownloadFileCompleted += (a, b) => action?.Invoke();
					client.DownloadFileAsync(new Uri(url), path, path);
				}
			}
			else action?.Invoke();
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