**5.4. Adding Icons (Thêm biểu tượng)**  

Sau khi tạo tab tùy chỉnh trên ribbon Revit, chúng ta có thể thêm biểu tượng (icon) vào các nút lệnh để dễ nhận diện. Phần này hướng dẫn cách thêm biểu tượng 32x32 pixel cho một nút lệnh trong lớp `IExternalApplication`.

**Yêu cầu về biểu tượng**:  
- Kích thước lý tưởng:  
  - 32x32 pixel cho nút thông thường.  
  - 16x16 pixel cho các điều khiển nhỏ (như dropdown).  
- Định dạng: `.PNG` được khuyến nghị.  
- Mẹo: Tạo hình ảnh ở kích thước lớn (ví dụ: 200x200 pixel) bằng phần mềm vector (như Illustrator), sau đó thu nhỏ để đảm bảo chất lượng.  
- Trong thư mục bài tập, đã có tệp `desk.png` (32x32 pixel) để sử dụng.  

**Tra cứu PushButton**:  
- Trong **Object Browser**, tìm lớp `PushButton` trong namespace `Autodesk.Revit.UI`.  
- Thuộc tính: `LargeImage` (kiểu `ImageSource`) để gán biểu tượng cho nút.  
- Lớp `BitmapImage` (namespace `System.Windows.Media.Imaging`) dùng để tải hình ảnh từ đường dẫn (URI).  

**Chỉnh sửa lớp ExternalApplication**:  
1. **Thêm tham chiếu PresentationCore**:  
   - Trong Visual Studio, nhấp chuột phải vào **References > Add Reference**.  
   - Tab **Assemblies**, tìm `PresentationCore`, chọn và nhấn **OK**.  
   - Namespace `System.Windows.Media.Imaging` sẽ cung cấp lớp `BitmapImage`.  

2. **Sửa mã trong OnStartup**:  
   - Trong phương thức `OnStartup` của lớp `ExternalApplication` (tạo từ phần 5.2), sửa dòng thêm nút để lấy đối tượng `PushButton`.  
   - Tải và gán biểu tượng cho nút.  

**Mã nguồn mẫu**:  
```csharp
using Autodesk.Revit.UI;
using System;
using System.Windows.Media.Imaging;

namespace MyRevitApps
{
    public class ExternalApplication : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            try
            {
                // Tạo ribbon tab
                application.CreateRibbonTab("MyTab");

                // Tạo ribbon panel
                RibbonPanel panel = application.CreateRibbonPanel("MyTab", "MyPanel");

                // Tạo nút lệnh
                PushButtonData buttonData = new PushButtonData("MyCommand", "My Command",
                    @"path\to\MyRevitCommands.dll", "MyRevitCommands.MyCommand");
                PushButton pushButton = panel.AddItem(buttonData) as PushButton;

                // Tải biểu tượng
                Uri imagePath = new Uri(@"C:\path\to\desk.png"); // Thay bằng đường dẫn thực tế
                BitmapImage image = new BitmapImage(imagePath);
                pushButton.LargeImage = image;

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
1. **Sửa dòng thêm nút**:  
   - Thay dòng `panel.AddItem(buttonData);` bằng:  
     - `PushButton pushButton = panel.AddItem(buttonData) as PushButton;`.  
   - `AddItem` trả về `RibbonItem`, được ép kiểu thành `PushButton` để truy cập thuộc tính `LargeImage`.  

2. **Tải biểu tượng**:  
   - Tạo `Uri` cho đường dẫn hình ảnh:  
     - Sao chép đường dẫn thư mục chứa `desk.png` (Ctrl+C, Ctrl+V).  
     - Thêm `@` trước chuỗi và nối `"\desk.png"`:  
       ```csharp
       Uri imagePath = new Uri(@"C:\path\to\desk.png");
       ```  
   - Tạo `BitmapImage`:  
     ```csharp
     BitmapImage image = new BitmapImage(imagePath);
     ```  
   - Gán biểu tượng:  
     ```csharp
     pushButton.LargeImage = image;
     ```  

3. **Thêm namespace**:  
   - Nếu Visual Studio báo lỗi thiếu `BitmapImage`, nhấp vào biểu tượng bóng đèn > **Add using System.Windows.Media.Imaging**.  

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

**Kiểm tra**:  
1. Biên dịch dự án trong Visual Studio.  
2. Sao chép `.dll` và `.addin` vào thư mục add-in của Revit (ví dụ: `%appdata%\Autodesk\Revit\Addins\202X`).  
3. Khởi động Revit, vào tab **MyTab**:  
   - Kiểm tra nút “My Command” trong panel “MyPanel” có biểu tượng `desk.png`.  
4. **Lưu ý**:  
   - Đảm bảo đường dẫn `desk.png` chính xác. Nếu không, Revit sẽ hiển thị nút mà không có biểu tượng.  
   - Để tránh lỗi đường dẫn, đặt `desk.png` cùng thư mục với `.dll` và sử dụng đường dẫn tương đối.  

**Mục tiêu**  
- Thêm biểu tượng 32x32 pixel cho nút lệnh trên ribbon.  
- Sử dụng `BitmapImage` và `Uri` để tải hình ảnh từ đường dẫn.  
- Truy cập thuộc tính `LargeImage` của `PushButton` để gán biểu tượng.  

**Mở rộng**:  
- Thêm biểu tượng 16x16 pixel cho các điều khiển nhỏ (dùng `SmallImage`).  
- Tùy chỉnh thêm kiểu nút (dropdown, radio button, list box) theo **Revit Developer Guidelines**.  
- Tìm hiểu các phương thức khác của `RibbonPanel` để tạo giao diện phức tạp hơn.  

Trong các phần tiếp theo, bạn sẽ học cách xử lý sự kiện hoặc tích hợp thêm các tính năng UI nâng cao trong Revit API.