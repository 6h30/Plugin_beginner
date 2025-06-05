3.4. Duyệt hình học (Traversing Geometry)  
Sau khi học cách lấy hình học (geometry) của một phần tử, chúng ta sẽ tạo một lệnh để trích xuất các mặt (faces) của một tường (wall) và tính tổng diện tích của chúng. Lệnh này sẽ sử dụng đối tượng `Options` để cấu hình chi tiết hình học và duyệt qua các đối tượng hình học để lấy thông tin.

**Tra cứu Geometry**:  
- Trong **Object Browser**, tìm lớp `Element` trong namespace `Autodesk.Revit.DB`.  
- Phương thức `get_Geometry(Options)` trả về một `GeometryElement`, chứa các đối tượng hình học của phần tử.  
- Tường thường có hình học dạng `Solid`, chứa danh sách các mặt (`Faces`) qua thuộc tính `Faces` (kiểu `FaceArray`).  
- Mỗi `Face` có thuộc tính `Area` để lấy diện tích (đơn vị nội bộ là feet vuông).  

**Tạo lệnh SelectGeometry**:  
1. **Tạo lớp lệnh**:  
   - Tạo lớp `SelectGeometry.cs`, triển khai `IExternalCommand`.  
   - Đặt thuộc tính `[Transaction(TransactionMode.ReadOnly)]` vì chỉ đọc dữ liệu.  
   - Thêm namespace: `Autodesk.Revit.DB`, `Autodesk.Revit.UI`, `Autodesk.Revit.Attributes`, `System`.  

2. **Tạo đối tượng Options**:  
   - Tạo `Options` để cấu hình chi tiết hình học:  
     - `Options gOptions = new Options();`.  
   - Đặt mức chi tiết (`DetailLevel`) là `Fine` để lấy hình học chi tiết nhất:  
     - `gOptions.DetailLevel = ViewDetailLevel.Fine;`.  

3. **Lấy GeometryElement**:  
   - Sử dụng `PickObject` để chọn phần tử, lấy `Element`:  
     - `Element ele = doc.GetElement(pickObj);`.  
   - Lấy hình học: `GeometryElement geom = ele.get_Geometry(gOptions);`.  

4. **Duyệt Geometry và lấy Faces**:  
   - Duyệt các đối tượng hình học trong `geom` bằng `foreach`:  
     - `foreach (GeometryObject gObj in geom)`.  
   - Ép kiểu `gObj` thành `Solid` (giả định chọn tường):  
     - `Solid gSolid = gObj as Solid;`.  
   - Khởi tạo biến đếm số mặt (`faces = 0`) và tổng diện tích (`area = 0.0`).  
   - Duyệt các mặt trong `gSolid.Faces`:  
     - `foreach (Face gface in gSolid.Faces)`.  
     - Cộng diện tích: `area += gface.Area;`.  
     - Tăng số mặt: `faces++;`.  

5. **Chuyển đổi đơn vị**:  
   - Diện tích mặc định là feet vuông (đơn vị nội bộ của Revit). Chuyển sang mét vuông:  
     - `area = UnitUtils.ConvertFromInternalUnits(area, DisplayUnitType.DUT_SQUARE_METERS);`.  

6. **Hiển thị kết quả**:  
   - Sử dụng `TaskDialog` để hiển thị số mặt và diện tích:  
     - `TaskDialog.Show("Geometry", string.Format("Number of Faces: {0}{1}Total Area: {2} square meters", faces, Environment.NewLine, area));`.  

**Mã nguồn mẫu**:  
```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.ReadOnly)]
    public class SelectGeometry : IExternalCommand
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
                    Options gOptions = new Options();
                    gOptions.DetailLevel = ViewDetailLevel.Fine;
                    GeometryElement geom = ele.get_Geometry(gOptions);

                    int faces = 0;
                    double area = 0.0;

                    foreach (GeometryObject gObj in geom)
                    {
                        Solid gSolid = gObj as Solid;
                        if (gSolid != null)
                        {
                            foreach (Face gface in gSolid.Faces)
                            {
                                area += gface.Area;
                                faces++;
                            }
                        }
                    }

                    // Chuyển đổi diện tích sang mét vuông
                    area = UnitUtils.ConvertFromInternalUnits(area, DisplayUnitType.DUT_SQUARE_METERS);

                    // Hiển thị kết quả
                    TaskDialog.Show("Geometry",
                        string.Format("Number of Faces: {0}{1}Total Area: {2} square meters",
                        faces, Environment.NewLine, area));
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
  - `<FullClassName>`: `MyRevitCommands.SelectGeometry`.  
  - `<Name>` và `<Text>`: `SelectGeometry`.  
  - `<Description>`: “Calculates the number of faces and total area of a selected wall”.  

**Kiểm tra lệnh**:  
1. Nhấn **Start** trong Visual Studio để chạy Debug.  
2. Mở Revit, mở tệp dự án từ thư mục bài tập.  
3. Vào **Add-Ins > External Tools > SelectGeometry`, chọn một tường (wall).  
4. Kiểm tra `TaskDialog` hiển thị:  
   - **Number of Faces**: Số mặt (thường là 6 cho một tường hình hộp).  
   - **Total Area**: Diện tích tổng (mét vuông, ví dụ: 51.9 m²).  
5. **Lưu ý**: Lệnh giả định phần tử là tường (có `Solid`). Để an toàn, kiểm tra `gSolid != null` trước khi duyệt mặt, như trong mã trên.

**Mục tiêu**  
- Hiểu cách lấy hình học phần tử bằng `get_Geometry(Options)`.  
- Duyệt các đối tượng hình học (`GeometryObject`) và trích xuất mặt (`Face`) từ `Solid`.  
- Tính tổng diện tích mặt và chuyển đổi đơn vị từ feet vuông sang mét vuông.  

**Lưu ý an toàn**:  
- Kiểm tra `gSolid != null` để tránh lỗi nếu phần tử không có hình học khối (Solid).  
- Có thể mở rộng lệnh để xử lý các loại phần tử khác (như sàn, cột) bằng cách kiểm tra thêm các kiểu `GeometryObject` (Solid, Curve, Point, v.v.).  

Trong các phần tiếp theo, bạn sẽ học cách làm việc sâu hơn với hình học hoặc sử dụng thông tin hình học để thực hiện các thay đổi trong mô hình Revit.