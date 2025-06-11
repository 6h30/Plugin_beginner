Kịch bản bài học: Tạo phần tử dựa trên đường trong Revit
Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ học cách tạo các phần tử dựa trên đường, cụ thể là tạo các tường sử dụng đường thẳng và đường cong. Chúng ta sẽ tạo một lệnh PlaceLineElement để vẽ một hình khép kín với ba tường thẳng và một tường cong. Sau bài học này, bạn sẽ biết:

Cách sử dụng phương thức Wall.Create để tạo tường dựa trên Curve.
Cách tạo các Curve (Line và Arc) sử dụng XYZ điểm.
Cách sử dụng FilteredElementCollector để tìm Level và quản lý Transaction để tạo phần tử.

Hãy cùng bắt đầu!

Phần 1: Tìm hiểu Wall.Create và Curve trong Object Browser
Hướng dẫn viên (giọng điệu hào hứng):Trong bài trước, chúng ta đã đặt một Family Instance dựa trên điểm. Hôm nay, chúng ta sẽ tạo các tường dựa trên đường bằng cách sử dụng Wall.Create và các loại Curve như Line và Arc. Hãy khám phá các phương thức này trước!

Bước 1: Khám phá Wall.CreateTrong Visual Studio, mở Object Browser bằng cách nhấp đúp vào tham chiếu RevitAPI (đảm bảo Custom Component Set bao gồm RevitAPI.dll và RevitAPIUI.dll). Tìm “Wall” trong ô tìm kiếm và chọn lớp Wall. Bạn sẽ thấy các phương thức Create, bao gồm một phiên bản với các tham số:

Document: Tài liệu Revit.
Curve: Đường trung tâm của tường.
ElementId: ID của Level mà tường gắn vào.
Boolean: Xác định tường có phải là kết cấu (structural) hay không.

Chúng ta sẽ sử dụng phiên bản này để tạo tường.
Hành động trên màn hình:  

Hiển thị Solution Explorer, nhấp đúp vào RevitAPI để mở Object Browser.  
Nhập “Wall”, highlight lớp Wall, zoom vào phương thức Create(Document, Curve, ElementId, Boolean).  
Chèn text: “Wall.Create: Tạo tường dựa trên Curve.”


Bước 2: Khám phá Curve và các loại conTrong Object Browser, tìm “Curve” và mở rộng Derived Types để xem các loại đường:

Line: Đường thẳng, sử dụng Line.CreateBound(XYZ start, XYZ end).
Arc: Đường cong, sử dụng Arc.Create(XYZ start, XYZ end, XYZ mid).

Chúng ta sẽ tạo ba đường thẳng và một đường cong để định hình tường.
Hành động trên màn hình:  

Trong Object Browser, tìm “Curve”, mở rộng Derived Types, highlight Line và Arc.  
Zoom vào Line.CreateBound và Arc.Create.  
Chèn text: “Curve: Line và Arc để định hình tường.”




Phần 2: Tạo lệnh PlaceLineElement trong Visual Studio
Hướng dẫn viên (giọng điệu rõ ràng):Hãy mở Visual Studio để tạo lệnh PlaceLineElement, tìm Level có tên “Ground Floor”, tạo các Curve, và sử dụng Wall.Create để vẽ tường.

Bước 3: Mở tệp PlaceLineElement.csMở tệp PlaceLineElement.cs trong dự án MyRevitCommands. Mã cơ bản đã được chuẩn bị với FilteredElementCollector để tìm Level:
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Linq;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class PlaceLineElement : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            Level level = collector.OfClass(typeof(Level)).Cast<Level>().First(x => x.Name == "Ground Floor");

            return Result.Succeeded;
        }
    }
}

Lưu ý: Nếu không tìm thấy Level “Ground Floor”, mã sẽ gây ngoại lệ. Để an toàn, bạn có thể bọc trong try-catch.
Hành động trên màn hình:  

Hiển thị Solution Explorer, nhấp vào PlaceLineElement.cs.  
Zoom vào FilteredElementCollector tìm Level, highlight x => x.Name == "Ground Floor".  
Chèn text: “FilteredElementCollector: Tìm Level ‘Ground Floor’.”


Bước 4: Tạo các điểm XYZThêm mã để tạo năm điểm XYZ cho các góc và điểm giữa của đường cong, theo thứ tự kim đồng hồ từ góc dưới bên trái:
XYZ p1 = new XYZ(-10, -10, 0);
XYZ p2 = new XYZ(10, -10, 0);
XYZ p3 = new XYZ(15, 0, 0); // Điểm giữa cho arc
XYZ p4 = new XYZ(10, 10, 0);
XYZ p5 = new XYZ(-10, 10, 0);

Hành động trên màn hình:  

Nhập các dòng tạo XYZ, highlight các tọa độ như (-10, -10, 0).  
Chèn sơ đồ minh họa 5 điểm trên mặt phẳng 2D với nhãn p1, p2, p3, p4, p5.  
Chèn text: “XYZ: Định nghĩa các điểm cho tường.”


Bước 5: Tạo các Curve (Line và Arc)Tạo một danh sách List và thêm ba đường thẳng (Line) và một đường cong (Arc):
List<Curve> curves = new List<Curve>();
Curve l1 = Line.CreateBound(p1, p2);
Curve l2 = Arc.Create(p1, p4, p3);
Curve l3 = Line.CreateBound(p4, p5);
Curve l4 = Line.CreateBound(p5, p1);

curves.Add(l1);
curves.Add(l2);
curves.Add(l3);
curves.Add(l4);


Line.CreateBound: Tạo đường thẳng từ hai điểm.  
Arc.Create: Tạo đường cong từ điểm đầu, điểm cuối, và điểm giữa.  
curves.Add: Thêm các Curve vào danh sách.

Hành động trên màn hình:  

Nhập mã tạo List và các Curve, highlight Line.CreateBound và Arc.Create.  
Chèn sơ đồ minh họa các đường: ba đường thẳng và một đường cong nối p1-p2, p1-p4 (arc), p4-p5, p5-p1.  
Chèn text: “Curve: Đường thẳng và cong để định hình tường.”


Bước 6: Tạo tường với TransactionThêm khối try-catch với Transaction để tạo các tường từ danh sách curves:
try
{
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


Wall.Create: Tạo tường với Curve, Level ID, và non-structural (false).  
foreach: Lặp qua danh sách curves để tạo từng tường.

Mã hoàn chỉnh trong phương thức Execute:
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

            try
            {
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

Hành động trên màn hình:  

Nhập khối try-catch và Transaction, highlight Wall.Create và vòng foreach.  
Chèn text: “Transaction: Tạo tường từ các Curve.”




Phần 3: Đăng ký và kiểm tra lệnh
Hướng dẫn viên (giọng điệu khích lệ):Lệnh PlaceLineElement đã được thêm vào tệp manifest với tên “PlaceLineElement”. Hãy kiểm tra nó trong Revit!

Bước 7: Chạy Debug và kiểm traTrong Visual Studio, nhấn F5 để chạy debug mode. Mở Revit, mở tệp bài tập của video này. Chuyển đến Add-Ins > External Tools > PlaceLineElement. Bạn sẽ thấy bốn tường được tạo, hình thành một hình khép kín với ba đường thẳng và một đường cong trên Level “Ground Floor”.
Hành động trên màn hình:  

Hiển thị Visual Studio, nhấn F5.  
Trong Revit, mở tệp bài tập, chạy PlaceLineElement.  
Zoom vào các tường trong viewport, highlight ba tường thẳng và một tường cong.  
Chèn text: “Kiểm tra: Tạo tường dựa trên Line và Arc.”




Phần 4: Kết luận và bước tiếp theo
Hướng dẫn viên (giọng điệu truyền cảm hứng):Chúc mừng các bạn! Hôm nay, chúng ta đã:

Tìm hiểu Wall.Create và các loại Curve (Line, Arc) trong Object Browser.  
Tạo các Curve sử dụng XYZ điểm và danh sách List.  
Sử dụng Transaction để tạo các tường dựa trên đường trong lệnh PlaceLineElement.

Lưu ý: Các Curve được tạo ngoài Transaction vì chúng không cần lưu vào mô hình, chỉ dùng để định hình tường. Kỹ thuật này rất hữu ích cho các phần tử dựa trên đường khác trong Revit.
Bước 8: Kêu gọi hành độngHãy thử thay đổi tọa độ XYZ hoặc tạo thêm Curve để vẽ các hình khác. Thêm try-catch quanh FilteredElementCollector để xử lý trường hợp không tìm thấy Level. Nếu bạn gặp vấn đề, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay!
Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!
Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"Bài học tiếp theo: Tạo phần tử dựa trên đường nâng cao."  
"Gặp vấn đề? Để lại câu hỏi trong phần bình luận!"


Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio, mô phỏng các tường trong mô hình.

