using System;
using System.Text;



namespace ExtensionMethods
{

	public static class Extensions
	{

		#region char

		public static bool IsEnglishLetter(this char _c)
		{
			return ((_c >= 'A' && _c <= 'Z') || (_c >= 'a' && _c <= 'z'));
		}

		public static bool IsUpper(this char _c)
		{
			return (_c >= 'A' && _c <= 'Z');
		}

		public static bool IsLower(this char _c)
		{
			return (_c >= 'a' && _c <= 'z');
		}

		#endregion char



		#region string

		public static string ToFirstCapsCase(this string _str)
		{
			char[] strAsChars = _str.ToLower().ToCharArray();

			strAsChars[0] = char.ToUpper(strAsChars[0]);
			for (var i = 1; i < strAsChars.Length; ++i)
			{
				if (!IsEnglishLetter(strAsChars[i - 1]))
				{
					strAsChars[i] = char.ToUpper(strAsChars[i]);
				}
			}

			return new string(strAsChars);
		}

		public static string SplitAtCaps(this string _str)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(_str[0]);
			for (var i = 1; i < _str.Length; ++i)
			{
				if (_str[i].IsUpper())
				{
					sb.Append(' ');
				}
				sb.Append(_str[i]);
			}

			return sb.ToString();
		}

		#endregion string

	} // end class

} // end namespace
