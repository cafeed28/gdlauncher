using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System;

namespace Launcher
{
	public partial class PlayersPage
	{
		public static string Type;

		private static readonly HttpClient client = new HttpClient();

		public PlayersPage()
		{
			InitializeComponent();

			Task.Run(() => GetScores());
		}

		private async Task StartParser(string userString)
		{
			Debug.WriteLine("StartParser()");

			UserParser.InitalizeParser(userString);
			Debug.WriteLine(userString);
			Status.Dispatcher.Invoke(() => { Status.Text = ""; });

			//if (Type == "Moderator")
			//{
			//    var values1 = new Dictionary<string, string>
			//    {
			//        { "targetAccountID", "10" },
			//        { "secret", "Wmfd2893gb7" }
			//    };

			//    var content1 = new FormUrlEncodedContent(values1);

			//    var response1 = await client.PostAsync("http://servers.gdhost.pe.hu/HJuAU/getGJUserInfo20.php", content1);

			//    var userString1 = await response1.Content.ReadAsStringAsync();
			//}

			for (int i = 0; i < UserParser.usersLength; i++)
			{
				if (i == 10) break;
				int num = i + 1;
				UserParser.ParseUserString(i);
				usersList.Dispatcher.Invoke(() =>
				{
					Console.WriteLine(userString);
					Console.WriteLine(UserParser.diamonds);
					usersList.Items.Add(
						new UserItem
						{
							Name = num + ": " + UserParser.name,
							//Name = num + ": " + userString,
							Stars = UserParser.stars,
							Diamonds = UserParser.diamonds,
							GoldCoins = UserParser.gCoins,
							UserCoins = UserParser.sCoins,
							Demons = UserParser.demons,
							CreatorPoints = UserParser.cp
						}
					);
				});
			}
		}

		private async Task GetScores()
		{
			var values = new Dictionary<string, string>
			{
				{ "count", "10" },
				{ "gameVersion", "21" },
				{ "binaryVersion", "35" },
				{ "secret", "Wmfd2893gb7" },
				{ "udid", "udid" },
				{ "type", "top" }
			};

			var content = new FormUrlEncodedContent(values);

			var response = await client.PostAsync("http://gdasserverx.7m.pl/database/getGJScores20.php", content);

			var userString = await response.Content.ReadAsStringAsync();

			if (userString == null)
			{
				Status.Dispatcher.Invoke(() => { Status.Text = "Ошибка. Код ошибки: 30"; });
				Debug.WriteLine("Error 30: Connection timed out");
				return;
			}

			if (userString == "-1")
			{
				Status.Dispatcher.Invoke(() => { Status.Text = "Ошибка. Код ошибки: 31"; });
				Debug.WriteLine("Error 31: Server returned -1");
				return;
			}

			await Task.Run(() => StartParser(userString));
		}
	}
}
