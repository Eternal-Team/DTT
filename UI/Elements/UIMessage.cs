﻿using BaseLib.Elements;
using BaseLib.Utility;
using DSharpPlus.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;

namespace DTT.UI.Elements
{
	public class UIMessage : BaseElement
	{
		public DiscordMessage message;
		public Texture2D avatar;

		public UIMessage(DiscordMessage message)
		{
			this.message = message;
			avatar = DTT.defaultIcon;
		}

		public override int CompareTo(object obj)
		{
			UIMessage other = obj as UIMessage;
			return other != null ? message.CreationTimestamp.CompareTo(other.message.CreationTimestamp) : 0;
		}

		private Snippet[] lines;
		public DiscordMember member;
		public void RecalculateMessage()
		{
			Recalculate();
			lines = message.Content.FormatMessage(896).ToArray();

			float height = lines.Last().Y + lines.Last().Height + 24;
			if (height < 40) height = 40;
			Height.Set(height, 0);

			if (message.Author.Presence != null && message.Author.Presence.Guild != null && message.Author.Presence.Guild.Id == DTT.Instance.currentGuild.Id)
			{
				Task<DiscordMember> t = message.Author.Presence.Guild.GetMemberAsync(message.Author.Id);
				t.ContinueWith(x => member = t.Result);
			}
		}

		public override void Click(UIMouseEvent evt)
		{
			CalculatedStyle dimensions = GetDimensions();

			for (int i = 0; i < lines.Length; i++)
			{
				Rectangle rect = new Rectangle((int)(dimensions.X + 48 + lines[i].X), (int)(dimensions.Y + 24 + lines[i].Y), (int)lines[i].Width, (int)lines[i].Height);
				if (rect.Contains(evt.MousePosition) && lines[i].OnClick != null) lines[i].OnClick.Invoke();
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();

			spriteBatch.EnableScissor();

			Color color = Color.White;
			if (message.Channel.Guild.Members.Any(x => x.Id == message.Author.Id)) color = message.Channel.Guild.Members.First(x => x.Id == message.Author.Id).Color.Value.FromInt();

			Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, message.Author.Username, dimensions.X + 48, dimensions.Y, color, Color.Black, Vector2.Zero);
			Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, $" - {message.CreationTimestamp.ToLocalTime().DateTime}", dimensions.X + 48 + message.Author.Username.Measure().X, dimensions.Y, Color.White, Color.Black, Vector2.Zero);

			for (int i = 0; i < lines.Length; i++)
			{
				if (lines[i].OnDraw != null) lines[i].OnDraw.Invoke(spriteBatch, new CalculatedStyle(dimensions.X + lines[i].X + 48, dimensions.Y + lines[i].Y + 24, dimensions.Width, dimensions.Height));
				else Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, lines[i].Text, dimensions.X + 48 + lines[i].X, dimensions.Y + 24 + lines[i].Y, lines[i].Color, Color.Black, Vector2.Zero);

				if (lines[i].OnHover != null)
				{
					Rectangle rect = new Rectangle((int)(dimensions.X + 48 + lines[i].X), (int)(dimensions.Y + 24 + lines[i].Y), (int)lines[i].Width, (int)lines[i].Height);
					if (rect.Contains(Main.MouseScreen)) lines[i].OnHover.Invoke(spriteBatch, dimensions);
				}
			}

			spriteBatch.SetupForShader(DTT.circleShader);

			spriteBatch.Draw(avatar, new Rectangle((int)dimensions.X, (int)dimensions.Y, 40, 40), Color.White);

			spriteBatch.DisableScissor();
		}
	}
}