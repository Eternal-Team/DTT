using DSharpPlus;
using DSharpPlus.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.UI;

namespace DTT
{
	public static class Utility
	{
		private static readonly Color colorInvisibleOffline = new Color(116, 127, 141);
		private static readonly Color colorActive = new Color(67, 181, 129);
		private static readonly Color colorIdle = new Color(250, 166, 26);
		private static readonly Color colorDoNotDisturb = new Color(240, 71, 71);

		public static readonly List<Regex> findRegexes = new List<Regex> { new Regex(@"<#\d+>"), new Regex(@"<@&\d+>"), new Regex(@"<@\d+>"), new Regex(@"<:\w+:\d+>"), new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase) };
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

		public static IEnumerable<Snippet> FormatMessage(this string stringToSplit, float maxLineLength)
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
					string text = words[i];
					ulong id;

					int index = findRegexes.FindIndex(z => z.IsMatch(text));

					switch (index)
					{
						case 0:     // Channels
							id = ulong.Parse(findIDRegexes[index].Match(text).Value);
							DiscordChannel channel = DTT.Instance.currentGuild.Channels.First(z => z.Id == id);
							text = $"#{channel.Name}";
							yield return new Snippet
							{
								Width = Main.fontMouseText.MeasureString(text).X,
								Height = Main.fontMouseText.MeasureString(text).Y,
								X = x,
								Y = y,
								Text = text,
								Color = new Color(105, 137, 200),
								OnClick = () => { DTT.Instance.currentChannel = channel; }
							};
							break;
						case 1:     // Roles
							id = ulong.Parse(findIDRegexes[index].Match(text).Value);
							DiscordRole role = DTT.Instance.currentGuild.Roles.First(z => z.Id == id);
							text = $"@{role.Name}";
							yield return new Snippet
							{
								Width = Main.fontMouseText.MeasureString(text).X,
								Height = Main.fontMouseText.MeasureString(text).Y,
								X = x,
								Y = y,
								Text = text,
								Color = role.Color.Value.FromInt()
							};
							break;
						case 2:     // Mentions
							id = ulong.Parse(findIDRegexes[index].Match(text).Value);
							DiscordMember member = DTT.Instance.currentGuild.Members.First(z => z.Id == id);
							text = $"@{member.DisplayName}";
							yield return new Snippet
							{
								Width = Main.fontMouseText.MeasureString(text).X,
								Height = Main.fontMouseText.MeasureString(text).Y,
								X = x,
								Y = y,
								Text = text,
								Color = member.Color.Value.FromInt()
							};
							break;
						case 3:     // Emotes
							id = ulong.Parse(findIDRegexes[index].Match(text).Value);
							DiscordEmoji emoji = DTT.Instance.currentGuild.Emojis.First(z => z.Id == id);
							text = emoji.GetDiscordName();
							string url = "https://" + $"cdn.discordapp.com/emojis/{id}.png";
							Texture2D emojiTexture = null;
							DownloadImage($"{DTT.Emojis}{id}.png", url, texture => emojiTexture = texture);
							yield return new Snippet
							{
								Width = 20,
								Height = 20,
								X = x,
								Y = y,
								OnDraw = (spriteBatch, dimensions) =>
								{
									if (cache.ContainsKey(url) && emojiTexture != null)
									{
										float scale = Math.Min(20f / emojiTexture.Width, 20f / emojiTexture.Height);
										spriteBatch.Draw(emojiTexture, dimensions.Position(), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
									}
								},
								OnHover = (spriteBatch, dimensions) => { BaseLib.Utility.Utility.DrawMouseText(text); }
							};
							x += 20;
							break;
						case 4:     // Links
							string link = text;
							text = "[Link]";
							yield return new Snippet
							{
								Width = Main.fontMouseText.MeasureString(text).X,
								Height = Main.fontMouseText.MeasureString(text).Y,
								X = x,
								Y = y,
								Text = text,
								Color = new Color(105, 137, 200),
								OnHover = (spriteBatch, dimensions) => { BaseLib.Utility.Utility.DrawMouseText(link); },
								OnClick = () => { Process.Start(link); }
							};
							break;
						default:    // Other
							yield return new Snippet
							{
								Width = Main.fontMouseText.MeasureString(text).X,
								Height = Main.fontMouseText.MeasureString(text).Y,
								X = x,
								Y = y,
								Text = text,
								Color = Color.White
							};
							break;
					}

					if (x + Main.fontMouseText.MeasureString(text).X > maxLineLength)
					{
						x = 0;
						y += Main.fontMouseText.MeasureString(text).Y;
					}

					if (index < 3 || index == 4) x += Main.fontMouseText.MeasureString(text).X + 8;
				}

				y += 20;
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

		public Action OnClick;
		public Action<SpriteBatch, CalculatedStyle> OnHover;
		public Action<SpriteBatch, CalculatedStyle> OnDraw;

		public override string ToString() => $"X: [{X}], Y: [{Y}], Width: [{Width}], Height: [{Height}], ID: [{ID}], Text: [{Text}]";

		public Rectangle ToRectangle() => new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
	}
}