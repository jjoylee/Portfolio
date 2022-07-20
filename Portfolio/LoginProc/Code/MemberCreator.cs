// 회원, 비회원 데이터 db에 INSERT
public class MemberCreator : IMemberCreator
{
    // 중복된 회원, 비회원 체크
    [Autowire]
    public IDuplicateChecker DuplicateChecker { get; set; }

    public MemberItem Create(SignUpParam param)
    {
        var memberItem = new MemberItem {
            Id = GetMemberId(param),
            // ... 생략
        };
        CheckDuplicateMemberExist(memberItem);
        return InsertMember(memberItem);
    }

    private string GetMemberId(SignUpParam param)
    {
        if (param.IsForNonmember())
        {
            // 비회원 아이디로 사용할 난수 생성
            return GetUniqNonmemberId(param);
        }
        return param.Id;
    }

    private void CheckDuplicateUserExist(MemberItem memberItem)
    {
        DuplicateChecker.Check(memberItem.Id);
    }

    // ... 생략
}
