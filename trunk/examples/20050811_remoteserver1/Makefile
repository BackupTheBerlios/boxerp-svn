CSC=mcs

%.exe: %.cs
	$(CSC) /r:System.Runtime.Remoting /r:PostgresObject.dll -r:System.Data -r:Npgsql $<

%.dll: %.cs
	$(CSC) /r:System.Runtime.Remoting /target:library -r:System.Data -r:Npgsql $<
		
all: PostgresObject.dll SampleServer.exe SampleClient.exe
		
PostgresObject.dll: PostgresObject.cs

SampleServer.exe: SampleServer.cs PostgresObject.dll

SampleClient.exe: SampleClient.cs PostgresObject.dll

clean:
	rm -f *.dll *.exe

			
