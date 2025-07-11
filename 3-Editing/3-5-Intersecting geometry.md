3.5. Giao cắt hình học (Intersecting Geometry)  

Sau khi học cách trích xuất hình học (geometry) của phần tử, chúng ta sẽ sử dụng hình học để kiểm tra va chạm (clash detection) bằng bộ lọc `ElementIntersectsSolidFilter`. Bộ lọc này là một bộ lọc chậm (slow filter), nhận một đối tượng `Solid` làm tham số, và được dùng để tìm các phần tử giao cắt với khối hình học đó. Trong phần này, chúng ta sẽ tạo một lệnh để chọn một cột (column), lấy khối hình học (`Solid`) của nó, và kiểm tra xem nó có giao cắt với mái (roof) nào không, sau đó chọn các mái giao cắt.

**Tra cứu ElementIntersectsSolidFilter**:  
- Trong **Object Browser**, tìm lớp `ElementIntersectsSolidFilter` trong namespace `Autodesk.Revit.DB`.  
- Constructor: `ElementIntersectsSolidFilter(Solid)` tạo bộ lọc kiểm tra phần tử giao cắt với một `Solid`.  
- **GeometryInstance**: Đối với các phần tử như cột (family instance), hình học nằm trong `GeometryInstance`. Sử dụng `GetInstanceGeometry()` để lấy `GeometryElement`.  

**Tạo lệnh ElementIntersection**:  
1. **Tạo lớp lệnh**:  
   - Tạo lớp `ElementIntersection.cs`, triển khai `IExternalCommand`.  
   - Đặt thuộc tính `[Transaction(TransactionMode.ReadOnly)]` vì chỉ đọc dữ liệu.  
   - Thêm namespace: `Autodesk.Revit.DB`, `Autodesk.Revit.UI`, `Autodesk.Revit.Attributes`, `System.Collections.Generic`.  

2. **Lấy Solid từ cột**:  
   - Sử dụng `PickObject` để chọn cột, lấy `Element`:  
     - `Element ele = doc.GetElement(pickObj);`.  
   - Tạo `Options` với mức chi tiết `Fine`:  
     - `Options gOptions = new Options { DetailLevel = ViewDetailLevel.Fine };`.  
   - Lấy hình học: `GeometryElement geom = ele.get_Geometry(gOptions);`.  
   - Duyệt `geom` để tìm `GeometryInstance`:  
     - `GeometryInstance gInst = gObj as GeometryInstance;`.  
   - Kiểm tra `gInst != null`, lấy `GeometryElement`:  
     - `GeometryElement gle = gInst.GetInstanceGeometry();`.  
   - Duyệt `gle` để tìm `Solid`:  
     - `Solid gSolid = go as Solid;`.  

3. **Tạo bộ lọc giao cắt**:  
   - Tạo `FilteredElementCollector` để tìm các mái:  
     - `FilteredElementCollector collector = new FilteredElementCollector(doc);`.  
   - Áp dụng bộ lọc nhanh `OfCategory(BuiltInCategory.OST_Roofs)` để tăng hiệu suất.  
   - Tạo bộ lọc chậm `ElementIntersectsSolidFilter`:  
     - `ElementIntersectsSolidFilter filter = new ElementIntersectsSolidFilter(gSolid);`.  
   - Áp dụng bộ lọc: `collector.WherePasses(filter);`.  
   - Lấy danh sách ID các phần tử giao cắt:  
     - `ICollection<ElementId> intersects = collector.ToElementIds();`.  

4. **Chọn các phần tử giao cắt**:  
   - Đặt các phần tử giao cắt (mái) vào lựa chọn của người dùng:  
     - `uiDoc.Selection.SetElementIds(intersects);`.  

**Mã nguồn mẫu**:  
```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Collections.Generic;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.ReadOnly)]
    public class ElementIntersection : IExternalCommand
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
                    Options gOptions = new Options { DetailLevel = ViewDetailLevel.Fine };
                    GeometryElement geom = ele.get_Geometry(gOptions);
                    Solid gSolid = null;

                    foreach (GeometryObject gObj in geom)
                    {
                        GeometryInstance gInst = gObj as GeometryInstance;
                        if (gInst != null)
                        {
                            GeometryElement gle = gInst.GetInstanceGeometry();
                            foreach (GeometryObject go in gle)
                            {
                                gSolid = go as Solid;
                                if (gSolid != null) break;
                            }
                            if (gSolid != null) break;
                        }
                    }

                    if (gSolid == null)
                    {
                        TaskDialog.Show("Error", "No solid found in selected element.");
                        return Result.Failed;
                    }

                    // Tạo bộ lọc giao cắt
                    FilteredElementCollector collector = new FilteredElementCollector(doc);
                    ElementIntersectsSolidFilter filter = new ElementIntersectsSolidFilter(gSolid);
                    ICollection<ElementId> intersects = collector
                        .OfCategory(BuiltInCategory.OST_Roofs)
                        .WherePasses(filter)
                        .ToElementIds();

                    // Chọn các phần tử giao cắt
                    uiDoc.Selection.SetElementIds(intersects);
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
  - `<FullClassName>`: `MyRevitCommands.ElementIntersection`.  
  - `<Name>` và `<Text>`: `ElementIntersection`.  
  - `<Description>`: “Selects roofs intersecting with a selected column”.  

**Kiểm tra lệnh**:  
1. Nhấn **Start** trong Visual Studio để chạy Debug.  
2. Mở Revit, mở tệp dự án từ thư mục bài tập (mở ở chế độ 3D View).  
3. Vào **Add-Ins > External Tools > ElementIntersection`, chọn một cột (column).  
4. Kiểm tra xem các mái (roofs) giao cắt với cột có được tự động chọn (highlighted) trong Revit không.  
5. **Lưu ý**: Nếu cột không có hình học `Solid` hoặc không giao cắt với mái, lệnh sẽ hiển thị lỗi hoặc không chọn được phần tử nào.

**Mục tiêu**  
- Hiểu cách sử dụng `ElementIntersectsSolidFilter` để kiểm tra va chạm hình học.  
- Trích xuất `Solid` từ `GeometryInstance` của một phần tử Family (như cột).  
- Kết hợp bộ lọc nhanh (`OfCategory`) và chậm (`ElementIntersectsSolidFilter`) để tối ưu hiệu suất.  

**Lưu ý an toàn**:  
- Kiểm tra `gSolid != null` để tránh lỗi nếu phần tử không có hình học khối.  
- Sử dụng bộ lọc nhanh trước bộ lọc chậm để tăng hiệu suất.  
- Lệnh có thể mở rộng để kiểm tra va chạm với các danh mục khác (như tường, sàn) hoặc hiển thị thông tin chi tiết về các phần tử giao cắt.  

Trong các phần tiếp theo, bạn sẽ học cách sử dụng hình học để thực hiện các tác vụ phức tạp hơn, như phân tích hoặc chỉnh sửa mô hình Revit.