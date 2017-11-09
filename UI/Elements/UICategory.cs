using BaseLib.Elements;
using BaseLib.UI;
using BaseLib.Utility;
using DSharpPlus.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.UI;

namespace DTT.UI.Elements
{
	public class UICategory : BaseElement
	{
		private class UIInnerList : BaseElement
		{
			public override bool ContainsPoint(Vector2 point) => true;

			protected override void DrawChildren(SpriteBatch spriteBatch)
			{
				Vector2 position = Parent.GetDimensions().Position();
				Vector2 dimensions = new Vector2(Parent.GetDimensions().Width, Parent.GetDimensions().Height);
				foreach (UIElement current in Elements)
				{
					Vector2 position2 = current.GetDimensions().Position();
					Vector2 dimensions2 = new Vector2(current.GetDimensions().Width, current.GetDimensions().Height);
					if (Collision.CheckAABBvAABBCollision(position, dimensions, position2, dimensions2)) current.Draw(spriteBatch);
				}
			}
		}

		public DiscordChannel category;
		public bool expanded;
		public string Text => category.Name.ToUpper();

		public List<UIElement> items = new List<UIElement>();

		public UIElement list = new UIInnerList();

		public UICategory(DiscordChannel category)
		{
			this.category = category;

			list.Width.Set(0f, 1f);
			list.Height.Set(0f, 1f);
			list.Top.Pixels = 44;
			Append(list);

			foreach (DiscordChannel channel in category.Children)
			{
				if (channel.CanJoin())
				{
					UIChannel uiChild = new UIChannel(channel);
					uiChild.Width.Set(-8, 1);
					uiChild.Height.Pixels = 40;
					uiChild.color = DTT.Instance.currentChannel.Id == channel.Id ? Color.Lime : Color.White;
					uiChild.OnClick += (a, b) =>
					{
						DTT.Instance.currentChannel = channel;

						DTT.Instance.SelectUI.gridSelect.items.ForEach(x =>
						{
							if (x is UIChannel) ((UIChannel)x).color = Color.White;
							else (x as UICategory)?.items.ForEach(y => ((UIChannel)y).color = Color.White);
						});
						uiChild.color = Color.Lime;

						DTT.log.Clear();
						Task<IReadOnlyList<DiscordMessage>> task = channel.GetMessagesAsync(50);
						task.ContinueWith(t => DTT.log.AddRange(t.Result.ToList()));
					};
					Add(uiChild);
				}
			}
		}

		public void Add(UIElement item)
		{
			item.Recalculate();
			items.Add(item);
			list.Append(item);
			list.Recalculate();
		}

		public void Expand()
		{
			expanded = !expanded;
			RecalculateChildren();
			list.Recalculate();

			if (expanded)
			{
				Height.Pixels += height;
				list.Height.Pixels += height;
			}
			else
			{
				Height.Pixels -= height;
				list.Height.Pixels -= height;
			}

			list.Recalculate();
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
		}

		private float height;
		public override void RecalculateChildren()
		{
			base.RecalculateChildren();
			float top = 0f;
			for (int i = 0; i < items.Count; i++)
			{
				items[i].Top.Set(top, 0f);
				items[i].Left.Set(0f, 0f);
				items[i].Recalculate();
				top += items[i].GetOuterDimensions().Height + 4f;
			}

			height = items.Sum(x => x.GetDimensions().Height + 4f);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.EnableScissor();

			RecalculateChildren();
			Recalculate();
			DrawSelf(spriteBatch);
			if (expanded) typeof(UIInnerList).InvokeMethod<object>("DrawChildren", new object[] { spriteBatch }, list);

			spriteBatch.DisableScissor();
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();
			CalculatedStyle drawDim = new CalculatedStyle(dimensions.X, dimensions.Y, dimensions.Width, 40);

			spriteBatch.DrawPanel(drawDim, BaseLib.Utility.Utility.backgroundTexture, BaseUI.panelColor);
			spriteBatch.DrawPanel(drawDim, BaseLib.Utility.Utility.borderTexture, Color.Black);

			Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, Text, drawDim.X + 8, drawDim.Y + drawDim.Height / 2 - 10f, Color.White, Color.Black, Vector2.Zero);
		}
	}
}