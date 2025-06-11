
## **Kịch bản bài học 1-1: Giới thiệu Revit API**

> **Thời lượng 3–5 phút**
> **Định dạng:** Hội thoại + thuyết minh + mô phỏng hình ảnh

---

### **Cảnh mở đầu: Giới thiệu**

**\[Instructor - Giọng nhẹ nhàng, rõ ràng]**

> Xin chào! Trong bài học đầu tiên này, chúng ta sẽ tìm hiểu về **Revit API** – công cụ chính giúp bạn tạo các plugin tùy chỉnh cho Revit.

---

### **Cảnh 1: API là gì?**

**\[Instructor]**

> Vậy Revit API là gì?
> API – hay “Application Programming Interface” – là một cầu nối giúp hai phần mềm giao tiếp với nhau.

**\[Chèn hình minh họa]**

* Một phần mềm A (Revit plugin)
* Gửi yêu cầu qua API
* API chuyển tiếp yêu cầu đến phần mềm B (Revit)

**\[Instructor tiếp tục]**

> Trong trường hợp của chúng ta, Revit API cho phép bạn **gửi lệnh** đến Revit và **nhận lại kết quả**, như là tạo một bức tường, truy xuất thông tin cửa sổ, hoặc chỉnh sửa thông số mô hình.

---

### **Cảnh 2: Ví dụ thực tế – API như người bồi bàn**

**\[Instructor]**

> Hãy tưởng tượng bạn đang trong một nhà hàng.

**\[Chèn hình minh họa vui nhộn: khách hàng, bồi bàn, bếp]**

**\[Instructor tiếp tục]**

> Bạn – người dùng – không thể tự mình vào bếp để lấy món ăn.
> Nhưng bạn có thực đơn. Bạn chọn món, và gọi bồi bàn.

> Bồi bàn – chính là API – sẽ truyền yêu cầu đến nhà bếp (Revit), và mang món ăn ra cho bạn (kết quả hoặc đối tượng Revit).

---

### **Cảnh 3: Các file API chính trong Revit**

**\[Instructor]**

> Vậy, Revit API xuất hiện dưới dạng các **file thư viện (DLL)** – chúng ta sẽ sử dụng hai file chính:

1. **RevitAPI.dll**
   ➤ Truy cập dữ liệu bên trong Revit: tài liệu, đối tượng, thông số kỹ thuật.
   ➤ Ví dụ: truy cập lớp `Wall` để tạo hoặc chỉnh sửa tường.

2. **RevitAPIUI.dll**
   ➤ Cho phép tùy biến **giao diện người dùng** của Revit: như tạo nút bấm, ribbon, menu tùy chỉnh.

**\[Chèn hình minh họa cây thư mục cài đặt Revit có chứa 2 file trên]**

**\[Instructor tiếp tục]**

> Ngoài ra còn có một vài file mở rộng, nhưng chúng ta sẽ **chỉ tập trung vào hai file chính này** trong khóa học.

---

### **Cảnh 4: Ngôn ngữ lập trình sử dụng**

**\[Instructor]**

> Revit API được xây dựng trên nền tảng **.NET Framework**, vì thế bạn có thể sử dụng các ngôn ngữ như **C# hoặc VB.NET**.
> Trong khóa học này, chúng ta sẽ dùng **C#** – vì đây là ngôn ngữ phổ biến, dễ học và mạnh mẽ.

**\[Hiển thị đoạn mã C# đơn giản]**

```csharp
Wall wall = doc.Create.NewWall(...);
```

---

### **Cảnh 5: Kết thúc – Mục tiêu của bạn là gì?**

**\[Instructor – tổng kết]**

> Qua API, bạn sẽ có khả năng **tự động hóa** các tác vụ, tạo **plugin tùy chỉnh**, và **tối ưu hóa quy trình thiết kế** trong Revit.

> Trong những bài học tiếp theo, chúng ta sẽ bắt đầu thiết lập môi trường lập trình và viết plugin đầu tiên của bạn!

---

### **Checklist cuối video (hiển thị dưới dạng bullet point)**

* [x] Hiểu Revit API là gì
* [x] Biết sự khác nhau giữa `RevitAPI.dll` và `RevitAPIUI.dll`
* [x] Nắm được vai trò của C# trong việc lập trình plugin
* [x] Sẵn sàng cho bài học tiếp theo!

