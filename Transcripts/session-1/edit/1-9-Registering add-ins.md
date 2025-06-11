# Kịch bản bài học: Đăng ký Add-in trong Revit

## Mục tiêu

Chúc mừng các bạn đã hoàn thành lệnh `GetElementId`! Trong bài học hôm nay, chúng ta sẽ học cách đăng ký plugin vào Revit bằng cách tạo một tệp manifest (.addin). Sau bài học này, bạn sẽ biết:

* Tệp manifest là gì và vai trò của nó trong việc tải plugin vào Revit.
* Cách tạo và cấu hình tệp manifest trong Visual Studio.
* Cách tạo GUID và đảm bảo tệp manifest được sao chép khi biên dịch.

**Hãy cùng bắt đầu!**

---

## Phần 1: Giới thiệu về Manifest File

**Hướng dẫn viên (giọng điệu hào hứng):**
Chúng ta đã viết lệnh `GetElementId`, nhưng để Revit nhận diện và hiển thị lệnh này, chúng ta cần đăng ký plugin bằng một tệp manifest (.addin). Khi Revit khởi động, nó sẽ tìm các tệp `.addin` trong hai thư mục cụ thể để xác định plugin nào cần tải và với tùy chọn nào. Hãy cùng tạo tệp manifest cho dự án `MyRevitCommands`!

### Bước 1: Hiểu về Manifest File

Tệp manifest là một tệp XML chứa các cài đặt cho plugin, như tên, đường dẫn tệp DLL, và ID duy nhất. Revit sử dụng tệp này để biết cách hiển thị lệnh trong tab **Add-Ins**.

**Hành động trên màn hình:**

* Hiển thị giao diện Revit, zoom vào tab **Add-Ins**.
* Chèn text: “**Manifest File: Đăng ký plugin để hiển thị trong Revit.**”

---

## Phần 2: Tạo tệp Manifest trong Visual Studio

**Hướng dẫn viên (giọng điệu rõ ràng):**
Hãy quay lại Visual Studio để thêm tệp manifest vào dự án `MyRevitCommands`.

### Bước 2: Thêm tệp Manifest

Trong **Solution Explorer**, nhấp chuột phải vào tên dự án (MyRevitCommands), chọn **Add > New Item**. Trong cửa sổ **Add New Item**, tìm và chọn **Application Manifest File**. Đổi tên tệp thành `MyRevitCommands.addin` (lưu ý phần mở rộng `.addin` là bắt buộc để Revit nhận diện). Nhấn **Add**.

**Hành động trên màn hình:**

* Hiển thị **Solution Explorer**, nhấp chuột phải vào `MyRevitCommands`, chọn **Add > New Item**.
* Hiển thị cửa sổ **Add New Item**, highlight **Application Manifest File**, nhập `MyRevitCommands.addin`.
* Zoom vào nút **Add**.

### Bước 3: Xóa nội dung mặc định

Tệp `MyRevitCommands.addin` mới tạo sẽ chứa mã mặc định không cần thiết. Mở tệp, chọn toàn bộ nội dung và xóa.

**Hành động trên màn hình:**

* Hiển thị tệp `MyRevitCommands.addin` trong Visual Studio, highlight toàn bộ nội dung và nhấn **Delete**.

### Bước 4: Thêm mã XML cho Manifest

Sao chép nội dung XML mẫu từ `MyRevitCommands.txt` trong thư mục bài tập của video và dán vào `MyRevitCommands.addin`.

```xml
<?xml version="1.0" encoding="utf-8"?>
<RevitAddIns>
  <AddIn Type="Command">
    <Name>MyRevitCommands</Name>
    <Assembly>MyRevitCommands.dll</Assembly>
    <AddInId>YOUR_GUID_HERE</AddInId>
    <FullClassName>MyRevitCommands.GetElementId</FullClassName>
    <Text>Get Element ID</Text>
    <Description>Retrieves the Element ID of a selected element</Description>
    <VendorId>Jeremy</VendorId>
    <VisibilityMode>NotVisibleWhenNoActiveDocument</VisibilityMode>
  </AddIn>
</RevitAddIns>
```

**Hành động trên màn hình:**

* Hiển thị tệp `MyRevitCommands.txt`, sao chép nội dung.
* Dán vào `MyRevitCommands.addin` trong Visual Studio.
* Chèn text: “**Manifest File: XML định nghĩa cài đặt plugin.**”

---

## Phần 3: Cấu hình Manifest File

**Hướng dẫn viên (giọng điệu cẩn thận):**
Bây giờ, chúng ta sẽ chỉnh sửa tệp manifest để đảm bảo nó phù hợp với lệnh `GetElementId`.

### Bước 5: Tạo và thêm GUID

Trong Visual Studio, vào **Tools > Create GUID**. Chọn định dạng **Registry Format**, nhấn **New GUID**, sau đó **Copy** và dán vào thẻ `<AddInId>`, xóa `{}` hoặc ký tự thừa.

```xml
<AddInId>550e8400-e29b-41d4-a716-446655440000</AddInId>
```

**Hành động trên màn hình:**

* Hiển thị Visual Studio, vào **Tools > Create GUID**.
* Hiển thị cửa sổ **Create GUID**, chọn **Registry Format**, nhấn **New GUID** và **Copy**.
* Dán GUID vào `MyRevitCommands.addin`, xóa `{}`.
* Chèn text: “**GUID: Định danh duy nhất cho lệnh.**”

### Bước 6: Cấu hình các thẻ XML

Kiểm tra và chỉnh sửa các thẻ:

* `Name`: `MyRevitCommands`
* `Assembly`: `MyRevitCommands.dll`
* `FullClassName`: `MyRevitCommands.GetElementId`
* `Text`: `Get Element ID`
* `Description`: `Retrieves the Element ID of a selected element`
* `VendorId`: `Jeremy` (có thể thay bằng ID của bạn)
* `VisibilityMode`: `NotVisibleWhenNoActiveDocument`

**Hành động trên màn hình:**

* Hiển thị `MyRevitCommands.addin`, highlight từng thẻ XML và giải thích ngắn gọn.
* Zoom vào: `<VisibilityMode>NotVisibleWhenNoActiveDocument</VisibilityMode>`
* Chèn text: “**Cấu hình Manifest: Đảm bảo lệnh hiển thị đúng ngữ cảnh.**”

---

## Phần 4: Cấu hình sao chép tệp Manifest

**Hướng dẫn viên (giọng điệu khích lệ):**
Để tệp manifest được sao chép khi biên dịch, chúng ta cần điều chỉnh thuộc tính của nó.

### Bước 7: Thay đổi thuộc tính Copy to Output Directory

Trong **Solution Explorer**, chọn `MyRevitCommands.addin`. Ở cửa sổ **Properties**, đổi **Copy to Output Directory** thành **Copy if newer**.

**Hành động trên màn hình:**

* Hiển thị **Solution Explorer**, highlight `MyRevitCommands.addin`.
* Hiển thị cửa sổ **Properties**, đổi **Copy to Output Directory** thành **Copy if newer**.
* Chèn text: “**Copy if newer: Đảm bảo tệp manifest được sao chép khi biên dịch.**”

---

## Phần 5: Kết luận và bước tiếp theo

**Hướng dẫn viên (giọng điệu truyền cảm hứng):**
Chúc mừng các bạn! Hôm nay, chúng ta đã:

* Tạo và cấu hình tệp manifest (`MyRevitCommands.addin`) để đăng ký plugin.
* Tạo GUID duy nhất và điều chỉnh các thẻ XML cần thiết.
* Đảm bảo tệp manifest được sao chép khi biên dịch.

### Bước 8: Kêu gọi hành động

Trong bài học tiếp theo, chúng ta sẽ **biên dịch dự án**, **sao chép tệp DLL và manifest** vào thư mục add-ins của Revit, và **chạy lệnh trong Revit**.

**Hãy giữ Visual Studio và Revit sẵn sàng!**
Nếu bạn gặp vấn đề, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay.

**Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!**

**Hành động trên màn hình:**

* Hiển thị màn hình kết thúc với text:

```
Bài học tiếp theo: Biên dịch và chạy plugin trong Revit.
Gặp vấn đề? Để lại câu hỏi trong phần bình luận!
```

* Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio.
