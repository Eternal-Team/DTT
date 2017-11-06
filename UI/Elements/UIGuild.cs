using BaseLib.Elements;
using BaseLib.UI;
using BaseLib.Utility;
using DSharpPlus.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace DTT.UI.Elements
{
	public class UIGuild : BaseElement
	{
		public DiscordGuild guild;
		private Texture2D texture;
		private float padding;
		public Color color = Color.White;

		public UIGuild(DiscordGuild guild, float padding = 6f)
		{
			this.guild = guild;
			this.padding = padding;
			texture = DTT.guilds[guild];
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();

			spriteBatch.DrawPanel(dimensions, BaseLib.Utility.Utility.backgroundTexture, BaseUI.panelColor);
			spriteBatch.DrawPanel(dimensions, BaseLib.Utility.Utility.borderTexture, Color.Black);

			spriteBatch.Draw(texture, new Rectangle((int)(dimensions.X + padding), (int)(dimensions.Y + padding), (int)(dimensions.Height - padding * 2), (int)(dimensions.Height - padding * 2)), Color.White);

			Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, guild.Name, dimensions.X + dimensions.Height, dimensions.Y + dimensions.Height / 2 - 10, color, Color.Black, Vector2.Zero);
		}
	}
}