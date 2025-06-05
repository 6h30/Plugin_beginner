2.6. Sửa đổi phần tử  

Chúng ta đã tạo một lệnh với **Transaction** trong phần trước. Bây giờ, chúng ta sẽ sử dụng lệnh này để xóa một phần tử được chọn trong Revit, đồng thời thêm xác nhận từ người dùng thông qua `TaskDialog`.

**Tra cứu phương thức Delete**:  
- Trong Visual Studio, mở **Object Browser**, tìm `Delete` trong lớp `Document`.  
- Phương thức `Delete(ElementId)` dùng để xóa phần tử dựa trên ID. Đảm bảo tùy chọn **Show inherited members** được bật để thấy phương thức này.

**Cập nhật lệnh DeleteElement**:  
1. **Thêm phương thức Delete**:  
   - Trong khối `using (Transaction trans = new Transaction(doc, "Delete Element"))`, sau `trans.Start()`, thêm dòng xóa phần tử:  
     - `doc.Delete(pickObj.ElementId);`.  

2. **Thêm xác nhận bằng TaskDialog**:  
   - Trước khi xóa, tạo `TaskDialog` để hỏi người dùng:  
     - `TaskDialog tDialog = new TaskDialog("Delete Element");`.  
     - Đặt nội dung: `tDialog.MainContent = "Are you sure you want to delete the element?";`.  
     - Thêm nút OK và Cancel: `tDialog.CommonButtons = TaskDialogCommonButtons.Ok | TaskDialogCommonButtons.Cancel;`.  
   - Kiểm tra lựa chọn người dùng:  
     - `if (tDialog.Show() == TaskDialogResult.Ok)`: Nếu chọn OK, gọi `trans.Commit()` và hiển thị thông báo xóa thành công.  
     - `else`: Nếu chọn Cancel, gọi `trans.RollBack()` và hiển thị thông báo không xóa.  

**Mã nguồn mẫu**:  
```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class DeleteElement : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            try
            {
                Reference pickObj = uiDoc.Selection.PickObject(ObjectType.Element);
                if (pickObj != null)
                {
                    TaskDialog tDialog = new TaskDialog("Delete Element");
                    tDialog.MainContent = "Are you sure you want to delete the element?";
                    tDialog.CommonButtons = TaskDialogCommonButtons.Ok | TaskDialogCommonButtons.Cancel;

                    if (tDialog.Show() == TaskDialogResult.Ok)
                    {
                        using (Transaction trans = new Transaction(doc, "Delete Element"))
                        {
                            trans.Start();
                            doc.Delete(pickObj.ElementId);
                            trans.Commit();
                        }
                        TaskDialog.Show("Delete", "Element ID: " + pickObj.ElementId.ToString() + " deleted.");
                    }
                    else
                    {
                        using (Transaction trans = new Transaction(doc, "Delete Element"))
                        {
                            trans.Start();
                            trans.RollBack();
                        }
                        TaskDialog.Show("Delete", "Element ID: " + pickObj.ElementId.ToString() + " not deleted.");
                    }
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

**Cập nhật tệp manifest (.addin)**:  
- Đảm bảo lệnh `DeleteElement` đã được thêm vào `MyRevitCommands.addin` (xem phần 2.5). Nếu chưa, sao chép khối `<AddIn>` từ lệnh trước và cập nhật:  
  - `<AddInId>`: Tạo GUID mới (**Tools > Create GUID**).  
  - `<FullClassName>`: `MyRevitCommands.DeleteElement`.  
  - `<Name>` và `<Text>`: `DeleteElement`.  
  - `<Description>`: “Deletes a selected element after user confirmation”.  

**Kiểm tra lệnh**:  
1. Nhấn **Start** trong Visual Studio để chạy Debug.  
2. Mở Revit, tạo dự án mới (dùng **Architectural Template**).  
3. Vào **Add-Ins > External Tools > DeleteElement**, chọn một phần tử (ví dụ: tường hoặc section).  
4. Xác nhận xóa trong `TaskDialog`:  
   - Nhấn **OK**: Phần tử bị xóa, thông báo xác nhận hiển thị.  
   - Nhấn **Cancel**: Phần tử không bị xóa, thông báo hủy hiển thị.  

**Mục tiêu**  
- Sử dụng phương thức `Delete` để xóa phần tử trong Revit.  
- Tích hợp `TaskDialog` để xác nhận hành động từ người dùng.  
- Quản lý `Transaction` với `Commit` và `Rollback` để đảm bảo thay đổi được kiểm soát.  

**Lưu ý quan trọng**: Luôn đóng giao dịch (`Commit` hoặc `Rollback`) sau khi mở (`Start`) để tránh lỗi. Trong các phần tiếp theo, bạn sẽ học cách áp dụng `Transaction` cho các thay đổi phức tạp hơn trong mô hình Revit.