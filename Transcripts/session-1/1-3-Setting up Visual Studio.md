kịch bản video hướng dẫn từng bước — trình bày dạng video tutorial có thoại, từng bước rõ ràng, kết hợp phần hình ảnh minh họa , với nội dung sau: 
1-3-Setting up Visual Studio
Selecting transcript lines in this section will navigate to timestamp in the video
- [Instructor] Now that we're familiar with the Revit API, let's start by creating our first Revit plugin. To create Revit plugins, I'll be using the Visual Studio IDE or Integrated Development Environment. If you prefer to use another IDE feel free to do so. However, all videos in this course will utilize Visual Studio. If you need to download Visual Studio, you can do so by navigating to visualstudio.microsoft.com on whatever browser you're using. Here you'll find the option to download Visual Studio IDE by hovering over the download for Windows. This pulls up the different download versions. I'll be using the free version, which is Community 2017. By selecting this, it will run the automatic download for the installer for Visual Studio. Go ahead and save this, and then start the install. When installing Visual Studio, you'll be prompted by this menu. This allows us to select different components that we want installed when installing Visual Studio. The different components allow us to develop four different platforms. As we'll be creating .NET plugins, make sure that the .NET desktop development item is ticked. In the individual components tab, make sure that the .NET Framework 4.7 targeting pack is also selected if you're creating plugins for Revit 2019. I'll explain this in a little bit more detail shortly. Once you have installed Visual Studio and opened up the application, you'll be prompted with this start page. So, let's start by creating a new project, which will be the main project file for our Revit plugins. To create a new project, go to the far menu in the top left and then, new and project. This will bring up all the different project templates. We need to create a C Sharp Plus library targeting the .NET Framework, which simply means a set of libraries that will be accessible throughout plugin. So, to do that, go to the installed tab on the left, Visual C Sharp, and then in the middle column Class Library with .NET Framework in brackets. It is important to note here that depending on what .NET Framework we target impacts the Revit version our plugin will run on. We need to ensure that the Revit API that we are working with is utilizing the same .NET Framework. If we select the .NET Framework tab on the bottom of the window, you can see all the different versions that we can target. This may look a little bit different depending on what Frameworks you have installed. If you are building this to run using the Revit 2017 or Revit 2018 APIs, you'll need to target version 4.6. Earlier versions, such as 2015 and 2016 require version 4.5. These, however, will not compatible with Revit 2019. The 2019 API requires that we build our plugin to target the .NET Framework version 4.7. So, I'm going to select that. If you're finding that it's not showing up in your menu, you can go back to the install window by going to tools, get tools and features, and individual components. Here we can select all the different Frameworks we want installed. After you've done that, restart Visual Studio and try again. Once we have the correct framework targeted, let's name our plugin. I'm going to call mine 'MyRevitCommands' as it will contain all the custom commands we're going to create for Revit. Then, check the solution location. As you can see, I'm saving mine in the C drive, Users\Jeremy\source\repos folder, which is the default file path. However, you can save this wherever you'd like. Then, make sure the create directory for solution on the right is selected and the add to source control is not selected, and go ahead and click okay. Perfect, so we have successfully setup the project, which we'll start to use to create Revit plugins.

---

## 🎬 **Kịch bản bài học 1-3: Thiết lập Visual Studio để lập trình Revit Plugin**

> **Thời lượng gợi ý:** 6–8 phút
> **Định dạng:** Video quay màn hình + lời giảng + giải thích kỹ thuật

---

### 📌 **Cảnh mở đầu: Giới thiệu bài học**

**\[Instructor – giọng rõ ràng, hào hứng]**

> Bây giờ bạn đã làm quen với Revit API, đã đến lúc bắt đầu **tạo plugin Revit đầu tiên**!
> Trong bài này, tôi sẽ hướng dẫn bạn cách cài đặt và thiết lập **Visual Studio** – công cụ lập trình mạnh mẽ, miễn phí và dễ sử dụng.

---

### 📌 **Cảnh 1: Tải và cài đặt Visual Studio**

**\[Instructor]**

> Đầu tiên, hãy truy cập vào:
> 🌐 **[https://visualstudio.microsoft.com/](https://visualstudio.microsoft.com/)**

**\[Chèn màn hình trình duyệt truy cập trang web Visual Studio]**

> Ở đây, rê chuột lên mục **"Download for Windows"**, sau đó chọn phiên bản **Community** – hoàn toàn miễn phí.
> Trong khóa học này, tôi sử dụng bản **Community 2017**, nhưng bạn có thể dùng bản mới hơn nếu muốn.

**\[Hiển thị quá trình tải về file cài đặt `.exe`]**

> Sau khi tải về, mở file lên để bắt đầu cài đặt.

---

### 📌 **Cảnh 2: Chọn thành phần khi cài đặt**

**\[Instructor – quay màn hình cài đặt]**

> Trong quá trình cài đặt, bạn sẽ thấy một bảng chọn các **"Workloads"** – đây là các gói chức năng.

🎯 **Hãy đảm bảo bạn chọn:**

* ✅ **“.NET desktop development”**

> Đây là gói cho phép bạn lập trình ứng dụng .NET – chính là nền tảng của plugin Revit.

**\[Chuyển sang tab “Individual Components”]**

> Trong tab này, hãy đảm bảo tick chọn:

* ✅ **.NET Framework 4.7 Targeting Pack**

> Vì chúng ta đang xây plugin cho **Revit 2019**, vốn yêu cầu Framework 4.7.

---

### 📌 **Cảnh 3: Tạo Project Plugin đầu tiên**

**\[Instructor – mở Visual Studio sau cài đặt]**

> Sau khi cài xong, mở Visual Studio. Bạn sẽ thấy màn hình chào mừng.
> Chọn **"Create a new project"** để bắt đầu.

**\[Hiển thị các bước tạo project]**

1. **Chọn loại project:**
   ➤ Trong mục **Installed > Visual C#**, chọn **Class Library (.NET Framework)**

2. **Đặt tên project:**
   ➤ Tôi sẽ đặt là **MyRevitCommands**
   ➤ Đây sẽ là nơi chứa toàn bộ lệnh plugin Revit mà chúng ta sẽ tạo

3. **Chọn Framework:**
   ➤ Dưới mục **Framework**, chọn **.NET Framework 4.7**

   ❗ **Lưu ý:**

   * Revit 2019 → .NET 4.7
   * Revit 2017–2018 → .NET 4.6
   * Revit 2015–2016 → .NET 4.5
   * ⚠ Không tương thích chéo nếu chọn sai framework!

---

### 📌 **Cảnh 4: Cách xử lý nếu thiếu framework**

**\[Instructor]**

> Nếu không thấy phiên bản 4.7 trong danh sách, bạn có thể:

1. Vào menu **Tools > Get Tools and Features**
2. Chuyển sang tab **Individual Components**
3. Tick chọn **.NET Framework 4.7 Targeting Pack**
4. Sau đó **khởi động lại Visual Studio**

---

### 📌 **Cảnh 5: Kiểm tra đường dẫn và tạo project**

**\[Instructor]**

> Kiểm tra lại đường dẫn lưu dự án.
> Mặc định thường là:

```
C:\Users\<tên người dùng>\source\repos
```

> ✅ Tick chọn **"Create directory for solution"**
> 🚫 Bỏ chọn **"Add to Source Control"** nếu bạn không dùng Git

> Rồi bấm **OK** để tạo project.

---

### 📌 **Cảnh 6: Tổng kết**

**\[Instructor – tổng kết]**

> 🎉 Tuyệt vời! Chúng ta đã tạo thành công một project C# để lập trình plugin cho Revit.
> Trong các bài học tiếp theo, chúng ta sẽ học cách **thêm thư viện Revit API**, tạo **class lệnh**, và thực thi plugin trong Revit.

---

### ✅ **Checklist cuối video**

* [x] Cài đặt Visual Studio thành công
* [x] Chọn đúng .NET Framework phù hợp với phiên bản Revit
* [x] Tạo project Class Library (.NET Framework)
* [x] Sẵn sàng viết plugin đầu tiên!


