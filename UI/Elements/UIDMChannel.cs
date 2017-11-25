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
	public class UIDMChannel : BaseElement
	{
		public DiscordDmChannel channel;
		public Texture2D texture;
		public Color color = Color.White;

		public UIDMChannel(DiscordDmChannel channel)
		{
			this.channel = channel;
			texture = DTT.defaultIcon;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();
			CalculatedStyle gridDim = DTT.Instance.MainUI.gridPMs.GetDimensions();

			RasterizerState state = new RasterizerState { ScissorTestEnable = true };

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, null, state, null, Main.UIScaleMatrix);
			Rectangle prevRect = spriteBatch.GraphicsDevice.ScissorRectangle;
			spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle((int)gridDim.X, (int)gridDim.Y - 6, Main.screenWidth - (int)gridDim.X, (int)gridDim.Height + 12);

			if (IsMouseHovering)
			{
				string name = channel.Name ?? channel.Recipients[0].Username;
				Rectangle rect = new Rectangle((int)(dimensions.X + dimensions.Width + 8), (int)(dimensions.Y + dimensions.Height / 2f - 16), (int)(name.Measure().X + 16), 32);

				spriteBatch.DrawPanel(rect, BaseLib.Utility.Utility.backgroundTexture, BaseUI.panelColor);
				spriteBatch.DrawPanel(rect, BaseLib.Utility.Utility.borderTexture, Color.Black);

				Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, name, rect.X + 8, rect.Y + 6, color, Color.Black, Vector2.Zero);
			}

			spriteBatch.SetupForShader(DTT.circleShader);

			spriteBatch.Draw(texture, new Rectangle((int)dimensions.X, (int)dimensions.Y, (int)dimensions.Height, (int)dimensions.Height), Color.White);

			spriteBatch.SetupForShader(DTT.activityShader);

			Vector4 activity = channel.Recipients[0].Presence.PresenceColor().ToVector4();
			DTT.activityShader.Parameters["drawColor"].SetValue(activity);

			int size = (int)(dimensions.Height / 4f);
			spriteBatch.Draw(Main.magicPixel, new Rectangle((int)(dimensions.X + dimensions.Height - size), (int)(dimensions.Y + dimensions.Height - size), size, size), Color.White);

			spriteBatch.GraphicsDevice.ScissorRectangle = prevRect;
			spriteBatch.DisableScissor();
		}
	}
}