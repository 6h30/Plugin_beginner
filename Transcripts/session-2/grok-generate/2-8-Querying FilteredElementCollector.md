Kịch bản bài học: Tối ưu hóa FilteredElementCollector với LINQ và Lambda Expressions
Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ cải tiến lệnh PlaceFamily bằng cách sử dụng các phương thức mở rộng LINQ và biểu thức lambda để làm cho FilteredElementCollector ngắn gọn và dễ đọc hơn. Sau bài học này, bạn sẽ biết:

LINQ và các phương thức mở rộng như Cast và First là gì.
Cách sử dụng biểu thức lambda để lọc dữ liệu trực tiếp trong FilteredElementCollector.
Cách tối ưu hóa mã để tìm FamilySymbol mà không cần vòng lặp foreach.

Hãy cùng bắt đầu!

Phần 1: Giới thiệu LINQ và Lambda Expressions
Hướng dẫn viên (giọng điệu hào hứng):Trong bài trước, chúng ta đã sử dụng FilteredElementCollector với vòng lặp foreach để tìm FamilySymbol. Hôm nay, chúng ta sẽ học cách sử dụng LINQ và biểu thức lambda để làm điều này một cách ngắn gọn và hiệu quả hơn!

Bước 1: Hiểu về LINQLINQ (Language-Integrated Query) là một tính năng của C# cho phép truy vấn dữ liệu từ các nguồn như danh sách hoặc FilteredElementCollector. LINQ sử dụng các toán tử như from, where, và select, ví dụ:
from x in dataSource where x == someValue select x;

Trong bài này, chúng ta sẽ dùng phương thức mở rộng LINQ (extension methods) để thêm chức năng truy vấn vào các kiểu IEnumerable, như FilteredElementCollector.
Hành động trên màn hình:  

Hiển thị sơ đồ đơn giản:  LINQ
├── Truy vấn: from, where, select
└── Extension Methods: Cast, First, Select


Chèn text: “LINQ: Truy vấn dữ liệu trong C#.”


Bước 2: Hiểu về phương thức mở rộng LINQMột số phương thức mở rộng LINQ phổ biến:

Cast: Ép kiểu tất cả phần tử trong danh sách thành kiểu T.  
First: Lấy phần tử đầu tiên thỏa mãn điều kiện.  
Select: Chọn các phần tử dựa trên một hàm.

Các phương thức này nên được dùng sau các bộ lọc Quick hoặc Slow của FilteredElementCollector, vì bộ lọc gốc nhanh hơn.
Hành động trên màn hình:  

Hiển thị mã giả lập:  var result = list.Cast<Type>().First(x => x.Name == "value");


Chèn text: “Extension Methods: Tăng khả năng lọc cho FilteredElementCollector.”


Bước 3: Hiểu về Lambda ExpressionsBiểu thức lambda là các hàm vô danh (không có tên), được viết inline với cú pháp:x => x.SomeProperty == value  

x: Tham số của hàm.  
=>: Toán tử lambda.  
x.SomeProperty == value: Hàm kiểm tra điều kiện.

Biểu thức lambda thường được dùng trong các phương thức LINQ như First.
Hành động trên màn hình:  

Hiển thị mã giả lập:  list.First(x => x == 2); // Trả về phần tử đầu tiên bằng 2


Chèn text: “Lambda: Hàm inline để lọc dữ liệu.”




Phần 2: Cải tiến lệnh PlaceFamily trong Visual Studio
Hướng dẫn viên (giọng điệu rõ ràng):Hãy quay lại Visual Studio để cải tiến lệnh PlaceFamily từ bài trước. Chúng ta sẽ thay vòng foreach bằng LINQ và biểu thức lambda để tìm FamilySymbol một cách ngắn gọn hơn.

Bước 4: Mở tệp PlaceFamily.csMở tệp PlaceFamily.cs trong dự án MyRevitCommands. Mã hiện tại sử dụng foreach để tìm FamilySymbol:
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

Hiển thị Solution Explorer, nhấp vào PlaceFamily.cs.  
Zoom vào vòng foreach, highlight điều kiện ele.Name == "1,525 by 762 millimeters".  
Chèn text: “foreach: Sẽ được thay bằng LINQ và Lambda.”


Bước 5: Thay foreach bằng LINQ và LambdaXóa vòng foreach và cải tiến FilteredElementCollector bằng các phương thức LINQ:

Xóa ToElements() và di chuyển WhereElementIsElementType() xuống dòng mới.
Thêm Cast() để ép kiểu các phần tử thành FamilySymbol.
Thêm First(x => x.Name == "1,525 by 762 millimeters") để lấy FamilySymbol đầu tiên có tên khớp.
Thay IList symbols bằng FamilySymbol symbol, vì chỉ cần một phần tử.

Mã được cải tiến:
FamilySymbol symbol = collector.OfClass(typeof(FamilySymbol))
                               .WhereElementIsElementType()
                               .Cast<FamilySymbol>()
                               .First(x => x.Name == "1,525 by 762 millimeters");

Thêm namespace System.Linq để sử dụng LINQ:
using System.Linq;

Mã hoàn chỉnh trong phương thức Execute:
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Linq;

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
            FamilySymbol symbol = collector.OfClass(typeof(FamilySymbol))
                                           .WhereElementIsElementType()
                                           .Cast<FamilySymbol>()
                                           .First(x => x.Name == "1,525 by 762 millimeters");

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

Xóa vòng foreach và IList symbols.  
Nhập Cast() và First(x => x.Name == "1,525 by 762 millimeters"), highlight x => x.Name.  
Thêm dòng using System.Linq; ở đầu tệp.  
Chèn text: “LINQ & Lambda: Lọc FamilySymbol trực tiếp, ngắn gọn hơn.”




Phần 3: Kiểm tra lệnh trong Revit
Hướng dẫn viên (giọng điệu khích lệ):Hãy kiểm tra xem lệnh PlaceFamily được cải tiến có hoạt động đúng không!

Bước 6: Chạy Debug và kiểm traTrong Visual Studio, nhấn F5 để chạy debug mode. Mở Revit, mở tệp bài tập của video này. Chuyển đến Add-Ins > External Tools > Place Family. Bàn làm việc kích thước 1,525 x 762 mm sẽ được đặt tại tọa độ (0, 0, 0), giống như bài trước, nhưng mã ngắn gọn hơn nhờ LINQ.
Hành động trên màn hình:  

Hiển thị Visual Studio, nhấn F5.  
Trong Revit, mở tệp bài tập, chạy Place Family.  
Zoom vào bàn làm việc tại (0, 0, 0), highlight trong viewport.  
Chèn text: “Kiểm tra: Đặt bàn làm việc với mã tối ưu.”




Phần 4: Kết luận và bước tiếp theo
Hướng dẫn viên (giọng điệu truyền cảm hứng):Chúc mừng các bạn! Hôm nay, chúng ta đã:

Tìm hiểu về LINQ, phương thức mở rộng, và biểu thức lambda.  
Cải tiến FilteredElementCollector trong lệnh PlaceFamily bằng Cast và First với lambda.  
Kiểm tra lệnh để đảm bảo mã tối ưu vẫn hoạt động đúng.

LINQ và lambda expressions giúp mã ngắn gọn và dễ đọc hơn. Chúng ta sẽ tiếp tục áp dụng chúng trong các bài học sau để lọc phần tử hiệu quả hơn.
Bước 7: Kêu gọi hành độngHãy thử thay đổi biểu thức lambda để tìm FamilySymbol khác, như ghế hoặc cửa sổ. Thêm breakpoint để kiểm tra giá trị của symbol. Nếu bạn gặp vấn đề, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay!
Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!
Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"Bài học tiếp theo: Lọc và tạo phần tử nâng cao với LINQ."  
"Gặp vấn đề? Để lại câu hỏi trong phần bình luận!"


Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio, mô phỏng bàn làm việc trong mô hình.

