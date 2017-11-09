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

			Main.NewText(message.Channel.Guild.CurrentMember.Roles.Select(x =>
			{
				int r = (x.Color.Value & 0xff0000) >> 16;
				int g = (x.Color.Value & 0xff00) >> 8;
				int b = (x.Color.Value & 0xff);
				Color color = new Color(r, g, b);

				return $"[{x.Name}] {color}";
			}).Aggregate((x, y) => x + ", " + y));
			Main.NewText(message.Content);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();

			spriteBatch.EnableScissor();

			int role = message.Channel.Guild.Members.First(x => x.Id == message.Author.Id).Color.Value;
			int r = (role & 0xff0000) >> 16;
			int g = (role & 0xff00) >> 8;
			int b = role & 0xff;

			Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, message.Author.Username, dimensions.X + 48, dimensions.Y, new Color(r, g, b), Color.Black, Vector2.Zero);

			spriteBatch.SetupForShader(DTT.circleShader);

			spriteBatch.Draw(avatar, new Rectangle((int)dimensions.X, (int)dimensions.Y, 40, 40), Color.White);

			spriteBatch.DisableScissor();
		}
	}
}