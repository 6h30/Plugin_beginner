Kịch bản bài học: Xem Revit API
Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ học cách xem và khám phá Revit API bằng cách sử dụng Revit SDK và tệp RevitAPI.CHM. Sau bài học này, bạn sẽ biết:

Cách cài đặt Revit SDK.
Cách truy cập và sử dụng tệp RevitAPI.CHM để xem tài liệu API.
Ý nghĩa của namespace và vai trò của Autodesk.Revit.DB.
Cách làm quen với các lớp (classes) trong Revit API.

Hãy cùng bắt đầu nào!

Phần 1: Cài đặt Revit SDK
Hướng dẫn viên (giọng điệu nhiệt tình):Để khám phá Revit API, cách đơn giản nhất là sử dụng Revit Software Development Kit (SDK). SDK chứa các công cụ, tài liệu và ví dụ giúp chúng ta làm việc với API. Hãy cùng cài đặt nó!

Bước 1: Tải và cài đặt Revit SDK
Nếu bạn đã cài Revit, bạn có thể sử dụng trình cài đặt Revit để thêm SDK. Nếu chưa có, đừng lo! Bạn có thể tải trình cài đặt từ trang web của Autodesk.
Hành động trên màn hình:  

Hiển thị trang web Autodesk (giả lập): www.autodesk.com.  
Chèn text: "Tìm Revit Installer để tải SDK."


Bước 2: Truy cập menu cài đặt
Mở trình cài đặt Revit, sau đó chọn tùy chọn Install Tools and Utilities. Trong menu này, bạn sẽ thấy mục Revit Software Development Kit. Hãy đảm bảo chọn nó và nhấn Install.
Hành động trên màn hình:  

Hiển thị giao diện trình cài đặt Revit với tùy chọn "Install Tools and Utilities" được đánh dấu.  
Zoom vào mục "Revit Software Development Kit" và nút Install.


Bước 3: Chọn đường dẫn cài đặt
Sau khi tải xong, trình cài đặt sẽ hỏi bạn muốn lưu SDK ở đâu. Mình khuyên bạn nên giữ đường dẫn mặc định, ví dụ: C:\Revit SDK 2019. Sau đó, nhấn Install và chờ quá trình hoàn tất. Khi xong, nhấn Finish.
Hành động trên màn hình:  

Hiển thị cửa sổ chọn đường dẫn với đường dẫn mặc định: C:\Revit SDK 2019.  
Zoom vào nút Install và Finish.




Phần 2: Khám phá thư mục SDK
Hướng dẫn viên (giọng điệu tò mò):Tuyệt vời! Bây giờ SDK đã được cài đặt. Hãy cùng khám phá những gì bên trong!

Bước 4: Mở thư mục SDKĐiều hướng đến thư mục bạn vừa cài đặt SDK. Nếu bạn giữ mặc định, nó sẽ nằm ở C:\Revit SDK 2019. Mở thư mục này, bạn sẽ thấy các tệp và thư mục liên quan đến lập trình với Revit, bao gồm:

Các tệp mẫu (sample files).  
Hướng dẫn bắt đầu (getting started guides).  
Và quan trọng nhất: tệp RevitAPI.CHM.

Hành động trên màn hình:  

Hiển thị File Explorer với thư mục C:\Revit SDK 2019.  
Highlight tệp RevitAPI.CHM ở cuối danh sách.


Bước 5: Mở tệp RevitAPI.CHMTệp RevitAPI.CHM là một tệp HTML được biên dịch, định dạng tài liệu trợ giúp của Microsoft. Hãy nhấp đúp vào tệp này để mở. Đây chính là "kho báu" chứa toàn bộ tài liệu về Revit API!
Hành động trên màn hình:  

Hiển thị hành động nhấp đúp vào tệp RevitAPI.CHM.  
Mở giao diện CHM với danh sách các mục như namespaces, classes, và prototypes.




Phần 3: Hiểu về Namespace và Autodesk.Revit.DB
Hướng dẫn viên (giọng điệu rõ ràng):Bây giờ, chúng ta đã mở được tệp CHM. Nhưng bạn có thấy từ namespace xuất hiện không? Nếu bạn chưa quen với thuật ngữ này, đừng lo, mình sẽ giải thích đơn giản!

Bước 6: Namespace là gì?Trong lập trình, namespace giống như một "hộp chứa" giúp nhóm các đối tượng liên quan lại với nhau, để tổ chức mã tốt hơn. Ví dụ, hai lớp có cùng tên nhưng nằm trong các namespace khác nhau sẽ không bị xung đột. Trong Revit, namespace chính mà chúng ta sẽ làm việc là Autodesk.Revit.DB.
Hành động trên màn hình:  

Hiển thị giao diện RevitAPI.CHM, zoom vào mục Namespaces.  
Highlight Autodesk.Revit.DB trong danh sách.


Bước 7: Khám phá Autodesk.Revit.DBNhấp vào Autodesk.Revit.DB để xem nội dung. Namespace này chứa nhiều lớp (classes), giao diện (interfaces), và liệt kê (enumerations), giúp chúng ta truy cập các phần tử, hình học, và thông tin dự án trong Revit. Nếu bạn đã quen dùng Revit, bạn sẽ nhận ra một số tên quen thuộc, như lớp ElevationMarker, dùng để tạo điểm đánh梭
Hành động trên màn hình:  

Hiển thị danh sách các lớp trong Autodesk.Revit.DB, highlight ElevationMarker.  
Chèn text: "Ví dụ: ElevationMarker class tạo điểm cao độ trong Revit."




Phần 4: Tầm quan trọng của RevitAPI.CHM
Hướng dẫn viên (giọng điệu khích lệ):Tệp RevitAPI.CHM là công cụ tuyệt vời để tìm hiểu các phần khác nhau của Revit API. Bạn có thể duyệt qua các lớp, phương thức, và thuộc tính để hiểu cách tương tác với Revit. Trong suốt khóa học, chúng ta sẽ sử dụng namespace Autodesk.Revit.DB rất nhiều, nên hãy dành thời gian khám phá nó!
Hành động trên màn hình:  

Hiển thị giao diện CHM với các mục như classes, methods, và properties.  
Chèn ví dụ: Mô tả ngắn về lớp Wall và phương thức như Wall.Create().


Phần 5: Kết luận và bước tiếp theo
Hướng dẫn viên (giọng điệu truyền cảm hứng):Chúc mừng các bạn! Hôm nay, chúng ta đã học cách:

Cài đặt Revit SDK và truy cập tệp RevitAPI.CHM.  
Hiểu về namespace và khám phá Autodesk.Revit.DB.  
Sử dụng tài liệu API để tìm hiểu về các lớp và phương thức.

Bước 8: Kêu gọi hành độngTrong bài học tiếp theo, chúng ta sẽ bắt đầu xây dựng plugin đầu tiên với Revit API, sử dụng C# trong Visual Studio. Hãy đảm bảo bạn đã cài đặt SDK và Visual Studio, sẵn sàng để viết mã nhé! Link hướng dẫn cài đặt sẽ có trong phần mô tả video.
Cảm ơn các bạn đã theo dõi! Nếu có câu hỏi, hãy để lại bình luận. Hẹn gặp lại ở bài học tiếp theo!
Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"Bài học tiếp theo: Xây dựng plugin đầu tiên với Revit API."  
"Kiểm tra phần mô tả video để tải SDK và Visual Studio!"


Chèn logo khóa học hoặc hình ảnh minh họa Revit.

