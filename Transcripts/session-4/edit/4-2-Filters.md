---
## Kịch bản bài học: Bộ lọc trong Revit
---

### Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ học cách **tạo và áp dụng bộ lọc (filters)** trong Revit API để kiểm soát hiển thị của các phần tử. Chúng ta sẽ tạo một lệnh **ViewFilter** để lọc các section có tên chứa chuỗi "WIP", tạo một **ParameterFilterElement**, và ẩn các section này trong view hiện tại. Sau bài học này, bạn sẽ biết:

* Sự khác biệt giữa **ParameterFilterElement** và **SelectionFilterElement**.
* Cách tạo bộ lọc quy tắc với **ParameterFilterRuleFactory**.
* Cách áp dụng và quản lý bộ lọc trong view.

Hãy bắt đầu!

---
## Phần 1: Tìm hiểu về bộ lọc trong Revit API
---

Hướng dẫn viên (giọng điệu hào hứng): Trong bài trước, chúng ta đã tạo một ViewPlan tự động. Hôm nay, chúng ta sẽ sử dụng bộ lọc để kiểm soát cách hiển thị phần tử, cụ thể là ẩn các section có tên chứa "WIP". Hãy khám phá các loại bộ lọc trong Revit API!

### Bước 1: Các loại Filter trong Revit API
Revit API cung cấp hai loại bộ lọc chính để lọc phần tử:

* **SelectionFilterElement**: Lọc một nhóm phần tử được chọn (dựa trên ElementIds).
* **ParameterFilterElement**: Lọc dựa trên quy tắc tham số (Parameter rules), ví dụ: lọc theo tên hoặc giá trị tham số.

Trong bài này, chúng ta sử dụng **ParameterFilterElement** để lọc section dựa trên tham số **VIEW_NAME**.

Hành động trên màn hình:  

Hiển thị sơ đồ:
```
Filters
├── SelectionFilterElement: Lọc phần tử được chọn
└── ParameterFilterElement: Lọc dựa trên quy tắc tham số
    └── ParameterFilterRuleFactory: Tạo quy tắc
```

Chèn text: “**Filters**: Kiểm soát hiển thị phần tử.”

### Bước 2: Tạo ParameterFilterElement
Để tạo một **ParameterFilterElement**, cần:

* **Categories**: Danh sách danh mục (ví dụ: **OST_Sections**).
* **FilterRule**: Quy tắc lọc, tạo bằng `ParameterFilterRuleFactory.CreateContainsRule`.
* **ElementFilter**: Đối tượng **ElementParameterFilter** chứa quy tắc.

Quy tắc `CreateContainsRule` kiểm tra xem một chuỗi trong tham số (như **VIEW_NAME**) có chứa giá trị cụ thể (như "WIP") không.

Hành động trên màn hình:  

Hiển thị mã giả lập:
```csharp
List<ElementId> cats = new List<ElementId> { new ElementId(BuiltInCategory.OST_Sections) };
FilterRule rule = ParameterFilterRuleFactory.CreateContainsRule(paramId, "WIP", false);
ElementParameterFilter filter = new ElementParameterFilter(rule);
ParameterFilterElement filterElement = ParameterFilterElement.Create(doc, "My Filter", cats, filter);
```

Chèn text: “**ParameterFilterElement**: Tạo bộ lọc quy tắc.”

### Bước 3: Áp dụng bộ lọc vào View
Để áp dụng bộ lọc:

* Sử dụng `View.AddFilter` để thêm **ParameterFilterElement** vào view.
* Sử dụng `View.SetFilterVisibility` để đặt hiển thị (`true` để hiện, `false` để ẩn).

Hành động trên màn hình:  

Hiển thị mã giả lập:
```csharp
doc.ActiveView.AddFilter(filterElement.Id);
doc.ActiveView.SetFilterVisibility(filterElement.Id, false);
```

Chèn text: “`View.AddFilter`: Áp dụng bộ lọc vào view.”

---
## Phần 2: Tạo lệnh ViewFilter trong Visual Studio
---

Hướng dẫn viên (giọng điệu rõ ràng): Hãy mở Visual Studio để tạo lệnh **ViewFilter**, tạo một **ParameterFilterElement** để lọc các section có tên chứa "WIP", áp dụng vào view hiện tại, và ẩn các section phù hợp.

### Bước 4: Mở tệp ViewFilter.cs
Mở tệp `ViewFilter.cs` trong dự án `MyRevitCommands`. Mã cơ bản đã được chuẩn bị, lấy **Document** và bắt đầu **Transaction**:

```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Collections.Generic;
using System; // Thêm namespace System cho Exception

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class ViewFilter : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                using (Transaction trans = new Transaction(doc, "Create View Filter"))
                {
                    trans.Start();
                    // Tạo bộ lọc tại đây
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

Hiển thị Solution Explorer, nhấp vào `ViewFilter.cs`.  
Zoom vào khối **Transaction**, highlight `doc`.  
Chèn text: “**ViewFilter**: Điểm khởi đầu để tạo bộ lọc.”

### Bước 5: Tạo danh sách danh mục (Categories)
Tạo danh sách **ElementId** chứa danh mục **OST_Sections**:

```csharp
List<ElementId> cats = new List<ElementId>();
cats.Add(new ElementId(BuiltInCategory.OST_Sections));
```

Hành động trên màn hình:  

Nhập mã `List`, highlight `OST_Sections`.  
Chèn text: “**Categories**: Lọc danh mục Sections.”

### Bước 6: Tạo ElementParameterFilter với FilterRule
Tạo quy tắc lọc `CreateContainsRule` và **ElementParameterFilter**:

```csharp
ElementParameterFilter filter = new ElementParameterFilter(
    ParameterFilterRuleFactory.CreateContainsRule(
        new ElementId(BuiltInParameter.VIEW_NAME), "WIP", false));
```

* **VIEW_NAME**: Tham số tên của view.
* `"WIP"`: Chuỗi cần tìm.
* `false`: Không phân biệt hoa thường.

Hành động trên màn hình:  

Nhập mã **ElementParameterFilter**, highlight **VIEW_NAME** và `"WIP"`.  
Chèn text: “**FilterRule**: Lọc tên chứa WIP.”

### Bước 7: Tạo ParameterFilterElement
Trong **Transaction**, tạo **ParameterFilterElement**:

```csharp
ParameterFilterElement filterElement = ParameterFilterElement.Create(doc, "My first filter", cats, filter);
```

* `Create`: Tạo bộ lọc với tên, danh mục, và quy tắc.

Hành động trên màn hình:  

Nhập mã **ParameterFilterElement**, highlight `"My first filter"`.  
Chèn text: “**ParameterFilterElement**: Tạo bộ lọc.”

### Bước 8: Áp dụng bộ lọc vào View và ẩn phần tử
Áp dụng bộ lọc vào view hiện tại và ẩn các section phù hợp:

```csharp
doc.ActiveView.AddFilter(filterElement.Id);
doc.ActiveView.SetFilterVisibility(filterElement.Id, false);
```

Mã hoàn chỉnh trong phương thức Execute:

```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Collections.Generic;
using System; // Thêm namespace System cho Exception

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class ViewFilter : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                List<ElementId> cats = new List<ElementId>();
                cats.Add(new ElementId(BuiltInCategory.OST_Sections));

                ElementParameterFilter filter = new ElementParameterFilter(
                    ParameterFilterRuleFactory.CreateContainsRule(
                        new ElementId(BuiltInParameter.VIEW_NAME), "WIP", false));

                using (Transaction trans = new Transaction(doc, "Create View Filter"))
                {
                    trans.Start();
                    ParameterFilterElement filterElement = ParameterFilterElement.Create(doc, "My first filter", cats, filter);
                    doc.ActiveView.AddFilter(filterElement.Id);
                    doc.ActiveView.SetFilterVisibility(filterElement.Id, false);
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

Nhập mã `AddFilter` và `SetFilterVisibility`, highlight `filterElement.Id`.  
Chèn text: “**View**: Áp dụng và ẩn section chứa WIP.”

---
## Phần 3: Đăng ký và kiểm tra lệnh
---

Hướng dẫn viên (giọng điệu khích lệ): Lệnh **ViewFilter** đã được thêm vào tệp manifest với tên “ViewFilter”. Hãy kiểm tra nó trong Revit!

### Bước 9: Chạy Debug và kiểm tra
Trong Visual Studio, nhấn **F5** để chạy debug mode. Mở Revit, mở tệp bài tập của video này, chứa bốn section, hai trong số đó có tên chứa "WIP". Chạy **Add-Ins > External Tools > ViewFilter**. Kết quả: các section có tên chứa "WIP" sẽ bị ẩn. Kiểm tra bộ lọc trong **Visibility/Graphics Overrides (VV)**, tab **Filters**, sẽ thấy “My first filter”.

Hành động trên màn hình:  

Hiển thị Visual Studio, nhấn F5.  
Trong Revit, mở tệp bài tập, chạy ViewFilter.  
Mở **Visibility/Graphics Overrides**, highlight “My first filter” trong tab **Filters**.  
Chèn text: “**Kiểm tra**: Ẩn section chứa WIP.”

---
## Phần 4: Kết luận và bước tiếp theo
---

Hướng dẫn viên (giọng điệu truyền cảm hứng): Chúc mừng các bạn! Hôm nay, chúng ta đã:

* Tìm hiểu **ParameterFilterElement** và **SelectionFilterElement**.  
* Tạo lệnh **ViewFilter** để lọc section có tên chứa "WIP".  
* Áp dụng bộ lọc vào view và ẩn các section phù hợp.

Tạo bộ lọc tự động giúp tiết kiệm thời gian khi quản lý hiển thị trong các dự án lớn.

### Bước 10: Kêu gọi hành động
Hãy thử thay đổi quy tắc lọc để tìm chuỗi khác hoặc lọc danh mục khác (như **OST_Walls**). Hoặc thêm **TaskDialog** để thông báo số section bị ẩn. Nếu bạn gặp vấn đề, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay!

Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!

Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"**Bài học tiếp theo: Tạo và quản lý Sheet.**"  
"**Gặp vấn đề? Để lại câu hỏi trong phần bình luận!**"

Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio, mô phỏng Visibility/Graphics với bộ lọc.