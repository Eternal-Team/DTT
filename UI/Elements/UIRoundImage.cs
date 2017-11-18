using BaseLib.Elements;
using BaseLib.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace DTT.UI.Elements
{
	public class UIRoundImage : BaseElement
	{
		public Texture2D texture;

		public UIRoundImage(Texture2D texture)
		{
			this.texture = texture;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (visible && texture != null)
			{
				CalculatedStyle dimensions = GetDimensions();

				spriteBatch.SetupForShader(DTT.circleShader);

				spriteBatch.Draw(texture, new Rectangle((int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height), Color.White);

				spriteBatch.DisableScissor();
			}
		}
	}
}