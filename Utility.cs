﻿using BaseLib.Utility;
using DSharpPlus;
using DSharpPlus.Entities;
using DTT.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Svg;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace DTT
{
	public static partial class Utility
	{
		public static readonly Dictionary<string, string> emojis = new Dictionary<string, string>
		{
			{"\\uD83D\\uDE00", "https://discordapp.com/assets/5c04ac2b97de83c767c22cb0028544ee.svg"},
			{"\\uD83D\\uDE2C", "https://discordapp.com/assets/be0923fd964bff1a6ea77c14fe227a63.svg"},
			{"\\uD83D\\uDE01", "https://discordapp.com/assets/b51b374967740c81d8681d7a6cb4218d.svg"},
			{"\\uD83D\\uDE02", "https://discordapp.com/assets/cae9e3b02af6e987442df2953de026fc.svg"},
			{"\\uD83D\\uDE03", "https://discordapp.com/assets/b731b88b6459090c02b8d1e31a552c5a.svg"},
			{"\\uD83D\\uDE04", "https://discordapp.com/assets/f0835a46b501ae0a182874b003fdbb65.svg"},
			{"\\uD83D\\uDE05", "https://discordapp.com/assets/d56fc4f12b790c6cef7b08a515e4cce9.svg"},
			{"\\uD83D\\uDE06", "https://discordapp.com/assets/add1f87676ce1d709db3efd005873142.svg"},
			{"\\uD83D\\uDE07", "https://discordapp.com/assets/4a2841a4746acdf4d391b4fd497e540b.svg"},
			{"\\uD83D\\uDE09", "https://discordapp.com/assets/b277c5ffb43011a356200198cf76b22d.svg"},
			{"\\uD83D\\uDE0A", "https://discordapp.com/assets/c7631d09241c31bb0e357ba4c877d726.svg"},
			{"\\uD83D\\uDE42", "https://discordapp.com/assets/4f22736614151ae463b63a5a78aac9d9.svg"},
			{"\\uD83D\\uDE43", "https://discordapp.com/assets/96b458e549758981dba74c32137c0784.svg"},
			{"\\u263A", "https://discordapp.com/assets/81be662a9bb9f2cfa8b7c8ac2f9dfcd6.svg"},
			{"\\uD83D\\uDE0B", "https://discordapp.com/assets/d51bbfccef4b12d5441ca23da921c604.svg"},
			{"\\uD83D\\uDE0C", "https://discordapp.com/assets/e9b114282c887f219e297d6cc7249363.svg"},
			{"\\uD83D\\uDE0D", "https://discordapp.com/assets/7e4f6dcf32845bfa865cf17491faf867.svg"},
			{"\\uD83D\\uDE18", "https://discordapp.com/assets/752d516f9363ed1a2ea60eace20ff801.svg"},
			{"\\uD83D\\uDE17", "https://discordapp.com/assets/4462ff23c2ca17710c52e92d1ea000a3.svg"},
			{"\\uD83D\\uDE19", "https://discordapp.com/assets/d54e2cf917ed7cf7e736a3f47dfe1345.svg"},
			{"\\uD83D\\uDE1A", "https://discordapp.com/assets/d1fa903a61500ee1c3d43d3751ceff8a.svg"},
			{"\\uD83D\\uDE1C", "https://discordapp.com/assets/eb3301ec88dae3fbc96c83bfe34dbdde.svg"},
			{"\\uD83D\\uDE1D", "https://discordapp.com/assets/c50ae461ee0618f7f64c481248468a22.svg"},
			{"\\uD83D\\uDE1B", "https://discordapp.com/assets/597c2af9a2b16473aa5d80dc46f2e112.svg"},
			{"\\uD83E\\uDD11", "https://discordapp.com/assets/5cdb67d23b259628f475e663ef9907e7.svg"},
			{"\\uD83E\\uDD13", "https://discordapp.com/assets/e694b29603bee9f93dea4cad64502a38.svg"},
			{"\\uD83D\\uDE0E", "https://discordapp.com/assets/d0df7bf4acd843defa4e417cf767a574.svg"},
			{"\\uD83E\\uDD17", "https://discordapp.com/assets/a3fb4592346733e0ef665ea7385c2721.svg"},
			{"\\uD83D\\uDE0F", "https://discordapp.com/assets/1b6c783f128fe9fa93aee4d32a7013d6.svg"},
			{"\\uD83D\\uDE36", "https://discordapp.com/assets/0c447d7b6e88284741bb9a70f213a17b.svg"},
			{"\\uD83D\\uDE10", "https://discordapp.com/assets/2c6041bfc91ee1174f11740dc26573fe.svg"},
			{"\\uD83D\\uDE11", "https://discordapp.com/assets/07ede26f668b74a5fbeefff6eb35e15e.svg"},
			{"\\uD83D\\uDE12", "https://discordapp.com/assets/76292b41a5fa5408d92f674ebf4b7326.svg"},
			{"\\uD83D\\uDE44", "https://discordapp.com/assets/df108c82f499b630411d1fc6594f3717.svg"},
			{"\\uD83E\\uDD14", "https://discordapp.com/assets/53ef346458017da2062aca5c7955946b.svg"},
			{"\\uD83D\\uDE33", "https://discordapp.com/assets/737302f9d68a5a14f95ea1beb1b198d6.svg"},
			{"\\uD83D\\uDE1E", "https://discordapp.com/assets/b180ba4b2cacbdaeb91885ef22c6bf21.svg"},
			{"\\uD83D\\uDE1F", "https://discordapp.com/assets/468d61fd9fd55d3f5d905005d2180daa.svg"},
			{"\\uD83D\\uDE20", "https://discordapp.com/assets/65bd38c1796f4959df4028fdf06aaf8f.svg"},
			{"\\uD83D\\uDE21", "https://discordapp.com/assets/10d135bf11670b6db1db682a512da004.svg"},
			{"\\uD83D\\uDE14", "https://discordapp.com/assets/f1f76882104c8724124954b6edfed6d4.svg"},
			{"\\uD83D\\uDE15", "https://discordapp.com/assets/b78035b8e2a6a4885d4448198963a14e.svg"},
			{"\\uD83D\\uDE41", "https://discordapp.com/assets/3b32193b9673582d2704e53ec1056b6e.svg"},
			{"\\u2639", "https://discordapp.com/assets/b61c4e14e90e796e36f0d10792fcc505.svg"},
			{"\\uD83D\\uDE23", "https://discordapp.com/assets/2fe6cd31e65e7a614dce24755303878b.svg"},
			{"\\uD83D\\uDE16", "https://discordapp.com/assets/05e137fbeb8e924737f4cb21e974aaa2.svg"},
			{"\\uD83D\\uDE2B", "https://discordapp.com/assets/cf4c66d6a78fdb1a6a77dc434c7d5eb2.svg"},
			{"\\uD83D\\uDE29", "https://discordapp.com/assets/2e1d6b723adec95eaa2a500141cf136d.svg"},
			{"\\uD83D\\uDE24", "https://discordapp.com/assets/17ce9728ad8efb8ffe2fa41f60c169be.svg"},
			{"\\uD83D\\uDE2E", "https://discordapp.com/assets/0dc84c65dd1003af7a7f9c29e5be0da0.svg"},
			{"\\uD83D\\uDE31", "https://discordapp.com/assets/9bd8b85559466379744360f8c9841f39.svg"},
			{"\\uD83D\\uDE28", "https://discordapp.com/assets/5f730d3f468c0b45ad924ebf061b0ad2.svg"},
			{"\\uD83D\\uDE30", "https://discordapp.com/assets/296af87a9a3b362dd6cce3b4afaaa1de.svg"},
			{"\\uD83D\\uDE2F", "https://discordapp.com/assets/cad1882ca3eeb04e786bc5d63e44477d.svg"},
			{"\\uD83D\\uDE26", "https://discordapp.com/assets/f71a48ebe4ebb6c0fb771721248d7523.svg"},
			{"\\uD83D\\uDE27", "https://discordapp.com/assets/f90a4ddd5d612bd89c0abe44c39fa4df.svg"},
			{"\\uD83D\\uDE22", "https://discordapp.com/assets/2a6e66e7de157c4051fb7abf7d8b0063.svg"},
			{"\\uD83D\\uDE25", "https://discordapp.com/assets/744e294f94042d53a35348ddc46747b5.svg"},
			{"\\uD83D\\uDE2A", "https://discordapp.com/assets/e301ba4fec009e9442b7016329d605e7.svg"},
			{"\\uD83D\\uDE13", "https://discordapp.com/assets/0702847ec6fe5542f0829e09e0c5bb22.svg"},
			{"\\uD83D\\uDE2D", "https://discordapp.com/assets/4dc13fd52f691020a1308c5b6cbc6f49.svg"},
			{"\\uD83D\\uDE35", "https://discordapp.com/assets/fc9afde78a605834bf01cedfcdad0b32.svg"},
			{"\\uD83D\\uDE32", "https://discordapp.com/assets/948071d5928127731164ba265b4c4734.svg"},
			{"\\uD83E\\uDD10", "https://discordapp.com/assets/da16c90202a239aff1212e3a8f57bc3e.svg"},
			{"\\uD83D\\uDE37", "https://discordapp.com/assets/e4f2f57f08a4e4a48546f5c4e21c9a77.svg"},
			{"\\uD83E\\uDD12", "https://discordapp.com/assets/ef589b423dfa7ae5054992649c873db7.svg"},
			{"\\uD83E\\uDD15", "https://discordapp.com/assets/720a392d0bcd8e644b0d3f8698889238.svg"},
			{"\\uD83D\\uDE34", "https://discordapp.com/assets/9ff23cdb0da49d75de71469a6bd4725c.svg"},
			{"\\uD83D\\uDCA4", "https://discordapp.com/assets/ebd55fc1b90bc34d1bfb36b466b19d54.svg"},
			{"\\uD83D\\uDCA9", "https://discordapp.com/assets/ced0c08553c2ade6cbeee29a40f4ac8c.svg"},
			{"\\uD83D\\uDE08", "https://discordapp.com/assets/69cc1b4583611ccc6a5652d1ddaee8fc.svg"},
			{"\\uD83D\\uDC7F", "https://discordapp.com/assets/09cf1b8ac070f68b451de754e56f464a.svg"},
			{"\\uD83D\\uDC79", "https://discordapp.com/assets/1a97e9e64e832ccb6eab7ec1661dd09f.svg"},
			{"\\uD83D\\uDC7A", "https://discordapp.com/assets/02b72abda0d394b1b194b9d788cbe070.svg"},
			{"\\uD83D\\uDC80", "https://discordapp.com/assets/ca5ca83f3d1c7d60dcdba18b97d68f7e.svg"},
			{"\\uD83D\\uDC7B", "https://discordapp.com/assets/52a1855c6542b674e2f145306c76bebe.svg"},
			{"\\uD83D\\uDC7D", "https://discordapp.com/assets/d14aa021cbb0a5819ec78a29291dee83.svg"},
			{"\\uD83E\\uDD16", "https://discordapp.com/assets/37721a3154785c9557e97172b60c6ce7.svg"},
			{"\\uD83D\\uDE3A", "https://discordapp.com/assets/a0c76fd4507e514834d4b91521b154a2.svg"},
			{"\\uD83D\\uDE38", "https://discordapp.com/assets/6eb0094a48d84f710cf75669a651fda6.svg"},
			{"\\uD83D\\uDE39", "https://discordapp.com/assets/9d0105dee7788a1fd4afbafe46dbd49b.svg"},
			{"\\uD83D\\uDE3B", "https://discordapp.com/assets/cc8afc69b651db9c2e9be9922eb84d52.svg"},
			{"\\uD83D\\uDE3C", "https://discordapp.com/assets/359da261fc3928d6be58c30d646b2183.svg"},
			{"\\uD83D\\uDE3D", "https://discordapp.com/assets/6330fe0d9849ac31f42a4421c38507e5.svg"},
			{"\\uD83D\\uDE40", "https://discordapp.com/assets/fc1ca9bdd3eb10c518fa9cc6adbb0dc0.svg"},
			{"\\uD83D\\uDE3F", "https://discordapp.com/assets/dad7a30149566338fbe1ea8de6e5e6bd.svg"},
			{"\\uD83D\\uDE3E", "https://discordapp.com/assets/b9466f87d129b12061fe2de01ce8067e.svg"},
			{"\\uD83D\\uDE4C", "https://discordapp.com/assets/b0e797f8bd2559fe2da945a8010c63fe.svg"},
			{"\\uD83D\\uDC4F", "https://discordapp.com/assets/79b9eb736bd31cd7d9ed23046929fda0.svg"},
			{"\\uD83D\\uDC4B", "https://discordapp.com/assets/593c4a3437fbb5b89fbb148f7b96424d.svg"},
			{"\\uD83D\\uDC4D", "https://discordapp.com/assets/2af915882260fdb89538d1610e1d9baa.svg"},
			{"\\uD83D\\uDC4E", "https://discordapp.com/assets/9e1c3ddc9da7effefe8a370b7c33ed7b.svg"},
			{"\\uD83D\\uDC4A", "https://discordapp.com/assets/fadb5208b3cfe613768e0ea7a8d1156c.svg"},
			{"\\u270A", "https://discordapp.com/assets/af184eec103ade6eab147ebab9ad651e.svg"},
			{"\\u270C", "https://discordapp.com/assets/140579c129c16668c3b91718fe747a75.svg"},
			{"\\uD83D\\uDC4C", "https://discordapp.com/assets/b6f700d4bc253abdb5ad576917b756d8.svg"},
			{"\\u270B", "https://discordapp.com/assets/fc89a10f50e8b0046eb7d640f5992aa5.svg"},
			{"\\uD83D\\uDC50", "https://discordapp.com/assets/0b8a25b6bf050e0cdc61f322e2c518d7.svg"},
			{"\\uD83D\\uDCAA", "https://discordapp.com/assets/0e2bb36113661c72bb9b3b4e5c834f97.svg"},
			{"\\uD83D\\uDE4F", "https://discordapp.com/assets/5b4053c17d4c674d7d379a97460df444.svg"},
			{"\\u261D", "https://discordapp.com/assets/b5cb3b03c8c473539a5d17fdd01b5d03.svg"},
			{"\\uD83D\\uDC46", "https://discordapp.com/assets/c3b50e0a2399559c80c84bc1766ca177.svg"},
			{"\\uD83D\\uDC47", "https://discordapp.com/assets/e60e16b4d0e0473e84e9253b6c31c9fe.svg"},
			{"\\uD83D\\uDC48", "https://discordapp.com/assets/079d5f5bb53850bc4f42a142467c3e49.svg"},
			{"\\uD83D\\uDC49", "https://discordapp.com/assets/6fc965fbef1b4aeb6167f652cd0544fc.svg"},
			{"\\uD83D\\uDD95", "https://discordapp.com/assets/209381ec0f39a61c1904269ed41c62eb.svg"},
			{"\\uD83D\\uDD90", "https://discordapp.com/assets/a7ca6d4faf1d497b4d75b44a6bb58f91.svg"},
			{"\\uD83E\\uDD18", "https://discordapp.com/assets/26363123de60a4946bf4e8987523dcb2.svg"},
			{"\\uD83D\\uDD96", "https://discordapp.com/assets/cf154cec5ac15640f8e980ce6026178b.svg"},
			{"\\u270D", "https://discordapp.com/assets/8367785ae4d4334f1f3371e239059ca1.svg"},
			{"\\uD83D\\uDC85", "https://discordapp.com/assets/d7433e51761b8c70de728e792c3e5182.svg"},
			{"\\uD83D\\uDC44", "https://discordapp.com/assets/1dc2d1d261117e88346765640e38eb47.svg"},
			{"\\uD83D\\uDC45", "https://discordapp.com/assets/03ba06f6abbf1e4e44ee591037c18c6b.svg"},
			{"\\uD83D\\uDC42", "https://discordapp.com/assets/13c22b05b67e203010c9bdf7bef139ef.svg"},
			{"\\uD83D\\uDC43", "https://discordapp.com/assets/dbdf73851d673c57783bb673cd92c2ac.svg"},
			{"\\uD83D\\uDC41", "https://discordapp.com/assets/14a28558668a65318bd7812da7d443bf.svg"},
			{"\\uD83D\\uDC40", "https://discordapp.com/assets/ccf4c733929efd9762ab02cd65175377.svg"},
			{"\\uD83D\\uDC64", "https://discordapp.com/assets/1223c2e358973af4766acf976e941520.svg"},
			{"\\uD83D\\uDC65", "https://discordapp.com/assets/cb3eea8759d62489b21ccd3ee913c1c8.svg"},
			{"\\uD83D\\uDDE3", "https://discordapp.com/assets/6fd0da46c7a400150ac09ba4a456a1f3.svg"},
			{"\\uD83D\\uDC76", "https://discordapp.com/assets/ffe9f8f9932a39a98b6cd7b65a6166f3.svg"},
			{"\\uD83D\\uDC66", "https://discordapp.com/assets/88e1c7830dd53d39ad610d683d72bd7a.svg"},
			{"\\uD83D\\uDC67", "https://discordapp.com/assets/60c15854817364e8cc890c854b597dbc.svg"},
			{"\\uD83D\\uDC68", "https://discordapp.com/assets/6fcc9018fde2319ef05e73926213cb22.svg"},
			{"\\uD83D\\uDC69", "https://discordapp.com/assets/4e2293fd5dd39f9dffa4a081d49eee85.svg"},
			{"\\uD83D\\uDC71", "https://discordapp.com/assets/9c3350157759e726b74dec0e9ca66236.svg"},
			{"\\uD83D\\uDC74", "https://discordapp.com/assets/49c3a3fcb6fce77858b1d839207d4485.svg"},
			{"\\uD83D\\uDC75", "https://discordapp.com/assets/e74caa2bcdbe35371940210cbba77a43.svg"},
			{"\\uD83D\\uDC72", "https://discordapp.com/assets/1684885d6fcc9004c4a2af3a84678e88.svg"},
			{"\\uD83D\\uDC73", "https://discordapp.com/assets/5f296f11809f5d2f79be9304f634ae30.svg"},
			{"\\uD83D\\uDC6E", "https://discordapp.com/assets/27ea5cf73c2aabb6e8bf523e117ff5dc.svg"},
			{"\\uD83D\\uDC77", "https://discordapp.com/assets/1bccaf07bdead06ebdc1a487ee546897.svg"},
			{"\\uD83D\\uDC82", "https://discordapp.com/assets/a3fdff4442a1292282f41fbfefab4c45.svg"},
			{"\\uD83D\\uDD75", "https://discordapp.com/assets/a39460d0f6baa307386a4bb2984de363.svg"},
			{"\\uD83C\\uDF85", "https://discordapp.com/assets/37017a416c2a6ab030f0de3db9f6e73e.svg"},
			{"\\uD83D\\uDC7C", "https://discordapp.com/assets/4e119a2ac01300de41aee4e941c8f714.svg"},
			{"\\uD83D\\uDC78", "https://discordapp.com/assets/bd64b69bc23bf78a5cc04ea738691317.svg"},
			{"\\uD83D\\uDC70", "https://discordapp.com/assets/d17b793b9284750b82a660da66de341e.svg"},
			{"\\uD83D\\uDEB6", "https://discordapp.com/assets/a3ca232394a8e24c2110860e52fba60f.svg"},
			{"\\uD83C\\uDFC3", "https://discordapp.com/assets/684da156eee6f555554432a375b67205.svg"},
			{"\\uD83D\\uDC83", "https://discordapp.com/assets/0912d3eafa64a0d5f4f1db5fd3598a93.svg"},
			{"\\uD83D\\uDC6F", "https://discordapp.com/assets/5b42fc2496caa87aad4ac3478bcd332d.svg"},
			{"\\uD83D\\uDC6B", "https://discordapp.com/assets/16f6089f47c4fb7b7bdf8ae57bfd4351.svg"},
			{"\\uD83D\\uDC6C", "https://discordapp.com/assets/570a9fecc052874ae0842f1a95f57cda.svg"},
			{"\\uD83D\\uDC6D", "https://discordapp.com/assets/51e6ffb5e64dbb6572dae6beeb4ed310.svg"},
			{"\\uD83D\\uDE47", "https://discordapp.com/assets/6ad7533814016970e37691af940737b1.svg"},
			{"\\uD83D\\uDC81", "https://discordapp.com/assets/ba00bf0e3b2d93d7be86cc5b1b580442.svg"},
			{"\\uD83D\\uDE45", "https://discordapp.com/assets/c8716626092cfde94ce42517ef389eac.svg"},
			{"\\uD83D\\uDE46", "https://discordapp.com/assets/237b70ca297af61150849c365804c779.svg"},
			{"\\uD83D\\uDE4B", "https://discordapp.com/assets/d9b465085bb2bdd5063063d25c63081b.svg"},
			{"\\uD83D\\uDE4E", "https://discordapp.com/assets/e85ff09eff5deffb7c5518b5f709f32f.svg"},
			{"\\uD83D\\uDE4D", "https://discordapp.com/assets/b3644635b7fa4c14b8c687279132d6d3.svg"},
			{"\\uD83D\\uDC87", "https://discordapp.com/assets/a1e516be5d5bfc7f5a5e303942d608d7.svg"},
			{"\\uD83D\\uDC86", "https://discordapp.com/assets/192e9e7cfbd38fe1cd24f12c4475d1c6.svg"},
			{"\\uD83D\\uDC91", "https://discordapp.com/assets/c51dd8cca368c0cd8eeb017e96d2c168.svg"},
			{"\\uD83D\\uDC69\\u200D\\u2764\\uFE0F\\u200D\\uD83D\\uDC69", "https://discordapp.com/assets/4f916b4d4139b90b19ffb3eeb8e376f6.svg"},
			{"\\uD83D\\uDC68\\u200D\\u2764\\uFE0F\\u200D\\uD83D\\uDC68", "https://discordapp.com/assets/e865d927daed301ac4ce03102a064560.svg"},
			{"\\uD83D\\uDC8F", "https://discordapp.com/assets/175753f47c4d8f06b4977920479a27d3.svg"},
			{"\\uD83D\\uDC69\\u200D\\u2764\\uFE0F\\u200D\\uD83D\\uDC8B\\u200D\\uD83D\\uDC69", "https://discordapp.com/assets/815fb767303df6eda484a4f6632bb767.svg"},
			{"\\uD83D\\uDC68\\u200D\\u2764\\uFE0F\\u200D\\uD83D\\uDC8B\\u200D\\uD83D\\uDC68", "https://discordapp.com/assets/77d250b4aa50598b06e2d4a756914677.svg"},
			{"\\uD83D\\uDC6A", "https://discordapp.com/assets/b0e3b6349576dab29bb73df4b4ce274e.svg"},
			{"\\uD83D\\uDC68\\u200D\\uD83D\\uDC69\\u200D\\uD83D\\uDC67", "https://discordapp.com/assets/cde634e84c397fcf91664d348fba462f.svg"},
			{"\\uD83D\\uDC68\\u200D\\uD83D\\uDC69\\u200D\\uD83D\\uDC67\\u200D\\uD83D\\uDC66", "https://discordapp.com/assets/23db9aa314959493a5d568de2454dce1.svg"},
			{"\\uD83D\\uDC68\\u200D\\uD83D\\uDC69\\u200D\\uD83D\\uDC66\\u200D\\uD83D\\uDC66", "https://discordapp.com/assets/f03b548a78f7a19596e01447becb6821.svg"},
			{"\\uD83D\\uDC68\\u200D\\uD83D\\uDC69\\u200D\\uD83D\\uDC67\\u200D\\uD83D\\uDC67", "https://discordapp.com/assets/468cd167e873cdaa1d27c862e1a7046b.svg"},
			{"\\uD83D\\uDC69\\u200D\\uD83D\\uDC69\\u200D\\uD83D\\uDC66", "https://discordapp.com/assets/0b242cf7dc9d2a4133943f8718f019e2.svg"},
			{"\\uD83D\\uDC69\\u200D\\uD83D\\uDC69\\u200D\\uD83D\\uDC67", "https://discordapp.com/assets/d8b0987deb221bc13bcaf5435601e61d.svg"},
			{"\\uD83D\\uDC69\\u200D\\uD83D\\uDC69\\u200D\\uD83D\\uDC67\\u200D\\uD83D\\uDC66", "https://discordapp.com/assets/ad8ccd0093ef970ad3d05904369e7331.svg"},
			{"\\uD83D\\uDC69\\u200D\\uD83D\\uDC69\\u200D\\uD83D\\uDC66\\u200D\\uD83D\\uDC66", "https://discordapp.com/assets/f4aebd7ac1c1c145b826f79e0d7366e9.svg"},
			{"\\uD83D\\uDC69\\u200D\\uD83D\\uDC69\\u200D\\uD83D\\uDC67\\u200D\\uD83D\\uDC67", "https://discordapp.com/assets/b9d9a2715cce7f809e0f281a5fbcde44.svg"},
			{"\\uD83D\\uDC68\\u200D\\uD83D\\uDC68\\u200D\\uD83D\\uDC66", "https://discordapp.com/assets/d31f3237d6f64283c2fe3f90305bfd16.svg"},
			{"\\uD83D\\uDC68\\u200D\\uD83D\\uDC68\\u200D\\uD83D\\uDC67", "https://discordapp.com/assets/1b75264578bd214440789e652a2f877e.svg"},
			{"\\uD83D\\uDC68\\u200D\\uD83D\\uDC68\\u200D\\uD83D\\uDC67\\u200D\\uD83D\\uDC66", "https://discordapp.com/assets/26d33aa82f5ff4a73a2ee14f04938650.svg"},
			{"\\uD83D\\uDC68\\u200D\\uD83D\\uDC68\\u200D\\uD83D\\uDC66\\u200D\\uD83D\\uDC66", "https://discordapp.com/assets/1a55a94b439c00190aa3b353e4c455e1.svg"},
			{"\\uD83D\\uDC68\\u200D\\uD83D\\uDC68\\u200D\\uD83D\\uDC67\\u200D\\uD83D\\uDC67", "https://discordapp.com/assets/906900c3a75d9a95d580f73f352e45be.svg"},
			{"\\uD83D\\uDC5A", "https://discordapp.com/assets/107ffb947b49100f34ab72d32093da4a.svg"},
			{"\\uD83D\\uDC55", "https://discordapp.com/assets/d871549666e6353d20b8a60778a077e0.svg"},
			{"\\uD83D\\uDC56", "https://discordapp.com/assets/d1e9173d0ec63f19c136a027b037c735.svg"},
			{"\\uD83D\\uDC54", "https://discordapp.com/assets/d49863f87f4f6b66c5cb52305d09cec7.svg"},
			{"\\uD83D\\uDC57", "https://discordapp.com/assets/33b9fde26b21aabfe60f2eee4eef6725.svg"},
			{"\\uD83D\\uDC59", "https://discordapp.com/assets/b4351e95f4ee4ba1ad225b996976a42b.svg"},
			{"\\uD83D\\uDC58", "https://discordapp.com/assets/d0e1b6e9b80de517cc572abb68e15591.svg"},
			{"\\uD83D\\uDC84", "https://discordapp.com/assets/8f05049e215401ab36b109f7540164be.svg"},
			{"\\uD83D\\uDC8B", "https://discordapp.com/assets/0f7bf0aae65b06ec5a342ae10c4b1856.svg"},
			{"\\uD83D\\uDC63", "https://discordapp.com/assets/56950b28b0de71b80cdb2fdb74affb03.svg"},
			{"\\uD83D\\uDC60", "https://discordapp.com/assets/6891601ed7363a53a8d81d8d661b564e.svg"},
			{"\\uD83D\\uDC61", "https://discordapp.com/assets/bcd5640706c77fef963998baa5d1d168.svg"},
			{"\\uD83D\\uDC62", "https://discordapp.com/assets/1ac6fb17c60570343eec4e81fe1e4073.svg"},
			{"\\uD83D\\uDC5E", "https://discordapp.com/assets/fa53f108d432bc4086b5849c99a1cee0.svg"},
			{"\\uD83D\\uDC5F", "https://discordapp.com/assets/f9b9ba8a54133d3e4fed11e512f43f83.svg"},
			{"\\uD83D\\uDC52", "https://discordapp.com/assets/92808c626aa527f9f0a643744dccf49a.svg"},
			{"\\uD83C\\uDFA9", "https://discordapp.com/assets/e52382056f97f305e32d8f14aa557526.svg"},
			{"\\u26D1", "https://discordapp.com/assets/ebc2b7d9c497f0e4663d46c13e35fe23.svg"},
			{"\\uD83C\\uDF93", "https://discordapp.com/assets/30031f3c30951eed996e7bacdbbfb61f.svg"},
			{"\\uD83D\\uDC51", "https://discordapp.com/assets/779fb0d7cf9afd16249ff8f82f0450e4.svg"},
			{"\\uD83C\\uDF92", "https://discordapp.com/assets/bbae6b9bd3d06c039ec917424473ee1a.svg"},
			{"\\uD83D\\uDC5D", "https://discordapp.com/assets/436c4e245d6573100d79a0f3d57fa0a3.svg"},
			{"\\uD83D\\uDC5B", "https://discordapp.com/assets/2384e7ab2ccb8ba9f4434ca35a414bea.svg"},
			{"\\uD83D\\uDC5C", "https://discordapp.com/assets/cf609ce2c1dd8d2ac6ec799d6bcc19ab.svg"},
			{"\\uD83D\\uDCBC", "https://discordapp.com/assets/0d9ad62c34798452bbc96cd531f91258.svg"},
			{"\\uD83D\\uDC53", "https://discordapp.com/assets/34e88504674828ff4c88b5e5279a794c.svg"},
			{"\\uD83D\\uDD76", "https://discordapp.com/assets/257c0cd89a832bbec1dd24f4ce2551fb.svg"},
			{"\\uD83D\\uDC8D", "https://discordapp.com/assets/1af980baa9905025f174dace30ac031b.svg"},
			{"\\uD83C\\uDF02", "https://discordapp.com/assets/d182716edc150e63958601807ae10a7a.svg"},
			{"\\uD83E\\uDD20", "https://discordapp.com/assets/984390b3eefc024ea770ccbfcfbdc4e2.svg"},
			{"\\uD83E\\uDD21", "https://discordapp.com/assets/1beee7912bb5016975747a3086794957.svg"},
			{"\\uD83E\\uDD22", "https://discordapp.com/assets/a9257530099447e1e7846cf269d16948.svg"},
			{"\\uD83E\\uDD23", "https://discordapp.com/assets/a8fc44fdb07e26cf994055376a719540.svg"},
			{"\\uD83E\\uDD24", "https://discordapp.com/assets/e915a145787a561dfb06fce7e0d55c2c.svg"},
			{"\\uD83E\\uDD25", "https://discordapp.com/assets/724802e432ab8683ca07516667271177.svg"},
			{"\\uD83E\\uDD27", "https://discordapp.com/assets/f2ff2b63da9f3c90788af6db317ee268.svg"},
			{"\\uD83E\\uDD34", "https://discordapp.com/assets/1492eba594b9880b0b8038a5646f7bff.svg"},
			{"\\uD83E\\uDD35", "https://discordapp.com/assets/7952d980903dc48db721d3d63a00769d.svg"},
			{"\\uD83E\\uDD36", "https://discordapp.com/assets/91f43b7b78d9077aca6fec698800d3bd.svg"},
			{"\\uD83E\\uDD26", "https://discordapp.com/assets/a2d0c0f7e2a7219cb5f9b951bba19437.svg"},
			{"\\uD83E\\uDD37", "https://discordapp.com/assets/656a6c9ad248051ce95bfb0bcec98ffa.svg"},
			{"\\uD83E\\uDD30", "https://discordapp.com/assets/c69556bbed1f340dbdc84299267738a6.svg"},
			{"\\uD83E\\uDD33", "https://discordapp.com/assets/b97d4816573933b53e290136980e4136.svg"},
			{"\\uD83D\\uDD7A", "https://discordapp.com/assets/6d5a34e74d7df5e18489d1ddc37da321.svg"},
			{"\\uD83E\\uDD19", "https://discordapp.com/assets/c90a58a821ce15bb04f0394c1abe5f38.svg"},
			{"\\uD83E\\uDD1A", "https://discordapp.com/assets/0c0a3500a658fb9df2fe8359cec22571.svg"},
			{"\\uD83E\\uDD1B", "https://discordapp.com/assets/86e370b15b66e9e93eddee319c654a43.svg"},
			{"\\uD83E\\uDD1C", "https://discordapp.com/assets/2125697f90e69344140e556a0f60b57b.svg"},
			{"\\uD83E\\uDD1D", "https://discordapp.com/assets/1c9af0d25ff26e228bf3d4828b6e10d1.svg"},
			{"\\uD83E\\uDD1E", "https://discordapp.com/assets/5a73c395732e49ad5768a857ab749677.svg"},
			{"\\uD83D\\uDC36", "https://discordapp.com/assets/d8225d4b952c1b5cc325e6e827da212a.svg"},
			{"\\uD83D\\uDC31", "https://discordapp.com/assets/a860a4e9c04e5cc2c8c48ebf51f7ed46.svg"},
			{"\\uD83D\\uDC2D", "https://discordapp.com/assets/3a4038e1fe59ffac18a805c38e24d3ea.svg"},
			{"\\uD83D\\uDC39", "https://discordapp.com/assets/ffda2444921b69228291fa50745022c9.svg"},
			{"\\uD83D\\uDC30", "https://discordapp.com/assets/84564edb3a34a7da402ae96da13baf58.svg"},
			{"\\uD83D\\uDC3B", "https://discordapp.com/assets/6ab348e4b840f727d585c68cbc8dc074.svg"},
			{"\\uD83D\\uDC3C", "https://discordapp.com/assets/495e6dc58a86913df2477399e3e78838.svg"},
			{"\\uD83D\\uDC28", "https://discordapp.com/assets/1b78123a5286af3af12ae91b105233b7.svg"},
			{"\\uD83D\\uDC2F", "https://discordapp.com/assets/04d668ecca6d551b0f08a73029ca43c9.svg"},
			{"\\uD83E\\uDD81", "https://discordapp.com/assets/4ed4f181c60639b9a97e17610393530d.svg"},
			{"\\uD83D\\uDC2E", "https://discordapp.com/assets/ee1d926e9c46e677e2242925f8911597.svg"},
			{"\\uD83D\\uDC37", "https://discordapp.com/assets/1036416ac37e433260e450a69f98827c.svg"},
			{"\\uD83D\\uDC3D", "https://discordapp.com/assets/b55bcafd9f7f3267d0b94e71e6eb69ca.svg"},
			{"\\uD83D\\uDC38", "https://discordapp.com/assets/2dc62d269672004b581d3e056151c938.svg"},
			{"\\uD83D\\uDC19", "https://discordapp.com/assets/d59493c473541bf5d036c56e996b152b.svg"},
			{"\\uD83D\\uDC35", "https://discordapp.com/assets/93e586bb99dac8dfaf6e6555f8694181.svg"},
			{"\\uD83D\\uDE48", "https://discordapp.com/assets/3b98d69b84b6d197204336b613538bc1.svg"},
			{"\\uD83D\\uDE49", "https://discordapp.com/assets/db43dae0609a391b37e34b42a27e2e93.svg"},
			{"\\uD83D\\uDE4A", "https://discordapp.com/assets/9e24093e9842eb0d73db991debf1d254.svg"},
			{"\\uD83D\\uDC12", "https://discordapp.com/assets/d4949506db2f4be8f969489d4e392965.svg"},
			{"\\uD83D\\uDC14", "https://discordapp.com/assets/1b0f762c9c14b7dc308a80e9c456010a.svg"},
			{"\\uD83D\\uDC27", "https://discordapp.com/assets/4c896778d815af37a1a3f82e271b77b2.svg"},
			{"\\uD83D\\uDC26", "https://discordapp.com/assets/03f65a2b1b7367dc7647ffc6e995b82a.svg"},
			{"\\uD83D\\uDC24", "https://discordapp.com/assets/d49ca9caa1630189340a05e11c31b836.svg"},
			{"\\uD83D\\uDC23", "https://discordapp.com/assets/24f401e5ee893e4a76f6628cb9991858.svg"},
			{"\\uD83D\\uDC25", "https://discordapp.com/assets/7a0c11d4a46373e2d313ba7b4f8b1141.svg"},
			{"\\uD83D\\uDC3A", "https://discordapp.com/assets/04ff67f3321f9158ad57242a5412782b.svg"},
			{"\\uD83D\\uDC17", "https://discordapp.com/assets/f1a79f1fefd94725135248214e72b9a4.svg"},
			{"\\uD83D\\uDC34", "https://discordapp.com/assets/fa4f3767ca6e775a52e2074c7cf593d7.svg"},
			{"\\uD83E\\uDD84", "https://discordapp.com/assets/bffd9a1668dbc714adda404b93489286.svg"},
			{"\\uD83D\\uDC1D", "https://discordapp.com/assets/e144c6c0a7a2434f662208bf5b89b798.svg"},
			{"\\uD83D\\uDC1B", "https://discordapp.com/assets/d9142d48843740db6d3f3f5d2900a1da.svg"},
			{"\\uD83D\\uDC0C", "https://discordapp.com/assets/3c5dba13de93507262fea339d4e32550.svg"},
			{"\\uD83D\\uDC1E", "https://discordapp.com/assets/83c94a65a51a850f3d43aed9eb60e042.svg"},
			{"\\uD83D\\uDC1C", "https://discordapp.com/assets/0460f909ad7af026badfc426a7e32f11.svg"},
			{"\\uD83D\\uDD77", "https://discordapp.com/assets/58e11980fae4c72a9625a2f531bbc4e4.svg"},
			{"\\uD83E\\uDD82", "https://discordapp.com/assets/6933bcd91ff81ff0d8b253dae5b592cb.svg"},
			{"\\uD83E\\uDD80", "https://discordapp.com/assets/aa41a897bc1bc175c41689b00b99e1ea.svg"},
			{"\\uD83D\\uDC0D", "https://discordapp.com/assets/b2b6a3c4ecc6af020a67a160ea8a8bb8.svg"},
			{"\\uD83D\\uDC22", "https://discordapp.com/assets/36cb5886ae3accf96c962ef823176596.svg"},
			{"\\uD83D\\uDC20", "https://discordapp.com/assets/6d51c298784d9f83dd9931ab7bedda9b.svg"},
			{"\\uD83D\\uDC1F", "https://discordapp.com/assets/d18dfb118ca075e3ab7eed6ebc9f881c.svg"},
			{"\\uD83D\\uDC21", "https://discordapp.com/assets/16c75fad5f214242fcc7a76074bdc80d.svg"},
			{"\\uD83D\\uDC2C", "https://discordapp.com/assets/e7479bab4adffd1e04a83d8068929cdb.svg"},
			{"\\uD83D\\uDC33", "https://discordapp.com/assets/6a850d8ed7c8f8b3e63329a2c136ff41.svg"},
			{"\\uD83D\\uDC0B", "https://discordapp.com/assets/1675e9676a9416f3689590c220a38c13.svg"},
			{"\\uD83D\\uDC0A", "https://discordapp.com/assets/255ee7696b55ca3868d00eb4e52c922b.svg"},
			{"\\uD83D\\uDC06", "https://discordapp.com/assets/e30e044accd084e550854c58a010bfea.svg"},
			{"\\uD83D\\uDC05", "https://discordapp.com/assets/7f5f4bf5c926e4998426a60d7c5ba29c.svg"},
			{"\\uD83D\\uDC03", "https://discordapp.com/assets/7f113473a29d9c7b7a954e669db5b167.svg"},
			{"\\uD83D\\uDC02", "https://discordapp.com/assets/3e5f9fbba5cd45c116b0a868509008b2.svg"},
			{"\\uD83D\\uDC04", "https://discordapp.com/assets/383fefaa251f7732759be07707a6aa70.svg"},
			{"\\uD83D\\uDC2A", "https://discordapp.com/assets/a2fb1d552818beaefb7d4653da207cf0.svg"},
			{"\\uD83D\\uDC2B", "https://discordapp.com/assets/50ea1e97a64ed97c5588f0126e315e45.svg"},
			{"\\uD83D\\uDC18", "https://discordapp.com/assets/d92014fedb8b81baab6ed750ba590a7b.svg"},
			{"\\uD83D\\uDC10", "https://discordapp.com/assets/b57b81cdf5618cbe809e3a425523a944.svg"},
			{"\\uD83D\\uDC0F", "https://discordapp.com/assets/a38c4f69457767c7e362f32a20cfcd0e.svg"},
			{"\\uD83D\\uDC11", "https://discordapp.com/assets/52df5e2c6d61b372cbaf75a278e17261.svg"},
			{"\\uD83D\\uDC0E", "https://discordapp.com/assets/b39eea19fdc9f773948cf64ccccebe8b.svg"},
			{"\\uD83D\\uDC16", "https://discordapp.com/assets/3dad82af3995eb17a59a2e6d7b7df7b0.svg"},
			{"\\uD83D\\uDC00", "https://discordapp.com/assets/2fe998aa3ba8e1024f36f4cafa6401a4.svg"},
			{"\\uD83D\\uDC01", "https://discordapp.com/assets/02dbf4bc2b1a58ec099b14e73cd3fea0.svg"},
			{"\\uD83D\\uDC13", "https://discordapp.com/assets/9b50f422b930f0815b1c349815fd5b52.svg"},
			{"\\uD83E\\uDD83", "https://discordapp.com/assets/21a56d74d4499530df9b3f54d4834e79.svg"},
			{"\\uD83D\\uDD4A", "https://discordapp.com/assets/18b7b5207b621acd50ca9322a8f60dab.svg"},
			{"\\uD83D\\uDC15", "https://discordapp.com/assets/42e685d97abaf277971bf5673732fe2f.svg"},
			{"\\uD83D\\uDC29", "https://discordapp.com/assets/15c006d1e1f0418cbc60358da6512e62.svg"},
			{"\\uD83D\\uDC08", "https://discordapp.com/assets/55d33f73a3c0a7737a11f6bcdc341f09.svg"},
			{"\\uD83D\\uDC07", "https://discordapp.com/assets/7b4593dfab26a088a6483a2b6eacb0f7.svg"},
			{"\\uD83D\\uDC3F", "https://discordapp.com/assets/84b1d1707ea9de159b520a46e3f8a7c8.svg"},
			{"\\uD83D\\uDC3E", "https://discordapp.com/assets/35e06b64acba7714e47cb907f1a51443.svg"},
			{"\\uD83D\\uDC09", "https://discordapp.com/assets/fa748c8c84723bcc66bb3009a0229442.svg"},
			{"\\uD83D\\uDC32", "https://discordapp.com/assets/57ee57d9c14b7d8ea5cc28b7be7bbbf8.svg"},
			{"\\uD83C\\uDF35", "https://discordapp.com/assets/030e6d2efc368f721295748885f468c4.svg"},
			{"\\uD83C\\uDF84", "https://discordapp.com/assets/a4fc3cdc6c4669dabe941e62b06f2d52.svg"},
			{"\\uD83C\\uDF32", "https://discordapp.com/assets/62a0e503ad0041f3adb93b5d20b29359.svg"},
			{"\\uD83C\\uDF33", "https://discordapp.com/assets/35472711332a495fb96cdde559792ed6.svg"},
			{"\\uD83C\\uDF34", "https://discordapp.com/assets/e267c9682f3ebc0c94683311f5ba86c1.svg"},
			{"\\uD83C\\uDF31", "https://discordapp.com/assets/a56e4d6cf086999a8870bb9c53f9c92c.svg"},
			{"\\uD83C\\uDF3F", "https://discordapp.com/assets/807f95efa69990d7af33ed12a0f62817.svg"},
			{"\\u2618", "https://discordapp.com/assets/5c6fa76f8837f9eb64596a5644e48f3e.svg"},
			{"\\uD83C\\uDF40", "https://discordapp.com/assets/dff3788a2facf7f845d5a556bca4b84e.svg"},
			{"\\uD83C\\uDF8D", "https://discordapp.com/assets/43be11ae9639bcf60c0e3a8d65de3b6c.svg"},
			{"\\uD83C\\uDF8B", "https://discordapp.com/assets/1253194d77d7fedd3f0ceddcd69a1e9a.svg"},
			{"\\uD83C\\uDF43", "https://discordapp.com/assets/9ca671f22bef7132cc6a245adfbb6f33.svg"},
			{"\\uD83C\\uDF42", "https://discordapp.com/assets/cb9350390bee90876d76088c32d6614d.svg"},
			{"\\uD83C\\uDF41", "https://discordapp.com/assets/746f57f37002009d0db8ae456329beb2.svg"},
			{"\\uD83C\\uDF3E", "https://discordapp.com/assets/5035c8a1d7423c77872979daff739850.svg"},
			{"\\uD83C\\uDF3A", "https://discordapp.com/assets/62dc78f6f9a73954e6454da485ea8147.svg"},
			{"\\uD83C\\uDF3B", "https://discordapp.com/assets/b6fafe51f63db5e8c7ccc33b5bd84c0a.svg"},
			{"\\uD83C\\uDF39", "https://discordapp.com/assets/36dd886a3c4b4cbd95ce9b5c9b09b319.svg"},
			{"\\uD83C\\uDF37", "https://discordapp.com/assets/9afb57a619ce0678333ee027e5ee1b14.svg"},
			{"\\uD83C\\uDF3C", "https://discordapp.com/assets/b26b930300cee75c3e86d5ac06cabd82.svg"},
			{"\\uD83C\\uDF38", "https://discordapp.com/assets/c0c3d14224896d2c097631cfb1f0a1d1.svg"},
			{"\\uD83D\\uDC90", "https://discordapp.com/assets/7df0c6179679d3e68ca94ae77f9d0af2.svg"},
			{"\\uD83C\\uDF44", "https://discordapp.com/assets/a14be676d662cda4d70be4b15fc0bbd8.svg"},
			{"\\uD83C\\uDF30", "https://discordapp.com/assets/8c5309c7dd9864ff56d106f1d9536499.svg"},
			{"\\uD83C\\uDF83", "https://discordapp.com/assets/2dcce6668e085caf506bff23c5fdec59.svg"},
			{"\\uD83D\\uDC1A", "https://discordapp.com/assets/d5490de23fbacae20a790c830ba5cf0b.svg"},
			{"\\uD83D\\uDD78", "https://discordapp.com/assets/432935945ad8b549c10692b2679e5768.svg"},
			{"\\uD83C\\uDF0E", "https://discordapp.com/assets/487af9f8669d2659ceb859f6dc7d2983.svg"},
			{"\\uD83C\\uDF0D", "https://discordapp.com/assets/e40b15e51f4fa295d6576ef6aff1128f.svg"},
			{"\\uD83C\\uDF0F", "https://discordapp.com/assets/92ac01847b2211edd71e887284fec8ff.svg"},
			{"\\uD83C\\uDF15", "https://discordapp.com/assets/37d1f79e28eb2f7c32e82b7dd3fa6f9c.svg"},
			{"\\uD83C\\uDF16", "https://discordapp.com/assets/c99a3d6a322cc9afbe91df4cfe308cbb.svg"},
			{"\\uD83C\\uDF17", "https://discordapp.com/assets/72366676f1a35daf460d3d93f80d88d5.svg"},
			{"\\uD83C\\uDF18", "https://discordapp.com/assets/94f92220e0771fe0a71fee2f64aa78ae.svg"},
			{"\\uD83C\\uDF11", "https://discordapp.com/assets/adff2251b7e3ebe10afd9a8a60247c66.svg"},
			{"\\uD83C\\uDF12", "https://discordapp.com/assets/47cf96971026fd3f1ec205928ca0a38a.svg"},
			{"\\uD83C\\uDF13", "https://discordapp.com/assets/ba707587014c62095171df32dea6e3ef.svg"},
			{"\\uD83C\\uDF14", "https://discordapp.com/assets/7426a0580d4a95133cd33630a2a784f7.svg"},
			{"\\uD83C\\uDF1A", "https://discordapp.com/assets/c1a0005ba1012d40010af3dcace883d4.svg"},
			{"\\uD83C\\uDF1D", "https://discordapp.com/assets/c00cc4a933e2b4920d4254bae11935fe.svg"},
			{"\\uD83C\\uDF1B", "https://discordapp.com/assets/a72f4b91069731e019bb2ecd765cec45.svg"},
			{"\\uD83C\\uDF1C", "https://discordapp.com/assets/8b0b7c1990c5ee39a2bafb3f9fb5b459.svg"},
			{"\\uD83C\\uDF1E", "https://discordapp.com/assets/4fd22f2a340c7a2eaad9218c2521aebb.svg"},
			{"\\uD83C\\uDF19", "https://discordapp.com/assets/beb0c376af200f686ff857f476214da5.svg"},
			{"\\u2B50", "https://discordapp.com/assets/e4d52f4d69d7bba67e5fd70ffe26b70d.svg"},
			{"\\uD83C\\uDF1F", "https://discordapp.com/assets/ca307d2c1ce9be6de43db70f55839499.svg"},
			{"\\uD83D\\uDCAB", "https://discordapp.com/assets/8df4c34679d6d9b893feb682d7568dbd.svg"},
			{"\\u2728", "https://discordapp.com/assets/c90098069e61110397d4552647ade33d.svg"},
			{"\\u2604", "https://discordapp.com/assets/cfd4367196d227d2d84f94e62b9b9a8a.svg"},
			{"\\u2600", "https://discordapp.com/assets/c98c3b661e0c450ea71e1f459a8a6f62.svg"},
			{"\\uD83C\\uDF24", "https://discordapp.com/assets/663c523aa6d80e3bb481c824ec589223.svg"},
			{"\\u26C5", "https://discordapp.com/assets/46c332bc675d27cb8bb088e2c1124483.svg"},
			{"\\uD83C\\uDF25", "https://discordapp.com/assets/155bfbdb20961c08e586dffbf6f3ef2b.svg"},
			{"\\uD83C\\uDF26", "https://discordapp.com/assets/e2b9bf0158515ef6a6c4edcf9c2325e5.svg"},
			{"\\u2601", "https://discordapp.com/assets/98e7433a3653b861949219fbac95b6af.svg"},
			{"\\uD83C\\uDF27", "https://discordapp.com/assets/150db78e52c699a04a9feb46592ebf89.svg"},
			{"\\u26C8", "https://discordapp.com/assets/67e37991670625f2ef4945de08106953.svg"},
			{"\\uD83C\\uDF29", "https://discordapp.com/assets/7fb143ba2eb69721e8335264f574f967.svg"},
			{"\\u26A1", "https://discordapp.com/assets/daa92de51b3a3c9825ea71a700823463.svg"},
			{"\\uD83D\\uDD25", "https://discordapp.com/assets/f91ea6469f14105cd066e706c9862fa0.svg"},
			{"\\uD83D\\uDCA5", "https://discordapp.com/assets/ef756c6ecfdc1cf509cb0175dd33c76d.svg"},
			{"\\u2744", "https://discordapp.com/assets/490f3ebaf8012cc0a2c3109ab3faff63.svg"},
			{"\\uD83C\\uDF28", "https://discordapp.com/assets/0af45b991f70fda65eb9992a21c82490.svg"},
			{"\\u2603", "https://discordapp.com/assets/850f3cbe3214da2d45417a999949b091.svg"},
			{"\\u26C4", "https://discordapp.com/assets/ca5cea8deb40a7f4d120acdc99323337.svg"},
			{"\\uD83C\\uDF2C", "https://discordapp.com/assets/d16111b8a6dbc0ac0259026e13102437.svg"},
			{"\\uD83D\\uDCA8", "https://discordapp.com/assets/13dde1c4a61bc1c8323b13c473ea4983.svg"},
			{"\\uD83C\\uDF2A", "https://discordapp.com/assets/98d9aff8b86b1fabcd71074550d09cd6.svg"},
			{"\\uD83C\\uDF2B", "https://discordapp.com/assets/de119213692744ef5a87a331435b065e.svg"},
			{"\\u2602", "https://discordapp.com/assets/bc4e973212a062a6eea1bb3b0705b52f.svg"},
			{"\\u2614", "https://discordapp.com/assets/7c73785b7addf766330c034e62e66f55.svg"},
			{"\\uD83D\\uDCA7", "https://discordapp.com/assets/de9fc0908fc4383c8e0dcf7eecda1c29.svg"},
			{"\\uD83D\\uDCA6", "https://discordapp.com/assets/5698d13f526f64539772f38b9992196c.svg"},
			{"\\uD83C\\uDF0A", "https://discordapp.com/assets/d1fff73b5411e88bba46ac2bc69398de.svg"},
			{"\\uD83E\\uDD85", "https://discordapp.com/assets/d29b206e5bc6855105b713ef409ff2d9.svg"},
			{"\\uD83E\\uDD86", "https://discordapp.com/assets/b055e46cbd47c1fd6185ec9adf020f56.svg"},
			{"\\uD83E\\uDD87", "https://discordapp.com/assets/46567dfa87a53033268d9f5cd4830200.svg"},
			{"\\uD83E\\uDD88", "https://discordapp.com/assets/eb8c35e8d77385fd4cfed7c1d529c4ce.svg"},
			{"\\uD83E\\uDD89", "https://discordapp.com/assets/4064436ec0e77108e7ea6cb76ac39ad6.svg"},
			{"\\uD83E\\uDD8A", "https://discordapp.com/assets/984ae0e09b54c95e13c22e34b925888f.svg"},
			{"\\uD83E\\uDD8B", "https://discordapp.com/assets/b789cf6fdb78ff5d936f31c4b32d4785.svg"},
			{"\\uD83E\\uDD8C", "https://discordapp.com/assets/b5bf289a185eb901d2eda938e2320ffc.svg"},
			{"\\uD83E\\uDD8D", "https://discordapp.com/assets/de65a7626f89403e26eed5c12e05fddf.svg"},
			{"\\uD83E\\uDD8E", "https://discordapp.com/assets/a87dad0daf3dfa2f18c5c5c572216542.svg"},
			{"\\uD83E\\uDD8F", "https://discordapp.com/assets/2d30feeb8bc13cec5920caab376b4078.svg"},
			{"\\uD83E\\uDD40", "https://discordapp.com/assets/c18bfb3e911ccfaac559be369fc4b55f.svg"},
			{"\\uD83E\\uDD90", "https://discordapp.com/assets/4870ba8a52c50997f3594e544198cf6b.svg"},
			{"\\uD83E\\uDD91", "https://discordapp.com/assets/c327d9beddd428512da1750773773fce.svg"},
			{"\\uD83C\\uDF4F", "https://discordapp.com/assets/da468af45a3cd2a5cb40c62610ee0637.svg"},
			{"\\uD83C\\uDF4E", "https://discordapp.com/assets/e763ebe2d978ea25333708ba18d37db6.svg"},
			{"\\uD83C\\uDF50", "https://discordapp.com/assets/e004a3a2bfa42a5d749c86a3afb325a7.svg"},
			{"\\uD83C\\uDF4A", "https://discordapp.com/assets/ffb5f4b99e0585db5b967bd44bb77a5b.svg"},
			{"\\uD83C\\uDF4B", "https://discordapp.com/assets/bea76643724cd4de0da49b16f54eb1e9.svg"},
			{"\\uD83C\\uDF4C", "https://discordapp.com/assets/52dc4e794e66fa9a88cefe291066e961.svg"},
			{"\\uD83C\\uDF49", "https://discordapp.com/assets/a4f65d197d2d32f3c91c96db998d487e.svg"},
			{"\\uD83C\\uDF47", "https://discordapp.com/assets/ac68e5f170ef78d8bb8b7fae73d68a53.svg"},
			{"\\uD83C\\uDF53", "https://discordapp.com/assets/17fd7b929f912001259e8b7222b5ff2a.svg"},
			{"\\uD83C\\uDF48", "https://discordapp.com/assets/a2c2224e27cb6739fa8c8074b0ed2733.svg"},
			{"\\uD83C\\uDF52", "https://discordapp.com/assets/804a42ae4de245f85cbc5eee348ab012.svg"},
			{"\\uD83C\\uDF51", "https://discordapp.com/assets/6e935b405970381b18c9b3fec0d8c686.svg"},
			{"\\uD83C\\uDF4D", "https://discordapp.com/assets/dc54ec2f7d2779b33e708fdb305baeea.svg"},
			{"\\uD83C\\uDF45", "https://discordapp.com/assets/1477eb87fb70989009ea4b337f6353ad.svg"},
			{"\\uD83C\\uDF46", "https://discordapp.com/assets/8234ccb1bad95b951ba3e44bf95214a6.svg"},
			{"\\uD83C\\uDF36", "https://discordapp.com/assets/b0dd0b816f72d053dd9d307940831d3f.svg"},
			{"\\uD83C\\uDF3D", "https://discordapp.com/assets/b16209365b7693d5f705a1ea450f7d45.svg"},
			{"\\uD83C\\uDF60", "https://discordapp.com/assets/5c73429380867c917ff1d494e747676d.svg"},
			{"\\uD83C\\uDF6F", "https://discordapp.com/assets/d02d2db4304bb6eb46c8da6e46f063c4.svg"},
			{"\\uD83C\\uDF5E", "https://discordapp.com/assets/a74bd6e914728e9ba7f10e8f70b75df0.svg"},
			{"\\uD83E\\uDDC0", "https://discordapp.com/assets/6697e53b3d3854090dd31a0b536855a7.svg"},
			{"\\uD83C\\uDF57", "https://discordapp.com/assets/e82ce0d17ebe97a2884a7e485bbbd73f.svg"},
			{"\\uD83C\\uDF56", "https://discordapp.com/assets/be462f59d47da4a9314c1dc11842136e.svg"},
			{"\\uD83C\\uDF64", "https://discordapp.com/assets/b606692f28666f0e692b19d920d3c6b1.svg"},
			{"\\uD83C\\uDF73", "https://discordapp.com/assets/dc316fcde03ae5c5189eb19b682f5c37.svg"},
			{"\\uD83C\\uDF54", "https://discordapp.com/assets/02e4fbcbf2ece779c9e0adbb842ccdcc.svg"},
			{"\\uD83C\\uDF5F", "https://discordapp.com/assets/cae1451b839f414214f667f49ecd70f2.svg"},
			{"\\uD83C\\uDF2D", "https://discordapp.com/assets/97c156c2095cc1b900faf86dcce7b214.svg"},
			{"\\uD83C\\uDF55", "https://discordapp.com/assets/59d34a4f4f1c4cd43b83304ebf1f9407.svg"},
			{"\\uD83C\\uDF5D", "https://discordapp.com/assets/b4db758a6551772dbd3fb0816b64c252.svg"},
			{"\\uD83C\\uDF2E", "https://discordapp.com/assets/e724ebe35afdf2e7056b8730b3b4247d.svg"},
			{"\\uD83C\\uDF2F", "https://discordapp.com/assets/6bf81ce7ef81aab8799951931e3fb230.svg"},
			{"\\uD83C\\uDF5C", "https://discordapp.com/assets/e9faa0cf8b68196acced35d681755756.svg"},
			{"\\uD83C\\uDF72", "https://discordapp.com/assets/af92e60c16b7019f34a467383b31490a.svg"},
			{"\\uD83C\\uDF65", "https://discordapp.com/assets/cf4ef732eb8dc647928341cddab48b36.svg"},
			{"\\uD83C\\uDF63", "https://discordapp.com/assets/d7edfc4ab51e91e644b09f0b2d61a8b5.svg"},
			{"\\uD83C\\uDF71", "https://discordapp.com/assets/797651f7e4a129779a076adcfb69b14b.svg"},
			{"\\uD83C\\uDF5B", "https://discordapp.com/assets/126f682c2b675d7bafc606d773169e7f.svg"},
			{"\\uD83C\\uDF59", "https://discordapp.com/assets/b44a5e03d61a498b3d9b64d2bd4cab7a.svg"},
			{"\\uD83C\\uDF5A", "https://discordapp.com/assets/4ca454e166b2afa9ef74ad954c93bd09.svg"},
			{"\\uD83C\\uDF58", "https://discordapp.com/assets/68f2d19eee1a02a16b6a77cf04bd0b53.svg"},
			{"\\uD83C\\uDF62", "https://discordapp.com/assets/a5363ca427118a229f8449f1abe5690f.svg"},
			{"\\uD83C\\uDF61", "https://discordapp.com/assets/0b857a06c3e3748666d53f9e1c28d274.svg"},
			{"\\uD83C\\uDF67", "https://discordapp.com/assets/44c4ef814686222cd0e7fbdbb4165916.svg"},
			{"\\uD83C\\uDF68", "https://discordapp.com/assets/9cb4585c4abac1921448ca433e65c21c.svg"},
			{"\\uD83C\\uDF66", "https://discordapp.com/assets/260e9e41b71d9e954d824e0df47786b0.svg"},
			{"\\uD83C\\uDF70", "https://discordapp.com/assets/5e8ba2c916f0ec894ee141a31b16d347.svg"},
			{"\\uD83C\\uDF82", "https://discordapp.com/assets/088958b946cca5e593f4ca8b4bfe2550.svg"},
			{"\\uD83C\\uDF6E", "https://discordapp.com/assets/3c97bd4978e173478233b8af4cbabe79.svg"},
			{"\\uD83C\\uDF6C", "https://discordapp.com/assets/49b17ff287afeb1d5feffe0e7af3c2ec.svg"},
			{"\\uD83C\\uDF6D", "https://discordapp.com/assets/004173a26af5bc31aa3ec9db2ad31eb5.svg"},
			{"\\uD83C\\uDF6B", "https://discordapp.com/assets/28754d9a4fc611d974e377eee8969be1.svg"},
			{"\\uD83C\\uDF7F", "https://discordapp.com/assets/0a093c2c2cbd9afd89ed5dc579db2a36.svg"},
			{"\\uD83C\\uDF69", "https://discordapp.com/assets/a1ea0f810fd17bf05c32df9bdc86754e.svg"},
			{"\\uD83C\\uDF6A", "https://discordapp.com/assets/08c483393c3109ec7e69f7e3c47336a0.svg"},
			{"\\uD83C\\uDF7A", "https://discordapp.com/assets/2bc8560ebf14cc0b478727ecc16733f1.svg"},
			{"\\uD83C\\uDF7B", "https://discordapp.com/assets/2c6b6f0f9347f3a5d0c708eaf7aa9dbd.svg"},
			{"\\uD83C\\uDF77", "https://discordapp.com/assets/8ae2c944431052f570dfbb743529504f.svg"},
			{"\\uD83C\\uDF78", "https://discordapp.com/assets/bd28dcbb121375a8c2eaa9e8bb65d883.svg"},
			{"\\uD83C\\uDF79", "https://discordapp.com/assets/bd479c1ecbab55df50ad81b6e7f5b71c.svg"},
			{"\\uD83C\\uDF7E", "https://discordapp.com/assets/bbfb5ecf0af3339a84e7f652234a0d84.svg"},
			{"\\uD83C\\uDF76", "https://discordapp.com/assets/771f1b2ac96411a6c12b14ed3d0279ba.svg"},
			{"\\uD83C\\uDF75", "https://discordapp.com/assets/4f7e968a9f51918bcd56429b57ffb619.svg"},
			{"\\u2615", "https://discordapp.com/assets/25c09e6fde32411da2b0da00f5cb9c84.svg"},
			{"\\uD83C\\uDF7C", "https://discordapp.com/assets/2113c50fce3233fa9f9095bae3d314b4.svg"},
			{"\\uD83C\\uDF74", "https://discordapp.com/assets/afc2fb8fe35e0bcebf88c7a58a83dec9.svg"},
			{"\\uD83C\\uDF7D", "https://discordapp.com/assets/8867614f678828b9e0182baff4b3d1d6.svg"},
			{"\\uD83E\\uDD50", "https://discordapp.com/assets/05c9b343892e5a3f9b0a903747df2ac7.svg"},
			{"\\uD83E\\uDD51", "https://discordapp.com/assets/56223cd60f7ec48e13e8a0db94dcc785.svg"},
			{"\\uD83E\\uDD52", "https://discordapp.com/assets/462ae06d5ffb1b8112d15720e6d8e64f.svg"},
			{"\\uD83E\\uDD53", "https://discordapp.com/assets/efdf49f9c7b2573cf9f1426d70a8fca3.svg"},
			{"\\uD83E\\uDD54", "https://discordapp.com/assets/5d931ac04b278a00dab9475bc9e6b7e9.svg"},
			{"\\uD83E\\uDD55", "https://discordapp.com/assets/663247501c40db47e427d91555ef8879.svg"},
			{"\\uD83E\\uDD56", "https://discordapp.com/assets/ac9a731e529a503e6802d1c88a7ec138.svg"},
			{"\\uD83E\\uDD57", "https://discordapp.com/assets/0e09f0ca5ca5bb86d61ea0c2014b5db1.svg"},
			{"\\uD83E\\uDD58", "https://discordapp.com/assets/e4965ff28544c020de27512bc0953117.svg"},
			{"\\uD83E\\uDD59", "https://discordapp.com/assets/28cbe4972becb7eb6fcff4ba2a53b547.svg"},
			{"\\uD83E\\uDD42", "https://discordapp.com/assets/a25c2bd97387b0675e9cd6ef232554cb.svg"},
			{"\\uD83E\\uDD43", "https://discordapp.com/assets/eacb28c08eee5cb0f337ce756f1e133f.svg"},
			{"\\uD83E\\uDD44", "https://discordapp.com/assets/4fec2689c8a6e17143289d586977ea71.svg"},
			{"\\uD83E\\uDD5A", "https://discordapp.com/assets/0d62be2acb3b25392155b171b1cb348b.svg"},
			{"\\uD83E\\uDD5B", "https://discordapp.com/assets/c8cab949ce51b231d2b9b516f022b23e.svg"},
			{"\\uD83E\\uDD5C", "https://discordapp.com/assets/02cc3a7f831134d0d75eb8d700ec8b64.svg"},
			{"\\uD83E\\uDD5D", "https://discordapp.com/assets/4e251ef255f5f06c84d22f425c99d77a.svg"},
			{"\\uD83E\\uDD5E", "https://discordapp.com/assets/6427eab8ed2ba33a8d4d6fb6fc1217a8.svg"},
			{"\\u26BD", "https://discordapp.com/assets/c60aaf4dfcb496fd6ffd825b2587b2d5.svg"},
			{"\\uD83C\\uDFC0", "https://discordapp.com/assets/255312d1086d2531262c3c7a035f0fb8.svg"},
			{"\\uD83C\\uDFC8", "https://discordapp.com/assets/b276cb78ae896507216721b470959e7a.svg"},
			{"\\u26BE", "https://discordapp.com/assets/27209854bc98486318964e188f82f753.svg"},
			{"\\uD83C\\uDFBE", "https://discordapp.com/assets/2e6dbd914bcb249de840736a7e08187c.svg"},
			{"\\uD83C\\uDFD0", "https://discordapp.com/assets/525d1096f42da4d5f3f0f240c6e94651.svg"},
			{"\\uD83C\\uDFC9", "https://discordapp.com/assets/baed78dd915c2058209edc028eead37a.svg"},
			{"\\uD83C\\uDFB1", "https://discordapp.com/assets/f73a8294c3ac519665af224d98bf411e.svg"},
			{"\\u26F3", "https://discordapp.com/assets/bdc79e5336d9995f9a4db9119866e63a.svg"},
			{"\\uD83C\\uDFCC", "https://discordapp.com/assets/c019e4eac0f6fddbd204a341d57d0440.svg"},
			{"\\uD83C\\uDFD3", "https://discordapp.com/assets/371886a66446c46e66e9435158468720.svg"},
			{"\\uD83C\\uDFF8", "https://discordapp.com/assets/a34fc833d8692402e06c3775bdaf1603.svg"},
			{"\\uD83C\\uDFD2", "https://discordapp.com/assets/2b74c0c2b1a7083a3d8215bcaf023799.svg"},
			{"\\uD83C\\uDFD1", "https://discordapp.com/assets/a167f9c82ddbe9fbdcdfe36822463d02.svg"},
			{"\\uD83C\\uDFCF", "https://discordapp.com/assets/acbd737806c28613d7bb04ad80097540.svg"},
			{"\\uD83C\\uDFBF", "https://discordapp.com/assets/223419a47a35ce812668e837d97f13d0.svg"},
			{"\\u26F7", "https://discordapp.com/assets/6625a50e5dc295557240bf5fc2f805b4.svg"},
			{"\\uD83C\\uDFC2", "https://discordapp.com/assets/dc17a2d947066ca7446f9d3987e26e5a.svg"},
			{"\\u26F8", "https://discordapp.com/assets/25d57342c1c757b77a7073e8e32178aa.svg"},
			{"\\uD83C\\uDFF9", "https://discordapp.com/assets/df798b4bcce8a9fa963193cc54ccd5c5.svg"},
			{"\\uD83C\\uDFA3", "https://discordapp.com/assets/5d1c4702f5cfaccd74882f2598c4ba0e.svg"},
			{"\\uD83D\\uDEA3", "https://discordapp.com/assets/e6953fd6629a4e56c7fa087586f2e30a.svg"},
			{"\\uD83C\\uDFCA", "https://discordapp.com/assets/51195c57a9ec87c842f7614302532ae1.svg"},
			{"\\uD83C\\uDFC4", "https://discordapp.com/assets/3e219e6ac2b1389b39ccbfb74b39414e.svg"},
			{"\\uD83D\\uDEC0", "https://discordapp.com/assets/1a6361ed9da761902998f0cec8fc7a34.svg"},
			{"\\u26F9", "https://discordapp.com/assets/449febad96c5b2a4c636324e03848876.svg"},
			{"\\uD83C\\uDFCB", "https://discordapp.com/assets/06bc1ae80d100774c51ebcbd3824c982.svg"},
			{"\\uD83D\\uDEB4", "https://discordapp.com/assets/4bd00770aeb2423f516537a387637c4a.svg"},
			{"\\uD83D\\uDEB5", "https://discordapp.com/assets/62d57225c20a09b58b79d9ddf7477b34.svg"},
			{"\\uD83C\\uDFC7", "https://discordapp.com/assets/bb38ab3106d26461f3703de34e699e09.svg"},
			{"\\uD83D\\uDD74", "https://discordapp.com/assets/1cbba79bb198dd09c5b9b6cc0a6c7ddf.svg"},
			{"\\uD83C\\uDFC6", "https://discordapp.com/assets/81852462a750c8929dde05a924ed0343.svg"},
			{"\\uD83C\\uDFBD", "https://discordapp.com/assets/0001a47876ba109e8e5c58d1fae40622.svg"},
			{"\\uD83C\\uDFC5", "https://discordapp.com/assets/520ae4acfffdaebc3b15af5dabd5173c.svg"},
			{"\\uD83C\\uDF96", "https://discordapp.com/assets/9ce76cd10d14d89247c296388ef14b1c.svg"},
			{"\\uD83C\\uDF97", "https://discordapp.com/assets/df64ebe6692abcfafda9b13dfb27a1f3.svg"},
			{"\\uD83C\\uDFF5", "https://discordapp.com/assets/885075cbaf384830f280fdc88e14723f.svg"},
			{"\\uD83C\\uDFAB", "https://discordapp.com/assets/0ceb447da6ca9eea6db4dae8058be578.svg"},
			{"\\uD83C\\uDF9F", "https://discordapp.com/assets/8fcc8d0ef94613a8b00f6caa0c615037.svg"},
			{"\\uD83C\\uDFAD", "https://discordapp.com/assets/66d3d8bfdfa13f415f7d9da2bd5dc022.svg"},
			{"\\uD83C\\uDFA8", "https://discordapp.com/assets/05b253435217af86a93a9b9716e7cd56.svg"},
			{"\\uD83C\\uDFAA", "https://discordapp.com/assets/0bc0dcbe06af597346a6cb69ce95ad46.svg"},
			{"\\uD83C\\uDFA4", "https://discordapp.com/assets/47535de9ad642a6045b1fd017b21aed5.svg"},
			{"\\uD83C\\uDFA7", "https://discordapp.com/assets/5229116797b54a6ce886b9517291aa0f.svg"},
			{"\\uD83C\\uDFBC", "https://discordapp.com/assets/c78b52229dc2aad445fd091453f4c75d.svg"},
			{"\\uD83C\\uDFB9", "https://discordapp.com/assets/7cd0ac1b40b476783bdd24079a289434.svg"},
			{"\\uD83C\\uDFB7", "https://discordapp.com/assets/1a27b6ad5cf99afee8fd1e5131ad7aec.svg"},
			{"\\uD83C\\uDFBA", "https://discordapp.com/assets/7f0a428e2ec84074019763ca8f4d3ef1.svg"},
			{"\\uD83C\\uDFB8", "https://discordapp.com/assets/b04f6915e15c402e7ea983b22b13b8e8.svg"},
			{"\\uD83C\\uDFBB", "https://discordapp.com/assets/6d8fcd27339ca609c6ae96db447b884e.svg"},
			{"\\uD83C\\uDFAC", "https://discordapp.com/assets/f8f1c8f06f243c0ef11c6cd1fff83bc1.svg"},
			{"\\uD83C\\uDFAE", "https://discordapp.com/assets/7d600babcd1bddfd7a7d35acc1ed4cd3.svg"},
			{"\\uD83D\\uDC7E", "https://discordapp.com/assets/42696bf24081bd55a03e15d4e4cc084a.svg"},
			{"\\uD83C\\uDFAF", "https://discordapp.com/assets/43df7da20ea936e1016d1ea5d79a2c6c.svg"},
			{"\\uD83C\\uDFB2", "https://discordapp.com/assets/1adc9faf91526bb7a2c1d0b7b3516cae.svg"},
			{"\\uD83C\\uDFB0", "https://discordapp.com/assets/e6ec28237dfa28af53d648bdec0a3d0d.svg"},
			{"\\uD83C\\uDFB3", "https://discordapp.com/assets/242bfc0589b06e155df9b58b6dcd73e9.svg"},
			{"\\uD83E\\uDD38", "https://discordapp.com/assets/e1994fb3ab91a3c99cde68c979a558ee.svg"},
			{"\\uD83E\\uDD39", "https://discordapp.com/assets/997bedb7542f94f18dff6504ce2c3f13.svg"},
			{"\\uD83E\\uDD3C", "https://discordapp.com/assets/0842cda4de0f01d4020013bcab7afa28.svg"},
			{"\\uD83E\\uDD4A", "https://discordapp.com/assets/0d0101d85a470c34a0b020e8ab875f2c.svg"},
			{"\\uD83E\\uDD4B", "https://discordapp.com/assets/8a098c50f641d3d5046a7c56952f237b.svg"},
			{"\\uD83E\\uDD3D", "https://discordapp.com/assets/9970d4d56dfd832c6ea99e927b13bb09.svg"},
			{"\\uD83E\\uDD3E", "https://discordapp.com/assets/6f6980df1f7bfbdf31b4651508e488da.svg"},
			{"\\uD83E\\uDD45", "https://discordapp.com/assets/5c01064e4ba7974949f1296a90204cb6.svg"},
			{"\\uD83E\\uDD3A", "https://discordapp.com/assets/ce8d2aed027c8cbc30bd1f40d7b3ca48.svg"},
			{"\\uD83E\\uDD47", "https://discordapp.com/assets/11f93b01fd905a56375d325d415ea670.svg"},
			{"\\uD83E\\uDD48", "https://discordapp.com/assets/e002839eeb2e5d558e4600159df6c24b.svg"},
			{"\\uD83E\\uDD49", "https://discordapp.com/assets/ce2c6afb5fcc844e5643ccc843d8da00.svg"},
			{"\\uD83E\\uDD41", "https://discordapp.com/assets/072de68f7012cd855f6281a9e1438c82.svg"},
			{"\\uD83D\\uDE97", "https://discordapp.com/assets/e95285636dd8086f3b383f4291d32761.svg"},
			{"\\uD83D\\uDE95", "https://discordapp.com/assets/bee7a41823a58db69b17748a6319b62f.svg"},
			{"\\uD83D\\uDE99", "https://discordapp.com/assets/1069ee1509716f8152b09f8c5b79ca11.svg"},
			{"\\uD83D\\uDE8C", "https://discordapp.com/assets/12c85e4c56ddfd3707584a851a6b8ce4.svg"},
			{"\\uD83D\\uDE8E", "https://discordapp.com/assets/85eb4cfbe4893aaa2198d9b6656ae418.svg"},
			{"\\uD83C\\uDFCE", "https://discordapp.com/assets/fc1956011b96828c246c19704512b50c.svg"},
			{"\\uD83D\\uDE93", "https://discordapp.com/assets/ff819634ae1a1b4b573b0110d683f7b1.svg"},
			{"\\uD83D\\uDE91", "https://discordapp.com/assets/5bfb1f479fd5b7c5bd662534abcd77db.svg"},
			{"\\uD83D\\uDE92", "https://discordapp.com/assets/9b478539dc650e8c6b690ab5e5b9f902.svg"},
			{"\\uD83D\\uDE90", "https://discordapp.com/assets/86f22f3882e2cbd61d51058bbd82af07.svg"},
			{"\\uD83D\\uDE9A", "https://discordapp.com/assets/4f49bdf61321f42eb0cce4da7bedc541.svg"},
			{"\\uD83D\\uDE9B", "https://discordapp.com/assets/350f1a3285b97e9901ff99d7f949aacc.svg"},
			{"\\uD83D\\uDE9C", "https://discordapp.com/assets/7ef22cb858ff3d144e47595c8effde2a.svg"},
			{"\\uD83C\\uDFCD", "https://discordapp.com/assets/a7049ab21a937d9f6176d9b782b6980d.svg"},
			{"\\uD83D\\uDEB2", "https://discordapp.com/assets/632c03a087369e4cc9f0e429acb17440.svg"},
			{"\\uD83D\\uDEA8", "https://discordapp.com/assets/6bf5a182b60b806be39de28837ecbaa1.svg"},
			{"\\uD83D\\uDE94", "https://discordapp.com/assets/6c3c4f357534c6f279e4743dc120baa6.svg"},
			{"\\uD83D\\uDE8D", "https://discordapp.com/assets/3843508d0a1fda2c0766304a1d253846.svg"},
			{"\\uD83D\\uDE98", "https://discordapp.com/assets/ad3c940cb714a888d0dcc80e1ea5e245.svg"},
			{"\\uD83D\\uDE96", "https://discordapp.com/assets/14f7153e13f4121d16af244aedd1e2c4.svg"},
			{"\\uD83D\\uDEA1", "https://discordapp.com/assets/ad601783c4334522845c122ea818cb5a.svg"},
			{"\\uD83D\\uDEA0", "https://discordapp.com/assets/ea8e0bcce115a3cf107a19c101f22977.svg"},
			{"\\uD83D\\uDE9F", "https://discordapp.com/assets/f4c46fbcae544e01fdb9cb9e47579f25.svg"},
			{"\\uD83D\\uDE83", "https://discordapp.com/assets/ebaa233b5d57ee7c8be025e7b5a705b0.svg"},
			{"\\uD83D\\uDE8B", "https://discordapp.com/assets/aee074549ff73c22cf56f61bc7acadc1.svg"},
			{"\\uD83D\\uDE9D", "https://discordapp.com/assets/9bb02c6ab42bf3463ea06678050a1308.svg"},
			{"\\uD83D\\uDE84", "https://discordapp.com/assets/649b6ae054a45fc6e999ba9a67b91b11.svg"},
			{"\\uD83D\\uDE85", "https://discordapp.com/assets/7794fd0349081f37e74c0c9c4a68a76f.svg"},
			{"\\uD83D\\uDE88", "https://discordapp.com/assets/68c8e7fade0798479dd54009e7c9d229.svg"},
			{"\\uD83D\\uDE9E", "https://discordapp.com/assets/e8b9f89711b04c2cb394d931f34813b7.svg"},
			{"\\uD83D\\uDE82", "https://discordapp.com/assets/c2359deddfc0aeeb0dadfd0e2e053e7f.svg"},
			{"\\uD83D\\uDE86", "https://discordapp.com/assets/acf8872f0f7b0f28966106dd96cde2f5.svg"},
			{"\\uD83D\\uDE87", "https://discordapp.com/assets/cd5e6a7c580c7a628d566a24377e988d.svg"},
			{"\\uD83D\\uDE8A", "https://discordapp.com/assets/b419ee03f12c909d7bb8a41f05a48628.svg"},
			{"\\uD83D\\uDE89", "https://discordapp.com/assets/3087acef97d5f2b28f0317cf114c6811.svg"},
			{"\\uD83D\\uDE81", "https://discordapp.com/assets/77f150891732342bfe04a212f9cc7c3b.svg"},
			{"\\uD83D\\uDEE9", "https://discordapp.com/assets/2111c60fe0d985eec6fb7fa6e8e7d24e.svg"},
			{"\\u2708", "https://discordapp.com/assets/bf8dcded6085559b2eaa41b5af25023e.svg"},
			{"\\uD83D\\uDEEB", "https://discordapp.com/assets/5482bfb5e445e124c39ada6827f63dee.svg"},
			{"\\uD83D\\uDEEC", "https://discordapp.com/assets/8dbf58d4961760e135928f4199e5b419.svg"},
			{"\\u26F5", "https://discordapp.com/assets/718c581ad949f6cf29d46c53f6a3f49b.svg"},
			{"\\uD83D\\uDEE5", "https://discordapp.com/assets/a5f633ebd99acbbe8817295fe576432f.svg"},
			{"\\uD83D\\uDEA4", "https://discordapp.com/assets/6a134d6b7933d5d66f8accfe0931beca.svg"},
			{"\\u26F4", "https://discordapp.com/assets/49139cc68b1d980a69fc29418d7df036.svg"},
			{"\\uD83D\\uDEF3", "https://discordapp.com/assets/47b71c0152f0830d021ac7416065a4f1.svg"},
			{"\\uD83D\\uDE80", "https://discordapp.com/assets/bc84b70369161737d127f8ed288dd43f.svg"},
			{"\\uD83D\\uDEF0", "https://discordapp.com/assets/8950ef013fdf4af550448d7f21df763a.svg"},
			{"\\uD83D\\uDCBA", "https://discordapp.com/assets/65732748e8d4e9d883f7d1438b581e7d.svg"},
			{"\\u2693", "https://discordapp.com/assets/654ebf14946cdb6e00310d792b2c97ea.svg"},
			{"\\uD83D\\uDEA7", "https://discordapp.com/assets/184ecbf5e4f44c79dc1c83b5363b9a9a.svg"},
			{"\\u26FD", "https://discordapp.com/assets/cbb2bfeee5a9aad3d9c47b700e7b0da6.svg"},
			{"\\uD83D\\uDE8F", "https://discordapp.com/assets/b0babd67d17035a8cec87b7b19ac3291.svg"},
			{"\\uD83D\\uDEA6", "https://discordapp.com/assets/1528601373aa5d9bdadb29f805d9726e.svg"},
			{"\\uD83D\\uDEA5", "https://discordapp.com/assets/a933220a202b461a84eb8eec5294d484.svg"},
			{"\\uD83C\\uDFC1", "https://discordapp.com/assets/e4f25e5fa71c595bedb946c8e9d7e705.svg"},
			{"\\uD83D\\uDEA2", "https://discordapp.com/assets/67a60f00862e4c53828e3cbbffcb44ad.svg"},
			{"\\uD83C\\uDFA1", "https://discordapp.com/assets/15f791f008a6df0056defdd8f728065d.svg"},
			{"\\uD83C\\uDFA2", "https://discordapp.com/assets/cda117c8a77c5586180130e7dd84c9c9.svg"},
			{"\\uD83C\\uDFA0", "https://discordapp.com/assets/e16c4e0b02500c326f7630b670b568c4.svg"},
			{"\\uD83C\\uDFD7", "https://discordapp.com/assets/3cb171198ee370995f31d71a351e4479.svg"},
			{"\\uD83C\\uDF01", "https://discordapp.com/assets/2c29a58a910e3a8b259b970a7062c421.svg"},
			{"\\uD83D\\uDDFC", "https://discordapp.com/assets/4cc687393878e00af08aeb97c3856ff0.svg"},
			{"\\uD83C\\uDFED", "https://discordapp.com/assets/e2d7d5a149db87a3d32a906fbe391841.svg"},
			{"\\u26F2", "https://discordapp.com/assets/463f88232b3b00cfc52e812b41db4260.svg"},
			{"\\uD83C\\uDF91", "https://discordapp.com/assets/ae5602ae96ca86cd8758f83cc83578bf.svg"},
			{"\\u26F0", "https://discordapp.com/assets/220cfd7325452391530ef9eb5d531661.svg"},
			{"\\uD83C\\uDFD4", "https://discordapp.com/assets/1f243d997a3ed0db6ba05f3e1fec8014.svg"},
			{"\\uD83D\\uDDFB", "https://discordapp.com/assets/2688a5bcbad5b071a8c0dc7a63b094b9.svg"},
			{"\\uD83C\\uDF0B", "https://discordapp.com/assets/d176b3056d8d190eb654a0fd10e6e290.svg"},
			{"\\uD83D\\uDDFE", "https://discordapp.com/assets/440fbdedb8e9643fb54bbec13d1d7525.svg"},
			{"\\uD83C\\uDFD5", "https://discordapp.com/assets/31de7533e976bc9195ba03e62fc061de.svg"},
			{"\\u26FA", "https://discordapp.com/assets/d24b29f7a189b5725cc654315fcb0463.svg"},
			{"\\uD83C\\uDFDE", "https://discordapp.com/assets/91820e51630aa00423526631bbebb769.svg"},
			{"\\uD83D\\uDEE3", "https://discordapp.com/assets/0e5882173de9ebc7df00fd1d7b06f90a.svg"},
			{"\\uD83D\\uDEE4", "https://discordapp.com/assets/06f47dd9f4c2d8bc7814832bfe0cb46f.svg"},
			{"\\uD83C\\uDF05", "https://discordapp.com/assets/09508d6d0cd7f3a2951e7ea99947dd86.svg"},
			{"\\uD83C\\uDF04", "https://discordapp.com/assets/75fe725460e3dc0ee55505c8b2fdc7f5.svg"},
			{"\\uD83C\\uDFDC", "https://discordapp.com/assets/99d70e4c68f53a18c2ce3f6f1ec91e9b.svg"},
			{"\\uD83C\\uDFD6", "https://discordapp.com/assets/275ae7c450b8262543d6a60e97642016.svg"},
			{"\\uD83C\\uDFDD", "https://discordapp.com/assets/b4006e1739443dbda80c2d0f0c046c8c.svg"},
			{"\\uD83C\\uDF07", "https://discordapp.com/assets/9977cba86d13064d6cf530c3640d1e1d.svg"},
			{"\\uD83C\\uDF06", "https://discordapp.com/assets/e11c9b8bf583f75155f53c274778aace.svg"},
			{"\\uD83C\\uDFD9", "https://discordapp.com/assets/363bbba4eb98c97d0bb4e3943e6f8fa1.svg"},
			{"\\uD83C\\uDF03", "https://discordapp.com/assets/2bc72481fc19657f00f8b5434088adf2.svg"},
			{"\\uD83C\\uDF09", "https://discordapp.com/assets/44f1bf70f1494c759b9bf0afc95304fd.svg"},
			{"\\uD83C\\uDF0C", "https://discordapp.com/assets/62067ffe12d7e210c8c806d11ad16452.svg"},
			{"\\uD83C\\uDF20", "https://discordapp.com/assets/fbc0ca8cac9f43e794b676cb7fd6cc9b.svg"},
			{"\\uD83C\\uDF87", "https://discordapp.com/assets/16b88c2a4eaaf06abb389d8b7c06bb39.svg"},
			{"\\uD83C\\uDF86", "https://discordapp.com/assets/197b958b01f6012cd753e543c3efb214.svg"},
			{"\\uD83C\\uDF08", "https://discordapp.com/assets/4691e32e64eb0d4c43f29252415cfd61.svg"},
			{"\\uD83C\\uDFD8", "https://discordapp.com/assets/01e114bffc312c399a9b8200143467a6.svg"},
			{"\\uD83C\\uDFF0", "https://discordapp.com/assets/94c64ab43aa561c6c14d5168dfc816e9.svg"},
			{"\\uD83C\\uDFEF", "https://discordapp.com/assets/c603d975d1311a7caa2d0d0e8790b270.svg"},
			{"\\uD83C\\uDFDF", "https://discordapp.com/assets/94d120956308f3a8d120f10fca3c321e.svg"},
			{"\\uD83D\\uDDFD", "https://discordapp.com/assets/7b09d6f8dbf82ad8e8374273804923a8.svg"},
			{"\\uD83C\\uDFE0", "https://discordapp.com/assets/904b7c9326d7282cce83957ae55fe61a.svg"},
			{"\\uD83C\\uDFE1", "https://discordapp.com/assets/33c10c78983469d46ac697db0870650f.svg"},
			{"\\uD83C\\uDFDA", "https://discordapp.com/assets/17dabf6188ff210e65f51f2ab425f8e0.svg"},
			{"\\uD83C\\uDFE2", "https://discordapp.com/assets/fe20714075186ac42f7bcb87eb1d5852.svg"},
			{"\\uD83C\\uDFEC", "https://discordapp.com/assets/e0d24860dc35915deed371660ce20e43.svg"},
			{"\\uD83C\\uDFE3", "https://discordapp.com/assets/b685facca0cc20ad482b031b29f290bb.svg"},
			{"\\uD83C\\uDFE4", "https://discordapp.com/assets/829b194fe7f4c20a60b9e9de764b33d7.svg"},
			{"\\uD83C\\uDFE5", "https://discordapp.com/assets/b54a27edba0a9ebc8bfc0fee86c10025.svg"},
			{"\\uD83C\\uDFE6", "https://discordapp.com/assets/987e6e5ad6b73717f167e124c9776eb3.svg"},
			{"\\uD83C\\uDFE8", "https://discordapp.com/assets/6a5f0525a51eec6c7bcf1a00c799e935.svg"},
			{"\\uD83C\\uDFEA", "https://discordapp.com/assets/970584d4400be7b36337b182e5919032.svg"},
			{"\\uD83C\\uDFEB", "https://discordapp.com/assets/6cc6f3b71b909ad12a822a07a60378cc.svg"},
			{"\\uD83C\\uDFE9", "https://discordapp.com/assets/7637cf80b97fd2e92befa6b0e6548b9a.svg"},
			{"\\uD83D\\uDC92", "https://discordapp.com/assets/ef10576152f5e5b78576f248134e79fd.svg"},
			{"\\uD83C\\uDFDB", "https://discordapp.com/assets/6c36cca35a8dfcd43876f0ddd29cbe12.svg"},
			{"\\u26EA", "https://discordapp.com/assets/2df63924603fd92b6031d4fb39fefa81.svg"},
			{"\\uD83D\\uDD4C", "https://discordapp.com/assets/904217d8ac9f9bd10ece3a2e523aee67.svg"},
			{"\\uD83D\\uDD4D", "https://discordapp.com/assets/d05e44e847a6f011a9f87cf90b750973.svg"},
			{"\\uD83D\\uDD4B", "https://discordapp.com/assets/6879e68e7e2dc60c3e1ce4004b0e9d15.svg"},
			{"\\u26E9", "https://discordapp.com/assets/962240d953609546c8792671a429fdd4.svg"},
			{"\\uD83D\\uDEF4", "https://discordapp.com/assets/899bd1c48c78aec311fbea9eaf891f46.svg"},
			{"\\uD83D\\uDEF5", "https://discordapp.com/assets/8e2ae381e00ef88f0299ae19556e0172.svg"},
			{"\\uD83D\\uDEF6", "https://discordapp.com/assets/1c184dd1f8ca4272905cec9360f9c5c7.svg"},
			{"\\u231A", "https://discordapp.com/assets/d4a3d47fbba119a003b11ee3c00e1936.svg"},
			{"\\uD83D\\uDCF1", "https://discordapp.com/assets/7d2027c235ae77bf376436b259acd6cd.svg"},
			{"\\uD83D\\uDCF2", "https://discordapp.com/assets/f88772439640d4ead866a77999d84464.svg"},
			{"\\uD83D\\uDCBB", "https://discordapp.com/assets/298d413d629b3b90d57400241dc1c041.svg"},
			{"\\u2328", "https://discordapp.com/assets/f21f7f8b3a3f188d267438d936be035f.svg"},
			{"\\uD83D\\uDDA5", "https://discordapp.com/assets/e2154b14424ed3161ed7a51b0f06d07e.svg"},
			{"\\uD83D\\uDDA8", "https://discordapp.com/assets/d8ad3595252449442e0c1578e467cb5a.svg"},
			{"\\uD83D\\uDDB1", "https://discordapp.com/assets/56f9ef06f28f6a472dfc3403ae1cf93c.svg"},
			{"\\uD83D\\uDDB2", "https://discordapp.com/assets/cfeca2e987d576e623e29717d1b58ca3.svg"},
			{"\\uD83D\\uDD79", "https://discordapp.com/assets/536c2c45ade326ef4197eb48c75cff6a.svg"},
			{"\\uD83D\\uDDDC", "https://discordapp.com/assets/a184a1cc32bccf9bddf49d66d06118af.svg"},
			{"\\uD83D\\uDCBD", "https://discordapp.com/assets/f4954ce09da70997331a55ea959ac3d8.svg"},
			{"\\uD83D\\uDCBE", "https://discordapp.com/assets/d049def26c077694f4f184be88cea9bb.svg"},
			{"\\uD83D\\uDCBF", "https://discordapp.com/assets/a2527ed63fbfe469cd8973970c6278a1.svg"},
			{"\\uD83D\\uDCC0", "https://discordapp.com/assets/6e829dbdc26783477c653d4a6fd6587c.svg"},
			{"\\uD83D\\uDCFC", "https://discordapp.com/assets/cb5fc2bda212a864f47e6724549c9be6.svg"},
			{"\\uD83D\\uDCF7", "https://discordapp.com/assets/57aea9031650f92408cb1d43f355fc74.svg"},
			{"\\uD83D\\uDCF8", "https://discordapp.com/assets/db199b8adb93c96fa1a8f03f69ca55e9.svg"},
			{"\\uD83D\\uDCF9", "https://discordapp.com/assets/169565d0b5f43d2c5da4dd26bb54b160.svg"},
			{"\\uD83C\\uDFA5", "https://discordapp.com/assets/8f58e04ab1a6dd3c9798877912c2cb93.svg"},
			{"\\uD83D\\uDCFD", "https://discordapp.com/assets/dd21835e0b93364676fce48d9e81c609.svg"},
			{"\\uD83C\\uDF9E", "https://discordapp.com/assets/a9c4bdab2900a9549cc718509c3ce323.svg"},
			{"\\uD83D\\uDCDE", "https://discordapp.com/assets/2d6a964ec20df9bf319d7858ed43bf75.svg"},
			{"\\u260E", "https://discordapp.com/assets/8ce180bda86f59e517eb6a1d0ed84eef.svg"},
			{"\\uD83D\\uDCDF", "https://discordapp.com/assets/146a26610c15c434d6809c72a5db6052.svg"},
			{"\\uD83D\\uDCE0", "https://discordapp.com/assets/da46757cdddad0507ddb04c48b7a9277.svg"},
			{"\\uD83D\\uDCFA", "https://discordapp.com/assets/5c2bef02d6ffc10c89f544c32c04ed46.svg"},
			{"\\uD83D\\uDCFB", "https://discordapp.com/assets/4b101099c8f43f00b16ec8712592dd07.svg"},
			{"\\uD83C\\uDF99", "https://discordapp.com/assets/269f6e131a6014360770f946afb002e9.svg"},
			{"\\uD83C\\uDF9A", "https://discordapp.com/assets/ea95086d85218b1edebcd3d1246878ea.svg"},
			{"\\uD83C\\uDF9B", "https://discordapp.com/assets/a0bf1bc997cdbd8115f3dbc8fd38f69d.svg"},
			{"\\u23F1", "https://discordapp.com/assets/e0bbe23c466e33530baf918b9634c4d3.svg"},
			{"\\u23F2", "https://discordapp.com/assets/b6f4905d7bad38e1a18782bf33616999.svg"},
			{"\\u23F0", "https://discordapp.com/assets/02b72128433bb14721cfa96689722dac.svg"},
			{"\\uD83D\\uDD70", "https://discordapp.com/assets/ab5b4d44e26e2cc47eb0da7289a93b75.svg"},
			{"\\u23F3", "https://discordapp.com/assets/3c49d00d065c85a89b996254e7423c33.svg"},
			{"\\u231B", "https://discordapp.com/assets/04f7d73e7ce036a854ff6aa9b24473d7.svg"},
			{"\\uD83D\\uDCE1", "https://discordapp.com/assets/d40cf1be5adc53731d8eea69fd2d6680.svg"},
			{"\\uD83D\\uDD0B", "https://discordapp.com/assets/7d9c752d553d6da23acf9c37323bcfe0.svg"},
			{"\\uD83D\\uDD0C", "https://discordapp.com/assets/7723ee55f259edfcc5bd5cb5d162bf3e.svg"},
			{"\\uD83D\\uDCA1", "https://discordapp.com/assets/d05c13355ab94b98d3c8ba0367c9b8f8.svg"},
			{"\\uD83D\\uDD26", "https://discordapp.com/assets/71a39123ea46d6cccac08a3754b4f7f5.svg"},
			{"\\uD83D\\uDD6F", "https://discordapp.com/assets/7a88ba7973ebbd228d236ab960ff280a.svg"},
			{"\\uD83D\\uDDD1", "https://discordapp.com/assets/0d6fbd1bceb7a00e24106fcf331cd9f4.svg"},
			{"\\uD83D\\uDEE2", "https://discordapp.com/assets/7757b499acbd14820e76d4bf228ffc86.svg"},
			{"\\uD83D\\uDCB8", "https://discordapp.com/assets/630828f0eaa647bf465b3b903b0dbc5f.svg"},
			{"\\uD83D\\uDCB5", "https://discordapp.com/assets/8ed2cc6920647efb9ed69ccc429fcee4.svg"},
			{"\\uD83D\\uDCB4", "https://discordapp.com/assets/33b5131336d90154281bc3fe328612e3.svg"},
			{"\\uD83D\\uDCB6", "https://discordapp.com/assets/e52ac65bc325dab0002455da28e4290f.svg"},
			{"\\uD83D\\uDCB7", "https://discordapp.com/assets/5f3999f6377384648ff727986091fb55.svg"},
			{"\\uD83D\\uDCB0", "https://discordapp.com/assets/ccebe0b729ff7530c5e37dbbd9f9938c.svg"},
			{"\\uD83D\\uDCB3", "https://discordapp.com/assets/452c49725a978a79195f81f7c2cf878c.svg"},
			{"\\uD83D\\uDC8E", "https://discordapp.com/assets/de1b252908d56824c94ecc7152f226b8.svg"},
			{"\\u2696", "https://discordapp.com/assets/e38daa2f258f6ef1987f8b49aecf1487.svg"},
			{"\\uD83D\\uDD27", "https://discordapp.com/assets/7247ff3d7a609104aa79cf391ea269b6.svg"},
			{"\\uD83D\\uDD28", "https://discordapp.com/assets/596bd0f8541debff8d44326e840ea085.svg"},
			{"\\u2692", "https://discordapp.com/assets/ef015a42cac1a2009f9eecf8915d9e35.svg"},
			{"\\uD83D\\uDEE0", "https://discordapp.com/assets/78200fb6296bd2ab02a834120606ae82.svg"},
			{"\\u26CF", "https://discordapp.com/assets/4d61ad14c090b4d6a2d1c989d78bc273.svg"},
			{"\\uD83D\\uDD29", "https://discordapp.com/assets/88ac2ca95abdde80b6e85f08c603d379.svg"},
			{"\\u2699", "https://discordapp.com/assets/c61c8e1ffdcbf98496bc098c35f0f694.svg"},
			{"\\u26D3", "https://discordapp.com/assets/64e220f66696627a95ea507ea09e9c9b.svg"},
			{"\\uD83D\\uDD2B", "https://discordapp.com/assets/3071dbc60204c84ca0cf423b8b08a204.svg"},
			{"\\uD83D\\uDCA3", "https://discordapp.com/assets/cec604bb59b00a2ef838c1c0bf34bc1d.svg"},
			{"\\uD83D\\uDD2A", "https://discordapp.com/assets/d8a6c0d6612fa1727117b00dd53665fc.svg"},
			{"\\uD83D\\uDDE1", "https://discordapp.com/assets/6a0cd251193e14e8fef4b258072f7400.svg"},
			{"\\u2694", "https://discordapp.com/assets/54e1b293350b4fdc172e2b5fb7e28bd8.svg"},
			{"\\uD83D\\uDEE1", "https://discordapp.com/assets/55c17959c1bb6b0891a94ccde3ba996b.svg"},
			{"\\uD83D\\uDEAC", "https://discordapp.com/assets/939451d132d1419d57158f8e38aad0b7.svg"},
			{"\\u2620", "https://discordapp.com/assets/eb2086bb1a6861d1c3eb6e8b2dd29ee8.svg"},
			{"\\u26B0", "https://discordapp.com/assets/ad3d49ad8c1fa9b5955ecdf67814727e.svg"},
			{"\\u26B1", "https://discordapp.com/assets/3fb75b9b7769319c978740cb5ac68ebd.svg"},
			{"\\uD83C\\uDFFA", "https://discordapp.com/assets/de1a2435b75d7424d1b2b907114ae688.svg"},
			{"\\uD83D\\uDD2E", "https://discordapp.com/assets/1c0daf2a40e63a1f59c1ae036c19adf7.svg"},
			{"\\uD83D\\uDCFF", "https://discordapp.com/assets/dd833e9a6b27f767eb7002e613047d03.svg"},
			{"\\uD83D\\uDC88", "https://discordapp.com/assets/9eab2519f1935bd91c427887710bd111.svg"},
			{"\\u2697", "https://discordapp.com/assets/2bab4c60959e6a163bcac46e012a613c.svg"},
			{"\\uD83D\\uDD2D", "https://discordapp.com/assets/d09a3ee01beceb01255e739e15c4c5f9.svg"},
			{"\\uD83D\\uDD2C", "https://discordapp.com/assets/f6a262ddcccdd48c4b4bca70fc67342e.svg"},
			{"\\uD83D\\uDD73", "https://discordapp.com/assets/b8c564524314f606251429fdd96d6d4c.svg"},
			{"\\uD83D\\uDC8A", "https://discordapp.com/assets/4829c20f23f41db51bc88122046d9759.svg"},
			{"\\uD83D\\uDC89", "https://discordapp.com/assets/ff63c8cfce7f17a8f00324ab0f891acb.svg"},
			{"\\uD83C\\uDF21", "https://discordapp.com/assets/c5378fec22ac945da535a25e89322173.svg"},
			{"\\uD83C\\uDFF7", "https://discordapp.com/assets/71419b0db7e6c2ae6cb2e27845bffc98.svg"},
			{"\\uD83D\\uDD16", "https://discordapp.com/assets/aab936f252f4221f5d0b92b20fa67f7a.svg"},
			{"\\uD83D\\uDEBD", "https://discordapp.com/assets/f69e1da6035d893a6a62782c5befd743.svg"},
			{"\\uD83D\\uDEBF", "https://discordapp.com/assets/9114eb3dd1724a82d71dbae885db13f0.svg"},
			{"\\uD83D\\uDEC1", "https://discordapp.com/assets/d1ee4664d81f783dfb02a096cc634d26.svg"},
			{"\\uD83D\\uDD11", "https://discordapp.com/assets/2bd74ab02e74e380eeae88a077051671.svg"},
			{"\\uD83D\\uDDDD", "https://discordapp.com/assets/e11f01ddc5cb675d9a36c52f5850cb92.svg"},
			{"\\uD83D\\uDECB", "https://discordapp.com/assets/68116c4b2fc5e16858a6062336a09920.svg"},
			{"\\uD83D\\uDECC", "https://discordapp.com/assets/9dde06f4675996347c363c6eee93c142.svg"},
			{"\\uD83D\\uDECF", "https://discordapp.com/assets/cb869f7cf0aadf77f1baabc33b97d96b.svg"},
			{"\\uD83D\\uDEAA", "https://discordapp.com/assets/4c231cb7c6f5bb352261a2e0bfa63fb1.svg"},
			{"\\uD83D\\uDECE", "https://discordapp.com/assets/eb26be30e5b0e617e84f2665ffb79404.svg"},
			{"\\uD83D\\uDDBC", "https://discordapp.com/assets/8237092de4f32d8777ab016b5fc8c093.svg"},
			{"\\uD83D\\uDDFA", "https://discordapp.com/assets/7661ca808d3be4d7292d9fd97e3c9b06.svg"},
			{"\\u26F1", "https://discordapp.com/assets/64d2d7d8d4d762ef5401d5bab84aadcc.svg"},
			{"\\uD83D\\uDDFF", "https://discordapp.com/assets/d0e0c9b76514a1c6cb8b9b988b05d2e9.svg"},
			{"\\uD83D\\uDECD", "https://discordapp.com/assets/6090d49b641e0667f6bec4038019163b.svg"},
			{"\\uD83C\\uDF88", "https://discordapp.com/assets/1b728acb33beb9640f696250c3de170d.svg"},
			{"\\uD83C\\uDF8F", "https://discordapp.com/assets/711fda2ac0c54e27bff54fd9a0d13565.svg"},
			{"\\uD83C\\uDF80", "https://discordapp.com/assets/c9044dba240b715f6d8039d627fa9e4e.svg"},
			{"\\uD83C\\uDF81", "https://discordapp.com/assets/739c1934cfb00cde067e3d45d49c5a45.svg"},
			{"\\uD83C\\uDF8A", "https://discordapp.com/assets/63fd532a87e1a7957e0534db68dbec9d.svg"},
			{"\\uD83C\\uDF89", "https://discordapp.com/assets/612f3fc9dedfd368820b55c4cf259c07.svg"},
			{"\\uD83C\\uDF8E", "https://discordapp.com/assets/a1dde7ef44b8960c407c4c1e0a68f72e.svg"},
			{"\\uD83C\\uDF90", "https://discordapp.com/assets/21d68051c7a4301c231d206f9dd40d1a.svg"},
			{"\\uD83C\\uDF8C", "https://discordapp.com/assets/b5c7ee42504027bef1d01c238f8229f6.svg"},
			{"\\uD83C\\uDFEE", "https://discordapp.com/assets/ea7399fa27ed7d125c181b67b5b0468c.svg"},
			{"\\u2709", "https://discordapp.com/assets/ccacda3342ed64236227eeabfa0f7dfa.svg"},
			{"\\uD83D\\uDCE9", "https://discordapp.com/assets/73cc87b44ecc2c2520700ef5498ce779.svg"},
			{"\\uD83D\\uDCE8", "https://discordapp.com/assets/9dc925a76f140aa24edec4d532a251bd.svg"},
			{"\\uD83D\\uDCE7", "https://discordapp.com/assets/86ac473ccadf924d6b8a1cfec2417d63.svg"},
			{"\\uD83D\\uDC8C", "https://discordapp.com/assets/2ae5d700ad126cd09e9ebe000f795c3d.svg"},
			{"\\uD83D\\uDCEE", "https://discordapp.com/assets/078b79244f2af1b793bc30b8fb21f4e6.svg"},
			{"\\uD83D\\uDCEA", "https://discordapp.com/assets/66a96d7cf3085ae40ace635f02267404.svg"},
			{"\\uD83D\\uDCEB", "https://discordapp.com/assets/099e7b57a3a594470fe78a59f58a253e.svg"},
			{"\\uD83D\\uDCEC", "https://discordapp.com/assets/d16c2e0135e0919fef9613e5b699601d.svg"},
			{"\\uD83D\\uDCED", "https://discordapp.com/assets/78fa1297ab34414c5483712201f03c8c.svg"},
			{"\\uD83D\\uDCE6", "https://discordapp.com/assets/c8a51a3e9444e4642c9426b45cb8b553.svg"},
			{"\\uD83D\\uDCEF", "https://discordapp.com/assets/a6a32075f4ca20bdfcbc812d6aed0009.svg"},
			{"\\uD83D\\uDCE5", "https://discordapp.com/assets/bbe497307fd7ede70e9561775c8ee112.svg"},
			{"\\uD83D\\uDCE4", "https://discordapp.com/assets/52576a2727e396c00ef7e3c664711173.svg"},
			{"\\uD83D\\uDCDC", "https://discordapp.com/assets/b7f66cb5c74fb6d0446459b0d9fe0899.svg"},
			{"\\uD83D\\uDCC3", "https://discordapp.com/assets/65deef156089c71e6dd16d9261d85dad.svg"},
			{"\\uD83D\\uDCD1", "https://discordapp.com/assets/ffcdb50ce310bfbe221f01a8e72034a8.svg"},
			{"\\uD83D\\uDCCA", "https://discordapp.com/assets/79a590ea84f8e3a347aa62b32b78e0d5.svg"},
			{"\\uD83D\\uDCC8", "https://discordapp.com/assets/dee149ab9c268e7b00744f0c40f1c7c3.svg"},
			{"\\uD83D\\uDCC9", "https://discordapp.com/assets/ddffbe71df7eb0cae001d9a4667084e1.svg"},
			{"\\uD83D\\uDCC4", "https://discordapp.com/assets/0aeefa18b249262cd4b8acf15149e3a0.svg"},
			{"\\uD83D\\uDCC5", "https://discordapp.com/assets/6aa386cf3b974f63cc39ce0b21822fbe.svg"},
			{"\\uD83D\\uDCC6", "https://discordapp.com/assets/3511cfe60e427f37d59c07548c8a3c8e.svg"},
			{"\\uD83D\\uDDD3", "https://discordapp.com/assets/644ab12f2f874b0c5fb5b5b5f88a0bef.svg"},
			{"\\uD83D\\uDCC7", "https://discordapp.com/assets/9130dfff888fdec6ece8dc31bb1060ff.svg"},
			{"\\uD83D\\uDDC3", "https://discordapp.com/assets/3f19971e1ed28b05a799827e337fd9fe.svg"},
			{"\\uD83D\\uDDF3", "https://discordapp.com/assets/8d2d76ac3edd946edd24776ac421b921.svg"},
			{"\\uD83D\\uDDC4", "https://discordapp.com/assets/0566e35a8bd81019c089f10058834a51.svg"},
			{"\\uD83D\\uDCCB", "https://discordapp.com/assets/7ecc37fc06a692387dc1c886e5eecc94.svg"},
			{"\\uD83D\\uDDD2", "https://discordapp.com/assets/908f9e562fb536d4f05c0e1d651ca6fe.svg"},
			{"\\uD83D\\uDCC1", "https://discordapp.com/assets/d68327d74508465432fe6e9dee35b9ff.svg"},
			{"\\uD83D\\uDCC2", "https://discordapp.com/assets/d05ed3c514abe6ef766928911748c431.svg"},
			{"\\uD83D\\uDDC2", "https://discordapp.com/assets/312bf4b5ed50687d0fa58dd59a3a54c8.svg"},
			{"\\uD83D\\uDDDE", "https://discordapp.com/assets/9da4147e4dc3afde7c67147afc0cb6c3.svg"},
			{"\\uD83D\\uDCF0", "https://discordapp.com/assets/d36b33903dafb0107bb067b55bdd9cbc.svg"},
			{"\\uD83D\\uDCD3", "https://discordapp.com/assets/32d8174b427d411c1c25119104ef1588.svg"},
			{"\\uD83D\\uDCD5", "https://discordapp.com/assets/e7fe52cb5188f123ea76492c4d1c156c.svg"},
			{"\\uD83D\\uDCD7", "https://discordapp.com/assets/613a24d7aea511a069db17607093a3d4.svg"},
			{"\\uD83D\\uDCD8", "https://discordapp.com/assets/f4510c6e5fec94458dd126b6b6eba558.svg"},
			{"\\uD83D\\uDCD9", "https://discordapp.com/assets/a7749e82b91fa892ce2d52bed286790b.svg"},
			{"\\uD83D\\uDCD4", "https://discordapp.com/assets/22c88b5a9f093e77043438feed1574aa.svg"},
			{"\\uD83D\\uDCD2", "https://discordapp.com/assets/5a94ca0d1453e052afc2609cfa75c7c6.svg"},
			{"\\uD83D\\uDCDA", "https://discordapp.com/assets/24eb87f78b76a8fdfad248afa701cc49.svg"},
			{"\\uD83D\\uDCD6", "https://discordapp.com/assets/09cfc9ecdfef219134f927c54129e96d.svg"},
			{"\\uD83D\\uDD17", "https://discordapp.com/assets/c4dfcf1f6ab66820a967f7c428454b83.svg"},
			{"\\uD83D\\uDCCE", "https://discordapp.com/assets/71e2667a9e9ed67d461fa8e9d3567d4a.svg"},
			{"\\uD83D\\uDD87", "https://discordapp.com/assets/30420d7a51fbaa836b521486226cf3d9.svg"},
			{"\\u2702", "https://discordapp.com/assets/c6022f827b7d2c59ab3ce4bf216b8d85.svg"},
			{"\\uD83D\\uDCD0", "https://discordapp.com/assets/b43c5c7f3fd21d04273303bc0db9cc02.svg"},
			{"\\uD83D\\uDCCF", "https://discordapp.com/assets/2fd5575c94854d956294a5dd6531628e.svg"},
			{"\\uD83D\\uDCCC", "https://discordapp.com/assets/e770e7da3fb872af10856268118a6e34.svg"},
			{"\\uD83D\\uDCCD", "https://discordapp.com/assets/df0fba917be2ad5fc7939019465de627.svg"},
			{"\\uD83D\\uDEA9", "https://discordapp.com/assets/a1f0c106b0a0f68f6b11c2dc0cc8d249.svg"},
			{"\\uD83C\\uDFF3", "https://discordapp.com/assets/1cc9f49b88b3373dd11112833af08826.svg"},
			{"\\uD83C\\uDFF4", "https://discordapp.com/assets/183718e983ac8ed0bc00c0c485ee8d7e.svg"},
			{"\\uD83D\\uDD10", "https://discordapp.com/assets/c85eb95ca88432c061e251b40b7e1983.svg"},
			{"\\uD83D\\uDD12", "https://discordapp.com/assets/86c36b8437a0bc80cf310733f54257c2.svg"},
			{"\\uD83D\\uDD13", "https://discordapp.com/assets/4f1e1fa42efdf4de983e3f609d56eb4c.svg"},
			{"\\uD83D\\uDD0F", "https://discordapp.com/assets/d72f52ce6c418c5c8fd5faac0e8c36ff.svg"},
			{"\\uD83D\\uDD8A", "https://discordapp.com/assets/00e8fe627e9bd3cedc9c07f5640b654c.svg"},
			{"\\uD83D\\uDD8B", "https://discordapp.com/assets/a307f008c0df67604bf9d07f4c11f6c2.svg"},
			{"\\u2712", "https://discordapp.com/assets/d88d46379fae148966f07b4a079528df.svg"},
			{"\\uD83D\\uDCDD", "https://discordapp.com/assets/a77e0fdf1c1dddb8de08f3b67a971bff.svg"},
			{"\\u270F", "https://discordapp.com/assets/bb12f326a3b4f486797c47af2480d66f.svg"},
			{"\\uD83D\\uDD8D", "https://discordapp.com/assets/f3b7783bfabe86069aa1f506f602496a.svg"},
			{"\\uD83D\\uDD8C", "https://discordapp.com/assets/35665b6147e6ea2d0a8c6cb759d4a281.svg"},
			{"\\uD83D\\uDD0D", "https://discordapp.com/assets/3896096ba07324c04ed0fe7e1acc3643.svg"},
			{"\\uD83D\\uDD0E", "https://discordapp.com/assets/5e9d1e5a1536cf6e2fcaf05f3eaf64dc.svg"},
			{"\\uD83D\\uDED2", "https://discordapp.com/assets/ad7bb0b7409afab511bb9c4d5959b8c7.svg"},
			{"\\uD83D\\uDCAF", "https://discordapp.com/assets/6f1049fe11f5b6bc18d9227fb29d237b.svg"},
			{"\\uD83D\\uDD22", "https://discordapp.com/assets/c64559ce7db12f6dea3404fc44e42b96.svg"},
			{"\\u2764", "https://discordapp.com/assets/dcbf6274f0ce0f393d064a72db2c8913.svg"},
			{"\\uD83D\\uDC9B", "https://discordapp.com/assets/381baae5679b73dc27b754329324491d.svg"},
			{"\\uD83D\\uDC9A", "https://discordapp.com/assets/80be7d03c69d814bd18ff86e7d5de8f3.svg"},
			{"\\uD83D\\uDC99", "https://discordapp.com/assets/599c362e721a4f13c753ebdc385725bf.svg"},
			{"\\uD83D\\uDC9C", "https://discordapp.com/assets/cc005253d0d5dfff7f7d317e164102d7.svg"},
			{"\\uD83D\\uDC94", "https://discordapp.com/assets/8fee3f6705505729fea8c7379934d794.svg"},
			{"\\u2763", "https://discordapp.com/assets/535fdf7c79c96bf2ebc63bf8c28c4a82.svg"},
			{"\\uD83D\\uDC95", "https://discordapp.com/assets/9243759187717f603a0b7e9c53cb939d.svg"},
			{"\\uD83D\\uDC9E", "https://discordapp.com/assets/3c9de8245e086437984f22da3f736eb6.svg"},
			{"\\uD83D\\uDC93", "https://discordapp.com/assets/3a19915ca846aa5ac3299a8b8f6a1bbe.svg"},
			{"\\uD83D\\uDC97", "https://discordapp.com/assets/70f567f2381c57bef68c7207047a11cf.svg"},
			{"\\uD83D\\uDC96", "https://discordapp.com/assets/a42df564f00ed8bbca652dc9345d3834.svg"},
			{"\\uD83D\\uDC98", "https://discordapp.com/assets/2edac3c81f106292d99e93a725390e15.svg"},
			{"\\uD83D\\uDC9D", "https://discordapp.com/assets/98b39a3802f75cda51a0b1b539aa7609.svg"},
			{"\\uD83D\\uDC9F", "https://discordapp.com/assets/02103c74ec6aee219711f3e119615582.svg"},
			{"\\u262E", "https://discordapp.com/assets/0d17df313552db01cac5f819a027eeeb.svg"},
			{"\\u271D", "https://discordapp.com/assets/b35b475501506952577db042be736edd.svg"},
			{"\\u262A", "https://discordapp.com/assets/8ecfe6ea3c44693dfb128a5270c0bbc8.svg"},
			{"\\uD83D\\uDD49", "https://discordapp.com/assets/f9f9458405746ad2210b6ef936526567.svg"},
			{"\\u2638", "https://discordapp.com/assets/96eb57da81fa54ab8c4626fba8416996.svg"},
			{"\\u2721", "https://discordapp.com/assets/b2581a7ae3d50d6ee9320746765d7198.svg"},
			{"\\uD83D\\uDD2F", "https://discordapp.com/assets/a76358025b30ce6b86d23dba8a4d1ce1.svg"},
			{"\\uD83D\\uDD4E", "https://discordapp.com/assets/8aaf90a53d76f5881f3e257be99e2b6c.svg"},
			{"\\u262F", "https://discordapp.com/assets/40dd1c55e485ef2d001167bb07a2658b.svg"},
			{"\\u2626", "https://discordapp.com/assets/949d2400a57304585c691dc7fb29b726.svg"},
			{"\\uD83D\\uDED0", "https://discordapp.com/assets/484469cd879958684c40b5376b61fab8.svg"},
			{"\\u26CE", "https://discordapp.com/assets/5bee9989bfe5df15382dd60b68126f94.svg"},
			{"\\u2648", "https://discordapp.com/assets/afcd9a726e8f4bc18cd6a0dc8186c1b5.svg"},
			{"\\u2649", "https://discordapp.com/assets/f90cc175929006858e436465bd7eb075.svg"},
			{"\\u264A", "https://discordapp.com/assets/2185a4cf906e7490ac6157d618bdd952.svg"},
			{"\\u264B", "https://discordapp.com/assets/38af37ef3576ec5f0f2f215e133c9e2a.svg"},
			{"\\u264C", "https://discordapp.com/assets/c5c8810b1469264a624e1141992433a5.svg"},
			{"\\u264D", "https://discordapp.com/assets/cd6f4d1cb8ed053abb221fd28767f1f3.svg"},
			{"\\u264E", "https://discordapp.com/assets/30e204d9374008ccecde7cc98643686f.svg"},
			{"\\u264F", "https://discordapp.com/assets/d8e1fbb0777c26064bf815397ba1d188.svg"},
			{"\\u2650", "https://discordapp.com/assets/44bf674252a9c89f206b22b9408b38c0.svg"},
			{"\\u2651", "https://discordapp.com/assets/4929f3f8276f650380741579e177dd77.svg"},
			{"\\u2652", "https://discordapp.com/assets/101cc8be0d6eca082212b73fc15d32a8.svg"},
			{"\\u2653", "https://discordapp.com/assets/d941df2b3f8c847501c7eb0f919c45d2.svg"},
			{"\\uD83C\\uDD94", "https://discordapp.com/assets/4bbe2360c233d56fd84a488e1c724d0d.svg"},
			{"\\u269B", "https://discordapp.com/assets/c7870519b21ef2bb85b69040e327752d.svg"},
			{"\\uD83C\\uDE33", "https://discordapp.com/assets/5f662a8a16551b942929b586f6ba6025.svg"},
			{"\\uD83C\\uDE39", "https://discordapp.com/assets/e3e43289d9ea623b43f389b6a4739d7f.svg"},
			{"\\u2622", "https://discordapp.com/assets/8198c351ed59dcb838bc8e0c8885a726.svg"},
			{"\\u2623", "https://discordapp.com/assets/a0ef880c7ab15889672c1495566042ef.svg"},
			{"\\uD83D\\uDCF4", "https://discordapp.com/assets/436f9bb1b2d2cccfae207dea96fa4fe2.svg"},
			{"\\uD83D\\uDCF3", "https://discordapp.com/assets/a3abd44adda55c39636801ca8e52450b.svg"},
			{"\\uD83C\\uDE36", "https://discordapp.com/assets/33a2054fc8ddb133f9b77bb80890d4cc.svg"},
			{"\\uD83C\\uDE1A", "https://discordapp.com/assets/2556ca47e43c136454f952dd8b8fc038.svg"},
			{"\\uD83C\\uDE38", "https://discordapp.com/assets/1deadd9669e097c128597412ac8da6d3.svg"},
			{"\\uD83C\\uDE3A", "https://discordapp.com/assets/5988fb83bc918e0c67298ddae5649b17.svg"},
			{"\\uD83C\\uDE37", "https://discordapp.com/assets/1e41f8d3a45ff4987d95290aa072aebe.svg"},
			{"\\u2734", "https://discordapp.com/assets/0732c78b1ca3eead19f00352190bd5f6.svg"},
			{"\\uD83C\\uDD9A", "https://discordapp.com/assets/88eaec49c2caccea9de21e771de5d2c7.svg"},
			{"\\uD83C\\uDE51", "https://discordapp.com/assets/e7db03d352d6957f9f2cf83fbba2e400.svg"},
			{"\\uD83D\\uDCAE", "https://discordapp.com/assets/27e1b9af2353738b51c8575b1bc30134.svg"},
			{"\\uD83C\\uDE50", "https://discordapp.com/assets/7ec607baa7e16e72348f5c2325460397.svg"},
			{"\\u3299", "https://discordapp.com/assets/535a5bdaf87e40b15cf81d8bfee80f79.svg"},
			{"\\u3297", "https://discordapp.com/assets/f059eac45dfa3143068a6b97fdd00ef9.svg"},
			{"\\uD83C\\uDE34", "https://discordapp.com/assets/003b9bfcd87dadcf6d801b8a00798292.svg"},
			{"\\uD83C\\uDE35", "https://discordapp.com/assets/583fad5f3e1f3cf97d403bd593015268.svg"},
			{"\\uD83C\\uDE32", "https://discordapp.com/assets/812c9eb9cc16b5bc5010c771792356df.svg"},
			{"\\uD83C\\uDD70", "https://discordapp.com/assets/9d089e5b57d6e0963e78689371f9fa02.svg"},
			{"\\uD83C\\uDD71", "https://discordapp.com/assets/cd5c9ccdf3c8d46d48107bfefcd4f44e.svg"},
			{"\\uD83C\\uDD8E", "https://discordapp.com/assets/f6025fcaa090a679651c6fe9d1bef154.svg"},
			{"\\uD83C\\uDD91", "https://discordapp.com/assets/e695fe6c90341251bc0fe15073c2d85b.svg"},
			{"\\uD83C\\uDD7E", "https://discordapp.com/assets/1379871eb798de836776ceb73641f44e.svg"},
			{"\\uD83C\\uDD98", "https://discordapp.com/assets/c01d9aac648747651e03ecd7425a3cae.svg"},
			{"\\u26D4", "https://discordapp.com/assets/15ccaf984f2fafcf3ed5d896763ed510.svg"},
			{"\\uD83D\\uDCDB", "https://discordapp.com/assets/833543b4e0f5175e6ff2fe80f2b1df57.svg"},
			{"\\uD83D\\uDEAB", "https://discordapp.com/assets/dc0a6320d907631d34e6655dff176295.svg"},
			{"\\u274C", "https://discordapp.com/assets/b1868d829b37f0a81533ededb9ffe5f4.svg"},
			{"\\u2B55", "https://discordapp.com/assets/1edfe735f87e201398d505124c421812.svg"},
			{"\\uD83D\\uDCA2", "https://discordapp.com/assets/5e5f4134450fe6dccdcef61ccb2f0ced.svg"},
			{"\\u2668", "https://discordapp.com/assets/b6ade0a54c6c9cffde14763f7f7a3d1e.svg"},
			{"\\uD83D\\uDEB7", "https://discordapp.com/assets/12ea85689f52f144e1529d5144864cf2.svg"},
			{"\\uD83D\\uDEAF", "https://discordapp.com/assets/3d984f2cbd6e6bc528d183afe0ddebe3.svg"},
			{"\\uD83D\\uDEB3", "https://discordapp.com/assets/c3a3491092a37bc7112d393a0275f800.svg"},
			{"\\uD83D\\uDEB1", "https://discordapp.com/assets/7568362c0e19ae556312471645d4174f.svg"},
			{"\\uD83D\\uDD1E", "https://discordapp.com/assets/03edf23717f92a677a0502874c163048.svg"},
			{"\\uD83D\\uDCF5", "https://discordapp.com/assets/24795e3f7a5ebffae763df499bfd9ea5.svg"},
			{"\\u2757", "https://discordapp.com/assets/0c4468aed0c4f665cc1ed5e5b07b52de.svg"},
			{"\\u2755", "https://discordapp.com/assets/ab1cf84eeca0da11a04680c2980edc62.svg"},
			{"\\u2753", "https://discordapp.com/assets/6e054ab8981d3f1ce8debfd1235d3ea3.svg"},
			{"\\u2754", "https://discordapp.com/assets/cef2d5ab02888e885953f945f9c39304.svg"},
			{"\\u203C", "https://discordapp.com/assets/94a29a1805306022db268195f85831fb.svg"},
			{"\\u2049", "https://discordapp.com/assets/0756351ac9eb496a210cd591acecf1d0.svg"},
			{"\\uD83D\\uDD05", "https://discordapp.com/assets/3c8a7e504001fcb17466ca55870cb357.svg"},
			{"\\uD83D\\uDD06", "https://discordapp.com/assets/d62729cf97a16fc38af3a4eb8ddc46c9.svg"},
			{"\\uD83D\\uDD31", "https://discordapp.com/assets/f9f863f85c2c4ffc8087cdac44293f38.svg"},
			{"\\u269C", "https://discordapp.com/assets/d21da75a5b9c2deaf4db75c80e28433e.svg"},
			{"\\u303D", "https://discordapp.com/assets/0859696b662433ab0f453840dc0af981.svg"},
			{"\\u26A0", "https://discordapp.com/assets/b04ecfe13d61a869b4c47a276b51b634.svg"},
			{"\\uD83D\\uDEB8", "https://discordapp.com/assets/583122438a98a4efd61c5509ad84f946.svg"},
			{"\\uD83D\\uDD30", "https://discordapp.com/assets/a3fc335f559f462df3e5d6cdbb9178e8.svg"},
			{"\\u267B", "https://discordapp.com/assets/ba21bae0723b6477627f456c587de6c4.svg"},
			{"\\uD83C\\uDE2F", "https://discordapp.com/assets/93a1ba2f70625ff7e50a5331b67afce9.svg"},
			{"\\uD83D\\uDCB9", "https://discordapp.com/assets/fe2e240d2a8b88c425391a0c740a2845.svg"},
			{"\\u2747", "https://discordapp.com/assets/895d98f334edf8fd3609d5eb34eb8871.svg"},
			{"\\u2733", "https://discordapp.com/assets/2f7fdcf194b46c1bafc18f1720bc6a6d.svg"},
			{"\\u274E", "https://discordapp.com/assets/d2f81f50b6bddfb840376afb4de21951.svg"},
			{"\\u2705", "https://discordapp.com/assets/c6b26ba81f44b0c43697852e1e1d1420.svg"},
			{"\\uD83D\\uDCA0", "https://discordapp.com/assets/4d2c2f2f933329e7409236aaeeaa7c57.svg"},
			{"\\uD83C\\uDF00", "https://discordapp.com/assets/09e6dcd34b6d48e8e85ecdc2f471df52.svg"},
			{"\\u27BF", "https://discordapp.com/assets/36342d54527e339d078dc5d9eabf3636.svg"},
			{"\\uD83C\\uDF10", "https://discordapp.com/assets/9c94147b257a3a5a0dbd0f7fcd4791df.svg"},
			{"\\u24C2", "https://discordapp.com/assets/0a216e1d772a1a9638b8d6c2acba0cab.svg"},
			{"\\uD83C\\uDFE7", "https://discordapp.com/assets/46dad66c11e8cb5ce1fd3191af0292ed.svg"},
			{"\\uD83C\\uDE02", "https://discordapp.com/assets/13137c4757a80d3a557ccbe1dcb2326c.svg"},
			{"\\uD83D\\uDEC2", "https://discordapp.com/assets/cdda47b108df90a11e4c9fd96ef1534b.svg"},
			{"\\uD83D\\uDEC3", "https://discordapp.com/assets/3e50c9d0300203598831bd391ab66af3.svg"},
			{"\\uD83D\\uDEC4", "https://discordapp.com/assets/6a3e1553634786227dab76f45c72cc6c.svg"},
			{"\\uD83D\\uDEC5", "https://discordapp.com/assets/53f7f23be1ad3dc9f20241c806a4130a.svg"},
			{"\\u267F", "https://discordapp.com/assets/4b26edda14a93364eb9fd330eef48b8a.svg"},
			{"\\uD83D\\uDEAD", "https://discordapp.com/assets/c707a49a6979a1b2093c13849e0a9990.svg"},
			{"\\uD83D\\uDEBE", "https://discordapp.com/assets/9b7293199330ec7aec11e5594f73bf98.svg"},
			{"\\uD83C\\uDD7F", "https://discordapp.com/assets/e78fc07369f1bb9b2f4f13aa6d230b64.svg"},
			{"\\uD83D\\uDEB0", "https://discordapp.com/assets/02d982088dfe349faf4f907443af1485.svg"},
			{"\\uD83D\\uDEB9", "https://discordapp.com/assets/27a8d5bbb2fe88679e0e985b68902826.svg"},
			{"\\uD83D\\uDEBA", "https://discordapp.com/assets/5de7cbddbb9871569ed57d44356e5168.svg"},
			{"\\uD83D\\uDEBC", "https://discordapp.com/assets/b96f90c005db117b0a7d077753224efa.svg"},
			{"\\uD83D\\uDEBB", "https://discordapp.com/assets/5a1b5dcd927cc2f1905c61dd3d9ddedd.svg"},
			{"\\uD83D\\uDEAE", "https://discordapp.com/assets/59ba1e8d8ce894a7b7d857c87434303d.svg"},
			{"\\uD83C\\uDFA6", "https://discordapp.com/assets/e1df9f0dbfaa31ca2649515c8acd78ba.svg"},
			{"\\uD83D\\uDCF6", "https://discordapp.com/assets/7e3eb3c8296980ab00ba40fa033cbaab.svg"},
			{"\\uD83C\\uDE01", "https://discordapp.com/assets/dfc29b9acfcee8e172a892340d9a351b.svg"},
			{"\\uD83C\\uDD96", "https://discordapp.com/assets/8e1bc7bf4c317925b1e82e4be8b39baf.svg"},
			{"\\uD83C\\uDD97", "https://discordapp.com/assets/a61b14400491c0c070e80c99a05cda82.svg"},
			{"\\uD83C\\uDD99", "https://discordapp.com/assets/f18cb914a14fb9e89f92c8be55bfc5e3.svg"},
			{"\\uD83C\\uDD92", "https://discordapp.com/assets/e49a859a67fa70d84f1f61590b93fc44.svg"},
			{"\\uD83C\\uDD95", "https://discordapp.com/assets/02c27b408520f8a5cdcd82082c43f53e.svg"},
			{"\\uD83C\\uDD93", "https://discordapp.com/assets/a802da49292419853c585395032ab284.svg"},
			{"\\u0030\\u20E3", "https://discordapp.com/assets/e2b775b138c186a30a49776bf8ebc324.svg"},
			{"\\u0031\\u20E3", "https://discordapp.com/assets/68643336fe7c2884c92028d2cf482238.svg"},
			{"\\u0032\\u20E3", "https://discordapp.com/assets/55b118d4673c8dd0d21bcc6a30f379f4.svg"},
			{"\\u0033\\u20E3", "https://discordapp.com/assets/12a9f39a3bfc18e0e4557f60302712a1.svg"},
			{"\\u0034\\u20E3", "https://discordapp.com/assets/2b8cceb02771f78e8cbbd3f427cf8643.svg"},
			{"\\u0035\\u20E3", "https://discordapp.com/assets/354a7b2558484b0a74ddd389e5a37ff7.svg"},
			{"\\u0036\\u20E3", "https://discordapp.com/assets/b7411fe643d8ed6f3354da028411c953.svg"},
			{"\\u0037\\u20E3", "https://discordapp.com/assets/28e8cb51f1b03b70a86cad0516cda58a.svg"},
			{"\\u0038\\u20E3", "https://discordapp.com/assets/70c4e22f26e7bdf28ef2eef454f65ad7.svg"},
			{"\\u0039\\u20E3", "https://discordapp.com/assets/75093178d6de1384bf2372c67321f1f4.svg"},
			{"\\uD83D\\uDD1F", "https://discordapp.com/assets/0245df6c6638f07c6d21abf67504d002.svg"},
			{"\\u25B6", "https://discordapp.com/assets/ae3264b1fee0d603faeb60f5b6f74e19.svg"},
			{"\\u23F8", "https://discordapp.com/assets/d37c45970ed3d9bc0be0acd36c7c6c77.svg"},
			{"\\u23EF", "https://discordapp.com/assets/de8fa839a61b39d17febf16f22fd8159.svg"},
			{"\\u23F9", "https://discordapp.com/assets/76bb3e920ff642a218226a6c8a4cbc07.svg"},
			{"\\u23FA", "https://discordapp.com/assets/e7e9c004c48e49c4ff4e0d970da0761c.svg"},
			{"\\u23ED", "https://discordapp.com/assets/4e5dd162acac96c5af80b8f9e67c4bf1.svg"},
			{"\\u23EE", "https://discordapp.com/assets/7546070621ede73be9fd4293db3818bb.svg"},
			{"\\u23E9", "https://discordapp.com/assets/53596500301b5ee4d5e2deaed3acd17c.svg"},
			{"\\u23EA", "https://discordapp.com/assets/c3fca1ffab05a4501f900a6bb1a302fe.svg"},
			{"\\uD83D\\uDD00", "https://discordapp.com/assets/1565c72398b5982dd5770991cb5b9344.svg"},
			{"\\uD83D\\uDD01", "https://discordapp.com/assets/e92dd44553a788b0be795bcf18ace251.svg"},
			{"\\uD83D\\uDD02", "https://discordapp.com/assets/0546b9006d9fcb5b91a366b40d0cf0b2.svg"},
			{"\\u25C0", "https://discordapp.com/assets/7c96e6b8f0dfc1c67f8c96a1fab75240.svg"},
			{"\\uD83D\\uDD3C", "https://discordapp.com/assets/07df4b65baa175bdfac54ddff185e051.svg"},
			{"\\uD83D\\uDD3D", "https://discordapp.com/assets/0187ff05d39e6b9ad65b39501a8302cc.svg"},
			{"\\u23EB", "https://discordapp.com/assets/c4458c71feaaa849b80714bc0492a809.svg"},
			{"\\u23EC", "https://discordapp.com/assets/417512f0360124c8fcebbda16018b3a6.svg"},
			{"\\u27A1", "https://discordapp.com/assets/5e282c0cf9ce12ca380a0385c8e64667.svg"},
			{"\\u2B05", "https://discordapp.com/assets/eed2347ee50b0549d498ddf399a48228.svg"},
			{"\\u2B06", "https://discordapp.com/assets/06da0010320338c248b0686ec1c00f4a.svg"},
			{"\\u2B07", "https://discordapp.com/assets/55815c73346babb78e035300e7cc9b09.svg"},
			{"\\u2197", "https://discordapp.com/assets/67b2b79b33ab14d3ae15ab458096e5f6.svg"},
			{"\\u2198", "https://discordapp.com/assets/ce9c7826ffbd00f8fd628eeb7205fb06.svg"},
			{"\\u2199", "https://discordapp.com/assets/4ebbef21a47a346709b7f341d82a6cdd.svg"},
			{"\\u2196", "https://discordapp.com/assets/0a661cc72d3d7a2b966e1ae241b49f7d.svg"},
			{"\\u2195", "https://discordapp.com/assets/04186bb59dbdc262948a8e40982d21db.svg"},
			{"\\u2194", "https://discordapp.com/assets/15e43e3d7ee0a8a11e2f2f58d34ecfd9.svg"},
			{"\\uD83D\\uDD04", "https://discordapp.com/assets/510b2959b8bd773f8d269ba54cf790ff.svg"},
			{"\\u21AA", "https://discordapp.com/assets/6e916eef58bac62dbf01ad6e9938ffae.svg"},
			{"\\u21A9", "https://discordapp.com/assets/0a1b64d0eb2783edb6ef2bcca2b158e8.svg"},
			{"\\u2934", "https://discordapp.com/assets/be2f6e79a170d14bd8181c1642b6146b.svg"},
			{"\\u2935", "https://discordapp.com/assets/e2440c59c9200ec19b87c5c5db78c113.svg"},
			{"\\u0023\\u20E3", "https://discordapp.com/assets/b0970076f1aaac08d24cc534a8afbf18.svg"},
			{"\\u002A\\u20E3", "https://discordapp.com/assets/fbbfde37a1d4065d9073098a7814e83a.svg"},
			{"\\u2139", "https://discordapp.com/assets/4757a3d415435a57d4db6fca022872e3.svg"},
			{"\\uD83D\\uDD24", "https://discordapp.com/assets/713d0a6d305ef8146058d416ea43d0ad.svg"},
			{"\\uD83D\\uDD21", "https://discordapp.com/assets/44c90a77a31ecd300654161820ce8d81.svg"},
			{"\\uD83D\\uDD20", "https://discordapp.com/assets/d73d26b4fdabb38cc24128200409c9ed.svg"},
			{"\\uD83D\\uDD23", "https://discordapp.com/assets/8ddcaf0f730cbec748e6f3731e9c9532.svg"},
			{"\\uD83C\\uDFB5", "https://discordapp.com/assets/bc55faff7081e38b9300f2cb4afa13f3.svg"},
			{"\\uD83C\\uDFB6", "https://discordapp.com/assets/9385f5b0c3bd2064e47d8ce5964aad5a.svg"},
			{"\\u3030", "https://discordapp.com/assets/deb1db01d54ee99ef50ceccc9aa79bfd.svg"},
			{"\\u27B0", "https://discordapp.com/assets/be73eca7ff2e6dd30b69a43974a19164.svg"},
			{"\\u2714", "https://discordapp.com/assets/13262ca66be5da2c98c6daa8bed7353a.svg"},
			{"\\uD83D\\uDD03", "https://discordapp.com/assets/3dc39675077548f1f868ba0cc641d061.svg"},
			{"\\u2795", "https://discordapp.com/assets/8502696d0b3ebe3eceb24c13888ea623.svg"},
			{"\\u2796", "https://discordapp.com/assets/c4c6f3a5f024d772b5164605db9d431b.svg"},
			{"\\u2797", "https://discordapp.com/assets/3d0695990d2375dbae86b87bf5205502.svg"},
			{"\\u2716", "https://discordapp.com/assets/ad7398486e336cf17b49bfe4fad446b6.svg"},
			{"\\uD83D\\uDCB2", "https://discordapp.com/assets/0c37ee07aabe19072b03afdf299ab7a6.svg"},
			{"\\uD83D\\uDCB1", "https://discordapp.com/assets/a65d35e1714c54139d79c90a2126acdd.svg"},
			{"\\u00A9", ""},
			{"\\u00AE", ""},
			{"\\u2122", ""},
			{"\\uD83D\\uDD1A", "https://discordapp.com/assets/4162176664cb7ec52ed19754b6818672.svg"},
			{"\\uD83D\\uDD19", "https://discordapp.com/assets/1d1fc4f2a7b7b936011a281da94b3652.svg"},
			{"\\uD83D\\uDD1B", "https://discordapp.com/assets/68eaf52110e1feeaf4eeabbb903b652d.svg"},
			{"\\uD83D\\uDD1D", "https://discordapp.com/assets/43f3d80150abccba31df3e32ea4b3cec.svg"},
			{"\\uD83D\\uDD1C", "https://discordapp.com/assets/4ca85cc4e104c1522ab1fb7b0eb90c3c.svg"},
			{"\\u2611", "https://discordapp.com/assets/c322098eb7d4cabe1c8a4fb68e5dfce4.svg"},
			{"\\uD83D\\uDD18", "https://discordapp.com/assets/a134dff4cbdf34e92247938283b39867.svg"},
			{"\\u26AA", "https://discordapp.com/assets/24c558fa9fed1d862de8f982fe55c97c.svg"},
			{"\\u26AB", "https://discordapp.com/assets/2ffa76c4b99ee4ed1cce07e2ea6eab7b.svg"},
			{"\\uD83D\\uDD34", "https://discordapp.com/assets/26a727a30b2342317fb0df394e399df1.svg"},
			{"\\uD83D\\uDD35", "https://discordapp.com/assets/5b68b417ff72b3a2f4c24426d1064b66.svg"},
			{"\\uD83D\\uDD38", "https://discordapp.com/assets/b231248aa321ecac50d2d5d2a0476f8a.svg"},
			{"\\uD83D\\uDD39", "https://discordapp.com/assets/8dd3fb05cf83cad5d68191580b638c50.svg"},
			{"\\uD83D\\uDD36", "https://discordapp.com/assets/dbd5f8f00f7dd51d9b608809f964857d.svg"},
			{"\\uD83D\\uDD37", "https://discordapp.com/assets/4bf415c80c002eaad2d23809951bbb07.svg"},
			{"\\uD83D\\uDD3A", "https://discordapp.com/assets/07715305b8a9a8550df60f5cbdcaa821.svg"},
			{"\\u25AA", "https://discordapp.com/assets/8dcca2877f51849a159035186bf93dff.svg"},
			{"\\u25AB", "https://discordapp.com/assets/e27821b687cbdc78563977c8b822aa0f.svg"},
			{"\\u2B1B", "https://discordapp.com/assets/90afb3e43c37af013870d5b1f530fb8b.svg"},
			{"\\u2B1C", "https://discordapp.com/assets/ea3b7f0aee3f51c3bbfe5a6d7f93e436.svg"},
			{"\\uD83D\\uDD3B", "https://discordapp.com/assets/afb82ce2cbadea31acc6f3374b7d338a.svg"},
			{"\\u25FC", "https://discordapp.com/assets/cf093ef298428cb03a9627721dd8ecbc.svg"},
			{"\\u25FB", "https://discordapp.com/assets/f31cb88726a692d38a73e65417b48b2a.svg"},
			{"\\u25FE", "https://discordapp.com/assets/634cc490e36cdb8b29c200f23f7b2f05.svg"},
			{"\\u25FD", "https://discordapp.com/assets/f0dc06370a08ebe2aa1165eea6cabbfb.svg"},
			{"\\uD83D\\uDD32", "https://discordapp.com/assets/5d15d81bc39f310bfa589fc4276c6d49.svg"},
			{"\\uD83D\\uDD33", "https://discordapp.com/assets/45d0e7d324f2aec6d85df219cb5b62ed.svg"},
			{"\\uD83D\\uDD08", "https://discordapp.com/assets/f0fa3c1e815fa570f97b21eec4c666dd.svg"},
			{"\\uD83D\\uDD09", "https://discordapp.com/assets/5bed81992d7fc4dca4539e6386d0769e.svg"},
			{"\\uD83D\\uDD0A", "https://discordapp.com/assets/fd2173327b6bf2cd86fdcc1dd6d4dee8.svg"},
			{"\\uD83D\\uDD07", "https://discordapp.com/assets/ae6953e18a8e761c0ca17c5b7ba9fa9c.svg"},
			{"\\uD83D\\uDCE3", "https://discordapp.com/assets/eb4ba561a219d88f1f2eb06114d6a9a5.svg"},
			{"\\uD83D\\uDCE2", "https://discordapp.com/assets/dac20f2ccbd28f469f3154cfe6ea1709.svg"},
			{"\\uD83D\\uDD14", "https://discordapp.com/assets/c2cde9981ca78feeaeaaa4ae23c4d955.svg"},
			{"\\uD83D\\uDD15", "https://discordapp.com/assets/66ff128fa27f05b0199e6e036b786704.svg"},
			{"\\uD83C\\uDCCF", "https://discordapp.com/assets/094868bfb782f145d4fe5cd8dbc49cc7.svg"},
			{"\\uD83C\\uDC04", "https://discordapp.com/assets/4a7e8a1c38d2b205bf0ed66287dc3878.svg"},
			{"\\u2660", "https://discordapp.com/assets/7b6619d2887c4129dee97040e2db0db6.svg"},
			{"\\u2663", "https://discordapp.com/assets/0fa5cea503e2cecc5dfe0d5eff8d2126.svg"},
			{"\\u2665", "https://discordapp.com/assets/bfdf252c56d399cc55179d7060b4eb2d.svg"},
			{"\\u2666", "https://discordapp.com/assets/a36fbb4c7ce0e0f2d1b9e036c4ddfa2b.svg"},
			{"\\uD83C\\uDFB4", "https://discordapp.com/assets/6bdac9f84dc45eb29c890530ca253741.svg"},
			{"\\uD83D\\uDCAD", "https://discordapp.com/assets/2a73aad553d69de29634941e4bb68fb8.svg"},
			{"\\uD83D\\uDDEF", "https://discordapp.com/assets/152ce02aa605fc4cda416f7c538eb2b0.svg"},
			{"\\uD83D\\uDCAC", "https://discordapp.com/assets/0b6fc9f58ca3827977d546a6ee0ca3e7.svg"},
			{"\\uD83D\\uDD50", "https://discordapp.com/assets/be21e06571be73619ebba2e9731abb11.svg"},
			{"\\uD83D\\uDD51", "https://discordapp.com/assets/4d3950c8201d9bac57ebbc3a1df92bac.svg"},
			{"\\uD83D\\uDD52", "https://discordapp.com/assets/841d44baf59b5bb6dde668a3d44e8e65.svg"},
			{"\\uD83D\\uDD53", "https://discordapp.com/assets/ccb48a1e7bb48aba28d9cd37123b3293.svg"},
			{"\\uD83D\\uDD54", "https://discordapp.com/assets/3a020d02dbef57ba2a126fe3c9e044fa.svg"},
			{"\\uD83D\\uDD55", "https://discordapp.com/assets/343f74429898d15e9dccf6146b47cd83.svg"},
			{"\\uD83D\\uDD56", "https://discordapp.com/assets/cc07144f5d31ec82c17ca00589b94a00.svg"},
			{"\\uD83D\\uDD57", "https://discordapp.com/assets/9f5bdceca467fde92f5fdc925bb5a1bd.svg"},
			{"\\uD83D\\uDD58", "https://discordapp.com/assets/d55c3296692cfe4ae472b97714270547.svg"},
			{"\\uD83D\\uDD59", "https://discordapp.com/assets/dba396ebe99cfc15da738d76650a3a3d.svg"},
			{"\\uD83D\\uDD5A", "https://discordapp.com/assets/115bc3964b9e499145a8c3d7767e13ef.svg"},
			{"\\uD83D\\uDD5B", "https://discordapp.com/assets/bc18e1d4939b4695b40f8c0e3cd4d13a.svg"},
			{"\\uD83D\\uDD5C", "https://discordapp.com/assets/60d12a68039b63ef2d74c4bf7b4b8b4f.svg"},
			{"\\uD83D\\uDD5D", "https://discordapp.com/assets/3e3464ecdbc8bb16396efc63aa6cc8bb.svg"},
			{"\\uD83D\\uDD5E", "https://discordapp.com/assets/b65b718774874a918590d3561a979aaf.svg"},
			{"\\uD83D\\uDD5F", "https://discordapp.com/assets/82055740ed4af85bd13a883caab68d74.svg"},
			{"\\uD83D\\uDD60", "https://discordapp.com/assets/d8e1f009b605f6bb62099e624de5ebb7.svg"},
			{"\\uD83D\\uDD61", "https://discordapp.com/assets/16655a981a6079e9d2401d9ed7899447.svg"},
			{"\\uD83D\\uDD62", "https://discordapp.com/assets/e69989b18d4261667966472d68071fe4.svg"},
			{"\\uD83D\\uDD63", "https://discordapp.com/assets/179ed90c54cbd0d141a64483a7b60fda.svg"},
			{"\\uD83D\\uDD64", "https://discordapp.com/assets/43a03f5fc9db11933516173f79432fc3.svg"},
			{"\\uD83D\\uDD65", "https://discordapp.com/assets/249f56483b4268e09660560cc7a9cd41.svg"},
			{"\\uD83D\\uDD66", "https://discordapp.com/assets/5dd6aaef3152fc6c1e422a9a741f9e87.svg"},
			{"\\uD83D\\uDD67", "https://discordapp.com/assets/6738d5cf36a4d7bc6e789d22cef831ef.svg"},
			{"\\uD83D\\uDC41\\u200D\\uD83D\\uDDE8", "https://discordapp.com/assets/4a03a6605e65f9adb0684458cccd0963.svg"},
			{"\\uD83D\\uDDE8", "https://discordapp.com/assets/cf7f3e86b93c14f8e53feac55a1b637f.svg"},
			{"\\u23CF", "https://discordapp.com/assets/e6ece1659e7c0e6f4ebd17bae7a8fc2f.svg"},
			{"\\uD83D\\uDDA4", "https://discordapp.com/assets/9e62a2acc00cd50132c7e09acf4671ff.svg"},
			{"\\uD83D\\uDED1", "https://discordapp.com/assets/7d8e9e2805bdb7b32248f72fc9c94080.svg"},
			{"\\uD83C\\uDDFF", "https://discordapp.com/assets/59b9aed7bbd7badd889af6bc4a586139.svg"},
			{"\\uD83C\\uDDFE", "https://discordapp.com/assets/6d975157bf496526db48222321db7dfb.svg"},
			{"\\uD83C\\uDDFD", "https://discordapp.com/assets/ce48e2893d90c111c6a223b7293bb2a9.svg"},
			{"\\uD83C\\uDDFC", "https://discordapp.com/assets/58866d5e3b7bee4b45c92ae09451f86b.svg"},
			{"\\uD83C\\uDDFB", "https://discordapp.com/assets/84cc28d4d232ad0c4fa42c01b4d7a1d4.svg"},
			{"\\uD83C\\uDDFA", "https://discordapp.com/assets/d054dc34be8c323352033df0ef614d2b.svg"},
			{"\\uD83C\\uDDF9", "https://discordapp.com/assets/1a8f1c43f58e917b5269bfad503244a6.svg"},
			{"\\uD83C\\uDDF8", "https://discordapp.com/assets/9e7e4d888e7491e22e49e1ef48b7c29e.svg"},
			{"\\uD83C\\uDDF7", "https://discordapp.com/assets/1833b371cf58d178074340b3d494f308.svg"},
			{"\\uD83C\\uDDF6", "https://discordapp.com/assets/370495016e00f9459438a21e790f2fb6.svg"},
			{"\\uD83C\\uDDF5", "https://discordapp.com/assets/200197699874981bef63cac3bbf1cf0e.svg"},
			{"\\uD83C\\uDDF4", "https://discordapp.com/assets/48c744b55b0f90fd423950df2b0e43e4.svg"},
			{"\\uD83C\\uDDF3", "https://discordapp.com/assets/56f2c6f0a1e2323ff28a7876fe527b4c.svg"},
			{"\\uD83C\\uDDF2", "https://discordapp.com/assets/05e934fc7cde7b2f9039522992cb0194.svg"},
			{"\\uD83C\\uDDF1", "https://discordapp.com/assets/5dec0be7440407fd74683561854d4371.svg"},
			{"\\uD83C\\uDDF0", "https://discordapp.com/assets/01367cc80714099ae6202fec97a3e6c1.svg"},
			{"\\uD83C\\uDDEF", "https://discordapp.com/assets/98eab6564de066b2c863248b4ad2a1ef.svg"},
			{"\\uD83C\\uDDEE", "https://discordapp.com/assets/9a9325d2bfd83add09376a7f2d2e928a.svg"},
			{"\\uD83C\\uDDED", "https://discordapp.com/assets/1d40afe15ffa7e64157b3554a7f0e3aa.svg"},
			{"\\uD83C\\uDDEC", "https://discordapp.com/assets/af664302dea9eab71858d360d3698061.svg"},
			{"\\uD83C\\uDDEB", "https://discordapp.com/assets/e99e3416d4825a09c106d7dfe51939cf.svg"},
			{"\\uD83C\\uDDEA", "https://discordapp.com/assets/ede3268aa377f9b55c96707b4332a518.svg"},
			{"\\uD83C\\uDDE9", "https://discordapp.com/assets/50118b4eac911e2134c145f4481e1eaf.svg"},
			{"\\uD83C\\uDDE8", "https://discordapp.com/assets/f5a3b10342ad117da7530ae96caa1f5e.svg"},
			{"\\uD83C\\uDDE7", "https://discordapp.com/assets/1f19240e4cb75bc359bf221d151a193e.svg"},
			{"\\uD83C\\uDDE6", "https://discordapp.com/assets/9d3334bc4be7f586fc00eb2772eb331f.svg"},
			{"\\uD83C\\uDDE6\\uD83C\\uDDE8", "https://discordapp.com/assets/15eb2b4336a23cbbfb2c7f65983674ca.svg"},
			{"\\uD83C\\uDDE6\\uD83C\\uDDEB", "https://discordapp.com/assets/3c4b934ef4f5946945565c90236fd0fd.svg"},
			{"\\uD83C\\uDDE6\\uD83C\\uDDF1", "https://discordapp.com/assets/2f0edb2adbf57b52c544b19d6a91d65d.svg"},
			{"\\uD83C\\uDDE9\\uD83C\\uDDFF", "https://discordapp.com/assets/f2099b7cb97462412803953c0b01332f.svg"},
			{"\\uD83C\\uDDE6\\uD83C\\uDDE9", "https://discordapp.com/assets/3f23733001b581f991045d7291133509.svg"},
			{"\\uD83C\\uDDE6\\uD83C\\uDDF4", "https://discordapp.com/assets/29fd528b0715dff4581d750d450253f2.svg"},
			{"\\uD83C\\uDDE6\\uD83C\\uDDEE", "https://discordapp.com/assets/04fd4e2ef09ca386b46d09ea17ca6db9.svg"},
			{"\\uD83C\\uDDE6\\uD83C\\uDDEC", "https://discordapp.com/assets/4ccc37c261c2e2d826b64fa9ce3dadf8.svg"},
			{"\\uD83C\\uDDE6\\uD83C\\uDDF7", "https://discordapp.com/assets/07a1c54d94b121d2766944c4ef3b5e69.svg"},
			{"\\uD83C\\uDDE6\\uD83C\\uDDF2", "https://discordapp.com/assets/afbffb7c24c3fbd62eed6347ec82e31c.svg"},
			{"\\uD83C\\uDDE6\\uD83C\\uDDFC", "https://discordapp.com/assets/b48f39cb189ba3712796dc24dc626e4d.svg"},
			{"\\uD83C\\uDDE6\\uD83C\\uDDFA", "https://discordapp.com/assets/0b8323ad8d6947ab2b5b874d8c12866d.svg"},
			{"\\uD83C\\uDDE6\\uD83C\\uDDF9", "https://discordapp.com/assets/6627a630cfc1a9673158ed0d459ed0b7.svg"},
			{"\\uD83C\\uDDE6\\uD83C\\uDDFF", "https://discordapp.com/assets/8db08f31f1918f1bc6295ebe255f5520.svg"},
			{"\\uD83C\\uDDE7\\uD83C\\uDDF8", "https://discordapp.com/assets/1b30231e4bde33e074de5871d51497f7.svg"},
			{"\\uD83C\\uDDE7\\uD83C\\uDDED", "https://discordapp.com/assets/6797c37fc1cd62176211eb4aad06e8b6.svg"},
			{"\\uD83C\\uDDE7\\uD83C\\uDDE9", "https://discordapp.com/assets/0c1b838cd2f467f5292126544358506e.svg"},
			{"\\uD83C\\uDDE7\\uD83C\\uDDE7", "https://discordapp.com/assets/2ea248c3dce94632b8b1d31d81aa768d.svg"},
			{"\\uD83C\\uDDE7\\uD83C\\uDDFE", "https://discordapp.com/assets/7ec13c049fe433a08fdc9545baec200e.svg"},
			{"\\uD83C\\uDDE7\\uD83C\\uDDEA", "https://discordapp.com/assets/703ed1cd4728c61677efc3239745dfa0.svg"},
			{"\\uD83C\\uDDE7\\uD83C\\uDDFF", "https://discordapp.com/assets/3cad4f8ee9dcbc15976920a33dc44ddc.svg"},
			{"\\uD83C\\uDDE7\\uD83C\\uDDEF", "https://discordapp.com/assets/184022bedf637f627695ac4eb6764725.svg"},
			{"\\uD83C\\uDDE7\\uD83C\\uDDF2", "https://discordapp.com/assets/19696347beb6c16e5a9d751085d948c5.svg"},
			{"\\uD83C\\uDDE7\\uD83C\\uDDF9", "https://discordapp.com/assets/8b328c672b6f834102fe49fbfa6591d9.svg"},
			{"\\uD83C\\uDDE7\\uD83C\\uDDF4", "https://discordapp.com/assets/9216e2d5d9f0cf3ddb610e0dd9f098a7.svg"},
			{"\\uD83C\\uDDE7\\uD83C\\uDDE6", "https://discordapp.com/assets/5f8852008dba7fe14a901637f559183a.svg"},
			{"\\uD83C\\uDDE7\\uD83C\\uDDFC", "https://discordapp.com/assets/d2c729f672dd737a22e7f3e403d41e71.svg"},
			{"\\uD83C\\uDDE7\\uD83C\\uDDF7", "https://discordapp.com/assets/5e0529582f11d6d30e74ed3a28ac486e.svg"},
			{"\\uD83C\\uDDE7\\uD83C\\uDDF3", "https://discordapp.com/assets/1e677b5cb2c717643a41f4b7155f09c9.svg"},
			{"\\uD83C\\uDDE7\\uD83C\\uDDEC", "https://discordapp.com/assets/1943b537414d2f03962fef04ef156678.svg"},
			{"\\uD83C\\uDDE7\\uD83C\\uDDEB", "https://discordapp.com/assets/ade4b25a1fd9a5965358a7e11a39af96.svg"},
			{"\\uD83C\\uDDE7\\uD83C\\uDDEE", "https://discordapp.com/assets/e054e26599b7b9fa70e4c713f3611c5c.svg"},
			{"\\uD83C\\uDDE8\\uD83C\\uDDFB", "https://discordapp.com/assets/aa519db9517a6af12e3d81a3b37853ec.svg"},
			{"\\uD83C\\uDDF0\\uD83C\\uDDED", "https://discordapp.com/assets/dd42ccb05e4a3128ea58365f25986489.svg"},
			{"\\uD83C\\uDDE8\\uD83C\\uDDF2", "https://discordapp.com/assets/0909810f3b137bee558f9af11bc9f07a.svg"},
			{"\\uD83C\\uDDE8\\uD83C\\uDDE6", "https://discordapp.com/assets/082992efec251eeb481db94bca05af5a.svg"},
			{"\\uD83C\\uDDF0\\uD83C\\uDDFE", "https://discordapp.com/assets/368d5e397d1de213216faaba8ccf8fa5.svg"},
			{"\\uD83C\\uDDE8\\uD83C\\uDDEB", "https://discordapp.com/assets/9d6bb2763a7da84ecc739d8d54a93e05.svg"},
			{"\\uD83C\\uDDF9\\uD83C\\uDDE9", "https://discordapp.com/assets/1e3803f3e8dd845c0cf9c8d3fa6f3375.svg"},
			{"\\uD83C\\uDDE8\\uD83C\\uDDF1", "https://discordapp.com/assets/0e1b3489b752297c26233895d554541e.svg"},
			{"\\uD83C\\uDDE8\\uD83C\\uDDF3", "https://discordapp.com/assets/cf284aea8c5a88741efd4e128180366b.svg"},
			{"\\uD83C\\uDDE8\\uD83C\\uDDF4", "https://discordapp.com/assets/983b6e88a2c114526b7325bdfb7b8d43.svg"},
			{"\\uD83C\\uDDF0\\uD83C\\uDDF2", "https://discordapp.com/assets/8dbf53dd594cfb7290b96f79cf237e53.svg"},
			{"\\uD83C\\uDDE8\\uD83C\\uDDEC", "https://discordapp.com/assets/6ff46fd914c993351e70852ec9f635b7.svg"},
			{"\\uD83C\\uDDE8\\uD83C\\uDDE9", "https://discordapp.com/assets/d01f3e5eb2a09a4b067ed09788c07407.svg"},
			{"\\uD83C\\uDDE8\\uD83C\\uDDF7", "https://discordapp.com/assets/2fd4f1f724e48824f71bd5671adc410c.svg"},
			{"\\uD83C\\uDDED\\uD83C\\uDDF7", "https://discordapp.com/assets/7b6e8d1d68b9c8fd00689680300a2ed7.svg"},
			{"\\uD83C\\uDDE8\\uD83C\\uDDFA", "https://discordapp.com/assets/8d883ec84bc443ae733ac0eb1f72ed91.svg"},
			{"\\uD83C\\uDDE8\\uD83C\\uDDFE", "https://discordapp.com/assets/6d0147261ee596db0af9c04ccd93c8fc.svg"},
			{"\\uD83C\\uDDE8\\uD83C\\uDDFF", "https://discordapp.com/assets/42c8e75954826dd9cb589a6e17322f7a.svg"},
			{"\\uD83C\\uDDE9\\uD83C\\uDDF0", "https://discordapp.com/assets/a002ff23dc7c7cb5be192a5e016e8574.svg"},
			{"\\uD83C\\uDDE9\\uD83C\\uDDEF", "https://discordapp.com/assets/0dac5446771f2eca48da3bcaa813b85a.svg"},
			{"\\uD83C\\uDDE9\\uD83C\\uDDF2", "https://discordapp.com/assets/be062c0379e4339e5bd787b328a273a4.svg"},
			{"\\uD83C\\uDDE9\\uD83C\\uDDF4", "https://discordapp.com/assets/313de0dadcef0e8da072cdfa249a3da1.svg"},
			{"\\uD83C\\uDDEA\\uD83C\\uDDE8", "https://discordapp.com/assets/1e84dcfc334c0c9840816673d699acde.svg"},
			{"\\uD83C\\uDDEA\\uD83C\\uDDEC", "https://discordapp.com/assets/076c484e08095be7dbfef889a81ba45e.svg"},
			{"\\uD83C\\uDDF8\\uD83C\\uDDFB", "https://discordapp.com/assets/0f75f1d6120f5d92faf8d14e88067344.svg"},
			{"\\uD83C\\uDDEC\\uD83C\\uDDF6", "https://discordapp.com/assets/87fed7a84a8a487a554ab645afee1ef0.svg"},
			{"\\uD83C\\uDDEA\\uD83C\\uDDF7", "https://discordapp.com/assets/cb76ccc83f396058efd1c53f213ea9d0.svg"},
			{"\\uD83C\\uDDEA\\uD83C\\uDDEA", "https://discordapp.com/assets/852828f0b5d3346065a03c47e040bb74.svg"},
			{"\\uD83C\\uDDEA\\uD83C\\uDDF9", "https://discordapp.com/assets/f1a0e3e8dcb19efa8d501b2b1880e72f.svg"},
			{"\\uD83C\\uDDEB\\uD83C\\uDDF0", "https://discordapp.com/assets/0051f09eb008cf1dd3eae339aa7e062c.svg"},
			{"\\uD83C\\uDDEB\\uD83C\\uDDF4", "https://discordapp.com/assets/26bce1410ca4a7e9992c342cfb0e7164.svg"},
			{"\\uD83C\\uDDEB\\uD83C\\uDDEF", "https://discordapp.com/assets/d34cbf26429b4216976994d571785e4d.svg"},
			{"\\uD83C\\uDDEB\\uD83C\\uDDEE", "https://discordapp.com/assets/3e544ea1ece2faef8549a45024d7737e.svg"},
			{"\\uD83C\\uDDEB\\uD83C\\uDDF7", "https://discordapp.com/assets/427c845111c8328fb785dbfe7052337e.svg"},
			{"\\uD83C\\uDDF5\\uD83C\\uDDEB", "https://discordapp.com/assets/d3926aeb79620af749bace296a835181.svg"},
			{"\\uD83C\\uDDEC\\uD83C\\uDDE6", "https://discordapp.com/assets/6a8b400df911e6346e8b3d7274df2ed3.svg"},
			{"\\uD83C\\uDDEC\\uD83C\\uDDF2", "https://discordapp.com/assets/7c43bf8fff165b3b4af2780babec28b3.svg"},
			{"\\uD83C\\uDDEC\\uD83C\\uDDEA", "https://discordapp.com/assets/423ee255525f029e9b6d840ba3925a16.svg"},
			{"\\uD83C\\uDDE9\\uD83C\\uDDEA", "https://discordapp.com/assets/a9c94412aab983dee63f57bc2c096fae.svg"},
			{"\\uD83C\\uDDEC\\uD83C\\uDDED", "https://discordapp.com/assets/2dc43fb96038e11f83127735165c6362.svg"},
			{"\\uD83C\\uDDEC\\uD83C\\uDDEE", "https://discordapp.com/assets/c714a85da23812cca444b61f64307bd5.svg"},
			{"\\uD83C\\uDDEC\\uD83C\\uDDF7", "https://discordapp.com/assets/97972f629a93e2a763b8c66053fe9f19.svg"},
			{"\\uD83C\\uDDEC\\uD83C\\uDDF1", "https://discordapp.com/assets/00c635d5b0b2f192e511df1250f86c56.svg"},
			{"\\uD83C\\uDDEC\\uD83C\\uDDE9", "https://discordapp.com/assets/6ba3d4fe60567dfc22c4d86788ae8ffd.svg"},
			{"\\uD83C\\uDDEC\\uD83C\\uDDFA", "https://discordapp.com/assets/bb93c0e2fee7242d4c7eb88a6212e1af.svg"},
			{"\\uD83C\\uDDEC\\uD83C\\uDDF9", "https://discordapp.com/assets/da1a14794dd6bdf5a3f031f6d99cb9e8.svg"},
			{"\\uD83C\\uDDEC\\uD83C\\uDDF3", "https://discordapp.com/assets/69e3edf67cd93d578edf6e652808fd4d.svg"},
			{"\\uD83C\\uDDEC\\uD83C\\uDDFC", "https://discordapp.com/assets/aa0c0b8e55c3e71302219218674aebe2.svg"},
			{"\\uD83C\\uDDEC\\uD83C\\uDDFE", "https://discordapp.com/assets/6d5c3c7789c95386efa74a4208f8d0a7.svg"},
			{"\\uD83C\\uDDED\\uD83C\\uDDF9", "https://discordapp.com/assets/1ea6864800c30887d4c810aabcdfefb5.svg"},
			{"\\uD83C\\uDDED\\uD83C\\uDDF3", "https://discordapp.com/assets/4272fd4245d4bcc7ccf2acea6898283e.svg"},
			{"\\uD83C\\uDDED\\uD83C\\uDDF0", "https://discordapp.com/assets/e8d65f351c02a7b42562e55b15a1e6d9.svg"},
			{"\\uD83C\\uDDED\\uD83C\\uDDFA", "https://discordapp.com/assets/f2a1ad637508714c905a69cd341894b4.svg"},
			{"\\uD83C\\uDDEE\\uD83C\\uDDF8", "https://discordapp.com/assets/381c5f7e98dee6eacb990fa47c4e805f.svg"},
			{"\\uD83C\\uDDEE\\uD83C\\uDDF3", "https://discordapp.com/assets/6eed577e184d922e97374184af656554.svg"},
			{"\\uD83C\\uDDEE\\uD83C\\uDDE9", "https://discordapp.com/assets/af05083c3601e8866a67d7bbe93d771b.svg"},
			{"\\uD83C\\uDDEE\\uD83C\\uDDF7", "https://discordapp.com/assets/d7ec8b8de889d90fe95deb2344bc69fa.svg"},
			{"\\uD83C\\uDDEE\\uD83C\\uDDF6", "https://discordapp.com/assets/24c4e5553b26421b478c3230feb5bec5.svg"},
			{"\\uD83C\\uDDEE\\uD83C\\uDDEA", "https://discordapp.com/assets/57a60c047629ee6bd10d9f50952ecd03.svg"},
			{"\\uD83C\\uDDEE\\uD83C\\uDDF1", "https://discordapp.com/assets/6231ac51dbd0c0e5ebf4a558bc5daddd.svg"},
			{"\\uD83C\\uDDEE\\uD83C\\uDDF9", "https://discordapp.com/assets/f740368e08d0e77f34546769b88d7ca6.svg"},
			{"\\uD83C\\uDDE8\\uD83C\\uDDEE", "https://discordapp.com/assets/17c76e8ad081575408af5a96b16bb0e9.svg"},
			{"\\uD83C\\uDDEF\\uD83C\\uDDF2", "https://discordapp.com/assets/bb175ca1caf96d4d5a1901983b94149e.svg"},
			{"\\uD83C\\uDDEF\\uD83C\\uDDF5", "https://discordapp.com/assets/bf5b3a6c0c677f13249b3b1c4142671d.svg"},
			{"\\uD83C\\uDDEF\\uD83C\\uDDEA", "https://discordapp.com/assets/8eb9e502ae4e7ecddab14030bab0b981.svg"},
			{"\\uD83C\\uDDEF\\uD83C\\uDDF4", "https://discordapp.com/assets/4f35695731fa0160f3d318160ff3cfba.svg"},
			{"\\uD83C\\uDDF0\\uD83C\\uDDFF", "https://discordapp.com/assets/b52a60f33ef69fba800fa50bd2ea4135.svg"},
			{"\\uD83C\\uDDF0\\uD83C\\uDDEA", "https://discordapp.com/assets/6c9cc28a0b24c4316e9283d154414fbc.svg"},
			{"\\uD83C\\uDDF0\\uD83C\\uDDEE", "https://discordapp.com/assets/82403f8701f9e2f597886e18f92db5dc.svg"},
			{"\\uD83C\\uDDFD\\uD83C\\uDDF0", "https://discordapp.com/assets/4f46750d80775185df691d51db7e9b13.svg"},
			{"\\uD83C\\uDDF0\\uD83C\\uDDFC", "https://discordapp.com/assets/4235e81e6ad8369f983970cd0730d7ee.svg"},
			{"\\uD83C\\uDDF0\\uD83C\\uDDEC", "https://discordapp.com/assets/6827c56af5b9e6e20dfb42b637a1a215.svg"},
			{"\\uD83C\\uDDF1\\uD83C\\uDDE6", "https://discordapp.com/assets/8f882c672ad1c882f584ba39fedf81d2.svg"},
			{"\\uD83C\\uDDF1\\uD83C\\uDDFB", "https://discordapp.com/assets/b773a54cbc605399ad76a97d05cda797.svg"},
			{"\\uD83C\\uDDF1\\uD83C\\uDDE7", "https://discordapp.com/assets/dbf699e6819fc3660a95c5d134dfc1af.svg"},
			{"\\uD83C\\uDDF1\\uD83C\\uDDF8", "https://discordapp.com/assets/eb27c925b55d71644578c9642f030105.svg"},
			{"\\uD83C\\uDDF1\\uD83C\\uDDF7", "https://discordapp.com/assets/c1faeebc309c132b689da7cec2176125.svg"},
			{"\\uD83C\\uDDF1\\uD83C\\uDDFE", "https://discordapp.com/assets/c5e4b6df957d7f68bfafac0fb0c7e144.svg"},
			{"\\uD83C\\uDDF1\\uD83C\\uDDEE", "https://discordapp.com/assets/37c6ea868dd65b99a2be9df3ec8592e9.svg"},
			{"\\uD83C\\uDDF1\\uD83C\\uDDF9", "https://discordapp.com/assets/e497cd1e8efb86ec32b707a634ac2f91.svg"},
			{"\\uD83C\\uDDF1\\uD83C\\uDDFA", "https://discordapp.com/assets/c3ddc5fa27db7837c4215f6dc6b6420a.svg"},
			{"\\uD83C\\uDDF2\\uD83C\\uDDF4", "https://discordapp.com/assets/fe2b68690ba096a8b37f5bf7d9ae5a53.svg"},
			{"\\uD83C\\uDDF2\\uD83C\\uDDF0", "https://discordapp.com/assets/17a822f5f2a6bb5493b2743c87367669.svg"},
			{"\\uD83C\\uDDF2\\uD83C\\uDDEC", "https://discordapp.com/assets/9b1284ec5c97e591b0ea1e1fd47de960.svg"},
			{"\\uD83C\\uDDF2\\uD83C\\uDDFC", "https://discordapp.com/assets/4099aad6de78c852dcd8f0a2823c9f6a.svg"},
			{"\\uD83C\\uDDF2\\uD83C\\uDDFE", "https://discordapp.com/assets/60690ff78b97adb7841a88885e577a3e.svg"},
			{"\\uD83C\\uDDF2\\uD83C\\uDDFB", "https://discordapp.com/assets/84127a388ce77985d21857900ca5536e.svg"},
			{"\\uD83C\\uDDF2\\uD83C\\uDDF1", "https://discordapp.com/assets/55d66a68007c7399a9118850bc0ce7c0.svg"},
			{"\\uD83C\\uDDF2\\uD83C\\uDDF9", "https://discordapp.com/assets/c9bf1f1439b8e08c40bba40b7352d0c2.svg"},
			{"\\uD83C\\uDDF2\\uD83C\\uDDED", "https://discordapp.com/assets/dd34e332d1245be7e26ad5b4c3861a02.svg"},
			{"\\uD83C\\uDDF2\\uD83C\\uDDF7", "https://discordapp.com/assets/9e51d6074748fb40ea58462e41c6ffe0.svg"},
			{"\\uD83C\\uDDF2\\uD83C\\uDDFA", "https://discordapp.com/assets/177a86c5469056deb54cceb655495287.svg"},
			{"\\uD83C\\uDDF2\\uD83C\\uDDFD", "https://discordapp.com/assets/f27fbcd0c7a06889d0dc98c596c6ed82.svg"},
			{"\\uD83C\\uDDEB\\uD83C\\uDDF2", "https://discordapp.com/assets/b7eba6a12f6ce777ada1f5bae010d065.svg"},
			{"\\uD83C\\uDDF2\\uD83C\\uDDE9", "https://discordapp.com/assets/ce1869d9d4ccdb6364078e97e574f745.svg"},
			{"\\uD83C\\uDDF2\\uD83C\\uDDE8", "https://discordapp.com/assets/286ee3c806ce51dccca5384a8cdcfbf7.svg"},
			{"\\uD83C\\uDDF2\\uD83C\\uDDF3", "https://discordapp.com/assets/e508708061195f32eea31b847123b344.svg"},
			{"\\uD83C\\uDDF2\\uD83C\\uDDEA", "https://discordapp.com/assets/32fd4bb3a268c1375f36acb8c806df88.svg"}
		};
	}

	public static partial class Utility
	{
		#region Colors
		private static readonly Color colorInvisibleOffline = new Color(116, 127, 141);
		private static readonly Color colorActive = new Color(67, 181, 129);
		private static readonly Color colorIdle = new Color(250, 166, 26);
		private static readonly Color colorDoNotDisturb = new Color(240, 71, 71);
		#endregion

		public static readonly List<Regex> findRegexes = new List<Regex> { new Regex(@"<#\d+>"), new Regex(@"<@&\d+>"), new Regex(@"<@\d+>"), new Regex(@"<:\w+:\d+>"), new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase) };
		public static readonly List<Regex> findIDRegexes = new List<Regex> { new Regex(@"(?<=<#)\d+(?=>)"), new Regex(@"(?<=<@&)\d+(?=>)"), new Regex(@"(?<=<@)\d+(?=>)"), new Regex(@"(?<=:)\d+(?=>)") };

		public static readonly Regex emojiOut = new Regex(@":\w*:");

		public static Dictionary<string, Texture2D> texCache = new Dictionary<string, Texture2D>();

		public static void DownloadImage(string path, string url, Action<Texture2D> action = null)
		{
			if (!File.Exists(path))
			{
				using (WebClient client = new WebClient())
				{
					client.DownloadFileAsync(new Uri(url), path, path);
					client.DownloadFileCompleted += (a, b) =>
					{
						Texture2D texture = path.ToTexture();
						texCache[url] = texture;
						action?.Invoke(texture);
					};
				}
			}
			else
			{
				if (!texCache.ContainsKey(url) && !path.IsFileLocked())
				{
					texCache[url] = path.ToTexture();
					action?.Invoke(texCache[url]);
				}
				else if (texCache.ContainsKey(url)) action?.Invoke(texCache[url]);
			}
		}

		public static void DownloadSvg(string path, string unicode, Action<Texture2D> action = null)
		{
			if (!File.Exists(path))
			{
				using (WebClient client = new WebClient())
				{
					client.DownloadFileAsync(new Uri(unicode), path, path);
					client.DownloadFileCompleted += (a, b) =>
					{
						var svgDocument = SvgDocument.Open(path);
						var bitmap = svgDocument.Draw();
						bitmap.Save(path, ImageFormat.Png);

						Texture2D texture = path.ToTexture();
						texCache[unicode] = texture;
						action?.Invoke(texture);
					};
				}
			}
			else
			{
				if (!texCache.ContainsKey(unicode) && !path.IsFileLocked())
				{
					texCache[unicode] = path.ToTexture();
					action?.Invoke(texCache[unicode]);
				}
				else if (texCache.ContainsKey(unicode)) action?.Invoke(texCache[unicode]);
			}
		}

		public static void PremultiplyTexture(this Texture2D texture)
		{
			Color[] buffer = new Color[texture.Width * texture.Height];
			texture.GetData(buffer);
			for (int i = 0; i < buffer.Length; i++) buffer[i] = Color.FromNonPremultiplied(buffer[i].R, buffer[i].G, buffer[i].B, buffer[i].A);
			texture.SetData(buffer);
		}

		public static Texture2D ToTexture(this string path)
		{
			Texture2D texture;
			using (MemoryStream buffer = new MemoryStream(File.ReadAllBytes(path))) texture = Texture2D.FromStream(Main.instance.GraphicsDevice, buffer);
			texture.PremultiplyTexture();
			return texture;
		}

		public static bool CanJoin(this DiscordChannel channel) => (channel.PermissionsFor(DTT.Instance.currentGuild.CurrentMember) & Permissions.ReadMessageHistory) != 0 && (channel.PermissionsFor(DTT.Instance.currentGuild.CurrentMember) & Permissions.AccessChannels) != 0;

		public static void Save(this Config config)
		{
			using (StreamWriter writer = File.CreateText(DTT.ConfigPath)) writer.WriteLine(JsonConvert.SerializeObject(config, Formatting.Indented));
		}

		public static Config Load(this string path) => File.Exists(path) ? JsonConvert.DeserializeObject<Config>(File.ReadAllText(path).Replace("\r\n", ""), new JsonSerializerSettings { Formatting = Formatting.Indented }) : new Config();

		public static void CleanDir(this string path)
		{
			if (Directory.Exists(path))
			{
				DirectoryInfo di = new DirectoryInfo(path);
				foreach (FileInfo file in di.GetFiles()) file.Delete();
			}
		}

		public static Color PresenceColor(this DiscordPresence presence)
		{
			if (presence != null)
			{
				switch (presence.Status)
				{
					case UserStatus.Online:
						return colorActive;
					case UserStatus.Idle:
						return colorIdle;
					case UserStatus.DoNotDisturb:
						return colorDoNotDisturb;
					default:
						return colorInvisibleOffline;
				}
			}
			return colorInvisibleOffline;
		}

		public static bool IsFileLocked(this string path)
		{
			FileInfo file = new FileInfo(path);
			FileStream stream = null;

			try
			{
				stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
			}
			catch (IOException)
			{
				return true;
			}
			finally
			{
				stream?.Close();
			}

			return false;
		}

		public static void DownloadLog(DiscordChannel channel)
		{
			Task<IReadOnlyList<DiscordMessage>> task = channel.GetMessagesAsync(10);
			task.ContinueWith(t =>
			{
				foreach (DiscordMessage message in t.Result)
				{
					UIMessage uiMessage = new UIMessage(message);
					uiMessage.Width.Set(0f, 1f);
					uiMessage.Height.Set(40, 0);
					string path = $"{DTT.Users}{message.Author.Id}.png";
					DownloadImage(path, message.Author.AvatarUrl, texture => uiMessage.avatar = texture);
					uiMessage.RecalculateMessage();
					DTT.Instance.MainUI.gridMessages.Add(uiMessage);
					DTT.Instance.MainUI.gridMessages.RecalculateChildren();
				}
			});
		}

		public static string FormatMessageOut(this string text)
		{
			string[] words = text.Split(' ');
			for (var i = 0; i < words.Length; i++)
			{
				if (emojiOut.IsMatch(words[i]))
				{
					try
					{
						DiscordEmoji emoji = DiscordEmoji.FromName(DTT.Instance.currentClient, words[i]);
						words[i] = $"<:{emoji.Name}:{emoji.Id}>";
					}
					catch
					{
					}
				}
			}

			return words.Aggregate((x, y) => x + " " + y);
		}

		public static IEnumerable<Snippet> FormatMessage(this DiscordMessage message, float maxLineLength)
		{
			float x = 0;
			float y = 0;

			#region Header
			string username = message.Author.Username;
			Color color = Color.White;
			if (message.Channel.Guild != null && message.Channel.Guild.Members.Any(z => z.Id == message.Author.Id))
			{
				DiscordMember member = message.Channel.Guild.Members.First(z => z.Id == message.Author.Id);
				color = member.Color.Value.FromInt();
				username = member.Nickname ?? member.Username;
			}

			yield return new Snippet
			{
				Width = Main.fontMouseText.MeasureString(username).X,
				Height = Main.fontMouseText.MeasureString(username).Y,
				X = x,
				Y = y,
				Text = username,
				Color = color
			};
			x += Main.fontMouseText.MeasureString(username).X + 8;

			DateTime dateTime = message.CreationTimestamp.ToLocalTime().DateTime;
			string time = dateTime.DayOfWeek + " at " + dateTime.ToShortTimeString();
			yield return new Snippet
			{
				Width = Main.fontMouseText.MeasureString(time).X * 0.7f,
				Height = Main.fontMouseText.MeasureString(time).Y * 0.7f,
				X = x,
				Y = 20 * 0.3f,
				Text = time,
				Scale = 0.7f,
				OnHover = (spriteBatch, dimensions) => BaseLib.Utility.Utility.DrawMouseText(dateTime)
			};
			x = 0;
			y += 24;
			#endregion

			// Body
			foreach (string line in message.Content.Split('\n'))
			{
				foreach (string word in line.Split(' '))
				{
					string text = word;
					ulong id;

					if (x > maxLineLength)
					{
						x = 0;
						y += 24;
					}

					if (!string.IsNullOrWhiteSpace(text))
					{
						int index = findRegexes.FindIndex(z => z.IsMatch(text));
						string unicode = text.Select(z => "\\u" + ((int)z).ToString("X4")).Aggregate((f, s) => f + s);
						if (emojis.ContainsKey(unicode)) index = 3;

						switch (index)
						{
							case 0:     // Channels
								id = ulong.Parse(findIDRegexes[index].Match(text).Value);
								DiscordChannel channel = DTT.Instance.currentGuild.Channels.First(z => z.Id == id);
								text = $"#{channel.Name}";
								yield return new Snippet
								{
									Width = Main.fontMouseText.MeasureString(text).X,
									Height = Main.fontMouseText.MeasureString(text).Y,
									X = x,
									Y = y,
									Text = text,
									Color = new Color(105, 137, 200),
									OnClick = () => DTT.Instance.currentChannel = channel
								};
								break;
							case 1:     // Roles
								id = ulong.Parse(findIDRegexes[index].Match(text).Value);
								DiscordRole role = DTT.Instance.currentGuild.Roles.First(z => z.Id == id);
								text = $"@{role.Name}";
								yield return new Snippet
								{
									Width = Main.fontMouseText.MeasureString(text).X,
									Height = Main.fontMouseText.MeasureString(text).Y,
									X = x,
									Y = y,
									Text = text,
									Color = role.Color.Value.FromInt()
								};
								break;
							case 2:     // Mentions
								id = ulong.Parse(findIDRegexes[index].Match(text).Value);
								DiscordMember member = DTT.Instance.currentGuild.Members.First(z => z.Id == id);
								text = $"@{member.DisplayName}";
								yield return new Snippet
								{
									Width = Main.fontMouseText.MeasureString(text).X,
									Height = Main.fontMouseText.MeasureString(text).Y,
									X = x,
									Y = y,
									Text = text,
									Color = member.Color.Value.FromInt()
								};
								break;
							case 3:     // Emojis
								Texture2D emojiTexture = null;
								if (emojis.ContainsKey(unicode))
								{
									string path = $"{DTT.Emojis}{unicode.Replace('\\', '-')}.png";

									DownloadSvg(path, emojis[unicode], texture => emojiTexture = texture);

									yield return new Snippet
									{
										Width = 32,
										Height = 32,
										X = x,
										Y = y,
										OnDraw = (spriteBatch, dimensions) =>
										{
											if (texCache.ContainsKey(emojis[unicode]) && emojiTexture != null)
											{
												float scale = Math.Min(20f / emojiTexture.Width, 20f / emojiTexture.Height);
												Main.spriteBatch.End();
												spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, new RasterizerState { ScissorTestEnable = true }, null, Main.UIScaleMatrix);
												spriteBatch.Draw(emojiTexture, dimensions.Position(), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
												Main.spriteBatch.End();
												spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, null, new RasterizerState { ScissorTestEnable = true }, null, Main.UIScaleMatrix);
											}
										},
										OnHover = (spriteBatch, dimensions) => BaseLib.Utility.Utility.DrawMouseText(text)
									};
								}
								else
								{
									id = ulong.Parse(findIDRegexes[index].Match(text).Value);
									DiscordEmoji emoji = DTT.Instance.currentGuild.Emojis.FirstOrDefault(z => z.Id == id);
									if (emoji != null)
									{
										text = emoji.GetDiscordName();
										string url = "https://" + $"cdn.discordapp.com/emojis/{id}.png?size=256";
										DownloadImage($"{DTT.Emojis}{id}.png", url, texture => emojiTexture = texture);
										yield return new Snippet
										{
											Width = 32,
											Height = 32,
											X = x,
											Y = y,
											OnDraw = (spriteBatch, dimensions) =>
											{
												if (texCache.ContainsKey(url) && emojiTexture != null)
												{
													float scale = Math.Min(20f / emojiTexture.Width, 20f / emojiTexture.Height);
													Main.spriteBatch.End();
													spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, new RasterizerState { ScissorTestEnable = true }, null, Main.UIScaleMatrix);
													spriteBatch.Draw(emojiTexture, dimensions.Position(), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
													Main.spriteBatch.End();
													spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, null, new RasterizerState { ScissorTestEnable = true }, null, Main.UIScaleMatrix);
												}
											},
											OnHover = (spriteBatch, dimensions) => BaseLib.Utility.Utility.DrawMouseText(text)
										};
									}
								}
								x += 40;
								break;
							case 4:     // Links
								string link = text;
								text = "[Link]";
								yield return new Snippet
								{
									Width = Main.fontMouseText.MeasureString(text).X,
									Height = Main.fontMouseText.MeasureString(text).Y,
									X = x,
									Y = y,
									Text = text,
									Color = new Color(105, 137, 200),
									OnHover = (spriteBatch, dimensions) => BaseLib.Utility.Utility.DrawMouseText(link),
									OnClick = () =>
									{
										try
										{
											Process.Start(link);
										}
										catch
										{
										}
									}
								};
								break;
							default:    // Other
								yield return new Snippet
								{
									Width = Main.fontMouseText.MeasureString(text).X,
									Height = Main.fontMouseText.MeasureString(text).Y,
									X = x,
									Y = y,
									Text = text,
									Color = Color.White
								};
								break;
						}

						if (index != 3) x += Main.fontMouseText.MeasureString(text).X + 8;
					}
				}

				x = 0;
				y += 24;
			}
		}
	}

	public class Snippet
	{
		public float X;
		public float Y;
		public float Width;
		public float Height;
		public string Text;
		public Color Color = Color.White;
		public float Scale = 1f;

		public Action OnClick;
		public Action<SpriteBatch, CalculatedStyle> OnHover;
		public Action<SpriteBatch, CalculatedStyle> OnDraw;

		public override string ToString() => $"X: [{X}], Y: [{Y}], Width: [{Width}], Height: [{Height}], Text: [{Text}]";

		public Rectangle ToRectangle() => new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
	}
}