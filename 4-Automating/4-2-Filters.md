**4.2. Filters (Tạo và Áp dụng Bộ lọc)** 
 
Revit API cho phép tạo các bộ lọc (filters) để kiểm soát hiển thị phần tử trong khung nhìn. Bộ lọc dựa trên tham số (`ParameterFilterElement`) là loại bộ lọc dựa trên quy tắc (rule-based), cho phép lọc phần tử theo giá trị tham số. Trong phần này, chúng ta sẽ tạo một lệnh để lọc các mặt cắt (sections) có tên chứa chuỗi “WIP” và ẩn chúng trong khung nhìn hiện tại.

**Tra cứu ParameterFilterElement**:  
- Trong **Object Browser**, tìm lớp `ParameterFilterElement` trong namespace `Autodesk.Revit.DB`.  
- Constructor: `ParameterFilterElement.Create(Document, string name, IList<ElementId> categories, ElementFilter filter)`:  
  - `Document`: Tài liệu Revit.  
  - `name`: Tên bộ lọc.  
  - `categories`: Danh sách ID danh mục (category) để áp dụng bộ lọc (ví dụ: `OST_Sections`).  
  - `filter`: Bộ lọc quy tắc, như `ElementParameterFilter`.  
- **FilterRule**: Tạo bằng `ParameterFilterRuleFactory.CreateContainsRule(ElementId parameter, string value, bool caseSensitive)` để kiểm tra chuỗi trong tham số (ví dụ: `VIEW_NAME`).  

**Tạo lệnh ViewFilter**:  
1. **Tạo lớp lệnh**:  
   - Tạo lớp `ViewFilter.cs`, triển khai `IExternalCommand`.  
   - Đặt thuộc tính `[Transaction(TransactionMode.Manual)]` để cho phép thay đổi mô hình.  
   - Thêm namespace: `Autodesk.Revit.DB`, `Autodesk.Revit.UI`, `Autodesk.Revit.Attributes`, `System.Collections.Generic`.  

2. **Tạo danh sách danh mục**:  
   - Tạo danh sách `IList<ElementId>` để chứa danh mục `OST_Sections`:  
     - `IList<ElementId> cats = new List<ElementId>();`.  
     - Thêm ID danh mục: `cats.Add(new ElementId(BuiltInCategory.OST_Sections));`.  

3. **Tạo FilterRule**:  
   - Tạo quy tắc lọc để kiểm tra tham số `VIEW_NAME` chứa “WIP”:  
     - `FilterRule rule = ParameterFilterRuleFactory.CreateContainsRule(new ElementId(BuiltInParameter.VIEW_NAME), "WIP", false);`.  
     - `false`: Không phân biệt chữ hoa/thường.  

4. **Tạo ElementParameterFilter**:  
   - Tạo bộ lọc tham số:  
     - `ElementParameterFilter filter = new ElementParameterFilter(rule);`.  

5. **Tạo ParameterFilterElement**:  
   - Trong khối `using (Transaction trans = new Transaction(doc, "Create Filter"))`:  
     - Gọi `trans.Start()`.  
     - Tạo bộ lọc:  
       - `ParameterFilterElement filterElement = ParameterFilterElement.Create(doc, "My First Filter", cats, filter);`.  

6. **Áp dụng bộ lọc vào khung nhìn**:  
   - Lấy khung nhìn hiện tại: `View activeView = doc.ActiveView;`.  
   - Thêm bộ lọc: `activeView.AddFilter(filterElement.Id);`.  
   - Ẩn các phần tử khớp bộ lọc: `activeView.SetFilterVisibility(filterElement.Id, false);`.  

**Mã nguồn mẫu**:  
```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Collections.Generic;

namespace MyRevitCommands
{
    [Transaction(TransactionMode.Manual)]
    public class ViewFilter : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            try
            {
                // Tạo danh sách danh mục
                IList<ElementId> cats = new List<ElementId>();
                cats.Add(new ElementId(BuiltInCategory.OST_Sections));

                // Tạo FilterRule
                FilterRule rule = ParameterFilterRuleFactory.CreateContainsRule(
                    new ElementId(BuiltInParameter.VIEW_NAME), "WIP", false);

                // Tạo ElementParameterFilter
                ElementParameterFilter filter = new ElementParameterFilter(rule);

                // Tạo và áp dụng ParameterFilterElement
                using (Transaction trans = new Transaction(doc, "Create Filter"))
                {
                    trans.Start();
                    ParameterFilterElement filterElement = ParameterFilterElement.Create(doc, "My First Filter", cats, filter);
                    View activeView = doc.ActiveView;
                    activeView.AddFilter(filterElement.Id);
                    activeView.SetFilterVisibility(filterElement.Id, false);
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

**Cập nhật tệp manifest (.addin)**:  
- Mở `MyRevitCommands.addin`, sao chép khối `<AddIn>` từ lệnh trước.  
- Cập nhật:  
  - `<AddInId>`: Tạo GUID mới (**Tools > Create GUID**).  
  - `<FullClassName>`: `MyRevitCommands.ViewFilter`.  
  - `<Name>` và `<Text>`: `ViewFilter`.  
  - `<Description>`: “Creates a filter to hide section views with 'WIP' in their name”.  

**Kiểm tra lệnh**:  
1. Nhấn **Start** trong Visual Studio để chạy Debug.  
2. Mở Revit, mở tệp dự án từ thư mục bài tập (có ít nhất 4 mặt cắt, 2 trong số đó có tên chứa “WIP”).  
3. Vào **Add-Ins > External Tools > ViewFilter`, chạy lệnh.  
4. Kiểm tra:  
   - Các mặt cắt có tên chứa “WIP” sẽ bị ẩn trong khung nhìn hiện tại.  
   - Vào **Visibility/Graphics Overrides (VV)**, tab **Filters**, kiểm tra bộ lọc “My First Filter” đã được thêm.  
5. **Lưu ý**: Nếu không có mặt cắt nào chứa “WIP” hoặc danh mục `OST_Sections` không tồn tại, bộ lọc vẫn được tạo nhưng không ảnh hưởng.

**Mục tiêu**  
- Hiểu cách tạo bộ lọc dựa trên tham số bằng `ParameterFilterElement`.  
- Sử dụng `ParameterFilterRuleFactory` để tạo quy tắc lọc chuỗi (`CreateContainsRule`).  
- Áp dụng và điều chỉnh hiển thị bộ lọc trong khung nhìn bằng `AddFilter` và `SetFilterVisibility`.  

**Ứng dụng**: Tự động hóa việc tạo và áp dụng bộ lọc để quản lý hiển thị phần tử (ví dụ: ẩn các khung nhìn đang trong quá trình xử lý – “WIP”). Trong các phần tiếp theo, bạn sẽ học cách gắn thẻ (tagging) hoặc tạo tờ (sheets) để hoàn thiện tài liệu thiết kế.