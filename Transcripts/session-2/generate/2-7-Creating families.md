Kịch bản bài học: Tạo Family Instance trong Revit
Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ học cách tạo một Family Instance từ đầu bằng cách đặt một bàn làm việc (desk) vào mô hình Revit. Sau bài học này, bạn sẽ biết:

Cách sử dụng FilteredElementCollector để tìm FamilySymbol theo tên.
Cách sử dụng phương thức NewFamilyInstance để tạo Family Instance dựa trên điểm.
Cách kích hoạt FamilySymbol và quản lý Transaction để đặt phần tử.

Hãy cùng bắt đầu!

Phần 1: Tìm hiểu phương thức NewFamilyInstance
Hướng dẫn viên (giọng điệu hào hứng):Chúng ta đã biết cách chỉnh sửa mô hình Revit với Transaction. Bây giờ, hãy học cách tạo một Family Instance, cụ thể là một bàn làm việc kích thước 1,525 x 762 mm, bằng cách sử dụng FilteredElementCollector để tìm FamilySymbol và NewFamilyInstance để đặt nó vào mô hình.

Bước 1: Khám phá NewFamilyInstance trong Object BrowserTrong Visual Studio, mở Object Browser bằng cách nhấp đúp vào tham chiếu RevitAPI. Tìm “NewFamilyInstance” trong ô tìm kiếm. Bạn sẽ thấy nhiều phiên bản của phương thức này, mỗi phiên bản dùng cho các trường hợp khác nhau. Vì chúng ta muốn đặt một Family Instance dựa trên điểm (point-based), chọn phiên bản với các tham số:

XYZ: Tọa độ điểm đặt (x, y, z).
FamilySymbol: Loại Family (Family Type).
StructuralType: Loại cấu trúc (ví dụ: NonStructural).

Phương thức này có thể được gọi từ Document.Create (thuộc Creation.Document) hoặc ItemFactoryBase.
Hành động trên màn hình:  

Hiển thị Solution Explorer, nhấp đúp vào RevitAPI để mở Object Browser.  
Nhập “NewFamilyInstance”, highlight phiên bản với tham số XYZ, FamilySymbol, StructuralType.  
Zoom vào Document.Create trong lớp Document, highlight Creation.Document.  
Chèn text: “NewFamilyInstance: Tạo Family Instance dựa trên điểm.”


Bước 2: Hiểu Creation.DocumentTrong Object Browser, tìm lớp Document trong namespace Autodesk.Revit.DB. Thuộc tính Create trả về một đối tượng Creation.Document, kế thừa từ ItemFactoryBase, dùng để tạo các phần tử như Family Instance.
Hành động trên màn hình:  

Trong Object Browser, tìm “Document”, highlight thuộc tính Create.  
Chèn text: “Creation.Document: Đối tượng để tạo Family Instance.”




Phần 2: Tạo lệnh PlaceFamily trong Visual Studio
Hướng dẫn viên (giọng điệu rõ ràng):Hãy mở Visual Studio để tạo lệnh PlaceFamily, tìm FamilySymbol của bàn làm việc và đặt nó vào tọa độ (0, 0, 0) trong mô hình Revit.

Bước 3: Mở tệp PlaceFamily.csMở tệp PlaceFamily.cs trong dự án MyRevitCommands. Mã cơ bản đã được chuẩn bị như sau:
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class PlaceFamily : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            return Result.Succeeded;
        }
    }
}

Hành động trên màn hình:  

Hiển thị Solution Explorer, nhấp vào PlaceFamily.cs.  
Zoom vào phương thức Execute, highlight UIDocument và Document.  
Chèn text: “PlaceFamily: Điểm bắt đầu để đặt bàn làm việc.”


Bước 4: Tìm FamilySymbol bằng FilteredElementCollectorThêm mã để sử dụng FilteredElementCollector tìm tất cả FamilySymbol và lọc ra bàn làm việc có tên “1,525 by 762 millimeters”:
FilteredElementCollector collector = new FilteredElementCollector(doc);
IList<Element> symbols = collector.OfClass(typeof(FamilySymbol)).WhereElementIsElementType().ToElements();

FamilySymbol symbol = null;
foreach (Element ele in symbols)
{
    if (ele.Name == "1,525 by 762 millimeters")
    {
        symbol = ele as FamilySymbol;
        break;
    }
}


OfClass(typeof(FamilySymbol)): Lọc các phần tử thuộc lớp FamilySymbol.  
WhereElementIsElementType(): Chỉ lấy Family Types, không lấy Instances.  
foreach: Duyệt danh sách để tìm FamilySymbol có tên khớp.

Hành động trên màn hình:  

Nhập mã FilteredElementCollector, highlight typeof(FamilySymbol) và WhereElementIsElementType().  
Nhập vòng foreach, highlight điều kiện ele.Name == "1,525 by 762 millimeters".  
Chèn text: “FilteredElementCollector: Tìm FamilySymbol của bàn làm việc.”


Bước 5: Thêm Transaction và kiểm tra FamilySymbolThêm khối try-catch với Transaction để đặt Family Instance. Kiểm tra và kích hoạt FamilySymbol trước khi đặt:
try
{
    using (Transaction trans = new Transaction(doc, "Place Family"))
    {
        trans.Start();
        if (!symbol.IsActive)
        {
            symbol.Activate();
        }
        doc.Create.NewFamilyInstance(new XYZ(0, 0, 0), symbol, StructuralType.NonStructural);
        trans.Commit();
    }
}
catch (Exception e)
{
    message = e.Message;
    return Result.Failed;
}


!symbol.IsActive: Kiểm tra xem FamilySymbol có đang hoạt động không. Revit có thể tắt FamilySymbol để tiết kiệm bộ nhớ.  
symbol.Activate(): Kích hoạt FamilySymbol nếu cần.  
NewFamilyInstance: Đặt bàn làm việc tại tọa độ (0, 0, 0) với NonStructural.

Mã hoàn chỉnh trong phương thức Execute:
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Collections.Generic;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class PlaceFamily : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            IList<Element> symbols = collector.OfClass(typeof(FamilySymbol)).WhereElementIsElementType().ToElements();

            FamilySymbol symbol = null;
            foreach (Element ele in symbols)
            {
                if (ele.Name == "1,525 by 762 millimeters")
                {
                    symbol = ele as FamilySymbol;
                    break;
                }
            }

            try
            {
                using (Transaction trans = new Transaction(doc, "Place Family"))
                {
                    trans.Start();
                    if (!symbol.IsActive)
                    {
                        symbol.Activate();
                    }
                    doc.Create.NewFamilyInstance(new XYZ(0, 0, 0), symbol, StructuralType.NonStructural);
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

Nhập khối try-catch và Transaction, highlight !symbol.IsActive và symbol.Activate().  
Nhập NewFamilyInstance, highlight new XYZ(0, 0, 0) và StructuralType.NonStructural.  
Chèn text: “Transaction: Đặt Family Instance với FamilySymbol đã kích hoạt.”




Phần 3: Đăng ký và kiểm tra lệnh
Hướng dẫn viên (giọng điệu khích lệ):Lệnh PlaceFamily đã được thêm vào tệp manifest với tên “Place Family”. Hãy kiểm tra nó trong Revit!

Bước 6: Kiểm tra trong RevitTrong Visual Studio, nhấn F5 để chạy debug mode. Mở Revit, mở tệp bài tập của video này. Chuyển đến Add-Ins > External Tools > Place Family. Một bàn làm việc kích thước 1,525 x 762 mm sẽ được đặt tại tọa độ (0, 0, 0) trong mô hình.
Hành động trên màn hình:  

Hiển thị Visual Studio, nhấn F5.  
Trong Revit, mở tệp bài tập, chạy Place Family.  
Zoom vào bàn làm việc tại (0, 0, 0), highlight trong viewport.  
Chèn text: “Kiểm tra: Đặt bàn làm việc tại (0, 0, 0).”




Phần 4: Kết luận và bước tiếp theo
Hướng dẫn viên (giọng điệu truyền cảm hứng):Chúc mừng các bạn! Hôm nay, chúng ta đã:

Tìm FamilySymbol bằng FilteredElementCollector và kiểm tra tên.  
Sử dụng NewFamilyInstance để đặt một bàn làm việc tại tọa độ (0, 0, 0).  
Quản lý Transaction và kích hoạt FamilySymbol để tạo Family Instance.

Kỹ thuật này rất hữu ích khi bạn muốn tự động đặt nhiều Family Instance, như một dãy cột hoặc đồ nội thất. Trong bài học tiếp theo, chúng ta sẽ khám phá cách đặt các Family Instance khác hoặc tùy chỉnh vị trí.
Bước 7: Kêu gọi hành độngHãy thử thay đổi tọa độ trong new XYZ(0, 0, 0) để đặt bàn làm việc ở vị trí khác. Hoặc thử tìm một FamilySymbol khác, như ghế, bằng cách thay tên trong vòng foreach. Nếu bạn gặp vấn đề, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay!
Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!
Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"Bài học tiếp theo: Đặt Family Instance nâng cao."  
"Gặp vấn đề? Để lại câu hỏi trong phần bình luận!"


Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio, mô phỏng bàn làm việc trong mô hình.

