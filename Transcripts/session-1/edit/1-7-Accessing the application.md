# Kịch bản bài học: Truy cập ứng dụng Revit qua API

##  Mục tiêu

Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ tìm hiểu cách truy cập ứng dụng Revit thông qua API để chuẩn bị cho việc chọn phần tử và lấy Element ID. Sau bài học này, bạn sẽ hiểu:

* Sự khác biệt giữa các đối tượng `Application`, `UIApplication`, `Document`, và `UIDocument`.
* Cách sử dụng `ExternalCommandData` để truy cập `UIDocument`.
* Cách chuẩn bị mã để tương tác với giao diện người dùng của Revit.

Hãy cùng bắt đầu!

---

##  Phần 1: Hiểu các đối tượng truy cập Revit

**Hướng dẫn viên (giọng điệu hào hứng):**
Khi làm việc với Revit API, chúng ta có thể truy cập Revit qua **ứng dụng (application)** – phiên làm việc hiện tại của Revit – hoặc qua **tài liệu (document)** – tệp dự án Revit mà chúng ta đang mở. Có bốn đối tượng chính mà bạn cần biết:

### Bước 1: Tìm hiểu các đối tượng

* **Application**: Truy cập các cài đặt toàn ứng dụng, như vị trí dự án, ngôn ngữ, hoặc sự kiện ứng dụng.
* **UIApplication**: Truy cập giao diện người dùng, như thanh ribbon hoặc các thuộc tính UI. Ví dụ, bạn có thể thêm nút tùy chỉnh vào ribbon.
* **Document**: Đại diện cho tệp dự án Revit, chứa tất cả phần tử, chế độ xem, và dữ liệu trong mô hình.
* **UIDocument**: Truy cập các phương thức và thuộc tính giao diện ở cấp dự án, như làm mới chế độ xem, lấy phần tử được chọn, hoặc yêu cầu người dùng chọn phần tử.

 **Trong bài hôm nay, chúng ta sẽ sử dụng `UIDocument` để yêu cầu người dùng chọn một phần tử, từ đó lấy Element ID.**

**Hành động trên màn hình:**

* Hiển thị sơ đồ đơn giản:

```
Application   →   Cài đặt toàn ứng dụng  
UIApplication →   Giao diện (ribbon, UI)  
Document      →   Dữ liệu dự án  
UIDocument    →   Giao diện cấp dự án
```

* 🔍 Highlight `UIDocument` và chèn text: **“Dùng để chọn phần tử và lấy Element ID.”**

---

##  Phần 2: Truy cập UIDocument trong Visual Studio

**Hướng dẫn viên (giọng điệu rõ ràng):**
Hãy quay lại Visual Studio, nơi chúng ta đã để lại lệnh `GetElementId` từ bài trước. Hiện tại, lệnh chỉ trả về `Result.Succeeded`. Bây giờ, chúng ta sẽ thêm mã để truy cập `UIDocument` từ `ExternalCommandData`, chuẩn bị cho việc chọn phần tử.

### Bước 2: Mở tệp `GetElementId.cs`

Trong Visual Studio, mở tệp `GetElementId.cs` từ Solution Explorer. Đây là nơi chúng ta đã triển khai giao diện `IExternalCommand` với phương thức `Execute`.

**Hành động trên màn hình:**

```csharp
public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
{
    return Result.Succeeded;
}
```

---

### Bước 3: Truy cập UIDocument

Để truy cập `UIDocument`, chúng ta sử dụng tham số `commandData` trong phương thức `Execute`.
Thêm dòng mã sau trước `return Result.Succeeded;` để lấy `UIDocument`:

```csharp
UIDocument uidoc = commandData.Application.ActiveUIDocument;
```

Dòng mã này:

* Lấy `UIApplication` từ `commandData.Application`.
* Từ `UIApplication`, lấy `ActiveUIDocument` – chính là `UIDocument` của dự án hiện tại.

### Mã hoàn chỉnh sẽ trông như sau:

```csharp
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
            return Result.Succeeded;
        }
    }
}
```

**Hành động trên màn hình:**

* Hiển thị tệp `GetElementId.cs`, zoom vào phương thức `Execute`.
* Nhập dòng: `UIDocument uidoc = commandData.Application.ActiveUIDocument;`
* Chèn text: **“UIDocument: Truy cập giao diện cấp dự án để chọn phần tử.”**

---

##  Phần 3: Ý nghĩa của UIDocument

**Hướng dẫn viên (giọng điệu khích lệ):**
Giờ chúng ta đã có `UIDocument` trong lệnh! Đây là **chìa khóa để tương tác với giao diện người dùng của Revit**, như yêu cầu người dùng chọn một phần tử hoặc làm mới chế độ xem.

 Trong bài tiếp theo, chúng ta sẽ sử dụng `UIDocument` để chọn một phần tử và lấy `Element ID` của nó.

**Hành động trên màn hình:**

* Hiển thị giao diện Revit, mô phỏng hành động chọn một phần tử (ví dụ: một bức tường).
* Chèn text: **“UIDocument: Cho phép chọn phần tử và lấy Element ID.”**

---

##  Phần 4: Kết luận và bước tiếp theo

**Hướng dẫn viên (giọng điệu truyền cảm hứng):**
Chúc mừng các bạn! Hôm nay, chúng ta đã:

*  Tìm hiểu về bốn đối tượng truy cập Revit: `Application`, `UIApplication`, `Document`, và `UIDocument`.
*  Thêm mã để truy cập `UIDocument` từ `ExternalCommandData` trong lệnh `GetElementId`.
*  Chuẩn bị nền tảng để tương tác với giao diện người dùng của Revit.

---

###  Bước 4: Kêu gọi hành động

Trong bài học tiếp theo, chúng ta sẽ sử dụng `UIDocument` để yêu cầu người dùng **chọn một phần tử và lấy Element ID**.
 Hãy giữ Visual Studio và Revit sẵn sàng!
 Nếu bạn có câu hỏi, hãy để lại trong phần bình luận, mình sẽ hỗ trợ ngay.

---

**Hành động trên màn hình:**

```
"Bài học tiếp theo: Chọn phần tử và lấy Element ID."  
"Gặp vấn đề? Để lại câu hỏi trong phần bình luận!"
```

> *Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio.*
