1.9. Đăng ký Add-in  

Chúng ta đã viết xong lệnh đầu tiên cho plugin Revit trong Visual Studio. Bước tiếp theo là đăng ký lệnh này để nó xuất hiện trong Revit. Để làm điều đó, chúng ta cần tạo một **tệp manifest (.addin)**. Khi Revit khởi động, nó sẽ đọc các tệp manifest trong hai thư mục cụ thể để xác định plugin nào sẽ được tải và với các tùy chọn nào.

**Tạo tệp manifest (.addin)**:  
1. **Thêm tệp manifest**:  
   - Trong Visual Studio, nhấp chuột phải vào tên dự án trong **Solution Explorer**, chọn **Add > New Item**.  
   - Chọn **Application Manifest File**, đổi tên thành `MyRevitCommands.addin` (phải có đuôi `.addin` để Revit nhận diện), sau đó nhấp **Add**.  
   - Tệp mặc định sẽ chứa mã không cần thiết. Xóa toàn bộ nội dung hiện có.  

2. **Thêm nội dung manifest**:  
   - Tệp manifest sử dụng định dạng XML (không phải C#) để cấu hình plugin. Bạn có thể tham khảo mẫu từ Autodesk Revit Developer Guide hoặc sử dụng nội dung sau:  
     ```xml
     <?xml version="1.0" encoding="utf-8"?>
     <RevitAddIns>
       <AddIn Type="Command">
         <Name>MyRevitCommands</Name>
         <Assembly>MyRevitCommands.dll</Assembly>
         <AddInId>[Your-GUID-Here]</AddInId>
         <FullClassName>MyRevitCommands.GetElementId</FullClassName>
         <Text>Get Element ID</Text>
         <Description>Retrieves the ID of a selected element</Description>
         <VendorId>YourVendorID</VendorId>
         <VisibilityMode>NotVisibleWhenNoActiveDocument</VisibilityMode>
       </AddIn>
     </RevitAddIns>
     ```
   - **Giải thích các thẻ XML**:  
     - `<Name>`: Tên plugin, ví dụ: `MyRevitCommands`.  
     - `<Assembly>`: Tên tệp `.dll` đã biên dịch (chỉ cần tên, không cần đường dẫn nếu lưu trong thư mục Add-in của Revit).  
     - `<AddInId>`: Mã định danh duy nhất (GUID). Tạo GUID bằng cách vào **Tools > Create GUID** trong Visual Studio, chọn định dạng không có dấu ngoặc, nhấp **New GUID**, sao chép và dán vào thẻ này (xóa dấu ngoặc và dấu ngoặc kép nếu có).  
     - `<FullClassName>`: Tên không gian tên và lớp lệnh, ví dụ: `MyRevitCommands.GetElementId`.  
     - `<Text>`: Tên lệnh hiển thị trong Revit (tùy chọn).  
     - `<Description>`: Mô tả hiển thị khi di chuột vào lệnh (tùy chọn).  
     - `<VendorId>`: Mã định danh nhà cung cấp, ví dụ: tên bạn hoặc công ty.  
     - `<VisibilityMode>`: Quy định khi nào lệnh hiển thị. Sử dụng `NotVisibleWhenNoActiveDocument` (viết hoa chữ cái đầu, không dấu cách) để ẩn lệnh khi không có dự án mở.  

3. **Cấu hình tệp manifest**:  
   - Trong **Solution Explorer**, chọn tệp `MyRevitCommands.addin`.  
   - Trong cửa sổ **Properties** (góc dưới bên phải), đặt **Copy to Output Directory** thành **Copy if newer** để đảm bảo tệp được sao chép khi biên dịch.  

**Mục tiêu**  
- Hiểu vai trò của tệp manifest (.addin) trong việc đăng ký plugin với Revit.  
- Tạo và cấu hình tệp manifest với các thẻ XML cần thiết.  
- Tạo GUID duy nhất và đảm bảo tệp manifest được sao chép khi biên dịch.  

Sau khi hoàn thành tệp manifest, bạn đã sẵn sàng để biên dịch mã và kiểm tra plugin trong Revit ở bước tiếp theo.