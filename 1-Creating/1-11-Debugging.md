1.11. Gỡ lỗi (Debugging) 
 
Chúng ta đã biên dịch mã và sao chép tệp `.dll` cùng `.addin` vào thư mục Add-in của Revit. Bây giờ, chúng ta sẽ chạy và gỡ lỗi lệnh trực tiếp từ Visual Studio để kiểm tra và sửa lỗi (nếu có).

**Thiết lập chạy Revit trong Debug Mode**:  
1. Trong Visual Studio, vào **Debug > [Project Name] Properties** (hoặc nhấp chuột phải vào dự án trong **Solution Explorer**, chọn **Properties**).  
2. Trong tab **Debug**, chọn **Start external program**, sau đó nhấp **Browse**.  
3. Điều hướng đến thư mục cài đặt Revit (thường là `C:\Program Files\Autodesk\Revit [phiên bản, ví dụ: 2022]`), chọn tệp `Revit.exe`, và nhấp **Open**.  
4. Lưu cài đặt bằng cách đóng cửa sổ **Properties**.

**Chạy và kiểm tra lệnh**:  
1. Nhấn nút **Start** (biểu tượng tam giác xanh) trong Visual Studio để chạy ở chế độ Debug.  
2. Revit sẽ khởi động. Nếu xuất hiện thông báo về Add-in chưa đăng ký (unregistered), chọn **Always Load** (vì chúng ta không phân phối plugin, không cần chứng chỉ).  
3. Trong Revit, tạo một dự án mới (sử dụng **Architectural Template**).  
4. Vào tab **Add-Ins > External Tools**, chọn lệnh **Get Element ID** (tên và mô tả được lấy từ tệp `.addin`).  
5. Chạy lệnh, chọn một phần tử (như tường hoặc cột). Một cửa sổ `TaskDialog` sẽ hiển thị ID của phần tử.  
6. Kiểm tra trường hợp lỗi: Chạy lại lệnh và nhấn **Esc** để hủy. Bạn sẽ thấy thông báo lỗi: “The user aborted the pick operation” (Người dùng đã hủy thao tác chọn).

**Gỡ lỗi với Breakpoint**:  
1. Trong Visual Studio, mở tệp `GetElementId.cs`.  
2. Đặt một **breakpoint** tại dòng muốn kiểm tra (ví dụ: dòng chứa `pickObj = uiDoc.Selection.PickObject(...)`). Nhấp chuột trái vào cột xám bên trái số dòng để thêm breakpoint (hiển thị vòng tròn đỏ).  
3. Chạy lại Debug bằng nút **Start**. Revit khởi động, và khi lệnh chạy đến dòng có breakpoint, Visual Studio sẽ tạm dừng.  
4. Trong cửa sổ **Autos** (hoặc **Locals**) ở dưới cùng, bạn có thể xem giá trị của các biến (như `pickObj`). Nhấp vào mũi tên bên cạnh biến để xem chi tiết thuộc tính.  
5. Để kiểm tra từng dòng mã:  
   - Nhấn nút **Step Into** (mũi tên xanh xuống) để chạy từng dòng và theo dõi sự thay đổi của dữ liệu.  
   - Nhấn **Continue** để tiếp tục chạy lệnh mà không dừng lại.  
6. Để dừng gỡ lỗi, nhấn nút **Stop Debugging** (hình vuông đỏ) trong Visual Studio.

**Mã nguồn mẫu (từ phần trước)**:  
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
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            try
            {
                Reference pickObj = uiDoc.Selection.PickObject(ObjectType.Element);
                if (pickObj != null)
                {
                    TaskDialog.Show("Element ID", pickObj.ElementId.ToString());
                }
            }
            catch (Exception e)
            {
                message = e.Message;
                return Result.Failed;
            }
            return Result.Succeeded;
        }
    }
}
```

**Mục tiêu**  
- Thiết lập Visual Studio để chạy và gỡ lỗi trực tiếp trong Revit.  
- Kiểm tra lệnh bằng cách chạy trong Revit và xác nhận kết quả.  
- Sử dụng breakpoint và công cụ gỡ lỗi để kiểm tra biến và luồng thực thi mã.  

Giờ bạn đã có thể chạy, kiểm tra và gỡ lỗi plugin trong Revit. Trong các bước tiếp theo, bạn có thể mở rộng lệnh hoặc tạo các lệnh mới để tự động hóa các tác vụ khác.