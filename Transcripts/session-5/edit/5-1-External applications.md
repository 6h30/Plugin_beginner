---
## Kịch bản bài học: Ứng dụng bên ngoài trong Revit
---

### Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ khám phá một cách mới để tương tác với Revit API: **External Applications**. Khác với các External Commands đã học, các ứng dụng bên ngoài cho phép thực thi mã dựa trên sự kiện hoặc tích hợp với giao diện người dùng. Sau bài học này, bạn sẽ biết:

* Sự khác biệt giữa **IExternalApplication** và **IExternalDBApplication**.
* Cách **OnStartup** và **OnShutdown** hoạt động.
* Cách đăng ký ứng dụng bên ngoài trong tệp manifest.

Hãy cùng bắt đầu!

---
## Phần 1: Tìm hiểu về External Applications trong Revit API
---

Hướng dẫn viên (giọng điệu hào hứng): Trong suốt khóa học, chúng ta đã tạo các External Commands để thực thi các tác vụ cụ thể trong Revit. Hôm nay, chúng ta sẽ học một cách mạnh mẽ hơn để tích hợp plugin: **External Applications**. Đây là cách để tùy chỉnh giao diện người dùng hoặc theo dõi sự kiện trong Revit. Hãy khám phá!

### Bước 1: External Applications là gì?
External Applications là các plugin Revit sử dụng hai giao diện chính:

* **IExternalApplication**: Tích hợp với giao diện người dùng (UI) và xử lý các sự kiện UI, như hiển thị hộp thoại, kích hoạt view, hoặc khi Revit ở trạng thái Idle.
* **IExternalDBApplication**: Xử lý các sự kiện cấp cơ sở dữ liệu (DB), như mở tài liệu, lưu tài liệu, hoặc đồng bộ với tệp trung tâm (Central).

Cả hai giao diện đều yêu cầu triển khai hai phương thức:

* **OnStartup**: Gọi khi Revit khởi động, nhận **UIControlledApplication** (cho **IExternalApplication**) hoặc **ControlledApplication** (cho **IExternalDBApplication**).
* **OnShutdown**: Gọi khi Revit đóng, nhận cùng đối tượng ứng dụng.

Hành động trên màn hình:  

Hiển thị sơ đồ:
```
External Applications
├── IExternalApplication
│   ├── OnStartup(UIControlledApplication)
│   ├── OnShutdown(UIControledApplication)
│   └── UI Events: DialogBoxShowing, ViewActivating, Idling
└── IExternalDBApplication
    ├── OnStartup(ControlledApplication)
    ├── OnShutdown(ControlledApplication)
    └── DB Events: DocumentOpening, DocumentSaving, Synchronizing
```

Chèn text: “**External Applications**: Tích hợp UI và DB.”

### Bước 2: UIControlledApplication và ControlledApplication  

* **UIControlledApplication** (dùng trong **IExternalApplication**):
    * Cung cấp các phương thức để tùy chỉnh giao diện, như `CreateRibbonTab`, `CreateRibbonPanel`, hoặc `AddStackedItems`.
    * Theo dõi sự kiện UI, như khi hộp thoại xuất hiện hoặc view được kích hoạt.

* **ControlledApplication** (dùng trong **IExternalDBApplication**):
    * Theo dõi sự kiện cơ sở dữ liệu, như mở tài liệu hoặc lưu tài liệu.
    * Cung cấp quyền truy cập tài liệu qua đối số sự kiện.

Hành động trên màn hình:  

Hiển thị mã giả lập:
```csharp
public class MyApp : IExternalApplication
{
    public Result OnStartup(UIControlledApplication app)
    {
        app.CreateRibbonTab("MyTab");
        return Result.Succeeded;
    }
    public Result OnShutdown(UIControlledApplication app)
    {
        return Result.Succeeded;
    }
}
```

Chèn text: “**UIControlledApplication**: Tùy chỉnh Ribbon.”

### Bước 3: Đăng ký trong Manifest
Để Revit nhận diện External Applications, cần đăng ký trong tệp manifest (.addin) với hai loại:

* **Application**: Cho **IExternalApplication**.
* **DBApplication**: Cho **IExternalDBApplication**.

Cấu trúc manifest tương tự ExternalCommand, nhưng sử dụng thẻ khác:
```xml
<AddIn Type="Application">
    <Name>MyApp</Name>
    <Assembly>MyApp.dll</Assembly>
    <FullClassName>MyRevitCommands.MyApp</FullClassName>
    <ClientId>your-guid-here</ClientId>
</AddIn>
```

Hành động trên màn hình:  

Hiển thị tệp manifest mẫu, highlight thẻ **Application**.  
Chèn text: “**Manifest**: Đăng ký External Application.”

---
## Phần 2: Chuẩn bị tạo External Application trong Visual Studio
---

Hướng dẫn viên (giọng điệu rõ ràng): Trong bài này, chúng ta sẽ tìm hiểu lý thuyết và chuẩn bị cho việc tạo một External Application. Hãy mở Visual Studio để xem cấu trúc cơ bản của một ứng dụng bên ngoài!

### Bước 4: Mở dự án và tạo lớp MyApp
Mở dự án `MyRevitCommands` trong Visual Studio. Tạo một lớp mới tên `MyApp` triển khai **IExternalApplication**:

```csharp
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System; // Thêm namespace này cho Result và Exception

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)] // Thuộc tính này thường không cần cho IExternalApplication, chỉ cho IExternalCommand
    public class MyApp : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication app)
        {
            // Tùy chỉnh UI tại đây
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication app)
        {
            // Dọn dẹp tại đây
            return Result.Succeeded;
        }
    }
}
```

Hành động trên màn hình:  

Hiển thị Solution Explorer, nhấp chuột phải vào dự án, chọn **Add > New Item > Class**, đặt tên `MyApp.cs`.  
Nhập mã **IExternalApplication**, highlight `OnStartup` và `OnShutdown`.  
Chèn text: “**MyApp**: Cấu trúc **IExternalApplication**.”

### Bước 5: Giải thích OnStartup và OnShutdown  

* **OnStartup**: 
    * Được gọi khi Revit khởi động.
    * Sử dụng **UIControlledApplication** để tạo tab Ribbon, thêm panel, hoặc đăng ký sự kiện UI.

* **OnShutdown**: 
    * Được gọi khi Revit đóng.
    * Dùng để dọn dẹp tài nguyên hoặc lưu trạng thái.

Hành động trên màn hình:  

Highlight `UIControlledApplication app` trong `OnStartup`.  
Chèn text: “**OnStartup**: Tùy chỉnh khi Revit khởi động.”

### Bước 6: Chuẩn bị Manifest
Tạo hoặc chỉnh sửa tệp manifest (.addin) để đăng ký `MyApp`:

```xml
<?xml version="1.0" encoding="utf-8"?>
<RevitAddIns>
    <AddIn Type="Application">
        <Name>My External App</Name>
        <Assembly>MyRevitCommands.dll</Assembly>
        <FullClassName>MyRevitCommands.MyApp</FullClassName>
        <ClientId>your-guid-here</ClientId>
    </AddIn>
</RevitAddIns>
```

Lưu ý: Thay `your-guid-here` bằng một GUID hợp lệ (tạo bằng Visual Studio hoặc công cụ trực tuyến).

Hành động trên màn hình:  

Hiển thị Solution Explorer, nhấp vào tệp `MyRevitCommands.addin`.  
Nhập XML, highlight `Type="Application"`.  
Chèn text: “**Manifest**: Đăng ký MyApp.”

---
## Phần 3: Kết luận và bước tiếp theo
---

Hướng dẫn viên (giọng điệu truyền cảm hứng): Chúc mừng các bạn! Hôm nay, chúng ta đã:

* Tìm hiểu **IExternalApplication** và **IExternalDBApplication**.  
* Hiểu cách **OnStartup** và **OnShutdown** hoạt động với **UIControlledApplication** và **ControlledApplication**.  
* Chuẩn bị cấu trúc cho một External Application và đăng ký trong manifest.

External Applications mở ra khả năng tùy chỉnh mạnh mẽ, từ giao diện người dùng đến sự kiện cơ sở dữ liệu.

### Bước 7: Kêu gọi hành động
Trong các bài tiếp theo, chúng ta sẽ tạo một External Application thực tế, như thêm tab Ribbon hoặc theo dõi sự kiện **DocumentOpening**. Hãy thử tạo một lớp `MyDBApp` triển khai **IExternalDBApplication** và đăng ký nó trong manifest. Nếu bạn gặp vấn đề, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay!

Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!

Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"**Bài học tiếp theo: Tùy chỉnh Ribbon với IExternalApplication.**"  
"**Gặp vấn đề? Để lại câu hỏi trong phần bình luận!**"

Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio, mô phỏng giao diện Ribbon hoặc sự kiện Revit.