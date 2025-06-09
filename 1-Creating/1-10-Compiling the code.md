1.10. Biên dịch mã  

Chúng ta đã tạo lệnh và tệp manifest (.addin) để đăng ký plugin với Revit. Bước tiếp theo là biên dịch mã thành tệp `.dll` (dynamic link library) và sao chép tệp vào thư mục Add-in của Revit để plugin có thể hoạt động.

**Biên dịch mã**:  
1. Trong Visual Studio, vào **Build > Build Solution** để biên dịch dự án.  
2. Sau khi biên dịch, kiểm tra cửa sổ **Output** (dưới cùng màn hình). Nếu thành công, bạn sẽ thấy thông báo “Build: 1 succeeded”. Nếu có lỗi, Visual Studio sẽ hiển thị chi tiết trong cửa sổ này để bạn sửa.  
3. Kết quả biên dịch tạo ra ba tệp trong thư mục đầu ra (thường là `bin\Debug`):  
   - `MyRevitCommands.dll`: Tệp chứa mã plugin.  
   - `MyRevitCommands.addin`: Tệp manifest đã tạo.  
   - `MyRevitCommands.pdb`: Tệp cơ sở dữ liệu chương trình, dùng khi gỡ lỗi (sẽ tìm hiểu sau).  

**Sao chép tệp vào thư mục Add-in của Revit**:  
Revit tìm các plugin trong hai thư mục khi khởi động:  
- **Thư mục chung**: `C:\ProgramData\Autodesk\Revit\Addins\[phiên bản, ví dụ: 2022]` (dành cho tất cả người dùng).  
- **Thư mục người dùng**: `C:\Users\[Tên người dùng]\AppData\Roaming\Autodesk\Revit\Addins\[phiên bản]` (riêng cho từng người dùng).  

Chúng ta sẽ dùng thư mục người dùng để lưu tệp. Bạn có thể sao chép thủ công các tệp `.dll` và `.addin` vào thư mục này, nhưng để tự động hóa, hãy thiết lập sự kiện sau biên dịch (post-build event) trong Visual Studio.

**Thiết lập Post-build Event**:  
1. Trong Visual Studio, vào **Project > MyRevitCommands Properties**.  
2. Chọn tab **Build Events** (bên trái).  
3. Nhấp vào **Edit Post-build** và nhập lệnh sao chép:  
   ```cmd
   copy "$(TargetDir)*.*" "$(AppData)\Autodesk\Revit\Addins\2022\"
   ```
   - `$(TargetDir)*.*`: Sao chép tất cả tệp từ thư mục đầu ra (nơi chứa `.dll`, `.addin`, `.pdb`).  
   - `$(AppData)\Autodesk\Revit\Addins\2022\`: Đường dẫn đến thư mục Add-in của người dùng (thay `2022` bằng phiên bản Revit bạn dùng).  
4. Nhấn **OK**, sau đó vào **Build > Build Solution** để kiểm tra.  
5. Kiểm tra cửa sổ **Output** để xác nhận các tệp đã được sao chép vào thư mục Add-in.

**Mục tiêu**  
- Biên dịch mã thành công để tạo tệp `.dll` và `.addin`.  
- Hiểu vai trò của thư mục Add-in và cách Revit đọc tệp manifest.  
- Tự động hóa việc sao chép tệp vào thư mục Add-in bằng sự kiện post-build.  

Sau khi biên dịch và sao chép tệp, bạn có thể mở Revit, vào tab **Add-Ins**, và kiểm tra xem lệnh `Get Element ID` có xuất hiện và hoạt động như mong đợi không.