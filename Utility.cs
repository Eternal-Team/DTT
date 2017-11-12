using DSharpPlus;
using DSharpPlus.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Terraria;

namespace DTT
{
	public static class Utility
	{
		private static readonly Color colorInvisibleOffline = new Color(116, 127, 141);
		private static readonly Color colorActive = new Color(67, 181, 129);
		private static readonly Color colorIdle = new Color(250, 166, 26);
		private static readonly Color colorDoNotDisturb = new Color(240, 71, 71);

		//public static readonly Regex channelFindID = new Regex(@"(?<=<#)\d+(?=>)");
		//public static readonly Regex channelFind = new Regex(@"<#\d+>");

		//public static readonly Regex roleFindID = new Regex(@"(?<=<@&)\d+(?=>)");
		//public static readonly Regex roleFind = new Regex(@"<@&\d+>");

		//public static readonly Regex userFindID = new Regex(@"(?<=<@)\d+(?=>)");
		//public static readonly Regex userFind = new Regex(@"<@\d+>");

		//public static readonly Regex emoteFindID = new Regex(@"(?<=:)\d+(?=>)");
		//public static readonly Regex emoteFind = new Regex(@"<:\w+:\d+>");

		public static readonly List<Regex> findRegexes = new List<Regex> { new Regex(@"<#\d+>"), new Regex(@"<@&\d+>"), new Regex(@"<@\d+>"), new Regex(@"<:\w+:\d+>") };
		public static readonly List<Regex> findIDRegexes = new List<Regex> { new Regex(@"(?<=<#)\d+(?=>)"), new Regex(@"(?<=<@&)\d+(?=>)"), new Regex(@"(?<=<@)\d+(?=>)"), new Regex(@"(?<=:)\d+(?=>)") };

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
			using (StreamWriter writer = File.CreateText(DTT.ConfigPath)) writer.WriteLine(JsonConvert.SerializeObject(config, Formatting.Indented));
		}

		public static Config Load(this string path) => File.Exists(path) ? JsonConvert.DeserializeObject<Config>(File.ReadAllText(path).Replace("\r\n", ""), new JsonSerializerSettings { Formatting = Formatting.Indented }) : new Config();

		public static void CleanDir(this string path)
		{
			if (Directory.Exists(path))
			{
				DirectoryInfo di = new DirectoryInfo(path);
				foreach (FileInfo file in di.GetFiles()) file.Delete();
			}
		}

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

		public static IEnumerable<Snippet> SplitToLines(this string stringToSplit, float maxLineLength)
		{
			float x = 0;
			float y = 0;

			string[] newLines = stringToSplit.Split('\n');

			for (var j = 0; j < newLines.Length; j++)
			{
				string newLine = newLines[j];
				string[] words = newLine.Split(' ');

				for (int i = 0; i < words.Length; i++)
				{
					string word = words[i];

					string text = word;
					ulong id = 0;
					Color color = Color.White;
					//if (findRegexes.Any(reg => reg.IsMatch(text)))
					//{
					//	Regex use = findRegexes.First(reg => reg.IsMatch(text));
					//	id = ulong.Parse(findIDRegexes[findRegexes.IndexOf(use)].Match(text).Value);
					//	text = use.Replace(text, $"#{DTT.Instance.currentGuild.Channels.First(z => z.Id == id).Name}");
					//	color = Color.LightBlue;
					//}

					if (x + Main.fontMouseText.MeasureString(text).X > maxLineLength)
					{
						x = 0;
						y += Main.fontMouseText.MeasureString(text).Y + 4;
					}

					yield return new Snippet
					{
						Height = Main.fontMouseText.MeasureString(text).Y,
						Width = Main.fontMouseText.MeasureString(text).X,
						X = x,
						Y = y,
						Text = text,
						ID = id,
						OnHover = a => { Main.NewText(a); },
						Color = color
					};

					x += Main.fontMouseText.MeasureString(word).X + 8;
				}

				y += 24;
				x = 0;
			}
		}

		public static Color FromInt(this int value) => value == 0 ? Color.White : new Color((value >> 16) & 0xff, (value >> 8) & 0xff, (value >> 0) & 0xff);
	}

	public class Snippet
	{
		public float X;
		public float Y;
		public float Width;
		public float Height;
		public string Text;
		public ulong ID;
		public Color Color;

		public Action<ulong> OnClick;
		public Action<ulong> OnHover;

		public override string ToString() => $"X: [{X}], Y: [{Y}], Width: [{Width}], Height: [{Height}], ID: [{ID}], Text: [{Text}]";

		public Rectangle ToRectangle() => new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
	}

	public class Emote : Snippet
	{

	}
}