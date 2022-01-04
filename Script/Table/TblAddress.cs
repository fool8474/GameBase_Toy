using Script.Manager.CSV;

namespace Script.Table
{
    public class DefAddress : DefBase
    {
        public readonly string City;
        public readonly string Country;
        
        public DefAddress(string city, string country)
        {
            City = city;
            Country = country;
        }

        public override string ToString()
        {
            return City + " " + Country;
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
            // 타 Def에 사용될 경우를 고려
            var defAddress = new DefAddress(City, Country);
            return (Id, defAddress);
        }
    }
}