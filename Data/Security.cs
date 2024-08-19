using System.Text.RegularExpressions;
namespace MVCE.Data
{
    public static class Security
    {
        public static bool StringCheck(string[] strings)
        {
            if (strings==null | strings.Length == 0) { return false; }
            for(int i = 0; i < strings.Length; i++)
            {
                if (!Regex.Match(strings[i], "^\\w+$").Success) 
                {
                    return false;
                }
            }
            return true;
        }
        public static bool NumberCheck(int[] ints)
        {
            if (ints==null | ints.Length == 0) { return false; }
            for (int i = 0; i < ints.Length; i++)
            {
                if (!Regex.Match(ints[i].ToString(), "^\\d+$").Success || ints[i]==0)
                {
                    return false;
                }
            }
            return true;
        }
        public static bool NumberCheck(string[] numbers)
        {
            if (numbers == null | numbers.Length == 0) { return false; }
            for (int i = 0; i < numbers.Length; i++) 
            {
                int number;
                if(!int.TryParse(numbers[i], out number) || !Regex.Match(numbers[i], "^\\d+$").Success || number == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
