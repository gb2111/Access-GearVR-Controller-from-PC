# Access-GearVR-Controller-from-PC


It aims to communicate with GearVR controller and read data. I was able to read buttons and axis. Still need decode rest of data to get orientation, speed, acceleration data although some initial attempt was done. Data is already displayed as bits on the sample form.

Project contains code that works 100% in my (very big) project however after I extracted this to this sample I get ystem.UnauthorizedAccessException during execution. Currently I leave it as it is. Issue is more general and tracked here:
https://social.msdn.microsoft.com/Forums/en-US/58da3fdb-a0e1-4161-8af3-778b6839f4e1/

Project imports UWP libs into classic Windows Form project.


