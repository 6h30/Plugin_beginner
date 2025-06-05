3.1. Vị trí phần tử (Element Locations)  

Trong Revit, phần tử được tạo dựa trên điểm (point-based, như nội thất) hoặc đường cong (line-based, như tường). Chúng ta có thể chỉnh sửa vị trí của phần tử bằng cách lấy và đặt lại thuộc tính vị trí (`Location`). Thuộc tính này trả về một đối tượng `Location`, có thể là `LocationPoint` (cho điểm) hoặc `LocationCurve` (cho đường cong). Trong phần này, chúng ta sẽ tạo một lệnh để di chuyển một phần tử điểm cơ sở (như nội thất) theo trục X.

**Tra cứu thuộc tính Location**:  
- Trong **Object Browser**, tìm lớp `Element` trong namespace `Autodesk.Revit.DB`.  
- Thuộc tính `Location` trả về một đối tượng `Location`, cần ép kiểu thành `LocationPoint` hoặc `LocationCurve` bằng từ khóa `as`.  
- `LocationPoint.Point` trả về tọa độ `XYZ` của phần tử.  

**Tạo lệnh ChangeLocation**:  
1. **Tạo lớp lệnh**:  
   - Tạo lớp `ChangeLocation.cs`, triển khai `IExternalCommand`.  
   - Đặt thuộc tính `[Transaction(TransactionMode.Manual)]` để cho phép thay đổi mô hình.  
   - Thêm namespace: `Autodesk.Revit.DB`, `Autodesk.Revit.UI`, `Autodesk.Revit.Attributes`.  

2. **Lấy phần tử và vị trí**:  
   - Sử dụng `PickObject` để chọn phần tử, lấy `Element`:  
     - `Element ele = doc.GetElement(pickObj);`.  
   - Lấy vị trí dưới dạng `LocationPoint`:  
     - `LocationPoint locp = ele.Location as LocationPoint;`.  
   - Kiểm tra `locp != null` để đảm bảo phần tử có `LocationPoint` (tránh lỗi nếu chọn phần tử như tường có `LocationCurve`).  

3. **Di chuyển phần tử**:  
   - Lấy tọa độ hiện tại: `XYZ loc = locp.Point;`.  
   - Tạo tọa độ mới, di chuyển 3 đơn vị theo trục X:  
     - `XYZ newLoc = new XYZ(loc.X + 3, loc.Y, loc.Z);`.  
   - Đặt lại vị trí: `locp.Point = newLoc;`.  

4. **Quản lý Transaction**:  
   - Thực hiện thay đổi trong khối `using (Transaction trans = new Transaction(doc, "Change Location"))`:  
     - Gọi `trans.Start()` trước khi thay đổi.  
     - Gọi `trans.Commit()` sau khi thay đổi, chỉ khi `locp != null`.  

**Mã nguồn mẫu**:  
```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class ChangeLocation : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            try
            {
                Reference pickObj = uiDoc.Selection.PickObject(ObjectType.Element);
                if (pickObj != null)
                {
                    Element ele = doc.GetElement(pickObj);
                    LocationPoint locp = ele.Location as LocationPoint;

                    if (locp != null)
                    {
                        using (Transaction trans = new Transaction(doc, "Change Location"))
                        {
                            trans.Start();
                            XYZ loc = locp.Point;
                            XYZ newLoc = new XYZ(loc.X + 3, loc.Y, loc.Z);
                            locp.Point = newLoc;
                            trans.Commit();
                        }
                    }
                    else
                    {
                        TaskDialog.Show("Error", "Selected element does not have a location point.");
                        return Result.Failed;
                    }
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

**Sửa lỗi Transaction**:  
- Trong video, lỗi xảy ra do có hai `trans.Commit()` hoặc giao dịch không được đặt đúng trong khối `if`.  
- Giải pháp: Đặt khối `using (Transaction trans = ...)` bên trong `if (locp != null)` để chỉ mở giao dịch khi phần tử có `LocationPoint`.  

**Cập nhật tệp manifest (.addin)**:  
- Mở `MyRevitCommands.addin`, sao chép khối `<AddIn>` từ lệnh trước.  
- Cập nhật:  
  - `<AddInId>`: Tạo GUID mới (**Tools > Create GUID**).  
  - `<FullClassName>`: `MyRevitCommands.ChangeLocation`.  
  - `<Name>` và `<Text>`: `ChangeLocation`.  
  - `<Description>`: “Moves a point-based element 3 units along the X-axis”.  

**Kiểm tra lệnh**:  
1. Nhấn **Start** trong Visual Studio để chạy Debug.  
2. Mở Revit, mở tệp dự án từ thư mục bài tập.  
3. Vào **Add-Ins > External Tools > ChangeLocation`, chọn một phần tử điểm cơ sở (như nội thất).  
4. Kiểm tra phần tử đã di chuyển 3 đơn vị theo trục X.  
5. Nếu chọn phần tử không phải điểm cơ sở (như tường), `TaskDialog` sẽ hiển thị lỗi: “Selected element does not have a location point.”  

**Mục tiêu**  
- Hiểu cách lấy và đặt vị trí phần tử điểm cơ sở bằng `LocationPoint`.  
- Sử dụng `Transaction` để chỉnh sửa vị trí phần tử trong mô hình Revit.  
- Kiểm tra loại vị trí (`LocationPoint` hay `LocationCurve`) để tránh lỗi.  

Trong các phần tiếp theo, bạn sẽ học cách chỉnh sửa vị trí của phần tử đường cong (line-based) hoặc các cách khác để thay đổi phần tử trong Revit.