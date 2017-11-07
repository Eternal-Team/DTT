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
		public Texture2D texture;
		private float padding;
		public Color color = Color.White;

		public UIGuild(DiscordGuild guild, float padding = 6f)
		{
			this.guild = guild;
			this.padding = padding;
			texture = DTT.defaultIcon;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();

			RasterizerState state = new RasterizerState { ScissorTestEnable = true };

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, null, state, null, Main.UIScaleMatrix);

			spriteBatch.DrawPanel(dimensions, BaseLib.Utility.Utility.backgroundTexture, BaseUI.panelColor);
			spriteBatch.DrawPanel(dimensions, BaseLib.Utility.Utility.borderTexture, Color.Black);
			Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, guild.Name, dimensions.X + dimensions.Height, dimensions.Y + dimensions.Height / 2 - 10, color, Color.Black, Vector2.Zero);

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, state, DTT.circleShader, Main.UIScaleMatrix);

			spriteBatch.Draw(texture, new Rectangle((int)(dimensions.X + padding), (int)(dimensions.Y + padding), (int)(dimensions.Height - padding * 2), (int)(dimensions.Height - padding * 2)), Color.White);

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, null, null, null, Main.UIScaleMatrix);
		}
	}
}