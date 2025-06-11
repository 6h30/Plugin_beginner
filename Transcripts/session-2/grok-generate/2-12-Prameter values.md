Kịch bản bài học: Lấy và đặt giá trị thông số trong Revit
Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ học cách lấy và đặt giá trị thông số của một phần tử, cụ thể là thông số Head Height của một cửa sổ. Sau bài học này, bạn sẽ biết:

Cách lấy thông số bằng BuiltInParameter để đảm bảo độ chính xác.
Cách kiểm tra StorageType và lấy giá trị thông số (ví dụ: AsDouble).
Cách đặt giá trị thông số bằng Set trong Transaction.

Hãy cùng bắt đầu!

Phần 1: Tìm hiểu về lấy và đặt giá trị Parameter
Hướng dẫn viên (giọng điệu hào hứng):Trong bài trước, chúng ta đã lấy thông tin thông số như Name và UnitType. Hôm nay, chúng ta sẽ tập trung vào việc lấy giá trị của thông số và cách thay đổi nó, sử dụng BuiltInParameter để truy xuất thông số an toàn hơn.

Bước 1: Lấy Parameter bằng BuiltInParameterThay vì dùng LookupParameter với tên chuỗi (có thể trùng lặp), ta sử dụng GetParameter(BuiltInParameter) để lấy thông số bằng định danh duy nhất của Revit, như INSTANCE_HEAD_HEIGHT_PARAM. Phương thức này là một indexed property, nên trong C# phải viết là Get_Parameter.
Hành động trên màn hình:  

Hiển thị mã giả lập:  Parameter param = element.Get_Parameter(BuiltInParameter.INSTANCE_HEAD_HEIGHT_PARAM);


Chèn text: “BuiltInParameter: Đảm bảo lấy đúng thông số duy nhất.”


Bước 2: Kiểm tra StorageType và lấy giá trịMỗi Parameter có StorageType xác định kiểu dữ liệu (ví dụ: Double, String, Integer, ElementId). Để lấy giá trị, sử dụng các phương thức phù hợp:

AsDouble: Lấy giá trị kiểu Double (ví dụ: chiều dài).
AsString: Lấy giá trị kiểu String.
AsInteger: Lấy giá trị kiểu Integer.

Nếu gọi sai phương thức (ví dụ: AsDouble trên một String), sẽ gây lỗi. Vì vậy, luôn kiểm tra StorageType trước.
Hành động trên màn hình:  

Hiển thị sơ đồ:  Parameter
├── StorageType: Double, String, Integer, ElementId
└── Methods: AsDouble, AsString, AsInteger, AsElementId


Chèn text: “StorageType: Xác định kiểu giá trị của Parameter.”


Bước 3: Đặt giá trị bằng SetĐể đặt giá trị, sử dụng phương thức Set của Parameter trong Transaction. Có hai phương thức:

Set(value): Đặt giá trị trực tiếp (theo đơn vị nội bộ, như feet).
SetValueString(string): Đặt giá trị dạng chuỗi (theo đơn vị hiển thị).

Trong bài này, chúng ta dùng Set với giá trị Double (7.0 feet).



Phần 2: Tạo lệnh SetParameter trong Visual Studio
Hướng dẫn viên (giọng điệu rõ ràng):Hãy mở Visual Studio để tạo lệnh SetParameter, lấy thông số Head Height, hiển thị StorageType và giá trị, sau đó đặt giá trị mới là 2.286 m (7.5 feet) cho cửa sổ.

Bước 4: Mở tệp SetParameter.csMở tệp SetParameter.cs trong dự án MyRevitCommands. Mã cơ bản tương tự GetParameter, đã bao gồm Transaction để đặt giá trị:
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class SetParameter : IExternalCommand
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
                    Element element = doc.GetElement(pickedObj);
                    using (Transaction trans = new Transaction(doc, "Set Parameter"))
                    {
                        trans.Start();
                        // Lấy và đặt Parameter tại đây
                        trans.Commit();
                    }
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

Hiển thị Solution Explorer, nhấp vào SetParameter.cs.  
Zoom vào khối Transaction, hiển thị trans.Start() và trans.Commit().  
Chèn text: “SetParameter: Điểm khởi đầu để lấy và đặt Head Height.”


Bước 5: Lấy Parameter bằng BuiltInParameterTrong khối Transaction, lấy thông số Head Height bằng Get_Parameter:
Parameter param = element.Get_Parameter(BuiltInParameter.INSTANCE_HEAD_HEIGHT_PARAM);

Hành động trên màn hình:  

Nhập dòng Get_Parameter, highlight INSTANCE_HEAD_HEIGHT_PARAM.  
Chèn text: “Get_Parameter: Lấy Head Height bằng BuiltInParameter.”


Bước 6: Hiển thị StorageType và giá trịSử dụng TaskDialog để hiển thị StorageType và giá trị hiện tại của thông số:
TaskDialog.Show("Parameter Values", string.Format("Storage Type: {0}\nValue: {1}", 
    param.StorageType.ToString(), param.AsDouble()));


StorageType.ToString(): Hiển thị kiểu dữ liệu (Double).
AsDouble(): Lấy giá trị (7.0 feet, vì Revit API dùng feet).

Hành động trên màn hình:  

Nhập mã TaskDialog, highlight param.StorageType và param.AsDouble().  
Chèn text: “TaskDialog: Hiển thị StorageType và giá trị hiện tại.”


Bước 7: Đặt giá trị mớiĐặt giá trị mới cho Head Height là 7.5 feet bằng Set:
param.Set(7.5);

Mã hoàn chỉnh trong phương thức Execute:
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class SetParameter : IExternalCommand
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
                    Element element = doc.GetElement(pickedObj);
                    using (Transaction trans = new Transaction(doc, "Set Parameter"))
                    {
                        trans.Start();
                        Parameter param = element.Get_Parameter(BuiltInParameter.INSTANCE_HEAD_HEIGHT_PARAM);
                        TaskDialog.Show("Parameter Values", string.Format("Storage Type: {0}\nValue: {1}", 
                            param.StorageType.ToString(), param.AsDouble()));
                        param.Set(7.5);
                        trans.Commit();
                    }
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

Nhập dòng param.Set(7.5), highlight 7.5.  
Chèn text: “Set: Đặt giá trị mới cho Head Height.”




Phần 3: Đăng ký và kiểm tra lệnh
Hướng dẫn viên (giọng điệu khích lệ):Lệnh SetParameter đã được thêm vào tệp manifest với tên “SetParameter”. Hãy kiểm tra nó trong Revit!

Bước 8: Chạy Debug và kiểm traTrong Visual Studio, nhấn F5 để chạy debug mode. Mở Revit, mở tệp bài tập của video này. Chuyển sang 3D View, chạy Add-Ins > External Tools > SetParameter, chọn một cửa sổ:

TaskDialog hiển thị:  
Storage Type: Double  
Value: 7.0 (feet)


Sau khi đóng TaskDialog, cửa sổ được cập nhật với Head Height = 7.5 feet (khoảng 2.286 m).

Lưu ý: Luôn kiểm tra StorageType trước khi lấy hoặc đặt giá trị để tránh lỗi.
Hành động trên màn hình:  

Hiển thị Visual Studio, nhấn F5.  
Trong Revit, mở tệp bài tập, chuyển sang 3D View, chạy SetParameter, chọn cửa sổ.  
Hiển thị TaskDialog, sau đó zoom vào cửa sổ trong 3D View để thấy chiều cao thay đổi.  
Chèn text: “Kiểm tra: Đặt Head Height thành 7.5 feet.”




Phần 4: Kết luận và bước tiếp theo
Hướng dẫn viên (giọng điệu truyền cảm hứng):Chúc mừng các bạn! Hôm nay, chúng ta đã:

Lấy thông số Head Height bằng BuiltInParameter và hiển thị StorageType, giá trị.  
Đặt giá trị mới (7.5 feet) bằng Set trong Transaction.  
Kiểm tra kết quả trong 3D View.

Kỹ thuật này rất hữu ích để tự động cập nhật nhiều thông số hoặc trích xuất dữ liệu từ phần tử. Hãy luôn kiểm tra StorageType để đảm bảo an toàn.
Bước 9: Kêu gọi hành độngHãy thử thay đổi giá trị Set (ví dụ: 8.0 feet) hoặc lấy thông số khác, như Width, và kiểm tra StorageType bằng if để xử lý các kiểu dữ liệu khác nhau. Nếu bạn gặp vấn đề, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay!
Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!
Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"Bài học tiếp theo: Làm việc nâng cao với Parameter."  
"Gặp vấn đề? Để lại câu hỏi trong phần bình luận!"


Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio, mô phỏng TaskDialog và cửa sổ cập nhật.

