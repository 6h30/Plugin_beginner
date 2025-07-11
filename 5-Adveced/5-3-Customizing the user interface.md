**5.3. Customizing the User Interface (Tùy chỉnh UI)**  

Sau khi triển khai giao diện `IExternalApplication`, chúng ta sẽ tùy chỉnh giao diện người dùng (UI) bằng cách thêm một tab ribbon, một panel và một nút lệnh liên kết với lệnh `PlaceFamily` (tạo từ các phần trước). Phần này hướng dẫn cách thực hiện trong Visual Studio.

**Sửa lỗi từ video trước**:  
- Trong video trước, các phương thức `OnStartup` và `OnShutdown` ném ngoại lệ (exception). Phần này sẽ sửa bằng cách trả về `Result.Succeeded` và thêm mã tùy chỉnh UI.

**Tra cứu UIControlledApplication**:  
- Trong **Object Browser**, tìm lớp `UIControlledApplication` trong namespace `Autodesk.Revit.UI`.  
- Phương thức:  
  - `CreateRibbonTab(string tabName)`: Tạo tab mới trên ribbon.  
  - `CreateRibbonPanel(string tabName, string panelName)`: Tạo panel trong tab.  
- Lớp `PushButtonData`: Tạo dữ liệu cho nút lệnh, liên kết với lệnh `IExternalCommand`.  

**Chỉnh sửa lớp ExternalApplication**:  
1. **Sửa OnShutdown**:  
   - Xóa ngoại lệ và trả về `Result.Succeeded`:  
     ```csharp
     public Result OnShutdown(UIControlledApplication application)
     {
         return Result.Succeeded;
     }
     ```

2. **Tùy chỉnh OnStartup**:  
   - Xóa ngoại lệ và thêm mã để tạo tab, panel, nút lệnh.  
   - Tạo tab ribbon: `application.CreateRibbonTab("My Commands");`.  
   - Tạo panel: `application.CreateRibbonPanel("My Commands", "Commands");`.  
   - Tạo nút lệnh liên kết với `PlaceFamily`.  

**Mã nguồn mẫu**:  
```csharp
using Autodesk.Revit.UI;
using System.Reflection;

namespace MyRevitApps
{
    public class ExternalApplication : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            try
            {
                // Tạo ribbon tab
                application.CreateRibbonTab("My Commands");

                // Tạo ribbon panel
                RibbonPanel panel = application.CreateRibbonPanel("My Commands", "Commands");

                // Lấy đường dẫn assembly
                string path = Assembly.GetExecutingAssembly().Location;

                // Tạo nút lệnh
                PushButtonData button = new PushButtonData("Button1", "PlaceFamily", path, "MyRevitCommands.PlaceFamily");
                panel.AddItem(button);

                return Result.Succeeded;
            }
            catch
            {
                return Result.Failed;
            }
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
```

**Hướng dẫn chi tiết**:  
1. **Thêm namespace System.Reflection**:  
   - Thêm `using System.Reflection;` để sử dụng `Assembly.GetExecutingAssembly()`.  

2. **Tạo tab ribbon**:  
   - Sử dụng `application.CreateRibbonTab("My Commands");` để tạo tab tên “My Commands”.  

3. **Tạo panel**:  
   - Sử dụng `application.CreateRibbonPanel("My Commands", "Commands");` để tạo panel “Commands” trong tab “My Commands”.  

4. **Tạo nút lệnh**:  
   - Lấy đường dẫn `.dll`:  
     ```csharp
     string path = Assembly.GetExecutingAssembly().Location;
     ```  
   - Tạo `PushButtonData` với các tham số:  
     - Tên nội bộ: `"Button1"`.  
     - Tên hiển thị: `"PlaceFamily"`.  
     - Đường dẫn assembly: `path`.  
     - Tên lớp lệnh: `"MyRevitCommands.PlaceFamily"`.  
   - Thêm nút vào panel: `panel.AddItem(button);`.  

**Cập nhật tệp manifest (.addin)**:  
- Đảm bảo tệp `.addin` đã cấu hình đúng (như phần 5.2):  
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
</RevitAddIns>
```
- **Lưu ý**: Không cần thêm lệnh `PlaceFamily` vào manifest vì nó được tham chiếu qua `IExternalApplication`.

**Kiểm tra**:  
1. Biên dịch dự án trong Visual Studio.  
2. Sao chép `.dll` và `.addin` vào thư mục add-in của Revit (ví dụ: `%appdata%\Autodesk\Revit\Addins\202X`).  
3. Khởi động Revit, vào tab **My Commands**:  
   - Kiểm tra panel “Commands” có nút “PlaceFamily”.  
   - Chuyển sang khung nhìn **Ground Floor Plan**, nhấp nút “PlaceFamily” để kiểm tra lệnh chạy đúng (tương tự lệnh từ các phần trước).  
4. **Lưu ý**: Nếu đường dẫn assembly hoặc tên lớp sai, nút sẽ không hoạt động.

**Mục tiêu**  
- Tùy chỉnh ribbon bằng cách thêm tab, panel và nút lệnh.  
- Liên kết nút với lệnh `IExternalCommand` (PlaceFamily) mà không cần cấu hình riêng trong manifest.  
- Sử dụng `Assembly.GetExecutingAssembly()` để lấy đường dẫn `.dll` động.  

**Ứng dụng**:  
- Nhóm các lệnh tùy chỉnh vào ribbon để cải thiện trải nghiệm người dùng.  
- Giảm số lượng mục trong manifest, đơn giản hóa quản lý add-in.  

Trong các phần tiếp theo, bạn sẽ học cách xử lý sự kiện hoặc tích hợp thêm các tính năng UI nâng cao hơn trong Revit API.