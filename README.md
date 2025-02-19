# NativeSPI-V1_Linux
Wrapper class for using the NativeSPI interface of the F&amp;S boards in  .NET environment

This project is intended to give the customers of F&S a unified software interface for SPI in .NET projects, regardless of using Windows or Linux as operating system.
The code shown here is the Linux equivalent to the software available at F&S: [Software .NET Drivers](https://www.fs-net.de/en/embedded-modules/accessories/software-net-drivers/)

# How to use this code
You can either implement NspiPortV1.cs as a class into your existing project, or compile a DLL which can be placed on your device alongside your project binaries.

If you have an existing WinForms application that is using NativeSPI-V1.dll for Windows, you can run this application under Linux using [Mono](https://www.mono-project.com/). Keep in mind, that Mono only supports .NET up to Framework 4.7! Replace the windows specific NativeSPI-V1.dll with the file provided here.
You can also build it yourself:

'''
csc.exe /target:library /out:NativeSPI-V1.dll /reference:System.dll .\NspiPortV1.cs
'''

When using Mono, you also need need a [special library](https://github.com/FSEmbedded/dotnet_linux_IO_API) provided by F&S.
Also have a look at [NativeI2C for Linux](https://github.com/FSEmbedded/NativeI2C_Linux) and this [demo application](https://github.com/FSEmbedded/WinForms_On_Linux_InterfaceDemo).