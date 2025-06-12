---
## Kịch bản bài học: Sử dụng Transaction để chỉnh sửa mô hình Revit
---

### Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ tìm hiểu về **Transaction** trong Revit API – một cơ chế cần thiết để chỉnh sửa mô hình Revit. Chúng ta sẽ tạo một lệnh **DeleteElement** để xóa một phần tử được chọn. Sau bài học này, bạn sẽ biết:

* **Transaction là gì** và tại sao cần nó để thay đổi mô hình.
* Các phương thức của lớp **Transaction**: `Start`, `Commit`, và `Rollback`.
* Cách sử dụng `using` hoặc `try-catch` để quản lý giao dịch an toàn.

Hãy cùng bắt đầu!

---
## Phần 1: Giới thiệu về Transaction trong Revit API
---

Hướng dẫn viên (giọng điệu hào hứng):Cho đến nay, chúng ta đã truy xuất dữ liệu từ mô hình Revit, như lấy Element ID hoặc lọc cửa sổ. Nhưng để thay đổi mô hình – như xóa, thêm, hoặc chỉnh sửa phần tử – chúng ta cần sử dụng **Transaction**. Nếu không, Revit sẽ báo lỗi. Hãy cùng tìm hiểu cách hoạt động của Transaction!

### Bước 1: Hiểu về Transaction
**Transaction** là một cơ chế đảm bảo mọi thay đổi trong mô hình được thực hiện an toàn. Mọi thay đổi phải nằm trong một Transaction đang hoạt động, và chỉ một Transaction có thể hoạt động tại một thời điểm. Lớp `Transaction` cung cấp ba phương thức chính:

* `Start()`: Bắt đầu giao dịch.
* `Commit()`: Lưu các thay đổi vào mô hình.
* `Rollback()`: Hủy bỏ các thay đổi, khôi phục trạng thái ban đầu.

Hành động trên màn hình:  

Hiển thị sơ đồ đơn giản:
```
Transaction
├── Start(): Bắt đầu giao dịch
├── Commit(): Lưu thay đổi
└── Rollback(): Hủy thay đổi
```

Chèn text: “**Transaction**: Đảm bảo thay đổi mô hình an toàn.”

### Bước 2: Quản lý Transaction an toàn
Để tránh giao dịch bị để mở (gây lỗi), luôn bọc Transaction trong một khối `using` hoặc `try-catch`. Khối `using` tự động gọi `Dispose` để đóng giao dịch, ngay cả khi có lỗi.
Hành động trên màn hình:  

Hiển thị mã giả lập:
```csharp
using (Transaction trans = new Transaction(doc, "My Transaction"))
{
    trans.Start();
    // Thay đổi mô hình
    trans.Commit();
}
```

Chèn text: “**using**: Đảm bảo Transaction được đóng đúng cách.”

---
## Phần 2: Tạo lệnh DeleteElement trong Visual Studio
---

Hướng dẫn viên (giọng điệu rõ ràng):Hãy tạo một lệnh **DeleteElement** để xóa một phần tử được chọn. Tôi đã chuẩn bị một tệp bài tập với lệnh cơ bản, bao gồm UIDocument, Document, và try-catch để chọn phần tử. Hãy mở Visual Studio và bắt đầu!

### Bước 3: Mở tệp DeleteElement.cs
Trong Visual Studio, mở tệp `DeleteElement.cs` từ dự án `MyRevitCommands`. Mã hiện tại như sau:

```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.ReadOnly)]
    public class DeleteElement : IExternalCommand
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
                    // Code để xóa phần tử sẽ được thêm vào đây
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

Hiển thị Solution Explorer, nhấp vào DeleteElement.cs.  
Zoom vào phương thức Execute, highlight UIDocument, Document, và khối if (pickedObj != null).  
Chèn text: “**DeleteElement**: Điểm bắt đầu để xóa phần tử.”

### Bước 4: Thay đổi TransactionMode thành Manual
Vì lệnh này sẽ chỉnh sửa mô hình, đổi thuộc tính `[Transaction(TransactionMode.ReadOnly)]` thành `[Transaction(TransactionMode.Manual)]`. Điều này cho phép tạo Transaction để thay đổi mô hình.

```csharp
[Transaction(TransactionMode.Manual)]
```

Hành động trên màn hình:  

Highlight `[Transaction(TransactionMode.ReadOnly)]`, thay bằng `[Transaction(TransactionMode.Manual)]`.  
Chèn text: “**Manual**: Cho phép chỉnh sửa mô hình với Transaction.”

### Bước 5: Thêm Transaction với using
Trong khối `if (pickedObj != null)`, thêm một khối `using` để tạo và bắt đầu Transaction:

```csharp
using (Transaction trans = new Transaction(doc, "Delete Element"))
{
    trans.Start();
    // Code xóa phần tử sẽ được thêm sau
}
```

`Transaction(doc, "Delete Element")`: Tạo giao dịch với tài liệu `doc` và tên “Delete Element” (hiển thị trong menu Undo của Revit).  
`trans.Start()`: Bắt đầu giao dịch, sẵn sàng cho thay đổi mô hình.

Mã hoàn chỉnh trong phương thức Execute (chưa xóa phần tử):

```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class DeleteElement : IExternalCommand
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
                    using (Transaction trans = new Transaction(doc, "Delete Element"))
                    {
                        trans.Start();
                        // Code xóa phần tử sẽ được thêm sau
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

Trong khối `if`, nhập khối `using` và dòng `trans.Start();`.  
Highlight "Delete Element", giải thích bằng text: “**Tên Transaction**: Hiển thị trong menu Undo.”  
Chèn text: “**using Transaction**: Bắt đầu giao dịch để chỉnh sửa mô hình.”

---
## Phần 3: Kết luận và bước tiếp theo
---

Hướng dẫn viên (giọng điệu truyền cảm hứng):Chúc mừng các bạn! Hôm nay, chúng ta đã:

* Tìm hiểu về **Transaction** và các phương thức `Start`, `Commit`, `Rollback`.  
* Hiểu cách sử dụng `using` để quản lý giao dịch an toàn.  
* Chuẩn bị lệnh **DeleteElement** với Transaction để sẵn sàng xóa phần tử.

Trong bài học tiếp theo, chúng ta sẽ hoàn thiện lệnh DeleteElement bằng cách thêm mã để xóa phần tử được chọn và gọi Commit để lưu thay đổi vào mô hình.

### Bước 6: Kêu gọi hành động
Hãy kiểm tra tệp `DeleteElement.cs` và đảm bảo `TransactionMode.Manual` cùng khối `using` được thêm đúng. Thử biên dịch dự án để kiểm tra lỗi cú pháp. Nếu bạn có câu hỏi, hãy để lại trong phần bình luận, mình sẽ hỗ trợ ngay!

Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!
Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"**Bài học tiếp theo: Hoàn thiện lệnh DeleteElement.**"  
"**Gặp vấn đề? Để lại câu hỏi trong phần bình luận!**"

Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio.