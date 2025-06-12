# Kịch bản bài học: Lọc phần tử với FilteredElementCollector trong Revit

---

## Mục tiêu

Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ tìm hiểu về **FilteredElementCollector** – một công cụ mạnh mẽ trong Revit API để lọc phần tử trong dự án. Sau bài học này, bạn sẽ biết:

* **FilteredElementCollector** là gì và cách tạo nó.
* Các loại bộ lọc: **Quick**, **Slow**, và **Logical**.
* Cách áp dụng bộ lọc và truy xuất phần tử hoặc **Element ID**.

Hãy cùng bắt đầu!

---

## Phần 1: Giới thiệu FilteredElementCollector

Hướng dẫn viên (giọng điệu hào hứng): Trong Revit, một dự án chứa hàng ngàn phần tử, như tường, cửa, hoặc đồ nội thất. Để tìm các phần tử cụ thể, chúng ta sử dụng **FilteredElementCollector** – một đối tượng giúp thu thập phần tử và áp dụng bộ lọc để thu hẹp kết quả. Hãy cùng khám phá cách nó hoạt động!

### Bước 1: Cách tạo FilteredElementCollector

**FilteredElementCollector** có thể được tạo theo ba cách:

* **Toàn bộ tài liệu (Document)**: Tìm tất cả phần tử trong dự án.
* **Tập hợp phần tử cụ thể**: Tìm trong một danh sách phần tử cho trước.
* **Chế độ xem cụ thể (View)**: Tìm phần tử hiển thị trong một view.

Sau khi tạo, bạn phải áp dụng ít nhất một bộ lọc để thu hẹp kết quả.

Hành động trên màn hình:

* Hiển thị sơ đồ đơn giản:

    ```
    FilteredElementCollector
    ├── Toàn bộ Document
    ├── Tập hợp phần tử
    └── Chế độ xem (View)
    ```

* Chèn text: “**FilteredElementCollector**: Công cụ lọc phần tử trong Revit.”

---

### Bước 2: Các loại bộ lọc

Có ba loại bộ lọc:

* **Quick Filters**: Kế thừa từ **ElementQuickFilter**, nhanh vì chỉ kiểm tra thông tin cơ bản (ví dụ: danh mục hoặc lớp). Có các phương thức tắt như **OfCategory** hoặc **OfClass**.
* **Slow Filters**: Kế thừa từ **ElementSlowFilter**, chậm hơn vì kiểm tra chi tiết (ví dụ: mức độ hoặc tham số). Hiệu quả khi kết hợp với Quick Filter.
* **Logical Filters**: Kế thừa từ **ElementLogicalFilter**, như **LogicalAndFilter** (và) hoặc **LogicalOrFilter** (hoặc), để kết hợp các bộ lọc Quick và Slow.

Hành động trên màn hình:

* Hiển thị sơ đồ:

    ```
    Bộ lọc
    ├── Quick (OfCategory, OfClass)
    ├── Slow (Level, Parameter)
    └── Logical (And, Or)
    ```

* Chèn text: “**Quick, Slow, Logical**: Các loại bộ lọc cho **FilteredElementCollector**.”

---

## Phần 2: Áp dụng bộ lọc và truy xuất phần tử

Hướng dẫn viên (giọng điệu rõ ràng): Sau khi tạo **FilteredElementCollector**, chúng ta áp dụng bộ lọc và truy xuất phần tử hoặc **Element ID**. Hãy xem cách thực hiện!

### Bước 3: Áp dụng bộ lọc

Có ba cách áp dụng bộ lọc:

* **WherePasses**: Phương thức chính, áp dụng một bộ lọc duy nhất (ví dụ: `collector.WherePasses(filter)`).
* **Phương thức tắt**: Như **OfCategory** hoặc **OfClass**, áp dụng bộ lọc nhanh trực tiếp (ví dụ: `collector.OfCategory(BuiltInCategory.OST_Walls)`).
* **Kết hợp bộ lọc**: Sử dụng **LogicalAndFilter** hoặc **LogicalOrFilter** để áp dụng nhiều bộ lọc (ví dụ: danh mục Walls và lớp Wall).

Hành động trên màn hình:

* Hiển thị mã giả lập:

    ```csharp
    FilteredElementCollector collector = new FilteredElementCollector(document);
    collector.OfCategory(BuiltInCategory.OST_Walls); // Phương thức tắt
    collector.WherePasses(new ElementCategoryFilter(BuiltInCategory.OST_Walls)); // WherePasses
    ```

* Chèn text: “Áp dụng bộ lọc: Thu hẹp tập hợp phần tử.”

---

### Bước 4: Truy xuất phần tử

Sau khi áp dụng bộ lọc, sử dụng các phương thức để lấy kết quả:

* **ToElements()**: Trả về danh sách các **Element**.
* **FirstElement()**: Trả về **Element** đầu tiên.
* **ToElementIds()**: Trả về danh sách **Element ID**.
* **FirstElementId()**: Trả về **Element ID** đầu tiên.

Hành động trên màn hình:

* Hiển thị mã giả lập:

    ```csharp
    IList<Element> elements = collector.ToElements();
    Element firstElement = collector.FirstElement();
    ```

* Chèn text: “Truy xuất: Lấy **Element** hoặc **Element ID** từ collector.”

---

## Phần 3: Chuẩn bị áp dụng FilteredElementCollector

Hướng dẫn viên (giọng điệu khích lệ): Bây giờ chúng ta đã hiểu **FilteredElementCollector**, hãy chuẩn bị áp dụng nó trong một lệnh mới để lọc phần tử, ví dụ: tìm tất cả các tường trong dự án. Trong bài học này, chúng ta sẽ chỉ giới thiệu lý thuyết, nhưng hãy sẵn sàng cho bài tiếp theo!

### Bước 5: Xem FilteredElementCollector trong Object Browser

Trong Visual Studio, mở **Object Browser** bằng cách nhấp đúp vào tham chiếu **RevitAPI**. Tìm lớp **FilteredElementCollector** và xem các phương thức như **OfCategory**, **WherePasses**, **ToElements**. Điều này giúp bạn làm quen với các tùy chọn có sẵn.

Hành động trên màn hình:

* Hiển thị Solution Explorer, nhấp đúp vào RevitAPI để mở Object Browser.
* Nhập “FilteredElementCollector”, highlight các phương thức như **OfCategory**, **WherePasses**, **ToElements**.
* Chèn text: “**Object Browser**: Khám phá phương thức của **FilteredElementCollector**.”

---

## Phần 4: Kết luận và bước tiếp theo

Hướng dẫn viên (giọng điệu truyền cảm hứng): Chúc mừng các bạn! Hôm nay, chúng ta đã:

* Tìm hiểu về **FilteredElementCollector** và cách tạo nó cho tài liệu, tập hợp phần tử, hoặc view.
* Hiểu ba loại bộ lọc: **Quick**, **Slow**, và **Logical**.
* Nắm cách áp dụng bộ lọc với **WherePasses**, phương thức tắt, hoặc bộ lọc logic, và truy xuất phần tử bằng **ToElements** hoặc **ToElementIds**.

Trong bài học tiếp theo, chúng ta sẽ tạo một lệnh mới sử dụng **FilteredElementCollector** để lọc và hiển thị danh sách các phần tử, như tất cả các tường trong dự án.

### Bước 6: Kêu gọi hành động

Hãy mở Visual Studio và khám phá thêm lớp **FilteredElementCollector** trong **Object Browser**. Ghi chú các phương thức như **OfCategory** hoặc **ToElements** để chuẩn bị cho bài sau. Nếu bạn có câu hỏi, hãy để lại trong phần bình luận, mình sẽ hỗ trợ ngay!

Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!

Hành động trên màn hình:

* Hiển thị màn hình kết thúc với text:
    "Bài học tiếp theo: Tạo lệnh lọc phần tử với **FilteredElementCollector**."
    "Gặp vấn đề? Để lại câu hỏi trong phần bình luận!"

* Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio.