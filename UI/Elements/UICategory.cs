using BaseLib.Elements;
using BaseLib.UI;
using BaseLib.Utility;
using DSharpPlus.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.UI;

namespace DTT.UI.Elements
{
	public class UICategory : BaseElement
	{
		public DiscordChannel channel;
		public bool expanded;
		public string Text => channel.Name.ToUpper();

		public List<UIElement> items = new List<UIElement>();

		public UICategory(DiscordChannel channel)
		{
			this.channel = channel;
		}

		public void Expand()
		{
			expanded = !expanded;
			RecalculateChildren();

			if (expanded) Height.Pixels += items.Count * 44;
			else Height.Pixels -= items.Count * 44;

			Recalculate();
		}

		public override void Click(UIMouseEvent evt)
		{
			CalculatedStyle dimensions = GetDimensions();
			Rectangle hitbox = new CalculatedStyle(dimensions.X, dimensions.Y, dimensions.Width, 40).ToRectangle();

			if (hitbox.Contains(evt.MousePosition))
			{
				Main.PlaySound(SoundID.MenuTick);
				
				Expand();
				DTT.Instance.SelectUI.gridSelect.RecalculateChildren();
			}
			else
			{
				Main.PlaySound(SoundID.MenuTick);
				
				UIChannel child = (UIChannel)items.FirstOrDefault(x => x.GetDimensions().ToRectangle().Contains(evt.MousePosition));
				if (child != null) Main.NewText(child.channel.Name);
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();
			CalculatedStyle drawDim = new CalculatedStyle(dimensions.X, dimensions.Y, dimensions.Width, 40);

			spriteBatch.DrawPanel(drawDim, BaseLib.Utility.Utility.backgroundTexture, BaseUI.panelColor);
			spriteBatch.DrawPanel(drawDim, BaseLib.Utility.Utility.borderTexture, Color.Black);

			Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, Text, drawDim.X + 8, drawDim.Y + drawDim.Height / 2 - 10f, Color.White, Color.Black, Vector2.Zero);

			if (expanded)
			{
				for (int i = 0; i < items.Count; i++)
				{
					UIElement el = items[i];
					el.Width.Set(dimensions.Width - 8, 0);
					el.Left.Set(dimensions.X + 8, 0);
					el.Top.Set(drawDim.Y + 44 + i * 44, 0);
					el.Recalculate();
					el.Draw(spriteBatch);
				}
			}
		}
	}
}