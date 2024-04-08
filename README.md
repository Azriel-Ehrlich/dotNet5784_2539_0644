# Tasks & Engineers Management System
_by Azriel Ehrlich and Ariel Halili_

### What is this?
This project is a management system for tasks and engineers. Using this app allow you to create list of tasks and assign an engineer for each task. Finally, you can see a gantt chart of your full project.

This project created during "Mini Project in Windows Systems" course at JCT.

> NOTE: This app uses .NET 8 

### Special additions - bonuses
* Awesome graphics (you can see it...)
* Automatic schedule (BL/BlImplementation/Bl.cs - `MakeSuggestedDates`)
* Icons for windows (PL/MainWindow.xaml)
* Restore deleted tasks and engineers (BL/BlImplementation/TaskImplementation.cs BL/BlImplementation/EngineerImplementation.cs - `Delete` and `Restore`)
* Saving the clock between runs (so you can continue from the last time) (BL/BlImplementation/ClockImplementation.cs - `SaveClock`)
* Use TryParse to validate input (DalText/Program.cs BlTest/Program.cs)
* Implement ToStringProperty (BL/BO/Tools.cs `ToStringProperty`)
* System clock in the background - the clock in main window advances automatically (BL/BlImplementation/ClockImplementation.cs `StartClockThread`)
* Fully Lazy Singleton (DalXml/DalXml.cs DalList/DalList.cs `lazy`, `Instance`)
* Data Triggers (PL/App.xaml `ListViewItem`, PL/Manager/ManagerWindow.xaml `StarToggleButton`)
* Use of shapes (PL/Manager/ManagerWindow.xaml `StarToggleButton`)
* Division of users and entry into the system according to current engineer (PL/MainWindow.xaml - buttons for manager and engineers) 
* Amazing README file (README.md)

### Screenshot
<img src="screenshot.png" alt="screenshot of the opening screen" height="500"/>
