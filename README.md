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
	AWSConstants belongs in the Constants folder. You may have to manually add the package to the files
	that use it. The LOCAL_DATABASE_STRING constant will have to be modified to include the password you
	set for MySQL.


MySQL Set Up:

	The next step is the setup of the MySql Server. Follow the link here:
	https://dev.mysql.com/downloads/windows/installer/8.0.html, and download the smaller msi file. Run the 
	file after it has finished downloading and follow the normal installation process choosing defaults for
	all of the options until you reach the root password creation window. If the installer doesn’t ask you 
	for a password at any point during installation, uninstall the version you installed, and using the same 
	link, download the larger of the two files. Leave the username as root. The password you set can be 
	whatever you want it to be, but the password in application_local.properties in the back end and 
	LOCAL_DATABASE_STRING in the control room must match it.  This will allow the Control Room to link to 
	your local MySql database. Follow through the rest of the installation process choosing defaults for 
	all of the rest of the options until the service is finished. The service should start running 
	immediately (you can look in the Services section of the Task Manager to check if it is running). 
	Finally, reopen the Control Room Application within Visual Studio, and build it. If you are still running 
	into a MySql connection error, run the MySql installer again. Select add, and then choose MySQL Connectors, 
	then Connector/NET, and then choose the version using the green arrow. Select next and follow the standard 
	installation procedure. Finally, you must install and run the RT-Contracts Application.
	
Running the Control Room Software for the first time:

	Prior to running the Control Room Software, you will need to set up the backend portion of the project, RT-Contracts. 
	That respository along with instructions can be found here: https://github.com/YCPRadioTelescope/RT-Contracts.
	Upon completing the setup for the backend, which will only **create** your local database(e.g. without any data), 
	you can run the Control Room software to begin populating it. If you have never run the control room before, you will 
	get a popup window stating that the "new telescope flag was set to true". Click "YES" or "OK" to allow this new telescope 
	creation to take place. This will populate your database with a new RadioTelescope instance that you can run now by placing 
	the ID of 1 inside the JSON file on subsequent runs. **NOTE:** anytime you wish to run a SPECIFIC telescope, set the 
	"newTelescope" flag to false in the JSON file, and place the ID of the telescope you wish to run inside of the "telescopeID" 
	field. This is our JSON configuration file found in the root directory of the project that will allow you to run specific 
	instances of a radio telescope from the database by its ID. Inside of the JSON file you will see something similar to this:

![Capture](https://user-images.githubusercontent.com/57024625/135680784-51a3f150-82b0-40f2-8baf-4f7cbf57f422.PNG)

	From here you can specify an ID or specify the newTelescope flag to be set to true (which creates
	a new telescope). However on your first run, since you do not have any telescopes created, you will 
	need to select "YES" or "OK" on the popup window that asks you to confirm you would like to create 
	a new telescope instance. After doing so, you will be able to use the control room normally with that 
	telescope you created.




