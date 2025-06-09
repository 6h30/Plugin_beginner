**4.5. Placing Views (Đặt View lên Sheet)**  

Sau khi tạo khung nhìn, chú thích và tạo tờ, bước cuối cùng là đặt khung nhìn lên tờ bằng cách sử dụng lớp `Viewport`. Không giống các lớp khung nhìn khác, `Viewport` kế thừa từ `Element` (không phải `View`) và được dùng để định vị khung nhìn trên tờ. Trong phần này, chúng ta sẽ tạo một lệnh để đặt một `ViewPlan` (tạo từ trước) vào trung tâm của một `ViewSheet` (tờ).

**Tra cứu Viewport**:  
- Trong **Object Browser**, tìm lớp `Viewport` trong namespace `Autodesk.Revit.DB`.  
- Phương thức tĩnh: `Viewport.Create(Document, ElementId sheetId, ElementId viewId, XYZ point)`:  
  - `Document`: Tài liệu Revit.  
  - `sheetId`: ID của tờ (`ViewSheet`).  
  - `viewId`: ID của khung nhìn cần đặt (ví dụ: `ViewPlan`).  
  - `point`: Tọa độ `XYZ` để đặt `Viewport`.  
- Phương thức: `Viewport.CanAddViewToSheet(Document, ElementId sheetId, ElementId viewId)` kiểm tra xem khung nhìn có thể đặt lên tờ không (ví dụ: schedules không thể đặt).  
- **BoundingBoxUV**: Lớp dùng để lấy vùng bao 2D của tờ (`Outline`), với tọa độ `U` và `V` (tương tự `X`, `Y` trên mặt phẳng 2D).  

**Tạo lệnh PlaceView**:  
1. **Tạo lớp lệnh**:  
   - Tạo lớp `PlaceView.cs`, triển khai `IExternalCommand`.  
   - Đặt thuộc tính `[Transaction(TransactionMode.Manual)]` để cho phép thay đổi mô hình.  
   - Thêm namespace: `Autodesk.Revit.DB`, `Autodesk.Revit.UI`, `Autodesk.Revit.Attributes`, `System.Linq`.  

2. **Lấy Sheet và View**:  
   - Lấy tờ có số hiệu “J101” (tạo từ phần 4.4):  
     - `ViewSheet vSheet = new FilteredElementCollector(doc).OfClass(typeof(ViewSheet)).Cast<ViewSheet>().First(xu => xu.SheetNumber == "J101");`.  
   - Lấy khung nhìn có tên “Our first plan!” (tạo từ phần 4.1):  
     - `ViewPlan vPlan = new FilteredElementCollector(doc).OfClass(typeof(ViewPlan)).Cast<ViewPlan>().First(xu => xu.Name == "Our first plan!");`.  

3. **Tính trung tâm của Sheet**:  
   - Lấy vùng bao của tờ: `BoundingBoxUV outline = vSheet.Outline;`.  
   - Tính tọa độ `U` trung tâm: `double xu = (outline.Max.U + outline.Min.U) / 2;`.  
   - Tính tọa độ `V` trung tâm: `double yu = (outline.Max.V + outline.Min.V) / 2;`.  
   - Tạo điểm trung tâm: `XYZ midPoint = new XYZ(xu, yu, 0);`.  

4. **Tạo Viewport**:  
   - Trong khối `using (Transaction trans = new Transaction(doc, "Place View"))`:  
     - Gọi `trans.Start()`.  
     - Kiểm tra xem khung nhìn có thể đặt lên tờ không: `Viewport.CanAddViewToSheet(doc, vSheet.Id, vPlan.Id)`.  
     - Tạo `Viewport`: `Viewport viewport = Viewport.Create(doc, vSheet.Id, vPlan.Id, midPoint);`.  
     - Gọi `trans.Commit()`.  

**Mã nguồn mẫu**:  
```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Linq;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class PlaceView : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            try
            {
                // Lấy Sheet và ViewPlan
                ViewSheet vSheet = new FilteredElementCollector(doc)
                    .OfClass(typeof(ViewSheet))
                    .Cast<ViewSheet>()
                    .First(xu => xu.SheetNumber == "J101");

                ViewPlan vPlan = new FilteredElementCollector(doc)
                    .OfClass(typeof(ViewPlan))
                    .Cast<ViewPlan>()
                    .First(xu => xu.Name == "Our first plan!");

                // Tính trung tâm của Sheet
                BoundingBoxUV outline = vSheet.Outline;
                double xu = (outline.Max.U + outline.Min.U) / 2;
                double yu = (outline.Max.V + outline.Min.V) / 2;
                XYZ midPoint = new XYZ(xu, yu, 0);

                // Tạo Viewport
                using (Transaction trans = new Transaction(doc, "Place View"))
                {
                    trans.Start();
                    if (Viewport.CanAddViewToSheet(doc, vSheet.Id, vPlan.Id))
                    {
                        Viewport viewport = Viewport.Create(doc, vSheet.Id, vPlan.Id, midPoint);
                    }
                    else
                    {
                        TaskDialog.Show("Error", "View cannot be placed on the sheet.");
                        return Result.Failed;
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

**Sửa lỗi trong video**:  
- Video gặp lỗi do biến `x` trùng trong lambda expression. Đã sửa bằng cách sử dụng `xu` và `yu` để tránh xung đột.  

**Cập nhật tệp manifest (.addin)**:  
- Mở `MyRevitCommands.addin`, sao chép khối `<AddIn>` từ lệnh trước.  
- Cập nhật:  
  - `<AddInId>`: Tạo GUID mới (**Tools > Create GUID**).  
  - `<FullClassName>`: `MyRevitCommands.PlaceView`.  
  - `<Name>` và `<Text>`: `PlaceView`.  
  - `<Description>`: “Places a view at the center of a sheet”.  

**Kiểm tra lệnh**:  
1. Nhấn **Start** trong Visual Studio để chạy Debug.  
2. Mở Revit, mở tệp dự án từ thư mục bài tập (đảm bảo có tờ “J101” và khung nhìn “Our first plan!”).  
3. Vào **Add-Ins > External Tools > PlaceView`, chạy lệnh.  
4. Kiểm tra tờ “J101” trong **Project Browser**:  
   - Khung nhìn “Our first plan!” được đặt ở trung tâm tờ.  
5. **Lưu ý**:  
   - Nếu khung nhìn không hợp lệ (ví dụ: schedule), lệnh sẽ hiển thị lỗi “View cannot be placed on the sheet.”  
   - Đã kiểm tra `Viewport.CanAddViewToSheet` để đảm bảo an toàn.  

**Mục tiêu**  
- Hiểu cách sử dụng `Viewport` để đặt khung nhìn lên tờ.  
- Tính toán trung tâm tờ bằng `BoundingBoxUV` và tọa độ `U`, `V`.  
- Kiểm tra tính hợp lệ của khung nhìn trước khi đặt bằng `CanAddViewToSheet`.  

**Ứng dụng**: Tự động hóa việc bố trí khung nhìn trên tờ, đặc biệt hữu ích khi xử lý nhiều tờ trong dự án lớn. Trong các phần tiếp theo, bạn có thể học cách điều chỉnh thuộc tính `Viewport` hoặc tự động hóa quy trình tạo tài liệu thiết kế hoàn chỉnh.