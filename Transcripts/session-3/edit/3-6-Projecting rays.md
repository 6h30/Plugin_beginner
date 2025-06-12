---
## Kịch bản bài học: Chiếu tia trong Revit
---

### Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ học cách sử dụng **ReferenceIntersector** để **chiếu tia (project rays)** từ một điểm để kiểm tra mối quan hệ giữa các phần tử không chạm nhau, cụ thể là tính khoảng cách từ chân cột đến mái. Chúng ta sẽ tạo một lệnh **ProjectRay** để chọn một cột, chiếu tia lên trục Z, và hiển thị khoảng cách giao nhau với mái. Sau bài học này, bạn sẽ biết:

* Cách sử dụng **ReferenceIntersector** để chiếu tia và tìm giao điểm.
* Cách tạo vector hướng và bộ lọc danh mục (**ElementCategoryFilter**).
* Cách tính khoảng cách giữa hai điểm và hiển thị bằng **TaskDialog**.

Hãy cùng bắt đầu!

---
## Phần 1: Tìm hiểu về chiếu tia với ReferenceIntersector
---

Hướng dẫn viên (giọng điệu hào hứng): Trong bài trước, chúng ta đã kiểm tra giao nhau giữa các phần tử chạm nhau bằng **ElementIntersectsSolidFilter**. Nhưng nếu các phần tử không chạm nhau thì sao? Hôm nay, chúng ta sẽ sử dụng **ReferenceIntersector** để chiếu tia, giúp tìm các phần tử trong không gian 3D!

### Bước 1: Hiểu về ReferenceIntersector
**ReferenceIntersector** là một lớp trong Revit API dùng để chiếu tia từ một điểm gốc (**origin**) theo một hướng (**vector**) và tìm các phần tử giao nhau. Các thành phần chính:

* **Constructor**: Yêu cầu bộ lọc (**ElementFilter**), loại hình học mục tiêu (**FindReferenceTarget**, như **Face**), và view 3D.
* **Find/FindNearest**:
    * **FindNearest**: Tìm phần tử gần nhất mà tia chạm.
    * **Find**: Tìm tất cả các giao điểm dọc theo tia.
* **ReferenceWithContext**: Kết quả trả về chứa **Reference** (tham chiếu đến hình học) và thông tin giao điểm (**GlobalPoint**).

Trong bài này, chúng ta dùng **FindNearest** để tìm mái gần nhất mà tia từ chân cột chạm vào.

Hành động trên màn hình:  

Hiển thị sơ đồ:
```
ReferenceIntersector
├── Constructor: Filter, FindReferenceTarget, View3D
├── FindNearest: Origin (XYZ), Direction (XYZ)
└── Output: ReferenceWithContext
    ├── Reference
    └── GlobalPoint (XYZ)
```

Chèn text: “**ReferenceIntersector**: Chiếu tia tìm giao điểm.”

### Bước 2: Quy trình chiếu tia
Để chiếu tia từ chân cột đến mái:

* Lấy **LocationPoint** của cột để làm điểm gốc (**origin**).
* Tạo vector hướng (**XYZ**) dọc trục Z (lên trên).
* Tạo **ReferenceIntersector** với bộ lọc **ElementCategoryFilter** cho mái (**OST_Roofs**).
* Gọi **FindNearest** để tìm giao điểm với mặt dưới của mái (**Face**).
* Tính khoảng cách từ origin đến điểm giao nhau (**GlobalPoint**) bằng **DistanceTo**.

Hành động trên màn hình:  

Hiển thị mã giả lập:
```csharp
XYZ origin = locationPoint.Point;
XYZ direction = new XYZ(0, 0, 1);
ReferenceIntersector refI = new ReferenceIntersector(filter, FindReferenceTarget.Face, view3D);
ReferenceWithContext refC = refI.FindNearest(origin, direction);
XYZ intPoint = refC.GetReference().GlobalPoint;
double dist = origin.DistanceTo(intPoint);
```

Chèn text: “**Chiếu tia**: Từ cột đến mái.”

---
## Phần 2: Tạo lệnh ProjectRay trong Visual Studio
---

Hướng dẫn viên (giọng điệu rõ ràng): Hãy mở Visual Studio để tạo lệnh **ProjectRay**, cho phép chọn một cột, chiếu tia từ chân cột lên trục Z, tìm giao điểm với mái, và hiển thị khoảng cách bằng **TaskDialog**.

### Bước 3: Mở tệp ProjectRay.cs
Mở tệp `ProjectRay.cs` trong dự án `MyRevitCommands`. Mã cơ bản đã được chuẩn bị, lấy **LocationPoint** và **XYZ** từ cột:

```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System; // Cần thêm namespace System cho Exception

namespace MyRevitCommands
{
    [Transaction(TransactionMode.ReadOnly)]
    public class ProjectRay : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                Reference pickedObj = uidoc.Selection.PickObject(ObjectType.Element);
                if (pickedObj != null)
                {
                    Element ele = doc.GetElement(pickedObj);
                    LocationPoint locPoint = ele.Location as LocationPoint;
                    if (locPoint != null)
                    {
                        XYZ p1 = locPoint.Point;
                        // Chiếu tia tại đây
                    }
                    else
                    {
                        TaskDialog.Show("Error", "Please select a point-based family.");
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

Hành động trên màn hình:  

Hiển thị Solution Explorer, nhấp vào `ProjectRay.cs`.  
Zoom vào khối `if (locPoint != null)`, highlight `p1`.  
Chèn text: “**ProjectRay**: Điểm khởi đầu để chiếu tia.”

### Bước 4: Tạo vector hướng tia
Tạo vector hướng tia dọc trục Z (lên trên):

```csharp
XYZ rayd = new XYZ(0, 0, 1);
```

Hành động trên màn hình:  

Nhập dòng `XYZ`, highlight `new XYZ(0, 0, 1)`.  
Chèn sơ đồ minh họa tia dọc trục Z từ `p1`.  
Chèn text: “**Vector**: Hướng tia lên trục Z.”

### Bước 5: Tạo ElementCategoryFilter và ReferenceIntersector
Tạo bộ lọc cho mái và khởi tạo **ReferenceIntersector**:

```csharp
ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Roofs);
View3D view3D = doc.ActiveView as View3D;
ReferenceIntersector refI = new ReferenceIntersector(filter, FindReferenceTarget.Face, view3D);
```

* **ElementCategoryFilter**: Giới hạn chỉ tìm mái (**OST_Roofs**).
* **View3D**: Ép kiểu `ActiveView` thành **View3D**. Các phần tử ẩn trong view sẽ không được tính.
* **FindReferenceTarget.Face**: Tìm giao điểm với mặt (**Face**) của mái.

Hành động trên màn hình:  

Nhập các dòng `filter`, `view3D`, và `refI`, highlight `OST_Roofs` và `FindReferenceTarget.Face`.  
Chèn text: “**ReferenceIntersector**: Tìm giao điểm với mặt mái.”

### Bước 6: Chiếu tia và lấy giao điểm
Sử dụng **FindNearest** để tìm giao điểm gần nhất và lấy **GlobalPoint**:

```csharp
ReferenceWithContext refC = refI.FindNearest(p1, rayd);
if (refC != null)
{
    Reference reference = refC.GetReference();
    XYZ intPoint = reference.GlobalPoint;
    // Tính khoảng cách tại đây
}
else
{
    TaskDialog.Show("No Intersection", "No roof found above the column.");
    return Result.Succeeded;
}
```

* **FindNearest**: Chiếu tia từ `p1` theo `rayd`.
* `GetReference`: Lấy **Reference** của giao điểm.
* **GlobalPoint**: Lấy tọa độ giao điểm (**XYZ**).
* Kiểm tra `refC != null` để xử lý trường hợp không tìm thấy giao điểm.

Hành động trên màn hình:  

Nhập mã `FindNearest`, highlight `refC.GetReference()` và `GlobalPoint`.  
Chèn text: “**FindNearest**: Lấy giao điểm gần nhất.”

### Bước 7: Tính khoảng cách và hiển thị
Tính khoảng cách từ `p1` đến `intPoint` và hiển thị bằng **TaskDialog**:

```csharp
double dist = p1.DistanceTo(intPoint);
TaskDialog.Show("Ray", string.Format("Distance to Roof: {0} feet", dist));
```

Lưu ý: Khoảng cách hiển thị bằng feet (đơn vị nội bộ của Revit). Để chuyển sang mét, có thể dùng **UnitUtils.ConvertFromInternalUnits**.

Mã hoàn chỉnh trong phương thức Execute:

```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System; // Cần thêm namespace System cho Exception

namespace MyRevitCommands
{
    [Transaction(TransactionMode.ReadOnly)]
    public class ProjectRay : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                Reference pickedObj = uidoc.Selection.PickObject(ObjectType.Element);
                if (pickedObj != null)
                {
                    Element ele = doc.GetElement(pickedObj);
                    LocationPoint locPoint = ele.Location as LocationPoint;
                    if (locPoint != null)
                    {
                        XYZ p1 = locPoint.Point;
                        XYZ rayd = new XYZ(0, 0, 1);

                        ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Roofs);
                        View3D view3D = doc.ActiveView as View3D;
                        if (view3D == null) // Đảm bảo có View3D
                        {
                            TaskDialog.Show("Error", "Please open a 3D view to use this command.");
                            return Result.Failed;
                        }

                        ReferenceIntersector refI = new ReferenceIntersector(filter, FindReferenceTarget.Face, view3D);

                        ReferenceWithContext refC = refI.FindNearest(p1, rayd);
                        if (refC != null)
                        {
                            Reference reference = refC.GetReference();
                            XYZ intPoint = reference.GlobalPoint;
                            double dist = p1.DistanceTo(intPoint);
                            TaskDialog.Show("Ray", string.Format("Distance to Roof: {0} feet", dist));
                        }
                        else
                        {
                            TaskDialog.Show("No Intersection", "No roof found above the column.");
                            return Result.Succeeded;
                        }
                    }
                    else
                    {
                        TaskDialog.Show("Error", "Please select a point-based family.");
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

Hành động trên màn hình:  

Nhập mã `DistanceTo` và **TaskDialog**, highlight `dist`.  
Chèn text: “**TaskDialog**: Hiển thị khoảng cách đến mái.”

---
## Phần 3: Đăng ký và kiểm tra lệnh
---

Hướng dẫn viên (giọng điệu khích lệ): Lệnh **ProjectRay** đã được thêm vào tệp manifest với tên “ProjectRay”. Hãy kiểm tra nó trong Revit!

### Bước 8: Chạy Debug và kiểm tra
Trong Visual Studio, nhấn **F5** để chạy debug mode. Mở Revit, mở tệp bài tập của video này. Chạy **Add-Ins > External Tools > ProjectRay**, chọn một cột. **TaskDialog** sẽ hiển thị khoảng cách từ chân cột đến mái (ví dụ: 15.7 feet). Nếu không có mái, hiển thị thông báo “No roof found.”

Hành động trên màn hình:  

Hiển thị Visual Studio, nhấn F5.  
Trong Revit, mở tệp bài tập, chạy ProjectRay, chọn một cột.  
Hiển thị **TaskDialog** với khoảng cách, zoom vào cột và mái trong 3D View.  
Chèn text: “**Kiểm tra**: Khoảng cách từ cột đến mái.”

---
## Phần 4: Kết luận và bước tiếp theo
---

Hướng dẫn viên (giọng điệu truyền cảm hứng): Chúc mừng các bạn! Hôm nay, chúng ta đã:

* Sử dụng **ReferenceIntersector** để chiếu tia từ chân cột lên mái.  
* Tạo bộ lọc **ElementCategoryFilter** và tính khoảng cách giao điểm.  
* Hiển thị kết quả bằng **TaskDialog** trong lệnh **ProjectRay**.

Kỹ thuật này rất hữu ích để phân tích mối quan hệ không gian giữa các phần tử, như đo khoảng cách hoặc kiểm tra tầm nhìn.

### Bước 9: Kêu gọi hành động
Hãy thử sử dụng **Find** thay vì **FindNearest** để tìm tất cả giao điểm dọc tia. Hoặc chuyển đổi khoảng cách sang mét bằng **UnitUtils**. Nếu bạn gặp vấn đề, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay!

Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!

Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"**Bài học tiếp theo: Phân tích không gian nâng cao.**"  
"**Gặp vấn đề? Để lại câu hỏi trong phần bình luận!**"

Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio, mô phỏng tia từ cột đến mái.