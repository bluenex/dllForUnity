# Introduction
This repository collects the dlls used in my Unity3d projects. There are two dlls in use which are dll for [dynamixel servo motor](http://support.robotis.com/en/product/dynamixel/mx_series/mx-64.htm) and dc motor controlled by arduino. This repo name was **dxlWrappedDLL** because there was only one dll.

# DLLs
### dxlWrappedDLL
This dll is a wrapper of dynamixel.dll which is not compatible with Unity3d. In order to use this dll, the dynamixel.dll must be recompiled from [source](http://support.robotis.com/en/software/dynamixel_sdk/usb2dynamixel/usb2dxl_windows.htm) with .NET 3.5 target (512,512 bytes on Windows).

### dcDLLforUnity
This dll is to connect with Arduino with [this sketch](https://github.com/bluenex/arduinewbie) for controlling dc motor.
