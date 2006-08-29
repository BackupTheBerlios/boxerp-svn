CSC=mcs

%.exe: %.cs
	$(CSC) /r:System.Runtime.Remoting /r:AccessController.dll /r:LoginObject.dll /r:Database.dll -r:System.Data -r:Npgsql $<

%.dll: %.cs
	$(CSC) /r:System.Runtime.Remoting /target:library -r:System.Data -r:Npgsql $<
		
all: AccessController.dll LoginObject.dll Database.dll SampleServer.exe SampleClient.exe
		
AccessController.dll: AccessController.cs LoginObject.cs Database.cs

SampleServer.exe: SampleServer.cs AccessController.dll LoginObject.dll Database.dll

SampleClient.exe: SampleClient.cs AccessController.dll LoginObject.dll Database.dll

clean:
	rm -f *.dll *.exe

			
