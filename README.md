# Access-GearVR-Controller-from-PC


This is a c# .NET project under Windows 10 that aims to communicate with GearVR controller and read data. Currently can read buttons and axis. Still need properly determine rest of data to get orientation, speed, acceleration data although some initial attempt is alreadty done. 

Project contains code that works 100% in my - very big ;) - project however after I extracted this to this sample I get ystem.UnauthorizedAccessException during execution. So you might want to try it or wait until I double check this project. Issue is more general and tracked here:
https://social.msdn.microsoft.com/Forums/en-US/58da3fdb-a0e1-4161-8af3-778b6839f4e1/

Project imports UWP libs into classic Windows Form project.


