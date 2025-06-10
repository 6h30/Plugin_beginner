Kịch bản bài học: Tạo FilteredElementCollector để lọc cửa sổ trong Revit
Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ áp dụng FilteredElementCollector để tạo một lệnh mới, CollectWindows, lọc tất cả các cửa sổ trong dự án và hiển thị số lượng. Sau bài học này, bạn sẽ biết:

Cách tạo FilteredElementCollector để tìm phần tử trong toàn bộ tài liệu.
Cách sử dụng ElementCategoryFilter và phương thức tắt WhereElementIsNotElementType.
Cách đăng ký lệnh mới trong tệp manifest và kiểm tra trong Revit.

Hãy cùng bắt đầu!

Phần 1: Tạo FilteredElementCollector trong Visual Studio
Hướng dẫn viên (giọng điệu hào hứng):Chúng ta đã học về FilteredElementCollector trong bài trước. Bây giờ, hãy tạo một lệnh CollectWindows để lọc tất cả các cửa sổ trong dự án Revit. Tôi đã chuẩn bị một tệp bài tập với lệnh CollectWindows cơ bản, chỉ lấy UIDocument và Document. Hãy cùng mở nó và bắt đầu!

Bước 1: Mở tệp CollectWindows.csTrong Visual Studio, mở tệp CollectWindows.cs từ dự án MyRevitCommands. Nếu bạn chưa có, hãy tìm trong thư mục bài tập của video này. Mã hiện tại như sau:
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.ReadOnly)]
    public class CollectWindows : IExternalCommand
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

Hiển thị Solution Explorer, nhấp vào CollectWindows.cs.  
Zoom vào phương thức Execute, highlight dòng lấy UIDocument và Document.  
Chèn text: “CollectWindows: Điểm bắt đầu để lọc cửa sổ.”


Bước 2: Tạo FilteredElementCollectorTrong phương thức Execute, sau dòng lấy Document, thêm mã để tạo FilteredElementCollector tìm trong toàn bộ tài liệu:
FilteredElementCollector collector = new FilteredElementCollector(doc);

Hành động trên màn hình:  

Nhập dòng FilteredElementCollector collector = new FilteredElementCollector(doc);.  
Chèn text: “FilteredElementCollector: Tìm tất cả phần tử trong Document.”




Phần 2: Áp dụng Quick Filters
Hướng dẫn viên (giọng điệu rõ ràng):Để lọc các cửa sổ, chúng ta cần áp dụng bộ lọc. Hãy khám phá các Quick Filters trong Object Browser để chọn ElementCategoryFilter phù hợp.

Bước 3: Khám phá ElementQuickFilter trong Object BrowserTrong Visual Studio, mở Object Browser bằng cách nhấp đúp vào tham chiếu RevitAPI. Nếu bạn chỉ thấy các lớp cơ sở của ElementQuickFilter, cần mở rộng phạm vi tìm kiếm:

Trong Object Browser, chọn Browse > Custom Component Set.  
Nhấp vào nút ba chấm, điều hướng đến thư mục cài đặt Revit (thường là C:\Program Files\Autodesk\Revit 2019).  
Giữ Ctrl và chọn RevitAPI.dll và RevitAPIUI.dll, nhấn OK.  
Tìm lại ElementQuickFilter, mở rộng để xem các lớp kế thừa, như ElementCategoryFilter.

Hành động trên màn hình:  

Hiển thị Object Browser, vào Browse > Custom Component Set, thêm RevitAPI.dll và RevitAPIUI.dll.  
Tìm “ElementQuickFilter”, mở rộng để highlight ElementCategoryFilter.  
Chèn text: “ElementCategoryFilter: Bộ lọc nhanh cho danh mục, như Windows.”


Bước 4: Tạo ElementCategoryFilter cho WindowsTrong CollectWindows.cs, thêm mã để tạo ElementCategoryFilter cho danh mục Windows (sử dụng BuiltInCategory.OST_Windows):
ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Windows);

Áp dụng bộ lọc này bằng phương thức WherePasses:
collector.WherePasses(filter);

Hành động trên màn hình:  

Nhập ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Windows); và collector.WherePasses(filter);.  
Highlight BuiltInCategory.OST_Windows trong IntelliSense.  
Chèn text: “ElementCategoryFilter: Lọc phần tử thuộc danh mục Windows.”


Bước 5: Thêm phương thức tắt WhereElementIsNotElementTypeĐể tăng tốc độ lọc, thêm phương thức tắt WhereElementIsNotElementType để chỉ lấy các instance (bỏ qua Element Types):
collector.WhereElementIsNotElementType();

Hành động trên màn hình:  

Nhập collector.WhereElementIsNotElementType();.  
Chèn text: “WhereElementIsNotElementType: Chỉ lấy instance, bỏ qua types.”




Phần 3: Truy xuất và hiển thị kết quả
Hướng dẫn viên (giọng điệu khích lệ):Bây giờ, chúng ta sẽ lấy các phần tử đã lọc và hiển thị số lượng cửa sổ bằng TaskDialog.

Bước 6: Lấy phần tử với ToElementsSử dụng phương thức ToElements để lấy danh sách các cửa sổ và lưu vào một IList:
IList<Element> windows = collector.ToElements();

Hành động trên màn hình:  

Nhập IList<Element> windows = collector.ToElements();.  
Di chuột lên ToElements để hiển thị tooltip trả về IList.  
Chèn text: “ToElements: Lấy danh sách phần tử đã lọc.”


Bước 7: Hiển thị số lượng cửa sổ với TaskDialogSử dụng TaskDialog để hiển thị số lượng cửa sổ, định dạng chuỗi bằng string.Format:
TaskDialog.Show("Windows", string.Format("{0} windows counted.", windows.Count));

Mã hoàn chỉnh trong phương thức Execute sẽ như sau:
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Collections.Generic; // Thêm namespace cho IList

namespace MyRevitCommands
{
    [Transaction(TransactionMode.ReadOnly)]
    public class CollectWindows : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Windows);
            collector.WherePasses(filter);
            collector.WhereElementIsNotElementType();

            IList<Element> windows = collector.ToElements();
            TaskDialog.Show("Windows", string.Format("{0} windows counted.", windows.Count));

            return Result.Succeeded;
        }
    }
}

Hành động trên màn hình:  

Thêm dòng using System.Collections.Generic; ở đầu tệp.  
Nhập TaskDialog và string.Format, highlight {0} và windows.Count.  
Chèn text: “TaskDialog: Hiển thị số lượng cửa sổ.”




Phần 4: Đăng ký lệnh trong Manifest
Hướng dẫn viên (giọng điệu cẩn thận):Để chạy lệnh CollectWindows trong Revit, chúng ta cần đăng ký nó trong tệp manifest.

Bước 8: Cập nhật MyRevitCommands.addinMở tệp MyRevitCommands.addin trong Solution Explorer. Sao chép khối  của lệnh GetElementId, dán để tạo một khối mới, và cập nhật:

AddInId: Tạo GUID mới bằng Tools > Create GUID, chọn Registry Format, nhấn Copy, dán và xóa {}.
FullClassName: Đổi thành MyRevitCommands.CollectWindows.
Name và Text: Đổi thành CollectWindows.
Description: Đổi thành “Counts all windows in the project”.

Ví dụ khối  mới:
<AddIn Type="Command">
    <Name>CollectWindows</Name>
    <Assembly>MyRevitCommands.dll</Assembly>
    <AddInId>123e4567-e89b-12d3-a456-426614174000</AddInId>
    <FullClassName>MyRevitCommands.CollectWindows</FullClassName>
    <Text>CollectWindows</Text>
    <Description>Counts all windows in the project</Description>
    <VendorId>Jeremy</VendorId>
    <VisibilityMode>NotVisibleWhenNoActiveDocument</VisibilityMode>
</AddIn>

Hành động trên màn hình:  

Hiển thị MyRevitCommands.addin, sao chép khối , dán và chỉnh sửa.  
Vào Tools > Create GUID, sao chép GUID mới, dán vào AddInId.  
Highlight các thay đổi: FullClassName, Name, Description.  
Chèn text: “Manifest: Đăng ký lệnh CollectWindows.”




Phần 5: Kiểm tra lệnh trong Revit
Hướng dẫn viên (giọng điệu truyền cảm hứng):Hãy kiểm tra lệnh CollectWindows trong Revit!

Bước 9: Chạy Debug và kiểm traTrong Visual Studio, nhấn F5 để chạy debug mode. Mở Revit, mở dự án mẫu (Sample Project), chuyển đến Level 1. Trong tab Add-Ins > External Tools, chọn CollectWindows. TaskDialog sẽ hiển thị số lượng cửa sổ, ví dụ: “17 windows counted.”
Hành động trên màn hình:  

Hiển thị Visual Studio, nhấn F5.  
Trong Revit, mở dự án mẫu, điều hướng đến Level 1, chạy CollectWindows.  
Hiển thị TaskDialog với thông báo “17 windows counted.”  
Chèn text: “Kiểm tra: Đếm số cửa sổ trong dự án.”




Phần 6: Kết luận và bước tiếp theo
Hướng dẫn viên (giọng điệu truyền cảm hứng):Chúc mừng các bạn! Hôm nay, chúng ta đã:

Tạo FilteredElementCollector để lọc cửa sổ trong toàn bộ tài liệu.  
Áp dụng ElementCategoryFilter và WhereElementIsNotElementType để thu hẹp kết quả.  
Đăng ký và kiểm tra lệnh CollectWindows trong Revit.

FilteredElementCollector là một công cụ rất mạnh mẽ, và chúng ta sẽ sử dụng nó nhiều trong khóa học để lọc các phần tử khác nhau.
Bước 10: Kêu gọi hành độngHãy thử thay đổi lệnh CollectWindows để lọc các phần tử khác, như tường (OST_Walls) hoặc cửa (OST_Doors). Thêm breakpoint để kiểm tra danh sách windows. Nếu bạn gặp vấn đề, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay!
Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!
Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"Bài học tiếp theo: Lọc phần tử nâng cao với FilteredElementCollector."  
"Gặp vấn đề? Để lại câu hỏi trong phần bình luận!"


Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio, mô phỏng TaskDialog hiển thị số lượng cửa sổ.

