CSC=mcs

%.exe: %.cs
	$(CSC) /r:System.Runtime.Remoting /r:ServerObject.dll $<

%.dll: %.cs
	$(CSC) /r:System.Runtime.Remoting /target:library $<
		
all: ServerObject.dll SampleServer.exe SampleClient.exe
		
ServerObject.dll: ServerObject.cs

SampleServer.exe: SampleServer.cs ServerObject.dll

SampleClient.exe: SampleClient.cs ServerObject.dll

clean:
	rm -f *.dll *.exe

			
