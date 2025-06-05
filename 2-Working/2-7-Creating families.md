2.7. Tạo Family Instance  

Sau khi học cách thay đổi mô hình Revit, chúng ta sẽ tạo một lệnh để đặt một **Family Instance** (điểm cơ sở) từ đầu, cụ thể là một bàn nội thất (desk) có kích thước 1,525 x 762 mm. Lệnh này sẽ lọc tất cả **FamilySymbol** trong dự án, tìm loại bàn mong muốn và đặt nó tại tọa độ xác định.

**Tra cứu phương thức NewFamilyInstance**:  
- Mở **Object Browser** trong Visual Studio, tìm `NewFamilyInstance` trong lớp `Document` (thuộc namespace `Autodesk.Revit.Creation`).  
- Đây là phương thức đa dạng (overloaded), có nhiều phiên bản. Để đặt Family điểm cơ sở (point-based), chọn phiên bản nhận tham số:  
  - `XYZ`: Tọa độ điểm đặt.  
  - `FamilySymbol`: Loại Family (Family Type).  
  - `StructuralType`: Loại cấu trúc (ví dụ: `NonStructural` cho nội thất).  
- Phương thức này thuộc đối tượng `Creation.Document`, truy cập qua `Document.Create`.

**Tạo lệnh PlaceFamily**:  
1. **Tạo lớp lệnh**:  
   - Tạo lớp `PlaceFamily.cs`, triển khai `IExternalCommand`.  
   - Đặt thuộc tính `[Transaction(TransactionMode.Manual)]` để cho phép thay đổi mô hình.  
   - Thêm namespace: `Autodesk.Revit.DB`, `Autodesk.Revit.UI`, `Autodesk.Revit.Attributes`, `System.Collections.Generic`.  

2. **Lọc FamilySymbol**:  
   - Tạo `FilteredElementCollector` để tìm tất cả **FamilySymbol**:  
     - `FilteredElementCollector collector = new FilteredElementCollector(doc);`.  
     - Áp dụng bộ lọc `OfClass(typeof(FamilySymbol))` và `WhereElementIsElementType()` để chỉ lấy các loại Family (không lấy instance).  
     - Lấy danh sách: `IList<Element> symbols = collector.ToElements();`.  
   - Duyệt danh sách để tìm **FamilySymbol** có tên “1,525 by 762 millimeters”:  
     - Sử dụng vòng lặp `foreach` và kiểm tra `ele.Name`.  
     - Ép kiểu sang `FamilySymbol` bằng từ khóa `as` và thoát vòng lặp khi tìm thấy.  

3. **Kích hoạt FamilySymbol**:  
   - Kiểm tra xem **FamilySymbol** có đang hoạt động không: `if (!symbol.IsActive)`.  
   - Nếu không, gọi `symbol.Activate()` để kích hoạt (Revit có thể tắt để tiết kiệm bộ nhớ).  

4. **Đặt Family Instance**:  
   - Trong khối `using (Transaction trans = new Transaction(doc, "Place Family"))`:  
     - Gọi `trans.Start()`.  
     - Sử dụng `doc.Create.NewFamilyInstance(new XYZ(0, 0, 0), symbol, StructuralType.NonStructural)` để đặt bàn tại tọa độ (0, 0, 0).  
     - Gọi `trans.Commit()` để lưu thay đổi.  

**Mã nguồn mẫu**:  
```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Collections.Generic;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class PlaceFamily : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            try
            {
                // Lọc FamilySymbol
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                IList<Element> symbols = collector.OfClass(typeof(FamilySymbol))
                                                  .WhereElementIsElementType()
                                                  .ToElements();
                FamilySymbol symbol = null;
                foreach (Element ele in symbols)
                {
                    if (ele.Name == "1,525 by 762 millimeters")
                    {
                        symbol = ele as FamilySymbol;
                        break;
                    }
                }

                if (symbol == null)
                {
                    TaskDialog.Show("Error", "Family Symbol not found.");
                    return Result.Failed;
                }

                // Kích hoạt FamilySymbol
                if (!symbol.IsActive)
                {
                    symbol.Activate();
                }

                // Đặt Family Instance
                using (Transaction trans = new Transaction(doc, "Place Family"))
                {
                    trans.Start();
                    doc.Create.NewFamilyInstance(new XYZ(0, 0, 0), symbol, StructuralType.NonStructural);
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
  - `<FullClassName>`: `MyRevitCommands.PlaceFamily`.  
  - `<Name>` và `<Text>`: `PlaceFamily`.  
  - `<Description>`: “Places a desk family at (0, 0, 0)”.  

**Kiểm tra lệnh**:  
1. Nhấn **Start** trong Visual Studio để chạy Debug.  
2. Mở Revit, mở tệp dự án từ thư mục bài tập.  
3. Vào **Add-Ins > External Tools > PlaceFamily**, chạy lệnh.  
4. Kiểm tra xem bàn (desk) có xuất hiện tại tọa độ (0, 0, 0) trong mô hình không.  

**Mục tiêu**  
- Hiểu cách sử dụng `NewFamilyInstance` để tạo **Family Instance** điểm cơ sở.  
- Lọc và kích hoạt **FamilySymbol** bằng `FilteredElementCollector`.  
- Sử dụng **Transaction** để đặt Family trong mô hình Revit.  

Lệnh này có thể được mở rộng để đặt nhiều **Family Instance** (như mảng cột) hoặc cho phép người dùng chọn vị trí. Trong các phần tiếp theo, bạn sẽ học cách làm việc với các loại Family phức tạp hơn.