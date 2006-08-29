@echo off
mcs -pkg:gtk-sharp -pkg:glade-sharp -resource:tree.glade tree.cs
@echo:
pause