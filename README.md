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
                                              Installations/Downloads
        First, to get set up, you must download and install the following applications. You should select the default options 
	and features for all of the applications below. 
        
   - Visual Studio Community 2017
   - MySQL Community Server
   - Arduino IDE (Optional - required for working with the scale model)
________________________________________________________________________________________________________________________________________
                                              Github Repository Setup
	The first thing to do in order to contribute code to the project is to get added as a contributor by the repository
	owner that is listed above. After you are added to the Github repository as a contributor you can move on to the Visual
	Studio Setup section of this manual.

   Steps:
    -Contact repository owner and get added as a contributor
________________________________________________________________________________________________________________________________________
                                              Visual Studio Setup
	Once you have Visual Studio installed and you are added as a contributor to the Github repository, you must setup your
	Visual Studio environment by connecting the repository inside of Visual Studio. This will allow you to push, pull, merge,
	fetch, etc. to the remote repository. With Visual Studio opened already:

Repository Setup Steps:
    - Go to “View” >> “Team Explorer” >> double click to open the Team Explorer window
    - In "Team Explorer” >> “Local Git Repositories” >> “Clone” >> Add the URL of the Github repository (or the SSH key if you are 	       Mr. Savvy). 
        - Edit the file path that your repository will be stored in if it is not correct.
    - Make sure that “Recursively Clone Submodules” is checked off
    - Enjoy your new Github Repository

Dependency/References Setup Steps:
    - Go to “View” >> “Solution Explorer” >> double click to open the Solution Explorer window
    - In “Solution Explorer” >> right click “ControlRoomApplication” >> “Manage NuGet Packages” >> click “Browse” >> Search/Install the       following:
        - EntityFramework (v 6.2.0)
        - MySql.Data.EntityFramework (v 8.0.13)
                -(DO NOT GET MySql.Data.Entity IT IS INCOMPATIBLE)
        - MySql.Data (v 8.0.13)
        - AASharp (1.93.3)

AWS Setup Steps:
    - Contact a repository contributor, and ask them for the file (as it will be a server that we will be actively using and 	               adding to, we cannot release the uid or password to the public)
________________________________________________________________________________________________________________________________________
                                                MySQL Server Setup
	MySQL server is the server provider that we are using to connect to our MySQL database that is hosted on Amazon Web
	Services (AWS). The only setup required for the MySQL server is that you have a local instance running on your machine.
	The local instance should automatically spin up after completion of the installation above. If it is not running,
	however, you can do the following:

Windows Startup Steps:
    - If you are installing MySql for the first time follow these steps:
    	- Go to this link and donlaowd the top msi file: https://dev.mysql.com/downloads/windows/installer/8.0.html
	- Run the msi file, go to add and add in the most recent version of MySql 8.XX
	- Choose the defaults through the install process but when it comes to setting the root password, change it ycpRT2018!
	- Follow throught the rest of the installation as normal
	- The MySql service should immediately being running after you finish through the installation wizard
    - If you have already installed MySql, follow these steps:
    	- Follow the steps seen here to reset the root password to that shown above
	- If you are unable to reset the password, stop the MySql service by opening task manager, finding your instance of MySql 		  Server, right clicking the service and select stop
	- Follow the steps above to reinstall MySql and set the password correctly

Linux Startup Steps:
    - Open a terminal
    - Enter “sudo mysql start” (I am not 100% sure this is correct, but it’s something like this. You’re using linux, you’ve probably got this.)
________________________________________________________________________________________________________________________________________
						Arduino IDE
	The Arduino IDE does not require any additional setup other than the initial default installation. After the default 
	installation, you should be able to simply download the Arduino driver files for the stepper motors from the shared 
	drive under Team Jupiter’s folder. After downloading the Arduino file(s), you can open them in the Arduino IDE and edit
	them or load them onto an Arduino board after setting the correct COM port under the “Tools” tab in the IDE. 

Steps:
    - “Tools” >> “Port” >> select the proper COM port that your USB is connected to.
