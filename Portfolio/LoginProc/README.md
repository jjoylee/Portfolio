
<br>

## 📌 비회원 가입 프로세스 개선

✅ BEFORE

비회원의 가입할때 입력받은 이메일(아이디)와 비밀번호를 db에 그대로 저장    
회원과 동일한 테이블에 저장
한 컬럼을 flag로 사용해 비회원임을 구분     
비회원도 로그인 페이지에서 이메일과 비밀번호를 입력해 로그인 할 수 있다.   
비회원도 여러번 주문할 수 있다.

→ 비회원 계정임에도 회원 계정으로 잘못 알고 문의가 들어오는 경우가 1년에 4 ~ 5번씩 발생

<br>

✅ AFTER
  
비회원 가입할 때 난수를 생성해 아이디로 사용.    
주문번호와 비밀번호로 로그인할 수 있다.
비회원은 여러번 주문할 수 없다. (자동으로 로그아웃)
로그인 해 주문조회만 가능하다.

→ 수정된 이후로(2022.02.02) 비회원으로 인한 문의가 들어오지 않음.

<br>

## 📌 주요 코드 

기존에는 회원가입을 위한 서비스, 비회원가입을 위한 서비스가 분리되어 있었다.   
db에 회원, 비회원을 insert하는 로직도 각 서비스에 따로 존재했다.   
회원가입과 비회원가입 프로세스는 거의 동일하다. 중복된 부분도 많다.

파라미터 체크 → 중복 체크 → 회원 테이블에 데이터 저장 → 회원일 경우 회원가입 후 프로세스 실행(포인트 지급 등)    

비회원가입 로직을 수정하면서 프로세스 단위로 인터페이스를 만들었다.   
IParameterChecker → IDuplicateChecker → IMemberCreator → IAfterSignUpService    
기존의 중복된 로직을 지우고, 하나의 서비스에서 (비)회원 가입 처리할 수 있도록 수정했다.   

<br>

``` C#
    
// 회원가입, 비회원가입
public MemberItem SignUp(SignUpParam param)
{
    if (param.IsForNonmember())
    {
        return NonmemberSignUp(param);
    }
    return MemberSignUp(param);
}

private MemberItem NonmemberSignUp(SignUpParam param)
{
    ParameterChecker.Check(param);
    return MemberCreator.Create(param);
}

private MemberItem MemberSignUp(SignUpParam param)
{
    ParameterChecker.Check(param);
    var memberItem = MemberCreator.Create(param);
    AfterSignUpService.Proc(param, memberItem);
    return userItem;
}

```

https://github.com/jjoylee/portfolio/tree/master/Portfolio/LoginProc/Code

<br>

## 📌 결과

### 로그인 페이지

<img src="https://github.com/jjoylee/portfolio/blob/master/Portfolio/LoginProc/Image/login.png" width="700" height="400">

https://www.adprint.jp/Members/LoginView    

### 비회원 주문조회 페이지

<img src="https://github.com/jjoylee/portfolio/blob/master/Portfolio/LoginProc/Image/nonmember.png" width="700" height="400">

https://www.adprint.jp/Members/NonmemberLoginView
