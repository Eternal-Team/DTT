using Newtonsoft.Json;

namespace DTT
{
	public class Config
	{
		[JsonProperty] public string token = string.Empty;
		[JsonProperty] public ulong? defaultGuildID;
		[JsonProperty] public ulong? defaultChannelID;
	}
}