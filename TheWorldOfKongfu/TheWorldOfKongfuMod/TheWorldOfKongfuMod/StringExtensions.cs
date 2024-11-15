using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TheWorldOfKongfuMod
{
    public static class StringExtensions
    {
        public static Color ToColor(this string htmlString)
        {
            string text = htmlString;
            if (text.StartsWith("#"))
            {
                text = text.Remove(0, 1);
            }
            if (text.Length == 8)
            {
                byte a = byte.Parse(text.Substring(0, 2), NumberStyles.HexNumber);
                byte r = byte.Parse(text.Substring(2, 2), NumberStyles.HexNumber);
                byte g = byte.Parse(text.Substring(4, 2), NumberStyles.HexNumber);
                byte b = byte.Parse(text.Substring(6, 2), NumberStyles.HexNumber);
                return new Color32(r, g, b, a);
            }
            if (text.Length == 6)
            {
                byte r2 = byte.Parse(text.Substring(0, 2), NumberStyles.HexNumber);
                byte g2 = byte.Parse(text.Substring(2, 2), NumberStyles.HexNumber);
                byte b2 = byte.Parse(text.Substring(4, 2), NumberStyles.HexNumber);
                return new Color32(r2, g2, b2, byte.MaxValue);
            }
            Debug.LogErrorFormat("Could not convert '{0}' to a color.", htmlString);
            return Color.magenta;
        }

        public static T ToEnum<T>(this string str)
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException($"Type '{typeof(T)}' is not an enum.");
            }
            string text = str.Replace("-", "_");
            try
            {
                return (T)Enum.Parse(typeof(T), text, ignoreCase: true);
            }
            catch
            {
                Debug.LogErrorFormat("Could not convert '{0}' to enum type '{1}'.", text, typeof(T).Name);
                return default(T);
            }
        }

        public static float ToFloat(this string str)
        {
            if (!float.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
            {
                Debug.LogErrorFormat("Could not convert '{0}' to float.", str);
            }
            return result;
        }

        public static int ToInt(this string str)
        {
            if (!int.TryParse(str, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
            {
                Debug.LogErrorFormat("Could not convert '{0}' to int.", str);
            }
            return result;
        }

        public static bool ToBool(this string str)
        {
            if (str.Equals("1") || str.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            if (str.Equals("0") || str.Equals("false", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            Debug.LogErrorFormat("Could not convert '{0}' to bool.", str);
            return false;
        }
    }


}
