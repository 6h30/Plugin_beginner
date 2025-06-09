kịch bản video hướng dẫn từng bước — trình bày dạng video tutorial có thoại, từng bước rõ ràng, kết hợp phần hình ảnh minh họa , với nội dung sau: 
1-4-
Adding the API
Selecting transcript lines in this section will navigate to timestamp in the video
- [Narrator] We now have our Revit plugin project set up within Visual Studio. Currently, we simply have a class which doesn't allow us to access the Revit API. First, we'll need to add the Revit API files to our project so that we can then access the different classes and types contained within it. To do that, go ahead and expand the references tab on the right in the solution explorer. This shows us all the references, or assemblies, available within our project. To add more, right click references, and select add reference. Here we can find all the standard, available assemblies to add to our project. As the Revit API files aren't standard, we'll need to add them by going to the browse button on the bottom right. The Revit API files are located in the install directory of Revit. So this may differ, depending on where you installed it and what Revit version you're using. I have mine saved in the default file path which is in C:program files, Autodesk, and then Revit 2019. This will contain the Revit API and RevitAPIUI.dil files we'll need to add. So let's scroll down to search for Revit API. Perfect. Select that and then hold down control and left click RevitAPIUI.dil. Then to add those, simply click add. This will add both of these to the browse tab in our references menu. Make sure that both of these are selected in the tick boxes to the left and click OK. Great, so you've see both of those pop up within the references tab in the solution explorer. Select the Revit API and in the properties window in the bottom right, we want to change copy local from true to false. And do the same for the RevitAPIUI. We do this because we don't need to copy these dil files when we compile our code. Revit will already have a reference to both of these files when it boots up. So we can now search through the Revit API as we did the CHM file. To do this, simply double click the Revit API file. This will bring up the object browser. This window lets us look through the classes and other types found in the different assemblies in our project. So let's go ahead and expand Revit API and we can see all the different name spaces contained in there. As you can see, I've got the Autodesk.Revit.DBnamespace expanded and here are all the classes that we saw earlier in the CHM file. The great thing about the object browser is we can easily search for things we need. For example, let's search for element to find the element class. Perfect, so you can see that it's found it at the top. Once you have that selected, you can see all the methods and properties associated with that class on the top right window. Then the documentation or description associated with that class in the bottom right. If we select a method in the top right, you can see that we get the description in the bottom right. I'll be referring to this quite a lot through the course to explain the different classes and types within the Revit API. So next, let's add references to the name spaces we need in the class within our project. Switch back to class one on the top left tab. We'll start by adding a few name spaces to this class which we will regularly access during the course. The first one that I've already mentioned is the Autodesk.Revit.dbnamespace. As this provides access to all the database element classes. So let's add using Autodesk.Revit.db. Next, let's add the UI name space as this provides access to the application which we'll need to use in the following videos. Let's add using Autodesk.Revit.UI. Lastly, let's add the attributes name space from Autodesk.Revit. We'll learn more about the attributes names space in the next video. Now that we have the name spaces within our class, we can go ahead and start implementing our first Revit command.

---

## 🎬 **Kịch bản bài học 1-4: Thêm thư viện Revit API vào project**

> **Thời lượng gợi ý:** 5–6 phút
> **Định dạng:** Video quay màn hình + lời giảng hướng dẫn từng bước

---

### 📌 **Cảnh mở đầu: Giới thiệu nội dung**

**\[Instructor – giọng đều, chắc chắn]**

> Trong bài học trước, chúng ta đã tạo thành công một project C# để lập trình plugin cho Revit.
> Tuy nhiên, hiện tại project này chưa thể truy cập được vào Revit API.
> Trong bài học này, chúng ta sẽ **thêm thư viện Revit API vào project** để sử dụng các class và method từ Revit.

---

### 📌 **Cảnh 1: Thêm RevitAPI.dll và RevitAPIUI.dll**

**\[Instructor – quay màn hình Visual Studio, mở Solution Explorer]**

> Trước tiên, mở phần **Solution Explorer** bên phải và tìm mục **References**.
> Click chuột phải vào **References** ➝ chọn **Add Reference…**

**\[Hiển thị hộp thoại “Reference Manager”]**

> Trong cửa sổ này, các thư viện .NET mặc định sẽ hiển thị.
> Tuy nhiên, **Revit API không có sẵn ở đây**, vì vậy ta cần **duyệt thủ công** tới nơi lưu file.

> Nhấp nút **Browse** ở góc dưới bên phải.

---

### 📌 **Cảnh 2: Tìm đến thư mục chứa Revit API**

> Đường dẫn mặc định tới Revit API thường là:

```
C:\Program Files\Autodesk\Revit 2019\
```

> Tại đây, tìm 2 file sau:

* ✅ `RevitAPI.dll`
* ✅ `RevitAPIUI.dll`

> Nhấn giữ **Ctrl** để chọn cả 2, sau đó nhấp **Add**.

> Bạn sẽ thấy cả hai file xuất hiện trong tab **Browse**. Đảm bảo cả hai đều được tick, rồi nhấn **OK**.

---

### 📌 **Cảnh 3: Cấu hình lại thuộc tính "Copy Local"**

**\[Instructor – trở lại Solution Explorer]**

> Bây giờ cả `RevitAPI` và `RevitAPIUI` đã xuất hiện trong mục **References**.
> Chọn từng file một, kiểm tra thuộc tính **Copy Local** trong **Properties** (góc dưới phải):

* Đặt **Copy Local = False** cho cả hai.

> ✅ Vì Revit đã tự động nạp các thư viện này khi chạy, chúng ta **không cần copy** chúng vào build output nữa.

---

### 📌 **Cảnh 4: Khám phá Revit API qua Object Browser**

**\[Instructor – mở Object Browser]**

> Để tìm hiểu nhanh các class và method trong API, nhấn **F12** hoặc vào **View > Object Browser**.

> Tại đây, mở rộng mục **RevitAPI**, sau đó mở tiếp **Autodesk.Revit.DB** — namespace quan trọng nhất của API.

> Bạn sẽ thấy các class như:

* `Element`
* `Wall`
* `Document`
* `View`
* ...

> Có thể tìm nhanh bằng cách gõ "Element" vào thanh tìm kiếm.

> Mỗi class đều có phần mô tả và danh sách method / property tương ứng.

---

### 📌 **Cảnh 5: Thêm namespace vào mã nguồn**

**\[Instructor – mở lại Class1.cs]**

> Bây giờ, ta sẽ **import các namespace cần thiết** để bắt đầu viết lệnh Revit.
> Thêm vào đầu file những dòng sau:

```csharp
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
```

> * `DB` là nơi chứa hầu hết các class làm việc với đối tượng Revit
> * `UI` dùng để tương tác với giao diện Revit (ví dụ như hiển thị hộp thoại)
> * `Attributes` chứa các thuộc tính đặc biệt (như đánh dấu một class là lệnh Revit)

---

### 📌 **Cảnh kết: Tổng kết và hướng dẫn tiếp theo**

**\[Instructor – tông giọng tích cực, khích lệ]**

> 🎉 Hoàn hảo! Chúng ta vừa thêm thành công thư viện Revit API vào project và cấu hình đầy đủ.
> Bây giờ chúng ta đã có thể bắt đầu viết **lệnh plugin đầu tiên cho Revit**!

> Trong bài học tiếp theo, tôi sẽ hướng dẫn bạn **viết class lệnh đầu tiên** và chạy thử plugin trong Revit.

---

### ✅ **Checklist cuối bài:**

* [x] Thêm `RevitAPI.dll` và `RevitAPIUI.dll` vào project
* [x] Cấu hình Copy Local = False
* [x] Tìm hiểu API qua Object Browser
* [x] Thêm namespace cần thiết (`DB`, `UI`, `Attributes`)
* [x] Sẵn sàng viết lệnh đầu tiên!

---

📦 Nếu bạn muốn, mình có thể:

* Cung cấp **mẫu mã nguồn Class1.cs** đã có namespace và cấu trúc sẵn
* Soạn **bài tiếp theo 1-5: Tạo lệnh Revit đầu tiên**
* Tạo **file video mẫu thao tác thực tế** nếu bạn muốn dùng cho khóa học

Bạn muốn tiếp tục với phần **viết lệnh đầu tiên trong Revit** chứ?

