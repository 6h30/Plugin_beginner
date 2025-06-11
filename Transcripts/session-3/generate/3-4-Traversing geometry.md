Kịch bản bài học: Duyệt hình học trong Revit
Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ học cách duyệt hình học của một phần tử, cụ thể là trích xuất các mặt (Face) của một tường và tính tổng diện tích của chúng. Chúng ta sẽ tạo một lệnh SelectGeometry để chọn một tường, đếm số mặt, và hiển thị diện tích bằng mét vuông. Sau bài học này, bạn sẽ biết:

Cách duyệt GeometryElement để lấy Solid và Face.
Cách tính diện tích các mặt và chuyển đổi đơn vị bằng UnitUtils.
Cách hiển thị kết quả bằng TaskDialog.

Hãy cùng bắt đầu!

Phần 1: Tìm hiểu về duyệt hình học
Hướng dẫn viên (giọng điệu hào hứng):Trong bài trước, chúng ta đã tìm hiểu các loại hình học trong Revit API và cách lấy GeometryElement. Hôm nay, chúng ta sẽ áp dụng kiến thức đó để duyệt hình học, tập trung vào việc lấy các Face từ một Solid của tường và tính diện tích.

Bước 1: Quy trình duyệt GeometryElementĐể trích xuất hình học từ một phần tử:

Tạo một đối tượng Options để cấu hình đầu ra (ví dụ: mức chi tiết Fine).
Lấy GeometryElement bằng Get_Geometry(Options).
Duyệt qua GeometryElement bằng vòng foreach để lấy các GeometryObject.
Ép kiểu GeometryObject thành Solid, Mesh, hoặc GeometryInstance. Với tường, ta thường nhận được Solid.
Duyệt qua Faces của Solid để lấy diện tích từng mặt.

Hành động trên màn hình:  

Hiển thị sơ đồ:  Element
└── GeometryElement (Get_Geometry)
    └── GeometryObject
        └── Solid
            └── Face (Area)


Chèn text: “Duyệt hình học: Từ Element đến Face.”


Bước 2: Chuyển đổi đơn vị và hiển thịDiện tích trong Revit API được tính bằng feet vuông (feet²). Để hiển thị bằng mét vuông (m²), sử dụng UnitUtils.ConvertFromInternalUnits với DisplayUnitType.DUT_SQUARE_METERS. Kết quả được hiển thị bằng TaskDialog.
Hành động trên màn hình:  

Hiển thị mã giả lập:  double area = face.Area; // feet²
area = UnitUtils.ConvertFromInternalUnits(area, DisplayUnitType.DUT_SQUARE_METERS); // m²
TaskDialog.Show("Result", $"Area: {area} m²");


Chèn text: “UnitUtils: Chuyển đổi diện tích sang mét vuông.”




Phần 2: Tạo lệnh SelectGeometry trong Visual Studio
Hướng dẫn viên (giọng điệu rõ ràng):Hãy mở Visual Studio để tạo lệnh SelectGeometry, cho phép chọn một tường, trích xuất các Face từ Solid, tính tổng diện tích, và hiển thị số mặt cùng diện tích bằng TaskDialog.

Bước 3: Mở tệp SelectGeometry.csMở tệp SelectGeometry.cs trong dự án MyRevitCommands. Mã cơ bản đã được chuẩn bị, cho phép chọn phần tử:
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.ReadOnly)]
    public class SelectGeometry : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                Reference pickedObj = uidoc.Selection.PickObject(ObjectType.Element);
                if (pickedObj != null)
                {
                    Element ele = doc.GetElement(pickedObj);
                    // Trích xuất hình học tại đây
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

Hành động trên màn hình:  

Hiển thị Solution Explorer, nhấp vào SelectGeometry.cs.  
Zoom vào khối if (pickedObj != null), highlight ele.  
Chèn text: “SelectGeometry: Điểm khởi đầu để duyệt hình học.”


Bước 4: Tạo Options và lấy GeometryElementTrong khối if, tạo đối tượng Options với mức chi tiết Fine và lấy GeometryElement:
Options gOptions = new Options();
gOptions.DetailLevel = ViewDetailLevel.Fine;
GeometryElement geom = ele.Get_Geometry(gOptions);


Options: Đặt DetailLevel thành Fine để lấy hình học chi tiết.
Get_Geometry: Truy xuất GeometryElement với Options.

Hành động trên màn hình:  

Nhập các dòng Options, highlight ViewDetailLevel.Fine.  
Chèn text: “Options: Cấu hình hình học chi tiết.”


Bước 5: Duyệt GeometryObject và lấy SolidDuyệt qua GeometryElement để tìm Solid:
foreach (GeometryObject gObj in geom)
{
    Solid gSolid = gObj as Solid;
    // Xử lý Solid tại đây
}

Lưu ý: Nếu gObj không phải Solid (ví dụ: GeometryInstance), gSolid sẽ là null. Để an toàn, có thể thêm kiểm tra if (gSolid != null), nhưng bài này giả định chọn tường (luôn có Solid).
Hành động trên màn hình:  

Nhập vòng foreach, highlight gObj as Solid.  
Chèn text: “Solid: Ép kiểu từ GeometryObject.”


Bước 6: Đếm Face và tính diện tíchKhởi tạo biến để đếm số mặt (faces) và tổng diện tích (area). Duyệt qua Faces của Solid để tính toán:
int faces = 0;
double area = 0.0;

foreach (GeometryObject gObj in geom)
{
    Solid gSolid = gObj as Solid;
    foreach (Face gFace in gSolid.Faces)
    {
        area += gFace.Area;
        faces++;
    }
}


gSolid.Faces: Trả về FaceArray (danh sách các Face).
gFace.Area: Lấy diện tích của mặt (feet²).
faces++: Tăng bộ đếm số mặt.

Lưu ý: Cần sửa lỗi trong transcript: vòng lặp foreach (Face gFace in gSolid) phải là gSolid.Faces.
Hành động trên màn hình:  

Nhập biến faces, area, và vòng foreach cho Faces, highlight gFace.Area và faces++.  
Chèn text: “Faces: Tính diện tích và đếm số mặt.”


Bước 7: Chuyển đổi đơn vị sang mét vuôngChuyển area từ feet² sang m² bằng UnitUtils:
area = UnitUtils.ConvertFromInternalUnits(area, DisplayUnitType.DUT_SQUARE_METERS);

Hành động trên màn hình:  

Nhập dòng UnitUtils, highlight DUT_SQUARE_METERS.  
Chèn text: “UnitUtils: Chuyển đổi sang mét vuông.”


Bước 8: Hiển thị kết quả bằng TaskDialogHiển thị số mặt và diện tích bằng TaskDialog:
TaskDialog.Show("Geometry", string.Format("Number of Faces: {0}{1}Total Area: {2} square meters", 
    faces, Environment.NewLine, area));


Environment.NewLine: Tạo dòng mới trong TaskDialog.
string.Format: Định dạng chuỗi với faces và area.

Thêm namespace System để sử dụng Environment:
using System;

Mã hoàn chỉnh trong phương thức Execute:
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.ReadOnly)]
    public class SelectGeometry : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                Reference pickedObj = uidoc.Selection.PickObject(ObjectType.Element);
                if (pickedObj != null)
                {
                    Element ele = doc.GetElement(pickedObj);
                    Options gOptions = new Options();
                    gOptions.DetailLevel = ViewDetailLevel.Fine;
                    GeometryElement geom = ele.Get_Geometry(gOptions);

                    int faces = 0;
                    double area = 0.0;

                    foreach (GeometryObject gObj in geom)
                    {
                        Solid gSolid = gObj as Solid;
                        if (gSolid != null)
                        {
                            foreach (Face gFace in gSolid.Faces)
                            {
                                area += gFace.Area;
                                faces++;
                            }
                        }
                    }

                    area = UnitUtils.ConvertFromInternalUnits(area, DisplayUnitType.DUT_SQUARE_METERS);
                    TaskDialog.Show("Geometry", string.Format("Number of Faces: {0}{1}Total Area: {2} square meters", 
                        faces, Environment.NewLine, area));
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

Hành động trên màn hình:  

Nhập mã TaskDialog, highlight Environment.NewLine.  
Thêm using System; ở đầu tệp.  
Thêm kiểm tra if (gSolid != null) để an toàn.  
Chèn text: “TaskDialog: Hiển thị số mặt và diện tích.”




Phần 3: Đăng ký và kiểm tra lệnh
Hướng dẫn viên (giọng điệu khích lệ):Lệnh SelectGeometry đã được thêm vào tệp manifest với tên “SelectGeometry”. Hãy kiểm tra nó trong Revit!

Bước 9: Chạy Debug và kiểm traTrong Visual Studio, nhấn F5 để chạy debug mode. Mở Revit, mở tệp bài tập của video này. Chạy Add-Ins > External Tools > SelectGeometry, chọn một tường. TaskDialog sẽ hiển thị:

Số mặt (ví dụ: 6 faces).
Tổng diện tích (ví dụ: 51.9 m²).

Hành động trên màn hình:  

Hiển thị Visual Studio, nhấn F5.  
Trong Revit, mở tệp bài tập, chạy SelectGeometry, chọn một tường.  
Hiển thị TaskDialog với kết quả, zoom vào tường để minh họa.  
Chèn text: “Kiểm tra: Tính số mặt và diện tích tường.”




Phần 4: Kết luận và bước tiếp theo
Hướng dẫn viên (giọng điệu truyền cảm hứng):Chúc mừng các bạn! Hôm nay, chúng ta đã:

Duyệt GeometryElement để lấy Solid và Face từ một tường.  
Tính tổng diện tích và số mặt, chuyển đổi sang mét vuông.  
Hiển thị kết quả bằng TaskDialog trong lệnh SelectGeometry.

Kỹ thuật này rất hữu ích để phân tích hình học, như tính diện tích bề mặt hoặc kiểm tra cấu trúc phần tử.
Bước 10: Kêu gọi hành độngHãy thử thay đổi DetailLevel thành Coarse hoặc Medium để xem số mặt thay đổi thế nào. Hoặc thêm kiểm tra if (gSolid != null) trong vòng foreach để xử lý các phần tử khác (như cửa). Nếu bạn gặp vấn đề, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay!
Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!
Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"Bài học tiếp theo: Phân tích hình học nâng cao."  
"Gặp vấn đề? Để lại câu hỏi trong phần bình luận!"


Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio, mô phỏng TaskDialog và tường.

