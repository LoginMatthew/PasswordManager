# Password Manager

## Overview

The password manager program for storing passwords with encryption on windows.

The program aims to use MVVM architecture and separates the specific program logic parts and layers and WPF solution project. The program has been written in such a way that it is easy to expand or replace the given element and modul unit. The program uses Generic type and reflection and dependencyInjection and hosting.

The program requires .NET 8.

## Features

* Store passwords with encryption (by Symmetric AES and Symmetric Rijndael) that can be changed
* Stored data (password and site and username and email) can be copied to clipboard
* Random enryption key can be generated
* Unique encryption key can be used
* The password file's name and type where data is stored can be changed
* Search by site
* Passwords can be stored in plain text too
* The background can be changed

## Build

- Build with Visual Studio 2022

## Future developement plans

* Add Login and Registration logic and layer
* Add another storage place(s) e.g.: MSSQL, MYSQL, XML etc
* Add Backup your files to a specific location. Save the files at the set time.
* Modify the program to connect to a server and store data there!
* Create test case(s)
