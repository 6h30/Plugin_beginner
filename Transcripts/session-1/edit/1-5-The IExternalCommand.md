# Kịch bản bài học: Tạo lệnh Revit đầu tiên với IExternalCommand

##  Mục tiêu

Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Hôm nay, chúng ta sẽ bắt tay vào tạo lệnh Revit đầu tiên bằng cách sử dụng giao diện `IExternalCommand`. Sau bài học này, bạn sẽ hiểu:

* `IExternalCommand` là gì và vai trò của nó trong Revit.
* Các tham số của phương thức `Execute` và cách chúng hoạt động.
* Ý nghĩa của các giá trị trả về (`Succeeded`, `Canceled`, `Failed`).
* Làm thế nào để chuẩn bị mã cho một lệnh Revit cơ bản.

**Hãy cùng bắt đầu!**

---

## Phần 1: Giới thiệu về `IExternalCommand`

###  Hướng dẫn viên (giọng điệu hào hứng):

Để tạo một plugin Revit, chúng ta cần tạo một **lệnh bên ngoài (external command)**. Đây là các đối tượng sử dụng giao diện `IExternalCommand` để tương tác với cơ sở dữ liệu Revit và các phần tử được chọn. Hãy cùng tìm hiểu nó là gì!

### 🔹 Bước 1: Hiểu về `IExternalCommand`

Trong C#, một `interface` giống như một hợp đồng, chứa các phương thức và thuộc tính mà một lớp phải triển khai. `IExternalCommand` là một interface đặc biệt trong Revit API, giúp tạo các lệnh hiển thị trong tab **Add-Ins** của Revit.

> Khi chúng ta biên dịch mã thành tệp DLL, Revit sẽ đọc tệp này và thêm các lệnh `IExternalCommand` vào giao diện, miễn là chúng được cấu hình đúng trong tệp manifest (chúng ta sẽ tìm hiểu về manifest sau).

**Hành động trên màn hình:**

* Hiển thị giao diện Revit, zoom vào tab **Add-Ins**.
* Chèn text: *“IExternalCommand: Tạo lệnh tùy chỉnh cho Revit.”*

### 🔹 Bước 2: Phương thức `Execute`

`IExternalCommand` có một phương thức quan trọng: `Execute`. Đây là phương thức được gọi khi bạn nhấn vào lệnh trong Revit.

**Khi lệnh chạy:**

* Một đối tượng lệnh được tạo.
* Phương thức `Execute` được gọi.
* Sau khi hoàn tất, đối tượng lệnh bị hủy.

**Hành động trên màn hình:**
Hiển thị sơ đồ đơn giản:
**Nhấn lệnh → Tạo đối tượng → Gọi Execute → Hủy đối tượng**

---

## Phần 2: Các tham số của phương thức Execute

###  Hướng dẫn viên (giọng điệu rõ ràng):

Phương thức `Execute` cần ba tham số để hoạt động trong Revit. Revit tự động cung cấp các tham số này khi lệnh được chạy. Hãy cùng xem chúng là gì!

### 🔹 Bước 3: Tham số 1 – `ExternalCommandData`

Chứa tham chiếu đến ứng dụng Revit và chế độ xem hiện tại. Qua đối tượng này, chúng ta có thể truy cập toàn bộ dữ liệu Revit.

```csharp
ExternalCommandData commandData;
```

>  *ExternalCommandData: Cửa ngõ vào dữ liệu Revit.*

### 🔹 Bước 4: Tham số 2 – `string` (Message)

Chuỗi dùng để gửi thông báo nếu lệnh thất bại hoặc bị hủy.

```csharp
string message = "Lệnh đã bị hủy!";
```

>  *String: Gửi thông báo khi lệnh thất bại hoặc bị hủy.*

### 🔹 Bước 5: Tham số 3 – `ElementSet`

Ban đầu rỗng, dùng để chứa các phần tử bạn muốn highlight nếu lệnh thất bại.

```csharp
ElementSet elements = new ElementSet();
```

>  *ElementSet: Highlight các phần tử khi lệnh thất bại.*

### 🔹 Bước 6: Giá trị trả về của `Execute`

Phương thức `Execute` phải trả về một trong ba giá trị kiểu `Result`:

* `Result.Succeeded`: Lệnh chạy thành công.
* `Result.Cancelled`: Người dùng hủy lệnh.
* `Result.Failed`: Gặp lỗi, trả về thông báo và danh sách phần tử.

```csharp
return Result.Succeeded;
```

> 📌 *Result: Báo cho Revit trạng thái của lệnh.*

---

## Phần 3: Chuẩn bị triển khai `IExternalCommand`

###  Hướng dẫn viên (giọng điệu khích lệ):

Bây giờ chúng ta đã hiểu về `IExternalCommand`, hãy chuẩn bị mã để triển khai lệnh đầu tiên trong Visual Studio!

### 🔹 Bước 7: Tạo lớp triển khai `IExternalCommand`

Mở tệp `Class1.cs` trong dự án `MyRevitCommands`.

Chúng ta cần:

* Kế thừa giao diện `IExternalCommand`.
* Triển khai phương thức `Execute`.

```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class MyFirstCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Mã lệnh sẽ được viết ở đây
            return Result.Succeeded;
        }
    }
}
```

**Hành động trên màn hình:**

* Hiển thị Visual Studio, mở tệp `Class1.cs`.
* Highlight các phần quan trọng:

  * `[Transaction(TransactionMode.Manual)]`: Thuộc tính cần thiết.
  * `IExternalCommand`: Giao diện được kế thừa.
  * `Execute`: Phương thức chính với 3 tham số.

---

## Phần 4: Kết luận và bước tiếp theo

### Hướng dẫn viên (giọng điệu truyền cảm hứng):

 **Chúc mừng các bạn!** Hôm nay, chúng ta đã:

* Tìm hiểu về `IExternalCommand` và vai trò trong Revit.
* Hiểu ba tham số của `Execute` và các giá trị trả về.
* Chuẩn bị mã cơ bản để tạo lệnh Revit đầu tiên.

### 🔹 Bước 8: Kêu gọi hành động

Trong bài học tiếp theo, chúng ta sẽ viết **mã cụ thể trong phương thức `Execute`** để tương tác với cơ sở dữ liệu Revit, như **tạo một phần tử** hoặc **hiển thị thông báo**.

Hãy giữ Visual Studio và Revit sẵn sàng! Nếu bạn có câu hỏi, hãy để lại trong phần bình luận nhé.

**Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!**

---

### Hành động trên màn hình:

Hiển thị màn hình kết thúc với text:

```
"Bài học tiếp theo: Viết mã cho lệnh Revit đầu tiên."  
"Gặp vấn đề? Để lại câu hỏi trong phần bình luận!"
```

> *Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio.*

---
