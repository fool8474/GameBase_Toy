using Script.Manager.CSV;

namespace Script.Table
{
    public class DefPerson : DefBase
    {
        public readonly string Name;
        public readonly string LastName;
        public DefAddress Address;

        public readonly int AddressId;

        public DefPerson(string name, string lastName, int addressId)
        {
            Name = name;
            LastName = lastName;
            AddressId = addressId;
        }

        public override string ToString()
        {
            return Name + " " + LastName + " " + Address;
        }
        
        public override void Build()
        {
            // 타 def 사용 데이터를 가져오도록 함.
            TableMgr.TryGetDefData(AddressId, out Address);
        }
    }
    
    public class TblPerson : TblBase
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public int AddressId { get; set; }

        public override string ToString()
        {
            return Id + " " + Name + " " + LastName + " " + AddressId;
        }

        public override (int id, DefBase def) Build()
        {
            var defPerson = new DefPerson(Name, LastName, AddressId);
            return (Id, defPerson);
        }
    }
}