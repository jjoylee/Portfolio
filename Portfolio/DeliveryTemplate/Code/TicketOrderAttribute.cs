namespace Lib.Service.Ticket.Module.FactoryTicketFileCreator.Model
{
    // 엑셀에서 표시할 데이터 순서 설정
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class TicketOrderAttribute : Attribute
    {
        public int Order;

        public TicketOrderAttribute(int order)
        {
            this.Order = order;
        }
    }
}
