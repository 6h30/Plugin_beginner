Kịch bản bài học: Thêm Revit API vào dự án
Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Hôm nay, chúng ta sẽ tiếp tục hành trình bằng cách thêm Revit API vào dự án Visual Studio mà chúng ta đã tạo. Sau bài học này, bạn sẽ biết:

Cách thêm các tệp Revit API vào dự án.
Cách sử dụng Object Browser để khám phá các lớp và phương thức của Revit API.
Cách thêm các namespace cần thiết vào mã để bắt đầu viết lệnh Revit.

Hãy cùng bắt đầu!

Phần 1: Thêm tệp Revit API vào dự án
Hướng dẫn viên (giọng điệu nhiệt tình):Chúng ta đã thiết lập dự án MyRevitCommands trong Visual Studio, nhưng hiện tại dự án chỉ có một lớp trống, chưa thể truy cập Revit API. Để bắt đầu, chúng ta cần thêm các tệp Revit API vào dự án. Hãy làm từng bước nhé!

Bước 1: Mở Solution Explorer và thêm tham chiếuTrong Visual Studio, nhìn sang Solution Explorer bên phải và mở rộng mục References. Đây là nơi hiển thị tất cả các assemblies (thư viện) mà dự án đang sử dụng. Để thêm Revit API, nhấp chuột phải vào References và chọn Add Reference.
Hành động trên màn hình:  

Hiển thị giao diện Visual Studio, zoom vào Solution Explorer.  
Highlight mục References, nhấp chuột phải và chọn Add Reference.


Bước 2: Tìm và thêm tệp Revit APITrong cửa sổ Reference Manager, các tệp Revit API không nằm trong danh sách assemblies chuẩn, nên chúng ta cần tìm chúng thủ công. Nhấn nút Browse ở góc dưới bên phải. Điều hướng đến thư mục cài đặt Revit, thường là C:\Program Files\Autodesk\Revit 2019. Tìm hai tệp:

RevitAPI.dll  
RevitAPIUI.dll

Chọn RevitAPI.dll, sau đó giữ phím Ctrl và nhấp trái vào RevitAPIUI.dll để chọn cả hai. Nhấn Add, đảm bảo cả hai tệp đều được chọn trong hộp kiểm, rồi nhấn OK.
Hành động trên màn hình:  

Hiển thị cửa sổ Reference Manager, zoom vào nút Browse.  
Hiển thị File Explorer tại C:\Program Files\Autodesk\Revit 2019, highlight RevitAPI.dll và RevitAPIUI.dll.  
Zoom vào nút Add và OK.


Bước 3: Cấu hình thuộc tính Copy LocalSau khi thêm, bạn sẽ thấy RevitAPI và RevitAPIUI xuất hiện trong References ở Solution Explorer. Chọn lần lượt từng tệp, sau đó ở cửa sổ Properties (góc dưới bên phải), đổi thuộc tính Copy Local từ True sang False. Điều này đảm bảo Visual Studio không sao chép các tệp DLL khi biên dịch, vì Revit đã tự động tham chiếu chúng khi khởi động.
Hành động trên màn hình:  

Hiển thị Solution Explorer, highlight RevitAPI và RevitAPIUI.  
Zoom vào cửa sổ Properties, đổi Copy Local từ True sang False.




Phần 2: Khám phá Revit API với Object Browser
Hướng dẫn viên (giọng điệu tò mò):Tuyệt! Bây giờ Revit API đã được thêm vào dự án. Hãy khám phá các lớp và phương thức bên trong nó bằng Object Browser – một công cụ siêu hữu ích trong Visual Studio!

Bước 4: Mở Object BrowserTrong Solution Explorer, nhấp đúp vào RevitAPI hoặc RevitAPIUI để mở Object Browser. Cửa sổ này cho phép bạn duyệt qua các namespace, lớp (classes), và các loại khác trong Revit API.
Hành động trên màn hình:  

Hiển thị Solution Explorer, nhấp đúp vào RevitAPI.  
Hiển thị giao diện Object Browser.


Bước 5: Khám phá namespace Autodesk.Revit.DBTrong Object Browser, mở rộng RevitAPI để xem danh sách các namespace. Tìm và mở Autodesk.Revit.DB – namespace chính mà chúng ta sẽ dùng. Bạn sẽ thấy các lớp quen thuộc như Element hoặc Wall, tương tự những gì chúng ta thấy trong tệp RevitAPI.CHM trước đó.
Hành động trên màn hình:  

Hiển thị Object Browser, mở rộng RevitAPI và highlight Autodesk.Revit.DB.  
Hiển thị danh sách các lớp như Element, Wall.


Bước 6: Tìm kiếm và xem chi tiết lớpHãy thử tìm một lớp cụ thể, ví dụ: Element. Trong Object Browser, nhập “Element” vào ô tìm kiếm. Khi lớp Element xuất hiện, chọn nó để xem các phương thức (methods) và thuộc tính (properties) ở cửa sổ trên bên phải. Mô tả chi tiết của lớp hoặc phương thức sẽ hiển thị ở cửa sổ dưới bên phải.
Hành động trên màn hình:  

Hiển thị ô tìm kiếm trong Object Browser, nhập “Element”.  
Highlight lớp Element, hiển thị danh sách phương thức/thuộc tính và mô tả ở cửa sổ tương ứng.  
Chèn text: “Object Browser: Công cụ tìm kiếm các lớp và phương thức của Revit API.”




Phần 3: Thêm namespace vào mã
Hướng dẫn viên (giọng điệu rõ ràng):Bây giờ, chúng ta sẽ thêm các namespace cần thiết vào mã để bắt đầu viết lệnh Revit. Hãy chuyển sang tệp mã của chúng ta!

Bước 7: Mở tệp Class1.csTrong Visual Studio, nhấp vào tab Class1.cs (hoặc tên lớp bạn đã đặt) ở góc trên bên trái. Đây là nơi chúng ta sẽ thêm các namespace.
Hành động trên màn hình:  

Hiển thị Visual Studio, nhấp vào tab Class1.cs.


Bước 8: Thêm các namespace cần thiếtỞ đầu tệp Class1.cs, thêm các dòng sau để sử dụng các namespace quan trọng:
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;


Autodesk.Revit.DB: Cung cấp quyền truy cập vào các lớp cơ sở dữ liệu, như Element hoặc Wall.  
Autodesk.Revit.UI: Cho phép tương tác với giao diện người dùng của Revit, như tạo nút hoặc cửa sổ.  
Autodesk.Revit.Attributes: Dùng để định nghĩa các thuộc tính cho lệnh Revit (chúng ta sẽ tìm hiểu thêm ở bài sau).

Hành động trên màn hình:  

Hiển thị tệp Class1.cs trong Visual Studio, zoom vào phần đầu tệp.  
Hiển thị hành động nhập các dòng using ở trên.  
Chèn text: “Thêm namespace để truy cập Revit API.”




Phần 4: Kết luận và bước tiếp theo
Hướng dẫn viên (giọng điệu truyền cảm hứng):Chúc mừng các bạn! Chúng ta đã:

Thêm RevitAPI.dll và RevitAPIUI.dll vào dự án.  
Sử dụng Object Browser để khám phá các lớp và phương thức của Revit API.  
Thêm các namespace cần thiết vào mã để chuẩn bị viết lệnh Revit.

Bước 9: Kêu gọi hành độngTrong bài học tiếp theo, chúng ta sẽ bắt đầu triển khai lệnh Revit đầu tiên bằng C#! Hãy giữ Visual Studio và Revit sẵn sàng. Nếu bạn gặp vấn đề khi thêm API hoặc namespace, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay.
Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!
Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"Bài học tiếp theo: Viết lệnh Revit đầu tiên với C#."  
"Gặp vấn đề? Để lại câu hỏi trong phần bình luận!"


Chèn logo khóa học hoặc hình ảnh minh họa Visual Studio/Revit.

