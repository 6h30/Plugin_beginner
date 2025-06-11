# Kịch bản bài học: Truy xuất thông tin phần tử trong Revit

## Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ cải tiến lệnh GetElementId để truy xuất và hiển thị thông tin chi tiết của một phần tử, bao gồm Category, Instance Name, Family Symbol Name, và Family Name. Sau bài học này, bạn sẽ biết:

- Cách sử dụng Object Browser để tìm thuộc tính và phương thức của lớp Element.
- Cách lấy Element Type bằng GetTypeId và truy xuất thông tin từ nó.
- Cách sử dụng TaskDialog để hiển thị nhiều thông tin phần tử.

Hãy cùng bắt đầu!

---

## Phần 1: Khám phá lớp Element trong Object Browser

**Hướng dẫn viên (giọng điệu hào hứng):**  
Trong bài trước, chúng ta đã lấy được đối tượng Element từ Element ID. Bây giờ, hãy khám phá lớp Element để tìm cách truy xuất thông tin chi tiết, như danh mục, tên instance, và thông tin về family.

### Bước 1: Mở Object Browser và tìm lớp Element  
Trong Visual Studio, mở Object Browser bằng cách nhấp đúp vào tham chiếu RevitAPI trong Solution Explorer. Tìm lớp Element bằng cách nhập “Element” vào ô tìm kiếm. Trong lớp Element, bạn sẽ thấy:

- **Category:** Thuộc tính trả về danh mục của phần tử (ví dụ: Walls, Doors).  
- **Name:** Thuộc tính trả về tên instance của phần tử.  
- **GetTypeId():** Phương thức trả về Element ID của Element Type (Family Symbol).

**Hành động trên màn hình:**  
- Hiển thị Solution Explorer, nhấp đúp vào RevitAPI để mở Object Browser.  
- Nhập “Element” vào ô tìm kiếm, highlight lớp Element.  
- Zoom vào các thuộc tính Category, Name, và phương thức GetTypeId().  
- Chèn text: “Lớp Element: Chứa thông tin về Category, Name, và Type.”

---

### Bước 2: Tìm hiểu lớp ElementType  
Tìm lớp ElementType trong Object Browser (vì Family Symbol kế thừa từ ElementType). Trong lớp ElementType, bạn sẽ thấy:

- **Name:** Thuộc tính trả về tên Family Symbol (ví dụ: “Brick Wall”).  
- **FamilyName:** Thuộc tính trả về tên Family (ví dụ: “Basic Wall”).

**Hành động trên màn hình:**  
- Trong Object Browser, nhập “ElementType”, highlight lớp ElementType.  
- Zoom vào Name và FamilyName.  
- Chèn text: “ElementType: Chứa thông tin về Family Symbol và Family Name.”

---

## Phần 2: Cập nhật mã trong Visual Studio

**Hướng dẫn viên (giọng điệu rõ ràng):**  
Hãy quay lại Visual Studio để cập nhật lệnh GetElementId. Chúng ta sẽ lấy Element Type và hiển thị thông tin chi tiết của phần tử trong TaskDialog.

### Bước 3: Mở tệp GetElementId.cs  
Mở tệp GetElementId.cs trong dự án MyRevitCommands. Tiếp tục từ mã của bài trước, nơi chúng ta đã lấy đối tượng Element. Nếu bạn chưa có mã này, hãy mở tệp từ thư mục bài tập của video.

**Hành động trên màn hình:**  
- Hiển thị Solution Explorer, nhấp vào GetElementId.cs.  
- Zoom vào phương thức Execute, highlight dòng lấy Element:  
  ```csharp
  Element ele = doc.GetElement(eleId);
```

---

### Bước 4: Lấy Element Type

Sau dòng lấy Element, thêm mã để lấy Element Type bằng GetTypeId và GetElement. Sử dụng từ khóa `as` để ép kiểu an toàn:

```csharp
ElementId eTypeId = ele.GetTypeId();
ElementType eType = doc.GetElement(eTypeId) as ElementType;
```

* **GetTypeId():** Trả về Element ID của Element Type.
* **as ElementType:** Ép kiểu an toàn, trả về null nếu không thành công, tránh ngoại lệ.

**Hành động trên màn hình:**

* Nhập các dòng:

  ```csharp
  ElementId eTypeId = ele.GetTypeId();
  ElementType eType = doc.GetElement(eTypeId) as ElementType;
  ```
* Highlight từ khóa `as`, giải thích bằng text: “as: Ép kiểu an toàn, trả về null nếu thất bại.”

---

### Bước 5: Cập nhật TaskDialog để hiển thị thông tin

Cập nhật TaskDialog để hiển thị Element ID, Category, Instance Name, Family Symbol Name, và Family Name. Sử dụng `Environment.NewLine` để xuống dòng. Thêm namespace `System` nếu chưa có. Mã hoàn chỉnh trong phương thức Execute sẽ như sau:

```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System; // Thêm namespace System cho Environment.NewLine

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
                    Document doc = uidoc.Document;
                    ElementId eleId = pickedObj.ElementId;
                    Element ele = doc.GetElement(eleId);
                    ElementId eTypeId = ele.GetTypeId();
                    ElementType eType = doc.GetElement(eTypeId) as ElementType;

                    string info = "Element ID: " + eleId.ToString() + Environment.NewLine +
                                  "Category: " + ele.Category.Name + Environment.NewLine +
                                  "Instance Name: " + ele.Name + Environment.NewLine +
                                  "Symbol Name: " + (eType != null ? eType.Name : "N/A") + Environment.NewLine +
                                  "Family Name: " + (eType != null ? eType.FamilyName : "N/A");

                    TaskDialog.Show("Element Classification", info);
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
```

* `Environment.NewLine`: Thêm dòng mới giữa các thông tin.
* `eType != null ? ... : "N/A"`: Kiểm tra eType để tránh lỗi nếu không có Element Type.

**Hành động trên màn hình:**

* Thêm dòng `using System;` ở đầu tệp.
* Cập nhật TaskDialog với chuỗi `info`, highlight `Environment.NewLine` và các thuộc tính như `ele.Category.Name`.
* Chèn text: “TaskDialog: Hiển thị thông tin phân loại phần tử.”

---

## Phần 3: Kiểm tra lệnh trong Revit

**Hướng dẫn viên (giọng điệu khích lệ):**
Hãy kiểm tra xem lệnh của chúng ta có hoạt động như mong đợi không!

### Bước 6: Chạy Debug và kiểm tra

Trong Visual Studio, nhấn F5 để chạy debug mode. Mở Revit, tạo một dự án mới với Architectural Template. Chuyển đến tab Add-Ins > External Tools > Get Element ID, chọn một phần tử (ví dụ: một section). TaskDialog sẽ hiển thị:

* Element ID: ID của phần tử.
* Category: Danh mục (ví dụ: Sections).
* Instance Name: Tên instance (ví dụ: Section 1).
* Symbol Name: Tên Family Symbol (ví dụ: Building Section).
* Family Name: Tên Family (ví dụ: Section).

**Hành động trên màn hình:**

* Hiển thị Visual Studio, nhấn F5.
* Trong Revit, tạo dự án mới, chạy lệnh Get Element ID, chọn một section.
* Hiển thị TaskDialog với các thông tin như “Category: Sections”, “Instance Name: Section 1”.
* Chèn text: “Kiểm tra: Hiển thị thông tin phân loại phần tử.”

---

## Phần 4: Kết luận và bước tiếp theo

**Hướng dẫn viên (giọng điệu truyền cảm hứng):**
Chúc mừng các bạn! Hôm nay, chúng ta đã:

* Khám phá lớp Element và ElementType trong Object Browser.
* Cập nhật lệnh GetElementId để hiển thị Category, Instance Name, Family Symbol Name, và Family Name.
* Kiểm tra lệnh trong Revit để xác nhận thông tin phân loại phần tử.

Những cách phân loại này rất quan trọng khi lọc, tìm kiếm, hoặc thao tác với phần tử trong Revit API. Trong bài học tiếp theo, chúng ta sẽ tìm hiểu cách lọc phần tử dựa trên các phân loại này, như tìm tất cả các tường trong dự án.

### Bước 7: Kêu gọi hành động

Hãy thử chạy lệnh với các phần tử khác (như tường, cửa) và kiểm tra thông tin trong TaskDialog. Thêm breakpoint để xem giá trị của eType hoặc ele.Category. Nếu bạn gặp vấn đề, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay!

Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo!

**Hành động trên màn hình:**

* Hiển thị màn hình kết thúc với text:

  * "Bài học tiếp theo: Lọc phần tử theo phân loại."
  * "Gặp vấn đề? Để lại câu hỏi trong phần bình luận!"

* Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio, mô phỏng TaskDialog hiển thị thông tin.
