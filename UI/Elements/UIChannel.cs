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
	public class UIChannel : BaseElement
	{
		public DiscordChannel channel;
		private float padding;
		public Color color = Color.White;

		public UIChannel(DiscordChannel channel, float padding = 6f)
		{
			this.channel = channel;
			this.padding = padding;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();

			spriteBatch.DrawPanel(dimensions, BaseLib.Utility.Utility.backgroundTexture, BaseUI.panelColor);
			spriteBatch.DrawPanel(dimensions, BaseLib.Utility.Utility.borderTexture, Color.Black);

			Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, "#" + channel.Name, dimensions.X + 8, dimensions.Y + dimensions.Height / 2 - 10, color, Color.Black, Vector2.Zero);
		}
	}
}