using BaseLib.Elements;
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
		public Color color = Color.White;
		public bool firstClick = true;

		public UIChannel(DiscordChannel channel)
		{
			this.channel = channel;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();

			//spriteBatch.DrawPanel(dimensions, BaseLib.Utility.Utility.backgroundTexture, BaseUI.panelColor);
			//spriteBatch.DrawPanel(dimensions, BaseLib.Utility.Utility.borderTexture, Color.Black);

			Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, "#" + channel.Name, dimensions.X, dimensions.Y, color, Color.Black, Vector2.Zero);
		}
	}
}