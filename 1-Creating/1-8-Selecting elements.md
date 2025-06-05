1.8. Chọn phần tử trong Revit  

Nhiều lệnh trong Revit bắt đầu bằng cách yêu cầu người dùng chọn một phần tử (như tường, cột) hoặc một phần của phần tử (như mặt). `UIDocument` cung cấp các phương thức giao diện để thực hiện việc này, bao gồm khả năng chọn phần tử trong dự án. Trong phần này, chúng ta sẽ sử dụng `UIDocument` trong lệnh đã tạo để chọn một phần tử và lấy ID của nó.

**Các bước thực hiện**:  
1. **Sử dụng phương thức PickObject**:  
   - Trong Visual Studio, mở tệp lớp `GetElementId.cs` (từ phần 1.7).  
   - Sau dòng lấy `UIDocument`, thêm mã để yêu cầu người dùng chọn một phần tử:  
     - Tạo biến `Reference` để lưu kết quả: `Reference pickObj = uiDoc.Selection.PickObject(ObjectType.Element);`.  
     - Phương thức `PickObject` từ lớp `Selection` (thuộc `UIDocument`) sẽ yêu cầu người dùng chọn một phần tử bất kỳ trong Revit. Tham số `ObjectType.Element` cho phép chọn mọi loại phần tử (như tường, cột, dầm).  

2. **Kiểm tra lựa chọn và hiển thị ID**:  
   - Để tránh lỗi khi người dùng không chọn phần tử (hoặc thoát lệnh), sử dụng câu lệnh `if` để kiểm tra:  
     - `if (pickObj != null)` – đảm bảo phần tử đã được chọn.  
   - Trong khối `if`, hiển thị ID của phần tử bằng `TaskDialog` (một cửa sổ thông báo của Revit API):  
     - `TaskDialog.Show("Element ID", pickObj.ElementId.ToString());`.  
     - `TaskDialog.Show` nhận hai tham số: tiêu đề (title) và nội dung (message). Ở đây, chúng ta lấy `ElementId` từ `pickObj` và chuyển thành chuỗi bằng `ToString()`.  

3. **Xử lý lỗi với Try-Catch**:  
   - Để tránh lỗi khi người dùng thoát lệnh (nhấn Esc), bọc mã trong khối `try-catch`:  
     - Trong khối `try`, đặt toàn bộ mã chọn phần tử và hiển thị ID.  
     - Trong khối `catch`, gán thông báo lỗi vào tham số `message` và trả về `Result.Failed`:  
       - `message = e.Messageಮ

**Mã nguồn mẫu**:  
```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.ReadOnly)]
    public class GetElementId : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            try
            {
                Reference pickObj = uiDoc.Selection.PickObject(ObjectType.Element);
                if (pickObj != null)
                {
                    TaskDialog.Show("Element ID", pickObj.ElementId.ToString());
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

**Mục tiêu**  
- Hiểu cách sử dụng `UIDocument.Selection.PickObject` để chọn phần tử trong Revit.  
- Biết cách hiển thị thông tin (như ID phần tử) bằng `TaskDialog`.  
- Xử lý lỗi bằng `try-catch` để đảm bảo lệnh hoạt động ổn định.  

Lệnh này hiện đã có thể yêu cầu người dùng chọn một phần tử và hiển thị ID của nó. Trong bước tiếp theo, chúng ta sẽ biên dịch và tải lệnh này vào Revit để kiểm tra.