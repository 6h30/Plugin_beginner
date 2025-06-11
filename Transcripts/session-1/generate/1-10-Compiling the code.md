Kịch bản bài học: Biên dịch mã và tự động sao chép vào Revit
Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ biên dịch dự án MyRevitCommands để tạo tệp DLL và tự động sao chép các tệp cần thiết vào thư mục add-ins của Revit. Sau bài học này, bạn sẽ biết:

Cách biên dịch dự án trong Visual Studio để tạo tệp DLL.
Các thư mục add-ins mà Revit kiểm tra để tải plugin.
Cách thiết lập Post-build Event để tự động sao chép tệp vào thư mục add-ins.

Hãy cùng bắt đầu!

Phần 1: Biên dịch dự án trong Visual Studio
Hướng dẫn viên (giọng điệu hào hứng):Chúng ta đã tạo lệnh GetElementId và tệp manifest trong các bài trước. Bây giờ, chúng ta sẽ biên dịch mã để tạo tệp DLL (Dynamic Link Library) – chứa toàn bộ mã và dữ liệu của plugin, sẵn sàng để Revit chạy. Hãy cùng làm nào!

Bước 1: Biên dịch dự ánTrong Visual Studio, vào menu Build ở thanh trên cùng và chọn Build Solution. Visual Studio sẽ biên dịch mã và tạo các tệp đầu ra.
Hành động trên màn hình:  

Hiển thị Visual Studio, vào Build > Build Solution.  
Zoom vào cửa sổ Output ở dưới cùng, highlight dòng “Build: 1 succeeded”.  
Chèn text: “Build Solution: Biên dịch mã thành tệp DLL.”


Bước 2: Kiểm tra kết quả biên dịchNếu biên dịch thành công, cửa sổ Output sẽ hiển thị “Build: 1 succeeded”. Nếu có lỗi, Visual Studio sẽ hiển thị chi tiết trong cửa sổ này, vì vậy hãy kiểm tra kỹ nếu biên dịch thất bại. Sau khi biên dịch, các tệp đầu ra sẽ nằm trong thư mục đầu ra của dự án (thường là bin\Debug).
Hành động trên màn hình:  

Hiển thị cửa sổ Output, highlight “Build: 1 succeeded”.  
Chèn text: “Kiểm tra Output: Đảm bảo không có lỗi khi biên dịch.”


Bước 3: Xem các tệp đầu raĐiều hướng đến thư mục đầu ra (thường là C:\Users\YourName\source\repos\MyRevitCommands\bin\Debug). Bạn sẽ thấy ba tệp:

MyRevitCommands.dll: Tệp chứa mã plugin.  
MyRevitCommands.addin: Tệp manifest để đăng ký plugin.  
MyRevitCommands.pdb: Tệp cơ sở dữ liệu chương trình, dùng để gỡ lỗi (chúng ta sẽ dùng sau).

Hành động trên màn hình:  

Hiển thị File Explorer tại thư mục bin\Debug, highlight ba tệp: MyRevitCommands.dll, MyRevitCommands.addin, MyRevitCommands.pdb.  
Chèn text: “Tệp đầu ra: DLL, manifest, và PDB.”




Phần 2: Thư mục Add-ins của Revit
Hướng dẫn viên (giọng điệu rõ ràng):Để Revit nhận diện và chạy plugin, chúng ta cần đặt tệp DLL và manifest vào một trong hai thư mục add-ins mà Revit kiểm tra khi khởi động.

Bước 4: Hiểu về thư mục Add-insRevit kiểm tra hai thư mục:

ProgramData\Autodesk\Revit\Addins\2019: Dành cho plugin chung, không phụ thuộc người dùng.  
Users\Username\AppData\Roaming\Autodesk\Revit\Addins\2019: Dành cho plugin riêng của từng người dùng.

Chúng ta sẽ sử dụng thư mục người dùng (AppData\Roaming) vì nó gắn với tài khoản của bạn.
Hành động trên màn hình:  

Hiển thị File Explorer, điều hướng đến C:\Users\Username\AppData\Roaming\Autodesk\Revit\Addins\2019.  
Chèn text: “Thư mục Add-ins: Nơi Revit tìm tệp manifest và DLL.”




Phần 3: Tự động hóa sao chép tệp với Post-build Event
Hướng dẫn viên (giọng điệu khích lệ):Sao chép tệp DLL và manifest thủ công vào thư mục add-ins rất tốn thời gian, đặc biệt khi chúng ta thử nghiệm nhiều lệnh. Hãy thiết lập Post-build Event trong Visual Studio để tự động hóa việc này!

Bước 5: Mở Project PropertiesTrong Visual Studio, vào menu Project > MyRevitCommands Properties. Chọn tab Build Events ở bên trái.
Hành động trên màn hình:  

Hiển thị Visual Studio, vào Project > MyRevitCommands Properties.  
Zoom vào tab Build Events.


Bước 6: Thêm lệnh Post-buildTrong tab Build Events, nhấp vào Edit Post-build. Trong cửa sổ hiện ra, chúng ta sẽ viết lệnh để sao chép tất cả tệp từ thư mục đầu ra vào thư mục add-ins. Nhập lệnh sau:
copy "$(TargetDir)*.*" "$(AppData)\Autodesk\Revit\Addins\2019\"

Giải thích:

copy: Lệnh sao chép tệp.  
"$(TargetDir).": Macro của Visual Studio, đại diện cho thư mục đầu ra (như bin\Debug) và sao chép tất cả tệp.  
"$(AppData)\Autodesk\Revit\Addins\2019": Đường dẫn đến thư mục add-ins của người dùng.

Nhấn OK để lưu.
Hành động trên màn hình:  

Hiển thị cửa sổ Edit Post-build, nhập lệnh trên.  
Highlight $(TargetDir) và $(AppData), giải thích bằng text:  
“$(TargetDir): Thư mục đầu ra (bin\Debug).”  
“$(AppData): Đường dẫn đến AppData\Roaming.”


Zoom vào nút OK.


Bước 7: Kiểm tra Post-build EventQuay lại menu Build > Build Solution để biên dịch lại. Kiểm tra cửa sổ Output để đảm bảo các tệp (DLL, manifest, PDB) được sao chép vào C:\Users\Username\AppData\Roaming\Autodesk\Revit\Addins\2019.
Hành động trên màn hình:  

Hiển thị Visual Studio, vào Build > Build Solution.  
Hiển thị cửa sổ Output, highlight thông báo về việc sao chép tệp.  
Mở File Explorer tại C:\Users\Username\AppData\Roaming\Autodesk\Revit\Addins\2019, highlight các tệp được sao chép.  
Chèn text: “Post-build Event: Tự động sao chép tệp vào thư mục Add-ins.”




Phần 4: Kết luận và bước tiếp theo
Hướng dẫn viên (giọng điệu truyền cảm hứng):Chúc mừng các bạn! Hôm nay, chúng ta đã:

Biên dịch dự án để tạo tệp DLL và các tệp liên quan.  
Hiểu về thư mục add-ins của Revit.  
Thiết lập Post-build Event để tự động sao chép tệp vào thư mục add-ins.

Bây giờ, plugin của chúng ta đã sẵn sàng để chạy trong Revit! Trong bài học tiếp theo, chúng ta sẽ mở Revit, kiểm tra lệnh GetElementId, và gỡ lỗi nếu cần.
Bước 8: Kêu gọi hành độngHãy mở Revit và kiểm tra thư mục C:\Users\Username\AppData\Roaming\Autodesk\Revit\Addins\2019 để đảm bảo các tệp đã được sao chép. Nếu bạn gặp vấn đề khi biên dịch hoặc sao chép, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay!
Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!
Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"Bài học tiếp theo: Chạy và gỡ lỗi plugin trong Revit."  
"Gặp vấn đề? Để lại câu hỏi trong phần bình luận!"


Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio.

