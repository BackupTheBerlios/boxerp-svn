CSC=mcs

%.exe: %.cs
	$(CSC) /r:System.Runtime.Remoting /r:ObjetoRemoto.dll -r:System.Data -r:Npgsql $<

%.dll: %.cs
	$(CSC) /r:System.Runtime.Remoting /target:library -r:System.Data -r:Npgsql $<
		
all: ObjetoRemoto.dll SampleServer.exe SampleClient.exe
		
ObjetoRemoto.dll: ObjetoRemoto.cs

SampleServer.exe: SampleServer.cs ObjetoRemoto.dll

SampleClient.exe: SampleClient.cs ObjetoRemoto.dll

clean:
	rm -f *.dll *.exe

			
