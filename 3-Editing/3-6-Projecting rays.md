3.6. Chiếu tia (Projecting Rays)  

Bộ lọc giao cắt (`ElementIntersectsSolidFilter`) hữu ích để kiểm tra va chạm giữa các phần tử tiếp xúc, nhưng để kiểm tra mối quan hệ giữa các phần tử không chạm nhau, chúng ta cần chiếu tia (project rays). Chiếu tia là việc gửi một vector từ một điểm xuất phát để xem nó giao cắt với phần tử nào trong không gian 3D. Trong phần này, chúng ta sẽ sử dụng lớp `ReferenceIntersector` để chiếu tia từ chân cột (column) lên trên và tính khoảng cách đến mái (roof).

**Tra cứu ReferenceIntersector**:  
- Trong **Object Browser**, tìm lớp `ReferenceIntersector` trong namespace `Autodesk.Revit.DB`.  
- Constructor: `ReferenceIntersector(ElementFilter, FindReferenceTarget, View3D)`:  
  - `ElementFilter`: Bộ lọc để giới hạn phần tử giao cắt (ví dụ: `ElementCategoryFilter` cho mái).  
  - `FindReferenceTarget`: Loại hình học mục tiêu (ví dụ: `Face` cho mặt dưới của mái).  
  - `View3D`: Khung nhìn 3D để thực hiện chiếu tia (các phần tử ẩn trong khung nhìn sẽ không được tính).  
- Phương thức:  
  - `FindNearest(XYZ origin, XYZ direction)`: Tìm phần tử gần nhất mà tia giao cắt, trả về `ReferenceWithContext`.  
  - `Find(XYZ origin, XYZ direction)`: Tìm tất cả giao cắt trên đường đi của tia.  

**Tạo lệnh ProjectRay**:  
1. **Tạo lớp lệnh**:  
   - Tạo lớp `ProjectRay.cs`, triển khai `IExternalCommand`.  
   - Đặt thuộc tính `[Transaction(TransactionMode.ReadOnly)]` vì chỉ đọc dữ liệu.  
   - Thêm namespace: `Autodesk.Revit.DB`, `Autodesk.Revit.UI`, `Autodesk.Revit.Attributes`.  

2. **Lấy điểm xuất phát**:  
   - Sử dụng `PickObject` để chọn cột, lấy `Element`:  
     - `Element ele = doc.GetElement(pickObj);`.  
   - Lấy `LocationPoint` và tọa độ `XYZ`:  
     - `LocationPoint locp = ele.Location as LocationPoint;`.  
     - `XYZ p1 = locp.Point;`.  
   - **Lưu ý**: Lệnh giả định chọn phần tử điểm cơ sở (như cột). Nếu chọn phần tử khác (như tường), cần kiểm tra `locp != null`.  

3. **Tạo vector hướng tia**:  
   - Chiếu tia thẳng đứng lên trên (trục Z):  
     - `XYZ rayd = new XYZ(0, 0, 1);`.  

4. **Tạo ReferenceIntersector**:  
   - Tạo bộ lọc danh mục cho mái:  
     - `ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Roofs);`.  
   - Lấy khung nhìn 3D hiện tại:  
     - `View3D view3D = doc.ActiveView as View3D;`.  
   - Tạo `ReferenceIntersector`:  
     - `ReferenceIntersector refI = new ReferenceIntersector(filter, FindReferenceTarget.Face, view3D);`.  

5. **Chiếu tia và lấy giao cắt**:  
   - Sử dụng `FindNearest` để tìm phần tử gần nhất:  
     - `ReferenceWithContext refC = refI.FindNearest(p1, rayd);`.  
   - Lấy tham chiếu và điểm giao cắt:  
     - `Reference reference = refC.GetReference();`.  
     - `XYZ intPoint = reference.GlobalPoint;`.  

6. **Tính khoảng cách**:  
   - Tính khoảng cách từ điểm xuất phát (`p1`) đến điểm giao cắt (`intPoint`):  
     - `double dist = p1.DistanceTo(intPoint);`.  

7. **Hiển thị kết quả**:  
   - Hiển thị khoảng cách bằng `TaskDialog`:  
     - `TaskDialog.Show("Ray", string.Format("Distance to Roof: {0}", dist));`.  
   - **Lưu ý**: Khoảng cách hiển thị theo feet (đơn vị nội bộ của Revit). Để chuyển sang mét, sử dụng `UnitUtils.ConvertFromInternalUnits(dist, DisplayUnitType.DUT_METERS)`.  

**Mã nguồn mẫu**:  
```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.ReadOnly)]
    public class ProjectRay : IExternalCommand
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
                    if (locp == null)
                    {
                        TaskDialog.Show("Error", "Selected element is not point-based.");
                        return Result.Failed;
                    }

                    XYZ p1 = locp.Point;
                    XYZ rayd = new XYZ(0, 0, 1); // Hướng tia lên trên (trục Z)

                    // Tạo bộ lọc và ReferenceIntersector
                    ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Roofs);
                    View3D view3D = doc.ActiveView as View3D;
                    if (view3D == null)
                    {
                        TaskDialog.Show("Error", "Active view is not a 3D view.");
                        return Result.Failed;
                    }
                    ReferenceIntersector refI = new ReferenceIntersector(filter, FindReferenceTarget.Face, view3D);

                    // Chiếu tia và lấy giao cắt
                    ReferenceWithContext refC = refI.FindNearest(p1, rayd);
                    if (refC == null)
                    {
                        TaskDialog.Show("Error", "No intersection found.");
                        return Result.Failed;
                    }

                    Reference reference = refC.GetReference();
                    XYZ intPoint = reference.GlobalPoint;

                    // Tính khoảng cách
                    double dist = p1.DistanceTo(intPoint);

                    // Hiển thị kết quả
                    TaskDialog.Show("Ray", string.Format("Distance to Roof: {0} feet", dist));
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
  - `<FullClassName>`: `MyRevitCommands.ProjectRay`.  
  - `<Name>` và `<Text>`: `ProjectRay`.  
  - `<Description>`: “Projects a ray from a column base to find the distance to a roof”.  

**Kiểm tra lệnh**:  
1. Nhấn **Start** trong Visual Studio để chạy Debug.  
2. Mở Revit, mở tệp dự án từ thư mục bài tập (đảm bảo ở chế độ 3D View).  
3. Vào **Add-Ins > External Tools > ProjectRay`, chọn một cột (column).  
4. Kiểm tra `TaskDialog` hiển thị khoảng cách từ chân cột đến mái (ví dụ: “Distance to Roof: 15.7 feet”).  
5. **Lưu ý**:  
   - Nếu khung nhìn hiện tại không phải 3D View, lệnh sẽ báo lỗi.  
   - Nếu không tìm thấy giao cắt (mái), lệnh sẽ hiển thị lỗi “No intersection found.”  
   - Để chuyển khoảng cách sang mét, thêm: `dist = UnitUtils.ConvertFromInternalUnits(dist, DisplayUnitType.DUT_METERS);` và cập nhật `TaskDialog` thành “meters”.  

**Mục tiêu**  
- Hiểu cách sử dụng `ReferenceIntersector` để chiếu tia và tìm giao cắt trong không gian 3D.  
- Tạo vector hướng và điểm xuất phát để kiểm tra mối quan hệ giữa các phần tử (cột và mái).  
- Trích xuất điểm giao cắt và tính khoảng cách bằng `DistanceTo`.  

**Mở rộng**:  
- Sử dụng `Find` thay vì `FindNearest` để tìm tất cả giao cắt trên đường đi của tia.  
- Thêm bộ lọc khác (như `OfClass` hoặc `OfCategory` cho các danh mục khác) để kiểm tra giao cắt với các phần tử khác (tường, sàn, v.v.).  
- Hiển thị thêm thông tin về phần tử giao cắt (như tên hoặc ID) trong `TaskDialog`.  

Trong các phần tiếp theo, bạn sẽ học cách áp dụng các kỹ thuật hình học nâng cao hơn hoặc tự động hóa các tác vụ phân tích không gian trong Revit.