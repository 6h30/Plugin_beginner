1.5. Giao diện IExternalCommand  

Để tạo lệnh (command) cho plugin Revit, chúng ta sử dụng **IExternalCommand** – một giao diện (interface) trong C# giúp tạo các lệnh bên ngoài (external commands). Lệnh này sẽ xuất hiện trong tab **Add-Ins** của Revit, cho phép truy cập cơ sở dữ liệu Revit và các phần tử được chọn.

**Giao diện (interface) là gì?**  
Trong C#, giao diện là một loại định nghĩa chứa các phương thức và thuộc tính liên quan mà một lớp (class) có thể triển khai. Với `IExternalCommand`, lớp của bạn sẽ triển khai một phương thức chính là **Execute**, được gọi khi người dùng chạy lệnh trong Revit.

**Cách hoạt động của IExternalCommand**:  
- Khi biên dịch mã thành tệp **.dll** (dynamic link library), Revit sẽ đọc tệp này và thêm các lệnh triển khai `IExternalCommand` vào tab **Add-Ins**, miễn là chúng được định nghĩa đúng và có tệp manifest (sẽ được giải thích sau).  
- Khi lệnh được kích hoạt trong Revit, một đối tượng lệnh được tạo, phương thức **Execute** được gọi, và sau khi hoàn thành, đối tượng lệnh sẽ bị hủy.

**Phương thức Execute**:  
Phương thức **Execute** yêu cầu ba tham số do Revit cung cấp:  
1. **ExternalCommandData**: Đối tượng chứa tham chiếu đến ứng dụng Revit và khung nhìn hiện tại, cho phép truy cập toàn bộ dữ liệu Revit (như phần tử, thông số).  
2. **String (message)**: Một chuỗi tham chiếu để gửi thông báo lỗi hoặc thông tin đến người dùng nếu lệnh thất bại hoặc bị hủy. Nếu không thiết lập, lệnh sẽ thoát mà không hiển thị thông báo.  
3. **ElementSet**: Một tập hợp phần tử (ban đầu rỗng) từ Revit API, có thể dùng để đánh dấu các phần tử liên quan nếu lệnh thất bại.  

Phương thức **Execute** phải trả về một kết quả kiểu **Result** (liệt kê), với ba giá trị:  
- **Succeeded**: Lệnh thực thi thành công.  
- **Canceled**: Người dùng hủy lệnh, và chuỗi thông báo (nếu có) sẽ được hiển thị.  
- **Failed**: Lệnh gặp lỗi, chuỗi thông báo và tập hợp phần tử (nếu có) sẽ được trả về cho người dùng.  

Lưu ý: Bạn không bắt buộc phải sử dụng tham số **String** hoặc **ElementSet**, nhưng chúng hữu ích để cung cấp phản hồi cho người dùng khi có lỗi.

**Mục tiêu**  
- Hiểu vai trò của giao diện `IExternalCommand` trong việc tạo lệnh Revit.  
- Nắm rõ cấu trúc và tham số của phương thức **Execute**.  
- Chuẩn bị triển khai lệnh đầu tiên bằng cách tạo một lớp sử dụng `IExternalCommand`.  

Trong bước tiếp theo, chúng ta sẽ thực hành tạo một lệnh Revit cơ bản sử dụng giao diện `IExternalCommand` trong Visual Studio.
