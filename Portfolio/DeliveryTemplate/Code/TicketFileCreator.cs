namespace Lib.Service.Ticket.Module.FactoryTicketFileCreator.Module.USPS
{
    public class TicketFileCreator : IFactoryTicketFileCreator
    {
        [Autowire]
        public ITicketDao TicketDao { get; set; }

        public string Create(TicketFileCreatorParam param)
        {
            var excelParam = new ExcelParam<TicketExcelItem>();
            excelParam.Headers = GetHeaders();
            excelParam.DataList = GetTicketExcelList(param.JoinerId, param.OrderIds);
            return new ExcelCreator<TicketExcelItem>().Create(param.FileCreateTargetDirectory, excelParam);
        }

        private string[][] GetHeaders()
        {
            return new string[][] { new string[] { "OrderID", "OrderDate", "Full Name", "Company", "Address 1", "Address 2",  "City", "State", "Zip", "Phone"
            , "Email", "Item Name", "Quantity", "Package Value",  "Return Name", "Return Company" , "Return Address", "Return City", "Return State", "Return Zip"
            , "Return Phone", "Return Email" } };
        }

        private List<TicketExcelItem> GetTicketExcelList(int joinerId, int[] orderIds)
        {
            var ticketList = TicketDao.FindTicket(joinerId, orderIds);
            var ticketExcelList = new List<TicketExcelItem>();
            foreach (var item in ticketList)
            {
                var excelItem = TicketItemConverter.ToExcelItem(item);
                ticketExcelList.Add(excelItem);
            }
            return ticketExcelList;
        }
    }
}