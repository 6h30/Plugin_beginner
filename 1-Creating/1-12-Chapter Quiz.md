
---

## Question 1 of 11

**An API allows one piece of software to communicate with another.**

* ✅ **TRUE**
  *Correct – An API provides access to the classes, functions, and variables that allow one piece of software to communicate with another.*

* ❌ FALSE

---

## Question 2 of 11

**Which of the following files provides a method to view the Revit API?**

* ✅ **RevitAPI.chm**
  *Correct – The RevitAPI.chm file is provided with the Revit SDK and is a documentation file for the Revit API.*

* ❌ Revit.dll
  *Incorrect – This is not an actual file and therefore cannot be used to view the Revit API.*

* ❌ RevitAPI.API
  *Incorrect – This is not an actual file and therefore cannot be used to view the Revit API.*

* ❌ Revit.chm

---

## Question 3 of 11

**From the following options, which framework do Revit plugins target?**

* ❌ Cocoa

* ❌ Revit

* ✅ **.Net**
  *Correct – Revit supports plugins that target the .Net framework. Which Revit version is being used impacts which version of the .Net framework the plugin needs to target.*

* ❌ ASP .Net

---

## Question 4 of 11

**How is the Revit API DB namespace accessed from a solution in Visual Studio?**

* ❌ by opening Revit.exe

* ✅ **by referencing the RevitAPI.dll file in the solution explorer**
  *Correct – When the RevitAPI.dll file is referenced into the solution, the namespaces from that assembly can be imported.*

* ❌ by referencing the Revit.exe file in the solution explorer

* ❌ by referencing the RevitAPI.chm file in the solution explorer
  *Incorrect – The RevitAPI.chm file cannot be used to access the RevitAPI namespaces in Visual Studio.*

---

## Question 5 of 11

**All classes that inherit the IExternalCommand interface require which method to be implemented?**

* ❌ Command

* ❌ Commit

* ✅ **Execute**
  *Correct – The Execute method is the only method that the IExternalCommand requires. This will be called when the command is run.*

* ❌ Finish

---

## Question 6 of 11

**When creating a command, what is the one thing that the Execute method needs to return?**

* ❌ An ElementSet

* ❌ An Element
  *Incorrect – The Execute method does not require an element to be returned.*

* ❌ A View

* ✅ **A Result enumeration**
  *Correct – Every Execute method needs to return a result enumeration (Succeeded, Failed, or Cancelled).*

---

## Question 7 of 11

**Which of the Execute method’s parameter types can be used to access the Revit UIDocument object?**

* ✅ **ExternalCommandData**
  *Correct – ExternalCommandData provides access to the application, which gives the ActiveUIDocument.*

* ❌ ElementSet

* ❌ String

* ❌ Document

---

## Question 8 of 11

**The user can be prompted to select an element from a command by calling a method from which class?**

* ❌ Document
* ❌ Element
* ❌ Pick
* ✅ **Selection**
  *Correct – The `Selection` class provides methods like `PickObject` to prompt user interaction.*

---

## Question 9 of 11

**Revit plugins need to be registered with a manifest file that has which file extension?**

* ❌ .mp4

* ❌ .manifest

* ❌ .dll
  *Incorrect – A .dll file is the compiled plugin, not the manifest.*

* ✅ **.addin**
  *Correct – Revit requires plugins to be registered with a `.addin` file.*

---

## Question 10 of 11

**When compiling a Revit plugin, how can the compiled files be automatically copied to the required addin folder?**

* ❌ by using copy paste shortcuts

* ✅ **by adding a Post-Build Event**
  *Correct – Post-Build Events in Visual Studio can automate file copying.*

* ❌ by adding a Pre-Build Event

* ❌ by closing Visual Studio
  *Incorrect – This won’t copy files automatically.*

---

## Question 11 of 11

**We can start debugging our plugin by pressing \_\_\_\_\_.**

* ✅ **Start**
  *Correct – The Start button initiates debugging mode.*

* ❌ Revit

* ❌ References

* ❌ End

---
