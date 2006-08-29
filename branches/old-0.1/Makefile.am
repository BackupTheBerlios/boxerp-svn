# Compile Makefile in this directory and Makefile in po directory:
SUBDIRS = . po

# Globals:
# $(srcdir) = this directory -> $PWD
CSC=mcs
DBPATH=$(srcdir)/database
CLIENTPATH=$(srcdir)/client
TESTPATH=$(srcdir)/test
SERVERPATH=$(srcdir)/server
BINPATH=$(srcdir)/bin
GLADE=-pkg:glade-sharp-2.0
GTK=-pkg:gtk-sharp-2.0
NOWARN=-nowarn:0169

### server sources:
SERVER_CSFILES=$(SERVERPATH)/Users.cs \
						$(SERVERPATH)/Enterprises.cs \
						$(SERVERPATH)/Groups.cs  \
						$(SERVERPATH)/AdminFacade.cs \
						$(SERVERPATH)/FacadeCache.cs \
						$(SERVERPATH)/ErrorManager.cs \
						$(SERVERPATH)/DebugTools.cs \
						$(SERVERPATH)/DataTools.cs \
						$(SERVERPATH)/ServicesTools.cs \
						$(SERVERPATH)/ConcurrencyControllerObject.cs \
						$(SERVERPATH)/LoginService.cs \
						$(SERVERPATH)/HistoryObject.cs \
						$(SERVERPATH)/SessionsObject.cs \
						$(DBPATH)/Database.cs \
						$(DBPATH)/GenericDatabase.cs 
	

### Base libraries: this dll dont depends any other boxerp files.
DEBUG_ASSEMBLIES=-r:$(BINPATH)/DebugTools.dll
DATA_ASSEMBLIES=-r:$(BINPATH)/DataTools.dll 
SERVER_EXCEPTIONS_ASSEMBLIES=-r:$(BINPATH)/ServerExceptions.dll
ERRORS_ASSEMBLIES=-r:$(BINPATH)/ErrorManager.dll
FACADE_ASSEMBLIES=-r:$(BINPATH)/FacadeCache.dll

DB_ASSEMBLIES=-r:$(BINPATH)/GenericDatabase.dll

DB_CSFILES= $(DBPATH)/Database.cs \
            $(DBPATH)/GenericDatabase.cs 

ADMINMODULE_CSFILES=$(SERVERPATH)/AdminFacade.cs \
							$(SERVERPATH)/Users.cs \
							$(SERVERPATH)/Groups.cs \
							$(SERVERPATH)/Enterprises.cs 

SERVER_SERVICES=-r:$(BINPATH)/AdminFacade.dll \
                -r:$(BINPATH)/LoginService.dll 
					
SERVICES_TOOLS=$(SERVERPATH)/ServicesTools.cs

SERVER_OBJECTS=-r:$(BINPATH)/HistoryObject.dll \
					-r:$(BINPATH)/ConcurrencyControllerObject.dll \
					-r:$(BINPATH)/SessionsObject.dll
						
SERVER_PROXYS= $(srcdir)/AdminFacadeProxy.cs \
               $(srcdir)/LoginServiceProxy.cs 
							 
### This are all boxerp dlls. Must be listed in dependency order, from less to more dependent.
ALL_ASSEMBLIES=$(BINPATH)/DebugTools.dll \
						$(BINPATH)/DataTools.dll \
						$(BINPATH)/ServerExceptions.dll \
						$(BINPATH)/ErrorManager.dll \
						$(BINPATH)/FacadeCache.dll \
						$(BINPATH)/GenericDatabase.dll \
						$(BINPATH)/ConcurrencyControllerObject.dll \
						$(BINPATH)/HistoryObject.dll \
						$(BINPATH)/SessionsObject.dll \
						$(BINPATH)/LoginService.dll \
						$(BINPATH)/AdminFacade.dll 
					
TEST_ASSEMBLIES=AdminTest.dll \
          AdminTest2.dll

										
### Client sources:
CLIENT_CSFILES=$(CLIENTPATH)/Client.cs \
						$(CLIENTPATH)/LoginWindow.cs \
						$(CLIENTPATH)/MainWindow.cs \
						$(CLIENTPATH)/GuiExtractor.cs \
						$(CLIENTPATH)/ClientExceptions.cs \
						$(CLIENTPATH)/GuiTreeview.cs \
  				      $(CLIENTPATH)/UsersWindow.cs \
						$(CLIENTPATH)/EnterprisesWindow.cs \
						$(CLIENTPATH)/GroupsWindow.cs \
						$(CLIENTPATH)/GuiTools.cs \
						$(CLIENTPATH)/BoxerpWindow.cs \
						$(CLIENTPATH)/EditGroupWindow.cs \
						$(CLIENTPATH)/EditEnterpriseWindow.cs \
						$(CLIENTPATH)/EditUserWindow.cs 


###*********************** --- Targets: 

all: $(ALL_ASSEMBLIES) Defines.cs $(BINPATH)/ObjectsServer.exe $(BINPATH)/Client.exe 
	
server: $(ALL_ASSEMBLIES) $(BINPATH)/ObjectsServer.exe Defines.cs
client: $(ALL_ASSEMBLIES) $(BINPATH)/Client.exe Defines.cs


### --- Server.exe
SERVERFILES = $(SERVERPATH)/ObjectsServer.cs $(srcdir)/Defines.cs
$(BINPATH)/ObjectsServer.exe: $(SERVERPATH)/ObjectsServer.cs $(SERVER_CSFILES)
	$(CSC) $(NOWARN) $(SERVER_OBJECTS) -r:Mono.Posix -r:System.Runtime.Remoting -out:$@ -doc:doc/Server.xml $(SERVERFILES)

### --- Client.exe
CLIENTFILES = $(CLIENT_CSFILES) $(srcdir)/Defines.cs $(SERVER_PROXYS)
$(BINPATH)/Client.exe: $(CLIENT_CSFILES) 
	$(CSC) $(NOWARN) $(GLADE) $(GTK) $(SERVER_OBJECTS) $(DEBUG_ASSEMBLIES) $(ERRORS_ASSEMBLIES) $(DATA_ASSEMBLIES) -r:Mono.Posix -r:System.Runtime.Remoting -r:System.Web.Services -out:$@ -doc:doc/Client.xml -r:System.Data $(CLIENTFILES)

### --- Base ASSEMBLIES:
$(BINPATH)/DebugTools.dll: $(SERVERPATH)/DebugTools.cs
	$(CSC) $(NOWARN) -target:library -out:$@ -doc:doc/DebugTools.xml $^

$(BINPATH)/ErrorManager.dll: $(SERVERPATH)/ErrorManager.cs
	$(CSC) $(NOWARN) -r:Mono.Posix -target:library -out:$@ -doc:doc/ErrorManager.xml -r:System.Data $^

$(BINPATH)/DataTools.dll: $(SERVERPATH)/DataTools.cs
	$(CSC) $(NOWARN) -target:library -out:$@ -doc:doc/DataTools.xml -r:System.Data $^
	
$(BINPATH)/ServerExceptions.dll: $(SERVERPATH)/ServerExceptions.cs
	$(CSC) $(NOWARN) -r:Mono.Posix -target:library -out:$@ -doc:doc/ServerExceptions.xml $^

$(BINPATH)/FacadeCache.dll: $(SERVERPATH)/FacadeCache.cs
	$(CSC) $(NOWARN) -target:library -out:$@ -doc:doc/FacadeCache.xml $^
	
### --- Database ASSEMBLIES:

$(BINPATH)/GenericDatabase.dll: $(DB_CSFILES) $(srcdir)/Defines.cs
	$(CSC) $(ERRORS_ASSEMBLIES) $(DEBUG_ASSEMBLIES) $(SERVER_EXCEPTIONS_ASSEMBLIES) -target:library -out:$@ -doc:doc/GenericDatabase.xml -r:System.Data -r:Npgsql $^

### --- Server side ASSEMBLIES:

$(BINPATH)/ConcurrencyControllerObject.dll: $(SERVERPATH)/ConcurrencyControllerObject.cs  
	$(CSC) $(NOWARN) $(DB_ASSEMBLIES) $(DEBUG_ASSEMBLIES) $(ERRORS_ASSEMBLIES) $(SERVER_EXCEPTIONS_ASSEMBLIES) -r:System.Runtime.Remoting -target:library -out:$@ -doc:doc/ConcurrencyControllerObject.xml -r:System.Data $^

$(BINPATH)/HistoryObject.dll: $(SERVERPATH)/HistoryObject.cs 
	$(CSC) $(NOWARN) $(DB_ASSEMBLIES) $(DEBUG_ASSEMBLIES) $(ERRORS_ASSEMBLIES) $(SERVER_EXCEPTIONS_ASSEMBLIES) -r:System.Runtime.Remoting -target:library -out:$@ -doc:doc/HistoryObject.xml -r:System.Data $^

$(BINPATH)/SessionsObject.dll: $(SERVERPATH)/SessionsObject.cs 
	$(CSC) $(NOWARN) $(DB_ASSEMBLIES) $(DEBUG_ASSEMBLIES) $(ERRORS_ASSEMBLIES) $(SERVER_EXCEPTIONS_ASSEMBLIES) -r:System.Runtime.Remoting -target:library -out:$@ -doc:doc/SessionsObject.xml -r:System.Data $^

$(BINPATH)/LoginService.dll: $(SERVERPATH)/LoginService.cs  $(SERVICES_TOOLS) $(srcdir)/Defines.cs
	$(CSC) $(NOWARN) $(DB_ASSEMBLIES) $(SERVER_EXCEPTIONS_ASSEMBLIES) $(DATA_ASSEMBLIES) $(ERRORS_ASSEMBLIES) $(DEBUG_ASSEMBLIES) $(SERVER_OBJECTS) -r:System.Web.Services -target:library -out:$@ -doc:doc/LoginService.xml -r:System.Data -r:System.Runtime.Remoting $^

$(BINPATH)/AdminFacade.dll: $(ADMINMODULE_CSFILES) $(SERVICES_TOOLS) $(srcdir)/Defines.cs
	$(CSC) $(NOWARN) $(DB_ASSEMBLIES) $(DATA_ASSEMBLIES) $(SERVER_EXCEPTIONS_ASSEMBLIES) $(ERRORS_ASSEMBLIES) $(DEBUG_ASSEMBLIES) $(FACADE_ASSEMBLIES) $(SERVER_OBJECTS) -r:System.Runtime.Remoting -r:System.Web.Services -target:library -out:$@ -doc:doc/AdminFacade.xml -r:System.Data $^

#AdminTest.dll: $(TESTPREF)/AdminTest.cs 
#	$(CSC) $(NOWARN) $(TOOLS_ASSEMBLIES) -r:AdminFacade.dll -target:library -r:System.Data -r:System.Runtime.Remoting -r:nunit.framework $^
#AdminTest2.dll: $(TESTPREF)/AdminTest2.cs 
#	$(CSC) $(NOWARN) $(TOOLS_ASSEMBLIES)  -r:AdminFacade.dll -target:library -r:System.Data -r:System.Runtime.Remoting -r:nunit.framework $^

### --- Clean:
#clean:
#	rm -f $(BINPATH)/* 

CLEANFILES =      \
	intltool-extract  \
	intltool-update   \
	intltool-merge \
	$(BINPATH)/*.dll \
	$(BINPATH)/*.exe 

EXTRA_DIST =      \
	intltool-extract.in \
	intltool-merge.in \
	intltool-update.in  

DISTCLEANFILES = \
	intltool-extract        \
	intltool-merge          \
	intltool-update         
																												
			
