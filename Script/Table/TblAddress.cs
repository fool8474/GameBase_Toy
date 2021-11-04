using Script.Manager.CSV;

namespace Script.Table
{
    public class DefAddress : DefBase
    {
        private string _city;
        private string _country;
        
        public DefAddress(string city, string country)
        {
            _city = city;
            _country = country;
        }

        public override string ToString()
        {
            return _city + " " + _country;
        }
    }

    public class TblAddress : TblBase
    {
        public string City { get; set; }
        public string Country { get; set; }
        
        public override string ToString()
        {
            return City + " " + Country;
        }

        public override (int id, DefBase def) Build()
        {
            var defAddress = new DefAddress(City, Country);
            return (Id, defAddress);
        }
    }
}