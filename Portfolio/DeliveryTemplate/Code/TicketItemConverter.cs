namespace Lib.Service.Ticket.Module.FactoryTicketFileCreator.Module.USPS
{
    public static class TicketItemConverter
    {
        public TicketExcelItem ToExcelItem(TicketItem item)
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