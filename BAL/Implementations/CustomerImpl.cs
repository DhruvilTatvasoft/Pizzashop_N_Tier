using DAL.interfaces;
using BAL.Interfaces;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;
using Microsoft.AspNetCore.Mvc;
using Azure;
using DAL.Data;

namespace BAL.Implementations
{
    public class CustomerImpl : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerImpl(ICustomerRepository customerRepository){
            _customerRepository = customerRepository;
        }
        private void ApplyMergedCellStyle(HSSFSheet sheet, CellRangeAddress range, ICellStyle style)
        {
            for (int rowNum = range.FirstRow; rowNum <= range.LastRow; rowNum++)
            {
                var row = sheet.GetRow(rowNum) ?? sheet.CreateRow(rowNum);
                for (int colNum = range.FirstColumn; colNum <= range.LastColumn; colNum++)
                {
                    var cell = row.GetCell(colNum) ?? row.CreateCell(colNum);
                    cell.CellStyle = style;
                }
            }
        }
        

        public byte[] exportCustomerDetails(int pageSize,int pageNumber,string sortBy,string sortOrder,string search,string filterBy,DateTime? startDate,DateTime? endDate,bool? isExport)
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
                // CompanyFont.FontHeightInPoints = ((short)16);
                Company.SetFont(CompanyFont);

                var Address = workbook.CreateCellStyle();
                Address.Alignment = HorizontalAlignment.Center;
                var AddressFont = workbook.CreateFont();
                AddressFont.FontName = "Arial";
                AddressFont.Boldweight = (short)FontBoldWeight.Bold;
                // AddressFont.FontHeightInPoints = ((short)10);
                Address.SetFont(AddressFont);

                var Address1 = workbook.CreateCellStyle();
                Address1.Alignment = HorizontalAlignment.Center;
                var AddressFont1 = workbook.CreateFont();
                AddressFont1.FontName = "Arial";
                AddressFont1.Boldweight = (short)FontBoldWeight.Bold;
                // AddressFont1.FontHeightInPoints = ((short)10);
                Address1.SetFont(AddressFont);


                var Header = workbook.CreateCellStyle();
                Header.Alignment = HorizontalAlignment.Center;
                Header.VerticalAlignment = VerticalAlignment.Center;
                Header.FillForegroundColor = HSSFColor.Blue.Index;
                Header.FillBackgroundColor = HSSFColor.Blue.Index;
                Header.FillPattern = FillPattern.SolidForeground;
                var HeaderFont = workbook.CreateFont();
                HeaderFont.FontName = "Arial";
                HeaderFont.Boldweight = (short)FontBoldWeight.Bold;
                HeaderFont.Color = HSSFColor.White.Index;
                // HeaderFont.FontHeightInPoints = ((short)10);
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
                // DataFont.FontHeightInPoints = ((short)9);
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
                // linkDataFont.FontHeightInPoints = ((short)9);
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
                cell.SetCellValue("Account");
                cell.CellStyle = Header;
                sheet.AddMergedRegion(new CellRangeAddress(1, 2, 0, 1));
                ApplyMergedCellStyle(sheet, new CellRangeAddress(1, 2, 0, 1), Header);

                cell = row.CreateCell(7);
                cell.SetCellValue("search text");
                cell.CellStyle = Header;
                sheet.AddMergedRegion(new CellRangeAddress(1, 2, 7, 8));
                ApplyMergedCellStyle(sheet, new CellRangeAddress(1, 2, 7, 8), Header);

                cell = row.CreateCell(9);
                cell.SetCellValue(search);
                cell.CellStyle = Data;
                sheet.AddMergedRegion(new CellRangeAddress(1, 2, 9, 12));
                ApplyMergedCellStyle(sheet, new CellRangeAddress(1, 2, 9, 12), Data);

                cell = row.CreateCell(2);
                cell.SetCellValue("");
                cell.CellStyle = Data;
                sheet.AddMergedRegion(new CellRangeAddress(1, 2, 2, 5));
                ApplyMergedCellStyle(sheet, new CellRangeAddress(1, 2, 2, 5), Data);

                rowIndex = 4;
                row = sheet.CreateRow(rowIndex);
                cell = row.CreateCell(0);
                cell.SetCellValue("Date : ");
                cell.CellStyle = Header;
                sheet.AddMergedRegion(new CellRangeAddress(4, 5, 0, 1));
                ApplyMergedCellStyle(sheet, new CellRangeAddress(4, 5, 0, 1), Header);

               
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

               

                CustomerViewModel model = new CustomerViewModel();
                model = _customerRepository.getAllCustomers(pageSize, pageNumber, sortBy, sortOrder, search, filterBy, startDate, endDate,true);

                cell = row.CreateCell(9);
                cell.SetCellValue(model.totalCustomers);
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
                excelheadercell.SetCellValue("Name");
                excelheadercell.CellStyle = Header;
                sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, cellheaderindex, cellheaderindex + 2));
                ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex, cellheaderindex, cellheaderindex + 2), Header);

                cellheaderindex = cellheaderindex + 3;
                excelheadercell = excelheaderrow.CreateCell(cellheaderindex);
                excelheadercell.SetCellValue("Email");
                excelheadercell.CellStyle = Header;
                sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, cellheaderindex, cellheaderindex + 2));
                ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex, cellheaderindex, cellheaderindex + 2), Header);


                cellheaderindex = cellheaderindex + 3;
                excelheadercell = excelheaderrow.CreateCell(cellheaderindex);
                excelheadercell.SetCellValue("Date");
                excelheadercell.CellStyle = Header;
                sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, cellheaderindex, cellheaderindex + 2));
                ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex, cellheaderindex, cellheaderindex + 2), Header);

                cellheaderindex = cellheaderindex + 3;
                excelheadercell = excelheaderrow.CreateCell(cellheaderindex);
                excelheadercell.SetCellValue("Mobile Number");
                excelheadercell.CellStyle = Header;
                sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, cellheaderindex, cellheaderindex + 1));
                ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex, cellheaderindex, cellheaderindex + 1), Header);

                cellheaderindex = cellheaderindex + 2;
                excelheadercell = excelheaderrow.CreateCell(cellheaderindex);
                excelheadercell.SetCellValue("Total orders");
                excelheadercell.CellStyle = Header;
                sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, cellheaderindex, cellheaderindex + 1));
                ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex, cellheaderindex, cellheaderindex + 1), Header);

               

                foreach (var customer in model.customers)
                {
                    rowIndex = rowIndex + 1;
                    SR_NO = SR_NO + 1;
                    var cellindex = 0;
                    var gridrow = sheet.CreateRow(rowIndex);
                    var gridcell = gridrow.CreateCell(cellindex);

                    gridcell.SetCellValue(customer.Customerid);

                    gridcell.CellStyle = Data;



                    cellindex = cellindex + 1;
                    gridcell = gridrow.CreateCell(cellindex);
                   gridcell.SetCellValue(customer.Customername);
                    gridcell.CellStyle = Data;
                    sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, cellindex, cellindex + 2));
                    ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex, cellindex, cellindex + 2), Data);


                    cellindex = cellindex + 3;
                    gridcell = gridrow.CreateCell(cellindex);
                    gridcell.SetCellValue(customer.Email);
                    
                    gridcell.CellStyle = Data;
                    sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, cellindex, cellindex + 2));
                    ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex, cellindex, cellindex + 2), Data);

                    cellindex = cellindex + 3;
                    gridcell = gridrow.CreateCell(cellindex);
                    gridcell.SetCellValue(customer.Createdat.HasValue ? customer.Createdat.Value.ToString("yyyy-MM-dd") : "N/A");
                    gridcell.CellStyle = Data;
                    sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, cellindex, cellindex + 2));
                    ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex, cellindex, cellindex + 2), Data);

                    cellindex = cellindex + 3;
                    gridcell = gridrow.CreateCell(cellindex);
                    gridcell.SetCellValue(customer.Phonenumber);
                    gridcell.CellStyle = Data;
                    sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, cellindex, cellindex + 1));
                    ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex, cellindex, cellindex + 1), Data);

                    cellindex = cellindex + 2;
                    gridcell = gridrow.CreateCell(cellindex);
                    gridcell.SetCellValue(customer.Totalorders ?? 0);
                    gridcell.CellStyle = Data;
                    sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, cellindex, cellindex + 1));
                    ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex, cellindex, cellindex + 1), Data);

                    // cellindex = cellindex + 2;
                    // gridcell = gridrow.CreateCell(cellindex);
                    // gridcell.SetCellValue((double)order.Totalamount);
                    // gridcell.CellStyle = Data;
                    // sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, cellindex, cellindex + 1));
                    // ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex, cellindex, cellindex + 1), Data);
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
                
                using (var memoryStream = new MemoryStream()){
                    workbook.Write(memoryStream);
                    // var result = new FileContentResult(memoryStream.ToArray(), "application/vnd.ms-excel");
                     memoryStream.Position = 0;
                    return memoryStream.ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new MemoryStream().ToArray();
            }
        }

        public CustomerViewModel getAllCustomers(int pageSize, int pageNumber, string sortBy, string sortOrder, string? search, string filterBy, DateTime? startDate, DateTime? endDate)
        {
           return _customerRepository.getAllCustomers(pageSize,pageNumber,sortBy,sortOrder,search,filterBy,startDate,endDate,false);
      
        }

        public CustomerViewModel getCustomerHistory(int customerid)
        {
            return _customerRepository.getCustomerHistory(customerid);
        }
         public static int LoadImage(string path, HSSFWorkbook wb)
        {
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[file.Length];
            file.Read(buffer, 0, (int)file.Length);
            return wb.AddPicture(buffer, PictureType.JPEG);
        }
    }
}