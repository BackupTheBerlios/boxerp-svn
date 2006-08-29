1. Create the database in your system.
2. Change activeRecord.xml accordingly your database
3. export MONO_PATH=$PWD/castle-bin
4. make dbgenerator
5. ./castle-bin/dbgenerator.exe   (will create tables into database)
6. psql -U user database < dump_data.sql  (will restore some data into database)
7. make
8. ./server.exe
