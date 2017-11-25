using BaseLib.Elements;
using BaseLib.Utility;
using DSharpPlus.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
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
		public float height;
		public DiscordMember member;
		public void RecalculateMessage()
		{
			lines = message.FormatMessage(DTT.Instance.MainUI.gridMessages.GetDimensions().Width - 100).ToArray();

			height = lines.Last().Y + lines.Last().Height;
			if (height < 40) height = 40;
			Height.Set(height, 0);
			Recalculate();

			if (message.Author.Presence != null && message.Author.Presence.Guild != null && message.Author.Presence.Guild.Id == DTT.Instance.currentGuild.Id) member = message.Author.Presence.Guild.Members.First(x => x.Id == message.Author.Id);
		}

		public override void RightClick(UIMouseEvent evt)
		{
			base.RightClick(evt);

			DTT.UI.OpenEditPanel(this);
		}

		public override void Click(UIMouseEvent evt)
		{
			base.Click(evt);

			CalculatedStyle dimensions = GetDimensions();

			for (int i = 0; i < lines.Length; i++)
			{
				Rectangle rect = new Rectangle((int)(dimensions.X + 48 + lines[i].X), (int)(dimensions.Y + lines[i].Y), (int)lines[i].Width, (int)lines[i].Height);
				if (rect.Contains(evt.MousePosition) && lines[i].OnClick != null) lines[i].OnClick.Invoke();
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();

			spriteBatch.EnableScissor();

			for (int i = 0; i < lines.Length; i++)
			{
				if (lines[i].OnDraw != null) lines[i].OnDraw.Invoke(spriteBatch, new CalculatedStyle(dimensions.X + lines[i].X + 48, dimensions.Y + lines[i].Y, dimensions.Width, dimensions.Height));
				else Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, lines[i].Text, dimensions.X + 48 + lines[i].X, dimensions.Y + lines[i].Y, lines[i].Color, Color.Black, Vector2.Zero, lines[i].Scale);

				if (lines[i].OnHover != null)
				{
					Rectangle rect = new Rectangle((int)(dimensions.X + 48 + lines[i].X), (int)(dimensions.Y + lines[i].Y), (int)lines[i].Width, (int)lines[i].Height);
					if (rect.Intersects(dimensions.ToRectangle()) && rect.Contains(Main.MouseScreen)) lines[i].OnHover.Invoke(spriteBatch, dimensions);
				}
			}

			spriteBatch.SetupForShader(DTT.circleShader);

			spriteBatch.Draw(avatar, new Rectangle((int)dimensions.X, (int)dimensions.Y, 40, 40), Color.White);

			spriteBatch.DisableScissor();
		}
	}
}