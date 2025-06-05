1.7. Truy cập ứng dụng Revit  

Khi sử dụng Revit API, bạn có thể tương tác với Revit thông qua hai cấp độ: **ứng dụng** (Application, đại diện cho phiên làm việc Revit) và **tài liệu** (Document, đại diện cho tệp dự án Revit). Có bốn đối tượng chính hỗ trợ việc này:  
- **Application**: Cung cấp quyền truy cập vào các cài đặt toàn ứng dụng, như vị trí dự án, ngôn ngữ, hoặc sự kiện ứng dụng.  
- **UIApplication**: Cho phép tương tác với giao diện người dùng của Revit, như thêm nút tùy chỉnh vào thanh ribbon.  
- **Document**: Đại diện cho tệp dự án Revit, cung cấp quyền truy cập vào các phần tử (như tường, cột), khung nhìn và dữ liệu mô hình.  
- **UIDocument**: Cung cấp các phương thức giao diện ở cấp dự án, như làm mới khung nhìn, lấy phần tử được chọn, hoặc yêu cầu người dùng chọn phần tử.  

Trong lệnh của chúng ta, để lấy ID của một phần tử, chúng ta cần sử dụng **UIDocument** để yêu cầu người dùng chọn một phần tử từ giao diện Revit.

**Thêm UIDocument vào lệnh**:  
Tiếp nối từ lệnh đã tạo ở phần trước (1.6), chúng ta sẽ thêm mã để truy cập **UIDocument** trong Visual Studio.  

1. Mở tệp lớp (ví dụ: `GetElementId.cs`).  
2. Trong phương thức `Execute`, trước dòng `return Result.Succeeded;`, thêm mã để lấy **UIDocument** từ tham số `commandData`:  
   - Tạo biến `UIDocument`: `UIDocument uiDoc = commandData.Application.ActiveUIDocument;`.  
   - Dòng mã này truy cập đối tượng `UIApplication` từ `commandData.Application`, sau đó lấy **ActiveUIDocument** (tài liệu giao diện hiện hoạt).  

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
            return Result.Succeeded;
        }
    }
}
```

**Mục tiêu**  
- Hiểu sự khác biệt giữa các đối tượng **Application**, **UIApplication**, **Document**, và **UIDocument**.  
- Biết cách truy cập **UIDocument** từ tham số `ExternalCommandData` trong lệnh.  
- Chuẩn bị nền tảng để sử dụng **UIDocument** cho các tác vụ như chọn phần tử hoặc lấy ID.  

Với **UIDocument** đã được thêm vào lệnh, bạn đã sẵn sàng để sử dụng các phương thức giao diện, như yêu cầu người dùng chọn một phần tử và lấy ID của nó, trong các bước tiếp theo.