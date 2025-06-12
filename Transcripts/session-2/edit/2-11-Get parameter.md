---
## Kịch bản bài học: Lấy thông số phần tử trong Revit
---

### Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ học cách **lấy thông số (parameter)** của một phần tử trong Revit, cụ thể là thông số **Head Height** của một cửa sổ. Sau bài học này, bạn sẽ biết:

* **Parameter** là gì và các loại giá trị của chúng.
* Các phương thức để lấy thông số, như **LookupParameter** và **GetOrderedParameters**.
* Cách hiển thị thông tin thông số (name, unit type, built-in parameter) bằng **TaskDialog**.

Hãy cùng bắt đầu!

---
## Phần 1: Tìm hiểu về Parameter trong Revit API
---

Hướng dẫn viên (giọng điệu hào hứng): Trong Revit, thông số (**parameters**) cho phép chúng ta điều chỉnh các đặc điểm của phần tử, như chiều cao đầu cửa sổ. Với Revit API, chúng ta có thể truy xuất và chỉnh sửa thông số trực tiếp. Hãy cùng tìm hiểu cách hoạt động của chúng!

### Bước 1: Hiểu về Parameter Object
Mỗi **Parameter** là một đối tượng chứa thông tin về một đặc tính của phần tử, bao gồm:

* **Definition**: Mô tả tên (ví dụ: “Head Height”) và loại đơn vị (ví dụ: length).
* **Value**: Giá trị của thông số, có thể là int, double, string, ElementId, hoặc none.

Tất cả các phần tử kế thừa từ lớp **Element** (như cửa, tường) đều có thể có thông số.

Hành động trên màn hình:  

Hiển thị sơ đồ đơn giản:
```
Parameter
├── Definition: Tên, Loại đơn vị
└── Value: int, double, ElementId, string, none
```

Chèn text: “**Parameter**: Đặc tính của phần tử trong Revit.”

### Bước 2: Các phương thức lấy Parameter
Có nhiều cách để lấy thông số từ phần tử:

* `Parameters`: Lấy tất cả thông số của phần tử.
* `GetOrderedParameters`: Lấy tất cả thông số theo thứ tự thấy trong giao diện Revit.
* `LookupParameter(string name)`: Lấy một thông số bằng tên (case-sensitive).
* `GetParameters(string name)`: Lấy danh sách các thông số có cùng tên (trường hợp project parameter trùng tên built-in parameter).

Để tránh nhầm lẫn khi có nhiều thông số cùng tên, nên sử dụng **BuiltInParameter** (một enumeration xác định thông số gốc của Revit).

Hành động trên màn hình:  

Hiển thị mã giả lập:
```csharp
Parameter param = element.LookupParameter("Head Height");
IList<Parameter> params = element.GetParameters("Height");
```

Chèn text: “**Phương thức**: Lấy Parameter từ Element.”

---
## Phần 2: Tạo lệnh GetParameter trong Visual Studio
---

Hướng dẫn viên (giọng điệu rõ ràng): Hãy mở Visual Studio để tạo lệnh **GetParameter**, cho phép chọn một cửa sổ và lấy thông số **Head Height**, sau đó hiển thị thông tin bằng **TaskDialog**.

### Bước 3: Mở tệp GetParameter.cs
Mở tệp `GetParameter.cs` trong dự án `MyRevitCommands`. Mã cơ bản đã được chuẩn bị, tương tự lệnh GetElementId, nhưng việc lấy phần tử được đặt trong `if` để an toàn hơn:

```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.ReadOnly)]
    public class GetParameter : IExternalCommand
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
                    Element element = doc.GetElement(pickedObj);
                    // Lấy Parameter tại đây
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

Hiển thị Solution Explorer, nhấp vào `GetParameter.cs`.  
Zoom vào khối `if (pickedObj != null)`, highlight `doc.GetElement(pickedObj)`.  
Chèn text: “**GetParameter**: Điểm bắt đầu để lấy Head Height.”

### Bước 4: Lấy Parameter Head Height
Trong khối `if`, thêm mã để lấy thông số **Head Height** bằng `LookupParameter`:

```csharp
Parameter param = element.LookupParameter("Head Height");
```

Lưu ý: Tên thông số phân biệt chữ hoa/thường, phải viết đúng “Head Height”.

Hành động trên màn hình:  

Nhập dòng `LookupParameter`, highlight `"Head Height"`.  
Chèn text: “**LookupParameter**: Lấy thông số Head Height.”

### Bước 5: Lấy InternalDefinition
Lấy thông tin từ **Definition** của thông số bằng cách ép kiểu sang **InternalDefinition**:

```csharp
InternalDefinition paramDef = param.Definition as InternalDefinition;
```

Mọi Parameter đều có **InternalDefinition**, nhưng không phải lúc nào cũng có **ExternalDefinition** (dành cho shared parameters).

Hành động trên màn hình:  

Nhập dòng `InternalDefinition`, highlight `param.Definition as InternalDefinition`.  
Chèn text: “**InternalDefinition**: Lấy thông tin tên và loại đơn vị.”

### Bước 6: Hiển thị thông tin bằng TaskDialog
Sử dụng **TaskDialog** để hiển thị tên, loại đơn vị, và **BuiltInParameter** của thông số:

```csharp
TaskDialog.Show("Parameters", string.Format("Parameter Name: {0}\nParameter Type: {1}\nBuiltInParameter: {2}",
    paramDef.Name, paramDef.UnitType, paramDef.BuiltInParameter));
```

* `paramDef.Name`: Tên thông số (Head Height).
* `paramDef.UnitType`: Loại đơn vị (`UT_Length`).
* `paramDef.BuiltInParameter`: Enumeration của thông số gốc (`INSTANCE_HEAD_HEIGHT_PARAM`).

Mã hoàn chỉnh trong phương thức Execute:

```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.ReadOnly)]
    public class GetParameter : IExternalCommand
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
                    Element element = doc.GetElement(pickedObj);
                    Parameter param = element.LookupParameter("Head Height");
                    InternalDefinition paramDef = param.Definition as InternalDefinition;

                    TaskDialog.Show("Parameters", string.Format("Parameter Name: {0}\nParameter Type: {1}\nBuiltInParameter: {2}",
                        paramDef.Name, paramDef.UnitType, paramDef.BuiltInParameter));
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

Nhập mã TaskDialog, highlight `string.Format` và các thuộc tính `Name`, `UnitType`, `BuiltInParameter`.  
Chèn text: “**TaskDialog**: Hiển thị thông tin Parameter.”

---
## Phần 3: Đăng ký và kiểm tra lệnh
---

Hướng dẫn viên (giọng điệu khích lệ): Lệnh **GetParameter** đã được thêm vào tệp manifest với tên “GetParameter”. Hãy kiểm tra nó trong Revit!

### Bước 7: Chạy Debug và kiểm tra
Trong Visual Studio, nhấn **F5** để chạy debug mode. Mở Revit, mở tệp bài tập của video này. Chạy **Add-Ins > External Tools > GetParameter**, chọn một cửa sổ. **TaskDialog** sẽ hiển thị thông tin:

```
Parameter Name: Head Height
Parameter Type: UT_Length
BuiltInParameter: INSTANCE_HEAD_HEIGHT_PARAM
```

Lưu ý: Nếu chọn phần tử không phải cửa sổ (không có thông số Head Height), lệnh có thể gây lỗi. Để an toàn, có thể thêm kiểm tra `if (param != null)`.

Hành động trên màn hình:  

Hiển thị Visual Studio, nhấn F5.  
Trong Revit, mở tệp bài tập, chạy GetParameter, chọn một cửa sổ.  
Hiển thị TaskDialog với thông tin thông số, zoom vào Properties của cửa sổ để so sánh Head Height.  
Chèn text: “**Kiểm tra**: Lấy thông số Head Height của cửa sổ.”

---
## Phần 4: Kết luận và bước tiếp theo
---

Hướng dẫn viên (giọng điệu truyền cảm hứng): Chúc mừng các bạn! Hôm nay, chúng ta đã:

* Tìm hiểu về **Parameter** và các phương thức lấy thông số như **LookupParameter**.  
* Lấy thông số **Head Height** của cửa sổ và hiển thị thông tin bằng **TaskDialog**.  
* Hiểu cách sử dụng **InternalDefinition** và **BuiltInParameter**.

Trong bài học tiếp theo, chúng ta sẽ học cách chỉnh sửa giá trị thông số để thay đổi đặc tính của phần tử.

### Bước 8: Kêu gọi hành động
Hãy thử thay đổi lệnh để lấy thông số khác, như “Width” của cửa sổ, hoặc thêm kiểm tra `if (param != null)` để tránh lỗi khi chọn phần tử không có thông số. Nếu bạn gặp vấn đề, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay!

Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!
Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"**Bài học tiếp theo: Chỉnh sửa Parameter trong Revit.**"  
"**Gặp vấn đề? Để lại câu hỏi trong phần bình luận!**"

Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio, mô phỏng TaskDialog hiển thị thông số.