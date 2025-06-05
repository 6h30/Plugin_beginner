1.6. Tạo một lệnh Revit  
Sau khi hiểu về giao diện `IExternalCommand`, chúng ta sẽ sử dụng nó để tạo một lệnh trong dự án Visual Studio. Lệnh này sẽ cho phép chọn một phần tử trong Revit (như tường hoặc cột) và lấy ID của nó. Hướng dẫn này tiếp nối từ bước thêm Revit API (phần 1.4). Nếu bạn chưa có dự án, hãy mở tệp dự án từ thư mục bài tập.

**Các bước tạo lệnh**:  
1. **Đổi tên lớp**:  
   - Trong **Solution Explorer** (bên phải), nhấp chuột phải vào lớp mặc định (thường là `Class1.cs`), chọn **Rename**.  
   - Đặt tên mô tả chức năng, ví dụ: `GetElementId.cs`. Nhấn **Enter**, sau đó chọn **Yes** để cập nhật tất cả tham chiếu.  

2. **Triển khai IExternalCommand**:  
   - Sau tên lớp, thêm dấu hai chấm (`:`) và gõ `IExternalCommand` (IntelliSense sẽ gợi ý vì chúng ta đã thêm `using Autodesk.Revit.UI` ở phần trước).  
   - Nếu xuất hiện gạch đỏ dưới `IExternalCommand`, điều này báo lỗi vì phương thức `Execute` chưa được triển khai.  
   - Nhấp vào biểu tượng bóng đèn (hoặc nhấp chuột phải), chọn **Implement interface** để Visual Studio tự động tạo phương thức `Execute`.  

3. **Hiểu phương thức Execute**:  
   Phương thức `Execute` có ba tham số:  
   - **ExternalCommandData commandData**: Chứa thông tin về ứng dụng Revit và khung nhìn hiện tại.  
   - **ref string message**: Chuỗi tham chiếu để gửi thông báo lỗi hoặc thông tin nếu lệnh thất bại/hủy.  
   - **ElementSet elements**: Tập hợp phần tử để đánh dấu nếu lệnh gặp lỗi.  
   - Hiện tại, phương thức `Execute` có dòng `throw new NotImplementedException()`. Hãy xóa dòng này và thay bằng `return Result.Succeeded;` để trả về trạng thái lệnh thành công. Các giá trị trả về có thể là:  
     - `Result.Succeeded`: Lệnh chạy thành công.  
     - `Result.Canceled`: Người dùng hủy lệnh.  
     - `Result.Failed`: Lệnh gặp lỗi.  

4. **Thêm thuộc tính Transaction**:  
   - Trước khai báo lớp, thêm thuộc tính `[Transaction(TransactionMode.ReadOnly)]`.  
   - Dòng này khai báo cách lệnh sử dụng giao dịch (transaction) trong Revit, có thể là `Manual` hoặc `ReadOnly`. Với lệnh đơn giản như lấy ID, dùng `ReadOnly` vì không cần sửa đổi dữ liệu Revit.  
   - Thuộc tính này là bắt buộc cho mọi lệnh sử dụng `IExternalCommand`.  

**Mã nguồn mẫu**:  
```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.ReadOnly)]
    public class GetElementId : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return Result.Succeeded;
        }
    }
}
```

**Mục tiêu**  
- Tạo và đổi tên lớp để triển khai `IExternalCommand`.  
- Hiểu cấu trúc phương thức `Execute` và các tham số của nó.  
- Thêm thuộc tính `Transaction` để đảm bảo lệnh hoạt động đúng trong Revit.  

Hiện tại, lệnh của chúng ta chưa làm gì ngoài việc trả về trạng thái thành công. Trong các bước tiếp theo, chúng ta sẽ thêm mã để chọn phần tử và lấy ID của nó trong Revit.
