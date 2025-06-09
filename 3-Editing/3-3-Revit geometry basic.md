3.3. Hình học cơ bản trong Revit (Revit Geometry Basics)  

Revit API cung cấp nhiều loại hình học (geometry types) như `XYZ`, `Line`, `Arc`, dùng để mô tả hình học của các phần tử trong mô hình 3D. Phần này giới thiệu tổng quan về các loại hình học và cách truy xuất đối tượng `GeometryElement` từ một phần tử để phân tích hình học của nó.

**Tổng quan về hình học trong Revit API**:  
- **GeometryElement**: Đối tượng biểu diễn hình học của một phần tử 3D, hoạt động như một danh sách chứa các loại hình học khác:  
  - **Solid**: Khối đặc, gồm các mặt (`Face`) và cạnh (`Edge`).  
  - **Mesh**: Lưới đa giác, thường mô tả bề mặt phức tạp.  
  - **GeometryInstance**: Đại diện cho tập hợp hình học được lưu trữ, thường dùng cho family instance (ví dụ: cửa, gồm khung, cánh, tay nắm). Có thể chứa các loại hình học khác hoặc lồng ghép nhiều lần.  
  - **Curve**: Đường cong (ví dụ: `Line`, `Arc`).  
  - **Point**: Điểm (`XYZ`).  
  - **Polyline**: Đường gấp khúc.  
- **GeometryObject**: Lớp cơ sở cho tất cả các loại hình học, cho phép ép kiểu (cast) sang loại cụ thể (ví dụ: `Face`, `Mesh`, `GeometryInstance`).  

**Truy xuất GeometryElement**:  
1. **Tra cứu trong Object Browser**:  
   - Tìm lớp `Element` trong namespace `Autodesk.Revit.DB`.  
   - Thuộc tính `Geometry` (phương thức `get_Geometry(Options)`): Truy xuất hình học của phần tử.  
   - Cần cung cấp đối tượng `Options` để tùy chỉnh đầu ra.  

2. **Tạo đối tượng Options**:  
   - Tìm lớp `Options` trong namespace `Autodesk.Revit.DB`.  
   - Constructor: `Options options = new Options();`.  
   - Thuộc tính:  
     - `ComputeReferences`: Đặt `true` để lấy các tham chiếu (references) cần cho một số phương thức API (ví dụ: dimensioning).  
     - `DetailLevel`: Chọn mức chi tiết (`ViewDetailLevel.Coarse`, `Medium`, `Fine`). `Fine` cho chất lượng cao, nhiều mặt hơn.  
     - `IncludeNonVisibleObjects`: Đặt `true` để lấy hình học ẩn.  
     - `View`: Chỉ định khung nhìn để trích xuất hình học (nếu bị cắt).  

3. **Duyệt GeometryElement**:  
   - `GeometryElement` giống như danh sách, cần duyệt qua để lấy `GeometryObject`.  
   - Ép kiểu `GeometryObject` sang loại cụ thể (ví dụ: `Solid`, `GeometryInstance`).  
   - Với `GeometryInstance`, dùng `GetInstanceGeometry()` để lấy `GeometryElement` liên quan và tiếp tục duyệt.  

**Mã nguồn mẫu**:  
```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class GetGeometry : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            try
            {
                // Chọn phần tử
                Reference reference = uiDoc.Selection.PickObject(ObjectType.Element, "Select an element");
                Element element = doc.GetElement(reference);

                // Tạo Options
                Options options = new Options
                {
                    ComputeReferences = true,
                    DetailLevel = ViewDetailLevel.Fine,
                    IncludeNonVisibleObjects = false
                };

                // Lấy GeometryElement
                GeometryElement geomElement = element.get_Geometry(options);

                // Duyệt GeometryElement
                foreach (GeometryObject geomObj in geomElement)
                {
                    if (geomObj is Solid solid)
                    {
                        // Xử lý khối đặc (Solid)
                        TaskDialog.Show("Geometry Info", $"Found Solid with {solid.Faces.Size} faces");
                    }
                    else if (geomObj is GeometryInstance geomInstance)
                    {
                        // Lấy hình học của GeometryInstance
                        GeometryElement instanceGeom = geomInstance.GetInstanceGeometry();
                        TaskDialog.Show("Geometry Info", "Found GeometryInstance");
                    }
                }

                return Result.Succeeded;
            }
            catch (Exception e)
            {
                message = e.Message;
                return Result.Failed;
            }
        }
    }
}
```

**Cập nhật tệp manifest (.addin)**:  
- Thêm lệnh vào tệp `.addin`:  
```xml
<AddIn Type="Command">
  <Name>GetGeometry</Name>
  <Assembly>path\to\MyRevitCommands.dll</Assembly>
  <AddInId>YOUR_GUID_HERE</AddInId>
  <FullClassName>MyRevitCommands.GetGeometry</FullClassName>
  <VendorId>YOUR_VENDOR_ID</VendorId>
</AddIn>
```
- Thay `YOUR_GUID_HERE` bằng GUID mới (**Tools > Create GUID**).  

**Kiểm tra**:  
1. Biên dịch dự án trong Visual Studio.  
2. Mở Revit, mở tệp dự án từ thư mục bài tập.  
3. Vào **Add-Ins > External Tools > GetGeometry**, chọn một phần tử (ví dụ: nội thất hoặc cửa).  
4. Kiểm tra:  
   - `TaskDialog` hiển thị thông tin hình học, ví dụ: “Found Solid with 6 faces” hoặc “Found GeometryInstance”.  
5. **Lưu ý**:  
   - Đảm bảo phần tử có hình học (không phải phần tử hệ thống như lưới).  
   - Sử dụng `options.View` nếu cần trích xuất hình học trong khung nhìn cụ thể.  

**Mục tiêu**:  
- Hiểu các loại hình học trong Revit API (`Solid`, `Mesh`, `GeometryInstance`, v.v.).  
- Truy xuất `GeometryElement` từ phần tử bằng `get_Geometry(Options)`.  
- Duyệt và xử lý các `GeometryObject`, bao gồm `GeometryInstance`.  

**Ứng dụng**:  
- Tính toán vị trí phần tử so với các phần tử khác.  
- Xác định thuộc tính hình học (diện tích, chiều dài).  
- Dùng hình học để định nghĩa phần tử mới hoặc tạo kích thước (dimensioning).  

Trong các phần tiếp theo, bạn có thể học cách phân tích chi tiết hơn về `Solid` (mặt, cạnh) hoặc xử lý các trường hợp hình học phức tạp trong Revit API.