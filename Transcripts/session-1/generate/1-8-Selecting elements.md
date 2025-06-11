Kịch bản bài học: Chọn phần tử và lấy Element ID trong Revit
Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Hôm nay, chúng ta sẽ hoàn thiện lệnh GetElementId bằng cách sử dụng UIDocument để yêu cầu người dùng chọn một phần tử và hiển thị Element ID của nó. Sau bài học này, bạn sẽ biết:

Cách sử dụng phương thức PickObject để chọn phần tử trong Revit.
Cách hiển thị Element ID bằng TaskDialog.
Cách xử lý lỗi (như người dùng thoát lệnh) bằng try-catch.

Hãy cùng bắt đầu!

Phần 1: Sử dụng UIDocument để chọn phần tử
Hướng dẫn viên (giọng điệu hào hứng):Trong bài trước, chúng ta đã truy cập UIDocument trong lệnh GetElementId. Bây giờ, chúng ta sẽ dùng nó để yêu cầu người dùng chọn một phần tử trong Revit và lấy Element ID. Nhiều lệnh trong Revit bắt đầu bằng cách yêu cầu người dùng chọn một phần tử, và UIDocument cung cấp các phương thức giao diện người dùng để làm việc này.

Bước 1: Sử dụng phương thức PickObjectMở tệp GetElementId.cs trong Visual Studio. Trong phương thức Execute, sau dòng lấy UIDocument, chúng ta sẽ sử dụng phương thức PickObject từ lớp Selection của UIDocument để chọn một phần tử. Thêm dòng mã sau:
Reference pickedObj = uidoc.Selection.PickObject(ObjectType.Element);

Phương thức PickObject yêu cầu người dùng chọn một phần tử và trả về một Reference, là tham chiếu ổn định đến một đối tượng hình học trong Revit. Tham số ObjectType.Element đảm bảo chúng ta có thể chọn bất kỳ phần tử Revit nào.
Hành động trên màn hình:  

Hiển thị Visual Studio, zoom vào phương thức Execute trong GetElementId.cs.  
Nhập dòng Reference pickedObj = uidoc.Selection.PickObject(ObjectType.Element);.  
Hiển thị IntelliSense gợi ý PickObject và các tùy chọn ObjectType, highlight Element.  
Chèn text: “PickObject: Yêu cầu người dùng chọn một phần tử.”




Phần 2: Hiển thị Element ID bằng TaskDialog
Hướng dẫn viên (giọng điệu rõ ràng):Bây giờ chúng ta đã có tham chiếu đến phần tử được chọn, hãy hiển thị Element ID của nó bằng TaskDialog – một cửa sổ thông báo của Revit API, đảm bảo giao diện giống các hộp thoại Revit khác.

Bước 2: Kiểm tra xem phần tử có được chọn khôngTrước tiên, chúng ta cần kiểm tra xem người dùng có chọn phần tử hay không, vì nếu không, biến pickedObj sẽ là null và gây lỗi. Thêm câu lệnh if để kiểm tra:
if (pickedObj != null)
{
    TaskDialog.Show("Element ID", pickedObj.ElementId.ToString());
}


TaskDialog.Show: Tạo một hộp thoại với tiêu đề (“Element ID”) và nội dung (chuỗi Element ID).  
pickedObj.ElementId.ToString(): Lấy Element ID từ Reference và chuyển thành chuỗi để hiển thị.

Hành động trên màn hình:  

Hiển thị Visual Studio, nhập câu lệnh if và TaskDialog.Show trong phương thức Execute.  
Zoom vào pickedObj.ElementId.ToString().  
Chèn text: “TaskDialog: Hiển thị Element ID trong hộp thoại Revit.”




Phần 3: Xử lý lỗi với Try-Catch
Hướng dẫn viên (giọng điệu cẩn thận):Để đảm bảo lệnh không bị lỗi khi người dùng thoát (nhấn Esc) hoặc gặp vấn đề khác, chúng ta sẽ bọc mã trong một khối try-catch. Điều này cũng cho phép chúng ta sử dụng tham số message để gửi thông báo lỗi về cho người dùng.

Bước 3: Thêm khối Try-CatchBọc toàn bộ mã trong phương thức Execute (sau dòng khai báo UIDocument) trong khối try, và thêm khối catch để xử lý lỗi. Mã hoàn chỉnh sẽ như sau:
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.ReadOnly)]
    public class GetElementId : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            try
            {
                Reference pickedObj = uidoc.Selection.PickObject(ObjectType.Element);
                if (pickedObj != null)
                {
                    TaskDialog.Show("Element ID", pickedObj.ElementId.ToString());
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


try: Chứa mã chính của lệnh (chọn phần tử và hiển thị Element ID).  
catch: Bắt lỗi (như người dùng nhấn Esc) và gán thông báo lỗi vào tham số message, trả về Result.Failed để báo cho Revit rằng lệnh thất bại.

Hành động trên màn hình:  

Hiển thị Visual Studio, bọc mã trong khối try và thêm khối catch.  
Highlight dòng message = e.Message; và return Result.Failed;.  
Chèn text: “Try-Catch: Xử lý lỗi, như khi người dùng thoát lệnh.”




Phần 4: Kết luận và bước tiếp theo
Hướng dẫn viên (giọng điệu truyền cảm hứng):Chúc mừng các bạn! Hôm nay, chúng ta đã:

Sử dụng UIDocument.Selection.PickObject để yêu cầu người dùng chọn một phần tử.  
Hiển thị Element ID bằng TaskDialog.  
Thêm khối try-catch để xử lý lỗi và gửi thông báo cho người dùng.

Lệnh GetElementId của chúng ta giờ đã hoạt động! Bước tiếp theo là biên dịch lệnh và tải nó vào Revit để kiểm tra.
Bước 4: Kêu gọi hành độngTrong bài học tiếp theo, chúng ta sẽ biên dịch dự án và tải plugin vào Revit để thấy lệnh hoạt động thực tế. Hãy giữ Visual Studio và Revit sẵn sàng! Nếu bạn gặp vấn đề, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay.
Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!
Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"Bài học tiếp theo: Biên dịch và tải plugin vào Revit."  
"Gặp vấn đề? Để lại câu hỏi trong phần bình luận!"


Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio, mô phỏng hộp thoại TaskDialog hiển thị Element ID.

