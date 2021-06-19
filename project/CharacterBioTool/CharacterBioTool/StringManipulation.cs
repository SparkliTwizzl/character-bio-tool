using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterBioTool
{

	class StringManipulation
	{

		public static bool CharIsLetter(char _c)
		{
			return ((_c >= 'A' && _c <= 'Z') || (_c >= 'a' && _c <= 'z'));
		}

		public static string ReplaceCharsInString(string _str,
			char[] _toReplace, char[] _replaceWith)
		{
			// validate args
			if (_str.Length < 1)
				return _str;
			if (_toReplace.Length < 1 || _replaceWith.Length < 1
				|| _toReplace.Length != _replaceWith.Length)
				throw new ArgumentException("Char arrays must be non-empty and equal in length");

			char[] strAsChars = _str.ToCharArray();
			for (int i = 0; i < strAsChars.Length; ++i)
			{
				// TODO: add logic to find & replace
			}
			return new string(strAsChars);
		}

		public static string ConvertCaseToFirstCaps(string _str)
		{
			char[] strAsChars = _str.ToLower().ToCharArray();
			// first char has to be handled separately since
			//   there's no previous char to compare to
			if (CharIsLetter(strAsChars[0]))
				strAsChars[0] = char.ToUpper(strAsChars[0]);
			for (int i = 1; i < strAsChars.Length; ++i)
			{
				if (CharIsLetter(strAsChars[i]) && !CharIsLetter(strAsChars[i - 1]))
				{
					strAsChars[i] = char.ToUpper(strAsChars[i]);
				}
			}
			return new string(strAsChars);
		}

	} // end class

} // end namespace
