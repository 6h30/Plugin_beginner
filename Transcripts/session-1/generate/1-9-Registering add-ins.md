Kịch bản bài học: Đăng ký Add-in trong Revit
Mục tiêu
Chúc mừng các bạn đã hoàn thành lệnh GetElementId! Trong bài học hôm nay, chúng ta sẽ học cách đăng ký plugin vào Revit bằng cách tạo một tệp manifest (.addin). Sau bài học này, bạn sẽ biết:

Tệp manifest là gì và vai trò của nó trong việc tải plugin vào Revit.
Cách tạo và cấu hình tệp manifest trong Visual Studio.
Cách tạo GUID và đảm bảo tệp manifest được sao chép khi biên dịch.

Hãy cùng bắt đầu!

Phần 1: Giới thiệu về Manifest File
Hướng dẫn viên (giọng điệu hào hứng):Chúng ta đã viết lệnh GetElementId, nhưng để Revit nhận diện và hiển thị lệnh này, chúng ta cần đăng ký plugin bằng một tệp manifest (.addin). Khi Revit khởi động, nó sẽ tìm các tệp .addin trong hai thư mục cụ thể để xác định plugin nào cần tải và với tùy chọn nào. Hãy cùng tạo tệp manifest cho dự án MyRevitCommands!

Bước 1: Hiểu về Manifest FileTệp manifest là một tệp XML chứa các cài đặt cho plugin, như tên, đường dẫn tệp DLL, và ID duy nhất. Revit sử dụng tệp này để biết cách hiển thị lệnh trong tab Add-Ins.
Hành động trên màn hình:  

Hiển thị giao diện Revit, zoom vào tab Add-Ins.  
Chèn text: “Manifest File: Đăng ký plugin để hiển thị trong Revit.”




Phần 2: Tạo tệp Manifest trong Visual Studio
Hướng dẫn viên (giọng điệu rõ ràng):Hãy quay lại Visual Studio để thêm tệp manifest vào dự án MyRevitCommands.

Bước 2: Thêm tệp ManifestTrong Solution Explorer, nhấp chuột phải vào tên dự án (MyRevitCommands), chọn Add > New Item. Trong cửa sổ Add New Item, tìm và chọn Application Manifest File. Đổi tên tệp thành MyRevitCommands.addin (lưu ý phần mở rộng .addin là bắt buộc để Revit nhận diện). Nhấn Add.
Hành động trên màn hình:  

Hiển thị Solution Explorer, nhấp chuột phải vào MyRevitCommands, chọn Add > New Item.  
Hiển thị cửa sổ Add New Item, highlight Application Manifest File, nhập MyRevitCommands.addin.  
Zoom vào nút Add.


Bước 3: Xóa nội dung mặc địnhTệp MyRevitCommands.addin mới tạo sẽ chứa mã mặc định không cần thiết. Mở tệp, chọn toàn bộ nội dung và xóa.
Hành động trên màn hình:  

Hiển thị tệp MyRevitCommands.addin trong Visual Studio, highlight toàn bộ nội dung và nhấn Delete.


Bước 4: Thêm mã XML cho ManifestThay vì viết từ đầu, chúng ta sẽ sử dụng mẫu XML từ Autodesk Revit Developer Guide. Để tiện, bạn có thể tìm mẫu này trong tệp MyRevitCommands.txt trong thư mục bài tập của video. Sao chép nội dung từ tệp MyRevitCommands.txt (Ctrl+C) và dán (Ctrl+V) vào MyRevitCommands.addin. Nội dung mẫu trông như sau:
<?xml version="1.0" encoding="utf-8"?>
<RevitAddIns>
  <AddIn Type="Command">
    <Name>MyRevitCommands</Name>
    <Assembly>MyRevitCommands.dll</Assembly>
    <AddInId>YOUR_GUID_HERE</AddInId>
    <FullClassName>MyRevitCommands.GetElementId</FullClassName>
    <Text>Get Element ID</Text>
    <Description>Retrieves the Element ID of a selected element</Description>
    <VendorId>Jeremy</VendorId>
    <VisibilityMode>NotVisibleWhenNoActiveDocument</VisibilityMode>
  </AddIn>
</RevitAddIns>

Hành động trên màn hình:  

Hiển thị tệp MyRevitCommands.txt (giả lập), sao chép nội dung.  
Dán nội dung vào MyRevitCommands.addin trong Visual Studio.  
Chèn text: “Manifest File: XML định nghĩa cài đặt plugin.”




Phần 3: Cấu hình Manifest File
Hướng dẫn viên (giọng điệu cẩn thận):Bây giờ, chúng ta sẽ chỉnh sửa tệp manifest để đảm bảo nó phù hợp với lệnh GetElementId.

Bước 5: Tạo và thêm GUIDTrong tệp manifest, thay YOUR_GUID_HERE bằng một GUID (Global Unique Identifier) duy nhất. Để tạo GUID, trong Visual Studio, vào Tools > Create GUID. Trong cửa sổ Create GUID, chọn định dạng Registry Format (không có định dạng thừa), nhấn New GUID, sau đó nhấn Copy. Dán GUID vào giữa thẻ <AddInId> và xóa các ký tự thừa (như {} hoặc dấu ngoặc). Ví dụ:
<AddInId>550e8400-e29b-41d4-a716-446655440000</AddInId>

Hành động trên màn hình:  

Hiển thị Visual Studio, vào Tools > Create GUID.  
Hiển thị cửa sổ Create GUID, chọn Registry Format, nhấn New GUID và Copy.  
Dán GUID vào MyRevitCommands.addin, xóa {} và các ký tự thừa.  
Chèn text: “GUID: Định danh duy nhất cho lệnh.”


Bước 6: Cấu hình các thẻ XMLKiểm tra và điều chỉnh các thẻ trong tệp manifest:

Name: Tên plugin, giữ là MyRevitCommands.
Assembly: Tên tệp DLL sau khi biên dịch, là MyRevitCommands.dll (vì chúng ta sẽ lưu vào thư mục add-ins, không cần đường dẫn đầy đủ).
FullClassName: Namespace và tên lớp, là MyRevitCommands.GetElementId.
Text: Tên hiển thị của lệnh trong Revit, là Get Element ID.
Description: Mô tả lệnh, ví dụ: Retrieves the Element ID of a selected element.
VendorId: ID nhà phát triển, ví dụ: Jeremy (bạn có thể thay bằng ID của mình).
VisibilityMode: Đổi từ AlwaysVisible thành NotVisibleWhenNoActiveDocument (viết hoa chữ cái đầu, không có dấu cách) để lệnh chỉ hiển thị khi có tài liệu đang mở.

Hành động trên màn hình:  

Hiển thị tệp MyRevitCommands.addin, highlight từng thẻ XML và giải thích ngắn gọn.  
Zoom vào <VisibilityMode>NotVisibleWhenNoActiveDocument</VisibilityMode>.  
Chèn text: “Cấu hình Manifest: Đảm bảo lệnh hiển thị đúng ngữ cảnh.”




Phần 4: Cấu hình sao chép tệp Manifest
Hướng dẫn viên (giọng điệu khích lệ):Để tệp manifest được sao chép khi biên dịch, chúng ta cần điều chỉnh thuộc tính của nó.

Bước 7: Thay đổi thuộc tính Copy to Output DirectoryTrong Solution Explorer, chọn MyRevitCommands.addin. Ở cửa sổ Properties (góc dưới bên phải), đổi thuộc tính Copy to Output Directory thành Copy if newer. Điều này đảm bảo tệp .addin được sao chép vào thư mục đầu ra khi biên dịch.
Hành động trên màn hình:  

Hiển thị Solution Explorer, highlight MyRevitCommands.addin.  
Hiển thị cửa sổ Properties, đổi Copy to Output Directory thành Copy if newer.  
Chèn text: “Copy if newer: Đảm bảo tệp manifest được sao chép khi biên dịch.”




Phần 5: Kết luận và bước tiếp theo
Hướng dẫn viên (giọng điệu truyền cảm hứng):Chúc mừng các bạn! Hôm nay, chúng ta đã:

Tạo và cấu hình tệp manifest (MyRevitCommands.addin) để đăng ký plugin.  
Tạo GUID duy nhất và điều chỉnh các thẻ XML cần thiết.  
Đảm bảo tệp manifest được sao chép khi biên dịch.

Bước tiếp theo là biên dịch dự án và tải plugin vào Revit để kiểm tra lệnh GetElementId hoạt động!
Bước 8: Kêu gọi hành độngTrong bài học tiếp theo, chúng ta sẽ biên dịch dự án, sao chép tệp DLL và manifest vào thư mục add-ins của Revit, và chạy lệnh trong Revit. Hãy giữ Visual Studio và Revit sẵn sàng! Nếu bạn gặp vấn đề, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay.
Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!
Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"Bài học tiếp theo: Biên dịch và chạy plugin trong Revit."  
"Gặp vấn đề? Để lại câu hỏi trong phần bình luận!"


Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio.

