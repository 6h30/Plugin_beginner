1.2. Xem tài liệu Revit API  

Để hiểu và sử dụng Revit API, bạn cần làm quen với tài liệu hướng dẫn đi kèm. Một cách đơn giản là cài đặt **Revit Software Development Kit (SDK)** khi cài đặt Revit, từ đó truy cập tệp **RevitAPI.CHM** – tài liệu chính thức của Revit API.

**Cách cài đặt và truy cập SDK**:  
1. **Tải SDK**: Nếu bạn đã có trình cài đặt Revit, vào phần **Install Tools and Utilities** và chọn **Revit Software Development Kit**. Nếu không, tải SDK từ trang web chính thức của Autodesk (developer.autodesk.com).  
2. **Cài đặt SDK**: Chạy trình cài đặt, chọn đường dẫn lưu SDK (mặc định thường là `C:\Revit SDK [phiên bản]`, ví dụ: `Revit SDK 2022`). Nhấn **Install** và chờ hoàn tất.  
3. **Truy cập tài liệu**: Sau khi cài đặt, mở thư mục SDK (ví dụ: `C:\Revit SDK 2022`). Bạn sẽ thấy các tệp và thư mục hỗ trợ lập trình Revit, bao gồm:  
   - Các mẫu ví dụ (sample files).  
   - Hướng dẫn bắt đầu (getting started guides).  
   - Tệp **RevitAPI.CHM** (tệp HTML được biên dịch, định dạng tài liệu trợ giúp của Microsoft).  

**Tệp RevitAPI.CHM**:  
- Mở tệp **RevitAPI.CHM** bằng cách nhấp đúp chuột. Tệp này chứa tài liệu chi tiết về Revit API, cho phép bạn duyệt qua các **namespace** (không gian tên), **class** (lớp), và **prototype** (nguyên mẫu).  
- **Namespace** là gì? Namespace là cách tổ chức mã lập trình, nhóm các đối tượng liên quan để dễ quản lý. Ví dụ, hai lớp có tên giống nhau nhưng thuộc namespace khác nhau sẽ không xung đột.  
- Namespace chính mà bạn sẽ sử dụng là **Autodesk.Revit.DB**, chứa các lớp, giao diện và liệt kê (enumerations) để làm việc với các phần tử (như tường, cột), hình học, và thông tin dự án trong Revit.  

**Khám phá Autodesk.Revit.DB**:  
- Mở namespace **Autodesk.Revit.DB** trong tệp CHM, bạn sẽ thấy các lớp quen thuộc như **Wall**, **Column**, hoặc **ElevationMarker**. Ví dụ, lớp **ElevationMarker** giúp bạn tạo điểm đánh dấu cao độ trong Revit.  
- Hãy dành thời gian xem qua các lớp trong namespace này để hiểu cách API tương tác với các phần tử trong Revit.

**Mục tiêu**  
- Biết cách cài đặt và truy cập Revit SDK.  
- Hiểu cách sử dụng tệp **RevitAPI.CHM** để tra cứu tài liệu API.  
- Làm quen với namespace **Autodesk.Revit.DB** và các lớp liên quan.  

Tệp **RevitAPI.CHM** là nguồn tài liệu quan trọng giúp bạn hiểu các công cụ Revit API cung cấp. Từ đây, bạn có thể bắt đầu xây dựng plugin đầu tiên, ví dụ như tạo một polyline hoặc tự động hóa một tác vụ đơn giản trong Revit.
