# WPF DataGrid Drag & Drop Effect

This project is about demonstrating the drag & drop effect which you can achieve with simple wpf datagrid. There are two demonstrating effect which has been done here:
### 1. Enable Drag & Drop for grid
In my case I have enabled for multi select rows and allowed for multiple rows drag & drop, here are the events to play with:
i. Enable AllowDrop="True" and SelectionMode="Extended" for the grid.
ii. Add LoadingRow event for dragging rows and indicator line effects. Below mentioned effects code is been handled from this event.
iii. Add SelectionChanged event to mark selected row. This event will set Model.IsSelected to true through which we identify this row(s) are selected.
iv. Add Drop event and the code related to dropping rows goes here, this is where collection source gets modified.
***Note: There are other DataGrid events like DragEnter, DragOver and DragLeave, which you can use based on your need.***

### 2. Show the selected rows while dragging
selected rows are shown in a popup while dragging over. This is achieved by adding a popup and following steps:
i. A container control to show data (selected rows). A DataGrid here.
ii. A popup which holds the container control.
iii. On Row_DragOver event binding the container control and displaying the popup on current mouse position and otherwise hide the popup.

### 3. Show the row indicator line for drop location
drop location is indicated with a blue highligted line which tells where exactly the selectes rows will be placed.To achieve this we need to figure out the drop location based on our data collection index position and then dynamically apply the styling to draw a Row Indicator line and this is achieved by:
 i. Creating an enum RowEffect which will help to hold the info for drag position.
 ii. Adding the in Person model.
 iii. Based on the datagrid's data collection change(i.e remove item and then add the dragged back at respective index), setting the value of RowEffect for Person model On Row_DragOver event.
 iv. Finally Overridding the DataGrid.RowStyle and change the style of the target row's border based on your Person.RowEffect value.

***Note: This is a prototype to help you on how to achieve the effect so you have to do your own customization or work to achieve what you need, if you need to. Also the cursor is missing in result image as I missed while taking the snapshot but it is there. ***
*** Use ctrl for multi select and then ctrl+shift to start dragging multiple selected rows. This behavior your can change based on your code logic.***

