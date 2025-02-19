/*****************************************************************************/
/***                 _    _        _   _____   ____ _    _                 ***/
/***                | \  | |      | | |  __ \ / ___/ |  | |                ***/
/***                |  \ | | ___ _| |_| |  \ | |   | |  | |                ***/
/***                |   \| |/ _ \_   _| |  | | |   | |  | |                ***/
/***                | |\   | |/_/ | | | |  | | |   | |  | |                ***/
/***                | | \  | |__  | | | |__/ | |___| |__| |                ***/
/***                |_|  \_|\___\ \_\ |_____/ \____\______/                ***/
/***                                                                       ***/
/*****************************************************************************/
/***                                                                       ***/
/***                                                                       ***/
/***             . N E T   N a t i v e S P I   A c c e s s                 ***/
/***                                                                       ***/
/***                                                                       ***/
/*****************************************************************************/
/*** File:     NspiPort-V1.cs                                              ***/
/*** Author:   Hartmut Keller                                              ***/
/*** Created:  28.02.2009                                                  ***/
/*** Modified: 16.04.2009 17:55:58 (HK)                                    ***/
/*** Modified  11.05.2023 14:06:58 (DD)                                    ***/
/*** Modified  13.02.2025 11:27:45 (BS)                                    ***/
/***                                                                       ***/
/*** Description:                                                          ***/
/*** Wrapper class for using the NativeSPI interface of the F&S boards in  ***/
/*** .NET environment (C#, VB, using the Compact Framework).               ***/
/***                                                                       ***/
/*** Modification History:                                                 ***/
/*** 28.02.2009 HK: First working version, including comments              ***/
/*** 16.04.2009 HK: New signatures with additional length paremeters added ***/
/*** 11.05.2023 DD: Fixed Error in handleError() and Transfer()            ***/
/***                Changed GetLastWin32Error() with GetLastPInvokeError() ***/
/*** 13.02.2025 BS: Changed GetLastPInvokeError() with GetLastWin32Error() ***/
/***                GetLastPInvokeError() could not be called from Mono	   ***/
/*****************************************************************************/

using System;
using System.Data;
using System.Text;
using System.Runtime.InteropServices;

//Set version number for the assembly.
//[assembly: System.Reflection.AssemblyInformationalVersion("1.1.0.0")] TODO gibt fehler in code

namespace FS.NetDCU
{
    //------------------- NspiPortV1Exception Class -------------------------------

    /*************************************************************************
     *** Class:      NspiPortV1Exception : Exception                       ***
     ***                                                                   ***
     *** Description:                                                      ***
     *** ------------                                                      ***
     *** Exception including some error code. Provide every parameter com- ***
     *** bination that is possible.                                        ***
     *************************************************************************/
    public class NspiPortV1Exception : ApplicationException
    {
        #region NspiPortV1Exception: Member variables
        /* Error value, usually from GetLastWin32Error() */
        private int reason;

        public int Reason
        {
            get
            {
                return reason;
            }
        }
        #endregion

        #region NspiPortV1Exception: Construction and Destruction
        /*********************************************************************
         *** Constructor: NspiPortV1Exception(string text, int reason,     ***
         ***                                             Exception inner)  ***
         ***                                                               ***
         *** Parameters:  text:   Error text                               ***
         ***              reason: Error value                              ***
         ***              inner:  Inner exception                          ***
         ***                                                               ***
         *** Description:                                                  ***
         *** ------------                                                  ***
         *** Use given error value and error text.                         ***
         *********************************************************************/
        public NspiPortV1Exception(string text, int reason, Exception inner)
            : base(text + ": Error code " + reason.ToString(), inner)
        {
            this.reason = reason;
        }


        /*********************************************************************
         *** Constructor: NspiPortV1Exception(string text, int reason)     ***
         ***                                                               ***
         *** Parameters:  text:   Error text                               ***
         ***              reason: Error value                              ***
         ***                                                               ***
         *** Description:                                                  ***
         *** ------------                                                  ***
         *** Use given error value and error text.                         ***
         *********************************************************************/
        public NspiPortV1Exception(string text, int reason)
            : this(text, reason, null)
        { }


        /*********************************************************************
         *** Constructor: NspiPortV1Exception(string text)                 ***
         ***                                                               ***
         *** Parameters:  text:  Error text                                ***
         ***                                                               ***
         *** Description:                                                  ***
         *** ------------                                                  ***
         *** Use GetLastWin32Error() and given error text.                 ***
         *********************************************************************/
        public NspiPortV1Exception(string text)
            : this(text, Marshal.GetLastWin32Error())
        { }


        /*********************************************************************
         *** Constructor: NspiPortV1Exception(string text, Exception inner)***
         ***                                                               ***
         *** Parameters:  text:  Error text                                ***
         ***              inner: Inner exception                           ***
         ***                                                               ***
         *** Description:                                                  ***
         *** ------------                                                  ***
         *** Use GetLastWin32Error() and given error text.                 ***
         *********************************************************************/
        public NspiPortV1Exception(string text, Exception inner)
            : this(text, Marshal.GetLastWin32Error(), inner)
        { }


        /*********************************************************************
         *** Constructor: NspiPortV1Exception(int reason)                  ***
         ***                                                               ***
         *** Parameters:  reason: Error value                              ***
         ***                                                               ***
         *** Description:                                                  ***
         *** ------------                                                  ***
         *** Use given error value and "System error".                     ***
         *********************************************************************/
        public NspiPortV1Exception(int reason)
            : this("System error", reason)
        { }


        /*********************************************************************
         *** Constructor: NspiPortV1Exception(int reason, Exception inner) ***
         ***                                                               ***
         *** Parameters:  reason: Error value                              ***
         ***              inner:  Inner exception                          ***
         ***                                                               ***
         *** Description:                                                  ***
         *** ------------                                                  ***
         *** Use given error value and "System error".                     ***
         *********************************************************************/
        public NspiPortV1Exception(int reason, Exception inner)
            : this("System error", reason, inner)
        { }


        /*********************************************************************
         *** Constructor: NspiPortV1Exception()                            ***
         ***                                                               ***
         *** Parameters:  -                                                ***
         ***                                                               ***
         *** Description:                                                  ***
         *** ------------                                                  ***
         *** Use GetLastWin32Error() and "System error".                   ***
         *********************************************************************/
        public NspiPortV1Exception()
            : this(Marshal.GetLastWin32Error())
        { }


        /*********************************************************************
         *** Constructor: NspiPortV1Exception(Exception inner)             ***
         ***                                                               ***
         *** Parameters:  inner: Inner exception                           ***
         ***                                                               ***
         *** Description:                                                  ***
         *** ------------                                                  ***
         *** Use GetLastWin32Error() and "System error".                   ***
         *********************************************************************/
        public NspiPortV1Exception(Exception inner)
            : this(Marshal.GetLastWin32Error(), inner)
        { }
        #endregion
    } // class NspiPortV1Exception


    //------------------- NspiPortV1 Class -------------------------------------------

    /*************************************************************************
     *** Class:      NspiPortV1                                            ***
     ***                                                                   ***
     *** Description:                                                      ***
     *** ------------                                                      ***
     *** Wrapper class to use the Win32 file interface to the NSPI devices ***
     *** SPI1:, SPI2:, etc.                                                ***
     *************************************************************************/
    public class NspiPortV1
    {
        #region NspiPortV1: IOCTL values
        /* IOCTL definitions */
        internal const UInt32 METHOD_BUFFERED = 0;
        internal const UInt32 METHOD_IN_DIRECT = 1;
        internal const UInt32 METHOD_OUT_DIRECT = 2;
        internal const UInt32 METHOD_NEITHER = 3;

        internal const UInt32 FILE_ANY_ACCESS = (0 << 14);
        internal const UInt32 FILE_READ_ACCESS = (1 << 14);    // file & pipe
        internal const UInt32 FILE_WRITE_ACCESS = (2 << 14);   // file & pipe

        internal const UInt32 DEVICE_NSPI = (0x0000800AU << 16);

        internal const UInt32 IOCTL_NSPI_SEND =
            DEVICE_NSPI | FILE_ANY_ACCESS | (0x800 << 2) | METHOD_BUFFERED;
        internal const UInt32 IOCTL_NSPI_RECEIVE =
            DEVICE_NSPI | FILE_ANY_ACCESS | (0x801 << 2) | METHOD_BUFFERED;
        internal const UInt32 IOCTL_NSPI_TRANSFER =
            DEVICE_NSPI | FILE_ANY_ACCESS | (0x802 << 2) | METHOD_BUFFERED;
        internal const UInt32 IOCTL_NSPI_EXCHANGE =
            DEVICE_NSPI | FILE_ANY_ACCESS | (0x803 << 2) | METHOD_BUFFERED;
        #endregion

        #region NspiPortV1: Constants and enumerations
        /* File access */
        internal const Int32 INVALID_HANDLE_VALUE = -1;
        internal const UInt32 OPEN_EXISTING = 3;

        /* Error values from API file calls. These can be used when checking
         * the return value. */
        public enum APIError : int
        {
            ERROR_SUCCESS = 0,            // No error
            ERROR_INVALID_FUNCTION = 1,   // Function not implemented
            ERROR_FILE_NOT_FOUND = 2,     // Port not found
            ERROR_ACCESS_DENIED = 5,      // Access denied
            ERROR_INVALID_HANDLE = 6,     // Invalid handle
            ERROR_NOT_READY = 21,         // Device not ready
            ERROR_WRITE_FAULT = 27,       // Write fault
            ERROR_GEN_FAILURE = 31,       // Device failure
            ERROR_DEV_NOT_EXIST = 55,     // Device does not exist
            ERROR_INVALID_PARAMETER = 87, // Bad parameters
            ERROR_INVALID_NAME = 123,     // Invalid port name
            ERROR_TIMEOUT = 1460,         // Timeout
        }

        /* Access to the device used in constructor */
        [Flags]
        public enum NspiAccess : uint
        {
            QUERY = 0x00000000,
            WRITE = 0x40000000,
            READ = 0x80000000,
            READ_WRITE = WRITE | READ
        }
        #endregion

        #region NspiPortV1: Member variables
        /* Device file handle; required in each of our API calls. */
        protected IntPtr hPort = (IntPtr)INVALID_HANDLE_VALUE;

        /* Error handling via return value (C style) or by exceptions */
        protected bool bCStyle = false;
        #endregion

        #region NspiPortV1: Construction, Destruction and Error Handling
        /*********************************************************************
         *** Constructor: NspiPortV1(string FileName, NspiAccess access)   ***
         ***                                                               ***
         *** Parameters:  FileName: Name of the device ("SPI1:", "SPI2:")  ***
         ***              access:   Any combination of GENERIC_WRITE and   ***
         ***                        GENERIC_READ                           ***
         ***                                                               ***
         *** Description:                                                  ***
         *** ------------                                                  ***
         *** Open the device file. Throw an exception if it fails.         ***
         *********************************************************************/
        public NspiPortV1(string FileName, NspiAccess access)
        {
            hPort = CECreateFileW(FileName, (UInt32)access, 0, IntPtr.Zero,
                                  OPEN_EXISTING, 0, IntPtr.Zero);

            if (hPort == (IntPtr)INVALID_HANDLE_VALUE)
            {
                throw new NspiPortV1Exception("NspiPortV1 construction failed");
            }
        }


        /*********************************************************************
         *** Destructor: NspiPortV1()                                      ***
         ***                                                               ***
         *** Description:                                                  ***
         *** ------------                                                  ***
         *** Close the device file (if open).                              ***
         *********************************************************************/
        ~NspiPortV1()
        {
            if (hPort != (IntPtr)INVALID_HANDLE_VALUE)
                CECloseHandle(hPort);
        }


        /*********************************************************************
         *** Function: void HandleErrorsViaReturn(bool bCStyle)            ***
         ***                                                               ***
         *** Parameters: bCStyle: TRUE: Return error as return value       ***
         ***                      FALSE: Throw exception on error (default)***
         ***                                                               ***
         *** Return:    -                                                  ***
         ***                                                               ***
         *** Description:                                                  ***
         *** ------------                                                  ***
         *** Set the error reporting method.                               ***
         *********************************************************************/
        public void HandleErrorsViaReturn(bool bCStyle)
        {
            this.bCStyle = bCStyle;
        }


        /*********************************************************************
         *** Function: int HandleError(int success, string errorfunction)  ***
         ***                                                               ***
         *** Parameters: success:       Return value of API function:      ***
         ***                            0: Failure, !=0: Success           ***
         ***             errorfunction: Name of function that failed       ***
         ***                                                               ***
         *** Return:     0: Success                                        ***
         ***             !=0: Error from GetLastWin32Error()               ***
         ***                                                               ***
         *** Description:                                                  ***
         *** ------------                                                  ***
         *** Depending on the error reporting method,  return the error of ***
         *** GetLastWin32Error() directly or throw an exception.           ***
         *** This function is meant to be called directly with the result  ***
         *** of the API function. Example:                                 ***
         ***                                                               ***
         ***    return HandleError(CESetCommMask(hPort, mask),             ***
         ***                       "SetCommMask() failed");                ***
         *********************************************************************/
        protected int HandleError(int success, string errorfunction)
        {
            if (success != 0)
            {
                if (bCStyle)
                    return Marshal.GetLastWin32Error();
                throw new NspiPortV1Exception(errorfunction + " failed");
            }
            return 0;
        }


        /*********************************************************************
         *** Function: int HandleError(string errorfunction, int reason)   ***
         ***                                                               ***
         *** Parameters: errorfunction: Name of function that failed       ***
         ***             reason:        Error value                        ***
         ***                                                               ***
         *** Return:     reason (as given)                                 ***
         ***                                                               ***
         *** Description:                                                  ***
         *** ------------                                                  ***
         *** Depending on the error reporting method, return the value gi- ***
         *** ven in reason or throw an exception.                          ***
         *********************************************************************/
        protected int HandleError(string errorfunction, int reason)
        {
            if (bCStyle)
                return reason;
            throw new NspiPortV1Exception(errorfunction + " failed", reason);
        }
        #endregion

        #region NspiPortV1: Member functions using DeviceIoControl()
        /*********************************************************************
         *** Function:   int Send(byte[] data)                             ***
         ***                                                               ***
         *** Parameters: data: Array with data to send                     ***
         ***                                                               ***
         *** Return:     0: Success                                        ***
         ***             !=0: Error from GetLastWin32Error()               ***
         ***                                                               ***
         *** Description:                                                  ***
         *** ------------                                                  ***
         *** Send the data bytes. All bytes of the array are sent.         ***
         *********************************************************************/
        public int Send(byte[] data)
        {
            return HandleError(
                CEDeviceIoControl(hPort, IOCTL_NSPI_SEND,
                                  null, 0, data, data.Length,
                                  IntPtr.Zero, IntPtr.Zero),
                "Send()");
        }


        /*********************************************************************
         *** Function:   int Send(byte[] cmd, byte[] data)                 ***
         ***                                                               ***
         *** Parameters: cmd:  Array with command bytes                    ***
         ***             data: Array with data to send                     ***
         ***                                                               ***
         *** Return:     0: Success                                        ***
         ***             !=0: Error from GetLastWin32Error()               ***
         ***                                                               ***
         *** Description:                                                  ***
         *** ------------                                                  ***
         *** Send the command bytes and then the data bytes.  All bytes of ***
         *** the arrays are sent.                                          ***
         *********************************************************************/
        public int Send(byte[] cmd, byte[] data)
        {
            return HandleError(
                CEDeviceIoControl(hPort, IOCTL_NSPI_SEND,
                                  cmd, cmd.Length, data, data.Length,
                                  IntPtr.Zero, IntPtr.Zero),
                "Send()");
        }


        /*********************************************************************
         *** Function:   int Send(byte[] data, int datalen)                ***
         ***                                                               ***
         *** Parameters: data:    Array with data to send                  ***
         ***             datalen: Number of valid bytes in data array      ***
         ***                                                               ***
         *** Return:     0: Success                                        ***
         ***             !=0: Error from GetLastWin32Error()               ***
         ***                                                               ***
         *** Description:                                                  ***
         *** ------------                                                  ***
         *** Send the first datalen bytes of the data array. The array may ***
         *** be larger, but only the given number of bytes are sent.       ***
         *********************************************************************/
        public int Send(byte[] data, int datalen)
        {
            if (data.Length < datalen)
                return HandleError("Send()",
                                   (int)APIError.ERROR_INVALID_PARAMETER);
            return HandleError(
                CEDeviceIoControl(hPort, IOCTL_NSPI_SEND,
                                  null, 0, data, datalen,
                                  IntPtr.Zero, IntPtr.Zero),
                "Send()");
        }


        /*********************************************************************
         *** Function:   int Send(byte[] cmd, int cmdlen,                  ***
         ***                                     byte[] data, int datalen) ***
         ***                                                               ***
         *** Parameters: cmd:     Array with command bytes                 ***
         ***             cmdlen:  Number of valid bytes in cmd array       ***
         ***             data:    Array with data to send                  ***
         ***             datalen: Number of valid bytes in data array      ***
         ***                                                               ***
         *** Return:     0: Success                                        ***
         ***             !=0: Error from GetLastWin32Error()               ***
         ***                                                               ***
         *** Description:                                                  ***
         *** ------------                                                  ***
         *** Send the first cmdlen bytes of the cmd array, then the first  ***
         *** datalen bytes of the data array. The arrays themselves may be ***
         *** larger, but only the given number of bytes are actually sent. ***
         *********************************************************************/
        public int Send(byte[] cmd, int cmdlen, byte[] data, int datalen)
        {
            if ((cmd.Length < cmdlen) || (data.Length < datalen))
                return HandleError("Send()",
                                   (int)APIError.ERROR_INVALID_PARAMETER);
            return HandleError(
                CEDeviceIoControl(hPort, IOCTL_NSPI_SEND,
                                  cmd, cmdlen, data, datalen,
                                  IntPtr.Zero, IntPtr.Zero),
                "Send()");
        }


        /*********************************************************************
         *** Function:   int Receive(out byte[] data, int datalen)         ***
         ***                                                               ***
         *** Parameters: data:    Array for received data                  ***
         ***             datalen: Number of data bytes to receive          ***
         ***                                                               ***
         *** Return:     0: Success                                        ***
         ***             !=0: Error from GetLastWin32Error()               ***
         ***                                                               ***
         *** Description:                                                  ***
         *** ------------                                                  ***
         *** Receive datalen data bytes and return an array of this size.  ***
         *********************************************************************/
        public int Receive(out byte[] data, int datalen)
        {

            data = new byte[datalen];
            return HandleError(
                CEDeviceIoControl(hPort, IOCTL_NSPI_RECEIVE,
                                  null, 0, data, datalen,
                                  IntPtr.Zero, IntPtr.Zero),
                "Receive()");
        }


        /*********************************************************************
         *** Function:   int Receive(byte[] cmd, out byte[] data,          ***
         ***                                                  int datalen) ***
         ***                                                               ***
         *** Parameters: cmd:     Array with command bytes                 ***
         ***             data:    Array for received data                  ***
         ***             datalen: Number of data bytes to receive          ***
         ***                                                               ***
         *** Return:     0: Success                                        ***
         ***             !=0: Error from GetLastWin32Error()               ***
         ***                                                               ***
         *** Description:                                                  ***
         *** ------------                                                  ***
         *** Send the command bytes (all bytes of the array) and then re-  ***
         *** ceive datalen data bytes. Return an array of this size.       ***
         *********************************************************************/
        public int Receive(byte[] cmd, out byte[] data, int datalen)
        {
            data = new byte[datalen];

            return HandleError(
                CEDeviceIoControl(hPort, IOCTL_NSPI_RECEIVE,
                                  cmd, cmd.Length, data, datalen,
                                  IntPtr.Zero, IntPtr.Zero),
                "Receive()");
        }


        /*********************************************************************
         *** Function:   int Receive(byte[] data, int datalen)             ***
         ***                                                               ***
         *** Parameters: data:    Array for received data                  ***
         ***             datalen: Number of data bytes to receive          ***
         ***                                                               ***
         *** Return:     0: Success                                        ***
         ***             !=0: Error from GetLastWin32Error()               ***
         ***                                                               ***
         *** Description:                                                  ***
         *** ------------                                                  ***
         *** Receive datalen data bytes and return them in the given data  ***
         *** array.                                                        ***
         *********************************************************************/
        public int Receive(byte[] data, int datalen)
        {
            if (data.Length < datalen)
                return HandleError("Receive()",
                                   (int)APIError.ERROR_INVALID_PARAMETER);
            return HandleError(
                CEDeviceIoControl(hPort, IOCTL_NSPI_RECEIVE,
                                  null, 0, data, datalen,
                                  IntPtr.Zero, IntPtr.Zero),
                "Receive()");
        }


        /*********************************************************************
         *** Function:   int Receive(byte[] cmd, int cmdlen,               ***
         ***                                     byte[] data, int datalen) ***
         ***                                                               ***
         *** Parameters: cmd:     Array with command bytes                 ***
         ***             cmdlen:  Number of valid bytes in cmd array       ***
         ***             data:    Array for received data                  ***
         ***             datalen: Number of data bytes to receive          ***
         ***                                                               ***
         *** Return:     0: Success                                        ***
         ***             !=0: Error from GetLastWin32Error()               ***
         ***                                                               ***
         *** Description:                                                  ***
         *** ------------                                                  ***
         *** Send the first cmdlen command bytes and then receive datalen  ***
         *** data bytes in the given data array.                           ***
         *********************************************************************/
        public int Receive(byte[] cmd, int cmdlen, byte[] data, int datalen)
        {
            if ((cmd.Length < cmdlen) || (data.Length < datalen))
                return HandleError("Receive()",
                                   (int)APIError.ERROR_INVALID_PARAMETER);
            return HandleError(
                CEDeviceIoControl(hPort, IOCTL_NSPI_RECEIVE,
                                  cmd, cmdlen, data, datalen,
                                  IntPtr.Zero, IntPtr.Zero),
                "Receive()");
        }


        /*********************************************************************
         *** Function:   int Transfer(byte[] sdata, out byte[] rdata)      ***
         ***                                                               ***
         *** Parameters: sdata: Data bytes to send                         ***
         ***             rdata: Array for received data                    ***
         ***                                                               ***
         *** Return:     0: Success                                        ***
         ***             !=0: Error from GetLastWin32Error()               ***
         ***                                                               ***
         *** Description:                                                  ***
         *** ------------                                                  ***
         *** Send the data bytes (all bytes of the array). At the same time***
         *** receive the same number of bytes and return an array of this  ***
         *** size.                                                         ***
         *********************************************************************/
        public int Transfer(byte[] sdata, out byte[] rdata)
        {
            rdata = new byte[sdata.Length];

            return HandleError(
                CEDeviceIoControl(hPort, IOCTL_NSPI_TRANSFER,
                                  sdata, sdata.Length, rdata, rdata.Length,
                                  IntPtr.Zero, IntPtr.Zero),
                "Transfer()");
        }


        /*********************************************************************
         *** Function:   int Transfer(byte[] cmd, byte[] sdata,            ***
         ***                                             out byte[] rdata) ***
         ***                                                               ***
         *** Parameters: cmd:   Array with command bytes                   ***
         ***             sdata: Data bytes to send                         ***
         ***             rdata: Array for received data                    ***
         ***                                                               ***
         *** Return:     0: Success                                        ***
         ***             !=0: Error from GetLastWin32Error()               ***
         ***                                                               ***
         *** Description:                                                  ***
         *** ------------                                                  ***
         *** Send the command bytes (all bytes of the array) and then the  ***
         *** data bytes (all bytes of the array). At the same time receive ***
         *** the same number of bytes and return an array of this size.    ***
         *********************************************************************/
        public int Transfer(byte[] cmd, byte[] sdata, out byte[] rdata)
        {
            rdata = new byte[sdata.Length];

            /* Concatenate command and send data in one array */
            byte[] cmddata = new byte[cmd.Length + sdata.Length];
            cmd.CopyTo(cmddata, 0);
            sdata.CopyTo(cmddata, cmd.Length);

            return HandleError(
                CEDeviceIoControl(hPort, IOCTL_NSPI_TRANSFER,
                                  cmddata, cmddata.Length, rdata, rdata.Length,
                                  IntPtr.Zero, IntPtr.Zero),
                "Transfer()");
        }


        /*********************************************************************
         *** Function:   int Transfer(byte[] sdata, int datalen,           ***
         ***                                             out byte[] rdata) ***
         ***                                                               ***
         *** Parameters: sdata:   Data bytes to send                       ***
         ***             datalen: Number of valid bytes in sdata array     ***
         ***                      (sent) and in rdata (received)           ***
         ***             rdata:   Array for received data                  ***
         ***                                                               ***
         *** Return:     0: Success                                        ***
         ***             !=0: Error from GetLastWin32Error()               ***
         ***                                                               ***
         *** Description:                                                  ***
         *** ------------                                                  ***
         *** Send the first datalen bytes of the sdata array.  At the same ***
         *** time receive datalen bytes  and store them in the given rdata ***
         *** array. The arrays may be larger, but only the given number of ***
         *** bytes are actually transferred.                               ***
         *********************************************************************/
        public int Transfer(byte[] sdata, int datalen, byte[] rdata)
        {
            if ((sdata.Length < datalen) || (rdata.Length < datalen))
                return HandleError("Transfer()",
                                   (int)APIError.ERROR_INVALID_PARAMETER);

            return HandleError(
                CEDeviceIoControl(hPort, IOCTL_NSPI_TRANSFER,
                                  sdata, datalen, rdata, datalen,
                                  IntPtr.Zero, IntPtr.Zero),
                "Transfer()");
        }


        /*********************************************************************
         *** Function:   int Transfer(byte[] cmd, int cmdlen,              ***
         ***                          byte[] sdata, int datalen,           ***
         ***                                             out byte[] rdata) ***
         ***                                                               ***
         *** Parameters: cmd:     Array with command bytes                 ***
         ***             cmdlen:  Number of valid bytes in cmd array       ***
         ***             sdata:   Data bytes to send                       ***
         ***             datalen: Number of valid bytes in sdata array     ***
         ***                      (sent) and in rdata (received)           ***
         ***             rdata:   Array for received data                  ***
         ***                                                               ***
         *** Return:     0: Success                                        ***
         ***             !=0: Error from GetLastWin32Error()               ***
         ***                                                               ***
         *** Description:                                                  ***
         *** ------------                                                  ***
         *** First send the first cmdlen bytes of the cmd array. Then send ***
         *** the first datalen bytes of the sdata array.  At the same time ***
         *** receive datalen bytes and store them in the given rdata array.***
         *** The arrays may be larger,  but only the given number of bytes ***
         *** are actually transferred.                                     ***
         *********************************************************************/
        public int Transfer(byte[] cmd, int cmdlen, byte[] sdata, int datalen,
                            ref byte[] rdata)
        {
            rdata = new byte[datalen];
            if ((cmd.Length < cmdlen)
                || (sdata.Length < datalen) || (rdata.Length < datalen))
                return HandleError("Transfer()",
                                   (int)APIError.ERROR_INVALID_PARAMETER);

            /* Concatenate command and send data in one array */
            byte[] cmddata = new byte[cmdlen + datalen];
            int i;
            for (i = 0; i < cmdlen; i++)
                cmddata[i] = cmd[i];
            for (i = 0; i < datalen; i++)
                cmddata[i + cmdlen] = sdata[i];

            return HandleError(
                CEDeviceIoControl(hPort, IOCTL_NSPI_TRANSFER,
                                  cmddata, cmdlen + datalen, rdata, datalen,
                                  IntPtr.Zero, IntPtr.Zero),
                "Transfer()");
        }


        /*********************************************************************
         *** Function:   int Exchange(byte[] data)                         ***
         ***                                                               ***
         *** Parameters: data: Array with data to send and receive         ***
         ***                                                               ***
         *** Return:     0: Success                                        ***
         ***             !=0: Error from GetLastWin32Error()               ***
         ***                                                               ***
         *** Description:                                                  ***
         *** ------------                                                  ***
         *** Send the data bytes (all bytes of the array). At the same time***
         *** receive the same number of bytes and replace the data in the  ***
         *** array.                                                        ***
         *********************************************************************/
        public int Exchange(byte[] data)
        {
            return HandleError(
                CEDeviceIoControl(hPort, IOCTL_NSPI_EXCHANGE,
                                  null, 0, data, data.Length,
                                  IntPtr.Zero, IntPtr.Zero),
                "Exchange()");
        }


        /*********************************************************************
         *** Function:   int Exchange(byte[] cmd, byte[] data)             ***
         ***                                                               ***
         *** Parameters: cmd:  Array with command bytes                    ***
         ***             data: Array with data to send and receive         ***
         ***                                                               ***
         *** Return:     0: Success                                        ***
         ***             !=0: Error from GetLastWin32Error()               ***
         ***                                                               ***
         *** Description:                                                  ***
         *** ------------                                                  ***
         *** Send the command bytes (all bytes of the array) and then the  ***
         *** data bytes (all bytes of the array). At the same time receive ***
         *** the same number of bytes and replace the data in the array.   ***
         *********************************************************************/
        public int Exchange(byte[] cmd, byte[] data)
        {
            return HandleError(
                CEDeviceIoControl(hPort, IOCTL_NSPI_EXCHANGE,
                                  cmd, cmd.Length, data, data.Length,
                                  IntPtr.Zero, IntPtr.Zero),
                "Exchange()");
        }


        /*********************************************************************
         *** Function:   int Exchange(byte[] data, int datalen)            ***
         ***                                                               ***
         *** Parameters: data:    Array with data to send and receive      ***
         ***             datalen: Number of valid bytes in data array      ***
         ***                                                               ***
         *** Return:     0: Success                                        ***
         ***             !=0: Error from GetLastWin32Error()               ***
         ***                                                               ***
         *** Description:                                                  ***
         *** ------------                                                  ***
         *** Send the first datalen bytes of the data array.   At the same ***
         *** time receive the same number of bytes and replace the data in ***
         *** the array.                                                    ***
         *********************************************************************/
        public int Exchange(byte[] data, int datalen)
        {
            if (data.Length < datalen)
                return HandleError("Exchange()",
                                   (int)APIError.ERROR_INVALID_PARAMETER);

            return HandleError(
                CEDeviceIoControl(hPort, IOCTL_NSPI_EXCHANGE,
                                  null, 0, data, datalen,
                                  IntPtr.Zero, IntPtr.Zero),
                "Exchange()");
        }


        /*********************************************************************
         *** Function:   int Exchange(byte[] cmd, int cmdlen,              ***
         ***                                     byte[] data, int datalen) ***
         ***                                                               ***
         *** Parameters: cmd:     Array with command bytes                 ***
         ***             cmdlen:  Number of valid bytes in cmd array       ***
         ***             data:    Array with data to send and receive      ***
         ***             datalen: Number of valid bytes in data array      ***
         ***                                                               ***
         *** Return:     0: Success                                        ***
         ***             !=0: Error from GetLastWin32Error()               ***
         ***                                                               ***
         *** Description:                                                  ***
         *** ------------                                                  ***
         *** First send the first cmdlen bytes of the cmd array. Then send ***
         *** the first datalen bytes of the data array.   At the same time ***
         *** receive the same number of bytes  and replace the data in the ***
         *** data array.                                                   ***
         *********************************************************************/
        public int Exchange(byte[] cmd, int cmdlen, byte[] data, int datalen)
        {
            if ((cmd.Length < cmdlen) || (data.Length < datalen))
                return HandleError("Exchange()",
                                   (int)APIError.ERROR_INVALID_PARAMETER);
            return HandleError(
                CEDeviceIoControl(hPort, IOCTL_NSPI_EXCHANGE,
                                  cmd, cmdlen, data, datalen,
                                  IntPtr.Zero, IntPtr.Zero),
                "Exchange()");
        }
        #endregion

        #region NspiPortV1: Windows CE API imports
        // CreateFileW()
        [DllImport("coredll.dll", EntryPoint = "CreateFileW",
                   SetLastError = true)]
        private static extern
        IntPtr CECreateFileW(String lpFileName, UInt32 dwDesiredAccess,
                             UInt32 dwShareMode, IntPtr lpSecurityAttributes,
                             UInt32 dwCreationDisposition,
                             UInt32 dwFlagsAndAttributes,
                             IntPtr hTemplateFile);

        // CloseHandle()
        [DllImport("coredll.dll", EntryPoint = "CloseHandle",
                   SetLastError = true)]
        private static extern
        int CECloseHandle(IntPtr hObject);

        // DeviceIoControl()
        [DllImport("coredll.dll", EntryPoint = "DeviceIoControl",
                   SetLastError = true)]
        private static extern
        int CEDeviceIoControl(IntPtr hFile, UInt32 dwIoControlCode,
                              byte[] lpInBuffer, Int32 nInBufferSize,
                              byte[] lpOutBuffer, Int32 nOutBufferSize,
                              IntPtr lpBytesReturned, IntPtr lpOverlapped);
        #endregion
    } // class NspiPortV1
} // namespace FS.NetDCU
