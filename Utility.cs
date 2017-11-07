using DSharpPlus.Entities;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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
			else if (cache.ContainsKey(url)) action?.Invoke(cache[url]);
		}

		public static Texture2D ToTexture(this string path)
		{
			Texture2D texture;
			using (MemoryStream buffer = new MemoryStream(File.ReadAllBytes(path))) texture = Texture2D.FromStream(Main.instance.GraphicsDevice, buffer);
			return texture;
		}
		
		/// <summary>
		/// Relays a message to the bot to update its current guild and channel
		/// </summary>
		public static void UpdateCurrent()
		{
			DTT.Instance.server.PushMessage(new Tuple<string, object>("UpdateCurrent", new Tuple<ulong, ulong>(DTT.Instance.currentGuild.Id, DTT.Instance.currentChannel.Id)));
		}
	}
}