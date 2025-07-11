**4.4. Sheets (Tạo ViewSheet)**  

Sau khi chú thích khung nhìn, bước tiếp theo là đặt chúng lên các tờ (sheets). Trong Revit API, tờ là một loại khung nhìn (`ViewSheet`), kế thừa từ lớp `View`. Chúng ta có thể tạo tờ bằng phương thức `ViewSheet.Create`. Trong phần này, chúng ta sẽ tạo một lệnh để tự động tạo một tờ mới, sử dụng `TitleBlock` mặc định và đặt tên cũng như số hiệu cho tờ.

**Tra cứu ViewSheet**:  
- Trong **Object Browser**, tìm lớp `ViewSheet` trong namespace `Autodesk.Revit.DB`.  
- Phương thức tĩnh: `ViewSheet.Create(Document, ElementId titleBlockTypeId)`:  
  - `Document`: Tài liệu Revit.  
  - `titleBlockTypeId`: ID của `FamilySymbol` cho `TitleBlock`. Có thể dùng `ElementId.InvalidElementId` để tạo tờ không có `TitleBlock`.  
- Thuộc tính:  
  - `Name`: Tên tờ (ví dụ: “My First Sheet”).  
  - `SheetNumber`: Số hiệu tờ (ví dụ: “J101”).  

**Tạo lệnh Sheet**:  
1. **Tạo lớp lệnh**:  
   - Tạo lớp `Sheet.cs`, triển khai `IExternalCommand`.  
   - Đặt thuộc tính `[Transaction(TransactionMode.Manual)]` để cho phép thay đổi mô hình.  
   - Thêm namespace: `Autodesk.Revit.DB`, `Autodesk.Revit.UI`, `Autodesk.Revit.Attributes`, `System.Linq`.  

2. **Lấy TitleBlock**:  
   - Tạo `FilteredElementCollector` để tìm `FamilySymbol` của `TitleBlock`:  
     - `FamilySymbol tBlock = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_TitleBlocks).WhereElementIsElementType().Cast<FamilySymbol>().First();`.  
   - **Lưu ý**: Giả định dự án chỉ có một `TitleBlock`. Nếu có nhiều, có thể lọc thêm theo tên bằng LINQ.  

3. **Tạo ViewSheet**:  
   - Trong khối `using (Transaction trans = new Transaction(doc, "Create Sheet"))`:  
     - Gọi `trans.Start()`.  
     - Tạo tờ: `ViewSheet vSheet = ViewSheet.Create(doc, tBlock.Id);`.  
     - Đặt tên: `vSheet.Name = "My First Sheet";`.  
     - Đặt số hiệu: `vSheet.SheetNumber = "J101";`.  
     - Gọi `trans.Commit()`.  

**Mã nguồn mẫu**:  
```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Linq;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class Sheet : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            try
            {
                // Lấy TitleBlock
                FamilySymbol tBlock = new FilteredElementCollector(doc)
                    .OfCategory(BuiltInCategory.OST_TitleBlocks)
                    .WhereElementIsElementType()
                    .Cast<FamilySymbol>()
                    .First();

                // Tạo ViewSheet
                using (Transaction trans = new Transaction(doc, "Create Sheet"))
                {
                    trans.Start();
                    ViewSheet vSheet = ViewSheet.Create(doc, tBlock.Id);
                    vSheet.Name = "My First Sheet";
                    vSheet.SheetNumber = "J101";
                    trans.Commit();
                }
            }
            catch (Exception e)
            {
                message = e.Message;
                return Result.Failed;
            }
            return Result.Succeeded;
        }
    }
}
```

**Cập nhật tệp manifest (.addin)**:  
- Mở `MyRevitCommands.addin`, sao chép khối `<AddIn>` từ lệnh trước.  
- Cập nhật:  
  - `<AddInId>`: Tạo GUID mới (**Tools > Create GUID**).  
  - `<FullClassName>`: `MyRevitCommands.Sheet`.  
  - `<Name>` và `<Text>`: `Sheet`.  
  - `<Description>`: “Creates a new sheet with a title block and sets its name and number”.  

**Kiểm tra lệnh**:  
1. Nhấn **Start** trong Visual Studio để chạy Debug.  
2. Mở Revit, mở tệp dự án từ thư mục bài tập (đảm bảo có ít nhất một `TitleBlock`).  
3. Vào **Add-Ins > External Tools > Sheet`, chạy lệnh.  
4. Kiểm tra trong **Project Browser**, mục **Sheets**:  
   - Tờ mới “My First Sheet” với số hiệu “J101” được tạo, sử dụng `TitleBlock` mặc định.  
5. **Lưu ý**:  
   - Nếu không có `TitleBlock` trong dự án, lệnh sẽ gây lỗi. Có thể kiểm tra `tBlock != null` để xử lý.  
   - Để tạo tờ không có `TitleBlock`, dùng `ElementId.InvalidElementId` thay cho `tBlock.Id`.  

**Mục tiêu**  
- Hiểu cách tạo tờ (`ViewSheet`) bằng `ViewSheet.Create`.  
- Lấy `FamilySymbol` của `TitleBlock` bằng `FilteredElementCollector`.  
- Đặt tên và số hiệu cho tờ bằng thuộc tính `Name` và `SheetNumber`.  

**Ứng dụng**: Tự động hóa tạo tờ trong các dự án lớn, tiết kiệm thời gian thiết lập tài liệu. Trong phần tiếp theo, bạn sẽ học cách đặt khung nhìn (views) lên tờ bằng cách tạo các `Viewport`.