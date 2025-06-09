kịch bản video hướng dẫn từng bước — trình bày dạng video tutorial có thoại, từng bước rõ ràng, kết hợp phần hình ảnh minh họa , với nội dung sau: 
1-2 Viewing the Revit API 
Selecting transcript lines in this section will navigate to timestamp in the video
- [Instructor] There are a few different ways to view the Revit API. One simple way is to install the SDK, or software development kit, when installing Revit, to view the CHM file that comes with it. To install the SDK, you'll need access to the standard Revit installer. If you don't have this, you can download it from the Autodesk website. Once downloaded, we have access to the Revit install menu. Go to the Install Tools and Utilities option. Here, we can install some additional items for Revit. Let's install the Revit Software Development Kit. Make sure that's selected, and go ahead and click install. Once the Revit video finishes downloading the SDK, it will request a file path location to save the SDK to. I'm leaving mine as default, which is a Revit SDK 2019 file path on the C drive. Then click install. Perfect, then click finish. Once the SDK is finished installing, navigate to the folder path the SDK's installed to. I've left mine as default, so it's on the C drive, and then Revit SDK 2019. Open this folder, and you'll find files and folders related to developing with Revit. This includes sample files, getting started guides, and the RevitAPI.CHM file at the bottom. Go ahead and double-click this to open it up. The CHM file is what is known as a Compiled HTML file, which is Microsoft's help file format. This CHM file provides with the documentation for the Revit API file. It lets us browse through namespaces, classes, and prototypes. If you aren't familiar with the term namespace, in programming, a namespace is a way to group related objects, to organize the development code. This allows for classes or interfaces, for example, to have shared names across different namespaces, as they derive from different namespaces. For the majority of this course, we'll be dealing with the Autodesk.Revit.DB namespace. Which you can see contains a variety of sub-namespaces. Let's select the Autodesk.Revit.DB to view its contents. In this namespace, you'll find a variety of classes, interfaces, and numerations, which are used to access Revit and work with its elements, geometry, and project information. Throughout the course, we'll spend a lot of time exploring and using type from this namespace, so it's worth getting familiar with it. You'll notice a lot of the names are familiar if you work in Revit already, such as ElevationMarker. This class, for example, can be used to create an elevation marker. The RevitAPI.CHM is a great way to view the contents of the Revit API, and learn about the different areas of Revit, that we can access through its API. Now that we know about the Revit API, we can start building our first polyline.

---

## 🎬 **Kịch bản bài học 1-2: Xem và tra cứu Revit API (Viewing the Revit API)**

> **Thời lượng gợi ý:** 4–6 phút
> **Định dạng:** Video hướng dẫn + chia sẻ màn hình + giải thích kỹ thuật

---

### 📌 **Cảnh mở đầu: Giới thiệu bài học**

**\[Instructor – giọng thân thiện, rõ ràng]**

> Ở bài học trước, bạn đã hiểu API là gì và vai trò của nó trong Revit.
> Trong bài này, chúng ta sẽ học cách **xem và tra cứu tài liệu Revit API**, một kỹ năng quan trọng để lập trình plugin hiệu quả.

---

### 📌 **Cảnh 1: Giới thiệu cách xem Revit API**

**\[Instructor]**

> Có nhiều cách để tra cứu API, nhưng cách đơn giản nhất là thông qua **Revit SDK – Software Development Kit**.
> SDK chứa tài liệu, ví dụ mã, và file hướng dẫn cực kỳ hữu ích.

---

### 📌 **Cảnh 2: Hướng dẫn cài đặt Revit SDK**

**\[Chèn màn hình thật hoặc mô phỏng trình cài đặt Revit]**

**\[Instructor hướng dẫn từng bước]**

1. **Mở trình cài đặt Revit (Revit Installer)**
2. Chọn **"Install Tools and Utilities"**
3. Tích chọn **"Revit Software Development Kit" (Revit SDK)**
4. Click **Install**
5. Khi được hỏi vị trí lưu, bạn có thể để mặc định (ví dụ: `C:\Revit SDK 2019`)
6. Click **Install**, rồi **Finish**

---

### 📌 **Cảnh 3: Truy cập tài liệu API**

**\[Instructor]**

> Sau khi cài xong, truy cập thư mục cài SDK:

**\[Chèn đường dẫn trên Windows Explorer]**

```
C:\Revit SDK 2019\
```

**\[Instructor tiếp tục]**

> Ở đây, bạn sẽ thấy rất nhiều tài liệu và ví dụ thực tế.
> Nhưng file quan trọng nhất chính là:

🟩 **RevitAPI.CHM**

> Hãy double-click để mở!

---

### 📌 **Cảnh 4: Giới thiệu RevitAPI.CHM**

**\[Instructor]**

> File `.CHM` là tài liệu HTML đã được biên dịch – đây là dạng phổ biến của các file trợ giúp trong hệ thống Windows.

**\[Hiển thị giao diện CHM mở ra]**

> Bạn có thể duyệt qua:

* **Namespaces** (Không gian tên)
* **Classes** (Lớp)
* **Interfaces** (Giao diện)
* **Enumerations** (Hằng số liệt kê)

---

### 📌 **Cảnh 5: Khái niệm Namespace là gì?**

**\[Instructor]**

> Nếu bạn chưa quen với lập trình, **Namespace** là cách nhóm các lớp và đối tượng liên quan lại với nhau.
> Ví dụ: hai lớp cùng tên nhưng khác namespace vẫn có thể cùng tồn tại.

🧠 **Mẹo ghi nhớ:** Think of namespaces as folders that help organize code.

---

### 📌 **Cảnh 6: Namespace quan trọng nhất – Autodesk.Revit.DB**

**\[Instructor]**

> Trong suốt khóa học, chúng ta sẽ làm việc chủ yếu với namespace:

🟦 **Autodesk.Revit.DB**

**\[Chèn hình minh họa: cây thư mục trong CHM hiển thị Autodesk.Revit.DB]**

> Đây là nơi chứa hầu hết các lớp và công cụ để truy cập **element, geometry, thông tin dự án**…

**Ví dụ:**

* `Wall` – Tường
* `ElementId` – Mã định danh phần tử
* `ElevationMarker` – Dấu độ cao

---

### 📌 **Cảnh 7: Tổng kết và định hướng**

**\[Instructor – tổng kết]**

> ✅ Revit SDK là kho tài nguyên quý giá
> ✅ RevitAPI.CHM giúp bạn tra cứu cấu trúc API, các class và namespace
> ✅ Namespace `Autodesk.Revit.DB` là trung tâm của lập trình Revit

> Trong bài học tiếp theo, chúng ta sẽ bắt đầu **xây dựng plugin đầu tiên** với C#!

---

### ✅ **Checklist cuối video**

* [x] Cài đặt Revit SDK thành công
* [x] Truy cập được file `RevitAPI.CHM`
* [x] Hiểu cách sử dụng namespace, đặc biệt là `Autodesk.Revit.DB`
* [x] Sẵn sàng viết dòng code đầu tiên!

---

Bạn có muốn mình:

* Thiết kế slide bài này (PowerPoint hoặc Canva)?
* Tạo sơ đồ cây namespace minh họa?
* Hoặc viết script plugin đầu tiên tương ứng cho bài 1-3?

Bạn chỉ cần nói rõ, mình sẽ hỗ trợ nhanh chóng.
