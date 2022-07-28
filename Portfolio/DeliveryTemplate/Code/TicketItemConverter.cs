namespace Lib.Service.Ticket.Module.FactoryTicketFileCreator.Module
{
    public static class TicketItemConverter
    {
        // db에서 select해 온 정보로 엑셀에 넣을 데이터들을 만들어 item에 넣는다.
        // db 데이터를 그대로 사용하지 않는 경우가 많다.
        // 아래와 같이 코딩으로 사용할 데이터를 쉽게 변경해서 넣을 수 있다. 
        public static TicketExcelItem ToExcelItem(TicketItem item)
        {
            var excelItem = new TicketExcelItem();
            excelItem.OrderId = item.OrderId;
            excelItem.OrderDate = item.OrderDate;
            excelItem.FullName = item.FullName;
            excelItem.Company = item.Company
            excelItem.Address1 = item.JsonAddress["Address1"].ToString();
            excelItem.Address2 = item.JsonAddress["Address2"].ToString();
            excelItem.City = item.JsonAddress["City"].ToString();
            excelItem.State = item.JsonAddress["State"].ToString();
            excelItem.Zip = item.Zip;
            excelItem.Phone = item.Phone;
            excelItem.Email = item.Email;
            // .. 생략
            return excelItem;
        }
    }
}