using System.Text.RegularExpressions;

namespace PackUtils
{
    public class SplittedAddress
    {
        public string Street { get; set; }

        public string Number { get; set; }

        public string Neighborhood { get; set; }
    }

    public static class AddressUtility
    {
        private static char[] SeparatorsAddress = new char[] { '\\', '/', ',', '.', '\t', '-', '_', '\'', '"' };

        public static SplittedAddress SplitAddress(string line1, string line2)
        {
            var splittedAddress = new SplittedAddress();

            splittedAddress.Street = line1;
            splittedAddress.Neighborhood = line2;
            splittedAddress.Number = "-";

            // remove special chars and try find some number sequence
            var address1_temp = line1?.Replace(AddressUtility.SeparatorsAddress, "") ?? "";
            var number1 = Regex.Match(address1_temp, @"\d+").Value;

            if (string.IsNullOrWhiteSpace(number1) == false)
            {
                splittedAddress.Street = address1_temp.Replace(number1, "");
                splittedAddress.Number = number1;
            }

            // repeat for address2
            var address2_temp = line2?.Replace(AddressUtility.SeparatorsAddress, "") ?? "";
            var number2 = Regex.Match(address2_temp, @"\d+").Value;

            if (string.IsNullOrWhiteSpace(number2) == false && string.IsNullOrWhiteSpace(number1) == true)
            {
                splittedAddress.Neighborhood = address2_temp.Replace(number2, "");
                splittedAddress.Number = number2;
            }

            if (string.IsNullOrWhiteSpace(splittedAddress.Street) == true)
            {
                splittedAddress.Street = "-";
            }

            if (string.IsNullOrWhiteSpace(splittedAddress.Neighborhood) == true)
            {
                splittedAddress.Neighborhood = "-";
            }

            // Handle size of address properties
            if (splittedAddress.Street.Length > 64)
            {
                splittedAddress.Street = splittedAddress.Street.Substring(0, 64);
            }

            if (splittedAddress.Number.Length > 16)
            {
                splittedAddress.Number = splittedAddress.Number.Substring(0, 16);
            }

            if (splittedAddress.Neighborhood.Length > 64)
            {
                splittedAddress.Neighborhood = splittedAddress.Neighborhood.Substring(0, 64);
            }

            return splittedAddress;
        }
    }
}
