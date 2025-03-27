// int rowIndex = 1;
//         var row = sheet.CreateRow(rowIndex);
//         var cell = row.CreateCell(0);
//         cell.SetCellValue("Status:");
//         sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex + 1, 0, 1));
//         ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex + 1, 0, 1), FieldNameBeforeTable);

//         cell = row.CreateCell(2);
//         cell.SetCellValue(statusName);
//         cell.CellStyle = FieldValueBeforeTable;
//         sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex + 1, 2, 5));
//         ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex + 1, 2, 5), FieldValueBeforeTable);

//         cell = row.CreateCell(7);
//         cell.SetCellValue("Search Text:");
//         cell.CellStyle = FieldNameBeforeTable;
//         sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex + 1, 7, 8));
//         ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex + 1, 7, 8), FieldNameBeforeTable);

//         cell = row.CreateCell(9);
//         cell.SetCellValue(searchTerm);
//         cell.CellStyle = FieldValueBeforeTable;
//         sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex + 1, 9, 12));
//         ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex + 1, 9, 12), FieldValueBeforeTable);

//         rowIndex = 4;
//         row = sheet.CreateRow(rowIndex);
//         cell = row.CreateCell(0);
//         cell.SetCellValue("Date:");
//         sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex + 1, 0, 1));
//         ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex + 1, 0, 1), FieldNameBeforeTable);

//         cell = row.CreateCell(2);
//         cell.SetCellValue(time);
//         cell.CellStyle = FieldValueBeforeTable;
//         sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex + 1, 2, 5));
//         ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex + 1, 2, 5), FieldValueBeforeTable);

//         cell = row.CreateCell(7);
//         cell.SetCellValue("No. Of Records:");
//         cell.CellStyle = FieldNameBeforeTable;
//         sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex + 1, 7, 8));
//         ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex + 1, 7, 8), FieldNameBeforeTable);

//         cell = row.CreateCell(9);
//         cell.SetCellValue(totalOrders);
//         cell.CellStyle = FieldValueBeforeTable;
//         sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex + 1, 9, 12));
//         ApplyMergedCellStyle(sheet, new CellRangeAddress(rowIndex, rowIndex + 1, 9, 12), FieldValueBeforeTable);
