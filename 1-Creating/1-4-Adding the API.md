1.4. Thêm Revit API vào dự án 

Chúng ta đã thiết lập dự án plugin Revit trong Visual Studio, nhưng hiện tại dự án chỉ chứa một lớp cơ bản chưa thể truy cập Revit API. Để sử dụng các lớp và loại dữ liệu trong Revit API, chúng ta cần thêm các tệp API vào dự án.

**Thêm tệp Revit API**:  
1. Trong Visual Studio, mở **Solution Explorer** (bên phải), nhấp vào **References** để xem các thư viện (assemblies) hiện có.  
2. Nhấp chuột phải vào **References**, chọn **Add Reference**.  
3. Vì tệp Revit API không nằm trong danh sách thư viện tiêu chuẩn, nhấp vào **Browse** (nút dưới cùng bên phải).  
4. Điều hướng đến thư mục cài đặt Revit (thường là `C:\Program Files\Autodesk\Revit [phiên bản]`, ví dụ: `Revit 2022`).  
5. Tìm và chọn hai tệp: **RevitAPI.dll** và **RevitAPIUI.dll**. Giữ phím **Ctrl** và nhấp chuột trái để chọn cả hai, sau đó nhấp **Add**.  
6. Trong cửa sổ **Reference Manager**, đảm bảo cả hai tệp được đánh dấu chọn (tick boxes), sau đó nhấp **OK**.  
7. Trong **Solution Explorer**, bạn sẽ thấy **RevitAPI** và **RevitAPIUI** xuất hiện trong **References**.  
8. Chọn từng tệp (**RevitAPI** và **RevitAPIUI**) trong **Solution Explorer**, mở **Properties** (thường ở góc dưới bên phải), và đặt **Copy Local** thành **False**. Điều này đảm bảo tệp DLL không được sao chép khi biên dịch, vì Revit đã tự động tải chúng khi khởi động.

**Khám phá Revit API bằng Object Browser**:  
1. Trong **Solution Explorer**, nhấp đúp vào **RevitAPI.dll** để mở **Object Browser**.  
2. Cửa sổ này hiển thị các namespace, lớp và loại dữ liệu trong Revit API. Mở rộng **Autodesk.Revit.DB** để xem các lớp quen thuộc (như **Wall**, **Element**).  
3. Sử dụng thanh tìm kiếm trong **Object Browser** để tra cứu, ví dụ: nhập “Element” để tìm lớp **Element**. Khi chọn một lớp, bạn sẽ thấy các phương thức (methods) và thuộc tính (properties) ở phía trên bên phải, cùng mô tả chi tiết ở phía dưới bên phải.

**Thêm namespace vào mã nguồn**:  
1. Mở tệp lớp chính (ví dụ: **Class1.cs**) trong dự án.  
2. Thêm các namespace thường dùng bằng cách thêm các dòng sau ở đầu tệp:  
   - `using Autodesk.Revit.DB;` – cung cấp quyền truy cập vào các lớp cơ sở dữ liệu như **Wall**, **Column**.  
   - `using Autodesk.Revit.UI;` – cho phép tương tác với giao diện Revit và ứng dụng.  
   - `using Autodesk.Revit.Attributes;` – hỗ trợ các thuộc tính cần thiết cho lệnh Revit (sẽ được giải thích ở phần sau).  

**Mục tiêu**  
- Thêm thành công **RevitAPI.dll** và **RevitAPIUI.dll** vào dự án.  
- Hiểu cách sử dụng **Object Browser** để tra cứu các lớp và phương thức trong Revit API.  
- Thêm các namespace cần thiết vào mã nguồn để sẵn sàng triển khai lệnh Revit đầu tiên.  

Giờ đây, dự án của bạn đã được thiết lập để sử dụng Revit API. Trong bước tiếp theo, chúng ta sẽ bắt đầu tạo lệnh Revit đầu tiên!
