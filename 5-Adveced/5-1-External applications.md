**5.1. External Applications (Ứng dụng bên ngoài)**  

Trong các phần trước, chúng ta đã sử dụng `IExternalCommand` để tạo các lệnh thực thi trong Revit. Phần này giới thiệu một cách khác để tương tác với Revit: **External Applications**, sử dụng các giao diện `IExternalApplication` và `IExternalDBApplication`. Các ứng dụng này cho phép thực thi mã dựa trên sự kiện (events) hoặc tùy chỉnh giao diện người dùng (UI).

**Tổng quan về External Applications**:  
- **IExternalApplication**:  
  - Cung cấp quyền truy cập vào giao diện người dùng (UI) và các sự kiện liên quan qua Revit API.  
  - Yêu cầu triển khai hai phương thức:  
    - `OnStartup(UIControlledApplication)`: Gọi khi Revit khởi động.  
    - `OnShutdown(UIControlledApplication)`: Gọi khi Revit tắt.  
  - `UIControlledApplication`: Đại diện cho UI, cho phép tùy chỉnh ribbon (tạo tab, panel, nút lệnh) bằng các phương thức như `CreateRibbonTab`, `CreateRibbonPanel`, `AddStackedItems`.  
  - Theo dõi các sự kiện UI như hiển thị hộp thoại, kích hoạt khung nhìn, hoặc trạng thái rảnh rỗi (idling).  
- **IExternalDBApplication**:  
  - Tương tự `IExternalApplication`, nhưng tập trung vào các sự kiện cấp cơ sở dữ liệu (database-level), không liên quan đến UI.  
  - Triển khai `OnStartup(ControlledApplication)` và `OnShutdown(ControlledApplication)`.  
  - `ControlledApplication`: Cung cấp quyền truy cập vào các sự kiện như mở tài liệu, đồng bộ với central, hoặc lưu tài liệu.  
  - Kết quả trả về thuộc lớp `ExternalDBApplicationResult`.  

**Cấu hình Manifest cho External Applications**:  
- Tệp `.addin` cần chỉ định loại ứng dụng:  
  - `<AddIn Type="Application">` cho `IExternalApplication`.  
  - `<AddIn Type="DBApplication">` cho `IExternalDBApplication`.  
- Các thẻ khác (như `Name`, `Assembly`, `AddInId`) tương tự như `IExternalCommand`.  

**Tạo ứng dụng mẫu (IExternalApplication)**:  
1. **Tạo lớp ứng dụng**:  
   - Tạo lớp `MyExternalApp.cs`, triển khai `IExternalApplication`.  
   - Thêm namespace: `Autodesk.Revit.UI`, `Autodesk.Revit.DB`.  

2. **Triển khai OnStartup và OnShutdown**:  
   - Trong `OnStartup`:  
     - Tùy chỉnh UI (ví dụ: tạo ribbon tab/panel).  
     - Đăng ký sự kiện (ví dụ: `Idling` hoặc `DialogBoxShowing`).  
   - Trong `OnShutdown`: Dọn dẹp tài nguyên hoặc hủy đăng ký sự kiện.  

**Mã nguồn mẫu (IExternalApplication)**:  
```csharp
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace MyRevitApps
{
    public class MyExternalApp : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            try
            {
                // Tạo ribbon tab
                application.CreateRibbonTab("MyTab");

                // Tạo ribbon panel
                RibbonPanel panel = application.CreateRibbonPanel("MyTab", "MyPanel");

                // Thêm nút lệnh (giả định có lệnh MyCommand)
                PushButtonData buttonData = new PushButtonData("MyCommand", "My Command",
                    @"path\to\MyRevitCommands.dll", "MyRevitCommands.MyCommand");
                panel.AddItem(buttonData);

                return Result.Succeeded;
            }
            catch
            {
                return Result.Failed;
            }
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            // Dọn dẹp tài nguyên (nếu cần)
            return Result.Succeeded;
        }
    }
}
```

**Cấu hình Manifest (.addin)**:  
```xml
<?xml version="1.0" encoding="utf-8"?>
<RevitAddIns>
  <AddIn Type="Application">
    <Name>MyExternalApp</Name>
    <Assembly>path\to\MyRevitApps.dll</Assembly>
    <AddInId>YOUR_GUID_HERE</AddInId>
    <FullClassName>MyRevitApps.MyExternalApp</FullClassName>
    <VendorId>YOUR_VENDOR_ID</VendorId>
  </AddIn>
</RevitAddIns>
```

**Kiểm tra ứng dụng**:  
1. Biên dịch dự án trong Visual Studio.  
2. Đặt tệp `.dll` và `.addin` vào thư mục add-in của Revit (ví dụ: `%appdata%\Autodesk\Revit\Addins\202X`).  
3. Khởi động Revit, kiểm tra:  
   - Tab “MyTab” xuất hiện trên ribbon.  
   - Panel “MyPanel” chứa nút “My Command”.  
4. **Lưu ý**: Đảm bảo đường dẫn trong `.addin` chính xác và GUID duy nhất.

**Mục tiêu**  
- Hiểu sự khác biệt giữa `IExternalApplication` (UI events) và `IExternalDBApplication` (database events).  
- Biết cách triển khai `OnStartup` và `OnShutdown` để tùy chỉnh UI hoặc xử lý sự kiện.  
- Cấu hình tệp manifest cho external applications.  

**Ứng dụng**:  
- Tạo giao diện tùy chỉnh (ribbon, nút lệnh) để cải thiện trải nghiệm người dùng.  
- Theo dõi và xử lý sự kiện (mở tài liệu, lưu, đồng bộ) để tự động hóa quy trình làm việc.  

Trong các phần tiếp theo, bạn sẽ học cách tích hợp sự kiện cụ thể hoặc tạo giao diện người dùng nâng cao hơn trong Revit API.