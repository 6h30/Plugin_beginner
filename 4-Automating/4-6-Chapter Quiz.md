Question 1 of 4
How is a filter added to a view?

❌ By accessing the ViewFilter property on the view.
Incorrect – There is no ViewFilter property on a view object.

❌ By calling the AddViewFilter method contained in the view object.
Incorrect – There is no AddViewFilter method on a view object.

✅ By calling the AddFilter method contained in the view object.
Correct – This is the correct method for adding a filter to a view.

❌ By accessing the Filters property on the Document.
Incorrect – The Document object does not have a Filters property.

Question 2 of 4
When creating a new tag element with the Revit API, the tag orientation can be set to Horizontal or Vertical. Is this statement true?

✅ TRUE
Correct – One of the parameters in the IndependentTag.Create method allows you to set the tag orientation to horizontal or vertical.

❌ FALSE
Incorrect – The tag orientation can be set via the API.

Question 3 of 4
Which view object is created when creating a new sheet?

✅ ViewSheet
Correct – ViewSheet is the object returned when creating a new sheet in Revit.

❌ ViewPlan
Incorrect – A ViewPlan is used for plan views, not sheets.

❌ SheetView
Incorrect – There is no SheetView object in the Revit API.

❌ ViewSheetPlan
Incorrect – There is no ViewSheetPlan object.

Question 4 of 4
A view can be placed on a viewsheet by _____.

❌ using the PlaceView method on the viewsheet object
Incorrect – There is no PlaceView method on the ViewSheet object.

❌ assigning the viewsheet to the sheet property on the view
Incorrect – There is no Sheet property on a view object.

❌ specifying the view when creating the viewsheet
Incorrect – The view is not specified during ViewSheet creation.

✅ specifying the view and view sheet when creating a Viewport
Correct – To place a view on a sheet, you create a Viewport and provide both the View and ViewSheet.
