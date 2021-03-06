using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace PacketLogConverter
{
	public sealed class Util
	{
		/// <summary>
		/// Parse the string to long.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static bool ParseLong(string str, out long outValue)
		{
			outValue = 0;
			foreach (string split in str.Split('^'))
			{
				try
				{
					string trim = split.Trim().ToLower();
					NumberStyles style = NumberStyles.Integer;
					if (trim.StartsWith("0x"))
					{
						style = NumberStyles.HexNumber;
						trim = trim.Substring(2);
					}
					outValue ^= long.Parse(trim, style);
				}
				catch(Exception)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Parse the string to int.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static bool ParseInt(string str, out int outValue)
		{
			outValue = 0;
			foreach (string split in str.Split('^'))
			{
				try
				{
					string trim = split.Trim().ToLower();
					NumberStyles style = NumberStyles.Integer;
					if (trim.StartsWith("0x"))
					{
						style = NumberStyles.HexNumber;
						trim = trim.Substring(2);
					}
					outValue ^= int.Parse(trim, style);
				}
				catch(Exception)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Parse the string to int.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static bool ParseFloat(string str, out float outValue, float defaultVal)
		{
			bool ret = true;
			try
			{
				outValue = float.Parse(str);
			}
			catch
			{
				outValue = defaultVal;
				ret = false;
			}
			return ret;
		}

		/// <summary>
		/// Converts a hex string to int value. Works only with uppercase letters.
		/// </summary>
		/// <param name="str">The string to parse</param>
		/// <param name="start">The start offset</param>
		/// <param name="len">The parse length</param>
		/// <param name="result">Where to store the result</param>
		/// <returns>true if successfull else false</returns>
		public static bool ParseHexFast(string str, int start, int len, out int result)
		{
			result = 0;
			int count = len;
			int last = start + count - 1;

			if (str.Length <= last)
				return false;

			int parsed = 0;
			string s = str;
			for (int i = 0; i < count; i++)
			{
				int b = s[last - i];
				int val = b - '0';
				if (val < 0) return false;
				if (val > 9)
				{
					val = b - 'A';
					if (val < 0 || val > 5) return false;
					val += 10;
				}
				parsed += val << i * 4; // 4 bits for each char
			}

			result = parsed;

			return true;
		}

		public static bool ParseDecFast(string str, int start, int len, out int result)
		{
			result = 0;
			int parsed = 0;
			int count = len;
			int last = start + count - 1;
			string s = str;
			int order = 1;
			for (int i = 0; i < count; i++)
			{
				int b = s[last - i];
				int val = b - '0';
				if (val < 0 || val > 9) return false;
				parsed += val*order;
				order *= 10;
			}

			result = parsed;

			return true;
		}

		public unsafe static float GetFloat(uint data)
		{
			return *(float*) &data;
		}

		public unsafe static TimeSpan GetTimeSpan(uint data)
		{
			return *(TimeSpan*) &data;
		}

		public static DateTime GetDateTime(uint data)
		{
			System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
			return dateTime.AddSeconds((double)data);
		}

		/// <summary>
		/// Gets the object by index if it exists.
		/// </summary>
		/// <param name="list">The list.</param>
		/// <param name="index">The index.</param>
		/// <param name="def">The default value.</param>
		/// <returns>Object for the list, <c>default(T)</c> otherwise.</returns>
		public static T GetObjectByIndexSafe<T>(IIndexedContainer<T> list, int index, T def)
		{
			T ret = def;
			if (list != null && list.Count >= 0 && index >= 0 && index < list.Count)
			{
				ret = list[index];
			}

			return ret;
		}

		/// <summary>
		/// Gets the object by index if it exists.
		/// </summary>
		/// <param name="list">The list.</param>
		/// <param name="index">The index.</param>
		/// <param name="def">The default value.</param>
		/// <returns>Object for the list, <c>default(T)</c> otherwise.</returns>
		public static T GetObjectByIndexSafe<T>(IList<T> list, int index, T def)
		{
			T ret = def;
			if (list != null && list.Count >= 0 && index >= 0 && index < list.Count)
			{
				ret = list[index];
			}

			return ret;
		}
	}
}
