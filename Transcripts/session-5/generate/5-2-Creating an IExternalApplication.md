Kịch bản bài học: Tạo IExternalApplication trong Revit
Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ bắt tay vào tạo một IExternalApplication để tùy chỉnh giao diện người dùng (UI) của Revit. Chúng ta sẽ tạo một lớp ExternalApplication, tổ chức dự án trong Visual Studio, và chuẩn bị tệp manifest. Sau bài học này, bạn sẽ biết:

Cách tạo và triển khai IExternalApplication trong Visual Studio.
Cách tổ chức dự án bằng thư mục Commands.
Cách tham chiếu Revit API và triển khai các phương thức OnStartup và OnShutdown.

Hãy cùng bắt đầu!

Phần 1: Tìm hiểu về IExternalApplication
Hướng dẫn viên (giọng điệu hào hứng):Trong bài trước, chúng ta đã tìm hiểu về IExternalApplication và cách nó giúp tùy chỉnh giao diện người dùng Revit. Hôm nay, chúng ta sẽ tạo một lớp triển khai giao diện này để chuẩn bị cho việc thêm các tab hoặc panel vào Ribbon. Hãy xem cách thiết lập trong Visual Studio!

Bước 1: Ôn tập IExternalApplicationIExternalApplication là giao diện cho phép plugin tương tác với giao diện Revit. Đặc điểm:

Yêu cầu triển khai hai phương thức:
OnStartup(UIControlledApplication): Chạy khi Revit khởi động, dùng để tạo Ribbon, thêm panel, hoặc đăng ký sự kiện UI.
OnShutdown(UIControlledApplication): Chạy khi Revit đóng, dùng để dọn dẹp tài nguyên.


Đăng ký trong tệp manifest với loại Application.

Hành động trên màn hình:  

Hiển thị sơ đồ:  IExternalApplication
├── OnStartup(UIControlledApplication)
│   └── Tùy chỉnh Ribbon, sự kiện UI
├── OnShutdown(UIControlledApplication)
│   └── Dọn dẹp tài nguyên
└── Manifest: Type="Application"


Chèn text: “IExternalApplication: Tùy chỉnh giao diện Revit.”


Bước 2: Tệp ManifestTệp manifest đã được cập nhật để đăng ký IExternalApplication (và tạm thời bình luận IExternalDBApplication). Cấu trúc mẫu:
<AddIn Type="Application">
    <Name>ExternalApplication</Name>
    <Assembly>MyRevitCommands.dll</Assembly>
    <FullClassName>MyRevitCommands.ExternalApplication</FullClassName>
    <ClientId>your-guid-here</ClientId>
</AddIn>
<!--
<AddIn Type="DBApplication">
    <Name>ExternalDBApplication</Name>
    <Assembly>MyRevitCommands.dll</Assembly>
    <FullClassName>MyRevitCommands.ExternalDBApplication</FullClassName>
    <ClientId>your-guid-here</ClientId>
</AddIn>
-->

Hành động trên màn hình:  

Hiển thị tệp manifest mẫu, highlight Type="Application" và phần bình luận DBApplication.  
Chèn text: “Manifest: Đăng ký IExternalApplication.”




Phần 2: Tạo IExternalApplication trong Visual Studio
Hướng dẫn viên (giọng điệu rõ ràng):Hãy mở Visual Studio để tạo lớp ExternalApplication triển khai IExternalApplication. Chúng ta cũng sẽ tổ chức dự án để dễ quản lý các lệnh đã tạo trước đó.

Bước 3: Mở dự án và tổ chức thư mục CommandsMở dự án MyRevitCommands trong Visual Studio. Để tổ chức các lệnh hiện có:

Nhấp chuột phải vào Solution trong Solution Explorer, chọn Add > New Folder, đặt tên là Commands.
Sử dụng Ctrl+Shift+Click trái để chọn tất cả các tệp lệnh (như ProjectRay.cs, PlanView.cs, v.v.).
Kéo các tệp vào thư mục Commands.

Hành động trên màn hình:  

Hiển thị Solution Explorer, nhấp chuột phải vào Solution, chọn Add > New Folder.  
Nhập tên Commands, kéo các tệp lệnh vào thư mục, highlight thông báo xác nhận.  
Chèn text: “Commands Folder: Tổ chức dự án.”


Bước 4: Tạo lớp ExternalApplicationTạo một lớp mới tên ExternalApplication:

Nhấp chuột phải vào dự án MyRevitCommands, chọn Add > New Item.
Trong Visual C# Items, chọn Class, đặt tên là ExternalApplication.cs.
Mở tệp và thêm mã để triển khai IExternalApplication:

using Autodesk.Revit.UI;

namespace MyRevitCommands
{
    public class ExternalApplication : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication app)
        {
            // Tùy chỉnh UI sẽ được thêm ở đây
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication app)
        {
            // Dọn dẹp tài nguyên sẽ được thêm ở đây
            return Result.Succeeded;
        }
    }
}

Hành động trên màn hình:  

Hiển thị Solution Explorer, nhấp chuột phải vào dự án, chọn Add > New Item.  
Chọn Class, nhập ExternalApplication.cs, highlight IExternalApplication.  
Chèn text: “ExternalApplication: Tạo lớp IExternalApplication.”


Bước 5: Thêm tham chiếu Revit APINếu Visual Studio báo lỗi thiếu IExternalApplication:

Nhấp chuột phải vào IExternalApplication trong mã, chọn Quick Actions and Refactorings.
Chọn using Autodesk.Revit.UI; để thêm thư viện.

Hành động trên màn hình:  

Highlight IExternalApplication trong mã, nhấp chuột phải, chọn Quick Actions, thêm using Autodesk.Revit.UI;.  
Chèn text: “Revit API: Thêm tham chiếu UI.”


Bước 6: Triển khai InterfaceTriển khai IExternalApplication bằng cách:

Nhấp chuột phải vào IExternalApplication trong mã, chọn Quick Actions and Refactorings.
Chọn Implement Interface để tự động tạo OnStartup và OnShutdown.

Hành động trên màn hình:  

Nhấp chuột phải vào IExternalApplication, chọn Implement Interface, highlight OnStartup và OnShutdown.  
Chèn text: “Implement Interface: Tạo phương thức OnStartup & OnShutdown.”




Phần 3: Kết luận và bước tiếp theo
Hướng dẫn viên (giọng điệu truyền cảm hứng):Chúc mừng các bạn! Hôm nay, chúng ta đã:

Tạo lớp ExternalApplication triển khai IExternalApplication.  
Tổ chức dự án bằng thư mục Commands.  
Chuẩn bị tệp manifest để đăng ký ứng dụng.

Lớp ExternalApplication là nền tảng để tùy chỉnh giao diện Revit, như thêm tab Ribbon hoặc xử lý sự kiện UI.
Bước 7: Kêu gọi hành độngTrong bài tiếp theo, chúng ta sẽ thêm một tab Ribbon và panel vào OnStartup. Hãy thử kiểm tra tệp manifest để đảm bảo ClientId là một GUID hợp lệ (tạo bằng Visual Studio: Tools > Create GUID). Nếu bạn gặp vấn đề, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay!
Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!
Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"Bài học tiếp theo: Tùy chỉnh Ribbon với CreateRibbonTab."  
"Gặp vấn đề? Để lại câu hỏi trong phần bình luận!"


Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio, mô phỏng Solution Explorer với thư mục Commands.

