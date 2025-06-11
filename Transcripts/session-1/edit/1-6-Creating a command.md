# Kịch bản bài học: Tạo lệnh Revit để lấy Element ID

## Mục tiêu

Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ sử dụng giao diện `IExternalCommand` để tạo một lệnh Revit thực sự. Lệnh này sẽ cho phép chúng ta chọn một phần tử trong Revit và lấy **Element ID** của nó. Sau bài học này, bạn sẽ biết:

* Cách đổi tên lớp để phản ánh chức năng của lệnh.
* Cách triển khai giao diện `IExternalCommand` trong Visual Studio.
* Cách thêm thuộc tính `Transaction` cần thiết cho lệnh Revit.

Hãy cùng bắt đầu!

---

## Phần 1: Đổi tên lớp và chuẩn bị

**Hướng dẫn viên (giọng điệu hào hứng):**
Chúng ta đã hiểu về `IExternalCommand` từ bài trước, giờ là lúc áp dụng nó để tạo một lệnh thực tế! Lệnh hôm nay sẽ chọn một phần tử trong Revit và hiển thị Element ID của nó. Hãy tiếp tục từ dự án `MyRevitCommands` mà chúng ta đã tạo. Nếu bạn chưa mở, hãy tìm tệp dự án trong thư mục bài tập của video này.

### Bước 1: Đổi tên lớp

Trong **Solution Explorer** bên phải, tìm tệp `Class1.cs`. Để lớp phản ánh đúng chức năng, nhấp chuột phải vào `Class1.cs`, chọn **Rename**, và đổi tên thành `GetElementId`. Nhấn Enter. Visual Studio sẽ hỏi bạn có muốn cập nhật tất cả tham chiếu đến `Class1` không – hãy chọn **Yes**.

**Hành động trên màn hình:**

* Hiển thị Solution Explorer, nhấp chuột phải vào `Class1.cs`, chọn **Rename**.
* Nhập `GetElementId.cs`, nhấn Enter.
* Hiển thị hộp thoại xác nhận đổi tên, highlight nút **Yes**.

---

## Phần 2: Triển khai giao diện IExternalCommand

**Hướng dẫn viên (giọng điệu rõ ràng):**
Bây giờ, hãy triển khai giao diện `IExternalCommand` để lớp của chúng ta trở thành một lệnh Revit thực sự!

### Bước 2: Thêm `IExternalCommand` vào lớp

Mở tệp `GetElementId.cs`. Sau tên lớp, thêm dấu hai chấm `:` và gõ `IExternalCommand`. Bạn sẽ thấy IntelliSense gợi ý giao diện này, vì chúng ta đã thêm namespace `Autodesk.Revit.UI` trước đó. Mã sẽ trông như sau:

```csharp
public class GetElementId : IExternalCommand
```

**Hành động trên màn hình:**

* Hiển thị tệp `GetElementId.cs` trong Visual Studio.
* Zoom vào dòng `public class GetElementId`, nhập `: IExternalCommand`.
* Hiển thị IntelliSense gợi ý `IExternalCommand`.

### Bước 3: Xử lý lỗi triển khai giao diện

Sau khi thêm `IExternalCommand`, bạn sẽ thấy một đường gạch đỏ dưới tên giao diện, cho biết lỗi: phương thức `Execute` chưa được triển khai. Di chuột lên đường gạch đỏ, bạn sẽ thấy Visual Studio báo lỗi này. Để khắc phục, nhấp vào biểu tượng bóng đèn (**Quick Actions**) và chọn **Implement interface**. Visual Studio sẽ tự động tạo phương thức `Execute` với ba tham số cần thiết.

**Hành động trên màn hình:**

* Hiển thị đường gạch đỏ dưới `IExternalCommand`, di chuột để hiện thông báo lỗi.
* Nhấp vào biểu tượng bóng đèn, chọn **Implement interface**.
* Hiển thị mã được tạo tự động:

```csharp
public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
{
    throw new NotImplementedException();
}
```

---

## Phần 3: Sửa phương thức Execute

**Hướng dẫn viên (giọng điệu khích lệ):**
Phương thức `Execute` hiện tại chỉ ném ra một ngoại lệ `NotImplementedException`. Chúng ta cần sửa nó để trả về một giá trị `Result` hợp lệ, như chúng ta đã học ở bài trước.

### Bước 4: Xóa ngoại lệ và trả về Result

Xóa dòng:

```csharp
throw new NotImplementedException();
```

và thay bằng:

```csharp
return Result.Succeeded;
```

Điều này báo cho Revit rằng lệnh đã chạy thành công. IntelliSense sẽ gợi ý ba tùy chọn: `Succeeded`, `Canceled`, và `Failed`.

**Hành động trên màn hình:**

* Hiển thị phương thức `Execute`, highlight dòng `throw new NotImplementedException();`.
* Xóa dòng này, nhập `return Result.Succeeded;`.
* Hiển thị IntelliSense với các tùy chọn: `Succeeded`, `Canceled`, `Failed`.

### Bước 5: Hiểu các tham số của Execute

Nhắc lại từ bài trước, phương thức `Execute` có ba tham số:

* `ExternalCommandData commandData`: Truy cập dữ liệu Revit (ứng dụng, tài liệu, chế độ xem).
* `ref string message`: Chuỗi để gửi thông báo nếu lệnh thất bại hoặc bị hủy. Từ khóa `ref` cho phép sửa đổi chuỗi này.
* `ElementSet elements`: Tập hợp các phần tử để highlight nếu lệnh thất bại.

Chúng ta sẽ sử dụng các tham số này khi thêm mã hữu ích ở bài sau.

**Hành động trên màn hình:**

* Hiển thị phương thức `Execute`, highlight từng tham số và giải thích ngắn gọn bằng text:

  * `commandData`: Truy cập dữ liệu Revit.
  * `ref message`: Thông báo lỗi/hủy.
  * `elements`: Highlight phần tử thất bại.

---

## Phần 4: Thêm thuộc tính Transaction

**Hướng dẫn viên (giọng điệu rõ ràng):**
Để lệnh hoạt động trong Revit, chúng ta cần thêm một **Transaction Attribute**. Thuộc tính này báo cho Revit cách lệnh sử dụng giao dịch (transaction) – một khái niệm quan trọng mà chúng ta sẽ tìm hiểu sau.

### Bước 6: Thêm Transaction Attribute

Trên dòng ngay trước khai báo lớp, thêm thuộc tính Transaction như sau:

```csharp
[Transaction(TransactionMode.ReadOnly)]
public class GetElementId : IExternalCommand
```

`TransactionMode.ReadOnly` cho biết lệnh chỉ đọc dữ liệu, không chỉnh sửa. Chúng ta sẽ học về các chế độ giao dịch khác (như `Manual` hoặc `Automatic`) sau.

**Hành động trên màn hình:**

* Hiển thị tệp `GetElementId.cs`, zoom vào dòng trước `public class`.
* Nhập `[Transaction(TransactionMode.ReadOnly)]`.
* Chèn text: “Transaction Attribute: Yêu cầu cho mọi lệnh IExternalCommand.”

---

## Phần 5: Kết luận và bước tiếp theo

**Hướng dẫn viên (giọng điệu truyền cảm hứng):**
Chúc mừng các bạn! Chúng ta đã:

* Đổi tên lớp thành `GetElementId` để phản ánh chức năng.
* Triển khai giao diện `IExternalCommand` với phương thức `Execute`.
* Thêm thuộc tính `Transaction` để lệnh hoạt động trong Revit.

Lệnh hiện tại chưa làm gì nhiều, nhưng đừng lo! Trong bài học tiếp theo, chúng ta sẽ thêm mã để **chọn một phần tử và lấy Element ID** của nó, khiến lệnh trở nên hữu ích hơn.

### Bước 7: Kêu gọi hành động

Hãy giữ Visual Studio và Revit sẵn sàng. Trong bài tiếp theo, chúng ta sẽ viết mã để tương tác với cơ sở dữ liệu Revit. Nếu bạn gặp vấn đề, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay!

**Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!**

**Hành động trên màn hình:**

Hiển thị màn hình kết thúc với text:

```
"Bài học tiếp theo: Thêm mã để lấy Element ID."  
"Gặp vấn đề? Để lại câu hỏi trong phần bình luận!"
```

> *Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio.*

