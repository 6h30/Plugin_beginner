---
## Kịch bản bài học: Gắn thẻ phần tử trong Revit
---

### Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ học cách **chú thích phần tử bằng cách gắn thẻ (tagging) tự động** trong Revit API. Chúng ta sẽ tạo một lệnh **TagView** để gắn thẻ tất cả cửa ra vào (**Doors**) và cửa sổ (**Windows**) trong view hiện tại, sử dụng **IndependentTag**. Sau bài học này, bạn sẽ biết:

* Sự khác biệt giữa **IndependentTag** và **RoomTag**.
* Cách sử dụng **ElementMulticategoryFilter** để lọc nhiều danh mục.
* Cách tạo thẻ với `IndependentTag.Create` và định vị trí thẻ.

Hãy cùng bắt đầu!

---
## Phần 1: Tìm hiểu về gắn thẻ trong Revit API
---

Hướng dẫn viên (giọng điệu hào hứng): Trong bài trước, chúng ta đã tạo bộ lọc để kiểm soát hiển thị phần tử. Hôm nay, chúng ta sẽ chú thích phần tử bằng cách gắn thẻ, một bước quan trọng để phát triển view trong Revit. Hãy khám phá cách sử dụng **IndependentTag**!

### Bước 1: Các loại Tag trong Revit API
Revit API cung cấp hai lớp chính để gắn thẻ:

* **IndependentTag**: Gắn thẻ cho các phần tử như cửa, cửa sổ, tường, v.v., với tùy chọn loại thẻ (**TagType**) hoặc mặc định dựa trên danh mục phần tử.
* **RoomTag**: Gắn thẻ cho phòng (**Rooms**).

Trong bài này, chúng ta sử dụng **IndependentTag** để gắn thẻ cửa ra vào và cửa sổ, với phương thức `IndependentTag.Create` mặc định (không chỉ định **TagType**).

Hành động trên màn hình:  

Hiển thị sơ đồ:
```
Tags
├── IndependentTag: Cửa, cửa sổ, tường...
│   └── Create: View, Reference, Leader, TagMode, Orientation, Location
└── RoomTag: Phòng
```

Chèn text: “**IndependentTag**: Gắn thẻ phần tử.”

### Bước 2: Tham số của IndependentTag.Create
Phương thức `IndependentTag.Create` (phiên bản mặc định) yêu cầu:

* **Document**: Tài liệu Revit.
* **ViewId**: View để gắn thẻ (2D hoặc 3D bị khóa).
* **Reference**: Tham chiếu đến phần tử cần gắn thẻ (tạo từ **Element**).
* **AddLeader**: Boolean, có hiển thị đường dẫn (**leader**) không.
* **TagMode**: Enum (**TagMode.TM_ADDBY_CATEGORY, TM_ADDBY_MATERIAL**, v.v.).
* **TagOrientation**: Enum (**TagOrientation.Horizontal, Vertical**).
* **Location**: Điểm **XYZ** để đặt thẻ.

Hành động trên màn hình:  

Hiển thị mã giả lập:
```csharp
IndependentTag tag = IndependentTag.Create(doc, viewId, reference, true, TagMode.TM_ADDBY_CATEGORY, TagOrientation.Horizontal, point);
```

Chèn text: “`IndependentTag.Create`: Tạo thẻ tự động.”

---
## Phần 2: Tạo lệnh TagView trong Visual Studio
---

Hướng dẫn viên (giọng điệu rõ ràng): Hãy mở Visual Studio để tạo lệnh **TagView**, tự động gắn thẻ tất cả cửa ra vào và cửa sổ trong view hiện tại, sử dụng **IndependentTag** với **TagMode** theo danh mục và thẻ nằm ngang.

### Bước 3: Mở tệp TagView.cs
Mở tệp `TagView.cs` trong dự án `MyRevitCommands`. Mã cơ bản đã được chuẩn bị, lấy **Document** và bắt đầu **Transaction**:

```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System; // Thêm namespace System cho Exception

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class TagView : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                using (Transaction trans = new Transaction(doc, "Tag Elements"))
                {
                    trans.Start();
                    // Gắn thẻ tại đây
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

Hành động trên màn hình:  

Hiển thị Solution Explorer, nhấp vào `TagView.cs`.  
Zoom vào khối **Transaction**, highlight `doc`.  
Chèn text: “**TagView**: Điểm khởi đầu để gắn thẻ.”

### Bước 4: Định nghĩa TagMode và TagOrientation
Tạo các biến cho **TagMode** và **TagOrientation**:

```csharp
TagMode tmode = TagMode.TM_ADDBY_CATEGORY;
TagOrientation torient = TagOrientation.Horizontal;
```

* `TM_ADDBY_CATEGORY`: Gắn thẻ theo danh mục phần tử (**Doors, Windows**).
* `Horizontal`: Thẻ nằm ngang.

Hành động trên màn hình:  

Nhập mã **TagMode** và **TagOrientation**, highlight `TM_ADDBY_CATEGORY` và `Horizontal`.  
Chèn text: “**TagMode & Orientation**: Cấu hình thẻ.”

### Bước 5: Lọc cửa ra vào và cửa sổ
Sử dụng **ElementMulticategoryFilter** để lấy các phần tử **Doors** và **Windows** trong view hiện tại:

```csharp
List<BuiltInCategory> cats = new List<BuiltInCategory>();
cats.Add(BuiltInCategory.OST_Windows);
cats.Add(BuiltInCategory.OST_Doors);

ElementMulticategoryFilter filter = new ElementMulticategoryFilter(cats);

IList<Element> tElements = new FilteredElementCollector(doc, doc.ActiveView.Id)
    .WherePasses(filter)
    .WhereElementIsNotElementType()
    .ToElements();
```

* **ElementMulticategoryFilter**: Lọc nhiều danh mục (**OST_Windows, OST_Doors**).
* **FilteredElementCollector**: Chỉ lấy phần tử trong **ActiveView**.
* `WhereElementIsNotElementType`: Loại bỏ các **ElementType**.
* `ToElements`: Trả về danh sách phần tử.

Hành động trên màn hình:  

Nhập mã **ElementMulticategoryFilter**, highlight `OST_Windows` và `OST_Doors`.  
Chèn text: “**ElementMulticategoryFilter**: Lọc Doors & Windows.”

### Bước 6: Gắn thẻ từng phần tử
Trong **Transaction**, duyệt qua danh sách phần tử và gắn thẻ:

```csharp
foreach (Element ele in tElements)
{
    Reference refe = new Reference(ele);
    LocationPoint loc = ele.Location as LocationPoint;
    if (loc != null)
    {
        XYZ point = loc.Point;
        IndependentTag tag = IndependentTag.Create(doc, doc.ActiveView.Id, refe, true, tmode, torient, point);
    }
}
```

* **Reference**: Tham chiếu đến phần tử cần gắn thẻ.
* **LocationPoint**: Lấy điểm vị trí của phần tử để đặt thẻ.
* `IndependentTag.Create`: Tạo thẻ với các tham số đã chuẩn bị.
* Kiểm tra `loc != null` để đảm bảo phần tử có **LocationPoint** (Doors và Windows đều là point-based).

Mã hoàn chỉnh trong phương thức Execute:

```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Collections.Generic;
using System; // Thêm namespace System cho Exception
using System.Linq; // Thêm namespace này cho ToElements()

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class TagView : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                TagMode tmode = TagMode.TM_ADDBY_CATEGORY;
                TagOrientation torient = TagOrientation.Horizontal;

                List<BuiltInCategory> cats = new List<BuiltInCategory>();
                cats.Add(BuiltInCategory.OST_Windows);
                cats.Add(BuiltInCategory.OST_Doors);

                ElementMulticategoryFilter filter = new ElementMulticategoryFilter(cats);

                IList<Element> tElements = new FilteredElementCollector(doc, doc.ActiveView.Id)
                    .WherePasses(filter)
                    .WhereElementIsNotElementType()
                    .ToElements();

                using (Transaction trans = new Transaction(doc, "Tag Elements"))
                {
                    trans.Start();
                    foreach (Element ele in tElements)
                    {
                        Reference refe = new Reference(ele);
                        LocationPoint loc = ele.Location as LocationPoint;
                        if (loc != null)
                        {
                            XYZ point = loc.Point;
                            IndependentTag tag = IndependentTag.Create(doc, doc.ActiveView.Id, refe, true, tmode, torient, point);
                        }
                    }
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

Hành động trên màn hình:  

Nhập mã vòng `foreach` và `IndependentTag.Create`, highlight `refe` và `point`.  
Chèn text: “**IndependentTag**: Gắn thẻ Doors & Windows.”

---
## Phần 3: Đăng ký và kiểm tra lệnh
---

Hướng dẫn viên (giọng điệu khích lệ): Lệnh **TagView** đã được thêm vào tệp manifest với tên “TagView”. Hãy kiểm tra nó trong Revit!

### Bước 7: Chạy Debug và kiểm tra
Trong Visual Studio, nhấn **F5** để chạy debug mode. Mở Revit, mở tệp bài tập của video này, chứa một số cửa ra vào và cửa sổ. Chạy **Add-Ins > External Tools > TagView**. Kết quả: Tất cả cửa ra vào và cửa sổ trong view hiện tại sẽ được gắn thẻ với leader, sử dụng **TagMode** ngang và theo danh mục.

Hành động trên màn hình:  

Hiển thị Visual Studio, nhấn F5.  
Trong Revit, mở tệp bài tập, chạy TagView.  
Zoom vào view, highlight các thẻ trên cửa ra vào và cửa sổ.  
Chèn text: “**Kiểm tra**: Gắn thẻ Doors & Windows.”

---
## Phần 4: Kết luận và bước tiếp theo
---

Hướng dẫn viên (giọng điệu truyền cảm hứng): Chúc mừng các bạn! Hôm nay, chúng ta đã:

* Tìm hiểu **IndependentTag** và cách gắn thẻ phần tử.  
* Tạo lệnh **TagView** để tự động gắn thẻ cửa ra vào và cửa sổ.  
* Sử dụng **ElementMulticategoryFilter** để lọc nhiều danh mục.

Tự động gắn thẻ là công cụ mạnh mẽ để tăng tốc quá trình chú thích trong các dự án lớn.

### Bước 8: Kêu gọi hành động
Hãy thử mở rộng lệnh để gắn thẻ trong nhiều view thay vì chỉ view hiện tại. Hoặc thử thay đổi **TagMode** thành `TM_ADDBY_MATERIAL` để gắn thẻ theo vật liệu. Nếu bạn gặp vấn đề, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay!

Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!

Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"**Bài học tiếp theo: Tạo và quản lý Sheet.**"  
"**Gặp vấn đề? Để lại câu hỏi trong phần bình luận!**"

Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio, mô phỏng view với các thẻ.