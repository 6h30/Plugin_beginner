1.3. Thiết lập Visual Studio  
Sau khi đã làm quen với Revit API, chúng ta sẽ bắt đầu tạo plugin Revit đầu tiên bằng **Visual Studio** – một môi trường phát triển tích hợp (IDE). Trong hướng dẫn này, chúng ta sử dụng Visual Studio, nhưng bạn có thể dùng IDE khác nếu muốn. Tuy nhiên, các bước dưới đây sẽ dựa trên Visual Studio.

**Cài đặt Visual Studio**:  
1. **Tải Visual Studio**: Truy cập **visualstudio.microsoft.com**, di chuột đến mục **Download for Windows**, chọn phiên bản miễn phí **Community 2022** (hoặc phiên bản phù hợp). Tải và chạy trình cài đặt.  
2. **Chọn thành phần cài đặt**: Trong trình cài đặt, chọn **.NET desktop development** để phát triển plugin .NET. Nếu bạn tạo plugin cho Revit 2022, đảm bảo chọn **.NET Framework 4.8 targeting pack** trong tab **Individual Components** (hoặc phiên bản phù hợp với Revit bạn dùng: Revit 2017/2018 cần .NET Framework 4.6, Revit 2015/2016 cần .NET Framework 4.5). Nhấn **Install** và chờ hoàn tất.

**Tạo dự án mới trong Visual Studio**:  
1. Mở Visual Studio, vào **File > New > Project**.  
2. Chọn **Class Library (.NET Framework)** trong mục **Visual C#** (dự án này tạo thư viện cho plugin Revit).  
3. Chọn phiên bản .NET Framework phù hợp với Revit:  
   - Revit 2022: .NET Framework 4.8.  
   - Revit 2019: .NET Framework 4.7.  
   - Revit 2017/2018: .NET Framework 4.6.  
   - Revit 2015/2016: .NET Framework 4.5.  
   Nếu phiên bản .NET Framework cần thiết không xuất hiện, vào **Tools > Get Tools and Features > Individual Components**, chọn Framework phù hợp, cài đặt và khởi động lại Visual Studio.  
4. Đặt tên dự án, ví dụ: **MyRevitCommands** (tên này sẽ chứa các lệnh tùy chỉnh cho Revit).  
5. Chọn đường dẫn lưu dự án (mặc định thường là `C:\Users\[Tên người dùng]\source\repos`). Đảm bảo chọn **Create directory for solution** và bỏ chọn **Add to source control**. Nhấn **OK** để tạo dự án.

**Mục tiêu**  
- Cài đặt Visual Studio và các thành phần cần thiết (.NET Framework).  
- Tạo dự án Class Library (.NET Framework) cho plugin Revit.  
- Đảm bảo chọn đúng phiên bản .NET Framework tương thích với Revit.  

Dự án của bạn giờ đã sẵn sàng để bắt đầu phát triển plugin Revit. Trong các bước tiếp theo, chúng ta sẽ thêm Revit API và bắt đầu viết mã lệnh.
