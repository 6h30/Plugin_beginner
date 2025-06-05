2.2. Lấy thông tin phần tử  

Trong phần trước, chúng ta đã lấy được đối tượng **Element** từ ID của phần tử được chọn. Bây giờ, chúng ta sẽ tìm hiểu cách trích xuất thông tin từ phần tử, bao gồm: **Category Name**, **Family Name**, **Family Symbol Name**, và **Instance Name**. Các thông tin này giúp phân loại và làm việc với phần tử trong dự án Revit.

**Tra cứu trong Object Browser**:  
- Mở **Object Browser** trong Visual Studio (nhấp đúp vào `RevitAPI.dll`), tìm lớp **Element**.  
- **Category Name**: Lấy từ thuộc tính `Category.Name` của đối tượng **Element**.  
- **Instance Name**: Lấy từ thuộc tính `Name` của đối tượng **Element**.  
- **Family Name** và **Family Symbol Name**: Cần lấy đối tượng **ElementType** bằng phương thức `GetTypeId` từ **Element**, sau đó truy cập:  
  - `Name` (tên Family Symbol).  
  - `FamilyName` (tên Family).  

**Cập nhật mã lệnh**:  
Tiếp nối từ lệnh `GetElementId` (phần 2.1), chúng ta sẽ mở rộng mã để hiển thị các thông tin trên bằng `TaskDialog`.

1. **Lấy ElementType**:  
   - Sau dòng lấy `Element`, thêm mã để lấy ID của **ElementType**:  
     - `ElementId eTypeId = ele.GetTypeId();`.  
   - Sử dụng `GetElement` để lấy **ElementType**, ép kiểu bằng từ khóa `as` để an toàn (tránh lỗi nếu ép kiểu thất bại):  
     - `ElementType eType = doc.GetElement(eTypeId) as ElementType;`.  

2. **Hiển thị thông tin trong TaskDialog**:  
   - Cập nhật `TaskDialog.Show` để hiển thị các thông tin: **Element ID**, **Category**, **Instance Name**, **Family Symbol Name**, và **Family Name**.  
   - Sử dụng `Environment.NewLine` (từ namespace `System`) để thêm dòng mới giữa các thông tin.  

**Mã nguồn mẫu**:  
```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.ReadOnly)]
    public class GetElementId : IExternalCommand
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
                    ElementId eleId = pickObj.ElementId;
                    Element ele = doc.GetElement(eleId);
                    ElementId eTypeId = ele.GetTypeId();
                    ElementType eType = doc.GetElement(eTypeId) as ElementType;

                    string info = "Element ID: " + eleId.ToString() + Environment.NewLine +
                                  "Category: " + ele.Category.Name + Environment.NewLine +
                                  "Instance Name: " + ele.Name + Environment.NewLine +
                                  "Family Symbol Name: " + (eType != null ? eType.Name : "N/A") + Environment.NewLine +
                                  "Family Name: " + (eType != null ? eType.FamilyName : "N/A");
                    TaskDialog.Show("Element Classification", info);
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

**Kiểm tra lệnh**:  
1. Trong Visual Studio, nhấn **Start** để chạy Debug.  
2. Mở Revit, tạo dự án mới (dùng **Architectural Template**).  
3. Vào **Add-Ins > External Tools > Get Element ID**, chọn một phần tử (ví dụ: tường hoặc section).  
4. Kiểm tra cửa sổ `TaskDialog` hiển thị thông tin:  
   - **Element ID**: ID của phần tử.  
   - **Category**: Danh mục (ví dụ: Walls, Sections).  
   - **Instance Name**: Tên instance (ví dụ: Section 1).  
   - **Family Symbol Name**: Tên loại family (ví dụ: Building Section).  
   - **Family Name**: Tên family (ví dụ: Section).  

**Mục tiêu**  
- Hiểu cách lấy thông tin phần tử (Category, Instance Name, Family Symbol Name, Family Name).  
- Sử dụng `GetTypeId` và ép kiểu để lấy thông tin từ **ElementType**.  
- Hiển thị thông tin phần tử trong `TaskDialog` với định dạng dễ đọc.  

Thông tin phân loại này rất hữu ích khi lọc, tạo, hoặc chỉnh sửa phần tử trong dự án Revit. Trong các phần tiếp theo, chúng ta sẽ tìm hiểu cách lọc phần tử dựa trên các phân loại này.