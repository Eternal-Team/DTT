using BaseLib.Elements;
using BaseLib.Utility;
using DSharpPlus.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace DTT.UI.Elements
{
	public class UIMessage : BaseElement
	{
		public DiscordMessage message;
		public Texture2D avatar;

		public UIMessage(DiscordMessage message)
		{
			this.message = message;
			avatar = DTT.defaultIcon;
		}

		public override int CompareTo(object obj)
		{
			UIMessage other = obj as UIMessage;
			return other != null ? message.CreationTimestamp.CompareTo(other.message.CreationTimestamp) : 0;
		}

		private Snippet[] lines;
		public void RecalculateMessage()
		{
			Recalculate();
			lines = message.Content.SplitToLines(GetDimensions().Width - 48).ToArray();
			foreach (Snippet snippet in lines) ErrorLogger.Log(snippet.ToString());
			float height = lines.Last().Y + lines.Last().Height + 24;
			if (height < 40) height = avatar.Height;
			Height.Set(height, 0);
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			Vector2 mousePosRel = evt.MousePosition - GetDimensions().Position();
			if (lines.Any(x => x.ToRectangle().Contains(mousePosRel)))
			{
				Snippet hover = lines.FirstOrDefault(x => x.ToRectangle().Contains(mousePosRel));
				hover.OnHover.Invoke(hover.ID);
			}
		}

		private string nameOverride;
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();

			spriteBatch.EnableScissor();

			Color color = Color.White;
			if (message.Channel.Guild.Members.Any(x => x.Id == message.Author.Id)) color = message.Channel.Guild.Members.First(x => x.Id == message.Author.Id).Color.Value.FromInt();

			//if (message.Author.Presence.Guild != null)
			//{
			//	if (message.Author.Presence.Guild.Id == DTT.Instance.currentGuild.Id)
			//	{
			//		Task<DiscordMember> t = message.Author.Presence.Guild.GetMemberAsync(message.Author.Id);
			//		t.ContinueWith(x => nameOverride = t.Result.Nickname ?? t.Result.Username);
			//	}
			//}

			Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, nameOverride ?? message.Author.Username, dimensions.X + 48, dimensions.Y, color, Color.Black, Vector2.Zero);
			Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, $" - {message.CreationTimestamp.ToLocalTime().DateTime}", dimensions.X + 48 + message.Author.Username.Measure().X, dimensions.Y, Color.White, Color.Black, Vector2.Zero);

			for (int i = 0; i < lines.Length; i++)
			{
				Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, lines[i].Text, dimensions.X + 48 + lines[i].X, dimensions.Y + 24 + lines[i].Y, Color.White, Color.Black, Vector2.Zero);
			}

			spriteBatch.SetupForShader(DTT.circleShader);

			spriteBatch.Draw(avatar, new Rectangle((int)dimensions.X, (int)dimensions.Y, 40, 40), Color.White);

			spriteBatch.DisableScissor();
		}
	}
}