2.5. Transaction  

Trong các phần trước, chúng ta đã sử dụng Revit API để truy cập phần tử và dữ liệu từ mô hình Revit mà không thay đổi mô hình. Để thực hiện thay đổi, chúng ta cần sử dụng **Transaction**. Mọi thay đổi trong mô hình Revit phải được bao bọc trong một **Transaction** đang hoạt động, nếu không sẽ xảy ra lỗi (exception). 

**Lớp Transaction**:  
- Lớp `Transaction` cung cấp các phương thức để quản lý thay đổi:  
  - `Start()`: Bắt đầu giao dịch.  
  - `Commit()`: Lưu các thay đổi vào mô hình.  
  - `Rollback()`: Hoàn tác các thay đổi về trạng thái trước giao dịch.  
- Chỉ một giao dịch được phép hoạt động tại một thời điểm.  
- Nên bao bọc giao dịch trong khối `using` hoặc `try-catch` để đảm bảo giao dịch không bị bỏ ngỏ.

**Tạo lệnh xóa phần tử với Transaction**:  
Chúng ta sẽ tạo một lệnh mới (`DeleteElement`) để xóa một phần tử được chọn trong Revit.

1. **Tạo lớp lệnh**:  
   - Trong Visual Studio, tạo lớp `DeleteElement.cs`, triển khai `IExternalCommand`.  
   - Thêm các namespace: `Autodesk.Revit.DB`, `Autodesk.Revit.UI`, `Autodesk.Revit.Attributes`.  
   - Đặt thuộc tính `[Transaction(TransactionMode.Manual)]` (thay vì `ReadOnly`) để cho phép thay đổi mô hình.  

2. **Thêm Transaction**:  
   - Trong phương thức `Execute`, lấy `UIDocument` và `Document`.  
   - Sử dụng `PickObject` để chọn phần tử, kiểm tra `if (pickObj != null)`.  
   - Trong khối `if`, thêm giao dịch bằng câu lệnh `using`:  
     - `using (Transaction trans = new Transaction(doc, "Delete Element")) { ... }`.  
     - Gọi `trans.Start()` để bắt đầu giao dịch.  

**Mã nguồn mẫu**:  
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
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            try
            {
                Reference pickObj = uiDoc.Selection.PickObject(ObjectType.Element);
                if (pickObj != null)
                {
                    using (Transaction trans = new Transaction(doc, "Delete Element"))
                    {
                        trans.Start();
                        // Code xóa phần tử sẽ được thêm ở phần sau
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

**Cập nhật tệp manifest (.addin)**:  
- Mở `MyRevitCommands.addin`, sao chép khối `<AddIn>` từ lệnh trước.  
- Cập nhật:  
  - `<AddInId>`: Tạo GUID mới (vào **Tools > Create GUID**).  
  - `<FullClassName>`: Đặt thành `MyRevitCommands.DeleteElement`.  
  - `<Name>` và `<Text>`: Đặt thành `DeleteElement`.  
  - `<Description>`: Cập nhật thành “Deletes a selected element in the project”.  

**Mục tiêu**  
- Hiểu vai trò của `Transaction` trong việc thay đổi mô hình Revit.  
- Biết cách sử dụng lớp `Transaction` với các phương thức `Start`, `Commit`, và `Rollback`.  
- Tạo một lệnh cơ bản với `Transaction` để chuẩn bị cho việc chỉnh sửa mô hình.  

Hiện tại, lệnh chỉ thiết lập giao dịch mà chưa thực hiện thay đổi (xóa phần tử). Trong phần tiếp theo, chúng ta sẽ thêm mã để xóa phần tử và hoàn tất lệnh.