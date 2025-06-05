2.10. Tạo phần tử dựa trên vòng lặp (Loop-Based Elements)  

Các phần tử trong Revit được tạo bằng vòng lặp khép kín trong chế độ phác thảo (sketch mode), như sàn (floors) hoặc mái (roofs), sử dụng các kiểu dữ liệu chứa tập hợp đường cong như `CurveLoop` và `CurveArray` trong Revit API. Trong phần này, chúng ta sẽ tạo một sàn mới bằng cách sử dụng các đường cong từ bài tập trước (phần 2.9) và dịch chuyển (offset) chúng để sàn khớp với mặt ngoài của tường.

**Tra cứu phương thức tạo sàn**:  
- Mở **Object Browser**, tìm `NewFloor` trong namespace `Autodesk.Revit.Creation.Document` (không phải lớp `Floor`).  
- Sử dụng phương thức `NewFloor(CurveArray, bool)`:  
  - `CurveArray`: Tập hợp các đường cong khép kín định nghĩa biên sàn.  
  - `bool`: Xác định sàn có phải là kết cấu (structural) hay không (dùng `false`).  

**Tra cứu CurveLoop và CurveArray**:  
- **CurveLoop**: Tập hợp các đường cong được sắp xếp tuần tự, khép kín, hỗ trợ các thao tác như dịch chuyển (offset).  
  - Tạo bằng `CurveLoop.Create(List<Curve>)`.  
  - Dịch chuyển bằng `CurveLoop.CreateViaOffset(CurveLoop, double, XYZ)`.  
- **CurveArray**: Tập hợp các đường cong, không yêu cầu tuần tự, dùng để truyền vào `NewFloor`.  

**Tạo lệnh PlaceLoopElement**:  
1. **Tạo lớp lệnh**:  
   - Tạo lớp `PlaceLoopElement.cs`, triển khai `IExternalCommand`.  
   - Đặt thuộc tính `[Transaction(TransactionMode.Manual)]`.  
   - Thêm namespace: `Autodesk.Revit.DB`, `Autodesk.Revit.UI`, `Autodesk.Revit.Attributes`, `System.Collections.Generic`.  

2. **Tái sử dụng Curves từ phần 2.9**:  
   - Sử dụng danh sách `curves` (3 đường thẳng và 1 cung tròn) từ bài trước.  
   - Tạo các điểm `p1` đến `p5` và các đường cong `l1` đến `l4` như trong phần 2.9.  

3. **Tạo CurveLoop**:  
   - Tạo `CurveLoop` từ danh sách `curves`:  
     - `CurveLoop crvLoop = CurveLoop.Create(curves);`.  

4. **Dịch chuyển CurveLoop**:  
   - Tường mặc định là “Basic Wall, Double Brick 270” (độ dày 270 mm), nên dịch chuyển ra ngoài 135 mm (nửa độ dày).  
   - Chuyển 135 mm sang đơn vị feet (Revit API dùng feet):  
     - `double offset = UnitUtils.ConvertToInternalUnits(135, DisplayUnitType.DUT_MILLIMETERS);`.  
   - Tạo `CurveLoop` dịch chuyển:  
     - `CurveLoop offsetCrv = CurveLoop.CreateViaOffset(crvLoop, offset, new XYZ(0, 0, 1));`.  
     - `new XYZ(0, 0, 1)` là vector pháp tuyến theo trục Z (hướng lên).  

5. **Chuyển CurveLoop sang CurveArray**:  
   - Tạo `CurveArray`:  
     - `CurveArray cArray = new CurveArray();`.  
   - Duyệt `offsetCrv` và thêm từng đường cong vào `cArray`:  
     - `foreach (Curve c in offsetCrv) { cArray.Append(c); }`.  

6. **Tạo Floor**:  
   - Trong khối `using (Transaction trans = new Transaction(doc, "Place Floor"))`:  
     - Gọi `trans.Start()`.  
     - Tạo sàn: `doc.Create.NewFloor(cArray, false);`.  
     - Gọi `trans.Commit()`.  

**Mã nguồn mẫu**:  
```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Collections.Generic;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class PlaceLoopElement : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            try
            {
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

                // Tạo CurveLoop
                CurveLoop crvLoop = CurveLoop.Create(curves);

                // Dịch chuyển CurveLoop
                double offset = UnitUtils.ConvertToInternalUnits(135, DisplayUnitType.DUT_MILLIMETERS);
                CurveLoop offsetCrv = CurveLoop.CreateViaOffset(crvLoop, offset, new XYZ(0, 0, 1));

                // Chuyển sang CurveArray
                CurveArray cArray = new CurveArray();
                foreach (Curve c in offsetCrv)
                {
                    cArray.Append(c);
                }

                // Tạo Floor
                using (Transaction trans = new Transaction(doc, "Place Floor"))
                {
                    trans.Start();
                    doc.Create.NewFloor(cArray, false);
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
  - `<FullClassName>`: `MyRevitCommands.PlaceLoopElement`.  
  - `<Name>` và `<Text>`: `PlaceLoopElement`.  
  - `<Description>`: “Creates a floor using an offset curve loop”.  

**Kiểm tra lệnh**:  
1. Nhấn **Start** trong Visual Studio để chạy Debug.  
2. Mở Revit, mở tệp dự án từ thư mục bài tập.  
3. Vào **Add-Ins > External Tools > PlaceLoopElement**, chạy lệnh.  
4. Chuyển sang chế độ 3D để kiểm tra sàn mới được tạo, đảm bảo nó khớp với mặt ngoài của tường (dịch chuyển 135 mm).  

**Mục tiêu**  
- Hiểu cách tạo phần tử dựa trên vòng lặp (loop-based) như sàn bằng `NewFloor`.  
- Sử dụng `CurveLoop` để tạo và dịch chuyển vòng lặp đường cong khép kín.  
- Chuyển đổi `CurveLoop` sang `CurveArray` để tạo phần tử trong Revit.  

**Mở rộng**:  
- Có thể lọc các tường hiện có trong khung nhìn để tự động xác định độ dày và điều chỉnh offset thay vì mã hóa cứng.  
- Sử dụng `Document.GetUnits()` để tự động lấy đơn vị từ dự án.  

Trong các phần tiếp theo, bạn sẽ học cách làm việc với các phần tử phức tạp hơn hoặc tự động hóa các tác vụ liên quan đến hình học trong Revit.