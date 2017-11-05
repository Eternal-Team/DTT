using Newtonsoft.Json;

namespace DTT
{
	public class Config
	{
		[JsonProperty]
		public string token = string.Empty;

		[JsonProperty]
		public string botExe = DTT.Instance.SavePath + "\\DTTBot.exe";

		[JsonProperty]
		public bool openWindow = true;
	}
}