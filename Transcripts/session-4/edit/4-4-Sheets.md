---
## Kịch bản bài học: Tạo Sheet trong Revit
---

### Mục tiêu
Chào mừng các bạn quay lại với khóa học lập trình plugin cho Revit! Trong bài học hôm nay, chúng ta sẽ học cách **tạo Sheet** trong Revit API, một bước quan trọng để tổ chức các view đã chú thích. Chúng ta sẽ tạo một lệnh **Sheet** để tự động tạo một **ViewSheet**, sử dụng **TitleBlock** mặc định, và cập nhật tên cùng số hiệu sheet. Sau bài học này, bạn sẽ biết:

* **ViewSheet** là gì và cách tạo nó trong Revit API.
* Cách tìm **TitleBlock** bằng **FilteredElementCollector**.
* Cách chỉnh sửa thuộc tính `Name` và `SheetNumber` của sheet.

Hãy cùng bắt đầu!

---
## Phần 1: Tìm hiểu về Sheet trong Revit API
---

Hướng dẫn viên (giọng điệu hào hứng): Trong bài trước, chúng ta đã gắn thẻ phần tử để chú thích view. Hôm nay, chúng ta sẽ đưa các view này lên Sheet, một loại view đặc biệt trong Revit. Hãy khám phá cách tạo **ViewSheet** trong Revit API!

### Bước 1: ViewSheet trong Revit API
**ViewSheet** là một lớp kế thừa từ **View**, dùng để tạo các tờ bản vẽ (**Sheet**) trong Revit. Đặc điểm:

* Tạo bằng phương thức tĩnh `ViewSheet.Create`, yêu cầu:
    * **Document**: Tài liệu Revit.
    * **TitleBlockId**: ID của **FamilySymbol** (**TitleBlock**). Có thể dùng `ElementId.InvalidElementId` để không sử dụng **TitleBlock**.

* Sau khi tạo, có thể đặt các view lên sheet bằng **Viewport** (sẽ học ở bài sau).
* Thuộc tính có thể chỉnh sửa: `Name` (tên sheet), `SheetNumber` (số hiệu sheet).

Hành động trên màn hình:  

Hiển thị sơ đồ:
```
ViewSheet
├── Inherits: View
├── Create: Document, TitleBlockId
├── Properties: Name, SheetNumber
└── Viewport: Đặt view lên sheet (bài sau)
```

Chèn text: “**ViewSheet**: Tạo tờ bản vẽ.”

### Bước 2: Tìm TitleBlock
Để tạo **ViewSheet** với **TitleBlock**, sử dụng **FilteredElementCollector** để tìm **FamilySymbol** thuộc danh mục **OST_TitleBlocks**. Nếu dự án có nhiều **TitleBlock**, có thể lọc thêm theo tên.

Hành động trên màn hình:  

Hiển thị mã giả lập:
```csharp
FamilySymbol tBlock = new FilteredElementCollector(doc)
    .OfCategory(BuiltInCategory.OST_TitleBlocks)
    .WhereElementIsElementType()
    .Cast<FamilySymbol>()
    .First();
ViewSheet vSheet = ViewSheet.Create(doc, tBlock.Id);
```

Chèn text: “**FilteredElementCollector**: Tìm TitleBlock.”

---
## Phần 2: Tạo lệnh Sheet trong Visual Studio
---

Hướng dẫn viên (giọng điệu rõ ràng): Hãy mở Visual Studio để tạo lệnh **Sheet**, tự động tạo một **ViewSheet** với **TitleBlock** mặc định, và đặt tên cùng số hiệu sheet.

### Bước 3: Mở tệp Sheet.cs
Mở tệp `Sheet.cs` trong dự án `MyRevitCommands`. Mã cơ bản đã được chuẩn bị, lấy **Document** và bắt đầu **Transaction**:

```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System; // Thêm namespace System cho Exception

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class Sheet : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                using (Transaction trans = new Transaction(doc, "Create Sheet"))
                {
                    trans.Start();
                    // Tạo sheet tại đây
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
```

Hành động trên màn hình:  

Hiển thị Solution Explorer, nhấp vào `Sheet.cs`.  
Zoom vào khối **Transaction**, highlight `doc`.  
Chèn text: “**Sheet**: Điểm khởi đầu để tạo ViewSheet.”

### Bước 4: Tìm TitleBlock
Sử dụng **FilteredElementCollector** để tìm **FamilySymbol** của **TitleBlock**:

```csharp
FamilySymbol tBlock = new FilteredElementCollector(doc)
    .OfCategory(BuiltInCategory.OST_TitleBlocks)
    .WhereElementIsElementType()
    .Cast<FamilySymbol>()
    .First();
```

* `OfCategory(OST_TitleBlocks)`: Lọc danh mục **TitleBlocks**.
* `WhereElementIsElementType`: Chỉ lấy các **ElementType** (**FamilySymbol**).
* `Cast()`: Ép kiểu kết quả.
* `First`: Lấy **TitleBlock** đầu tiên (giả định chỉ có một).

Hành động trên màn hình:  

Nhập mã **FilteredElementCollector**, highlight `OST_TitleBlocks` và `First`.  
Chèn text: “**FamilySymbol**: Tìm TitleBlock mặc định.”

### Bước 5: Tạo ViewSheet và chỉnh sửa thuộc tính
Trong **Transaction**, tạo **ViewSheet** và đặt `Name` cùng `SheetNumber`:

```csharp
ViewSheet vSheet = ViewSheet.Create(doc, tBlock.Id);
vSheet.Name = "My First Sheet";
vSheet.SheetNumber = "J101";
```

* `ViewSheet.Create`: Tạo sheet với **Document** và **TitleBlockId**.
* `Name`: Đặt tên sheet.
* `SheetNumber`: Đặt số hiệu sheet.

Mã hoàn chỉnh trong phương thức Execute:

```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Linq; // Thêm namespace này cho First
using System; // Thêm namespace System cho Exception

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class Sheet : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                FamilySymbol tBlock = new FilteredElementCollector(doc)
                    .OfCategory(BuiltInCategory.OST_TitleBlocks)
                    .WhereElementIsElementType()
                    .Cast<FamilySymbol>()
                    .First();

                using (Transaction trans = new Transaction(doc, "Create Sheet"))
                {
                    trans.Start();
                    ViewSheet vSheet = ViewSheet.Create(doc, tBlock.Id);
                    vSheet.Name = "My First Sheet";
                    vSheet.SheetNumber = "J101";
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
```

Hành động trên màn hình:  

Nhập mã `ViewSheet.Create`, highlight `"My First Sheet"` và `"J101"`.  
Thêm `using System.Linq;` cho `First`.  
Chèn text: “**ViewSheet**: Tạo và đặt tên sheet.”

---
## Phần 3: Đăng ký và kiểm tra lệnh
---

Hướng dẫn viên (giọng điệu khích lệ): Lệnh **Sheet** đã được thêm vào tệp manifest với tên “Sheet”. Hãy kiểm tra nó trong Revit!

### Bước 6: Chạy Debug và kiểm tra
Trong Visual Studio, nhấn **F5** để chạy debug mode. Mở Revit, mở tệp bài tập của video này. Chạy **Add-Ins > External Tools > Sheet**. Kết quả: Một **ViewSheet** mới tên “My First Sheet” với số hiệu “J101” sẽ xuất hiện trong Project Browser, sử dụng **TitleBlock** mặc định.

Hành động trên màn hình:  

Hiển thị Visual Studio, nhấn F5.  
Trong Revit, mở tệp bài tập, chạy Sheet.  
Mở **Project Browser**, highlight sheet “My First Sheet” (J101).  
Chèn text: “**Kiểm tra**: Tạo ViewSheet mới.”

---
## Phần 4: Kết luận và bước tiếp theo
---

Hướng dẫn viên (giọng điệu truyền cảm hứng): Chúc mừng các bạn! Hôm nay, chúng ta đã:

* Tìm hiểu **ViewSheet** và cách tạo nó trong Revit API.  
* Tạo lệnh **Sheet** để tự động tạo sheet với **TitleBlock**.  
* Sử dụng **FilteredElementCollector** để tìm **TitleBlock** và chỉnh sửa thuộc tính sheet.

Tạo sheet tự động rất hữu ích để chuẩn hóa và tăng tốc quy trình thiết kế.

### Bước 7: Kêu gọi hành động
Hãy thử lọc **TitleBlock** theo tên nếu dự án có nhiều **TitleBlock**. Hoặc thử tạo sheet mà không dùng **TitleBlock** bằng `ElementId.InvalidElementId`. Nếu bạn gặp vấn đề, hãy để lại câu hỏi trong phần bình luận, mình sẽ hỗ trợ ngay!

Cảm ơn các bạn đã theo dõi! Hẹn gặp lại ở bài học tiếp theo, nơi chúng ta sẽ đặt view lên sheet!

Hành động trên màn hình:  

Hiển thị màn hình kết thúc với text:  
"**Bài học tiếp theo: Đặt View lên Sheet.**"  
"**Gặp vấn đề? Để lại câu hỏi trong phần bình luận!**"

Chèn logo khóa học hoặc hình ảnh minh họa Revit/Visual Studio, mô phỏng Project Browser với sheet mới.