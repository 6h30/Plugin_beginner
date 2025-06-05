2.4. Tạo FilteredElementCollector  

Sau khi hiểu về `FilteredElementCollector`, chúng ta sẽ tạo một lệnh để lọc tất cả các phần tử thuộc danh mục **Windows** trong dự án Revit. Lệnh này sẽ sử dụng bộ lọc nhanh (`ElementQuickFilter`) để tìm các cửa sổ (Windows) trong tài liệu Revit.

**Các bước thực hiện**:  
1. **Tạo lớp lệnh mới**:  
   - Trong Visual Studio, tạo một lớp mới, ví dụ: `CollectWindows.cs`, triển khai `IExternalCommand`.  
   - Thêm các namespace cần thiết: `Autodesk.Revit.DB`, `Autodesk.Revit.UI`, và `System.Collections.Generic` (cho `IList`).  

2. **Tạo FilteredElementCollector**:  
   - Trong phương thức `Execute`, tạo `FilteredElementCollector` để tìm trong toàn bộ tài liệu:  
     - `FilteredElementCollector collector = new FilteredElementCollector(doc);`.  

3. **Áp dụng bộ lọc nhanh (Quick Filter)**:  
   - Tạo bộ lọc danh mục bằng `ElementCategoryFilter` với `BuiltInCategory.OST_Windows`:  
     - `ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Windows);`.  
   - Áp dụng bộ lọc vào collector bằng `WherePasses`:  
     - `collector.WherePasses(filter);`.  
   - Thêm bộ lọc phụ để loại bỏ các **ElementType** (chỉ lấy instance):  
     - `collector.WhereElementIsNotElementType();`.  

4. **Lấy danh sách phần tử**:  
   - Sử dụng phương thức `ToElements` để lấy danh sách các phần tử:  
     - `IList<Element> windows = collector.ToElements();`.  
   - Hiển thị số lượng cửa sổ bằng `TaskDialog`:  
     - `TaskDialog.Show("Windows", string.Format("Windows counted: {0}", windows.Count));`.  

5. **Cập nhật tệp manifest (.addin)**:  
   - Mở tệp `MyRevitCommands.addin`, sao chép khối `<AddIn>` từ lệnh trước (ví dụ: `GetElementId`).  
   - Cập nhật các thông tin:  
     - `<AddInId>`: Tạo GUID mới (vào **Tools > Create GUID**, sao chép và dán).  
     - `<FullClassName>`: Thay bằng `MyRevitCommands.CollectWindows`.  
     - `<Name>` và `<Text>`: Đặt thành `CollectWindows`.  
     - `<Description>`: Cập nhật thành “Counts all windows in the project”.  

**Mã nguồn mẫu**:  
```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Collections.Generic;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.ReadOnly)]
    public class CollectWindows : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            try
            {
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Walls);
                collector.WherePasses(filter).WhereElementIsNotElementType();
                IList<Element> windows = collector.ToElements();
                TaskDialog.Show("Windows", string.Format("Windows counted: {0}", windows.Count));
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

**Tra cứu trong Object Browser**:  
- Mở **Object Browser** (nhấp đúp `RevitAPI.dll`), tìm `ElementQuickFilter`.  
- Để xem các lớp kế thừa (như `ElementCategoryFilter`), vào **Browse > Custom Component Set**, thêm `RevitAPI.dll` và `RevitAPIUI.dll`, sau đó tìm lại `ElementQuickFilter` để xem các bộ lọc nhanh.  

**Kiểm tra lệnh**:  
1. Nhấn **Start** trong Visual Studio để chạy Debug.  
2. Mở Revit, tạo hoặc mở dự án (ví dụ: dùng **Architectural Template**).  
3. Vào **Add-Ins > External Tools > CollectWindows**, chạy lệnh.  
4. Kiểm tra `TaskDialog` hiển thị số lượng cửa sổ (Windows) trong dự án.  

**Mục tiêu**  
- Tạo và sử dụng `FilteredElementCollector` để lọc phần tử theo danh mục.  
- Áp dụng bộ lọc nhanh (`ElementCategoryFilter`) và phương thức `WhereElementIsNotElementType`.  
- Hiển thị số lượng phần tử được tìm thấy bằng `TaskDialog`.  

`FilteredElementCollector` là công cụ mạnh mẽ để làm việc với Revit API. Trong các phần tiếp theo, bạn sẽ học cách sử dụng các bộ lọc nâng cao hơn để lọc và xử lý phần tử.