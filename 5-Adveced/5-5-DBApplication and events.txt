**5.5 DB Application and Events (Ứng dụng DB và Đăng ký sự kiện)**  

Phần này hướng dẫn cách sử dụng giao diện `IExternalDBApplication` để tạo một add-in xử lý các sự kiện cấp cơ sở dữ liệu (database-level events) trong Revit, cụ thể là sự kiện `DocumentChanged`. Add-in sẽ theo dõi các thay đổi của phần tử nội thất (`OST_Furniture`) và hiển thị thông báo về phần tử thay đổi đầu tiên cùng tên giao dịch (transaction name). Chúng ta cũng sẽ đăng ký và hủy đăng ký sự kiện để đảm bảo add-in hoạt động đúng.

**Tra cứu IExternalDBApplication**:  
- Trong **Object Browser**, tìm lớp `IExternalDBApplication` trong namespace `Autodesk.Revit.DB`.  
- Yêu cầu triển khai:  
  - `ExternalDBApplicationResult OnStartup(ControlledApplication)`: Đăng ký sự kiện.  
  - `ExternalDBApplicationResult OnShutdown(ControlledApplication)`: Hủy đăng ký sự kiện.  
- `ControlledApplication`: Cung cấp quyền truy cập các sự kiện như `DocumentChanged`, `DocumentOpened`, `DocumentSaved`.  
- Sự kiện `DocumentChanged`:  
  - Thuộc namespace `Autodesk.Revit.DB.Events`.  
  - Cung cấp `DocumentChangedEventArgs` với các phương thức:  
    - `GetModifiedElementIds(ElementFilter)`: Lấy danh sách ID phần tử thay đổi.  
    - `GetTransactionNames()`: Lấy tên các giao dịch gây ra thay đổi.  
- Delegate: `EventHandler<DocumentChangedEventArgs>` liên kết phương thức xử lý sự kiện.  

**Tạo lớp ExternalDBApp**:  
1. **Tạo lớp mới**:  
   - Trong Visual Studio, nhấp chuột phải vào Solution → **Add > New Item**.  
   - Chọn **Visual C# Items > Class**, đặt tên là `ExternalDBApp.cs`.  
   - Làm lớp kế thừa `IExternalDBApplication`:  
     - Thêm `: IExternalDBApplication`.  
     - Nhấp vào biểu tượng bóng đèn → **Add using Autodesk.Revit.DB**.  
     - Nhấp lại → **Implement Interface** để tạo `OnStartup` và `OnShutdown`.  

2. **Tạo phương thức xử lý sự kiện**:  
   - Tạo phương thức `ElementChangedEvent`:  
     - Thêm `using Autodesk.Revit.DB.Events;` để dùng `DocumentChangedEventArgs`.  
     - Thêm `using Autodesk.Revit.UI;` để dùng `TaskDialog`.  
     - Tạo bộ lọc: `ElementCategoryFilter` cho `OST_Furniture`.  
     - Lấy ID phần tử thay đổi đầu tiên và tên giao dịch.  
     - Hiển thị thông báo qua `TaskDialog`.  

3. **Đăng ký sự kiện trong OnStartup**:  
   - Bọc mã trong `try-catch` để xử lý ngoại lệ.  
   - Đăng ký sự kiện:  
     ```csharp
     application.DocumentChanged += new EventHandler<DocumentChangedEventArgs>(ElementChangedEvent);
     ```  
   - Trả về `ExternalDBApplicationResult.Succeeded` hoặc `Failed`.  

4. **Hủy đăng ký trong OnShutdown**:  
   - Hủy đăng ký sự kiện:  
     ```csharp
     application.DocumentChanged -= new EventHandler<DocumentChangedEventArgs>(ElementChangedEvent);
     ```  

**Mã nguồn mẫu**:  
```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System.Linq;

namespace MyRevitApps
{
    public class ExternalDBApp : IExternalDBApplication
    {
        public ExternalDBApplicationResult OnStartup(ControlledApplication application)
        {
            try
            {
                // Đăng ký sự kiện DocumentChanged
                application.DocumentChanged += new EventHandler<DocumentChangedEventArgs>(ElementChangedEvent);
                return ExternalDBApplicationResult.Succeeded;
            }
            catch
            {
                return ExternalDBApplicationResult.Failed;
            }
        }

        public ExternalDBApplicationResult OnShutdown(ControlledApplication application)
        {
            // Hủy đăng ký sự kiện
            application.DocumentChanged -= new EventHandler<DocumentChangedEventArgs>(ElementChangedEvent);
            return ExternalDBApplicationResult.Succeeded;
        }

        public void ElementChangedEvent(object sender, DocumentChangedEventArgs args)
        {
            // Tạo bộ lọc cho nội thất
            ElementCategoryFilter filter = new(BuiltInCategory.OST_Furniture);

            // Lấy ID phần tử thay đổi đầu tiên
            ElementId elementId = args.GetModifiedElementIds(filter).FirstOrDefault();
            if (elementId != null)
            {
                // Lấy tên giao dịch
                string name = args.GetTransactionNames().FirstOrDefault() ?? "Unknown";

                // Hiển thị thông báo
                TaskDialog.Show("Modified Element",
                    $"Element ID {elementId} changed by transaction: {name}");
            }
        }
    }
}
```

**Cập nhật tệp manifest (.addin)**:  
- Bỏ comment mục `<AddIn Type="DBApplication">` trong tệp `.addin`:  
```xml
<?xml version="1.0" encoding="utf-8"?>
<RevitAddIns>
  <AddIn Type="DBApplication">
    <Name>MyExternalDBApp</Name>
    <Assembly>path\to\MyRevitApps.dll</Assembly>
    <AddInId>YOUR_GUID_HERE</AddInId>
    <FullClassName>MyRevitApps.ExternalDBApp</FullClassName>
    <VendorId>YOUR_VENDOR_ID</VendorId>
  </AddIn>
</RevitAddIns>
```
- Thay `YOUR_GUID_HERE` bằng GUID mới (**Tools > Create GUID**).  
- Cập nhật đường dẫn `.dll` và `VendorId`.  

**Kiểm tra**:  
1. Biên dịch dự án trong Visual Studio.  
2. Sao chép `.dll` và `.addin` vào thư mục add-in của Revit (ví dụ: `%appdata%\Autodesk\Revit\Addins\202X`).  
3. Khởi động Revit, mở tệp dự án từ thư mục bài tập.  
4. Thay đổi một phần tử nội thất (furniture), ví dụ: sử dụng lệnh **Move**.  
5. Kiểm tra:  
   - `TaskDialog` hiển thị, ví dụ: “Element ID 217813 changed by transaction: Move”.  
6. **Lưu ý**:  
   - Nếu không có nội thất thay đổi, `TaskDialog` không xuất hiện (`FirstOrDefault` trả về `null`).  
   - Hủy đăng ký sự kiện trong `OnShutdown` ngăn rò rỉ bộ nhớ.  

**Mục tiêu**:  
- Tạo và cấu hình lớp `IExternalDBApplication`.  
- Xử lý sự kiện `DocumentChanged` để theo dõi thay đổi phần tử nội thất.  
- Đăng ký và hủy đăng ký sự kiện đúng cách.  

**Mở rộng**:  
- Xử lý nhiều phần tử thay đổi bằng `IList<ElementId>`.  
- Theo dõi các danh mục khác (`OST_Walls`, `OST_Doors`).  
- Ghi log sự kiện vào tệp thay vì `TaskDialog`.  

Nội dung này hoàn thiện hướng dẫn về `IExternalDBApplication`, cung cấp nền tảng để phát triển các add-in sự kiện phức tạp hơn trong Revit API.