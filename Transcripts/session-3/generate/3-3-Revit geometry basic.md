Kịch bản bài học: Cơ bản về hình học trong Revit
Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ khám phá các loại hình học trong Revit API, như Solid, Mesh, GeometryInstance, và cách truy xuất chúng từ một phần tử bằng GeometryElement. Sau bài học này, bạn sẽ biết:

Các loại hình học chính trong Revit API và cấu trúc của chúng.
Cách lấy GeometryElement từ một phần tử sử dụng Get_Geometry và Options.
Ứng dụng của hình học trong việc tính toán vị trí, diện tích, hoặc định nghĩa phần tử khác.

Hãy cùng bắt đầu!

Phần 1: Tìm hiểu các loại hình học trong Revit API
Hướng dẫn viên (giọng điệu hào hứng):Trong các bài trước, chúng ta đã sử dụng một số loại hình học như XYZ, Line, và Arc. Hôm nay, chúng ta sẽ đi sâu vào các loại hình học khác trong Revit API và cách chúng định nghĩa mô hình 3D của phần tử. Hãy khám phá nào!

Bước 1: Hiểu về GeometryElementMô hình hình học của một phần tử 3D trong Revit được biểu diễn bởi GeometryElement, một đối tượng chứa tập hợp các loại hình học khác nhau, như:

Solid: Khối rắn, gồm các Face (mặt) và Edge (cạnh).
Mesh: Lưới đa giác, thường dùng cho hình học phức tạp.
GeometryInstance: Đại diện cho tập hợp hình học lặp lại, như FamilyInstance (cửa, nội thất).
Curve: Đường cong, như Line, Arc.
Point: Điểm đơn lẻ.
Polyline: Chuỗi đường nối các điểm.

GeometryInstance có thể chứa các GeometryElement khác hoặc lồng nhau nhiều lần, ví dụ: một cửa có khung, tấm cửa, và tay nắm.
Hành động trên màn hình:  

Hiển thị sơ đồ:  GeometryElement
├── Solid: Face, Edge
├── Mesh
├── GeometryInstance: Solid, Mesh, Curve...
├── Curve: Line, Arc
├── Point
└── Polyline


Chèn text: “GeometryElement: Mô hình hình học của phần tử.”


Bước 2: Truy xuất GeometryElementĐể lấy hình học từ một phần tử:

Sử dụng thuộc tính Geometry của lớp Element, nhưng phải gọi là Get_Geometry vì đây là một indexed property yêu cầu tham số Options.
Lặp qua GeometryElement như một danh sách, ép kiểu các đối tượng thành Solid, Mesh, hoặc GeometryInstance.
Với GeometryInstance, gọi GetInstanceGeometry để lấy GeometryElement bên trong và tiếp tục lặp.

Hành động trên màn hình:  

Hiển thị mã giả lập:  GeometryElement geom = element.Get_Geometry(options);
foreach (GeometryObject obj in geom)
{
    if (obj is Solid solid) { /* Xử lý Solid */ }
    else if (obj is GeometryInstance instance)
    {
        GeometryElement instGeom = instance.GetInstanceGeometry();
        /* Lặp tiếp */
    }
}


Chèn text: “Get_Geometry: Truy xuất hình học từ Element.”


Bước 3: Tùy chỉnh với OptionsThuộc tính Get_Geometry yêu cầu một đối tượng Options để tùy chỉnh đầu ra:

ComputeReferences: Đặt true để lấy các tham chiếu (references) cần cho các phương thức như đo kích thước.
DetailLevel: Chọn mức chi tiết (Coarse, Medium, Fine). Fine cho chất lượng cao hơn (nhiều mặt hơn).
IncludeNonVisibleObjects: Đặt true để lấy hình học ẩn.
View: Xác định view để trích xuất hình học (có thể bị cắt trong view cụ thể).

Hành động trên màn hình:  

Hiển thị sơ đồ:  Options
├── ComputeReferences: true/false
├── DetailLevel: Coarse, Medium, Fine
├── IncludeNonVisibleObjects: true/false
└── View: View3D, Plan...


Chèn text: “Options: Tùy chỉnh đầu ra hình học.”




Phần 2: Khám phá GeometryElement trong Visual Studio
Hướng dẫn viên (giọng điệu rõ ràng):Hãy mở Visual Studio để xem cách truy xuất GeometryElement từ một phần tử trong Object Browser và hiểu cách sử dụng Options.

Bước 4: Mở Object BrowserTrong Visual Studio, mở Object Browser (đảm bảo RevitAPI.dll được tham chiếu). Tìm “Element” để xem thuộc tính Geometry:

Chọn phương thức Get_Geometry(Options) trong danh sách phương thức của lớp Element.
Highlight tham số Options, cho thấy nó yêu cầu một đối tượng Options.

Hành động trên màn hình:  

Hiển thị Solution Explorer, nhấp đúp vào RevitAPI để mở Object Browser.  
Nhập “Element”, highlight Get_Geometry(Options).  
Chèn text: “Get_Geometry: Truy xuất GeometryElement.”


Bước 5: Khám phá lớp OptionsTrong Object Browser, tìm “Options” và xem các thuộc tính:

ComputeReferences: Boolean để lấy tham chiếu.
DetailLevel: Enum (ViewDetailLevel.Coarse, Medium, Fine).
IncludeNonVisibleObjects: Boolean để lấy hình học ẩn.
View: Kiểu View để trích xuất hình học theo view.

Hành động trên màn hình:  

Nhập “Options” trong Object Browser, highlight các thuộc tính ComputeReferences, DetailLevel.  
Chèn text: “Options: Cấu hình chi tiết hình học.”


Bước 6: Minh họa mã mẫuĐể minh họa, hiển thị một đoạn mã mẫu giả lập (không triển khai lệnh cụ thể trong bài này, chỉ giới thiệu):
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace MyRevitCommands
{
    public class GetGeometryExample
    {
        public void Execute(UIDocument uidoc, Element element)
        {
            Document doc = uidoc.Document;
            Options options = new Options
            {
                ComputeReferences = true,
                DetailLevel = ViewDetailLevel.Fine,
                IncludeNonVisibleObjects = false
            };

            GeometryElement geom = element.Get_Geometry(options);
            foreach (GeometryObject obj in geom)
            {
                if (obj is Solid solid)
                {
                    // Xử lý Solid, ví dụ: tính diện tích mặt
                    foreach (Face face in solid.Faces)
                    {
                        TaskDialog.Show("Face Area", $"Area: {face.Area}");
                    }
                }
                else if (obj is GeometryInstance instance)
                {
                    GeometryElement instGeom = instance.GetInstanceGeometry();
                    // Lặp tiếp để xử lý
                }
            }
        }
    }
}

Hành động trên màn hình:  

Hiển thị đoạn mã trên trong Visual Studio, highlight Options, Get_Geometry, và vòng foreach.  
Chèn text: “Mã mẫu: Truy xuất và xử lý GeometryElement.”




Phần 3: Ứng dụng của hình học trong Revit
Hướng dẫn viên (giọng điệu khích lệ):Hình học trong Revit API có nhiều ứng dụng thực tế. Hãy điểm qua một số ví dụ!

Bước 7: Ứng dụng thực tếHình học từ GeometryElement có thể được sử dụng để:

Tính toán vị trí: Xác định vị trí tương đối giữa các phần tử (ví dụ: khoảng cách từ cửa đến tường).
Định nghĩa phần tử mới: Sử dụng hình học của phần tử hiện có để tạo phần tử khác (ví dụ: tạo sàn từ đường viền tường).
Trích xuất thuộc tính: Tính diện tích, chiều dài, hoặc thể tích (ví dụ: diện tích mặt của Solid).

Hành động trên màn hình:  

Hiển thị sơ đồ minh họa:  Ứng dụng GeometryElement
├── Tính khoảng cách giữa phần tử
├── Tạo phần tử mới từ hình học
└── Tính diện tích, thể tích


Chèn text: “Ứng dụng: Tận dụng hình học trong Revit API.”




Phần 4: Kết luận và bước tiếp theo
Hướng dẫn viên (giọng điệu truyền cảm hứng):Chúc mừng các bạn! Hôm nay, chúng ta đã:

Tìm hiểu các loại hình học trong Revit API, như Solid, Mesh, GeometryInstance.  
Khám phá cách lấy GeometryElement bằng Get_Geometry và tùy chỉnh với Options.  
Hiểu các ứng dụng thực tế của hình học trong lập trình Revit.

Trong bài học tiếp theo, chúng ta sẽ thực hiện một lệnh cụ thể để trích xuất và xử lý hình học từ một phần tử.
Bước 8: Kêu gọi hành độngHãy thử mở Object Browser và khám phá thêm các phương thức của Solid hoặc Mesh. Tìm hiểu cách lấy Face hoặc Edge từ Solid. Nếu bạn gặp vấn đề, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay!
Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!
Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"Bài học tiếp theo: Trích xuất và xử lý hình học."  
"Gặp vấn đề? Để lại câu hỏi trong phần bình luận!"


Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio, mô phỏng mô hình 3D với các loại hình học.

