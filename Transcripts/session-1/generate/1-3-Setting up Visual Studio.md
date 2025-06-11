Kịch bản bài học: Thiết lập Visual Studio cho Revit Plugin
Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Hôm nay, chúng ta sẽ bắt đầu hành trình tạo plugin đầu tiên bằng cách thiết lập môi trường lập trình với Visual Studio. Sau bài học này, bạn sẽ biết:

Cách tải và cài đặt Visual Studio.
Cách chọn các thành phần cần thiết để phát triển plugin Revit.
Cách tạo một dự án C# Class Library phù hợp với Revit API.
Tầm quan trọng của việc chọn đúng phiên bản .NET Framework.

Hãy cùng bắt đầu!

Phần 1: Tải và cài đặt Visual Studio
Hướng dẫn viên (giọng điệu hào hứng):Để tạo plugin cho Revit, chúng ta sẽ sử dụng Visual Studio, một môi trường phát triển tích hợp (IDE) mạnh mẽ. Nếu bạn thích IDE khác, không sao cả, nhưng trong khóa học này, mình sẽ sử dụng Visual Studio Community 2017 – phiên bản miễn phí. Hãy cùng cài đặt nào!

Bước 1: Tải Visual Studio
Mở trình duyệt và truy cập visualstudio.microsoft.com. Tại đây, bạn sẽ thấy tùy chọn tải Visual Studio. Di chuột đến Download for Windows, chọn Community 2017 để tải phiên bản miễn phí. Sau khi tải, lưu tệp cài đặt và chạy nó.
Hành động trên màn hình:  

Hiển thị trang web visualstudio.microsoft.com (giả lập).  
Zoom vào mục Download for Windows và chọn Community 2017.  
Hiển thị hành động lưu và chạy tệp cài đặt.


Bước 2: Chọn thành phần cài đặt
Khi chạy trình cài đặt, bạn sẽ thấy một menu cho phép chọn các thành phần. Vì chúng ta sẽ phát triển plugin .NET cho Revit, hãy đảm bảo chọn .NET desktop development.  
Hành động trên màn hình:  

Hiển thị giao diện trình cài đặt Visual Studio, zoom vào mục .NET desktop development được đánh dấu.


Bước 3: Kiểm tra .NET Framework 4.7
Chuyển sang tab Individual Components, đảm bảo rằng .NET Framework 4.7 Targeting Pack được chọn. Điều này rất quan trọng nếu bạn làm việc với Revit 2019, vì Revit 2019 yêu cầu .NET Framework 4.7. Sau khi chọn xong, nhấn Install và chờ quá trình hoàn tất.
Hành động trên màn hình:  

Hiển thị tab Individual Components, highlight .NET Framework 4.7 Targeting Pack.  
Zoom vào nút Install.




Phần 2: Tạo dự án mới trong Visual Studio
Hướng dẫn viên (giọng điệu rõ ràng):Tuyệt vời! Bây giờ Visual Studio đã được cài đặt. Hãy mở nó lên và bắt đầu tạo dự án đầu tiên cho plugin Revit của chúng ta!

Bước 4: Mở Visual Studio và tạo dự án mớiKhi mở Visual Studio, bạn sẽ thấy trang bắt đầu. Để tạo dự án mới, vào menu File ở góc trên bên trái, chọn New, rồi chọn Project.  
Hành động trên màn hình:  

Hiển thị giao diện Visual Studio với trang bắt đầu.  
Zoom vào menu File > New > Project.


Bước 5: Chọn mẫu dự án Class Library
Trong cửa sổ New Project, bên cột trái, chọn Installed > Visual C#. Ở cột giữa, tìm và chọn Class Library (.NET Framework). Đây là mẫu dự án tạo thư viện mã mà chúng ta sẽ dùng cho plugin Revit.
Hành động trên màn hình:  

Hiển thị cửa sổ New Project, highlight Visual C# và Class Library (.NET Framework).


Bước 6: Chọn đúng .NET Framework
Ở dưới cùng cửa sổ, bạn sẽ thấy một menu thả xuống để chọn phiên bản .NET Framework. Điều này rất quan trọng vì phiên bản .NET Framework phải khớp với phiên bản Revit bạn sử dụng:

Revit 2017 hoặc 2018: Chọn .NET Framework 4.6.
Revit 2015 hoặc 2016: Chọn .NET Framework 4.5.
Revit 2019: Chọn .NET Framework 4.7 (phiên bản chúng ta sẽ dùng).

Nếu bạn không thấy .NET Framework 4.7, đừng lo! Vào menu Tools > Get Tools and Features, chọn tab Individual Components, tìm và chọn .NET Framework 4.7 Targeting Pack, sau đó cài đặt và khởi động lại Visual Studio.
Hành động trên màn hình:  

Hiển thị menu thả xuống .NET Framework, highlight 4.7.  
Chèn text: "Revit 2019 yêu cầu .NET Framework 4.7".  
(Nếu cần) Hiển thị menu Tools > Get Tools and Features, highlight .NET Framework 4.7 Targeting Pack.




Phần 3: Đặt tên và lưu dự án
Hướng dẫn viên (giọng điệu khích lệ):Bây giờ, chúng ta sẽ đặt tên cho dự án và lưu nó đúng cách để bắt đầu viết mã!

Bước 7: Đặt tên dự ánTrong cửa sổ New Project, đặt tên dự án là MyRevitCommands – cái tên này sẽ đại diện cho bộ lệnh tùy chỉnh chúng ta tạo cho Revit.  
Hành động trên màn hình:  

Hiển thị cửa sổ New Project, nhập MyRevitCommands vào ô tên dự án.


Bước 8: Chọn vị trí lưu và cài đặt cuối cùng
Kiểm tra đường dẫn lưu dự án. Mặc định, Visual Studio sẽ lưu ở C:\Users\YourName\source\repos. Bạn có thể thay đổi nếu muốn. Đảm bảo chọn Create directory for solution và bỏ chọn Add to source control. Sau đó, nhấn OK.
Hành động trên màn hình:  

Hiển thị đường dẫn mặc định C:\Users\YourName\source\repos.  
Highlight ô Create directory for solution được chọn và Add to source control không được chọn.  
Zoom vào nút OK.




Phần 4: Kết luận và bước tiếp theo
Hướng dẫn viên (giọng điệu truyền cảm hứng):Chúc mừng các bạn! Chúng ta đã:

Cài đặt Visual Studio Community 2017.  
Chọn đúng thành phần .NET Framework 4.7 cho Revit 2019.  
Tạo dự án MyRevitCommands để bắt đầu phát triển plugin.

Bước 9: Kêu gọi hành độngTrong bài học tiếp theo, chúng ta sẽ bắt đầu viết mã C# để tạo plugin Revit đầu tiên. Hãy đảm bảo Visual Studio và Revit SDK đã sẵn sàng! Nếu bạn gặp vấn đề khi cài đặt, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay.
Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!
Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"Bài học tiếp theo: Viết mã C# cho plugin Revit đầu tiên."  
"Gặp vấn đề? Để lại câu hỏi trong phần bình luận!"


Chèn logo khóa học hoặc hình ảnh minh họa Visual Studio/Revit.

