<br>

## 📌 은행 입금 결제 주문 입금 처리 프로세스 자동화

### 입금 처리 프로세스

✅ BEFORE

1. 은행 입금 결제 주문   
2. 고객이 은행에 입금    
3. 은행입금 내역 db에 수집     
4. 관리자가 수집내역과 주문금액을 확인   
5. **관리자가 입금내역을 기준으로 고객 주문 입금처리(입금 상태 변경)**

기존에는 관리자가 총 주문금액과 입금한 금액이 일치하는지 직접 계산    
입금액이 부족할 경우 포인트를 사용할 수 있는지 계산 후 수동으로 포인트 차감    
입금액이 많은 경우에도 계산해서 수동으로 포인트로 지급    

<br>

✅ AFTER

### 자동화 

입금한 금액이 총 주문금액(선택한 주문들의 주문금액 합)과 일치하는지, 포인트 사용해서 처리 가능한지 자동으로 체크   
차감 또는 지급할 포인트 자동으로 계산해서 처리    
입금처리 = 입금 내역 처리완료 상태로 변경 & 주문 상태 입금완료로 변경   
하나의 입금내역으로 여러 주문을 입금처리 할 수 있다. 

CASE1. 총 주문금액 == 입금액      
입금처리    

CASE2. 총 주문금액 < 입금액   
입금처리 후 남은 금액 자동으로 계산해서 고객에게 포인트로 지급   

CASE3. 입금액 < 총 주문금액 < 입금액 + 고객이 보유한 포인트   
부족한 금액만큼 고객 포인트 차감 후 입금처리

CASE4. 입금액 < 입금액 + 고객이 보유한 포인트 < 총 주문금액     
입금 처리 불가

<br>

## 📌 주요 코드 

```C#
  public enum ProcType
  {
      GENERALPROC, // CASE1
      RETURNPOINTPROC, // CASE2
      DEDUCTIONPOINTPROC, // CASE3
      CANNOTPROC // CASE4
  }
```

``` C#
public DepositMatchingInfo Get(int id, string orderIds)
{ 
    var depositTargetOrderList = GetDepositTargetOrderList(orderIds); // 해당 입금 내역으로 입금처리할 주문들
    var memberId = GetMemberId(depositTargetOrderList); // 주문의 고객
    var currentMemberPoint = GetCurrentPointByMemberId(memberId); // 현재 보유 포인트
    var totalOrderAmount = GetTotalOrderAmount(depositTargetOrderList); // 총 주문금액
    var depositProcPrice = GetDepositProcPrice(id); // 입금액, id == 입금내역 id
    return new DepositMatchingInfo  {
        TotalOrderAmount = totalOrderAmount, CurrentMemberPoint = currentMemberPoint,
        DepositProcPrice = depositProcPrice, MemberId = memberId
    };
}

```

``` C#
    [Transaction]
    public void ConfirmDeposit(int id, string orderIds, string memo, DepositMatchingInfo submitInfo, string adminId)
    {
        DepositConfirmOrders(id, orderIds, submitInfo, adminId);
        DepositDao.ConfirmDeposit(id, orderIds, memo, adminId);  // 입금내역 상태 변경
    }

    private void DepositConfirmOrders(int id, string orderIds, DepositMatchingInfo submitInfo, string adminId)
    {
        var currentInfo = DepositProcInfoService.GetDepositProcInfo(id, orderNums);
        // 기존에 조회했던 데이터 중 바뀐 것이 있는지 체크
        if (InfoIsChanged(currentInfo, submitInfo)) throw new Exception("주문을 확인해주세요."); 
        DoDepositConfirmProc(orderIds, adminId); // 주문 입금 상태 변경 (미입금 → 입금완료)
        DoPointProc(currentInfo, id, orderIds.Split('/')[0]); // 포인트 처리 (지급 or 차감)
    }
```

[주요 코드 링크](./Code)

<br>

## 📌 결과 

입금 처리를 위해 사람이 직접 금액 계산을 하지 않아도 된다.    
입금 처리에 소요되는 시간이 줄어든다.   
포인트를 잘못 지급하거나 금액을 잘못보는 등 기존의 수동 작업으로 발생할 수 있었던 문제를 예방할 수 있다.


CASE1. 총 주문금액 == 입금액       
<br>
<img src="./Image/GENERALPROC.png" width="600" height="350">

CASE2. 총 주문금액 < 입금액   
<br>
<img src="./Image/RETURNPOINTPROC.png" width="600" height="350">

CASE3. 입금액 < 총 주문금액 < 입금액 + 고객이 보유한 포인트   
<br>
<img src="./Image/DEDUCTIONPOINTPROC.png" width="600" height="350">

CASE4. 입금액 < 입금액 + 고객이 보유한 포인트 < 총 주문금액     
<br>
<img src="./Image/CANNOTPROC.png" width="600" height="350">
