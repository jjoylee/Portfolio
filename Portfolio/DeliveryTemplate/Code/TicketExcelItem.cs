namespace Lib.Service.Ticket.Module.FactoryTicketFileCreator.Model
{
    public class TicketExcelItem
    {
        [TicketOrder(1)]
        public int OrderId { get; set; }
        [TicketOrder(2)]
        public string OrderDate { get; set; } = string.Empty;
        [TicketOrder(3)]
        public string FullName { get; set; } = string.Empty;
        [TicketOrder(4)]
        public string Company { get; set; } = string.Empty;
        [TicketOrder(5)]
        public string Address1 { get; set; } = string.Empty;
        [TicketOrder(6)]
        public string Address2 { get; set; } = string.Empty;
        [TicketOrder(7)]
        public string City { get; set; } = string.Empty;
        [TicketOrder(8)]
        public string State { get; set; } = string.Empty;
        [TicketOrder(9)]
        public string Zip { get; set; } = string.Empty;
        [TicketOrder(10)]
        public string Phone { get; set; } = string.Empty;
        [TicketOrder(11)]
        public string Email { get; set; } = string.Empty;
        //.. 생략 
    }
}
