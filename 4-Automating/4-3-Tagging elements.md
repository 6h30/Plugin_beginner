**4.3. Tagging Elements (Gắn thẻ phần tử)**  

Sau khi học cách tạo khung nhìn và áp dụng bộ lọc, chúng ta sẽ tìm hiểu cách chú thích (annotate) phần tử bằng cách gắn thẻ (tagging) sử dụng Revit API. Lớp `IndependentTag` được sử dụng để tạo thẻ cho các phần tử như cửa sổ, cửa đi, hoặc tường. Trong phần này, chúng ta sẽ tạo một lệnh để tự động gắn thẻ cho tất cả cửa sổ (windows) và cửa đi (doors) trong khung nhìn hiện tại.

**Tra cứu IndependentTag**:  
- Trong **Object Browser**, tìm lớp `IndependentTag` trong namespace `Autodesk.Revit.DB`.  
- Phương thức tĩnh: `IndependentTag.Create(Document, ElementId viewId, Reference reference, bool addLeader, TagMode tagMode, TagOrientation orientation, XYZ point)`:  
  - `Document`: Tài liệu Revit.  
  - `viewId`: ID của khung nhìn (2D hoặc 3D khóa).  
  - `reference`: Tham chiếu đến phần tử cần gắn thẻ.  
  - `addLeader`: Có hiển thị đường dẫn (leader) không (`true`/`false`).  
  - `tagMode`: Chế độ gắn thẻ (ví dụ: `TM_ADDBY_CATEGORY` để gắn theo danh mục).  
  - `orientation`: Hướng thẻ (`Horizontal` hoặc `Vertical`).  
  - `point`: Vị trí đặt thẻ (tọa độ `XYZ`).  

**Tạo lệnh TagView**:  
1. **Tạo lớp lệnh**:  
   - Tạo lớp `TagView.cs`, triển khai `IExternalCommand`.  
   - Đặt thuộc tính `[Transaction(TransactionMode.Manual)]` để cho phép thay đổi mô hình.  
   - Thêm namespace: `Autodesk.Revit.DB`, `Autodesk.Revit.UI`, `Autodesk.Revit.Attributes`, `System.Collections.Generic`.  

2. **Định nghĩa TagMode và TagOrientation**:  
   - Đặt chế độ gắn thẻ: `TagMode tmode = TagMode.TM_ADDBY_CATEGORY;`.  
   - Đặt hướng thẻ: `TagOrientation tOrient = TagOrientation.Horizontal;`.  

3. **Lấy danh sách cửa sổ và cửa đi**:  
   - Tạo danh sách danh mục:  
     - `IList<BuiltInCategory> cats = new List<BuiltInCategory> { BuiltInCategory.OST_Windows, BuiltInCategory.OST_Doors };`.  
   - Tạo bộ lọc đa danh mục:  
     - `ElementMulticategoryFilter filter = new ElementMulticategoryFilter(cats);`.  
   - Sử dụng `FilteredElementCollector` để lấy phần tử trong khung nhìn hiện tại:  
     - `IList<Element> tElements = new FilteredElementCollector(doc, doc.ActiveView.Id).WherePasses(filter).WhereElementIsNotElementType().ToElements();`.  

4. **Gắn thẻ cho từng phần tử**:  
   - Trong khối `using (Transaction trans = new Transaction(doc, "Tag Elements"))`:  
     - Gọi `trans.Start()`.  
     - Duyệt qua danh sách `tElements`:  
       - Lấy tham chiếu: `Reference refe = new Reference(ele);`.  
       - Lấy vị trí: `LocationPoint loc = ele.Location as LocationPoint; XYZ point = loc.Point;`.  
       - Tạo thẻ: `IndependentTag.Create(doc, doc.ActiveView.Id, refe, true, tmode, tOrient, point);`.  
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
    public class TagView : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            try
            {
                // Định nghĩa TagMode và TagOrientation
                TagMode tmode = TagMode.TM_ADDBY_CATEGORY;
                TagOrientation tOrient = TagOrientation.Horizontal;

                // Tạo danh sách danh mục
                IList<BuiltInCategory> cats = new List<BuiltInCategory>
                {
                    BuiltInCategory.OST_Windows,
                    BuiltInCategory.OST_Doors
                };

                // Tạo bộ lọc và lấy phần tử
                ElementMulticategoryFilter filter = new ElementMulticategoryFilter(cats);
                IList<Element> tElements = new FilteredElementCollector(doc, doc.ActiveView.Id)
                    .WherePasses(filter)
                    .WhereElementIsNotElementType()
                    .ToElements();

                // Gắn thẻ cho từng phần tử
                using (Transaction trans = new Transaction(doc, "Tag Elements"))
                {
                    trans.Start();
                    foreach (Element ele in tElements)
                    {
                        Reference refe = new Reference(ele);
                        LocationPoint loc = ele.Location as LocationPoint;
                        if (loc != null)
                        {
                            XYZ point = loc.Point;
                            IndependentTag.Create(doc, doc.ActiveView.Id, refe, true, tmode, tOrient, point);
                        }
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
  - `<FullClassName>`: `MyRevitCommands.TagView`.  
  - `<Name>` và `<Text>`: `TagView`.  
  - `<Description>`: “Tags all windows and doors in the active view”.  

**Kiểm tra lệnh**:  
1. Nhấn **Start** trong Visual Studio để chạy Debug.  
2. Mở Revit, mở tệp dự án từ thư mục bài tập (có cửa sổ và cửa đi trong khung nhìn hiện tại).  
3. Vào **Add-Ins > External Tools > TagView`, chạy lệnh.  
4. Kiểm tra:  
   - Tất cả cửa sổ và cửa đi trong khung nhìn hiện tại được gắn thẻ với đường dẫn (leader).  
   - Thẻ được đặt ở vị trí của phần tử (dựa trên `LocationPoint`) và hiển thị ngang (`Horizontal`).  
5. **Lưu ý**:  
   - Nếu phần tử không có `LocationPoint` (ví dụ: tường), thẻ sẽ không được tạo cho phần tử đó. Đã kiểm tra `loc != null` để tránh lỗi.  
   - Lệnh sử dụng loại thẻ mặc định của Revit. Để chỉ định loại thẻ cụ thể, sử dụng constructor khác của `IndependentTag.Create` với tham số `tagTypeId`.  

**Mục tiêu**  
- Hiểu cách sử dụng `IndependentTag` để gắn thẻ phần tử trong khung nhìn.  
- Lọc cửa sổ và cửa đi bằng `ElementMulticategoryFilter` và `FilteredElementCollector`.  
- Tạo thẻ với các thông số như đường dẫn, chế độ gắn thẻ, và hướng thẻ.  

**Mở rộng**:  
- Áp dụng lệnh cho nhiều khung nhìn thay vì chỉ khung nhìn hiện tại.  
- Cho phép người dùng chọn danh mục để gắn thẻ (thay vì cố định cửa sổ và cửa đi).  
- Chỉ định loại thẻ cụ thể bằng cách lọc `FamilySymbol` của thẻ.  

Trong các phần tiếp theo, bạn sẽ học cách tạo tờ (sheets) để hoàn thiện tài liệu thiết kế hoặc tự động hóa các tác vụ chú thích khác trong Revit.