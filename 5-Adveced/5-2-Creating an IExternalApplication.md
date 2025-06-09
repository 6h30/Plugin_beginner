**5.2. Creating an IExternalApplication (Tạo lớp IExternalApplication)**  

Như đã học ở phần trước, chúng ta có thể tùy chỉnh giao diện ribbon của Revit bằng cách tạo một lớp add-in triển khai giao diện `IExternalApplication`. Trong phần này, chúng ta sẽ tạo một lớp `IExternalApplication` cơ bản để bắt đầu tùy chỉnh giao diện người dùng (UI).

**Hướng dẫn tạo IExternalApplication**:  
1. **Chuẩn bị dự án trong Visual Studio**:  
   - Mở tệp bài tập Visual Studio cho phần này.  
   - Trong tệp manifest (`.addin`), đã thêm hai loại add-in:  
     - `<AddIn Type="Application">` cho `IExternalApplication` (đang sử dụng).  
     - `<AddIn Type="DBApplication">` cho `IExternalDBApplication` (tạm thời bị comment, sẽ dùng sau).  

2. **Tổ chức Solution**:  
   - Tạo thư mục mới trong Solution để quản lý các lệnh:  
     - Nhấp chuột phải vào Solution > **Add > New Folder**, đặt tên là `Commands`.  
     - Chọn tất cả các lệnh hiện có (dùng **Ctrl+Shift+Click**) và kéo vào thư mục `Commands`.  
     - Xác nhận thao tác di chuyển (có thể mất vài giây).  

3. **Tạo lớp ExternalApplication**:  
   - Nhấp chuột phải vào Solution > **Add > New Item**.  
   - Chọn **Visual C# Items > Class**, đặt tên là `ExternalApplication.cs`.  
   - Mở tệp `ExternalApplication.cs` và làm lớp kế thừa `IExternalApplication`:  
     - Thêm `: IExternalApplication` sau tên lớp.  
     - Nếu chưa có tham chiếu Revit API, nhấp chuột phải vào `IExternalApplication` > **Quick Actions > Add using Autodesk.Revit.UI**.  
   - Nhấp chuột phải vào `IExternalApplication` > **Quick Actions > Implement Interface** để tự động tạo hai phương thức:  
     - `Result OnStartup(UIControlledApplication application)`  
     - `Result OnShutdown(UIControlledApplication application)`  

**Mã nguồn mẫu**:  
```csharp
using Autodesk.Revit.UI;

namespace MyRevitApps
{
    public class ExternalApplication : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            // Code tùy chỉnh UI (sẽ thêm ở các phần sau)
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            // Dọn dẹp tài nguyên (nếu cần)
            return Result.Succeeded;
        }
    }
}
```

**Cập nhật tệp manifest (.addin)**:  
- Đảm bảo tệp `.addin` đã được cấu hình cho `IExternalApplication`:  
```xml
<?xml version="1.0" encoding="utf-8"?>
<RevitAddIns>
  <AddIn Type="Application">
    <Name>MyExternalApp</Name>
    <Assembly>path\to\MyRevitApps.dll</Assembly>
    <AddInId>YOUR_GUID_HERE</AddInId>
    <FullClassName>MyRevitApps.ExternalApplication</FullClassName>
    <VendorId>YOUR_VENDOR_ID</VendorId>
  </AddIn>
  <!-- <AddIn Type="DBApplication"> tạm thời bị comment </AddIn> -->
</RevitAddIns>
```
- Thay `YOUR_GUID_HERE` bằng GUID mới (**Tools > Create GUID**).  
- Cập nhật đường dẫn đến tệp `.dll` và `VendorId` nếu cần.  

**Kiểm tra**:  
1. Biên dịch dự án trong Visual Studio.  
2. Sao chép tệp `.dll` và `.addin` vào thư mục add-in của Revit (ví dụ: `%appdata%\Autodesk\Revit\Addins\202X`).  
3. Khởi động Revit để kiểm tra lớp `ExternalApplication` được nạp mà không gây lỗi.  
4. **Lưu ý**: Hiện tại, lớp chưa có chức năng cụ thể (chỉ trả về `Result.Succeeded`). Các phần sau sẽ thêm tùy chỉnh UI (ribbon, sự kiện).

**Mục tiêu**  
- Tạo và cấu hình một lớp `IExternalApplication` cơ bản.  
- Tổ chức dự án bằng cách nhóm các lệnh vào thư mục `Commands`.  
- Hiểu cách thêm tham chiếu Revit API và triển khai giao diện `IExternalApplication`.  

**Ứng dụng**: Lớp `ExternalApplication` là nền tảng để tùy chỉnh ribbon hoặc xử lý sự kiện UI trong Revit. Trong các phần tiếp theo, bạn sẽ học cách thêm tab, panel, hoặc nút lệnh vào ribbon, cũng như xử lý các sự kiện như `Idling` hoặc `DialogBoxShowing`.