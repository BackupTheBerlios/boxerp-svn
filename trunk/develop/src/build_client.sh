#!/bin/sh

make server
/etc/init.d/apache2 restart
wsdl http://localhost/boxerp/admin.asmx?wsdl -out:AdminFacadeProxy.cs -n:Boxerp.Facade
wsdl http://localhost/boxerp/login.asmx?wsdl -out:LoginServiceProxy.cs -n:Boxerp.Objects 
make client

