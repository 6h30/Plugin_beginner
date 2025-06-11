Kịch bản bài học: Tạo phần tử vòng khép kín trong Revit
Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ học cách tạo các phần tử vòng khép kín, như sàn (floor), sử dụng CurveLoop và CurveArray trong Revit API. Chúng ta sẽ tạo một lệnh PlaceLoopElement để tạo một sàn dựa trên các đường từ bài trước, với vòng offset để căn chỉnh với mặt ngoài của tường. Sau bài học này, bạn sẽ biết:

Cách sử dụng Creation.Document.NewFloor để tạo sàn.
Cách tạo và offset CurveLoop để định hình sàn.
Cách chuyển đổi CurveLoop thành CurveArray và quản lý Transaction.

Hãy cùng bắt đầu!

Phần 1: Tìm hiểu NewFloor và CurveLoop trong Object Browser
Hướng dẫn viên (giọng điệu hào hứng):Trong bài trước, chúng ta đã tạo các tường dựa trên đường. Hôm nay, chúng ta sẽ tái sử dụng các đường đó để tạo một sàn sử dụng CurveLoop và CurveArray. Hãy khám phá các phương thức cần thiết trước!

Bước 1: Khám phá NewFloorTrong Visual Studio, mở Object Browser (đảm bảo Custom Component Set bao gồm RevitAPI.dll và RevitAPIUI.dll). Tìm “NewFloor” trong ô tìm kiếm. Bạn sẽ thấy các phương thức NewFloor trong namespace Autodesk.Revit.Creation.Document, không phải trong lớp Floor (đặc điểm của Revit API cũ). Chọn phiên bản đơn giản với tham số:

CurveArray: Tập hợp các đường khép kín để định hình sàn.
Boolean: Xác định sàn có phải là kết cấu (structural) hay không.

Hành động trên màn hình:  

Hiển thị Solution Explorer, nhấp đúp vào RevitAPI để mở Object Browser.  
Nhập “NewFloor”, highlight phương thức Creation.Document.NewFloor(CurveArray, Boolean).  
Chèn text: “NewFloor: Tạo sàn từ CurveArray.”


Bước 2: Hiểu CurveLoop và CurveArrayTrong Object Browser, tìm “CurveLoop” và “CurveArray”:

CurveArray: Một tập hợp đơn giản chứa các Curve, sử dụng phương thức Append để thêm Curve.
CurveLoop: Một danh sách các Curve được sắp xếp tuần tự, khép kín, hỗ trợ các thao tác như CreateViaOffset để tạo vòng offset. Phương thức CreateViaOffset cần:
CurveLoop: Vòng đường gốc.
Double: Khoảng cách offset (tính bằng feet trong Revit API).
XYZ: Vector pháp tuyến (normal) để xác định hướng offset.



Hành động trên màn hình:  

Trong Object Browser, tìm “CurveLoop”, highlight CreateViaOffset.  
Tìm “CurveArray”, highlight phương thức Append.  
Chèn text: “CurveLoop & CurveArray: Tạo vòng khép kín cho sàn.”




Phần 2: Tạo lệnh PlaceLoopElement trong Visual Studio
Hướng dẫn viên (giọng điệu rõ ràng):Hãy mở Visual Studio để tạo lệnh PlaceLoopElement, tái sử dụng các đường từ bài trước, tạo CurveLoop offset, chuyển thành CurveArray, và sử dụng NewFloor để tạo sàn.

Bước 3: Mở tệp PlaceLoopElement.csMở tệp PlaceLoopElement.cs trong dự án MyRevitCommands. Mã cơ bản đã được chuẩn bị, dựa trên bài trước, với FilteredElementCollector tìm Level và danh sách Curve:
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class PlaceLoopElement : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            Level level = collector.OfClass(typeof(Level)).Cast<Level>().First(x => x.Name == "Ground Floor");

            XYZ p1 = new XYZ(-10, -10, 0);
            XYZ p2 = new XYZ(10, -10, 0);
            XYZ p3 = new XYZ(15, 0, 0);
            XYZ p4 = new XYZ(10, 10, 0);
            XYZ p5 = new XYZ(-10, 10, 0);

            List<Curve> curves = new List<Curve>();
            Curve l1 = Line.CreateBound(p1, p2);
            Curve l2 = Arc.Create(p1, p4, p3);
            Curve l3 = Line.CreateBound(p4, p5);
            Curve l4 = Line.CreateBound(p5, p1);

            curves.Add(l1);
            curves.Add(l2);
            curves.Add(l3);
            curves.Add(l4);

            return Result.Succeeded;
        }
    }
}

Hành động trên màn hình:  

Hiển thị Solution Explorer, nhấp vào PlaceLoopElement.cs.  
Zoom vào FilteredElementCollector và danh sách curves, highlight Arc.Create.  
Chèn text: “Điểm khởi đầu: Tái sử dụng Curves từ bài trước.”


Bước 4: Tạo CurveLoopThêm mã để tạo một CurveLoop từ danh sách curves:
CurveLoop crvLoop = CurveLoop.Create(curves);

Hành động trên màn hình:  

Nhập dòng CurveLoop.Create, highlight curves.  
Chèn text: “CurveLoop: Tạo vòng khép kín từ Curves.”


Bước 5: Tạo offset CurveLoopTính khoảng cách offset dựa trên độ dày tường (Basic Wall, Double Brick, 270 mm, offset 135 mm). Vì Revit API sử dụng feet, chuyển đổi 135 mm sang feet bằng UnitUtils:
double offset = UnitUtils.ConvertToInternalUnits(135, DisplayUnitType.DUT_MILLIMETERS);

Tạo CurveLoop offset với vector pháp tuyến theo trục Z (lên):
XYZ normal = new XYZ(0, 0, 1);
CurveLoop offsetCrv = CurveLoop.CreateViaOffset(crvLoop, offset, normal);

Hành động trên màn hình:  

Nhập dòng UnitUtils.ConvertToInternalUnits, highlight 135 và DUT_MILLIMETERS.  
Nhập CurveLoop.CreateViaOffset, highlight offset và normal.  
Chèn sơ đồ minh họa: Vòng crvLoop (đường trung tâm tường) và offsetCrv (căn ngoài tường).  
Chèn text: “Offset: Tạo vòng căn chỉnh với mặt ngoài tường.”


Bước 6: Chuyển CurveLoop thành CurveArrayTạo CurveArray và thêm các Curve từ offsetCrv:
CurveArray cArray = new CurveArray();
foreach (Curve c in offsetCrv)
{
    cArray.Append(c);
}

Hành động trên màn hình:  

Nhập mã tạo CurveArray và vòng foreach, highlight cArray.Append.  
Chèn text: “CurveArray: Chuyển CurveLoop thành tập hợp Curves.”


Bước 7: Tạo sàn với TransactionThêm khối try-catch với Transaction để tạo sàn sử dụng NewFloor:
try
{
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

Mã hoàn chỉnh trong phương thức Execute:
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class PlaceLoopElement : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            Level level = collector.OfClass(typeof(Level)).Cast<Level>().First(x => x.Name == "Ground Floor");

            XYZ p1 = new XYZ(-10, -10, 0);
            XYZ p2 = new XYZ(10, -10, 0);
            XYZ p3 = new XYZ(15, 0, 0);
            XYZ p4 = new XYZ(10, 10, 0);
            XYZ p5 = new XYZ(-10, 10, 0);

            List<Curve> curves = new List<Curve>();
            Curve l1 = Line.CreateBound(p1, p2);
            Curve l2 = Arc.Create(p1, p4, p3);
            Curve l3 = Line.CreateBound(p4, p5);
            Curve l4 = Line.CreateBound(p5, p1);

            curves.Add(l1);
            curves.Add(l2);
            curves.Add(l3);
            curves.Add(l4);

            CurveLoop crvLoop = CurveLoop.Create(curves);
            double offset = UnitUtils.ConvertToInternalUnits(135, DisplayUnitType.DUT_MILLIMETERS);
            XYZ normal = new XYZ(0, 0, 1);
            CurveLoop offsetCrv = CurveLoop.CreateViaOffset(crvLoop, offset, normal);

            CurveArray cArray = new CurveArray();
            foreach (Curve c in offsetCrv)
            {
                cArray.Append(c);
            }

            try
            {
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

Hành động trên màn hình:  

Nhập khối try-catch và Transaction, highlight doc.Create.NewFloor.  
Chèn text: “Transaction: Tạo sàn từ CurveArray.”




Phần 3: Đăng ký và kiểm tra lệnh
Hướng dẫn viên (giọng điệu khích lệ):Lệnh PlaceLoopElement đã được thêm vào tệp manifest với tên “PlaceLoopElement”. Hãy kiểm tra nó trong Revit!

Bước 8: Chạy Debug và kiểm traTrong Visual Studio, nhấn F5 để chạy debug mode. Mở Revit, mở tệp bài tập của video này. Trong Ground Floor plan, chạy Add-Ins > External Tools > PlaceLoopElement. Chuyển sang 3D View để thấy sàn mới được tạo, căn chỉnh với mặt ngoài của các tường từ bài trước.
Hành động trên màn hình:  

Hiển thị Visual Studio, nhấn F5.  
Trong Revit, mở tệp bài tập, chạy PlaceLoopElement trong Ground Floor plan.  
Chuyển sang 3D View, zoom vào sàn, highlight vòng khép kín.  
Chèn text: “Kiểm tra: Tạo sàn căn chỉnh với mặt ngoài tường.”




Phần 4: Kết luận và bước tiếp theo
Hướng dẫn viên (giọng điệu truyền cảm hứng):Chúc mừng các bạn! Hôm nay, chúng ta đã:

Tìm hiểu NewFloor và các loại CurveLoop, CurveArray trong Object Browser.  
Tạo CurveLoop offset để căn chỉnh sàn với mặt ngoài tường.  
Sử dụng Transaction để tạo sàn trong lệnh PlaceLoopElement.

Kỹ thuật này áp dụng cho các phần tử vòng khép kín khác như mái (roofs) hoặc trần (ceilings). Để nâng cao, bạn có thể lọc tường trong view để tự động tính khoảng cách offset thay vì mã cứng 135 mm.
Bước 9: Kêu gọi hành độngHãy thử thay đổi khoảng cách offset hoặc thêm try-catch quanh FilteredElementCollector để xử lý trường hợp không tìm thấy Level. Thử lấy đơn vị từ Document bằng UnitUtils để tự động hóa đơn vị. Nếu bạn gặp vấn đề, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay!
Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!
Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"Bài học tiếp theo: Tạo phần tử vòng khép kín nâng cao."  
"Gặp vấn đề? Để lại câu hỏi trong phần bình luận!"


Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio, mô phỏng sàn trong 3D View.

