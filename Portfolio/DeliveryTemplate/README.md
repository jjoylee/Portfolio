<br>

### 📌 DB 데이터 추출해서 엑셀에 보여주기

송장 출력을 위해 배송사 시스템에 주문 데이터를 엑셀 파일에 넣어 업로드해야한다.
즉 db에서 데이터를 추출해서 excel에 넣어 해당 파일을 고객이 다운받을 수 있도록 해야한다.
배송사마다 업로드하는 엑셀 데이터, 순서, 형식이 다르다.
여러 배송사가 사용할 수 있는 구조를 만들어야 한다.
데이터 형식도 배송사마다 다르기 때문에 db 데이터를 추출한 뒤 이를 가공해서 엑셀에 넣을 수 있어야 한다.

### 📌 주요 코드

[ExcelCreator.cs](./Code/ExcelCreator.cs)

```C#
    var row = sheet.CreateRow(currentRowIndex);
    
    // TicketOrderAttribute로 지정한 순서대로 item 속성들 정렬
    var cellDataList = item.GetType()
                            .GetProperties()
                            .OrderBy(o => o.GetCustomAttributes<TicketOrderAttribute>().Single().Order)
                            .ToList();

    for (var i = 0; i < cellDataList.Count; i++)
    {
        var cell = row.CreateCell(i);
        cell.SetCellValue(cellDataList[i].GetValue(item)?.ToString() ?? string.Empty);
        if (style != null) cell.CellStyle = style;
    }    
```

[TicketExcelItem.cs](./Code/TicketExcelItem.cs)

``` C#

    [TicketOrder(1)]
    public int OrderId { get; set; }
    [TicketOrder(2)]
    public string OrderDate { get; set; } = string.Empty;
    [TicketOrder(3)]
    public string FullName { get; set; } = string.Empty;
    [TicketOrder(4)]
    public string Company { get; set; } = string.Empty;

```

[TicketFileCreator.cs](./Code/TicketFileCreator.cs)

```C#

    // db에서 데이터 추출한뒤 이를 가공해서 excel에 넣을 데이터 list로 만듬
    var ticketList = TicketDao.FindTicket(siteId, orderIds); // db에서 데이터 추출
    var ticketExcelList = new List<TicketExcelItem>();
    foreach (var item in ticketList)
    {
        // 엑셀 데이터 row
        // 추출한 데이터 가공해서 실제 엑셀에 넣어야 하는 데이터 만들기
        var excelItem = TicketItemConverter.ToExcelItem(item); 
        ticketExcelList.Add(excelItem);
    }
    return ticketExcelList;
    
```

[TicketItemConverter.cs](./Code/TicketItemConverter.cs)

``` C#
    var excelItem = new TicketExcelItem();
    excelItem.OrderId = item.OrderId;
    excelItem.OrderDate = item.OrderDate;
    excelItem.FullName = item.FullName;
    excelItem.Company = item.Company
```

[주요 코드 링크](./Code)
