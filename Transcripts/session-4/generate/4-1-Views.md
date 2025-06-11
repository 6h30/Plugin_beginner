Kịch bản bài học: Tạo và chỉnh sửa View trong Revit
Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ khám phá cách tạo và hiển thị các view trong Revit API, cụ thể là tự động tạo một ViewPlan cho tầng trệt. Chúng ta sẽ tạo một lệnh PlanView để lấy ViewFamilyType, tạo view, và đặt tên cho nó. Sau bài học này, bạn sẽ biết:

Các loại view trong Revit API (View3D, ViewPlan, ViewSection, v.v.).
Cách lấy ViewFamilyType bằng FilteredElementCollector.
Cách tạo ViewPlan và chỉnh sửa thuộc tính (như tên).

Hãy cùng bắt đầu!

Phần 1: Tìm hiểu về View trong Revit API
Hướng dẫn viên (giọng điệu hào hứng):Trong các bài trước, chúng ta đã tạo và chỉnh sửa phần tử trong Revit. Hôm nay, chúng ta sẽ học cách hiển thị và chú thích phần tử bằng cách tạo các view tự động. Hãy cùng khám phá các loại view trong Revit API!

Bước 1: Các loại View trong Revit APIRevit API cung cấp nhiều loại view, tất cả đều kế thừa từ lớp View:

View3D: View 3D (isometric hoặc perspective).
ViewPlan: View mặt bằng (floor plan, ceiling plan).
ViewSection: View mặt cắt (section, elevation).
ViewDrafting: View phác thảo (drafting view).
ViewSheet: Tờ bản vẽ (sheet).

Mỗi view có các thuộc tính để phân biệt:

ViewType: Enum xác định loại view (ví dụ: FloorPlan, Elevation, Detail).
ViewFamilyType: Kiểu gia đình của view, lấy bằng GetTypeId.

Hành động trên màn hình:  

Hiển thị sơ đồ:  View
├── View3D
├── ViewPlan
├── ViewSection
├── ViewDrafting
└── ViewSheet
Properties:
├── ViewType: FloorPlan, Elevation...
└── ViewFamilyType: GetTypeId()


Chèn text: “View Classes: Các loại view trong Revit API.”


Bước 2: Tạo ViewPlanĐể tạo một ViewPlan, sử dụng phương thức tĩnh ViewPlan.Create với các tham số:

Document: Tài liệu Revit.
ViewFamilyTypeId: ID của ViewFamilyType (kiểu mặt bằng).
LevelId: ID của tầng (level) để tạo view.

Kết quả trả về là một ViewPlan có thể chỉnh sửa thuộc tính, như Name.
Hành động trên màn hình:  

Hiển thị mã giả lập:  ViewPlan vPlan = ViewPlan.Create(doc, viewFamilyType.Id, level.Id);
vPlan.Name = "New Floor Plan";


Chèn text: “ViewPlan.Create: Tạo view mặt bằng.”




Phần 2: Tạo lệnh PlanView trong Visual Studio
Hướng dẫn viên (giọng điệu rõ ràng):Hãy mở Visual Studio để tạo lệnh PlanView, tự động tạo một ViewPlan cho tầng trệt, sử dụng ViewFamilyType phù hợp và đặt tên cho view.

Bước 3: Mở Object Browser để xem ViewPlanTrong Visual Studio, mở Object Browser (đảm bảo RevitAPI.dll được tham chiếu). Tìm “ViewPlan” để xem phương thức Create:

Highlight ViewPlan.Create(Document, ElementId, ElementId).
Cho thấy tham số yêu cầu ViewFamilyTypeId và LevelId.

Hành động trên màn hình:  

Hiển thị Solution Explorer, nhấp đúp vào RevitAPI để mở Object Browser.  
Nhập “ViewPlan”, highlight Create.  
Chèn text: “ViewPlan.Create: Tạo ViewPlan tự động.”


Bước 4: Mở tệp PlanView.csMở tệp PlanView.cs trong dự án MyRevitCommands. Mã cơ bản đã được chuẩn bị, lấy Document và Level (Ground Floor), với Transaction:
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class PlanView : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                // Lấy level "Ground Floor"
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                Level level = collector.OfClass(typeof(Level))
                    .Cast<Level>()
                    .FirstOrDefault(l => l.Name == "Ground Floor");

                if (level != null)
                {
                    using (Transaction trans = new Transaction(doc, "Create View Plan"))
                    {
                        trans.Start();
                        // Tạo ViewPlan tại đây
                        trans.Commit();
                    }
                }
                else
                {
                    TaskDialog.Show("Error", "Ground Floor level not found.");
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

Hiển thị Solution Explorer, nhấp vào PlanView.cs.  
Zoom vào khối if (level != null) và Transaction, highlight level.  
Chèn text: “PlanView: Điểm khởi đầu để tạo ViewPlan.”


Bước 5: Lấy ViewFamilyType cho FloorPlanSử dụng FilteredElementCollector để tìm ViewFamilyType có ViewFamily là FloorPlan:
ViewFamilyType viewFamily = new FilteredElementCollector(doc)
    .OfClass(typeof(ViewFamilyType))
    .Cast<ViewFamilyType>()
    .First(x => x.ViewFamily == ViewFamily.FloorPlan);


OfClass(typeof(ViewFamilyType)): Lọc các ViewFamilyType.
Cast(): Ép kiểu kết quả.
First(x => x.ViewFamily == ViewFamily.FloorPlan): Lấy ViewFamilyType đầu tiên có ViewFamily là FloorPlan.

Hành động trên màn hình:  

Nhập mã FilteredElementCollector, highlight ViewFamily.FloorPlan.  
Chèn text: “ViewFamilyType: Lấy kiểu mặt bằng.”


Bước 6: Tạo ViewPlan và đặt tênTrong Transaction, tạo ViewPlan và đặt tên:
ViewPlan vPlan = ViewPlan.Create(doc, viewFamily.Id, level.Id);
vPlan.Name = "Our first plan!";


ViewPlan.Create: Tạo view mặt bằng với Document, ViewFamilyTypeId, và LevelId.
vPlan.Name: Đặt tên cho view.

Mã hoàn chỉnh trong phương thức Execute:
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Linq;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class PlanView : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                // Lấy level "Ground Floor"
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                Level level = collector.OfClass(typeof(Level))
                    .Cast<Level>()
                    .FirstOrDefault(l => l.Name == "Ground Floor");

                if (level != null)
                {
                    // Lấy ViewFamilyType cho FloorPlan
                    ViewFamilyType viewFamily = new FilteredElementCollector(doc)
                        .OfClass(typeof(ViewFamilyType))
                        .Cast<ViewFamilyType>()
                        .First(x => x.ViewFamily == ViewFamily.FloorPlan);

                    using (Transaction trans = new Transaction(doc, "Create View Plan"))
                    {
                        trans.Start();
                        ViewPlan vPlan = ViewPlan.Create(doc, viewFamily.Id, level.Id);
                        vPlan.Name = "Our first plan!";
                        trans.Commit();
                    }
                }
                else
                {
                    TaskDialog.Show("Error", "Ground Floor level not found.");
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

Nhập mã ViewPlan.Create và vPlan.Name, highlight "Our first plan!".  
Thêm using System.Linq; cho First.  
Chèn text: “ViewPlan: Tạo và đặt tên view.”




Phần 3: Đăng ký và kiểm tra lệnh
Hướng dẫn viên (giọng điệu khích lệ):Lệnh PlanView đã được thêm vào tệp manifest với tên “PlanView”. Hãy kiểm tra nó trong Revit!

Bước 7: Chạy Debug và kiểm traTrong Visual Studio, nhấn F5 để chạy debug mode. Mở Revit, mở tệp bài tập của video này. Chạy Add-Ins > External Tools > PlanView. Kết quả: một ViewPlan mới tên “Our first plan!” sẽ xuất hiện trong Project Browser, dựa trên tầng trệt.
Hành động trên màn hình:  

Hiển thị Visual Studio, nhấn F5.  
Trong Revit, mở tệp bài tập, chạy PlanView.  
Mở Project Browser, highlight view “Our first plan!”.  
Chèn text: “Kiểm tra: Tạo ViewPlan mới.”




Phần 4: Kết luận và bước tiếp theo
Hướng dẫn viên (giọng điệu truyền cảm hứng):Chúc mừng các bạn! Hôm nay, chúng ta đã:

Tìm hiểu các loại view trong Revit API và thuộc tính của chúng.  
Tạo lệnh PlanView để tự động tạo ViewPlan và đặt tên.  
Sử dụng FilteredElementCollector để lấy ViewFamilyType.

Tạo view tự động rất hữu ích để xử lý hàng loạt, như tạo nhiều mặt bằng cho các tầng khác nhau.
Bước 8: Kêu gọi hành độngHãy thử tạo ViewPlan cho tầng khác hoặc thay đổi tên view động (ví dụ: dựa trên tên tầng). Hoặc khám phá View3D.CreateIsometric để tạo view 3D. Nếu bạn gặp vấn đề, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay!
Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!
Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"Bài học tiếp theo: Tạo và chú thích Sheet."  
"Gặp vấn đề? Để lại câu hỏi trong phần bình luận!"


Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio, mô phỏng Project Browser với view mới.

