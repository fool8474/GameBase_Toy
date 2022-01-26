namespace Script.Manager.CSV
{
    // 스크립트에서 사용할 수 있도록 테이블로부터 가공된 데이터 클래스
    public class DefBase
    {
        public readonly int Id;

        public DefBase(int id)
        {
            Id = id;
        }

        // Tbl데이터를 사용하기 원활하도록 데이터 가공
        public virtual void Build() {}
    }
}