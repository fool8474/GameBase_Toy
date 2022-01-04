namespace Script.Inject
{
    public interface IInjectedClass
    {
        // 의존성 주입을 통해 외부에서 인스턴스를 받아오는 역할을 하는 함수
        void Inject();
    }
}