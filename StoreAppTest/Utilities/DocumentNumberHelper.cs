

namespace StoreAppTest.Utilities
{
    using System.Text;

    public static class DocumentNumberHelper
    {
        public static string GetRecipeDocumentNumber(string prefix, int number)
        {
            int length = 7;
            int numberLength = number.ToString().Length;
            if (length < numberLength)
                return string.Format("{0}{1}", prefix, number.ToString());
            int prefixLength = prefix.Length;
            StringBuilder builder = new StringBuilder();
            builder.Append(prefix);
            var zerosLength = length - (prefixLength + numberLength);
            for (int i = 0; i < zerosLength; i++)
            {
                builder.Append("0");
            }
            builder.Append(number.ToString());

            return builder.ToString();
        }
    }
}
