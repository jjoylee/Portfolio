/**
    입금 처리 CASE 분리
    GENERALPROC : 총 주문금액 == 입금액
    RETURNPOINTPROC : 총 주문금액 < 입금액
    DEDUCTIONPOINTPROC : 입금액 < 총 주문금액 < 입금액 + 고객이 보유한 포인트
    CANNOTPROC : 입금액 < 입금액 + 고객이 보유한 포인트 < 총 주문금액
**/
public enum ProcType
{
    GENERALPROC,
    RETURNPOINTPROC,
    DEDUCTIONPOINTPROC,
    CANNOTPROC
}