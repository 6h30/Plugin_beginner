2.8. Truy vấn FilteredElementCollector với LINQ  

Để tối ưu hóa việc lọc phần tử bằng `FilteredElementCollector`, chúng ta có thể sử dụng **LINQ Extensions** và **Lambda Expressions** trong C#. Các công cụ này giúp mã ngắn gọn, dễ đọc hơn, đặc biệt khi tìm kiếm phần tử cụ thể từ một tập hợp lớn.

**LINQ và LINQ Extensions**:  
- **LINQ** (Language-Integrated Query): Tính năng của C# cho phép truy vấn dữ liệu từ các nguồn như danh sách hoặc `FilteredElementCollector`. Cú pháp cơ bản: `from x in data where condition select x`.  
- **LINQ Extensions**: Các phương thức mở rộng (extension methods) thêm chức năng truy vấn vào các kiểu `IEnumerable` (như danh sách hoặc `FilteredElementCollector`). Một số phương thức phổ biến:  
  - `Cast<T>`: Ép kiểu tất cả phần tử trong danh sách thành kiểu `T`.  
  - `First`: Lấy phần tử đầu tiên thỏa mãn điều kiện.  
  - `Select`: Chọn phần tử dựa trên hàm xử lý.  
- **Lưu ý**: Nên sử dụng bộ lọc nhanh (`Quick Filters`) hoặc chậm (`Slow Filters`) của Revit API trước khi áp dụng LINQ, vì chúng nhanh hơn.

**Lambda Expressions**:  
- Là hàm ẩn danh (anonymous function), viết ngắn gọn theo cú pháp: `x => điều_kiện`.  
- Ví dụ: `x => x.Name == "SomeName"` kiểm tra xem phần tử `x` có tên là “SomeName” không.  
- Lambda thường được dùng trong các phương thức LINQ như `First` để lọc trực tiếp.

**Cập nhật lệnh PlaceFamily**:  
Tiếp nối từ lệnh `PlaceFamily` (phần 2.7), chúng ta sẽ tối ưu đoạn mã lọc `FamilySymbol` bằng LINQ thay vì vòng lặp `foreach`.

1. **Xóa vòng lặp foreach**:  
   - Trong phần 2.7, chúng ta dùng `foreach` để duyệt danh sách `symbols` và tìm `FamilySymbol` có tên “1,525 by 762 millimeters”. Bây giờ, chúng ta sẽ thay thế bằng LINQ.  

2. **Sử dụng LINQ Extensions**:  
   - Sửa đoạn mã lọc `FilteredElementCollector`:  
     - Thay vì gọi `ToElements()` và duyệt, sử dụng `Cast<FamilySymbol>` để ép kiểu và `First` với Lambda để tìm phần tử đầu tiên thỏa mãn.  
     - Cú pháp: `FamilySymbol symbol = collector.OfClass(typeof(FamilySymbol)).WhereElementIsElementType().Cast<FamilySymbol>().First(x => x.Name == "1,525 by 762 millimeters");`.  
   - Loại bỏ biến `IList<Element> symbols` và vòng lặp `foreach`.  

**Mã nguồn mẫu (đã tối ưu)**:  
```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Linq;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class PlaceFamily : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            try
            {
                // Lọc FamilySymbol bằng LINQ
                FamilySymbol symbol = new FilteredElementCollector(doc)
                    .OfClass(typeof(FamilySymbol))
                    .WhereElementIsElementType()
                    .Cast<FamilySymbol>()
                    .First(x => x.Name == "1,525 by 762 millimeters");

                if (symbol == null)
                {
                    TaskDialog.Show("Error", "Family Symbol not found.");
                    return Result.Failed;
                }

                // Kích hoạt FamilySymbol
                if (!symbol.IsActive)
                {
                    symbol.Activate();
                }

                // Đặt Family Instance
                using (Transaction trans = new Transaction(doc, "Place Family"))
                {
                    trans.Start();
                    doc.Create.NewFamilyInstance(new XYZ(0, 0, 0), symbol, StructuralType.NonStructural);
                    trans.Commit();
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

**Giải thích mã**:  
- `Cast<FamilySymbol>()`: Ép kiểu tất cả phần tử trong collector thành `FamilySymbol`.  
- `First(x => x.Name == "1,525 by 762 millimeters")`: Tìm phần tử đầu tiên có tên khớp, sử dụng Lambda Expression `x => x.Name == ...`.  
- Loại bỏ vòng lặp `foreach`, giúp mã ngắn gọn và hiệu quả hơn.  

**Kiểm tra lệnh**:  
1. Nhấn **Start** trong Visual Studio để chạy Debug.  
2. Mở Revit, mở tệp dự án từ thư mục bài tập.  
3. Vào **Add-Ins > External Tools > PlaceFamily**, chạy lệnh.  
4. Kiểm tra xem bàn (desk) có xuất hiện tại tọa độ (0, 0, 0) không.  

**Mục tiêu**  
- Hiểu cách sử dụng **LINQ Extensions** (`Cast`, `First`) và **Lambda Expressions** để tối ưu `FilteredElementCollector`.  
- Thay thế vòng lặp `foreach` bằng truy vấn LINQ để mã ngắn gọn và dễ đọc.  
- Áp dụng các kỹ thuật này để lọc phần tử hiệu quả hơn trong Revit API.  

LINQ và Lambda Expressions sẽ được sử dụng nhiều trong các phần tiếp theo để xử lý dữ liệu phức tạp hơn. Trong phần sau, bạn sẽ học cách mở rộng các lệnh để làm việc với nhiều loại phần tử hoặc tạo các Family Instance nâng cao.