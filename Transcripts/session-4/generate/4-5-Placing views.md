Kịch bản bài học: Đặt View lên Sheet trong Revit
Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ học cách đặt một view lên sheet trong Revit API, hoàn thiện quy trình tạo bản vẽ. Chúng ta sẽ tạo một lệnh PlaceView để đặt một ViewPlan vào trung tâm của một ViewSheet, sử dụng Viewport và tính toán vị trí trung tâm bằng BoundingBoxUV. Sau bài học này, bạn sẽ biết:

Viewport là gì và cách sử dụng nó để đặt view lên sheet.
Cách tính trung tâm sheet bằng BoundingBoxUV.
Cách kiểm tra tính hợp lệ của view trước khi đặt.

Hãy cùng bắt đầu!

Phần 1: Tìm hiểu về Viewport trong Revit API
Hướng dẫn viên (giọng điệu hào hứng):Trong các bài trước, chúng ta đã tạo view, gắn thẻ, và tạo sheet. Hôm nay, chúng ta sẽ hoàn thiện quy trình bằng cách đặt view lên sheet, sử dụng Viewport. Hãy khám phá cách hoạt động của Viewport!

Bước 1: Viewport trong Revit APIViewport là một lớp đặc biệt, không kế thừa từ View mà từ Element, dùng để đặt các view (như ViewPlan, ViewSection) lên ViewSheet. Đặc điểm:

Tạo bằng phương thức tĩnh Viewport.Create, yêu cầu:
Document: Tài liệu Revit.
SheetId: ID của ViewSheet.
ViewId: ID của view cần đặt.
Point: Tọa độ XYZ để đặt Viewport.


Lưu ý: Không phải tất cả view đều có thể đặt lên sheet (ví dụ: Schedule). Sử dụng Viewport.CanAddViewToSheet để kiểm tra tính hợp lệ.

Hành động trên màn hình:  

Hiển thị sơ đồ:  Viewport
├── Inherits: Element
├── Create: Document, SheetId, ViewId, XYZ
├── Properties: Position, Size
└── CanAddViewToSheet: Kiểm tra view hợp lệ


Chèn text: “Viewport: Đặt view lên sheet.”


Bước 2: Tính trung tâm Sheet bằng BoundingBoxUVĐể đặt Viewport ở trung tâm sheet:

Lấy Outline của ViewSheet bằng thuộc tính Outline, trả về BoundingBoxUV (hình chữ nhật 2D với tọa độ U, V).
Tính trung tâm:
X = (Max.U + Min.U) / 2
Y = (Max.V + Min.V) / 2
Z = 0 (vì sheet là mặt phẳng 2D).


Sử dụng tọa độ XYZ(X, Y, 0) để đặt Viewport.

Hành động trên màn hình:  

Hiển thị mã giả lập:  BoundingBoxUV outline = sheet.Outline;
double xu = (outline.Max.U + outline.Min.U) / 2;
double yu = (outline.Max.V + outline.Min.V) / 2;
XYZ midPoint = new XYZ(xu, yu, 0);
Viewport viewport = Viewport.Create(doc, sheetId, viewId, midPoint);


Chèn text: “BoundingBoxUV: Tính trung tâm sheet.”




Phần 2: Tạo lệnh PlaceView trong Visual Studio
Hướng dẫn viên (giọng điệu rõ ràng):Hãy mở Visual Studio để tạo lệnh PlaceView, đặt một ViewPlan vào trung tâm của một ViewSheet, sử dụng Viewport và tính toán vị trí bằng BoundingBoxUV.

Bước 3: Mở tệp PlaceView.csMở tệp PlaceView.cs trong dự án MyRevitCommands. Mã cơ bản đã được chuẩn bị, lấy Document, ViewSheet, và ViewPlan, với Transaction:
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class PlaceView : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                // Lấy ViewSheet
                ViewSheet vSheet = new FilteredElementCollector(doc)
                    .OfClass(typeof(ViewSheet))
                    .Cast<ViewSheet>()
                    .FirstOrDefault(s => s.Name == "My First Sheet");

                // Lấy ViewPlan
                ViewPlan vPlan = new FilteredElementCollector(doc)
                    .OfClass(typeof(ViewPlan))
                    .Cast<ViewPlan>()
                    .FirstOrDefault(v => v.Name == "Our first plan!");

                if (vSheet != null && vPlan != null)
                {
                    using (Transaction trans = new Transaction(doc, "Place View"))
                    {
                        trans.Start();
                        // Đặt view tại đây
                        trans.Commit();
                    }
                }
                else
                {
                    TaskDialog.Show("Error", "Sheet or View not found.");
                    return Result.Failed;
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

Hiển thị Solution Explorer, nhấp vào PlaceView.cs.  
Zoom vào khối if (vSheet != null && vPlan != null), highlight vSheet và vPlan.  
Chèn text: “PlaceView: Điểm khởi đầu để đặt view.”


Bước 4: Tính trung tâm SheetLấy BoundingBoxUV và tính tọa độ trung tâm:
BoundingBoxUV outline = vSheet.Outline;
double xu = (outline.Max.U + outline.Min.U) / 2;
double yu = (outline.Max.V + outline.Min.V) / 2;
XYZ midPoint = new XYZ(xu, yu, 0);


Outline: Lấy hình chữ nhật 2D bao quanh sheet.
xu, yu: Tính tọa độ trung tâm theo công thức.
XYZ: Tạo điểm trung tâm với Z = 0.

Hành động trên màn hình:  

Nhập mã BoundingBoxUV, highlight Max.U và Min.V.  
Chèn sơ đồ minh họa U, V trên sheet.  
Chèn text: “BoundingBoxUV: Tính trung tâm sheet.”


Bước 5: Tạo ViewportTrong Transaction, tạo Viewport để đặt view:
Viewport viewport = Viewport.Create(doc, vSheet.Id, vPlan.Id, midPoint);

Hành động trên màn hình:  

Nhập mã Viewport.Create, highlight midPoint.  
Chèn text: “Viewport: Đặt view vào trung tâm.”


Bước 6: Kiểm tra tính hợp lệ của ViewThêm kiểm tra CanAddViewToSheet để đảm bảo view có thể đặt lên sheet:
if (Viewport.CanAddViewToSheet(doc, vSheet.Id, vPlan.Id))
{
    Viewport viewport = Viewport.Create(doc, vSheet.Id, vPlan.Id, midPoint);
}
else
{
    TaskDialog.Show("Error", "View cannot be placed on this sheet.");
    return Result.Failed;
}

Mã hoàn chỉnh trong phương thức Execute:
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
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                // Lấy ViewSheet
                ViewSheet vSheet = new FilteredElementCollector(doc)
                    .OfClass(typeof(ViewSheet))
                    .Cast<ViewSheet>()
                    .FirstOrDefault(s => s.Name == "My First Sheet");

                // Lấy ViewPlan
                ViewPlan vPlan = new FilteredElementCollector(doc)
                    .OfClass(typeof(ViewPlan))
                    .Cast<ViewPlan>()
                    .FirstOrDefault(v => v.Name == "Our first plan!");

                if (vSheet != null && vPlan != null)
                {
                    BoundingBoxUV outline = vSheet.Outline;
                    double xu = (outline.Max.U + outline.Min.U) / 2;
                    double yu = (outline.Max.V + outline.Min.V) / 2;
                    XYZ midPoint = new XYZ(xu, yu, 0);

                    using (Transaction trans = new Transaction(doc, "Place View"))
                    {
                        trans.Start();
                        if (Viewport.CanAddViewToSheet(doc, vSheet.Id, vPlan.Id))
                        {
                            Viewport viewport = Viewport.Create(doc, vSheet.Id, vPlan.Id, midPoint);
                        }
                        else
                        {
                            TaskDialog.Show("Error", "View cannot be placed on this sheet.");
                            return Result.Failed;
                        }
                        trans.Commit();
                    }
                }
                else
                {
                    TaskDialog.Show("Error", "Sheet or View not found.");
                    return Result.Failed;
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

Nhập mã CanAddViewToSheet, highlight Viewport.Create.  
Chèn text: “CanAddViewToSheet: Kiểm tra tính hợp lệ.”




Phần 3: Đăng ký và kiểm tra lệnh
Hướng dẫn viên (giọng điệu khích lệ):Lệnh PlaceView đã được thêm vào tệp manifest với tên “PlaceView”. Hãy kiểm tra nó trong Revit!

Bước 7: Chạy Debug và kiểm traTrong Visual Studio, nhấn F5 để chạy debug mode. Mở Revit, mở tệp bài tập của video này, chứa ViewSheet “My First Sheet” và ViewPlan “Our first plan!”. Chạy Add-Ins > External Tools > PlaceView. Kết quả: ViewPlan sẽ được đặt ở trung tâm ViewSheet thông qua Viewport.
Hành động trên màn hình:  

Hiển thị Visual Studio, nhấn F5.  
Trong Revit, mở tệp bài tập, chạy PlaceView.  
Mở Project Browser, nhấp vào “My First Sheet”, zoom vào view ở trung tâm sheet.  
Chèn text: “Kiểm tra: Đặt ViewPlan vào trung tâm sheet.”




Phần 4: Kết luận và bước tiếp theo
Hướng dẫn viên (giọng điệu truyền cảm hứng):Chúc mừng các bạn! Hôm nay, chúng ta đã:

Tìm hiểu Viewport và cách đặt view lên sheet.  
Tạo lệnh PlaceView để đặt ViewPlan vào trung tâm ViewSheet.  
Sử dụng BoundingBoxUV để tính toán vị trí và kiểm tra tính hợp lệ bằng CanAddViewToSheet.

Tự động đặt view lên sheet mở ra tiềm năng lớn để tự động hóa quy trình tạo bản vẽ.
Bước 8: Kêu gọi hành độngHãy thử đặt nhiều view lên sheet hoặc điều chỉnh vị trí Viewport (ví dụ: dịch chuyển sang trái/phải). Hoặc khám phá các thuộc tính khác của Viewport, như kích thước. Nếu bạn gặp vấn đề, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay!
Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!
Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"Bài học tiếp theo: Tự động hóa bản vẽ nâng cao."  
"Gặp vấn đề? Để lại câu hỏi trong phần bình luận!"


Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio, mô phỏng ViewSheet với Viewport.

