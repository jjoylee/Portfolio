public class MemberSignUpService : IMemberSignUpService
{
    [Autowire]
    public IMemberCreator MemberCreator { get; set; }

    [Autowire]
    public IParameterChecker ParameterChecker { get; set; }
    
    [Autowire]
    public IAfterSignUpService AfterSignUpService { get; set; }

    [Transaction]
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
}
