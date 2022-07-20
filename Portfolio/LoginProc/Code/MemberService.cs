public class MemberService : IMemberService
{
    [Autowire]
    public IMemberSignUpService MemberSignUpService { get; set; }

    // 회원, 비회원 가입 시작점
    [Transaction]
    public UserItem SignUp(SignUpParam param)
    {
        return MemberSignUpService.SignUp(param);
    }
}