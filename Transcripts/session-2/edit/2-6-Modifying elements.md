---
## Kịch bản bài học: Chỉnh sửa mô hình với Transaction trong Revit
---

### Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ hoàn thiện lệnh **DeleteElement** để xóa một phần tử được chọn, sử dụng **Transaction** để chỉnh sửa mô hình. Sau bài học này, bạn sẽ biết:

* Cách sử dụng phương thức **Delete** để xóa phần tử.
* Cách tạo **TaskDialog** với các nút tùy chỉnh (OK và Cancel) để xác nhận hành động.
* Cách sử dụng **Commit** và **Rollback** để quản lý thay đổi trong Transaction.

Hãy cùng bắt đầu!

---
## Phần 1: Tìm hiểu phương thức Delete trong Object Browser
---

Hướng dẫn viên (giọng điệu hào hứng): Trong bài trước, chúng ta đã tạo lệnh `DeleteElement` với một Transaction đã được bắt đầu. Bây giờ, hãy tìm cách xóa phần tử bằng phương thức **Delete** và thêm xác nhận từ người dùng để đảm bảo an toàn!

### Bước 1: Khám phá phương thức Delete
Trong Visual Studio, mở **Object Browser** bằng cách nhấp đúp vào tham chiếu `RevitAPI` trong **Solution Explorer**. Tìm “Delete” trong ô tìm kiếm. Bạn sẽ thấy phương thức `Delete` trong lớp `Document`, chấp nhận một `ElementId` làm tham số để xóa phần tử tương ứng. Đảm bảo Object Browser hiển thị “Show inherited members” để thấy phương thức này.

Hành động trên màn hình:  

Hiển thị Solution Explorer, nhấp đúp vào RevitAPI để mở Object Browser.  
Nhập “Delete” vào ô tìm kiếm, highlight phương thức `Document.Delete(ElementId)`.  
Chèn text: “**Delete**: Xóa phần tử bằng Element ID.”

---
## Phần 2: Cập nhật lệnh DeleteElement trong Visual Studio
---

Hướng dẫn viên (giọng điệu rõ ràng): Hãy quay lại Visual Studio để thêm mã xóa phần tử và xác nhận bằng TaskDialog trong lệnh DeleteElement.

### Bước 2: Mở tệp DeleteElement.cs
Mở tệp `DeleteElement.cs` trong dự án `MyRevitCommands`. Mã hiện tại đã có Transaction với Start, như sau:

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
                        // Code xóa phần tử sẽ được thêm vào đây
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

Hiển thị Solution Explorer, nhấp vào DeleteElement.cs.  
Zoom vào khối `using (Transaction trans...)` và dòng `trans.Start();`.  
Chèn text: “**Transaction đã sẵn sàng**: Tiếp tục thêm mã xóa phần tử.”

### Bước 3: Thêm mã xóa phần tử
Trong khối `using`, sau dòng `trans.Start()`, thêm mã để xóa phần tử bằng `Document.Delete`:

```csharp
doc.Delete(pickedObj.ElementId);
```

Hành động trên màn hình:  

Nhập dòng `doc.Delete(pickedObj.ElementId);`.  
Highlight `pickedObj.ElementId`, giải thích bằng text: “**ElementId**: Xác định phần tử cần xóa.”  
Chèn text: “**Document.Delete**: Xóa phần tử, nhưng cần Commit để lưu.”

### Bước 4: Tạo TaskDialog để xác nhận
Để đảm bảo người dùng muốn xóa, tạo một **TaskDialog** với các nút OK và Cancel trước khi xóa. Thêm mã trước dòng `doc.Delete`:

```csharp
TaskDialog tDialog = new TaskDialog("Delete Element");
tDialog.MainContent = "Are you sure you want to delete the element?";
tDialog.CommonButtons = TaskDialogCommonButtons.Ok | TaskDialogCommonButtons.Cancel;
```

* `TaskDialog("Delete Element")`: Tạo hộp thoại với tiêu đề “Delete Element”.  
* `MainContent`: Nội dung hỏi người dùng.  
* `CommonButtons`: Thêm nút OK và Cancel, phân tách bằng ký tự `|`.

Hành động trên màn hình:  

Nhập các dòng tạo TaskDialog, highlight `TaskDialogCommonButtons.Ok | TaskDialogCommonButtons.Cancel`.  
Chèn text: “**TaskDialog**: Xác nhận trước khi xóa phần tử.”

### Bước 5: Kiểm tra kết quả TaskDialog và Commit/Rollback
Hiển thị TaskDialog và kiểm tra kết quả để quyết định Commit hoặc Rollback:

```csharp
if (tDialog.Show() == TaskDialogResult.Ok)
{
    doc.Delete(pickedObj.ElementId);
    trans.Commit();
    TaskDialog.Show("Delete", pickedObj.ElementId.ToString() + " deleted.");
}
else
{
    trans.Rollback();
    TaskDialog.Show("Delete", pickedObj.ElementId.ToString() + " not deleted.");
}
```

* `tDialog.Show()`: Hiển thị hộp thoại và trả về `TaskDialogResult`.  
* `TaskDialogResult.Ok`: Nếu người dùng nhấn OK, xóa phần tử và `Commit`.  
* `else`: Nếu nhấn Cancel, `Rollback` và thông báo không xóa.

Mã hoàn chỉnh trong phương thức Execute:

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
                        TaskDialog tDialog = new TaskDialog("Delete Element");
                        tDialog.MainContent = "Are you sure you want to delete the element?";
                        tDialog.CommonButtons = TaskDialogCommonButtons.Ok | TaskDialogCommonButtons.Cancel;

                        if (tDialog.Show() == TaskDialogResult.Ok)
                        {
                            doc.Delete(pickedObj.ElementId);
                            trans.Commit();
                            TaskDialog.Show("Delete", pickedObj.ElementId.ToString() + " deleted.");
                        }
                        else
                        {
                            trans.Rollback();
                            TaskDialog.Show("Delete", pickedObj.ElementId.ToString() + " not deleted.");
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

Nhập khối `if (tDialog.Show()...)`, highlight `trans.Commit()` và `trans.Rollback()`.  
Chèn text: “**Commit/Rollback**: Lưu hoặc hủy thay đổi dựa trên lựa chọn người dùng.”

---
## Phần 3: Đăng ký và kiểm tra lệnh
---

Hướng dẫn viên (giọng điệu khích lệ): Lệnh DeleteElement đã được thêm vào tệp manifest với tên “Delete Element”. Hãy kiểm tra nó trong Revit!

### Bước 6: Kiểm tra trong Revit
Trong Visual Studio, nhấn **F5** để chạy debug mode. Mở Revit, tạo một dự án mới, thêm một section (hoặc chọn một phần tử bất kỳ). Chuyển đến **Add-Ins > External Tools > Delete Element**:

* Chọn một section, **TaskDialog** sẽ hỏi “Are you sure you want to delete the element?”.  
* Nhấn **OK**, phần tử sẽ bị xóa, và một TaskDialog khác thông báo “{Element ID} deleted.”  
* Thử lại, nhấn **Cancel**, phần tử không bị xóa, và thông báo “{Element ID} not deleted.”

Hành động trên màn hình:  

Hiển thị Visual Studio, nhấn F5.  
Trong Revit, tạo dự án mới, thêm section, chạy Delete Element.  
Hiển thị TaskDialog xác nhận, nhấn OK, sau đó hiển thị thông báo xóa thành công.  
Lặp lại, nhấn Cancel, hiển thị thông báo không xóa.  
Chèn text: “**Kiểm tra**: Xóa phần tử với xác nhận người dùng.”

---
## Phần 4: Kết luận và bước tiếp theo
---

Hướng dẫn viên (giọng điệu truyền cảm hứng): Chúc mừng các bạn! Hôm nay, chúng ta đã:

* Sử dụng phương thức **Document.Delete** để xóa phần tử.  
* Tạo **TaskDialog** với nút OK và Cancel để xác nhận hành động.  
* Quản lý **Transaction** với **Commit** và **Rollback** dựa trên lựa chọn người dùng.

Transaction là yếu tố quan trọng khi chỉnh sửa mô hình, và bạn cần luôn đóng nó đúng cách. Chúng ta sẽ tiếp tục sử dụng Transaction trong các bài học sau.

### Bước 7: Kêu gọi hành động
Hãy thử chạy lệnh `DeleteElement` với các phần tử khác, như tường hoặc cửa. Thêm breakpoint trong Visual Studio để kiểm tra `TaskDialogResult`. Nếu bạn gặp vấn đề, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay!

Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!
Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"**Bài học tiếp theo: Chỉnh sửa nâng cao với Transaction.**"  
"**Gặp vấn đề? Để lại câu hỏi trong phần bình luận!**"

Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio, mô phỏng TaskDialog xác nhận xóa.