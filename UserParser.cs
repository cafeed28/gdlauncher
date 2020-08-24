using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher
{
	public class UserParser
	{
		public static int usersLength;

		public static string users;
		public static string[] text;

		public static string name = "N/A";
		public static string stars = "N/A";
		public static string diamonds = "N/A";
		public static string gCoins = "N/A";
		public static string sCoins = "N/A";
		public static string demons = "N/A";
		public static string cp = "N/A";

		public static void InitalizeParser(string usersString)
		{
			Debug.WriteLine("InitalizeParser()");
			users = usersString.Remove(usersString.Length - 1);
			text = users.Split('|');
			usersLength = text.Length;
			char last = users.Last();
			for (int i = 0; i < usersLength; i++)
			{
				Debug.WriteLine("i = " + i + ", text = " + text[i]);
			}
		}

		public static void ParseUserString(int i)
		{
			Debug.WriteLine("ParseUserString()");
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			string[] values = text[i].Split(':');
			for (int j = 0; j < values.Length - 1; j += 2)
			{
				if (int.TryParse(values[j], out int index))
				{
					if (values[j + 1] == "")
					{
						dictionary.Add(index, "0");
						return;
					}
					dictionary.Add(index, values[j + 1]);
				}
			}

			foreach (KeyValuePair<int, string> pair in dictionary)
			{
				switch (pair.Key)
				{
					case 1:
						name = pair.Value;
						break;
					case 3:
						stars = pair.Value;
						break;
					case 46:
						diamonds = pair.Value;
						break;
					case 13:
						gCoins = pair.Value;
						break;
					case 17:
						sCoins = pair.Value;
						break;
					case 4:
						demons = pair.Value;
						break;
					case 8:
						cp = pair.Value;
						break;
				}
			}
		}
	}
}
