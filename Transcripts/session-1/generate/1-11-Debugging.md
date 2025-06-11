Kịch bản bài học: Gỡ lỗi plugin trong Revit
Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ học cách chạy và gỡ lỗi lệnh GetElementId bằng chế độ debug trong Visual Studio. Sau bài học này, bạn sẽ biết:

Cách cấu hình Visual Studio để chạy Revit trong chế độ debug.
Cách xử lý thông báo về add-in chưa đăng ký trong Revit.
Cách thêm breakpoint và sử dụng các công cụ gỡ lỗi để kiểm tra biến và dữ liệu.

Hãy cùng bắt đầu!

Phần 1: Cấu hình Visual Studio để chạy Revit
Hướng dẫn viên (giọng điệu hào hứng):Chúng ta đã biên dịch dự án MyRevitCommands và tự động sao chép tệp DLL và manifest vào thư mục add-ins. Bây giờ, chúng ta sẽ cấu hình Visual Studio để chạy Revit trực tiếp và kiểm tra lệnh GetElementId trong chế độ debug, giúp phát hiện lỗi nếu có.

Bước 1: Cấu hình chạy Revit.exeTrong Visual Studio, vào menu Debug > Properties (hoặc nhấp chuột phải vào dự án MyRevitCommands và chọn Properties). Trong tab Debug, chọn tùy chọn Start external program và nhấn Browse. Điều hướng đến thư mục cài đặt Revit, thường là C:\Program Files\Autodesk\Revit 2019, và chọn Revit.exe. Nhấn Open để thêm.
Hành động trên màn hình:  

Hiển thị Visual Studio, vào Debug > Properties.  
Zoom vào tùy chọn Start external program, nhấn Browse.  
Hiển thị File Explorer tại C:\Program Files\Autodesk\Revit 2019, highlight Revit.exe, nhấn Open.  
Chèn text: “Cấu hình Revit.exe: Chạy Revit trực tiếp từ Visual Studio.”




Phần 2: Chạy và kiểm tra lệnh trong Revit
Hướng dẫn viên (giọng điệu rõ ràng):Hãy chạy lệnh GetElementId trong chế độ debug để xem nó hoạt động thế nào trong Revit!

Bước 2: Chạy debug modeTrong Visual Studio, nhấp vào biểu tượng Play (hoặc nhấn F5) để chạy giải pháp ở chế độ debug. Revit sẽ khởi động. Khi Revit mở, bạn có thể thấy một cửa sổ thông báo rằng plugin chưa được đăng ký (do thiếu chứng chỉ đáng tin cậy). Vì chúng ta không định phát hành plugin, chọn Always Load để luôn tải plugin.
Hành động trên màn hình:  

Hiển thị Visual Studio, nhấp vào biểu tượng Play.  
Hiển thị Revit khởi động với cửa sổ thông báo “Unregistered Add-in”.  
Zoom vào nút Always Load, nhấn chọn.  
Chèn text: “Always Load: Cho phép Revit tải plugin chưa đăng ký.”


Bước 3: Kiểm tra lệnh trong RevitTrong Revit, tạo một dự án mới bằng cách nhấp New và chọn Architectural Template. Chuyển đến tab Add-Ins > External Tools, bạn sẽ thấy lệnh Get Element ID (tên và mô tả được định nghĩa trong tệp manifest). Nhấp vào lệnh, sau đó chọn một phần tử (như một bức tường). Một hộp thoại TaskDialog sẽ hiện ra, hiển thị Element ID của phần tử.
Hành động trên màn hình:  

Hiển thị Revit, nhấp New > Architectural Template.  
Zoom vào tab Add-Ins > External Tools, highlight Get Element ID.  
Mô phỏng chọn một bức tường, hiển thị hộp thoại TaskDialog với Element ID.  
Chèn text: “Lệnh GetElementId: Hiển thị Element ID của phần tử được chọn.”


Bước 4: Kiểm tra Try-CatchĐể kiểm tra khối try-catch trong mã, chạy lại lệnh Get Element ID và nhấn Esc để hủy. Một thông báo sẽ hiển thị: “The user aborted the pick operation.” Điều này xác nhận khối try-catch hoạt động đúng, bắt lỗi khi người dùng thoát.
Hành động trên màn hình:  

Hiển thị Revit, chạy lại lệnh Get Element ID, nhấn Esc.  
Hiển thị thông báo lỗi: “The user aborted the pick operation.”  
Chèn text: “Try-Catch: Xử lý lỗi khi người dùng hủy lệnh.”




Phần 3: Thêm Breakpoint và Gỡ lỗi
Hướng dẫn viên (giọng điệu khích lệ):Chế độ debug không chỉ giúp chạy lệnh mà còn cho phép chúng ta kiểm tra dữ liệu và phát hiện lỗi. Hãy thêm breakpoint để xem điều gì xảy ra bên trong mã khi lệnh chạy!

Bước 5: Dừng debug và thêm BreakpointQuay lại Visual Studio, nhấn biểu tượng Stop (hình vuông đỏ) để dừng phiên debug hiện tại. Mở tệp GetElementId.cs, tìm dòng mã nơi gọi PickObject (ví dụ: dòng 27). Nhấp vào cột màu xám bên trái số dòng để thêm breakpoint – một vòng tròn đỏ sẽ xuất hiện.
Hành động trên màn hình:  

Hiển thị Visual Studio, nhấn biểu tượng Stop.  
Mở GetElementId.cs, nhấp vào cột bên trái dòng Reference pickedObj = uidoc.Selection.PickObject(ObjectType.Element);.  
Highlight vòng tròn đỏ của breakpoint.  
Chèn text: “Breakpoint: Dừng mã tại dòng cụ thể để kiểm tra.”


Bước 6: Chạy lại Debug và Kiểm tra biếnNhấn Play để chạy lại debug mode. Trong Revit, tạo một dự án mới và chạy lệnh Get Element ID. Khi mã đến breakpoint, Visual Studio sẽ tạm dừng. Cửa sổ Autos (hoặc Locals) ở dưới cùng sẽ hiển thị các biến hiện tại, như pickedObj. Mở rộng pickedObj để xem các thuộc tính, như ElementId.
Hành động trên màn hình:  

Hiển thị Visual Studio, nhấn Play để chạy debug.  
Trong Revit, chạy lệnh Get Element ID, chọn một phần tử.  
Hiển thị Visual Studio tạm dừng tại breakpoint, zoom vào cửa sổ Autos, mở rộng pickedObj để xem ElementId.  
Chèn text: “Gỡ lỗi: Kiểm tra giá trị biến trong khi mã chạy.”


Bước 7: Bước qua mã (Step Into)Để kiểm tra mã từng dòng, nhấp vào biểu tượng Step Into (mũi tên xanh hướng xuống) ở thanh công cụ debug. Xem dữ liệu trong cửa sổ Autos cập nhật khi mã chạy qua từng dòng. Điều này rất hữu ích để tìm lỗi nếu lệnh hoạt động không như mong đợi. Để tiếp tục lệnh, nhấn Continue (mũi tên xanh) ở thanh công cụ.
Hành động trên màn hình:  

Hiển thị Visual Studio, nhấp Step Into, highlight dữ liệu cập nhật trong cửa sổ Autos.  
Nhấn Continue để tiếp tục lệnh.  
Chèn text: “Step Into: Kiểm tra mã từng dòng để tìm lỗi.”




Phần 4: Kết luận và bước tiếp theo
Hướng dẫn viên (giọng điệu truyền cảm hứng):Chúc mừng các bạn! Hôm nay, chúng ta đã:

Cấu hình Visual Studio để chạy Revit trong chế độ debug.  
Kiểm tra lệnh GetElementId trong Revit và xác nhận try-catch hoạt động.  
Thêm breakpoint và sử dụng công cụ gỡ lỗi để kiểm tra biến và dữ liệu.

Những công cụ gỡ lỗi này sẽ rất hữu ích khi bạn phát triển các plugin phức tạp hơn. Trong bài học tiếp theo, chúng ta sẽ khám phá cách cải tiến lệnh hoặc tạo các lệnh Revit mới.
Bước 8: Kêu gọi hành độngHãy thử chạy lại lệnh GetElementId và sử dụng breakpoint để kiểm tra các biến khác. Nếu bạn gặp vấn đề khi debug hoặc chạy plugin, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay!
Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!
Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"Bài học tiếp theo: Cải tiến hoặc tạo lệnh Revit mới."  
"Gặp vấn đề? Để lại câu hỏi trong phần bình luận!"


Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio, mô phỏng hộp thoại TaskDialog hiển thị Element ID.

