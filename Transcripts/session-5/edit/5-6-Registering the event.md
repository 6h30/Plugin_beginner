---
## Kịch bản bài học: Đăng ký sự kiện trong Revit
---

### Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ hoàn thiện lớp **ExternalDBApp** bằng cách **đăng ký sự kiện DocumentChanged** trong `OnStartup` và **hủy đăng ký** trong `OnShutdown`. Chúng ta cũng sẽ cập nhật tệp manifest để kiểm thử plugin, theo dõi thay đổi của phần tử **Furniture**. Sau bài học này, bạn sẽ:

* Biết cách đăng ký và hủy đăng ký sự kiện với **EventHandler**.
* Hiểu cách sử dụng `try-catch` để xử lý lỗi khi đăng ký sự kiện.
* Biết cách kiểm tra và cập nhật tệp manifest cho **IExternalDBApplication**.

Hãy cùng bắt đầu!

---
## Phần 1: Tìm hiểu về đăng ký sự kiện trong Revit
---

Hướng dẫn viên (giọng điệu hào hứng): Trong bài trước, chúng ta đã tạo phương thức `ElementChangedEvent` để xử lý sự kiện **DocumentChanged**. Hôm nay, chúng ta sẽ đăng ký sự kiện này để nó thực thi mỗi khi tài liệu Revit thay đổi, và hủy đăng ký khi Revit đóng. Hãy khám phá cách kết nối sự kiện với plugin của chúng ta!

### Bước 1: Đăng ký và hủy đăng ký sự kiện
Trong **IExternalDBApplication**:

* `OnStartup(ControlledApplication)`: Đăng ký sự kiện bằng `+=` để liên kết **EventHandler** với phương thức xử lý (như `ElementChangedEvent`).
* `OnShutdown(ControlledApplication)`: Hủy đăng ký bằng `-=` để tránh thực thi ngoài ý muốn.
* **EventHandler**: Đại diện tham chiếu đến phương thức xử lý, đảm bảo khớp với chữ ký sự kiện (`object sender`, `DocumentChangedEventArgs args`).

Sử dụng `try-catch` trong `OnStartup` để xử lý lỗi khi đăng ký sự kiện, trả về `ExternalDBApplicationResult.Failed` nếu lỗi xảy ra.

Hành động trên màn hình:  

Hiển thị sơ đồ:
```
IExternalDBApplication
├── OnStartup
│   └── app.DocumentChanged += EventHandler<DocumentChangedEventArgs>(Method)
├── OnShutdown
│   └── app.DocumentChanged -= EventHandler<DocumentChangedEventArgs>(Method)
└── Result: ExternalDBApplicationResult.Succeeded/Failed
```

Chèn text: “**Event Registration**: Liên kết sự kiện DocumentChanged.”

### Bước 2: Manifest cho IExternalDBApplication
Tệp manifest cần chứa mục **DBApplication** để đăng ký **ExternalDBApp**. Cần bỏ bình luận và kiểm tra `FullClassName` khớp với tên lớp.

Hành động trên màn hình:  

Hiển thị tệp manifest mẫu:
```xml
<AddIn Type="DBApplication">
    <Name>ExternalDBApp</Name>
    <Assembly>MyRevitCommands.dll</Assembly>
    <FullClassName>MyRevitCommands.ExternalDBApp</FullClassName>
    <ClientId>your-guid-here</ClientId>
</AddIn>
```

Chèn text: “**Manifest**: Đăng ký ExternalDBApp.”

---
## Phần 2: Đăng ký sự kiện trong Visual Studio
---

Hướng dẫn viên (giọng điệu rõ ràng): Hãy mở Visual Studio để chỉnh sửa lớp **ExternalDBApp**, đăng ký sự kiện **DocumentChanged** trong `OnStartup`, hủy đăng ký trong `OnShutdown`, và cập nhật manifest.

### Bước 3: Mở tệp ExternalDBApp.cs
Mở tệp `ExternalDBApp.cs` trong dự án `MyRevitCommands`. Mã hiện tại từ bài trước:

```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System.Linq;
using System; // Required for NotImplementedException, EventHandler

namespace MyRevitCommands
{
    public class ExternalDBApp : IExternalDBApplication
    {
        public ExternalDBApplicationResult OnStartup(ControlledApplication app)
        {
            throw new NotImplementedException();
        }

        public ExternalDBApplicationResult OnShutdown(ControlledApplication app)
        {
            throw new NotImplementedException();
        }

        public void ElementChangedEvent(object sender, DocumentChangedEventArgs args)
        {
            ElementFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Furniture);
            // Added check for Any() before First() to prevent errors if no furniture elements are modified
            if (args.GetModifiedElementIds(filter).Any())
            {
                ElementId element = args.GetModifiedElementIds(filter).First();
                string name = args.GetTransactionNames().First();
                TaskDialog.Show("Modified Element", $"{element.ToString()} changed by {name}");
            }
        }
    }
}
```

Hành động trên màn hình:  

Hiển thị **Solution Explorer**, nhấp vào `ExternalDBApp.cs`.  
Zoom vào `OnStartup` và `OnShutdown`, highlight `throw new NotImplementedException`.  
Chèn text: “**ExternalDBApp**: Điểm khởi đầu để đăng ký sự kiện.”

### Bước 4: Đăng ký sự kiện trong OnStartup
Sửa `OnStartup` để đăng ký **DocumentChanged** trong khối `try-catch`:

```csharp
public ExternalDBApplicationResult OnStartup(ControlledApplication app)
{
    try
    {
        app.DocumentChanged += new EventHandler<DocumentChangedEventArgs>(ElementChangedEvent);
        return ExternalDBApplicationResult.Succeeded;
    }
    catch (Exception ex) // Catch specific exception for better error handling
    {
        // Optionally log the exception: Console.WriteLine(ex.Message);
        return ExternalDBApplicationResult.Failed;
    }
}
```

* `try-catch`: Xử lý lỗi khi đăng ký sự kiện.
* `app.DocumentChanged +=`: Liên kết `ElementChangedEvent` với sự kiện.
* `ExternalDBApplicationResult`: Trả về `Succeeded` nếu thành công, `Failed` nếu lỗi.

Hành động trên màn hình:  

Xóa `throw` trong `OnStartup`, nhập mã `try-catch`, highlight `DocumentChanged +=`.  
Chèn text: “**OnStartup**: Đăng ký DocumentChanged.”

### Bước 5: Hủy đăng ký sự kiện trong OnShutdown
Sửa `OnShutdown` để hủy đăng ký **DocumentChanged**:

```csharp
public ExternalDBApplicationResult OnShutdown(ControlledApplication app)
{
    app.DocumentChanged -= new EventHandler<DocumentChangedEventArgs>(ElementChangedEvent);
    return ExternalDBApplicationResult.Succeeded;
}
```

* `app.DocumentChanged -=`: Hủy liên kết `ElementChangedEvent`.
* `ExternalDBApplicationResult.Succeeded`: Trả về kết quả thành công.

Mã hoàn chỉnh:

```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System.Linq;
using System; // For EventHandler, Exception, and ExternalDBApplicationResult

namespace MyRevitCommands
{
    public class ExternalDBApp : IExternalDBApplication
    {
        public ExternalDBApplicationResult OnStartup(ControlledApplication app)
        {
            try
            {
                app.DocumentChanged += new EventHandler<DocumentChangedEventArgs>(ElementChangedEvent);
                return ExternalDBApplicationResult.Succeeded;
            }
            catch (Exception ex)
            {
                // Optionally log the exception here. For example:
                // System.Diagnostics.Debug.WriteLine($"Error registering DocumentChanged event: {ex.Message}");
                return ExternalDBApplicationResult.Failed;
            }
        }

        public ExternalDBApplicationResult OnShutdown(ControlledApplication app)
        {
            app.DocumentChanged -= new EventHandler<DocumentChangedEventArgs>(ElementChangedEvent);
            return ExternalDBApplicationResult.Succeeded;
        }

        public void ElementChangedEvent(object sender, DocumentChangedEventArgs args)
        {
            ElementFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Furniture);
            if (args.GetModifiedElementIds(filter).Any())
            {
                ElementId element = args.GetModifiedElementIds(filter).First();
                string name = args.GetTransactionNames().First();
                TaskDialog.Show("Modified Element", $"{element.ToString()} changed by {name}");
            }
        }
    }
}
```

Hành động trên màn hình:  

Xóa `throw` trong `OnShutdown`, nhập mã hủy đăng ký, highlight `DocumentChanged -=`.  
Chèn text: “**OnShutdown**: Hủy đăng ký DocumentChanged.”

### Bước 6: Cập nhật Manifest
Chỉnh sửa tệp manifest (`.addin`) để đảm bảo **DBApplication** được kích hoạt và `FullClassName` khớp với **ExternalDBApp**:

```xml
<?xml version="1.0" encoding="utf-8"?>
<RevitAddIns>
    <AddIn Type="Application">
        <Name>ExternalApplication</Name>
        <Assembly>MyRevitCommands.dll</Assembly>
        <FullClassName>MyRevitCommands.ExternalApplication</FullClassName>
        <ClientId>your-guid-for-ExternalApplication</ClientId>
    </AddIn>
    <AddIn Type="DBApplication">
        <Name>ExternalDBApp</Name>
        <Assembly>MyRevitCommands.dll</Assembly>
        <FullClassName>MyRevitCommands.ExternalDBApp</FullClassName>
        <ClientId>your-guid-for-ExternalDBApp</ClientId>
    </AddIn>
</RevitAddIns>
```

* Bỏ bình luận mục **DBApplication** (xóa thẻ ``).
* Cập nhật `FullClassName` thành `MyRevitCommands.ExternalDBApp`.

Hành động trên màn hình:  

Hiển thị **Solution Explorer**, mở `MyRevitCommands.addin`, bỏ bình luận **DBApplication**, highlight **ExternalDBApp**.  
Chèn text: “**Manifest**: Kích hoạt ExternalDBApp.”

---
## Phần 3: Đăng ký và kiểm tra
---

Hướng dẫn viên (giọng điệu khích lệ): Plugin **ExternalDBApp** đã sẵn sàng với sự kiện **DocumentChanged** được đăng ký. Hãy kiểm tra nó trong Revit!

### Bước 7: Chạy Debug và kiểm tra
Trong Visual Studio, nhấn **F5** để chạy debug mode. Mở Revit, mở tệp bài tập của video này, chứa các phần tử **Furniture**. Thực hiện thay đổi:

* Chọn một phần tử **Furniture** và sử dụng lệnh **Move**.
* Kết quả: **TaskDialog** hiển thị ID phần tử (ví dụ: 217813) và tên giao dịch (ví dụ: "Move").

Hành động trên màn hình:  

Hiển thị Visual Studio, nhấn **F5**.  
Trong Revit, mở tệp bài tập, chọn Furniture, dùng lệnh Move, highlight **TaskDialog** với ID và tên giao dịch.  
Chèn text: “**Kiểm tra**: TaskDialog thông báo Furniture thay đổi.”

---
## Phần 4: Kết luận và bước tiếp theo
---

Hướng dẫn viên (giọng điệu truyền cảm hứng): Chúc mừng các bạn! Hôm nay, chúng ta đã:

* Đăng ký sự kiện **DocumentChanged** trong `OnStartup` với `try-catch`.  
* Hủy đăng ký sự kiện trong `OnShutdown` để quản lý tài nguyên.  
* Cập nhật manifest để kích hoạt **ExternalDBApp**.

Sự kiện **DocumentChanged** là một công cụ mạnh mẽ để tự động hóa quy trình trong Revit, từ theo dõi thay đổi đến tích hợp với quy trình bên ngoài.

### Bước 8: Kêu gọi hành động
Hãy thử mở rộng plugin để theo dõi nhiều phần tử **Furniture** (sử dụng `List`) hoặc đăng ký sự kiện khác như **DocumentSaving**. Tham khảo **Revit API Docs** để khám phá các sự kiện DB khác. Nếu bạn gặp vấn đề, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay!

Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!

Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"**Bài học tiếp theo: Tự động hóa quy trình với sự kiện DB.**"  
"**Gặp vấn đề? Để lại câu hỏi trong phần bình luận!**"

Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio, mô phỏng **TaskDialog** với thông báo sự kiện.