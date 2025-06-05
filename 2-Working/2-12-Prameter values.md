2.12. Giá trị tham số (Parameter Values) 
 
Sau khi học cách lấy thông tin tham số (parameter) như tên, loại đơn vị, và mã định danh tích hợp (BuiltInParameter), chúng ta sẽ tìm hiểu cách lấy và đặt giá trị tham số. Trong phần này, chúng ta sẽ tạo lệnh `SetParameter` để lấy giá trị tham số “Head Height” của một cửa sổ (window) và đặt giá trị mới cho nó.

**Lấy tham số bằng BuiltInParameter**:  
- Sử dụng `get_Parameter(BuiltInParameter)` để lấy tham số, thay vì `LookupParameter`, vì phương thức này ổn định hơn (tránh trùng lặp tên tham số từ tham số dự án).  
- Ví dụ: `BuiltInParameter.INSTANCE_HEAD_HEIGHT_PARAM` cho tham số “Head Height”.  
- **Lưu ý**: Cần thêm tiền tố `get_` vì đây là **index property**, một đặc điểm của C# khi truy cập thuộc tính giống như phương thức.

**Lấy giá trị tham số**:  
- **StorageType**: Loại dữ liệu của tham số (`Double`, `String`, `Integer`, `ElementId`, hoặc `None`). Lấy bằng `param.StorageType`.  
- **Giá trị**: Dùng phương thức phù hợp với `StorageType`:  
  - `AsDouble()`: Lấy giá trị `double` (ví dụ: chiều cao, độ dài).  
  - `AsString()`: Lấy giá trị chuỗi.  
  - `AsInteger()`: Lấy giá trị số nguyên.  
  - `AsElementId()`: Lấy giá trị ID của phần tử.  
- **Cảnh báo**: Sử dụng sai phương thức (ví dụ: gọi `AsDouble()` trên tham số `String`) sẽ gây lỗi.  

**Đặt giá trị tham số**:  
- Sử dụng `Set(value)` để đặt giá trị mới, phù hợp với `StorageType`.  
- Ví dụ: `param.Set(7.5)` để đặt chiều cao 7.5 feet.  
- Cần thực hiện trong một giao dịch (`Transaction`) vì đây là thay đổi mô hình.  
- Có thêm phương thức `SetValueString(string)` để đặt giá trị dưới dạng chuỗi (theo đơn vị hiển thị), nhưng ở đây chúng ta dùng `Set` để làm việc trực tiếp với đơn vị nội bộ (feet).

**Tạo lệnh SetParameter**:  
1. **Tạo lớp lệnh**:  
   - Tạo lớp `SetParameter.cs`, triển khai `IExternalCommand`.  
   - Đặt thuộc tính `[Transaction(TransactionMode.Manual)]` để cho phép thay đổi mô hình.  
   - Thêm namespace: `Autodesk.Revit.DB`, `Autodesk.Revit.UI`, `Autodesk.Revit.Attributes`.  

2. **Lấy tham số và giá trị**:  
   - Sử dụng `PickObject` để chọn phần tử, lấy `Element`:  
     - `Element ele = doc.GetElement(pickObj);`.  
   - Lấy tham số “Head Height” bằng `BuiltInParameter`:  
     - `Parameter param = ele.get_Parameter(BuiltInParameter.INSTANCE_HEAD_HEIGHT_PARAM);`.  
   - Hiển thị `StorageType` và giá trị bằng `TaskDialog`:  
     - `TaskDialog.Show("Parameter Values", string.Format("Storage Type: {0}\nValue: {1}", param.StorageType.ToString(), param.AsDouble()));`.  

3. **Đặt giá trị mới**:  
   - Trong khối `using (Transaction trans = new Transaction(doc, "Set Head Height"))`:  
     - Gọi `trans.Start()`.  
     - Đặt giá trị mới: `param.Set(7.5);` (7.5 feet).  
     - Gọi `trans.Commit()`.  

**Mã nguồn mẫu**:  
```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class SetParameter : IExternalCommand
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
                    Element ele = doc.GetElement(pickObj);
                    Parameter param = ele.get_Parameter(BuiltInParameter.INSTANCE_HEAD_HEIGHT_PARAM);
                    if (param != null)
                    {
                        // Hiển thị StorageType và giá trị hiện tại
                        TaskDialog.Show("Parameter Values",
                            string.Format("Storage Type: {0}\nValue: {1}", param.StorageType.ToString(), param.AsDouble()));

                        // Đặt giá trị mới
                        using (Transaction trans = new Transaction(doc, "Set Head Height"))
                        {
                            trans.Start();
                            param.Set(7.5); // Đặt chiều cao 7.5 feet
                            trans.Commit();
                        }
                    }
                    else
                    {
                        TaskDialog.Show("Error", "Head Height parameter not found.");
                        return Result.Failed;
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
- Mở `MyRevitCommands.addin`, sao chép khối `<AddIn>` từ lệnh trước.  
- Cập nhật:  
  - `<AddInId>`: Tạo GUID mới (**Tools > Create GUID**).  
  - `<FullClassName>`: `MyRevitCommands.SetParameter`.  
  - `<Name>` và `<Text>`: `SetParameter`.  
  - `<Description>`: “Gets and sets the Head Height parameter of a selected window”.  

**Kiểm tra lệnh**:  
1. Nhấn **Start** trong Visual Studio để chạy Debug.  
2. Mở Revit, mở tệp dự án từ thư mục bài tập.  
3. Vào **Add-Ins > External Tools > SetParameter`, chọn một cửa sổ (window).  
4. Kiểm tra `TaskDialog` hiển thị:  
   - **Storage Type**: Double.  
   - **Value**: 7.00 (feet, đơn vị nội bộ của Revit).  
5. Chuyển sang chế độ 3D, kiểm tra cửa sổ đã được cập nhật chiều cao “Head Height” thành 7.5 feet.  
6. **Lưu ý**: Nếu chọn phần tử không phải cửa sổ (không có tham số “Head Height”), lệnh sẽ hiển thị lỗi. Kiểm tra `param != null` để đảm bảo an toàn.

**Mục tiêu**  
- Hiểu cách lấy giá trị tham số bằng `AsDouble`, `AsString`, v.v., dựa trên `StorageType`.  
- Sử dụng `Set` để đặt giá trị mới cho tham số trong một giao dịch.  
- Đảm bảo kiểm tra loại giá trị để tránh lỗi khi làm việc với tham số.  

**Lưu ý an toàn**: Luôn kiểm tra `StorageType` và `param != null` trước khi lấy hoặc đặt giá trị để tránh lỗi. Trong các phần tiếp theo, bạn sẽ học cách làm việc với nhiều tham số hoặc tự động hóa cập nhật tham số hàng loạt trong Revit.