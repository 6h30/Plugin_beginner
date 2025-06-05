2.1. Phân loại phần tử 
 
Trong Revit, mọi đối tượng như tường, cửa, sàn, hoặc nội thất đều là **phần tử** (elements), được kế thừa từ lớp cơ sở **Element** trong Revit API. Một dự án Revit là tập hợp các phần tử, và để tìm kiếm hoặc làm việc với chúng, chúng ta cần hiểu cách phân loại phần tử. Có bốn cách chính để phân loại phần tử: **Category**, **Family**, **Symbol** (Family Type), và **Instance**.

- **Category**: Mỗi phần tử thuộc một danh mục (category), xác định loại phần tử. Ví dụ, tất cả tường thuộc danh mục **Walls**, cửa thuộc danh mục **Doors**. Mỗi danh mục mặc định trong Revit (BuiltInCategory) có một giá trị liệt kê tương ứng (ví dụ: `BuiltInCategory.OST_Walls`).  
- **Family**: Các phần tử được nhóm thành các **family** trong một danh mục, dựa trên công dụng, tham số, và hình dạng. Có hai loại family:  
  - **System Families**: Được tích hợp sẵn, như tường, sàn.  
  - **Component Families**: Được tạo bên ngoài và nhập vào dự án, như nội thất.  
- **Symbol (Family Type)**: Mỗi family có các **family symbol** (hay family type), là các biến thể với cài đặt riêng. Ví dụ, family “Basic Wall” có thể có nhiều loại như “Brick Wall” hoặc “Concrete Wall”.  
- **Instance**: Một **family instance** là đối tượng cụ thể được đặt trong mô hình Revit, thuộc một family type cụ thể.  

Hiểu cách phân loại giúp chúng ta lọc và truy cập các phần tử trong dự án khi sử dụng Revit API.

**Lấy phần tử bằng GetElement**:  
Tiếp nối từ lệnh `GetElementId` (phần 1.8), chúng ta sẽ mở rộng lệnh để lấy đối tượng **Element** từ ID của phần tử đã chọn.  

1. **Mở Visual Studio**: Mở tệp `GetElementId.cs`.  
2. **Lấy Document**: Thêm dòng lấy `Document` từ `UIDocument` để truy cập dữ liệu dự án:  
   - `Document doc = uiDoc.Document;`.  
3. **Lấy Element từ ID**: Sau khi chọn phần tử bằng `PickObject`, lấy ID và sử dụng phương thức `GetElement` để lấy đối tượng **Element**:  
   - `ElementId eleId = pickObj.ElementId;`.  
   - `Element ele = doc.GetElement(eleId);`.  
4. **Hiển thị thông tin**: Hiện tại, lệnh chỉ hiển thị ID. Ở các bước sau, chúng ta sẽ lấy thêm thông tin từ `ele` (như tên, loại, hoặc tham số).  

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
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            try
            {
                Reference pickObj = uiDoc.Selection.PickObject(ObjectType.Element);
                if (pickObj != null)
                {
                    ElementId eleId = pickObj.ElementId;
                    Element ele = doc.GetElement(eleId);
                    TaskDialog.Show("Element ID", eleId.ToString());
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

**Tra cứu GetElement trong Object Browser**:  
- Trong Visual Studio, mở **Object Browser** (nhấp đúp vào `RevitAPI.dll`).  
- Tìm `GetElement` (thuộc lớp `Document`). Có ba phiên bản của `GetElement`, nhận tham số là `ElementId`, `Reference`, hoặc `string` (mã định danh duy nhất). Ở đây, chúng ta dùng phiên bản nhận `ElementId`.  

**Mục tiêu**  
- Hiểu cách phân loại phần tử trong Revit (Category, Family, Symbol, Instance).  
- Sử dụng phương thức `GetElement` để lấy đối tượng **Element** từ ID.  
- Chuẩn bị nền tảng để lấy thông tin chi tiết từ phần tử trong các bước tiếp theo.  

Trong phần sau, bạn sẽ học cách trích xuất thông tin chi tiết từ đối tượng `Element`, như danh mục, loại family, hoặc các tham số.