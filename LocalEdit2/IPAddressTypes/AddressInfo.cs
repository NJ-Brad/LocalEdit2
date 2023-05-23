using System.Diagnostics.Metrics;

namespace LocalEdit2.IPAddressTypes
{
    public class AddressInfo
    {
        public string ip { get; set; } = "?.?.?.?"; //IP address
        public string country { get; set; } = "Unknown"; // IP country location in English language
        public string cc { get; set; } = "??"; // Two-letter country code in ISO 3166-1 alpha-2 format
    }
}
