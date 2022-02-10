1. Plugins 폴더에 WebAuth 폴더를 복사한다.

2. scripts 폴더의WebAuthManager.cs 를 원하는 오브젝트에 컴포넌트 추가한다.
- WebAuthManager.cs의 Check_Auth()를 인증이 필요한 시점에 호출한다.
- OnSuccessEvent(bool, int, string) 이벤트가 발생했을때 처리할 함수를 연결해 준다.
- bool 의 값이 true일 경우 인증 성공

3. 추가된 컴포넌트에 AuthInfo 값을 입력한다. (영소문자 or 숫자)
- developCode : 개발사 코드(4자리, 브로틴에서 지정값 배포)
- gameCode : 게임 종류 코드(4자리, 브로틴에서 지정값 배포)
- versionNumber : 게임 버전 넘버(4자리, 개발사에서 입력. 빌드 전달 시 버전넘버 브로틴 측에 전달 부탁드립니다)

