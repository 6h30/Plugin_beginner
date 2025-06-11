Kịch bản bài học: IExternalDBApplication và Sự kiện trong Revit
Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ khám phá IExternalDBApplication, một cách để theo dõi các sự kiện cơ sở dữ liệu trong Revit mà không liên quan đến giao diện người dùng. Chúng ta sẽ tạo một lớp ExternalDBApp để đăng ký sự kiện DocumentChanged, theo dõi các thay đổi của phần tử Furniture, và hiển thị thông báo qua TaskDialog. Sau bài học này, bạn sẽ biết:

Cách tạo và triển khai IExternalDBApplication.
Cách đăng ký và xử lý sự kiện DocumentChanged.
Cách sử dụng ElementCategoryFilter và TaskDialog để thông báo thay đổi.

Hãy cùng bắt đầu!

Phần 1: Tìm hiểu về IExternalDBApplication và Sự kiện
Hướng dẫn viên (giọng điệu hào hứng):Trong các bài trước, chúng ta đã tùy chỉnh giao diện Revit với IExternalApplication. Hôm nay, chúng ta sẽ tập trung vào IExternalDBApplication, một công cụ mạnh mẽ để theo dõi các sự kiện cơ sở dữ liệu như mở tài liệu hoặc thay đổi phần tử. Hãy khám phá cách hoạt động của nó!

Bước 1: IExternalDBApplication là gì?IExternalDBApplication là giao diện cho phép plugin xử lý các sự kiện cấp cơ sở dữ liệu (DB) trong Revit, không liên quan đến giao diện người dùng. Đặc điểm:

Triển khai hai phương thức:
OnStartup(ControlledApplication): Chạy khi Revit khởi động, dùng để đăng ký sự kiện DB.
OnShutdown(ControlledApplication): Chạy khi Revit đóng, dùng để hủy đăng ký sự kiện.


ControlledApplication: Cung cấp quyền truy cập các sự kiện DB như DocumentOpening, DocumentSaving, hoặc DocumentChanged.

Trong bài này, chúng ta sẽ đăng ký sự kiện DocumentChanged để theo dõi thay đổi của phần tử Furniture.
Hành động trên màn hình:  

Hiển thị sơ đồ:  IExternalDBApplication
├── OnStartup(ControlledApplication)
│   └── Đăng ký sự kiện: DocumentChanged, DocumentOpening...
├── OnShutdown(ControlledApplication)
│   └── Hủy đăng ký sự kiện
└── Events: DocumentChanged, DocumentSaving...


Chèn text: “IExternalDBApplication: Theo dõi sự kiện DB.”


Bước 2: Sự kiện DocumentChangedSự kiện DocumentChanged kích hoạt khi tài liệu Revit thay đổi (thêm, sửa, xóa phần tử). Để xử lý:

Tạo một Event Handler với các tham số:
object sender: Tài liệu xảy ra sự kiện.
DocumentChangedEventArgs args: Chứa thông tin về thay đổi (như GetModifiedElementIds).


Sử dụng ElementFilter (như ElementCategoryFilter) để lọc phần tử cụ thể (ví dụ: OST_Furniture).
Hiển thị thông tin qua TaskDialog.

Hành động trên màn hình:  

Hiển thị mã giả lập:  public void ElementChangedEvent(object sender, DocumentChangedEventArgs args)
{
    ElementFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Furniture);
    ElementId element = args.GetModifiedElementIds(filter).First();
    string name = args.GetTransactionNames().First();
    TaskDialog.Show("Modified Element", $"{element.ToString()} changed by {name}");
}


Chèn text: “DocumentChanged: Theo dõi thay đổi Furniture.”




Phần 2: Tạo IExternalDBApplication trong Visual Studio
Hướng dẫn viên (giọng điệu rõ ràng):Hãy mở Visual Studio để tạo lớp ExternalDBApp triển khai IExternalDBApplication, đăng ký sự kiện DocumentChanged, và hiển thị thông báo khi phần tử Furniture thay đổi.

Bước 3: Tạo lớp ExternalDBAppMở dự án MyRevitCommands trong Visual Studio. Tạo lớp mới:

Nhấp chuột phải vào Solution trong Solution Explorer, chọn Add > New Item.
Trong Visual C# Items, chọn Class, đặt tên là ExternalDBApp.cs.
Mở tệp và thêm mã để triển khai IExternalDBApplication:

using Autodesk.Revit.DB;

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
    }
}

Hành động trên màn hình:  

Hiển thị Solution Explorer, nhấp chuột phải vào Solution, chọn Add > New Item.  
Chọn Class, nhập ExternalDBApp.cs, highlight IExternalDBApplication.  
Chèn text: “ExternalDBApp: Tạo lớp IExternalDBApplication.”


Bước 4: Thêm tham chiếu Revit APINếu Visual Studio báo lỗi thiếu IExternalDBApplication:

Nhấp chuột phải vào IExternalDBApplication, chọn Quick Actions and Refactorings.
Chọn using Autodesk.Revit.DB;.

Tiếp theo, triển khai giao diện:

Nhấp chuột phải vào IExternalDBApplication, chọn Quick Actions and Refactorings.
Chọn Implement Interface để tạo OnStartup và OnShutdown.

Hành động trên màn hình:  

Highlight IExternalDBApplication, nhấp chuột phải, thêm using Autodesk.Revit.DB;.  
Chọn Implement Interface, highlight OnStartup và OnShutdown.  
Chèn text: “Revit API: Thêm tham chiếu và triển khai giao diện.”


Bước 5: Tạo Event Handler cho DocumentChangedThêm phương thức Event Handler để xử lý sự kiện DocumentChanged:
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System.Linq;

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
            ElementId element = args.GetModifiedElementIds(filter).First();
            string name = args.GetTransactionNames().First();
            TaskDialog.Show("Modified Element", $"{element.ToString()} changed by {name}");
        }
    }
}


ElementCategoryFilter: Lọc phần tử OST_Furniture.
GetModifiedElementIds: Lấy ID của phần tử Furniture đã thay đổi (chỉ lấy đầu tiên để đơn giản).
GetTransactionNames: Lấy tên giao dịch gây ra thay đổi.
TaskDialog: Hiển thị ID phần tử và tên giao dịch.
Thêm using Autodesk.Revit.DB.Events;, using Autodesk.Revit.UI;, và using System.Linq; cho First.

Hành động trên màn hình:  

Nhập mã ElementChangedEvent, highlight OST_Furniture và TaskDialog.Show.  
Thêm các using cần thiết, chèn text: “Event Handler: Theo dõi Furniture thay đổi.”


Bước 6: Đăng ký sự kiện trong OnStartupTrong OnStartup, đăng ký sự kiện DocumentChanged:
public ExternalDBApplicationResult OnStartup(ControlledApplication app)
{
    app.DocumentChanged += new EventHandler<DocumentChangedEventArgs>(ElementChangedEvent);
    return ExternalDBApplicationResult.Succeeded;
}


DocumentChanged +=: Đăng ký ElementChangedEvent cho sự kiện.
ExternalDBApplicationResult.Succeeded: Trả về kết quả thành công.

Hành động trên màn hình:  

Xóa throw trong OnStartup, nhập mã đăng ký sự kiện, highlight DocumentChanged.  
Chèn text: “OnStartup: Đăng ký DocumentChanged.”


Bước 7: Hủy đăng ký sự kiện trong OnShutdownTrong OnShutdown, hủy đăng ký sự kiện để tránh rò rỉ bộ nhớ:
public ExternalDBApplicationResult OnShutdown(ControlledApplication app)
{
    app.DocumentChanged -= new EventHandler<DocumentChangedEventArgs>(ElementChangedEvent);
    return ExternalDBApplicationResult.Succeeded;
}

Mã hoàn chỉnh:
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System.Linq;

namespace MyRevitCommands
{
    public class ExternalDBApp : IExternalDBApplication
    {
        public ExternalDBApplicationResult OnStartup(ControlledApplication app)
        {
            app.DocumentChanged += new EventHandler<DocumentChangedEventArgs>(ElementChangedEvent);
            return ExternalDBApplicationResult.Succeeded;
        }

        public ExternalDBApplicationResult OnShutdown(ControlledApplication app)
        {
            app.DocumentChanged -= new EventHandler<DocumentChangedEventArgs>(ElementChangedEvent);
            return ExternalDBApplicationResult.Succeeded;
        }

        public void ElementChangedEvent(object sender, DocumentChangedEventArgs args)
        {
            ElementFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Furniture);
            ElementId element = args.GetModifiedElementIds(filter).First();
            string name = args.GetTransactionNames().First();
            TaskDialog.Show("Modified Element", $"{element.ToString()} changed by {name}");
        }
    }
}

Hành động trên màn hình:  

Nhập mã OnShutdown, highlight DocumentChanged -=.  
Chèn text: “OnShutdown: Hủy đăng ký sự kiện.”


Bước 8: Cập nhật ManifestChỉnh sửa tệp manifest (.addin) để đăng ký ExternalDBApp (bỏ bình luận DBApplication):
<?xml version="1.0" encoding="utf-8"?>
<RevitAddIns>
    <AddIn Type="Application">
        <Name>ExternalApplication</Name>
        <Assembly>MyRevitCommands.dll</Assembly>
        <FullClassName>MyRevitCommands.ExternalApplication</FullClassName>
        <ClientId>your-guid-here</ClientId>
    </AddIn>
    <AddIn Type="DBApplication">
        <Name>ExternalDBApp</Name>
        <Assembly>MyRevitCommands.dll</Assembly>
        <FullClassName>MyRevitCommands.ExternalDBApp</FullClassName>
        <ClientId>your-guid-here</ClientId>
    </AddIn>
</RevitAddIns>

Hành động trên màn hình:  

Hiển thị Solution Explorer, mở MyRevitCommands.addin, bỏ bình luận DBApplication, highlight ExternalDBApp.  
Chèn text: “Manifest: Đăng ký ExternalDBApp.”




Phần 3: Đăng ký và kiểm tra
Hướng dẫn viên (giọng điệu khích lệ):Lớp ExternalDBApp đã sẵn sàng và được đăng ký trong manifest. Hãy kiểm tra sự kiện DocumentChanged trong Revit!

Bước 9: Chạy Debug và kiểm traTrong Visual Studio, nhấn F5 để chạy debug mode. Mở Revit, mở tệp bài tập của video này, chứa các phần tử Furniture. Thực hiện thay đổi (di chuyển, chỉnh sửa, hoặc xóa một phần tử Furniture). Kết quả: Một TaskDialog hiển thị ID của phần tử Furniture đã thay đổi và tên giao dịch.
Hành động trên màn hình:  

Hiển thị Visual Studio, nhấn F5.  
Trong Revit, mở tệp bài tập, chỉnh sửa một Furniture, highlight TaskDialog với ID và tên giao dịch.  
Chèn text: “Kiểm tra: TaskDialog thông báo Furniture thay đổi.”




Phần 4: Kết luận và bước tiếp theo
Hướng dẫn viên (giọng điệu truyền cảm hứng):Chúc mừng các bạn! Hôm nay, chúng ta đã:

Tạo lớp ExternalDBApp triển khai IExternalDBApplication.  
Đăng ký sự kiện DocumentChanged để theo dõi thay đổi Furniture.  
Hiển thị thông báo qua TaskDialog với ID phần tử và tên giao dịch.

IExternalDBApplication mở ra khả năng theo dõi và tự động hóa các sự kiện cơ sở dữ liệu trong Revit.
Bước 10: Kêu gọi hành độngHãy thử mở rộng Event Handler để lấy danh sách tất cả phần tử Furniture thay đổi (sử dụng List). Hoặc đăng ký sự kiện khác như DocumentOpening. Nếu bạn gặp vấn đề, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay!
Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!
Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"Bài học tiếp theo: Tùy chỉnh sự kiện DB nâng cao."  
"Gặp vấn đề? Để lại câu hỏi trong phần bình luận!"


Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio, mô phỏng TaskDialog với thông báo sự kiện.

