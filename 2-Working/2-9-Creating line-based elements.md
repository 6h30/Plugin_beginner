2.9. Tạo phần tử dựa trên đường (Line-Based Elements)  

Trong phần trước, chúng ta đã tạo **Family Instance** dựa trên điểm (point-based). Trong phần này, chúng ta sẽ tạo các phần tử dựa trên đường (line-based) như tường (walls) bằng cách sử dụng các đối tượng **Curve** (đường thẳng hoặc cung tròn) trong Revit API.

**Tra cứu phương thức tạo tường**:  
- Mở **Object Browser** trong Visual Studio, tìm lớp `Wall` trong namespace `Autodesk.Revit.DB`.  
- Sử dụng phương thức `Wall.Create(Document, Curve, ElementId, bool)` với các tham số:  
  - `Document`: Tài liệu Revit.  
  - `Curve`: Đường cong (line hoặc arc) làm đường tâm của tường.  
  - `ElementId`: ID của tầng (Level) làm tham chiếu.  
  - `bool`: Xác định tường có phải là kết cấu (structural) hay không (ở đây dùng `false`).  

**Tra cứu lớp Curve**:  
- Tìm lớp `Curve` trong `Autodesk.Revit.DB`.  
- Mở rộng **Derived Types** để xem các loại đường cong:  
  - `Line`: Đường thẳng, tạo bằng `Line.CreateBound(XYZ start, XYZ end)`.  
  - `Arc`: Cung tròn, tạo bằng `Arc.Create(XYZ start, XYZ end, XYZ midPoint)`.  

**Tạo lệnh PlaceLineElement**:  
1. **Tạo lớp lệnh**:  
   - Tạo lớp `PlaceLineElement.cs`, triển khai `IExternalCommand`.  
   - Đặt thuộc tính `[Transaction(TransactionMode.Manual)]` để cho phép thay đổi mô hình.  
   - Thêm namespace: `Autodesk.Revit.DB`, `Autodesk.Revit.UI`, `Autodesk.Revit.Attributes`, `System.Collections.Generic`, `System.Linq`.  

2. **Lấy Level (tầng)**:  
   - Sử dụng `FilteredElementCollector` để tìm tầng có tên “Ground Floor”:  
     - `Level level = new FilteredElementCollector(doc).OfClass(typeof(Level)).Cast<Level>().First(x => x.Name == "Ground Floor");`.  
   - **Lưu ý**: Nếu không tìm thấy tầng, lệnh sẽ gây lỗi. Để an toàn, có thể bọc trong `try-catch`.  

3. **Tạo các điểm XYZ**:  
   - Tạo 5 điểm để xác định 4 đường cong (3 đường thẳng và 1 cung tròn) theo thứ tự kim đồng hồ:  
     - `p1`: (-10, -10, 0) – góc dưới trái.  
     - `p2`: (10, -10, 0) – góc dưới phải.  
     - `p3`: (15, 0, 0) – điểm giữa của cung.  
     - `p4`: (10, 10, 0) – góc trên phải.  
     - `p5`: (-10, 10, 0) – góc trên trái.  

4. **Tạo Curves**:  
   - Tạo danh sách `List<Curve> curves` để lưu các đường cong.  
   - Tạo 4 đường cong:  
     - `l1`: Đường thẳng từ `p1` đến `p2` (`Line.CreateBound(p1, p2)`).  
     - `l2`: Cung tròn từ `p2` đến `p4` qua `p3` (`Arc.Create(p2, p4, p3)`).  
     - `l3`: Đường thẳng từ `p4` đến `p5` (`Line.CreateBound(p4, p5)`).  
     - `l4`: Đường thẳng từ `p5` đến `p1` (`Line.CreateBound(p5, p1)`).  
   - Thêm các đường cong vào danh sách: `curves.Add(l1);`, v.v.  

5. **Tạo Walls**:  
   - Trong khối `using (Transaction trans = new Transaction(doc, "Place Walls"))`:  
     - Gọi `trans.Start()`.  
     - Duyệt danh sách `curves` bằng `foreach` và tạo tường cho mỗi đường cong:  
       - `Wall.Create(doc, c, level.Id, false);`.  
     - Gọi `trans.Commit()` để lưu thay đổi.  

**Mã nguồn mẫu**:  
```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class PlaceLineElement : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            try
            {
                // Lấy Level
                Level level = new FilteredElementCollector(doc)
                    .OfClass(typeof(Level))
                    .Cast<Level>()
                    .First(x => x.Name == "Ground Floor");

                // Tạo các điểm XYZ
                XYZ p1 = new XYZ(-10, -10, 0);
                XYZ p2 = new XYZ(10, -10, 0);
                XYZ p3 = new XYZ(15, 0, 0);
                XYZ p4 = new XYZ(10, 10, 0);
                XYZ p5 = new XYZ(-10, 10, 0);

                // Tạo Curves
                List<Curve> curves = new List<Curve>();
                Curve l1 = Line.CreateBound(p1, p2);
                Curve l2 = Arc.Create(p2, p4, p3);
                Curve l3 = Line.CreateBound(p4, p5);
                Curve l4 = Line.CreateBound(p5, p1);
                curves.Add(l1);
                curves.Add(l2);
                curves.Add(l3);
                curves.Add(l4);

                // Tạo Walls
                using (Transaction trans = new Transaction(doc, "Place Walls"))
                {
                    trans.Start();
                    foreach (Curve c in curves)
                    {
                        Wall.Create(doc, c, level.Id, false);
                    }
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

**Cập nhật tệp manifest (.addin)**:  
- Mở `MyRevitCommands.addin`, sao chép khối `<AddIn>` từ lệnh trước.  
- Cập nhật:  
  - `<AddInId>`: Tạo GUID mới (**Tools > Create GUID**).  
  - `<FullClassName>`: `MyRevitCommands.PlaceLineElement`.  
  - `<Name>` và `<Text>`: `PlaceLineElement`.  
  - `<Description>`: “Creates walls using lines and an arc”.  

**Kiểm tra lệnh**:  
1. Nhấn **Start** trong Visual Studio để chạy Debug.  
2. Mở Revit, mở tệp dự án từ thư mục bài tập.  
3. Vào **Add-Ins > External Tools > PlaceLineElement**, chạy lệnh.  
4. Kiểm tra xem các tường (3 đường thẳng và 1 cung tròn) có được tạo đúng tại các tọa độ đã định không.  

**Mục tiêu**  
- Hiểu cách tạo phần tử dựa trên đường (line-based) như tường bằng `Wall.Create`.  
- Sử dụng các đối tượng `Curve` (`Line` và `Arc`) để định nghĩa hình học.  
- Tích hợp `FilteredElementCollector` và `Transaction` để tạo phần tử trong mô hình Revit.  

**Lưu ý**: Các đường cong (`Curve`) được tạo ngoài giao dịch (`Transaction`) vì chúng không cần lưu vào mô hình, chỉ dùng để định nghĩa hình học. Trong các phần tiếp theo, bạn sẽ học cách tạo các phần tử phức tạp hơn hoặc tùy chỉnh hình học.