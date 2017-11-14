using BaseLib.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics;
using Terraria.UI;

namespace DTT.UI.Elements
{
	public class UIScrollbarReversed : BaseElement
	{
		private float viewPosition;
		private float viewSize = 1f;
		private float maxViewSize = 20f;
		private bool isDragging;
		private bool isHoveringOverHandle;
		private float dragXOffset;
		private Texture2D texture;
		private Texture2D innerTexture;

		public float ViewPosition
		{
			get { return viewPosition; }
			set { viewPosition = MathHelper.Clamp(value, 0f, maxViewSize - viewSize); }
		}

		public UIScrollbarReversed()
		{
			Width.Set(20f, 0f);
			MaxWidth.Set(20f, 0f);
			texture = TextureManager.Load("Images/UI/Scrollbar");
			innerTexture = TextureManager.Load("Images/UI/ScrollbarInner");
			PaddingTop = 5f;
			PaddingBottom = 5f;
		}

		public void SetView(float viewSize, float maxViewSize)
		{
			viewSize = MathHelper.Clamp(viewSize, 0f, maxViewSize);
			viewPosition = MathHelper.Clamp(viewPosition, 0f, maxViewSize - viewSize);
			this.viewSize = viewSize;
			this.maxViewSize = maxViewSize;
		}

		public float GetValue() => viewPosition;

		private Rectangle GetHandleRectangle()
		{
			CalculatedStyle innerDimensions = GetInnerDimensions();
			if (maxViewSize == 0f && viewSize == 0f)
			{
				viewSize = 1f;
				maxViewSize = 1f;
			}

			return new Rectangle(
				(int)innerDimensions.X,
				(int)(innerDimensions.Y + innerDimensions.Height - innerDimensions.Height * (viewSize / maxViewSize) - innerDimensions.Height * (viewPosition / maxViewSize) - 3),
				20,
				(int)(innerDimensions.Height * (viewSize / maxViewSize)) + 7);
		}

		private void DrawBar(SpriteBatch spriteBatch, Texture2D texture, Rectangle dimensions, Color color)
		{
			spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y - 6, dimensions.Width, 6), new Rectangle(0, 0, texture.Width, 6), color);
			spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y, dimensions.Width, dimensions.Height), new Rectangle(0, 6, texture.Width, 4), color);
			spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y + dimensions.Height, dimensions.Width, 6), new Rectangle(0, texture.Height - 6, texture.Width, 6), color);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();
			CalculatedStyle innerDimensions = GetInnerDimensions();
			if (isDragging)
			{
				float num = UserInterface.ActiveInstance.MousePosition.Y - innerDimensions.Y - dragXOffset;
				viewPosition = MathHelper.Clamp(num / innerDimensions.Height * maxViewSize, 0f, maxViewSize - viewSize);
			}
			Rectangle handleRectangle = GetHandleRectangle();
			Vector2 mousePosition = UserInterface.ActiveInstance.MousePosition;
			bool isHoveringOverHandle = this.isHoveringOverHandle;
			this.isHoveringOverHandle = handleRectangle.Contains(new Point((int)mousePosition.X, (int)mousePosition.Y));
			if (!isHoveringOverHandle && this.isHoveringOverHandle && Main.hasFocus)
			{
				Main.PlaySound(12);
			}
			DrawBar(spriteBatch, texture, dimensions.ToRectangle(), Color.White);
			DrawBar(spriteBatch, innerTexture, handleRectangle, Color.White * (isDragging || this.isHoveringOverHandle ? 1f : 0.85f));
		}

		public override void MouseDown(UIMouseEvent evt)
		{
			base.MouseDown(evt);
			if (evt.Target == this)
			{
				Rectangle handleRectangle = GetHandleRectangle();
				if (handleRectangle.Contains(new Point((int)evt.MousePosition.X, (int)evt.MousePosition.Y)))
				{
					isDragging = true;
					dragXOffset = evt.MousePosition.Y - handleRectangle.Y;
					return;
				}
				CalculatedStyle innerDimensions = GetInnerDimensions();
				float num = UserInterface.ActiveInstance.MousePosition.Y - innerDimensions.Y - (handleRectangle.Height >> 1);
				viewPosition = MathHelper.Clamp(num / innerDimensions.Height * maxViewSize, 0f, maxViewSize - viewSize);
			}
		}

		public override void MouseUp(UIMouseEvent evt)
		{
			base.MouseUp(evt);
			isDragging = false;
		}
	}
}