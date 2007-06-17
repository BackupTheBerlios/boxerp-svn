BIN= . 
MODELS=  -r:./Models.dll
INTERFACES= -r:./Interfaces.dll  
CASTLE= -r:./Castle.ActiveRecord 
TESTS= Test1.cs Test2.cs

MONO= -r:Mono.Posix -r:System.Runtime.Remoting -pkg:nunit

all: test

test: 
	mcs $(TESTS) RemotingHelper.cs $(MODELS) $(INTERFACES) $(MONO) $(CASTLE) -target:library -out:./test.dll
