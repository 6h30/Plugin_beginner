---
## Kịch bản bài học: Tùy chỉnh giao diện người dùng trong Revit
---

### Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ **tùy chỉnh giao diện người dùng (UI) của Revit** bằng cách thêm một tab Ribbon, một panel, và một nút để thực thi lệnh **PlaceFamily** đã tạo trước đó. Chúng ta sẽ sử dụng lớp **ExternalApplication** triển khai **IExternalApplication** từ bài trước. Sau bài học này, bạn sẽ biết:

* Cách tạo tab Ribbon bằng `CreateRibbonTab`.
* Cách thêm panel và nút bằng `CreateRibbonPanel` và `PushButtonData`.
* Cách liên kết nút với lệnh hiện có mà không cần đăng ký thêm trong manifest.

Hãy cùng bắt đầu!

---
## Phần 1: Tìm hiểu về tùy chỉnh giao diện Revit
---

Hướng dẫn viên (giọng điệu hào hứng): Trong bài trước, chúng ta đã tạo lớp **ExternalApplication** triển khai **IExternalApplication**. Hôm nay, chúng ta sẽ sử dụng nó để thêm một tab Ribbon mới, chứa một panel và một nút để chạy lệnh **PlaceFamily**. Hãy khám phá cách tùy chỉnh UI trong Revit API!

### Bước 1: Tùy chỉnh Ribbon với UIControlledApplication
**UIControlledApplication** trong `OnStartup` cung cấp các phương thức để tùy chỉnh giao diện:

* `CreateRibbonTab`: Tạo tab mới trên Ribbon.
* `CreateRibbonPanel`: Tạo panel trong tab.
* `PushButtonData`: Tạo nút liên kết với lệnh (**IExternalCommand**).
* `RibbonPanel.AddItem`: Thêm nút vào panel.

Hành động trên màn hình:  

Hiển thị sơ đồ:
```
UIControlledApplication
├── CreateRibbonTab: Tạo tab
├── CreateRibbonPanel: Tạo panel
└── PushButtonData: Tạo nút
    └── Liên kết: IExternalCommand
```

Chèn text: “**UIControlledApplication**: Tùy chỉnh Ribbon.”

### Bước 2: PushButtonData và liên kết lệnh
**PushButtonData** tạo nút với các tham số:

* `Name`: Tên nội bộ của nút.
* `Text`: Tên hiển thị trên UI.
* `AssemblyPath`: Đường dẫn đến tệp `.dll` chứa lệnh.
* `ClassName`: Tên đầy đủ của lớp lệnh (ví dụ: `MyRevitCommands.PlaceFamily`).

Nút có thể liên kết với các lệnh **IExternalCommand** hiện có, giúp di chuyển chúng từ **External Tools** sang **Ribbon**.

Hành động trên màn hình:  

Hiển thị mã giả lập:
```csharp
PushButtonData button = new PushButtonData("Button1", "PlaceFamily", assemblyPath, "MyRevitCommands.PlaceFamily");
RibbonPanel panel = app.CreateRibbonPanel("My Commands", "Commands");
panel.AddItem(button); // Changed from AddFilter to AddItem
```

Chèn text: “**PushButtonData**: Tạo nút liên kết lệnh.”

---
## Phần 2: Tùy chỉnh UI trong Visual Studio
---

Hướng dẫn viên (giọng điệu rõ ràng): Hãy mở Visual Studio để chỉnh sửa lớp **ExternalApplication**, thêm tab **My Commands**, panel **Commands**, và nút **PlaceFamily** liên kết với lệnh **PlaceFamily**.

### Bước 3: Mở tệp ExternalApplication.cs
Mở tệp `ExternalApplication.cs` trong dự án `MyRevitCommands`. Mã hiện tại có `OnStartup` và `OnShutdown` đang ném ngoại lệ (`throw new NotImplementedException`):

```csharp
using Autodesk.Revit.UI;
using System; // Make sure System is included for NotImplementedException

namespace MyRevitCommands
{
    public class ExternalApplication : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication app)
        {
            throw new NotImplementedException();
        }

        public Result OnShutdown(UIControlledApplication app)
        {
            throw new NotImplementedException();
        }
    }
}
```

Hành động trên màn hình:  

Hiển thị **Solution Explorer**, nhấp vào `ExternalApplication.cs`.  
Zoom vào `OnStartup` và `OnShutdown`, highlight `throw new NotImplementedException`.  
Chèn text: “**ExternalApplication**: Điểm khởi đầu để tùy chỉnh UI.”

### Bước 4: Sửa OnShutdown
Vì `OnShutdown` không cần xử lý sự kiện, chỉ trả về `Result.Succeeded`:

```csharp
public Result OnShutdown(UIControlledApplication app)
{
    return Result.Succeeded;
}
```

Hành động trên màn hình:  

Thay `throw` bằng `return Result.Succeeded`, highlight `Result.Succeeded`.  
Chèn text: “**OnShutdown**: Trả về kết quả thành công.”

### Bước 5: Tạo RibbonTab trong OnStartup
Trong `OnStartup`, xóa ngoại lệ và thêm tab **My Commands**:

```csharp
app.CreateRibbonTab("My Commands");
```

Hành động trên màn hình:  

Xóa `throw` trong `OnStartup`, nhập `CreateRibbonTab`, highlight `"My Commands"`.  
Chèn text: “`CreateRibbonTab`: Tạo tab My Commands.”

### Bước 6: Tạo PushButtonData cho PlaceFamily
Thêm thư viện `System.Reflection` và tạo **PushButtonData** để liên kết với lệnh **PlaceFamily**:

```csharp
using System.Reflection; // Add this using directive
// ...
string path = Assembly.GetExecutingAssembly().Location;
PushButtonData button = new PushButtonData("Button1", "PlaceFamily", path, "MyRevitCommands.Commands.PlaceFamily");
// Note: Class name includes the "Commands" namespace if you moved the class there
```

* `GetExecutingAssembly().Location`: Lấy đường dẫn tệp `.dll` hiện tại.
* **PushButtonData**: 
    * `Button1`: Tên nội bộ.
    * `PlaceFamily`: Tên hiển thị.
    * `path`: Đường dẫn `.dll`.
    * `MyRevitCommands.Commands.PlaceFamily`: Tên đầy đủ của lớp lệnh (đã bao gồm thư mục `Commands`).

Hành động trên màn hình:  

Thêm `using System.Reflection;`, nhập mã **PushButtonData**, highlight `"MyRevitCommands.Commands.PlaceFamily"`.  
Chèn text: “**PushButtonData**: Tạo nút PlaceFamily.”

### Bước 7: Tạo RibbonPanel và thêm nút
Tạo panel **Commands** trong tab **My Commands** và thêm nút:

```csharp
RibbonPanel panel = app.CreateRibbonPanel("My Commands", "Commands");
panel.AddItem(button); // Corrected from AddFilter to AddItem
```

* `CreateRibbonPanel`: Tạo panel trong tab đã chỉ định.
* `AddItem`: Thêm nút vào panel.

Hành động trên màn hình:  

Nhập mã `CreateRibbonPanel` và `AddItem`, highlight `"Commands"`.  
Chèn text: “**RibbonPanel**: Thêm panel và nút.”

### Bước 8: Hoàn thiện OnStartup
Trả về `Result.Succeeded` để hoàn thành `OnStartup`. Mã hoàn chỉnh:

```csharp
using Autodesk.Revit.UI;
using System.Reflection;
using System; // For Result.Succeeded

namespace MyRevitCommands
{
    public class ExternalApplication : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication app)
        {
            app.CreateRibbonTab("My Commands");

            string path = Assembly.GetExecutingAssembly().Location;
            PushButtonData button = new PushButtonData("Button1", "PlaceFamily", path, "MyRevitCommands.Commands.PlaceFamily");

            RibbonPanel panel = app.CreateRibbonPanel("My Commands", "Commands");
            panel.AddItem(button);

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication app)
        {
            return Result.Succeeded;
        }
    }
}
```

Hành động trên màn hình:  

Thêm `return Result.Succeeded`, highlight toàn bộ mã `OnStartup`.  
Chèn text: “**OnStartup**: Hoàn thiện tùy chỉnh UI.”

---
## Phần 3: Đăng ký và kiểm tra
---

Hướng dẫn viên (giọng điệu khích lệ): Lệnh **ExternalApplication** đã được đăng ký trong tệp manifest từ bài trước. Hãy kiểm tra tab Ribbon mới trong Revit!

### Bước 9: Chạy Debug và kiểm tra
Trong Visual Studio, nhấn **F5** để chạy debug mode. Mở Revit, mở tệp bài tập của video này. Kết quả:

* Tab **My Commands** xuất hiện trên Ribbon.
* Panel **Commands** chứa nút **PlaceFamily**.
* Nhấp nút **PlaceFamily**, chuyển đến view Ground Floor, và kiểm tra lệnh hoạt động như đã tạo trước đó.

Hành động trên màn hình:  

Hiển thị Visual Studio, nhấn **F5**.  
Trong Revit, highlight tab **My Commands**, nhấp nút **PlaceFamily**, zoom vào Ground Floor để kiểm tra.  
Chèn text: “**Kiểm tra**: Tab My Commands và nút PlaceFamily.”

---
## Phần 4: Kết luận và bước tiếp theo
---

Hướng dẫn viên (giọng điệu truyền cảm hứng): Chúc mừng các bạn! Hôm nay, chúng ta đã:

* Tùy chỉnh UI Revit bằng cách thêm tab **My Commands** và panel **Commands**.  
* Tạo nút **PlaceFamily** liên kết với lệnh hiện có.  
* Sử dụng **PushButtonData** và `CreateRibbonPanel` để tích hợp lệnh vào Ribbon.

Tùy chỉnh Ribbon giúp tổ chức các lệnh tùy chỉnh một cách chuyên nghiệp, giảm sự phụ thuộc vào **External Tools**.

### Bước 10: Kêu gọi hành động
Hãy thử thêm các nút khác cho các lệnh như **PlanView** hoặc **TagView**. Hoặc tùy chỉnh nút với biểu tượng bằng `PushButton.SetLargeImage`. Nếu bạn gặp vấn đề, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay!

Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!

Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"**Bài học tiếp theo: Xử lý sự kiện với IExternalApplication.**"  
"**Gặp vấn đề? Để lại câu hỏi trong phần bình luận!**"

Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio, mô phỏng tab **My Commands** trên Ribbon.