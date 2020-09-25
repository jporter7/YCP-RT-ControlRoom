                                               "# YCP-RT-ControlRoom" 

                                        Control Room: Development Environment
                                        
________________________________________________________________________________________________________________________________________
                                                Github Repository
    Website URL: https://github.com/kkennelly/YCP-RT-ControlRoom
    Clone URL: https://github.com/kkennelly/YCP-RT-ControlRoom.git 
    Repository owner: Kate Kennelly  
    Github account: kkennelly 
    E-mail: kkennelly@ycp.edu
________________________________________________________________________________________________________________________________________
							Set Up
Repo Cloning:

	The control room software code base is developed in Visual Studio 2017 and is hosted on Github at:
	https://github.com/YCPRadioTelescope/YCP-RT-ControlRoom. You will need to have Visual Studio 2017
	installed on your computer as well as an installation of git for repo cloning. Go to the link
	shown above, copy the clone link, and then open a command line (I prefer to use git bash). Clone
	the repo into the directory of your choice, and then open the repo in Visual Studio using the
	solution file within the ControlRoomApplication folder.

Package Settings:

	Now that we have the repo cloned, and the solution file is open, we must set up the package settings
	for the project. Start by right clicking the project icon in the solution explorer for
	ControlRoomApplication. In the right click menu, select Manage NuGet Packages. A new file will open
	in the code window, allowing you to search for the necessary packages. For each package that you
	install, take care to choose the correct version of the package. The packages you will install are:
	EntityFramework v6.2.0, My.Sql.Data.EntityFrameWork v8.0.17, MySql.Data v8.0.17, AASharp v1.93.3,
	and AWSSDK.RDS v3.3.38.2. Be sure to only install the packages specified, as certain other packages
	will negate the effects of others and may break you project. Finally, for security reasons, you must
	contact a system administrator for a copy of the AWSConstants file for access to the AWS Server’s data,
	and the pushNotifications file for sending push notifications. The pushNotifications file belongs in
	"ControlRoomApplication\ControlRoomApplication\Controllers\Communications\pushNotification.cs".

MySQL Set Up:

	The next step is the setup of the MySql Server. Follow the link here:
	https://dev.mysql.com/downloads/windows/installer/8.0.html, and download the smaller msi file. Run the 
	file after it has finished downloading and follow the normal installation process choosing defaults for
	all of the options until you reach the root password creation window. Contact a System Administrator to
	receive the password. This will allow the Control Room to link to your local MySql database. Follow
	through the rest of the installation process choosing defaults for all of the rest of the options until
	the service is finished. The service should start running immediately (you can look in the Services
	section of the Task Manager to check if it is running). Finally, reopen the Control Room Application
	within Visual Studio, and build it. If you are still running into a MySql connection error, run the
	MySql installer again. Select add, and then choose MySQL Connectors, then Connector/NET, and then choose
	the version using the green arrow. Select next and follow the standard installation procedure. Finally,
	you must install and run the RT-Contracts Application.

