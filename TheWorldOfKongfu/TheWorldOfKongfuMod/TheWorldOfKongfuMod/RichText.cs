using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorldOfKongfuMod
{
    public class RichText
    {
        public static string Green(string text)
        {
            return "<color=#90ed4c>" + text + "</color>";
        }

        public static string Blue(string text)
        {
            return "<color=#1287A8>" + text + "</color>";
        }

        public static string Purple(string text)
        {
            return "<color=#9932CC>" + text + "</color>";
        }

        public static string Orange(string text)
        {
            return "<color=#DA621E>" + text + "</color>";
        }

        public static string Red(string text)
        {
            return "<color=#CD594A>" + text + "</color>";
        }

        public static string Golden(string text)
        {
            return "<color=#BCA136>" + text + "</color>";
        }

        public static string SeparateLine(string text)
        {
            int num = 68 - text.Length;
            return new string('═', num / 2) + text + new string('═', num / 2);
        }

        public static string ColorText(string text)
        {
            if (!text.Contains("+"))
            {
                return Red(text);
            }
            return Green(text);
        }
    }

}
