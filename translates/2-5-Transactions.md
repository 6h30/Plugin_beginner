### **2-5. Làm việc với Transactions trong Revit API**  

Trong các phần trước của khóa học, chúng ta đã sử dụng Revit API để truy cập các phần tử và dữ liệu từ mô hình Revit, nhưng chưa thực sự thay đổi mô hình. Để thực hiện các thay đổi, chúng ta cần tìm hiểu về **Transactions** (giao dịch) trong Revit.  

### **1. Transactions là gì?**  
Mọi thay đổi đối với mô hình Revit đều phải được đóng gói bên trong một **Transaction** đang hoạt động. Nếu không, hệ thống sẽ báo lỗi (**exception**).  

Chúng ta tạo một Transaction bằng cách sử dụng lớp **Transaction**. Sau khi khởi tạo, lớp này cung cấp một số phương thức quan trọng:  
- **Start()**: Bắt đầu một Transaction.  
- **Commit()**: Xác nhận thay đổi vào mô hình.  
- **RollBack()**: Hủy bỏ mọi thay đổi (trở về trạng thái trước khi Transaction bắt đầu).  

Một khi Transaction được **Commit**, các thay đổi sẽ trở thành một phần của mô hình.  

> **Lưu ý quan trọng:**  
> - Chỉ **một Transaction** có thể hoạt động tại một thời điểm.  
> - Luôn đảm bảo Transaction được bao bọc trong **`using`** hoặc **`try-catch`** để tránh việc Transaction vô tình vẫn ở trạng thái hoạt động.  

### **2. Tạo một lệnh thay đổi mô hình**  
Trong ví dụ này, chúng ta sẽ tạo một lệnh **DeleteElement** để xóa một phần tử khỏi mô hình.  

#### **Các bước thực hiện:**  
1. **Thay đổi thuộc tính của lệnh** từ **ReadOnly** sang **Manual** để cho phép tạo Transactions.  
2. **Bọc Transaction trong `using`** để đảm bảo nó được xử lý an toàn.  
3. **Bắt đầu Transaction** bằng phương thức `Start()`.  
4. **Thực hiện thay đổi** (trong ví dụ này là xóa phần tử).  
5. **Commit** để áp dụng thay đổi.  

#### **Code ví dụ:**  
```csharp
[Transaction(TransactionMode.Manual)] // Đổi từ ReadOnly sang Manual
public class DeleteElement : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        UIDocument uidoc = commandData.Application.ActiveUIDocument;
        Document doc = uidoc.Document;

        try
        {
            // Chọn một phần tử từ mô hình
            Reference obj = uidoc.Selection.PickObject(ObjectType.Element);
            if (obj != null)
            {
                // Bắt đầu Transaction
                using (Transaction trans = new Transaction(doc, "Delete Element"))
                {
                    trans.Start(); // Bắt đầu Transaction

                    // Lấy phần tử và xóa
                    Element ele = doc.GetElement(obj.ElementId);
                    doc.Delete(ele.Id);

                    trans.Commit(); // Xác nhận thay đổi
                }
            }
            return Result.Succeeded;
        }
        catch (Exception ex)
        {
            message = ex.Message;
            return Result.Failed;
        }
    }
}
```

### **3. Giải thích chi tiết**  
- **`TransactionMode.Manual`**: Cho phép lệnh tạo và quản lý Transactions thủ công.  
- **`using (Transaction trans = ...)`**: Đảm bảo Transaction được giải phóng đúng cách, ngay cả khi có lỗi.  
- **`trans.Start()` & `trans.Commit()`**: Mọi thay đổi (như `doc.Delete()`) chỉ có hiệu lực sau khi `Commit()`.  
- **Undo/Redo**: Tên Transaction ("Delete Element") sẽ xuất hiện trong menu **Undo** của Revit.  

### **4. Kết luận**  
Transactions là cơ chế **bắt buộc** khi muốn thay đổi mô hình Revit qua API. Việc sử dụng đúng cách giúp:  
- Đảm bảo tính toàn vẹn dữ liệu.  
- Hỗ trợ Undo/Redo.  
- Tránh lỗi do nhiều Transaction chồng chéo.  

Chúng ta sẽ tiếp tục áp dụng Transactions trong các bài tập tiếp theo! 🚀