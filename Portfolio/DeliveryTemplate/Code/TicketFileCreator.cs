namespace Lib.Service.Ticket.Module.FactoryTicketFileCreator.Module.USPS
{
    public class TicketFileCreator : IFactoryTicketFileCreator
    {
        [Autowire]
        public ITicketDao TicketDao { get; set; }

        // 시작점
        public string Create(TicketFileCreatorParam param)
        {
            var excelParam = new ExcelParam<TicketExcelItem>();
            excelParam.Headers = GetHeaders();
            excelParam.DataList = GetTicketExcelList(param.JoinerId, param.OrderIds);
            return new ExcelCreator<TicketExcelItem>().Create(param.FileCreateTargetDirectory, excelParam);
        }

        // 엑셀 헤더
        private string[][] GetHeaders()
        {
            return new string[][] { new string[] { "OrderID", "OrderDate", "Full Name", "Company", "Address 1", "Address 2",  "City", "State", "Zip", "Phone"
            , "Email", "Item Name", "Quantity", "Package Value",  "Return Name", "Return Company" , "Return Address", "Return City", "Return State", "Return Zip"
            , "Return Phone", "Return Email" } };
        }

        // db에서 데이터 추출한뒤 이를 가공해서 excel에 넣을 데이터 list로 만듬
        private List<TicketExcelItem> GetTicketExcelList(int joinerId, int[] orderIds)
        {
            var ticketList = TicketDao.FindTicket(joinerId, orderIds); // db에서 데이터 추출
            var ticketExcelList = new List<TicketExcelItem>();
            foreach (var item in ticketList)
            {
                // 엑셀 데이터 row
                // 추출한 데이터 가공해서 실제 엑셀에 넣어야 하는 데이터 만들기
                var excelItem = TicketItemConverter.ToExcelItem(item); 
                ticketExcelList.Add(excelItem);
            }
            return ticketExcelList;
        }
    }
}