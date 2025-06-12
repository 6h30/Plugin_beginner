---
## Kịch bản bài học: Thay đổi vị trí phần tử trong Revit
---

### Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ học cách **lấy và thay đổi vị trí của một phần tử dựa trên điểm**, như một món đồ nội thất. Chúng ta sẽ tạo một lệnh **ChangeLocation** để di chuyển phần tử 3 đơn vị theo trục X. Sau bài học này, bạn sẽ biết:

* Cách lấy vị trí phần tử bằng thuộc tính **Location** và ép kiểu sang **LocationPoint**.
* Cách đặt vị trí mới bằng **XYZ** trong **Transaction**.
* Cách xử lý lỗi khi phần tử không phải là point-based (như tường).

Hãy cùng bắt đầu!

---
## Phần 1: Tìm hiểu về Location trong Revit API
---

Hướng dẫn viên (giọng điệu hào hứng): Chúng ta đã biết cách tạo phần tử dựa trên điểm (point-based, như nội thất) hoặc đường (line-based, như tường). Hôm nay, chúng ta sẽ học cách lấy và thay đổi vị trí của phần tử bằng thuộc tính **Location**, tập trung vào **LocationPoint** cho các phần tử dựa trên điểm.

### Bước 1: Hiểu về Location Property
Mỗi phần tử trong Revit có thuộc tính **Location**, trả về một đối tượng thuộc lớp **Location**. Có hai loại chính:

* **LocationPoint**: Dành cho phần tử dựa trên điểm (như nội thất), chứa tọa độ **XYZ**.
* **LocationCurve**: Dành cho phần tử dựa trên đường (như tường), chứa **Curve**.

Để lấy **LocationPoint**, ta ép kiểu từ **Location** bằng từ khóa `as`. Nếu ép kiểu thất bại (như khi chọn tường), kết quả sẽ là `null`.

Hành động trên màn hình:  

Hiển thị sơ đồ:
```
Element.Location
├── LocationPoint: XYZ (Point-based)
└── LocationCurve: Curve (Line-based)
```

Chèn text: “**Location**: Xác định vị trí phần tử trong Revit.”

### Bước 2: Thay đổi vị trí
Để thay đổi vị trí của một phần tử point-based, ta:

1. Lấy tọa độ hiện tại từ `LocationPoint.Point` (kiểu **XYZ**).
2. Tạo một **XYZ** mới với tọa độ mong muốn.
3. Gán `LocationPoint.Point` bằng **XYZ** mới trong **Transaction**.

Hành động trên màn hình:  

Hiển thị mã giả lập:
```csharp
LocationPoint locp = element.Location as LocationPoint;
if (locp != null)
{
    XYZ newLoc = new XYZ(locp.Point.X + 3, locp.Point.Y, locp.Point.Z);
    locp.Point = newLoc;
}
```

Chèn text: “**LocationPoint**: Di chuyển phần tử dựa trên điểm.”

---
## Phần 2: Tạo lệnh ChangeLocation trong Visual Studio
---

Hướng dẫn viên (giọng điệu rõ ràng): Hãy mở Visual Studio để tạo lệnh **ChangeLocation**, cho phép chọn một phần tử point-based, lấy vị trí hiện tại, và di chuyển nó 3 đơn vị theo trục X.

### Bước 3: Mở tệp ChangeLocation.cs
Mở tệp `ChangeLocation.cs` trong dự án `MyRevitCommands`. Mã cơ bản đã được chuẩn bị với Transaction trống:

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
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                Reference pickedObj = uidoc.Selection.PickObject(ObjectType.Element);
                if (pickedObj != null)
                {
                    Element ele = doc.GetElement(pickedObj);
                    using (Transaction trans = new Transaction(doc, "Change Location"))
                    {
                        trans.Start();
                        // Thay đổi vị trí tại đây
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

Hiển thị Solution Explorer, nhấp vào `ChangeLocation.cs`.  
Zoom vào khối `if (pickedObj != null)` và Transaction, highlight `ele`.  
Chèn text: “**ChangeLocation**: Điểm khởi đầu để thay đổi vị trí.”

### Bước 4: Lấy LocationPoint
Trong khối Transaction, lấy **LocationPoint** từ phần tử và kiểm tra `null`:

```csharp
LocationPoint locp = ele.Location as LocationPoint;
if (locp != null)
{
    // Thay đổi vị trí tại đây
}
```

Nếu `locp` là `null`, phần tử không phải point-based (như tường), và lệnh sẽ bỏ qua.

Hành động trên màn hình:  

Nhập dòng LocationPoint và khối if, highlight `ele.Location as LocationPoint`.  
Chèn text: “**LocationPoint**: Kiểm tra phần tử point-based.”

### Bước 5: Tạo vị trí mới
Trong khối `if`, lấy tọa độ hiện tại và tạo **XYZ** mới, di chuyển 3 đơn vị theo trục X:

```csharp
XYZ loc = locp.Point;
XYZ newLoc = new XYZ(loc.X + 3, loc.Y, loc.Z);
locp.Point = newLoc;
```

* `locp.Point`: Lấy tọa độ hiện tại.
* `newLoc`: Tạo tọa độ mới, tăng X thêm 3 đơn vị (feet).
* `locp.Point = newLoc`: Đặt vị trí mới.

Hành động trên màn hình:  

Nhập các dòng XYZ, highlight `loc.X + 3`.  
Chèn text: “**XYZ**: Di chuyển phần tử 3 đơn vị theo trục X.”

### Bước 6: Sửa lỗi Transaction
Ban đầu, mã có hai lệnh `trans.Commit()`, gây lỗi nếu Transaction đã kết thúc. Để sửa, di chuyển Transaction vào trong khối `if` để chỉ thực hiện khi `locp != null`:

```csharp
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
```
Mã hoàn chỉnh trong phương thức Execute:

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
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                Reference pickedObj = uidoc.Selection.PickObject(ObjectType.Element);
                if (pickedObj != null)
                {
                    Element ele = doc.GetElement(pickedObj);
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

Di chuyển khối Transaction vào `if (locp != null)`, xóa `trans.Commit()` thừa.  
Chèn text: “**Transaction**: Chỉ thực hiện khi có LocationPoint.”

---
## Phần 3: Đăng ký và kiểm tra lệnh
---

Hướng dẫn viên (giọng điệu khích lệ): Lệnh **ChangeLocation** đã được thêm vào tệp manifest với tên “ChangeLocation”. Hãy kiểm tra nó trong Revit!

### Bước 7: Chạy Debug và kiểm tra
Trong Visual Studio, nhấn **F5** để chạy debug mode. Mở Revit, mở tệp bài tập của video này. Chạy **Add-Ins > External Tools > ChangeLocation**, chọn một món đồ nội thất (point-based). Đồ nội thất sẽ di chuyển 3 đơn vị (feet) theo trục X. Nếu chọn tường (line-based), lệnh sẽ bỏ qua mà không gây lỗi.

Hành động trên màn hình:  

Hiển thị Visual Studio, nhấn F5.  
Trong Revit, mở tệp bài tập, chạy ChangeLocation, chọn một món nội thất.  
Zoom vào nội thất trước và sau khi di chuyển, highlight thay đổi vị trí.  
Chèn text: “**Kiểm tra**: Di chuyển nội thất 3 đơn vị theo trục X.”

---
## Phần 4: Kết luận và bước tiếp theo
---

Hướng dẫn viên (giọng điệu truyền cảm hứng): Chúc mừng các bạn! Hôm nay, chúng ta đã:

* Tìm hiểu về **LocationPoint** và cách lấy vị trí phần tử point-based.  
* Tạo lệnh **ChangeLocation** để di chuyển phần tử 3 đơn vị theo trục X.  
* Sửa lỗi **Transaction** để đảm bảo lệnh hoạt động ổn định.

Thay đổi vị trí là một trong nhiều cách chỉnh sửa phần tử trong Revit API. Trong bài học tiếp theo, chúng ta sẽ khám phá thêm các cách chỉnh sửa khác, như thay đổi **LocationCurve** cho tường.

### Bước 8: Kêu gọi hành động
Hãy thử thay đổi tọa độ trong `newLoc` để di chuyển theo trục Y hoặc Z. Hoặc thêm **TaskDialog** để thông báo khi chọn phần tử không phải point-based. Nếu bạn gặp vấn đề, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay!

Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!
Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"**Bài học tiếp theo: Chỉnh sửa vị trí Line-based Elements.**"  
"**Gặp vấn đề? Để lại câu hỏi trong phần bình luận!**"

Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio, mô phỏng nội thất di chuyển.