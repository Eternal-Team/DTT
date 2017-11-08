using BaseLib.Elements;
using BaseLib.Utility;
using DSharpPlus.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace DTT.UI.Elements
{
	public class UIUser : BaseElement
	{
		public DiscordUser user;
		public Texture2D texture;
		public Color color = Color.White;

		public UIUser(DiscordUser user)
		{
			this.user = user;
			texture = DTT.defaultIcon;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (user != null)
			{
				CalculatedStyle dimensions = GetDimensions();

				spriteBatch.EnableScissor();

				Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, user.Username, dimensions.X + dimensions.Height + 8, dimensions.Y + dimensions.Height / 2 - 10, color, Color.Black, Vector2.Zero);

				spriteBatch.SetupForShader(DTT.circleShader);

				spriteBatch.Draw(texture, new Rectangle((int)dimensions.X, (int)dimensions.Y, (int)dimensions.Height, (int)dimensions.Height), Color.White);

				RasterizerState state = new RasterizerState { ScissorTestEnable = true };

				spriteBatch.End();

				Vector4 activity = user.Presence.PresenceColor().ToVector4();
				DTT.activityShader.Parameters["drawColor"].SetValue(activity);
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, state, DTT.activityShader, Main.UIScaleMatrix);

				int size = (int)(dimensions.Height / 4f);
				spriteBatch.Draw(Main.magicPixel, new Rectangle((int)(dimensions.X + dimensions.Height - size), (int)(dimensions.Y + dimensions.Height - size), size, size), Color.White);

				spriteBatch.DisableScissor();
			}
		}
	}
}