using NamedPipeWrapper;
using System;
using System.Diagnostics;
using Terraria;
using Terraria.ModLoader;

namespace DTT
{
	public class DTT : Mod
	{
		public static DTT Instance;

		public DTT()
		{
			Properties = new ModProperties
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};

			bot = new Process
			{
				StartInfo =
				{
					FileName = "C:\\Development\\Apps\\Discord\\TestBot\\TestBot\\bin\\Debug\\TestBot.exe",
					UseShellExecute = false
				}
			};
		}

		public override void PostUpdateInput()
		{
			if (server != null)
			{
				//PlayerInput.WritingText = true;
				//Main.instance.HandleIME();

				//string newString = Main.GetInputText("");
				//server.PushMessage(newString);
			}
		}

		private NamedPipeServer<string> server;
		private Process bot;

		public void InitComms()
		{
			server = new NamedPipeServer<string>("DTTPipe");

			server.ClientConnected += delegate (NamedPipeConnection<string, string> conn)
			{
				ErrorLogger.Log($"Bot [{conn.Id}] connected");
				conn.PushMessage("Estabilished a link with Terraria");
			};
			server.ClientDisconnected += delegate (NamedPipeConnection<string, string> conn)
			{
				ErrorLogger.Log($"Bot [{conn.Id}] disconnected");
			};

			server.ClientMessage += delegate (NamedPipeConnection<string, string> conn, string message)
			{
				if (!Main.gameMenu) Main.NewTextMultiline(message);
			};

			server.Start();
			bot.Start();
		}

		public override void Load()
		{
			ErrorLogger.ClearLog();

			Instance = this;

			Main.instance.Exiting += Instance_Exiting;

			InitComms();
		}

		public override void Unload()
		{
			Main.instance.Exiting -= Instance_Exiting;

			Instance = null;

			GC.Collect();
		}

		private void Instance_Exiting(object sender, EventArgs e)
		{
			if (!bot.HasExited) bot.Kill();

			if (server != null)
			{
				foreach (NamedPipeConnection<string, string> connection in server._connections) connection.Close();
				server.Stop();
			}
		}
	}
}