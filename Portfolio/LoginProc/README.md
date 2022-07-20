
<br>

## 📌 로그인 프로세스 개선

✅ BEFORE

비회원의 가입할때 입력받은 이메일(아이디)와 비밀번호를 db에 그대로 저장    
한 컬럼을 flag로 사용해 비회원임을 구분     
비회원도 로그인 페이지에서 이메일과 비밀번호를 입력해 로그인 할 수 있다.
비회원도 여러번 주문할 수 있다.

→ 비회원 계정임에도 회원 계정으로 잘못 알고 문의가 들어오는 경우가 1년에 4 ~ 5번씩 발생

<br>

✅ AFTER
  
비회원 가입할 때 난수를 생성해 아이디로 사용.    
주문번호와 비밀번호로 로그인할 수 있다.
한번 주문한 경우 다시 주문할 수 없다. 
로그인 해 주문조회만 가능하다.

→ 수정된 이후로(2022.02.02) 비회원으로 인한 문의가 들어오지 않음.

<br>

## 📌 결과

### 로그인 페이지

<img src="https://github.com/jjoylee/portfolio/blob/master/Portfolio/LoginProc/Image/login.png" width="700" height="400">

https://www.adprint.jp/Members/LoginView    

### 비회원 주문조회 페이지

<img src="https://github.com/jjoylee/portfolio/blob/master/Portfolio/LoginProc/Image/nonmember.png" width="700" height="400">

https://www.adprint.jp/Members/NonmemberLoginView
