3.2. Phương thức chỉnh sửa phần tử (Editing Methods) 

Revit API cung cấp nhiều cách để chỉnh sửa phần tử, một trong những cách chính là sử dụng lớp `ElementTransformUtils`. Phần này hướng dẫn cách tạo một lệnh để di chuyển (move) và xoay (rotate) một phần tử được chọn trong Revit.

**Tra cứu ElementTransformUtils**:  
- Trong **Object Browser**, tìm lớp `ElementTransformUtils` trong namespace `Autodesk.Revit.DB`.  
- Các phương thức tĩnh:  
  - `MoveElement(Document, ElementId, XYZ)`: Di chuyển phần tử theo vector dịch chuyển (`XYZ`).  
  - `RotateElement(Document, ElementId, Line, double)`: Xoay phần tử quanh một trục (`Line`) với góc xoay (radian).  
  - `CanMirrorElement(Document, ElementId)`: Kiểm tra xem phần tử có thể được phản chiếu (mirror) hay không.  

**Tạo lệnh EditElement**:  
1. **Tạo lớp lệnh**:  
   - Trong Visual Studio, tệp bài tập đã có sẵn lớp `EditElement.cs` triển khai `IExternalCommand`.  
   - Lệnh này cho phép người dùng chọn một phần tử, lấy `ElementId` và bắt đầu một giao dịch (`Transaction`) tên “Edit Elements”.  
   - Thêm namespace: `Autodesk.Revit.DB`, `Autodesk.Revit.UI`, `System`.  

2. **Di chuyển phần tử**:  
   - Tạo vector dịch chuyển: `XYZ moveVec = new XYZ(3, 3, 0);` (di chuyển 3 đơn vị theo X, 3 đơn vị theo Y, 0 theo Z).  
   - Gọi phương thức di chuyển: `ElementTransformUtils.MoveElement(doc, elementId, moveVec);`.  

3. **Xoay phần tử**:  
   - Lấy vị trí phần tử: `LocationPoint location = element.Location as LocationPoint; XYZ p1 = location.Point;`.  
   - Tạo điểm thứ hai trên trục Z: `XYZ p2 = new XYZ(p1.X, p1.Y, p1.Z + 10);`.  
   - Tạo trục xoay: `Line axis = Line.CreateBound(p1, p2);`.  
   - Tính góc xoay (30 độ sang radian): `double angle = 30 * Math.PI / 180;`.  
   - Gọi phương thức xoay: `ElementTransformUtils.RotateElement(doc, elementId, axis, angle);`.  

**Mã nguồn mẫu**:  
```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class EditElement : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            try
            {
                // Chọn phần tử
                Reference reference = uiDoc.Selection.PickObject(ObjectType.Element, "Select an element");
                Element element = doc.GetElement(reference);
                ElementId elementId = element.Id;

                // Bắt đầu giao dịch
                using (Transaction trans = new Transaction(doc, "Edit Elements"))
                {
                    trans.Start();

                    // Di chuyển phần tử
                    XYZ moveVec = new XYZ(3, 3, 0);
                    ElementTransformUtils.MoveElement(doc, elementId, moveVec);

                    // Xoay phần tử
                    LocationPoint location = element.Location as LocationPoint;
                    if (location != null)
                    {
                        XYZ p1 = location.Point;
                        XYZ p2 = new XYZ(p1.X, p1.Y, p1.Z + 10);
                        Line axis = Line.CreateBound(p1, p2);
                        double angle = 30 * Math.PI / 180;
                        ElementTransformUtils.RotateElement(doc, elementId, axis, angle);
                    }

                    trans.Commit();
                }
                return Result.Succeeded;
            }
            catch (Exception e)
            {
                message = e.Message;
                return Result.Failed;
            }
        }
    }
}
```

**Cập nhật tệp manifest (.addin)**:  
- Đảm bảo lệnh đã được thêm vào tệp `.addin`:  
```xml
<AddIn Type="Command">
  <Name>EditElement</Name>
  <Assembly>path\to\MyRevitCommands.dll</Assembly>
  <AddInId>YOUR_GUID_HERE</AddInId>
  <FullClassName>MyRevitCommands.EditElement</FullClassName>
  <VendorId>YOUR_VENDOR_ID</VendorId>
</AddIn>
```
- Thay `YOUR_GUID_HERE` bằng GUID mới (**Tools > Create GUID**).  

**Kiểm tra**:  
1. Biên dịch dự án trong Visual Studio.  
2. Mở Revit, mở tệp dự án từ thư mục bài tập (có phần tử nội thất).  
3. Vào **Add-Ins > External Tools > EditElement**, chọn một phần tử nội thất.  
4. Kiểm tra:  
   - Phần tử di chuyển 3 đơn vị theo X và Y.  
   - Phần tử xoay 30 độ quanh trục Z qua tâm của nó.  
5. **Lưu ý**:  
   - Phần tử phải có `LocationPoint` (như nội thất), nếu không xoay sẽ không thực hiện (`location != null` ngăn lỗi).  
   - Kiểm tra khả năng chỉnh sửa trước khi thực hiện, ví dụ: sử dụng `ElementTransformUtils.CanMirrorElement` cho các thao tác khác.  

**Mục tiêu**:  
- Sử dụng `ElementTransformUtils` để di chuyển và xoay phần tử.  
- Tạo vector dịch chuyển (`XYZ`) và trục xoay (`Line`).  
- Chuyển đổi độ sang radian để xoay phần tử.  

**Ứng dụng**:  
- Tự động hóa chỉnh sửa vị trí và hướng phần tử trong các dự án lớn.  
- Kiểm tra điều kiện trước khi chỉnh sửa để tránh lỗi (ví dụ: `CanMirrorElement`).  
- Mở rộng để thực hiện các thao tác khác như sao chép (`CopyElement`) hoặc phản chiếu (`MirrorElement`).  

Trong các phần tiếp theo, bạn có thể học cách xử lý nhiều phần tử hoặc tích hợp thêm các thao tác chỉnh sửa phức tạp hơn trong Revit API.