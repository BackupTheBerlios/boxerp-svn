CSC=mcs

%.exe: %.cs
	$(CSC) /r:System.Runtime.Remoting /r:AccessController.dll -r:System.Data -r:Npgsql $<

%.dll: %.cs
	$(CSC) /r:System.Runtime.Remoting /target:library -r:System.Data -r:Npgsql $<
		
all: AccessController.dll SampleServer.exe SampleClient.exe
		
AccessController.dll: AccessController.cs

SampleServer.exe: SampleServer.cs AccessController.dll

SampleClient.exe: SampleClient.cs AccessController.dll

clean:
	rm -f *.dll *.exe

			
