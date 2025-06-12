---
## Kịch bản bài học: Chỉnh sửa phần tử trong Revit
---

### Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ học cách **chỉnh sửa phần tử** bằng cách sử dụng lớp **ElementTransformUtils** trong Revit API. Chúng ta sẽ tạo một lệnh **EditElement** để di chuyển một món đồ nội thất 3 đơn vị theo trục X và Y, sau đó xoay nó 30 độ quanh trục Z. Sau bài học này, bạn sẽ biết:

* Cách sử dụng **MoveElement** để di chuyển phần tử.
* Cách sử dụng **RotateElement** với trục xoay và góc quay.
* Cách đảm bảo an toàn khi chỉnh sửa phần tử (kiểm tra khả năng chỉnh sửa).

Hãy cùng bắt đầu!

---
## Phần 1: Tìm hiểu ElementTransformUtils trong Object Browser
---

Hướng dẫn viên (giọng điệu hào hứng): Trong bài trước, chúng ta đã thay đổi vị trí phần tử bằng **LocationPoint**. Hôm nay, chúng ta sẽ sử dụng lớp **ElementTransformUtils** để thực hiện các chỉnh sửa phức tạp hơn, như di chuyển, xoay, hoặc sao chép phần tử. Hãy khám phá lớp này trước!

### Bước 1: Khám phá ElementTransformUtils
Trong Visual Studio, mở **Object Browser** (đảm bảo Custom Component Set bao gồm `RevitAPI.dll`). Tìm “ElementTransformUtils”. Lớp này chứa các **phương thức tĩnh (static methods)** để chỉnh sửa phần tử, bao gồm:

* **MoveElement**: Di chuyển một phần tử bằng vector dịch chuyển (**XYZ**).
* **RotateElement**: Xoay phần tử quanh một trục (**Line**) với góc quay (radian).
* **CopyElement**: Sao chép phần tử.
* **CanMirrorElement**: Kiểm tra xem phần tử có thể được phản chiếu hay không.

Các phương thức này hoạt động trên một hoặc nhiều phần tử, tùy thuộc vào tham số.

Hành động trên màn hình:  

Hiển thị Solution Explorer, nhấp đúp vào RevitAPI để mở Object Browser.  
Nhập “ElementTransformUtils”, highlight phương thức MoveElement và RotateElement.  
Chèn text: “**ElementTransformUtils**: Công cụ chỉnh sửa phần tử.”

### Bước 2: Hiểu tham số của MoveElement và RotateElement  

* **MoveElement(Document, ElementId, XYZ)**: Di chuyển phần tử bằng vector XYZ (dịch chuyển theo hướng và độ dài).
* **RotateElement(Document, ElementId, Line, double)**: Xoay phần tử quanh trục (**Line**) với góc quay (radian). Trục được tạo từ hai điểm XYZ.

Hành động trên màn hình:  

Zoom vào `MoveElement(Document, ElementId, XYZ)` và `RotateElement(Document, ElementId, Line, double)` trong Object Browser.  
Chèn text: “**Move & Rotate**: Di chuyển và xoay phần tử.”

---
## Phần 2: Tạo lệnh EditElement trong Visual Studio
---

Hướng dẫn viên (giọng điệu rõ ràng): Hãy mở Visual Studio để tạo lệnh **EditElement**, cho phép chọn một phần tử point-based, di chuyển nó 3 đơn vị theo X và Y, sau đó xoay 30 độ quanh trục Z đi qua tâm của nó.

### Bước 3: Mở tệp EditElement.cs
Mở tệp `EditElement.cs` trong dự án `MyRevitCommands`. Mã cơ bản đã được chuẩn bị, cho phép chọn phần tử và bắt đầu Transaction:

```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class EditElement : IExternalCommand
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
                    using (Transaction trans = new Transaction(doc, "Edit Elements"))
                    {
                        trans.Start();
                        // Chỉnh sửa phần tử tại đây
                        trans.Commit();
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

Hiển thị Solution Explorer, nhấp vào `EditElement.cs`.  
Zoom vào khối `if (pickedObj != null)` và Transaction, highlight `ele`.  
Chèn text: “**EditElement**: Điểm khởi đầu để di chuyển và xoay.”

### Bước 4: Di chuyển phần tử bằng MoveElement
Tạo vector dịch chuyển **XYZ** và sử dụng **MoveElement** để di chuyển phần tử 3 đơn vị theo X và Y:

```csharp
XYZ moveVec = new XYZ(3, 3, 0);
ElementTransformUtils.MoveElement(doc, ele.Id, moveVec);
```

* `moveVec`: Vector dịch chuyển từ (0,0,0) đến (3,3,0), tức 3 đơn vị theo X và Y.
* `MoveElement`: Di chuyển phần tử với `ElementId` và vector.

Hành động trên màn hình:  

Nhập dòng `XYZ` và `MoveElement`, highlight `new XYZ(3, 3, 0)`.  
Chèn sơ đồ minh họa vector từ (0,0,0) đến (3,3,0).  
Chèn text: “**MoveElement**: Di chuyển 3 đơn vị theo X và Y.”

### Bước 5: Xoay phần tử bằng RotateElement
Lấy **LocationPoint** của phần tử, tạo trục xoay (**Line**) qua tâm phần tử, và xoay 30 độ (radian):

```csharp
LocationPoint location = ele.Location as LocationPoint;
XYZ p1 = location.Point;
XYZ p2 = new XYZ(p1.X, p1.Y, p1.Z + 10);
Line axis = Line.CreateBound(p1, p2);
double angle = 30 * Math.PI / 180;
ElementTransformUtils.RotateElement(doc, ele.Id, axis, angle);
```

* `location.Point`: Lấy tọa độ tâm phần tử.
* `p1`, `p2`: Tạo hai điểm trên trục Z (p2 cao hơn p1 10 đơn vị).
* `Line.CreateBound`: Tạo trục xoay từ `p1` đến `p2`.
* `angle`: Chuyển 30 độ sang radian (`30 * π / 180`).
* `RotateElement`: Xoay phần tử quanh `axis` với góc `angle`.

Thêm namespace `System` để sử dụng `Math`:

```csharp
using System;
```

Mã hoàn chỉnh trong phương thức Execute:

```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class EditElement : IExternalCommand
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
                    using (Transaction trans = new Transaction(doc, "Edit Elements"))
                    {
                        trans.Start();
                        XYZ moveVec = new XYZ(3, 3, 0);
                        ElementTransformUtils.MoveElement(doc, ele.Id, moveVec);

                        LocationPoint location = ele.Location as LocationPoint;
                        XYZ p1 = location.Point;
                        XYZ p2 = new XYZ(p1.X, p1.Y, p1.Z + 10);
                        Line axis = Line.CreateBound(p1, p2);
                        double angle = 30 * Math.PI / 180;
                        ElementTransformUtils.RotateElement(doc, ele.Id, axis, angle);

                        trans.Commit();
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

Nhập mã `LocationPoint`, `Line`, và `RotateElement`, highlight `30 * Math.PI / 180`.  
Chèn sơ đồ minh họa trục xoay qua p1 và p2, với góc 30 độ.  
Thêm dòng `using System;` ở đầu tệp.  
Chèn text: “**RotateElement**: Xoay 30 độ quanh trục Z.”

---
## Phần 3: Đăng ký và kiểm tra lệnh
---

Hướng dẫn viên (giọng điệu khích lệ): Lệnh **EditElement** đã được thêm vào tệp manifest với tên “EditElement”. Hãy kiểm tra nó trong Revit!

### Bước 6: Chạy Debug và kiểm tra
Trong Visual Studio, nhấn **F5** để chạy debug mode. Mở Revit, mở tệp bài tập của video này. Chạy **Add-Ins > External Tools > EditElement**, chọn một món đồ nội thất (point-based). Kết quả:

* Đồ nội thất di chuyển 3 đơn vị theo X và Y.
* Sau đó, nó xoay 30 độ quanh trục Z qua tâm của nó.

Lưu ý: Nếu chọn phần tử không phải point-based (như tường), lệnh có thể gây lỗi do `location` là `null`. Có thể thêm kiểm tra `if (location != null)` để an toàn.

Hành động trên màn hình:  

Hiển thị Visual Studio, nhấn F5.  
Trong Revit, mở tệp bài tập, chạy EditElement, chọn một món nội thất.  
Zoom vào nội thất trước và sau khi di chuyển/xoay, highlight thay đổi.  
Chèn text: “**Kiểm tra**: Di chuyển và xoay nội thất.”

---
## Phần 4: Kết luận và bước tiếp theo
---

Hướng dẫn viên (giọng điệu truyền cảm hứng): Chúc mừng các bạn! Hôm nay, chúng ta đã:

* Tìm hiểu lớp **ElementTransformUtils** với các phương thức **MoveElement** và **RotateElement**.  
* Tạo lệnh **EditElement** để di chuyển và xoay một món nội thất.  
* Sử dụng **XYZ** làm vector và tạo trục xoay từ **LocationPoint**.

Kỹ thuật này rất hữu ích để tự động chỉnh sửa nhiều phần tử. Hãy luôn kiểm tra khả năng chỉnh sửa của phần tử (như **CanMirrorElement**) để tránh lỗi.

### Bước 7: Kêu gọi hành động
Hãy thử thay đổi vector `moveVec` hoặc góc `angle` (ví dụ: 45 độ). Thêm kiểm tra `if (location != null)` để xử lý phần tử không phải point-based. Nếu bạn gặp vấn đề, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay!

Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!
Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"**Bài học tiếp theo: Chỉnh sửa phần tử nâng cao.**"  
"**Gặp vấn đề? Để lại câu hỏi trong phần bình luận!**"

Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio, mô phỏng nội thất di chuyển và xoay.