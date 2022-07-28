namespace Lib.Service.Ticket.Module.FactoryTicketFileCreator.Model
{
    public class ExcelCreator<T>
    {
        public string Create(string fileCreateTargetDir, ExcelParam<T> excelParam)
        {
            var workbook = CreateFile(excelParam);
            return SaveFile(fileCreateTargetDir, workbook);
        }

        private string SaveFile(string fileCreateTargetDir, XSSFWorkbook workbook)
        {
            var filePath = Path.Combine(fileCreateTargetDir, Path.GetRandomFileName() + ".xlsx");
            var dirInfo = new DirectoryInfo(fileCreateTargetDir);
            if (!dirInfo.Exists) dirInfo.Create();
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                workbook.Write(fs);
            }
            return filePath;
        }

        private XSSFWorkbook CreateFile(ExcelParam<T> param)
        {
            var workbook = new XSSFWorkbook();
            var styleUtil = new UseableStyleUtil(workbook);
            var sheet = workbook.CreateSheet();
            int currentRowIndex = -1;
            CreateHeader(ref currentRowIndex, param, sheet, styleUtil);
            CreateDataRow(ref currentRowIndex, param, sheet, styleUtil);
            AutoSize(sheet);
            return workbook;
        }

        private void AutoSize(ISheet sheet)
        {
            for (var i = 0; i < sheet.GetRow(0).Cells.Count; i++)
            {
                sheet.AutoSizeColumn(i);
            }
        }

        private void CreateHeader(ref int currentRowIndex, ExcelParam<T> param, ISheet sheet, UseableStyleUtil styleUtil)
        {
            for(int i = 0; i < param.Headers.Length; i++)
            {
                currentRowIndex++;
                SetHeader(currentRowIndex, param, sheet, styleUtil, i);
            }
        }

        private void SetHeader(int currentRowIndex, ExcelParam<T> param, ISheet sheet, UseableStyleUtil styleUtil, int headersIdx)
        {
            var row = sheet.CreateRow(currentRowIndex);
            var header = param.Headers[headersIdx];
            for (var i = 0; i < header.Length; i++)
            {
                var cell = row.CreateCell(i);
                cell.SetCellValue(header[i].ToString());
                var style = styleUtil.GetStyle(param.HeaderStyles.ElementAtOrDefault(headersIdx));
                if (style != null) cell.CellStyle = style;
            }
        }

        private void CreateDataRow(ref int currentRowIndex, ExcelParam<T> param, ISheet sheet, UseableStyleUtil styleUtil)
        {
            var style = styleUtil.GetStyle(param.DataStyles);
            foreach (var item in param.DataList)
            {
                currentRowIndex++;
                SetData(currentRowIndex, sheet, style, item);
            }
        }

        private void SetData(int currentRowIndex, ISheet sheet, ICellStyle style, T item)
        {
            var row = sheet.CreateRow(currentRowIndex);
            var cellDataList = item.GetType().GetProperties()
                                        .OrderBy(o => o.GetCustomAttributes<TicketOrderAttribute>().Single().Order).ToList();
            for (var i = 0; i < cellDataList.Count; i++)
            {
                var cell = row.CreateCell(i);
                cell.SetCellValue(cellDataList[i].GetValue(item)?.ToString() ?? string.Empty);
                if (style != null) cell.CellStyle = style;
            }
        }
    }
}
