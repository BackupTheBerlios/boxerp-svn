BIN= castle-bin
MODELS=  -r:./Models.dll  
CASTLE= -r:./$(BIN)/Castle.ActiveRecord \
        -r:./$(BIN)/Iesi.Collections \
		  -r:./$(BIN)/NHibernate
OBJECTS= *Object.cs 

MONO= -r:Mono.Posix -r:System.Runtime.Remoting -r:System.Data
BOXERPBASE= DebugTools.cs ErrorManager.cs ServerExceptions.cs

server: objects
	mcs ObjectsServer.cs $(MODELS) $(MONO) $(CASTLE) -r:./Objects.dll  -out:./server.exe 

#objects: 
#	mcs -target:library $(OBJECTS) $(MODELS) $(CASTLE) $(MONO) $(BOXERPBASE) -out:Objects.dll

objects: interfaces
	mcs -target:library LoginObject.cs AdminObject.cs SessionsObject.cs -r:./Interfaces.dll $(MODELS) $(MONO) $(CASTLE) $(BOXERPBASE) -out:Objects.dll
interfaces: models
	mcs -target:library ILogin.cs IAdmin.cs $(MODELS) $(CASTLE) $(MONO) -out:Interfaces.dll
models:
	mcs -target:library Models/*.cs $(CASTLE) $(MONO) -out:Models.dll

dbgenerator:
	mcs -r:./$(BIN)/Castle.ActiveRecord -r:./$(BIN)/NHibernate -r:./$(BIN)/Castle.Model SchemaGenerator.cs $(MODELS) $(CASTLE) -out:dbgenerator.exe
