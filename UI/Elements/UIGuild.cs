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
			Rectangle prevRect = spriteBatch.GraphicsDevice.ScissorRectangle;
			spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle((int)DTT.Instance.SelectUI.gridGuilds.GetDimensions().X, (int)DTT.Instance.SelectUI.gridGuilds.GetDimensions().Y - 6, Main.screenWidth - (int)DTT.Instance.SelectUI.gridGuilds.GetDimensions().X, (int)DTT.Instance.SelectUI.gridGuilds.GetDimensions().Height + 12);

			if (IsMouseHovering)
			{
				Rectangle rect = new Rectangle((int)(dimensions.X + dimensions.Width + 8), (int)(dimensions.Y + dimensions.Height / 2f - 16), (int)(guild.Name.Measure().X + 16), 32);

				spriteBatch.DrawPanel(rect, BaseLib.Utility.Utility.backgroundTexture, BaseUI.panelColor);
				spriteBatch.DrawPanel(rect, BaseLib.Utility.Utility.borderTexture, Color.Black);

				Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, guild.Name, rect.X + 8, rect.Y + 6, color, Color.Black, Vector2.Zero);
			}

			spriteBatch.SetupForShader(DTT.circleShader);

			spriteBatch.Draw(texture, new Rectangle((int)dimensions.X, (int)dimensions.Y, (int)dimensions.Height, (int)dimensions.Height), Color.White);

			spriteBatch.GraphicsDevice.ScissorRectangle = prevRect;
			spriteBatch.DisableScissor();
		}
	}
}