using Script.Manager.CSV;

namespace Script.Table
{
    public class DefPerson : DefBase
    {
        private string _name;
        private string _lastName;
        private DefAddress _address;

        private int _addressId;

        public DefPerson(string name, string lastName, int addressId)
        {
            _name = name;
            _lastName = lastName;
            _addressId = addressId;
        }

        public override string ToString()
        {
            return _name + " " + _lastName + " " + _address;
        }
        
        public override void Build()
        {
            TableMgr.TryGetDefData(_addressId, out _address);
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