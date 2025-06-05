2.11. Lấy tham số (Get Parameter)  

Trong Revit, mỗi phần tử có các tham số (parameters) để điều chỉnh đặc tính, như chiều cao, chiều rộng, hoặc vật liệu. Revit API cho phép truy cập và chỉnh sửa các tham số này thông qua mã lệnh. Trong phần này, chúng ta sẽ lấy thông tin tham số “Head Height” của một cửa sổ (window) và hiển thị các thuộc tính như tên, loại đơn vị, và mã định danh tham số tích hợp.

**Hiểu về Parameter**:  
- **Parameter**: Đối tượng chứa thông tin về một tham số của phần tử, có thể lấy hoặc chỉnh sửa qua API, tương tự giao diện người dùng.  
- **Definition**: Thuộc tính của `Parameter`, trả về đối tượng `Definition` mô tả tên và loại tham số (ví dụ: tên “Head Height”, loại “Length”).  
- **Giá trị**: Giá trị của tham số có thể là `int`, `double`, `string`, `ElementId`, hoặc `none`.  
- **Các phương thức lấy Parameter**:  
  - `Parameters`: Lấy tất cả tham số của phần tử.  
  - `GetOrderedParameters`: Lấy danh sách tham số theo thứ tự hiển thị trong giao diện.  
  - `LookupParameter(string)`: Lấy một tham số bằng tên (ví dụ: “Head Height”).  
  - `GetParameters(string)`: Lấy tất cả tham số có cùng tên (có thể trùng do tham số dự án).  
  - **BuiltInParameter**: Sử dụng mã định danh tích hợp (enumeration) để lấy tham số chính xác, tránh nhầm lẫn với tham số dự án trùng tên.  

**Tạo lệnh GetParameters**:  
1. **Tạo lớp lệnh**:  
   - Tạo lớp `GetParameters.cs`, triển khai `IExternalCommand`.  
   - Đặt thuộc tính `[Transaction(TransactionMode.ReadOnly)]` vì chỉ đọc dữ liệu.  
   - Thêm namespace: `Autodesk.Revit.DB`, `Autodesk.Revit.UI`, `Autodesk.Revit.Attributes`.  

2. **Lấy phần tử và tham số**:  
   - Sử dụng `PickObject` để chọn phần tử, lấy `Element` từ `Document`:  
     - `Element ele = doc.GetElement(pickObj);`.  
   - Lấy tham số “Head Height” bằng `LookupParameter`:  
     - `Parameter param = ele.LookupParameter("Head Height");`.  

3. **Lấy thông tin từ Definition**:  
   - Lấy đối tượng `InternalDefinition` từ `param`:  
     - `InternalDefinition paramDef = param.Definition as InternalDefinition;`.  
   - Trích xuất thông tin: tên (`Name`), loại đơn vị (`UnitType`), và mã định danh tích hợp (`BuiltInParameter`).  

4. **Hiển thị thông tin**:  
   - Sử dụng `TaskDialog` để hiển thị thông tin tham số:  
     - `TaskDialog.Show("Parameters", string.Format("Parameter Name: {0}\nParameter Type: {1}\nBuiltIn Parameter: {2}", paramDef.Name, paramDef.UnitType, paramDef.BuiltInParameter));`.  

**Mã nguồn mẫu**:  
```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.ReadOnly)]
    public class GetParameters : IExternalCommand
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
                    Parameter param = ele.LookupParameter("Head Height");
                    if (param != null)
                    {
                        InternalDefinition paramDef = param.Definition as InternalDefinition;
                        TaskDialog.Show("Parameters",
                            string.Format("Parameter Name: {0}\nParameter Type: {1}\nBuiltIn Parameter: {2}",
                            paramDef.Name, paramDef.UnitType, paramDef.BuiltInParameter));
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
  - `<FullClassName>`: `MyRevitCommands.GetParameters`.  
  - `<Name>` và `<Text>`: `GetParameters`.  
  - `<Description>`: “Retrieves the Head Height parameter of a selected window”.  

**Kiểm tra lệnh**:  
1. Nhấn **Start** trong Visual Studio để chạy Debug.  
2. Mở Revit, mở tệp dự án từ thư mục bài tập.  
3. Vào **Add-Ins > External Tools > GetParameters`, chọn một cửa sổ (window).  
4. Kiểm tra `TaskDialog` hiển thị thông tin:  
   - **Parameter Name**: Head Height.  
   - **Parameter Type**: UT_Length (đơn vị chiều dài).  
   - **BuiltIn Parameter**: WINDOW_HEAD_HEIGHT_PARAM (mã định danh tích hợp).  
5. **Lưu ý**: Nếu chọn phần tử không phải cửa sổ (không có tham số “Head Height”), lệnh có thể gây lỗi. Để an toàn, kiểm tra `param != null` trước khi truy cập.

**Mục tiêu**  
- Hiểu cách lấy tham số của phần tử bằng các phương thức như `LookupParameter`.  
- Trích xuất thông tin từ `Definition` (tên, loại đơn vị, BuiltInParameter).  
- Hiển thị thông tin tham số trong `TaskDialog`.  

**Lưu ý an toàn**: Kiểm tra xem tham số có tồn tại (`param != null`) để tránh lỗi khi phần tử không có tham số mong muốn. Trong phần tiếp theo, bạn sẽ học cách chỉnh sửa giá trị tham số để thay đổi đặc tính phần tử.