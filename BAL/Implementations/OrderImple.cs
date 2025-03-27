using BAL.Interfaces;
using DAL.Data;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace BAL.Implementations
{
    public class OrderImple : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderImple(IOrderRepository orderRepository){
            _orderRepository = orderRepository;
        }

        public List<Order> getAllOrderByDateFilter(int? status,string? searchedOrder,string? filterBy,DateTime? startDate,DateTime? endDate)
        {
            return _orderRepository.getAllOrderByDateFilter(status,searchedOrder,filterBy,startDate,endDate);
        }

        public List<Order> getAllOrderByOptionFilter(int? status,string? searchedOrder,string? filterBy,DateTime? startDate,DateTime? endDate)
        {
          
            return _orderRepository.getAllOrderByOptionFilter(status,searchedOrder,filterBy,startDate,endDate);
          
        }

        public List<Order> getAllOrders()
        {
            return _orderRepository.getAllorders();
        }

        public List<Order> getAllOrdersBySearch(int? status,string? searchedOrder,string? filterBy,DateTime? startDate,DateTime? endDate)
        {
            return _orderRepository.getAllordersBySearch(status,searchedOrder,filterBy,startDate,endDate);
        }


        public List<Order> getAllOrdersByStatus(int? status,string? searchedOrder,string? filterBy,DateTime? startDate,DateTime? endDate)
        {
            return _orderRepository.getAllOrdersFromStatus(status,searchedOrder,filterBy,startDate,endDate);
        }

        public List<Orderstatus> getAllStatus()
        {
            return _orderRepository.getAllStatus();
        }

        public List<Order> getOrdersByFilters(int? status,string? searchedOrder,string? filterBy,DateTime? startDate,DateTime? endDate){
            return _orderRepository.GetAllOrdersByFilters(status,searchedOrder,filterBy,startDate,endDate);
        }

        public void createExcelSheet(string? searchedOrder, int? searchbystatus, string searchByPeriod, DateTime? startDate, DateTime? endDate)
        {
             HSSFWorkbook workbook = new HSSFWorkbook();
             HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("orders");
             HSSFFont font = (HSSFFont)workbook.CreateFont();

                var Company = workbook.CreateCellStyle();
                Company.Alignment = HorizontalAlignment.Left;
                var CompanyFont = workbook.CreateFont();
                CompanyFont.FontName = "Arial";
                CompanyFont.Color = HSSFColor.Blue.Index;
                CompanyFont.Boldweight = (short)FontBoldWeight.Bold;
                CompanyFont.FontHeightInPoints = ((short)16);
                Company.SetFont(CompanyFont);

                var Address = workbook.CreateCellStyle();
                Address.Alignment = HorizontalAlignment.Left;
                var AddressFont = workbook.CreateFont();
                AddressFont.FontName = "Arial";
                AddressFont.Boldweight = (short)FontBoldWeight.Bold;
                AddressFont.FontHeightInPoints = ((short)10);
                Address.SetFont(AddressFont);

                var Header = workbook.CreateCellStyle();
                Header.Alignment = HorizontalAlignment.Center;
                Header.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightBlue.Index;
                Header.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.LightBlue.Index;
                Header.FillPattern = FillPattern.SolidForeground;
                var HeaderFont = workbook.CreateFont();
                HeaderFont.FontName = "Arial";
                HeaderFont.Boldweight = (short)FontBoldWeight.Bold;
                HeaderFont.Color = HSSFColor.White.Index;
                HeaderFont.FontHeightInPoints = ((short)10);
                Header.SetFont(HeaderFont);
                Header.BorderLeft = BorderStyle.Thin;
                Header.BorderTop = BorderStyle.Thin;
                Header.BorderRight = BorderStyle.Thin;
                Header.BorderBottom = BorderStyle.Thin;

                var NumData = workbook.CreateCellStyle();
                var formatId = HSSFDataFormat.GetBuiltinFormat("##0.00");
                if (formatId == -1)
                {
                    var newDataFormat = workbook.CreateDataFormat();
                    NumData.DataFormat = newDataFormat.GetFormat("##0.00");
                }
                else{
                    NumData.DataFormat = formatId;
                }
                var Data = workbook.CreateCellStyle();
                Data.Alignment = HorizontalAlignment.Center;
                var DataFont = workbook.CreateFont();
                DataFont.FontName = "Arial";
                DataFont.FontHeightInPoints = ((short)9);
                Data.SetFont(DataFont);
                Data.BorderLeft = BorderStyle.Thin;
                Data.BorderTop = BorderStyle.Thin;
                Data.BorderRight = BorderStyle.Thin;
                Data.BorderBottom = BorderStyle.Thin;

                var linkData = workbook.CreateCellStyle();
                linkData.Alignment = HorizontalAlignment.Center;

                var linkDataFont = workbook.CreateFont();
                linkDataFont.FontName = "Arial";
                linkDataFont.Color = HSSFColor.Blue.Index;
                linkDataFont.FontHeightInPoints = ((short)9);
                linkDataFont.Underline = FontUnderlineType.Single;
                linkDataFont.Color = HSSFColor.Blue.Index;
                linkData.SetFont(linkDataFont);
                linkData.BorderLeft = BorderStyle.Thin;
                linkData.BorderTop = BorderStyle.Thin;
                linkData.BorderRight = BorderStyle.Thin;
                linkData.BorderBottom = BorderStyle.Thin;

                 int rowIndex = 2; 
                var row = sheet.CreateRow(rowIndex);
                var cell = row.CreateCell(4);
                cell.SetCellValue("order details");
                cell.CellStyle = Company;
                sheet.AddMergedRegion(new CellRangeAddress(4, 4, 4, 14));

                 rowIndex = rowIndex + 1;
                var row1 = sheet.CreateRow(rowIndex);
                var cell1 = row1.CreateCell(4);
                cell1.SetCellValue("1988/2019, 5th floor, Tower B, Bajrang Vihar, Patia, Bhubaneswar-400051, India");
                cell1.CellStyle = Address;
                sheet.AddMergedRegion(new CellRangeAddress(5, 5, 4, 14));

                rowIndex = 7;
                var SR_NO = 0; 
                var cellheaderindex = 0;

                var excelheaderrow = sheet.CreateRow(rowIndex);
                var excelheadercell = excelheaderrow.CreateCell(cellheaderindex);
                excelheadercell.SetCellValue("ID");
                excelheadercell.CellStyle = Header;

                cellheaderindex = cellheaderindex + 1;
                excelheadercell = excelheaderrow.CreateCell(cellheaderindex);
                excelheadercell.SetCellValue("Date");
                excelheadercell.CellStyle = Header;

                cellheaderindex = cellheaderindex + 1;
                excelheadercell = excelheaderrow.CreateCell(cellheaderindex);
                excelheadercell.SetCellValue("Customer");
                excelheadercell.CellStyle = Header;

                cellheaderindex = cellheaderindex + 1;
                excelheadercell = excelheaderrow.CreateCell(cellheaderindex);
                excelheadercell.SetCellValue("status");
                excelheadercell.CellStyle = Header;

                cellheaderindex = cellheaderindex + 1;
                excelheadercell = excelheaderrow.CreateCell(cellheaderindex);
                excelheadercell.SetCellValue("Payment Mode");
                excelheadercell.CellStyle = Header;

                cellheaderindex = cellheaderindex + 1;
                excelheadercell = excelheaderrow.CreateCell(cellheaderindex);
                excelheadercell.SetCellValue("Ratting");
                excelheadercell.CellStyle = Header;

                cellheaderindex = cellheaderindex + 1;
                excelheadercell = excelheaderrow.CreateCell(cellheaderindex);
                excelheadercell.SetCellValue("Total Amount");
                excelheadercell.CellStyle = Header;
        }
    }
}