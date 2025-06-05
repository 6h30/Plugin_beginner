2.3. FilteredElementCollector  

Trong Revit API, để tìm và lọc các phần tử (như tường, cửa, cột) trong dự án Revit, chúng ta sử dụng **FilteredElementCollector**. Đây là một công cụ mạnh mẽ giúp thu thập và lọc phần tử theo các tiêu chí cụ thể.

**Cách tạo FilteredElementCollector**:  
`FilteredElementCollector` có thể được tạo theo ba cách:  
1. **Tìm trong toàn bộ tài liệu (Document)**: Thu thập tất cả phần tử trong dự án Revit.  
2. **Tìm trong tập hợp phần tử cụ thể**: Chỉ tìm trong một tập hợp phần tử được chỉ định.  
3. **Tìm trong một khung nhìn (View)**: Chỉ thu thập phần tử hiển thị trong một khung nhìn cụ thể.  

Sau khi tạo, bạn cần áp dụng ít nhất một bộ lọc (filter) để thu hẹp danh sách phần tử.

**Các loại bộ lọc**:  
1. **Quick Filter** (Bộ lọc nhanh):  
   - Kế thừa từ lớp `ElementQuickFilter`, kiểm tra thông tin cơ bản của phần tử (nhanh nhất).  
   - Ví dụ:  
     - `OfCategory`: Lọc phần tử theo danh mục (như `BuiltInCategory.OST_Walls`).  
     - `OfClass`: Lọc phần tử theo lớp (như `Wall`, `Floor`).  
2. **Slow Filter** (Bộ lọc chậm):  
   - Kế thừa từ lớp `ElementSlowFilter`, kiểm tra chi tiết hơn (chậm hơn).  
   - Nên kết hợp với Quick Filter để tăng hiệu suất, ví dụ: lọc danh mục trước, sau đó lọc theo tầng (level).  
3. **Logical Filter** (Bộ lọc logic):  
   - Kế thừa từ lớp `ElementLogicalFilter`, bao gồm `LogicalAndFilter` (và) và `LogicalOrFilter` (hoặc).  
   - Dùng để kết hợp nhiều bộ lọc Quick và Slow.  

**Cách áp dụng bộ lọc**:  
- **Đơn lẻ**: Sử dụng phương thức `WherePasses` trên `FilteredElementCollector`, truyền bộ lọc làm tham số.  
- **Phím tắt**: Sử dụng các phương thức như `OfCategory` hoặc `OfClass`.  
- **Kết hợp**: Sử dụng các bộ lọc logic hoặc kết hợp phím tắt với `WherePasses`.  

**Lấy phần tử từ Collector**:  
Sau khi áp dụng bộ lọc, bạn có thể lấy phần tử hoặc ID của chúng bằng các phương thức:  
- `ToElements`: Trả về danh sách các đối tượng **Element**.  
- `FirstElement`: Trả về phần tử đầu tiên.  
- `ToElementIds`: Trả về danh sách ID của phần tử.  
- `FirstElementId`: Trả về ID của phần tử đầu tiên.  

**Mã nguồn mẫu**:  
Dưới đây là ví dụ lọc tất cả các bức tường trong dự án:  
```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.ReadOnly)]
    public class FilterWalls : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            try
            {
                // Tạo FilteredElementCollector để tìm trong toàn bộ tài liệu
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                // Áp dụng bộ lọc Quick để tìm các bức tường
                collector.OfCategory(BuiltInCategory.OST_Walls).OfClass(typeof(Wall));
                // Lấy danh sách phần tử
                string info = "Found " + collector.Count() + " walls.";
                TaskDialog.Show("Wall Count", info);
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
- Hiểu cách sử dụng `FilteredElementCollector` để thu thập và lọc phần tử.  
- Nắm rõ các loại bộ lọc: Quick, Slow, và Logical.  
- Biết cách lấy danh sách phần tử hoặc ID bằng các phương thức như `ToElements` và `ToElementIds`.  

Trong các phần tiếp theo, bạn sẽ học cách áp dụng các bộ lọc nâng cao để làm việc với các phần tử cụ thể trong dự án Revit.