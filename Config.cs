using Newtonsoft.Json;
using Terraria;

namespace DTT
{
	public class Config
	{
		[JsonProperty]
		public string token = string.Empty;

		[JsonProperty]
		public string botExe = Main.SavePath + "\\DTT\\DTTBot.exe";

		[JsonProperty]
		public bool openWindow = true;
	}
}