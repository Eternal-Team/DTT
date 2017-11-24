using BaseLib.Elements;
using BaseLib.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.UI;

namespace DTT.UI.Elements
{
	public class UIChat : BaseElement
	{
		public delegate bool ElementSearchMethod(UIElement element);

		public class UIInnerList : BaseElement
		{
			public override bool ContainsPoint(Vector2 point) => true;

			protected override void DrawChildren(SpriteBatch spriteBatch)
			{
				Vector2 position = Parent.GetDimensions().Position();
				Vector2 dimensions = new Vector2(Parent.GetDimensions().Width, Parent.GetDimensions().Height);
				for (int i = 0; i < Elements.Count; i++)
				{
					UIElement current = Elements[i];
					Vector2 position2 = current.GetDimensions().Position();
					Vector2 dimensions2 = new Vector2(current.GetDimensions().Width, ((UIMessage)current).height);
					if (Collision.CheckAABBvAABBCollision(position, dimensions, position2, dimensions2)) current.Draw(spriteBatch);
				}
			}
		}

		public List<UIElement> items = new List<UIElement>();
		protected UIScrollbarReversed scrollbar;
		internal UIElement innerList = new UIInnerList();
		private float innerListHeight;
		public float ListPadding = 4f;

		public int Count => items.Count;

		public UIChat()
		{
			innerList.Width.Set(0f, 1f);
			innerList.Height.Set(0f, 1f);
			Append(innerList);
		}

		public float GetTotalHeight() => innerListHeight;

		public void Goto(ElementSearchMethod searchMethod, bool center = false)
		{
			for (int i = 0; i < items.Count; i++)
			{
				if (searchMethod(items[i]))
				{
					scrollbar.ViewPosition = items[i].Top.Pixels;
					if (center) scrollbar.ViewPosition = items[i].Top.Pixels - GetInnerDimensions().Height / 2 + items[i].GetOuterDimensions().Height / 2;
					return;
				}
			}
		}

		public override void Update(GameTime gameTime)
		{
			for (int i = 0; i < items.Count; i++) items[i].Update(gameTime);

			base.Update(gameTime);
		}

		public virtual void Add(UIElement item)
		{
			item.Recalculate();
			items.Add(item);
			innerList.Append(item);
			UpdateOrder();
			innerList.Recalculate();
		}

		public virtual void Edit(UIElement item, int index)
		{
			item.Recalculate();
			items[index] = item;
			RecalculateChildren();
			typeof(UIInnerList).GetField<List<UIElement>>("Elements", innerList).RemoveAt(index);
			innerList.Append(item);
			innerList.RecalculateChildren();
			UpdateOrder();
			innerList.Recalculate();
			Main.NewText(item.GetDimensions().ToRectangle());
		}

		public virtual bool Remove(UIElement item)
		{
			innerList.RemoveChild(item);
			UpdateOrder();
			return items.Remove(item);
		}

		public virtual void Clear()
		{
			innerList.RemoveAllChildren();
			items.Clear();
		}

		public override void Recalculate()
		{
			base.Recalculate();

			innerList.Recalculate();
			UpdateScrollbar();
		}

		private float target;
		private void Lerp()
		{
			if (scrollbar != null)
			{
				if (Math.Abs(scrollbar.ViewPosition - target) > 16.0)
				{
					if (target < scrollbar.ViewPosition) scrollbar.ViewPosition -= 16;
					else scrollbar.ViewPosition += 16;
				}
			}
		}

		public override void ScrollWheel(UIScrollWheelEvent evt)
		{
			base.ScrollWheel(evt);
			if (scrollbar != null) target = scrollbar.ViewPosition + evt.ScrollWheelValue;
		}

		public override void RecalculateChildren()
		{
			base.RecalculateChildren();
			innerListHeight = items.Select(x => ((UIMessage)x).height + ListPadding).Sum();
			float top = -innerListHeight + GetDimensions().Height;

			for (int i = 0; i < items.Count; i++)
			{
				items[i].Top.Set(top, 0f);
				items[i].Left.Set(0f, 0f);
				items[i].Recalculate();
				top += ((UIMessage)items[i]).height + ListPadding;
			}
		}

		private void UpdateScrollbar()
		{
			scrollbar?.SetView(GetInnerDimensions().Height, innerListHeight);
		}

		public void SetScrollbar(UIScrollbarReversed scrollbar)
		{
			this.scrollbar = scrollbar;
			UpdateScrollbar();
		}

		public void RemoveScrollbar()
		{
			scrollbar = null;
		}

		public void UpdateOrder()
		{
			items.Sort(SortMethod);
			UpdateScrollbar();
		}

		public int SortMethod(UIElement item1, UIElement item2) => item1.CompareTo(item2);

		public override List<SnapPoint> GetSnapPoints()
		{
			List<SnapPoint> list = new List<SnapPoint>();
			SnapPoint item;
			if (GetSnapPoint(out item)) list.Add(item);
			foreach (UIElement current in items) list.AddRange(current.GetSnapPoints());
			return list;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.EnableScissor();

			Rectangle prevRect = spriteBatch.GraphicsDevice.ScissorRectangle;
			spriteBatch.GraphicsDevice.ScissorRectangle = GetInnerDimensions().ToRectangle();

			Lerp();
			if (scrollbar != null) innerList.Top.Set(scrollbar.GetValue(), 0f);
			RecalculateChildren();
			Recalculate();

			base.DrawSelf(spriteBatch);
			typeof(UIInnerList).InvokeMethod<object>("DrawChildren", new object[] { spriteBatch }, innerList);

			spriteBatch.GraphicsDevice.ScissorRectangle = prevRect;
			spriteBatch.DisableScissor();
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (scrollbar != null) innerList.Top.Set(scrollbar.GetValue(), 0f);
			Recalculate();

			base.DrawSelf(spriteBatch);
		}
	}
}