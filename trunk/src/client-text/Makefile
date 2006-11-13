BIN= castle-bin
MODELS=  -r:./Models.dll  
CASTLE= -r:./$(BIN)/Castle.ActiveRecord \

MONO= -r:Mono.Posix -r:System.Runtime.Remoting 

client: 
	mcs Client.cs RemotingHelper.cs $(MODELS) $(MONO) $(CASTLE) -r:./Interfaces.dll  -out:./client.exe 
