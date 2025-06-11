Kịch bản bài học: Thêm biểu tượng vào nút trong Revit
Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ nâng cấp giao diện người dùng bằng cách thêm biểu tượng vào nút PlaceFamily trong tab Ribbon My Commands. Chúng ta sẽ sử dụng một hình ảnh PNG 32x32 pixel và tích hợp nó vào nút bằng PushButton.LargeImage. Sau bài học này, bạn sẽ biết:

Cách chuẩn bị hình ảnh cho nút Ribbon (32x32 hoặc 16x16 pixel).
Cách sử dụng BitmapImage để tải hình ảnh từ tệp.
Cách gán biểu tượng cho nút bằng PushButton.LargeImage.

Hãy cùng bắt đầu!

Phần 1: Tìm hiểu về biểu tượng trong Revit UI
Hướng dẫn viên (giọng điệu hào hứng):Trong bài trước, chúng ta đã tạo tab My Commands và nút PlaceFamily. Hôm nay, chúng ta sẽ làm nút này dễ nhận diện hơn bằng cách thêm biểu tượng. Hãy khám phá cách tích hợp biểu tượng vào giao diện Revit!

Bước 1: Yêu cầu về hình ảnh biểu tượngRevit hỗ trợ hình ảnh cho nút Ribbon với kích thước:

32x32 pixel: Dùng cho nút tiêu chuẩn (PushButton).
16x16 pixel: Dùng cho các điều khiển nhỏ hơn (như DropDown).
Định dạng: PNG là lý tưởng, đảm bảo nền trong suốt nếu cần.
Mẹo: Tạo hình ảnh lớn (ví dụ: 200x200 pixel) trong phần mềm vector như Illustrator, sau đó thu nhỏ để đảm bảo chất lượng.

Trong bài này, chúng ta sử dụng tệp desk.png (32x32 pixel) có sẵn trong thư mục bài tập.
Hành động trên màn hình:  

Hiển thị hình ảnh desk.png trong File Explorer, zoom vào để thấy kích thước 32x32.  
Chèn text: “Biểu tượng: 32x32 pixel cho PushButton.”


Bước 2: Tích hợp biểu tượng với PushButtonĐể thêm biểu tượng vào nút:

Sử dụng PushButton.LargeImage để gán hình ảnh.
Tải hình ảnh bằng BitmapImage (thuộc System.Windows.Media.Imaging), sử dụng URI để định vị tệp.
Yêu cầu tham chiếu PresentationCore assembly để truy cập ImageSource.

Hành động trên màn hình:  

Hiển thị mã giả lập:  Uri imagePath = new Uri(@"C:\path\to\desk.png");
BitmapImage image = new BitmapImage(imagePath);
PushButton pushButton = panel.AddItem(buttonData) as PushButton;
pushButton.LargeImage = image;


Chèn text: “PushButton.LargeImage: Gán biểu tượng.”




Phần 2: Thêm biểu tượng trong Visual Studio
Hướng dẫn viên (giọng điệu rõ ràng):Hãy mở Visual Studio để chỉnh sửa lớp ExternalApplication, thêm biểu tượng desk.png vào nút PlaceFamily trong tab My Commands.

Bước 3: Mở tệp ExternalApplication.csMở tệp ExternalApplication.cs trong dự án MyRevitCommands. Mã hiện tại từ bài trước:
using Autodesk.Revit.UI;
using System.Reflection;

namespace MyRevitCommands
{
    public class ExternalApplication : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication app)
        {
            app.CreateRibbonTab("My Commands");

            string path = Assembly.GetExecutingAssembly().Location;
            PushButtonData button = new PushButtonData("Button1", "PlaceFamily", path, "MyRevitCommands.PlaceFamily");

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

Hành động trên màn hình:  

Hiển thị Solution Explorer, nhấp vào ExternalApplication.cs.  
Zoom vào OnStartup, highlight dòng panel.AddItem(button).  
Chèn text: “ExternalApplication: Điểm khởi đầu để thêm biểu tượng.”


Bước 4: Thêm tham chiếu PresentationCoreĐể sử dụng BitmapImage, thêm tham chiếu PresentationCore:

Nhấp chuột phải vào References trong Solution Explorer, chọn Add Reference.
Trong tab Assemblies, tìm PresentationCore, chọn và nhấn OK.

Hành động trên màn hình:  

Hiển thị Solution Explorer, nhấp chuột phải vào References, mở Add Reference, tìm PresentationCore, highlight và chọn OK.  
Chèn text: “PresentationCore: Thêm tham chiếu cho BitmapImage.”


Bước 5: Tải hình ảnh desk.pngTrước khi tạo PushButtonData, thêm mã để tải desk.png bằng BitmapImage:
using System.Windows.Media.Imaging;
// ...
Uri imagePath = new Uri(@"C:\ExerciseFiles\desk.png");
BitmapImage image = new BitmapImage(imagePath);


Uri: Sử dụng ký hiệu @ để xử lý dấu gạch chéo ngược trong đường dẫn.
BitmapImage: Tải hình ảnh từ Uri.
Thêm using System.Windows.Media.Imaging; nếu Visual Studio báo lỗi.

Hành động trên màn hình:  

Thêm using System.Windows.Media.Imaging;, nhập mã Uri và BitmapImage, highlight @"C:\ExerciseFiles\desk.png".  
Chèn text: “BitmapImage: Tải desk.png.”


Bước 6: Gán biểu tượng cho PushButtonSửa dòng panel.AddItem(button) để lưu PushButton và gán LargeImage:
PushButton pushButton = panel.AddItem(button) as PushButton;
pushButton.LargeImage = image;


AddItem: Trả về RibbonItem, ép kiểu thành PushButton.
LargeImage: Gán BitmapImage làm biểu tượng.

Mã hoàn chỉnh trong OnStartup:
using Autodesk.Revit.UI;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace MyRevitCommands
{
    public class ExternalApplication : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication app)
        {
            app.CreateRibbonTab("My Commands");

            string path = Assembly.GetExecutingAssembly().Location;
            PushButtonData button = new PushButtonData("Button1", "PlaceFamily", path, "MyRevitCommands.PlaceFamily");

            Uri imagePath = new Uri(@"C:\ExerciseFiles\desk.png");
            BitmapImage image = new BitmapImage(imagePath);

            RibbonPanel panel = app.CreateRibbonPanel("My Commands", "Commands");
            PushButton pushButton = panel.AddItem(button) as PushButton;
            pushButton.LargeImage = image;

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication app)
        {
            return Result.Succeeded;
        }
    }
}

Hành động trên màn hình:  

Sửa panel.AddItem, nhập pushButton.LargeImage, highlight as PushButton.  
Chèn text: “PushButton.LargeImage: Gán biểu tượng cho nút.”




Phần 3: Đăng ký và kiểm tra
Hướng dẫn viên (giọng điệu khích lệ):Lớp ExternalApplication đã được đăng ký trong tệp manifest từ bài trước. Hãy kiểm tra nút PlaceFamily với biểu tượng mới trong Revit!

Bước 7: Chạy Debug và kiểm traTrong Visual Studio, nhấn F5 để chạy debug mode. Mở Revit, mở tệp bài tập của video này. Kết quả:

Tab My Commands hiển thị panel Commands.
Nút PlaceFamily có biểu tượng desk.png (32x32 pixel).
Nhấp nút để kiểm tra lệnh vẫn hoạt động.

Hành động trên màn hình:  

Hiển thị Visual Studio, nhấn F5.  
Trong Revit, highlight tab My Commands, zoom vào nút PlaceFamily với biểu tượng desk.png.  
Nhấp nút, kiểm tra lệnh, chèn text: “Kiểm tra: Nút PlaceFamily với biểu tượng.”




Phần 4: Kết luận và bước tiếp theo
Hướng dẫn viên (giọng điệu truyền cảm hứng):Chúc mừng các bạn! Hôm nay, chúng ta đã:

Thêm biểu tượng desk.png vào nút PlaceFamily trong tab My Commands.  
Sử dụng BitmapImage và PushButton.LargeImage để tích hợp hình ảnh.  
Thêm tham chiếu PresentationCore để hỗ trợ ImageSource.

Biểu tượng giúp giao diện Revit trực quan và chuyên nghiệp hơn.
Bước 8: Kêu gọi hành độngHãy thử thêm biểu tượng cho các nút khác hoặc thử tạo nút DropDown với biểu tượng 16x16 pixel. Tham khảo Revit Developer Guidelines để khám phá các kiểu nút như RadioButton hoặc ListBox. Nếu bạn gặp vấn đề, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay!
Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!
Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"Bài học tiếp theo: Tùy chỉnh nút DropDown hoặc sự kiện UI."  
"Gặp vấn đề? Để lại câu hỏi trong phần bình luận!"


Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio, mô phỏng tab My Commands với nút có biểu tượng.

