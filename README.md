# GameBase_ToyProject
* 게임의 베이스 시스템이 되는 코드들을 차근차근 작성해 나갑니다.
* 사용 라이브러리 / UniRx

1. DI Container (Injector)
- DI Container를 통한 의존성 주입 시스템
- 필요할때 등록해 둔 타입의 인스턴스를 DI Container를 통해 받아 사용할 수 있음.
2. CSV Reader
- Reflection을 이용하여 자동으로 테이블을 클래스에 파싱해주는 시스템
- 기획과의 협업을 위해 특정 단어가 들어간 열은 무시하도록 설정
- 기본적으로 파싱해 온 Data를 저장해두는 Tbl 클래스와 이를 가공하여 사용하는 Def 클래스를 함께 사용하는 시스템
3. Object Pool / Prefab Load
- Resources 및 Addressable 사용 케이스 모두 제공
- Object Pool을 통한 리소스 자원 관리
4. UI System
- MVC에 기반한 UI 시스템 구현
- Director를 통해 다양한 연출이 가능하도록 설정
- UIMgr에서는 IController Dictionary를 보유하고 있어 각 UI에 명령을 내릴 수 있음 
