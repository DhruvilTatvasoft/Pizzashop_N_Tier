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
        public OrderImple(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public List<Order> getAllOrderByDateFilter(int? status, string? searchedOrder, string? filterBy, DateTime? startDate, DateTime? endDate)
        {
            return _orderRepository.getAllOrderByDateFilter(status, searchedOrder, filterBy, startDate, endDate);
        }

        public List<Order> getAllOrderByOptionFilter(int? status, string? searchedOrder, string? filterBy, DateTime? startDate, DateTime? endDate)
        {

            return _orderRepository.getAllOrderByOptionFilter(status, searchedOrder, filterBy, startDate, endDate);

        }

        public List<Order> getAllOrders()
        {
            return _orderRepository.getAllorders();
        }

        public List<Order> getAllOrdersBySearch(int? status, string? searchedOrder, string? filterBy, DateTime? startDate, DateTime? endDate)
        {
            return _orderRepository.getAllordersBySearch(status, searchedOrder, filterBy, startDate, endDate);
        }


        public List<Order> getAllOrdersByStatus(int? status, string? searchedOrder, string? filterBy, DateTime? startDate, DateTime? endDate)
        {
            return _orderRepository.getAllOrdersFromStatus(status, searchedOrder, filterBy, startDate, endDate);
        }

        public List<Orderstatus> getAllStatus()
        {
            return _orderRepository.getAllStatus();
        }

        public List<Order> getOrdersByFilters(int? status, string? searchedOrder, string? filterBy, DateTime? startDate, DateTime? endDate)
        {
            return _orderRepository.GetAllOrdersByFilters(status, searchedOrder, filterBy, startDate, endDate);
        }


        private void ApplyMergedCellStyle(ISheet sheet, CellRangeAddress range, ICellStyle style)
        {
            for (int i = range.FirstRow; i <= range.LastRow; i++)
            {
                var row = sheet.GetRow(i) ?? sheet.CreateRow(i);
                for (int j = range.FirstColumn; j <= range.LastColumn; j++)
                {
                    var cell = row.GetCell(j) ?? row.CreateCell(j);
                    cell.CellStyle = style;
                }
            }
        }

        public void createExcelSheet(int? status, string? searchedOrder, string? filterBy, DateTime? startDate, DateTime? endDate)
        {
            try
            {

                HSSFWorkbook workbook = new HSSFWorkbook();
                HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("orders");
                HSSFFont font = (HSSFFont)workbook.CreateFont();
                HSSFPalette palette = workbook.GetCustomPalette();
                palette.SetColorAtIndex(HSSFColor.Blue.Index,
                                        (byte)27,
                                        (byte)101,
                                        (byte)161);

                var Company = workbook.CreateCellStyle();
                Company.Alignment = HorizontalAlignment.Left;
                Company.VerticalAlignment = VerticalAlignment.Center;
                var CompanyFont = workbook.CreateFont();
                CompanyFont.FontName = "Arial";
                CompanyFont.Color = HSSFColor.Blue.Index;
                CompanyFont.Boldweight = (short)FontBoldWeight.Bold;
                CompanyFont.FontHeightInPoints = ((short)16);
                Company.SetFont(CompanyFont);

                var Address = workbook.CreateCellStyle();
                Address.Alignment = HorizontalAlignment.Center;
                var AddressFont = workbook.CreateFont();
                AddressFont.FontName = "Arial";
                AddressFont.Boldweight = (short)FontBoldWeight.Bold;
                AddressFont.FontHeightInPoints = ((short)10);
                Address.SetFont(AddressFont);

                var Address1 = workbook.CreateCellStyle();
                Address1.Alignment = HorizontalAlignment.Center;
                var AddressFont1 = workbook.CreateFont();
                AddressFont1.FontName = "Arial";
                AddressFont1.Boldweight = (short)FontBoldWeight.Bold;
                AddressFont1.FontHeightInPoints = ((short)10);
                Address1.SetFont(AddressFont);


                var Header = workbook.CreateCellStyle();
                Header.Alignment = HorizontalAlignment.Center;
                Header.VerticalAlignment = VerticalAlignment.Center;
                Header.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Blue.Index;
                Header.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Blue.Index;
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
                else
                {
                    NumData.DataFormat = formatId;
                }
                var Data = workbook.CreateCellStyle();
                Data.Alignment = HorizontalAlignment.Center;
                Data.VerticalAlignment = VerticalAlignment.Center;
                var DataFont = workbook.CreateFont();
                DataFont.FontName = "Arial";
                DataFont.FontHeightInPoints = ((short)9);
                Data.SetFont(DataFont);
                Data.BorderLeft = BorderStyle.Thin;
                Data.BorderTop = BorderStyle.Thin;
                Data.BorderRight = BorderStyle.Thin;
                Data.BorderBottom = BorderStyle.Thin;


                List<Orderstatus> statusList = _orderRepository.getAllStatus();
                string statusName = "";
                if (status.Value == 0)
                {
                    statusName = "All status";
                }
                else
                {
                    foreach (var Status in statusList)
                    {
                        if (Status.Orderstatusid == status)
                        {
                            statusName = Status.Statusname;
                            break;
                        }
                    }
                }

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

                int rowIndex = 1;
                var row = sheet.CreateRow(rowIndex);
                var cell = row.CreateCell(0);
                cell.SetCellValue("Status");
                cell.CellStyle = Header;
                sheet.AddMergedRegion(new CellRangeAddress(1, 2, 0, 1));
                ApplyMergedCellStyle(sheet, new CellRangeAddress(1, 2, 0, 1), Header);
                
                cell = row.CreateCell(7);
                cell.SetCellValue("Search Text:");
                cell.CellStyle = Header;
                sheet.AddMergedRegion(new CellRangeAddress(1, 2, 7, 8));
                ApplyMergedCellStyle(sheet, new CellRangeAddress(1, 2, 7, 8), Header);

                cell = row.CreateCell(9);
                cell.SetCellValue(searchedOrder);
                cell.CellStyle = Data;
                sheet.AddMergedRegion(new CellRangeAddress(1, 2, 9,12));
                ApplyMergedCellStyle(sheet, new CellRangeAddress(1, 2, 9,12), Data);

                cell = row.CreateCell(2);
                cell.SetCellValue(statusName);
                cell.CellStyle = Data;
                sheet.AddMergedRegion(new CellRangeAddress(1, 2, 2, 5));
                ApplyMergedCellStyle(sheet, new CellRangeAddress(1, 2, 2, 5), Data);
                

                // cell = row.CreateCell(7);
                // cell.SetCellValue(searchedOrder);
                // cell.CellStyle = Header;
                // sheet.AddMergedRegion(new CellRangeAddress(4, 5, 7, 8));

                rowIndex = 4;
                row = sheet.CreateRow(rowIndex);
                cell = row.CreateCell(0);
                cell.SetCellValue("Date : ");
                cell.CellStyle = Header;
                sheet.AddMergedRegion(new CellRangeAddress(4, 5, 0, 1));
                ApplyMergedCellStyle(sheet, new CellRangeAddress(4, 5, 0, 1), Header);

                // row = sheet.CreateRow(rowIndex);
                cell = row.CreateCell(7);
                cell.SetCellValue("No of records:");
                cell.CellStyle = Header;
                sheet.AddMergedRegion(new CellRangeAddress(4, 5, 7, 8));
                ApplyMergedCellStyle(sheet, new CellRangeAddress(4, 5, 7, 8), Header);

          
                cell = row.CreateCell(2);
                cell.SetCellValue(filterBy);
                cell.CellStyle = Data;
                sheet.AddMergedRegion(new CellRangeAddress(4, 5, 2, 5));
                ApplyMergedCellStyle(sheet, new CellRangeAddress(4, 5, 2, 5), Data);

                List<Order> orders = _orderRepository.GetAllOrdersByFilters(status, searchedOrder, filterBy, startDate, endDate);

                cell = row.CreateCell(9);
                cell.SetCellValue(orders.Count);
                cell.CellStyle = Data;
                sheet.AddMergedRegion(new CellRangeAddress(4, 5, 9, 12));
                ApplyMergedCellStyle(sheet, new CellRangeAddress(4, 5, 9, 12), Data);

                rowIndex = 8;
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
                sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, cellheaderindex, cellheaderindex + 2));
                ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex, cellheaderindex, cellheaderindex + 2), Header);

                cellheaderindex = cellheaderindex + 3;
                excelheadercell = excelheaderrow.CreateCell(cellheaderindex);
                excelheadercell.SetCellValue("Customer");
                excelheadercell.CellStyle = Header;
                sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, cellheaderindex, cellheaderindex + 2));
                ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex, cellheaderindex, cellheaderindex + 2), Header);


                cellheaderindex = cellheaderindex + 3;
                excelheadercell = excelheaderrow.CreateCell(cellheaderindex);
                excelheadercell.SetCellValue("status");
                excelheadercell.CellStyle = Header;
                sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, cellheaderindex, cellheaderindex + 2));
                ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex, cellheaderindex, cellheaderindex + 2), Header);

                cellheaderindex = cellheaderindex + 3;
                excelheadercell = excelheaderrow.CreateCell(cellheaderindex);
                excelheadercell.SetCellValue("Payment Mode");
                excelheadercell.CellStyle = Header;
                sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, cellheaderindex, cellheaderindex + 1));
                ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex, cellheaderindex, cellheaderindex + 1), Header);

                cellheaderindex = cellheaderindex + 2;
                excelheadercell = excelheaderrow.CreateCell(cellheaderindex);
                excelheadercell.SetCellValue("Ratting");
                excelheadercell.CellStyle = Header;
                sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, cellheaderindex, cellheaderindex + 1));
                ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex, cellheaderindex, cellheaderindex + 1), Header);

                cellheaderindex = cellheaderindex + 2;
                excelheadercell = excelheaderrow.CreateCell(cellheaderindex);
                excelheadercell.SetCellValue("Total Amount");
                excelheadercell.CellStyle = Header;
                sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, cellheaderindex, cellheaderindex + 1));
                ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex, cellheaderindex, cellheaderindex + 1), Header);

               
                foreach (var order in orders)
                {
                    rowIndex = rowIndex + 1;
                    SR_NO = SR_NO + 1;
                    var cellindex = 0;
                    var gridrow = sheet.CreateRow(rowIndex);
                    var gridcell = gridrow.CreateCell(cellindex);

                    gridcell.SetCellValue(order.Orderid);

                    gridcell.CellStyle = Data;



                    cellindex = cellindex + 1;
                    gridcell = gridrow.CreateCell(cellindex);
                    gridcell.SetCellValue(order.Createdat.HasValue ? order.Createdat.Value.ToString("yyyy-MM-dd") : "N/A");
                    gridcell.CellStyle = Data;
                    sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, cellindex, cellindex + 2));
                    ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex, cellindex, cellindex + 2), Data);


                    cellindex = cellindex + 3;
                    gridcell = gridrow.CreateCell(cellindex);
                    gridcell.SetCellValue(order.Customer.Customername);
                    gridcell.CellStyle = Data;
                    sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, cellindex, cellindex + 2));
                    ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex, cellindex, cellindex + 2), Data);

                    cellindex = cellindex + 3;
                    gridcell = gridrow.CreateCell(cellindex);
                    gridcell.SetCellValue(order.Status.Statusname.Trim());
                    gridcell.CellStyle = Data;
                    sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, cellindex, cellindex + 2));
                    ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex, cellindex, cellindex + 2), Data);

                    cellindex = cellindex + 3;
                    gridcell = gridrow.CreateCell(cellindex);
                    gridcell.SetCellValue(order.Paymentmethod);
                    gridcell.CellStyle = Data;
                    sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, cellindex, cellindex + 1));
                    ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex, cellindex, cellindex + 1), Data);

                    cellindex = cellindex + 2;
                    gridcell = gridrow.CreateCell(cellindex);
                    gridcell.SetCellValue(order.Rattings);
                    gridcell.CellStyle = Data;
                    sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, cellindex, cellindex + 1));
                    ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex, cellindex, cellindex + 1), Data);

                    cellindex = cellindex + 2;
                    gridcell = gridrow.CreateCell(cellindex);
                    gridcell.SetCellValue((double)order.Totalamount);
                    gridcell.CellStyle = Data;
                    sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, cellindex, cellindex + 1));
                    ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex, cellindex, cellindex + 1), Data);
                }
                sheet.CreateFreezePane(0, 8, 0, 8);
                for (int i = 0; i <= cellheaderindex; i++)
                {
                    sheet.SetColumnWidth(i, 2700);
                }

                HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 14, 0, 20, 20)
                {
                    AnchorType = (int)NPOI.SS.UserModel.AnchorType.MoveAndResize
                };
                //Here, you need to replace the Image Path and Name as per your directory structure and Image Name
                HSSFPicture picture = (HSSFPicture)patriarch.CreatePicture(anchor, LoadImage(@"C:\Users\pct78\pizzashop_N_tier\pizzashop_n_tier\wwwroot\images\pizzashop_logo.png", workbook));
                 picture.Resize(0.34);
                picture.LineStyle = (LineStyle)HSSFPicture.LINESTYLE_NONE;


                string FileName = "MyExcel_" + DateTime.Now.ToString("yyyy-dd-MM--HH-mm-ss") + ".xls";
                using (FileStream file = new FileStream(@"C:\Users\pct78\pizzashop_N_tier\pizzashop_n_tier\wwwroot" + FileName, FileMode.Create))
                {
                    workbook.Write(file);
                    file.Close();
                    Console.WriteLine("File Created Successfully...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static int LoadImage(string path, HSSFWorkbook wb)
        {
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[file.Length];
            file.Read(buffer, 0, (int)file.Length);
            return wb.AddPicture(buffer, PictureType.JPEG);
        }

        public Order? getOrderDetails(int orderid)
        {
            return _orderRepository.GetOrderDetails(orderid);
        }

        public Dictionary<Item, List<Modifier>> getItemsAndModifiers(int orderid)
        {
            return _orderRepository.GetItemsAndModifiersForOrder(orderid);
        }
    }
}