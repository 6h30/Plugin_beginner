
---

## Question 1 of 12

**Which method for a Document object can be used to retrieve an element from its ElementId?**

* ❌ **RetrieveElement**
  *Incorrect – RetrieveElement is not a method in the Revit API.*

* ❌ pickedObj

* ✅ **GetElement**
  *Correct – The `GetElement` method can be accessed from the Document and provides a way to retrieve an element using its ElementId.*

* ❌ PickObject

---

## Question 2 of 12

**The Category to which an element belongs can be retrieved from an element by using the GetElementCategory method from the element object.**

* ❌ **TRUE**
  *Incorrect – The `Element` class does not have a method named `GetElementCategory`. The `Category` is retrieved by accessing the `Category` property.*

* ✅ **FALSE**

---

## Question 3 of 12

**Which of the following filters cannot be applied to a FilteredElementCollector?**

* ❌ **ElementQuickFilter**
  *Incorrect – ElementQuickFilters can be applied to a FilteredElementCollector.*

* ✅ **ElementFastFilter**
  *Correct – There is no `ElementFastFilter` in the Revit API.*

* ❌ **ElementSlowFilter**

* ❌ **ElementLogicalFilter**

---

## Question 4 of 12

**What is one way of speeding up the results of a FilteredElementCollector?**

* ✅ **by combining ElementQuickFilters**
  *Correct – Combining quick filters reduces memory expansion and speeds up performance.*

* ❌ by retrieving the elements as ElementIds

* ❌ by adding two FilteredElementCollectors together
  *Incorrect – They cannot be combined this way.*

* ❌ by adding at least two slow filters

---

## Question 5 of 12

**After Revit elements are edited in a Revit plugin, how are the changes committed to the model?**

* ✅ **by calling the Commit method from the current transaction**
  *Correct – The `Commit` method finalizes all changes within the transaction.*

* ❌ by calling the Rollback method from the current transaction

* ❌ by calling the Start method from the current transaction

* ❌ by returning a result from the method

---

## Question 6 of 12

**To reverse any changes made to the model in a transaction, which method can be used from the transaction object?**

* ✅ **Rollback**
  *Correct – `Rollback` reverses all changes made in the transaction.*

* ❌ Start

* ❌ Commit

* ❌ Cancel

---

## Question 7 of 12

**How can the name of a family type be retrieved in the Revit API?**

* ✅ **by accessing the Name property from a family symbol element**
  *Correct – The `Name` property of a `FamilySymbol` gives the name of the type.*

* ❌ by accessing the Name property from a Category

* ❌ by accessing the FamilyName property from a Category

* ❌ by accessing the FamilyName property from a family symbol

---

## Question 8 of 12

**LINQ queries are useful for \_\_\_\_\_.**

* ✅ **filtering data with the use of query operators**
  *Correct – LINQ allows querying data with expressive syntax.*

* ❌ compiling code

* ❌ querying the manifest file
  *Incorrect – LINQ is not used for manifest files.*

* ❌ adding references to a command

---

## Question 9 of 12

**Line based families are those that \_\_\_\_\_.**

* ❌ are created using line geometry created from the Revit API

* ❌ are created with at least 3 lines from the Revit API
  *Incorrect – There is no minimum of 3 lines.*

* ❌ are the only way to place elements in the Revit API

* ❌ are placed using XYZ objects from the Revit API
  *Incorrect – Point-based families use XYZ; not line-based.*

---

## Question 10 of 12

**What type of object is the CurveLoop used for?**

* ✅ **Storing a collection of sequential curves.**
  *Correct – CurveLoop is used to store an ordered collection of `Curve` objects.*

* ❌ Storing a collection of points that define curves.

* ❌ Storing a single curve.

* ❌ Storing a collection of random curves.

---

## Question 11 of 12

**Which method is best for getting all the parameters associated with an element?**

* ❌ Parameter

* ❌ LookupParameter

* ✅ **Parameters**
  *Correct – The `Parameters` property returns all parameters of an element.*

* ❌ GetAllParameters

---

## Question 12 of 12

**What information does the StorageType property on a parameter object store?**

* ❌ The value that a parameter stores.

* ❌ The number of parameters in the project.

* ✅ **The value type that a parameter stores.**
  *Correct – `StorageType` returns an enum that describes the type of value the parameter holds (e.g., Integer, String).*

* ❌ The name of a parameter.

---

