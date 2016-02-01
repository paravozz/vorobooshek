# VOROBOOSHEK

[![Build Status](https://travis-ci.org/paravozz/vorobooshek.svg?branch=master)](https://travis-ci.org/paravozz/vorobooshek)

-------------------------------------
## Included in Release folder:

1. ChatServer;
2. MySQL.Connector dll library;
3. Vorobooshek Application Client;
4. vorobooshek_database;
5. mysql_vorobooshek_user_database;

-------------------------------------
ChatServer default setup at 7770 port. Client Application default setup is 127.0.0.1 : 7770.

It means, that if you want to test the application, you need:


### !!!WARNING!!!

PATCHING OF MYSQL.USER TABLE IS DROP YOURE 'USER' TABLE AND CREATE NEW WITH A 1 RECORD - vorobooshek_user;
IF YOU DONT NOW WHAT IT MEAN, PLEASE DON'T TRY TO DO IT, OR DUMP YOURE CURRENT USER TABLE, AND PATCH WITH A NEW AFTER THAT

-------------------------------------

1. Patch youre MySQL database 'User' table, by file "mysql_vorobooshek_user_database.sql";
2. Create database vorobooshek. Patch youre MySQL base by file "vorobooshek_database.sql"(This patch must create database 'vorobooshek' with a table 'accounts' and fields 'account: Test', 'password: Test');
3. Open port 7770 (if not opened) and run "ChatServer.exe";
4. Run "Vorobooshek Test.exe" and try to chat.
5. Profit!

-------------------------------------

If you want to change youre IP and setup application to your own resources( Like port, or MySQL Binding IP):

You must open the project, and go to Resources.resx and change some symbols.
Just try.

-------------------------------------

if you have any questions:

e-mail: prvz-work@outlook.com

P.S. THE IDEA OF CHAT REALISATION IS BY HackingMemory - https://www.youtube.com/user/HackingMemory

>﻿ Copyright 2015 Ilya Perevoznik

>Licensed under the Apache License, Version 2.0 (the "License");
>you may not use this file except in compliance with the License.
>You may obtain a copy of the License at
>
>    http://www.apache.org/licenses/LICENSE-2.0
>
>Unless required by applicable law or agreed to in writing, software
>distributed under the License is distributed on an "AS IS" BASIS,
>WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
>See the License for the specific language governing permissions and
>limitations under the License. 
