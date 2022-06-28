### 📂 Middleware that enables an application to support Yahoo Japan's OAuth 2.0 authentication workflow.

<br>

### 📌 How To Use

```csharp
// Startup.cs
app.UseYahooJapanAuthentication(
  new YahooJapanAuthenticationOptions
  {
    ClientId = "ClientId",
    ClientSecret = "ClientSecret",
    SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie
  }
);
```
<br>

### 📌 Owin Authentication Flow

### AuthenticationMiddleware


CreateHanlder()에서 AuthenticationHandler 인스턴스 생성

AuthenticationOptions의 기본 값들 설정

<br>

### FLOW

1. SNS 로그인 버튼 클릭 

&nbsp;&nbsp;&nbsp;&nbsp;인증 관리자에 대한 Challenge 생성 → 응답의 상태 코드를 401로 변경

&nbsp;&nbsp;&nbsp;&nbsp;Challenge : 신원을 확인해 달라는 요청

```csharp
context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
```

<br>

2. AuthenticationHandler.ApplyResponseChallengeAsync()

&nbsp;&nbsp;&nbsp;&nbsp;주로 401 상태 코드의 응답(Challenge)을 처리하기 위한 로직 구현

&nbsp;&nbsp;&nbsp;&nbsp;Resource Owner와 상호 작용하고 보호된 리소스에 액세스할 수 있는 권한을 얻기 위해 Authorization Endpoint로 redirect

&nbsp;&nbsp;&nbsp;&nbsp;Resource Owner는 로그인 후 리소스 사용에 동의

<br>

3. AuthenticationHandler.InvokeAsync()

&nbsp;&nbsp;&nbsp;&nbsp;Authorization Server의 올바른 인증 콜백인지 확인한다. 

&nbsp;&nbsp;&nbsp;&nbsp;올바른 인증 콜백이면 AuthenticateAsync  메소드를 호출해서 인증 결과를 반환한다.

```csharp
AuthenticationTicket ticket = await AuthenticateAsync();
```

<br>

4. AuthenticationHandler.AuthenticateCoreAsync()

&nbsp;&nbsp;&nbsp;&nbsp;state와 code 파라미터 값으로 validation check

&nbsp;&nbsp;&nbsp;&nbsp;code 값을 이용해 TokenEndPoint에 요청해서 access token을 발급받는다.

&nbsp;&nbsp;&nbsp;&nbsp;access token을 사용해 리소스 요청(계정 정보) 

<br>

5. 얻은 정보를 DB에 넣어 회원가입 또는 로그인

<br>

### 📌 Reference

https://jjoystory.tistory.com/45

https://jjoystory.tistory.com/46

https://jjoystory.tistory.com/47
