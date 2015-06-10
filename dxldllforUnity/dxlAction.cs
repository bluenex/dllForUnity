using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using UnityEngine;

namespace dxldllforUnity
{
    class exportfunc
    {
        public const int MAXNUM_TXPARAM = 150;
        public const int MAXNUM_RXPARAM = 60;

        public const int BROADCAST_ID = 254;

        public const int INST_PING = 1;
        public const int INST_READ = 2;
        public const int INST_WRITE = 3;
        public const int INST_REG_WRITE = 4;
        public const int INST_ACTION = 5;
        public const int INST_RESET = 6;
        public const int INST_SYNC_WRITE = 131;

        public const int ERRBIT_VOLTAGE = 1;
        public const int ERRBIT_ANGLE = 2;
        public const int ERRBIT_OVERHEAT = 4;
        public const int ERRBIT_RANGE = 8;
        public const int ERRBIT_CHECKSUM = 16;
        public const int ERRBIT_OVERLOAD = 32;
        public const int ERRBIT_INSTRUCTION = 64;

        public const int COMM_TXSUCCESS = 0;
        public const int COMM_RXSUCCESS = 1;
        public const int COMM_TXFAIL = 2;
        public const int COMM_RXFAIL = 3;
        public const int COMM_TXERROR = 4;
        public const int COMM_RXWAITING = 5;
        public const int COMM_RXTIMEOUT = 6;
        public const int COMM_RXCORRUPT = 7;


        [DllImport("dynamixel.dll")]
        public static extern int dxl_initialize(int devIndex, int baudnum);

        [DllImport("dynamixel.dll")]
        public static extern void dxl_terminate();

        [DllImport("dynamixel.dll")]
        public static extern int dxl_get_result();

        [DllImport("dynamixel.dll")]
        public static extern void dxl_tx_packet();

        [DllImport("dynamixel.dll")]
        public static extern void dxl_rx_packet();

        [DllImport("dynamixel.dll")]
        public static extern void dxl_txrx_packet();

        [DllImport("dynamixel.dll")]
        public static extern void dxl_set_txpacket_id(int id);

        [DllImport("dynamixel.dll")]
        public static extern void dxl_set_txpacket_instruction(int instruction);

        [DllImport("dynamixel.dll")]
        public static extern void dxl_set_txpacket_parameter(int index, int value);

        [DllImport("dynamixel.dll")]
        public static extern void dxl_set_txpacket_length(int length);

        [DllImport("dynamixel.dll")]
        public static extern int dxl_get_rxpacket_error(int errbit);

        [DllImport("dynamixel.dll")]
        public static extern int dxl_get_rxpacket_length();

        [DllImport("dynamixel.dll")]
        public static extern int dxl_get_rxpacket_parameter(int index);

        [DllImport("dynamixel.dll")]
        public static extern int dxl_makeword(int lowbyte, int highbyte);

        [DllImport("dynamixel.dll")]
        public static extern int dxl_get_lowbyte(int word);

        [DllImport("dynamixel.dll")]
        public static extern int dxl_get_highbyte(int word);

        [DllImport("dynamixel.dll")]
        public static extern void dxl_ping(int id);

        [DllImport("dynamixel.dll")]
        public static extern int dxl_read_byte(int id, int address);

        [DllImport("dynamixel.dll")]
        public static extern void dxl_write_byte(int id, int address, int value);

        [DllImport("dynamixel.dll")]
        public static extern int dxl_read_word(int id, int address);

        [DllImport("dynamixel.dll")]
        public static extern void dxl_write_word(int id, int address, int value);
    }

    public class dxlAction
    {
        // Control table address
        public const int P_GOAL_POSITION_L = 30;
        public const int P_GOAL_POSITION_H = 31;
        public const int P_PRESENT_POSITION_L = 36;
        public const int P_PRESENT_POSITION_H = 37;
        public const int P_MOVING = 46;


       


        public int defaultPort;
        public int defaultBaudrate;
        public int defaultSpeed;
        public int setID;
        public const int broadcastID = 254;
        public const int defaultMotorID = broadcastID;
        public int[] zeroArray = new int[0]; // use for returning value

        public dxlAction() // Constructors
        {
            // pre-assign default values
            defaultPort = 3;
            defaultBaudrate = 1; // # Ax12 = 1, Mx64 = 34
            defaultSpeed = 300;
        }
        public dxlAction(int port, int baudrate) // overloads for 2 arguments
        {
            this.defaultPort = port;
            this.defaultBaudrate = baudrate;
            this.defaultSpeed = 300;
        }
        public dxlAction(int port, int baudrate, int motorID) // overloads for 2 arguments
        {
            this.defaultPort = port;
            this.defaultBaudrate = baudrate;
            this.setID = motorID;
            this.defaultSpeed = 256;
        }
        public dxlAction(int port, int baudrate, int motorID, int speed) // overloads for 3 arguments
        {
            this.defaultPort = port;
            this.defaultBaudrate = baudrate;
            this.setID = motorID;
            this.defaultSpeed = speed;
        }

        // read present position by using control table address
        #region connection and error
        public void PrintCommStatus(int CommStatus)
        {
            switch (CommStatus)
            {
                case exportfunc.COMM_TXFAIL:
                    Debug.Log("COMM_TXFAIL: Failed transmit instruction packet!");
                    break;

                case exportfunc.COMM_TXERROR:
                    Debug.Log("COMM_TXERROR: Incorrect instruction packet!");
                    break;

                case exportfunc.COMM_RXFAIL:
                    Debug.Log("COMM_RXFAIL: Failed get status packet from device!");
                    break;

                case exportfunc.COMM_RXWAITING:
                    Debug.Log("COMM_RXWAITING: Now recieving status packet!");
                    break;

                case exportfunc.COMM_RXTIMEOUT:
                    Debug.Log("COMM_RXTIMEOUT: There is no status packet!");
                    break;

                case exportfunc.COMM_RXCORRUPT:
                    Debug.Log("COMM_RXCORRUPT: Incorrect status packet!");
                    break;

                default:
                    Debug.Log("This is unknown error code!");
                    break;
            }
        }

        public void PrintErrorCode()
        {
            if (exportfunc.dxl_get_rxpacket_error(exportfunc.ERRBIT_VOLTAGE) == 1)
                Debug.Log("Input voltage error!");

            if (exportfunc.dxl_get_rxpacket_error(exportfunc.ERRBIT_ANGLE) == 1)
                Debug.Log("Angle limit error!");

            if (exportfunc.dxl_get_rxpacket_error(exportfunc.ERRBIT_OVERHEAT) == 1)
                Debug.Log("Overheat error!");

            if (exportfunc.dxl_get_rxpacket_error(exportfunc.ERRBIT_RANGE) == 1)
                Debug.Log("Out of range error!");

            if (exportfunc.dxl_get_rxpacket_error(exportfunc.ERRBIT_CHECKSUM) == 1)
                Debug.Log("Checksum error!");

            if (exportfunc.dxl_get_rxpacket_error(exportfunc.ERRBIT_OVERLOAD) == 1)
                Debug.Log("Overload error!");

            if (exportfunc.dxl_get_rxpacket_error(exportfunc.ERRBIT_INSTRUCTION) == 1)
                Debug.Log("Instruction code error!");
        }

        public void dxlInit()
        {
            if (exportfunc.dxl_initialize(defaultPort, defaultBaudrate) == 1)
            {
                Debug.Log("Succeed to open USB2Dynamixel");
            }
            else
            {
                Debug.Log("Failed to open USB2Dynamixel");
            }
        }

        public void commStatus()
        {
            int CommStatus = exportfunc.dxl_get_result();
            if (CommStatus == exportfunc.COMM_RXSUCCESS)
            {

            }
        }

        public void dxlTerminate()
        {
            exportfunc.dxl_terminate();
        }
        #endregion

        #region new read
        public int readPosWord()
        {
            int presentPos = exportfunc.dxl_read_word(setID, P_PRESENT_POSITION_L);
            return presentPos;
        }

        public int readPosWord(int motorID)
        {
            int presentPos = exportfunc.dxl_read_word(motorID, P_PRESENT_POSITION_L);
            return presentPos;
        }
        #endregion

        #region new write
        public void writePosWord(int target)
        {
            exportfunc.dxl_write_word(setID, P_GOAL_POSITION_L, target);
        }

        public void readPosWord(int motorID, int target)
        {
            exportfunc.dxl_write_word(motorID, P_GOAL_POSITION_L, target);
        }
        #endregion

        //#region old read
        //public int readPosWord()
        //{
        //    if (exportfunc.dxl_initialize(defaultPort, defaultBaudrate) == 1)
        //    {
        //        int presentPos = exportfunc.dxl_read_word(setID, P_PRESENT_POSITION_L);
        //        return presentPos;
        //    }
        //    else
        //    {
        //        Console.WriteLine("Cannot connect to USB2Dynamixel..");
        //    }
        //    // close device
        //    exportfunc.dxl_terminate();
        //    return 0;
        //}
        
        //public int readPosWord(int motorID)
        //{
        //    if (exportfunc.dxl_initialize(defaultPort, defaultBaudrate) == 1)
        //    {
        //        int presentPos = exportfunc.dxl_read_word(motorID, P_PRESENT_POSITION_L);
        //        return presentPos;
        //    }
        //    else
        //    {
        //        Console.WriteLine("Cannot connect to USB2Dynamixel..");
        //    }
        //    // close device
        //    exportfunc.dxl_terminate();
        //    return 0;
        //}
        //#endregion

        // methods copied from old lib
        // the methods will be real needed are read, write, regwrite, regaction, torque, led, backhome
        
        public int[] scanID() // use maximum as broadcastID
        {
            int[] foundID = new int[254]; // declare array for all motor ID

            if (exportfunc.dxl_initialize(defaultPort, defaultBaudrate) == 1)
            {
                Console.WriteLine("Succeed to connect!");
                int ID = 0;
                int motorCount = 0;

                //while (ID < broadcastID)
                while (ID < broadcastID)
                {
                    double percent = (ID * 100) / broadcastID;
                    Console.WriteLine("Searching.. {0:F0}%", percent);

                    exportfunc.dxl_ping(ID);
                    if (exportfunc.dxl_get_result() == exportfunc.COMM_RXSUCCESS)
                    {
                        //Console.WriteLine("Found motor ID: {0}", ID);
                        if (ID != 254)
                        {
                            foundID[motorCount] = ID;
                        }
                        motorCount++;
                    }
                    ID++;
                }

                int[] onlyFoundID = new int[motorCount]; // declare array for only found motor ID
                for (int i = 0; i <= motorCount - 1; i++)
                {
                    onlyFoundID[i] = foundID[i];
                }

                Console.Write("Found motor ID: ");
                for (int i = 0; i < motorCount; i++)
                {
                    Console.Write("{0} ", onlyFoundID[i]);
                }
                Console.WriteLine();
                return onlyFoundID;
            }

            else { Console.WriteLine("Cannot connect to USB2Dynamixel"); }

            // close device
            exportfunc.dxl_terminate();
            return zeroArray;
        }

        public int[] scanID(int maxMotor) // use maximum motor ID number
        {
            if (maxMotor <= 254)
            {
                int[] foundID = new int[254]; // declare array for all motor ID

                if (exportfunc.dxl_initialize(defaultPort, defaultBaudrate) == 1)
                {
                    Console.WriteLine("Succeed to connect!");
                    int ID = 0;
                    int motorCount = 0;

                    //while (ID < broadcastID)
                    while (ID < maxMotor)
                    {
                        double percent = (ID * 100) / maxMotor;
                        Console.WriteLine("Searching.. {0:F0}%", percent);

                        exportfunc.dxl_ping(ID);
                        if (exportfunc.dxl_get_result() == exportfunc.COMM_RXSUCCESS)
                        {
                            //Console.WriteLine("Found motor ID: {0}", ID);
                            if (ID != 254)
                            {
                                foundID[motorCount] = ID;
                            }
                            motorCount++;
                        }
                        ID++;
                    }

                    int[] onlyFoundID = new int[motorCount]; // declare array for only found motor ID
                    for (int i = 0; i <= motorCount - 1; i++)
                    {
                        onlyFoundID[i] = foundID[i];
                    }

                    Console.Write("Found motor ID: ");
                    for (int i = 0; i < motorCount; i++)
                    {
                        Console.Write("{0} ", onlyFoundID[i]);
                    }
                    Console.WriteLine();
                    return onlyFoundID;

                }
                else
                {
                    Console.WriteLine("Cannot connect to USB2Dynamixel");
                }
                // close device
                exportfunc.dxl_terminate();
                return zeroArray;
            }
            else
            {
                Console.WriteLine("Maximum motor id is 254");
                return zeroArray;
            }
        }    
        
        public void ledControl(int status) // broadcastID, status: 0 = off, 1 = on
        {
            if (exportfunc.dxl_initialize(defaultPort, defaultBaudrate) == 1)
            {
                switch (status)
                {
                    case 0: // turn LED off
                        exportfunc.dxl_set_txpacket_id(broadcastID);
                        exportfunc.dxl_set_txpacket_length(4);
                        exportfunc.dxl_set_txpacket_instruction(3);
                        exportfunc.dxl_set_txpacket_parameter(0, 25);
                        exportfunc.dxl_set_txpacket_parameter(1, 0);
                        exportfunc.dxl_tx_packet();
                        Console.WriteLine("LED turned off");
                        break;
                    case 1: // turn LED on
                        exportfunc.dxl_set_txpacket_id(broadcastID);
                        exportfunc.dxl_set_txpacket_length(4);
                        exportfunc.dxl_set_txpacket_instruction(3);
                        exportfunc.dxl_set_txpacket_parameter(0, 25);
                        exportfunc.dxl_set_txpacket_parameter(1, 1);
                        exportfunc.dxl_tx_packet();
                        Console.WriteLine("LED turned on");
                        break;
                    default: Console.WriteLine("Invalid input!");
                        Console.WriteLine("Your input is {0}: please use 0 = OFF and 1 = ON", status);
                        break;
                }
            }
            else
            {
                Console.WriteLine("Cannot connect to USB2Dynamixel..");
            }
            // close device
            exportfunc.dxl_terminate();
        }

        public void ledControl(int motorID, int status) // specificID, status: 0 = off, 1 = on
        {
            if (exportfunc.dxl_initialize(defaultPort, defaultBaudrate) == 1)
            {
                switch (status)
                {
                    case 0: // turn LED off
                        exportfunc.dxl_set_txpacket_id(motorID);
                        exportfunc.dxl_set_txpacket_length(4);
                        exportfunc.dxl_set_txpacket_instruction(3);
                        exportfunc.dxl_set_txpacket_parameter(0, 25);
                        exportfunc.dxl_set_txpacket_parameter(1, 0);
                        exportfunc.dxl_tx_packet();
                        Console.WriteLine("LED turned off");
                        break;
                    case 1: // turn LED on
                        exportfunc.dxl_set_txpacket_id(motorID);
                        exportfunc.dxl_set_txpacket_length(4);
                        exportfunc.dxl_set_txpacket_instruction(3);
                        exportfunc.dxl_set_txpacket_parameter(0, 25);
                        exportfunc.dxl_set_txpacket_parameter(1, 1);
                        exportfunc.dxl_tx_packet();
                        Console.WriteLine("LED turned on");
                        break;
                    default: Console.WriteLine("Invalid input!");
                        Console.WriteLine("Your input is {0}: please use 0 = OFF and 1 = ON", status);
                        break;
                }
            }
            else
            {
                Console.WriteLine("Cannot connect to USB2Dynamixel..");
            }
            // close device
            exportfunc.dxl_terminate();
        }

        public void torqueControl(int status) // broadcastID, status: 0 = disable, 1 = enable
        {
            if (exportfunc.dxl_initialize(defaultPort, defaultBaudrate) == 1)
            {
                switch (status)
                {
                    case 0: // turn torque off
                        exportfunc.dxl_set_txpacket_id(broadcastID);
                        exportfunc.dxl_set_txpacket_length(4);
                        exportfunc.dxl_set_txpacket_instruction(3);
                        exportfunc.dxl_set_txpacket_parameter(0, 24);
                        exportfunc.dxl_set_txpacket_parameter(1, status);
                        exportfunc.dxl_tx_packet();
                        Console.WriteLine("Torque disabled");
                        ledControl(0);
                        break;
                    case 1: // turn torque on
                        exportfunc.dxl_set_txpacket_id(broadcastID);
                        exportfunc.dxl_set_txpacket_length(4);
                        exportfunc.dxl_set_txpacket_instruction(3);
                        exportfunc.dxl_set_txpacket_parameter(0, 24);
                        exportfunc.dxl_set_txpacket_parameter(1, status);
                        exportfunc.dxl_tx_packet();
                        Console.WriteLine("Torque enabled");
                        ledControl(1);
                        break;
                    default: Console.WriteLine("Invalid input!");
                        Console.WriteLine("Your input is {0}: please use 0 = OFF and 1 = ON", status);
                        break;
                }
            }
            else
            {
                Console.WriteLine("Cannot connect to USB2Dynamixel..");
            }
            // close device
            exportfunc.dxl_terminate();
        }

        public void torqueControl(int motorID, int status) // specificID, status: 0 = disable, 1 = enable
        {
            if (exportfunc.dxl_initialize(defaultPort, defaultBaudrate) == 1)
            {
                switch (status)
                {
                    case 0: // turn torque off
                        exportfunc.dxl_set_txpacket_id(motorID);
                        exportfunc.dxl_set_txpacket_length(4);
                        exportfunc.dxl_set_txpacket_instruction(3);
                        exportfunc.dxl_set_txpacket_parameter(0, 24);
                        exportfunc.dxl_set_txpacket_parameter(1, status);
                        exportfunc.dxl_tx_packet();
                        Console.WriteLine("Torque disabled");
                        ledControl(0);
                        break;
                    case 1: // turn torque on
                        exportfunc.dxl_set_txpacket_id(motorID);
                        exportfunc.dxl_set_txpacket_length(4);
                        exportfunc.dxl_set_txpacket_instruction(3);
                        exportfunc.dxl_set_txpacket_parameter(0, 24);
                        exportfunc.dxl_set_txpacket_parameter(1, status);
                        exportfunc.dxl_tx_packet();
                        Console.WriteLine("Torque enabled");
                        ledControl(1);
                        break;
                    default: Console.WriteLine("Invalid input!");
                        Console.WriteLine("Your input is {0}: please use 0 = OFF and 1 = ON", status);
                        break;
                }
            }
            else
            {
                Console.WriteLine("Cannot connect to USB2Dynamixel..");
            }
            // close device
            exportfunc.dxl_terminate();
        }

        public void backHome() // broadcastID
        {
            if (exportfunc.dxl_initialize(defaultPort, defaultBaudrate) == 1)
            {
                //ledControl(1);
                exportfunc.dxl_set_txpacket_id(broadcastID);
                exportfunc.dxl_set_txpacket_length(7);
                exportfunc.dxl_set_txpacket_instruction(3);
                exportfunc.dxl_set_txpacket_parameter(0, 30);
                exportfunc.dxl_set_txpacket_parameter(1, 512 % 256);
                exportfunc.dxl_set_txpacket_parameter(2, 512 / 256);
                exportfunc.dxl_set_txpacket_parameter(3, defaultSpeed % 256); // it was 512 % 256 
                exportfunc.dxl_set_txpacket_parameter(4, defaultSpeed / 256); // changed to be 360 to reduce speed
                exportfunc.dxl_tx_packet();
                //ledControl(0);
            }
            else
            {
                Console.WriteLine("Cannot connect to USB2Dynamixel");
            }
            // close device
            exportfunc.dxl_terminate();
        }

        public void backHome(int motorID) // specificID
        {
            if (exportfunc.dxl_initialize(defaultPort, defaultBaudrate) == 1)
            {
                //ledControl(1);
                exportfunc.dxl_set_txpacket_id(motorID);
                exportfunc.dxl_set_txpacket_length(7);
                exportfunc.dxl_set_txpacket_instruction(3);
                exportfunc.dxl_set_txpacket_parameter(0, 30);
                exportfunc.dxl_set_txpacket_parameter(1, 512 % 256);
                exportfunc.dxl_set_txpacket_parameter(2, 512 / 256);
                exportfunc.dxl_set_txpacket_parameter(3, defaultSpeed % 256); // it was 512 % 256 
                exportfunc.dxl_set_txpacket_parameter(4, defaultSpeed / 256); // changed to be 360 to reduce speed
                exportfunc.dxl_tx_packet();
                //ledControl(0);
            }
            else
            {
                Console.WriteLine("Cannot connect to USB2Dynamixel");
            }
            // close device
            exportfunc.dxl_terminate();
        }

        //public void backHome(int speed) // broadcastID
        //{
        //    if (exportfunc.dxl_initialize(defaultPort, defaultBaudrate) == 1)
        //    {
        //        //ledControl(1);
        //        exportfunc.dxl_set_txpacket_id(broadcastID);
        //        exportfunc.dxl_set_txpacket_length(7);
        //        exportfunc.dxl_set_txpacket_instruction(3);
        //        exportfunc.dxl_set_txpacket_parameter(0, 30);
        //        exportfunc.dxl_set_txpacket_parameter(1, 512 % 256);
        //        exportfunc.dxl_set_txpacket_parameter(2, 512 / 256);
        //        exportfunc.dxl_set_txpacket_parameter(3, speed % 256); // it was 512 % 256 
        //        exportfunc.dxl_set_txpacket_parameter(4, speed / 256); // changed to be 360 to reduce speed
        //        exportfunc.dxl_tx_packet();
        //        //ledControl(0);
        //    }
        //    else
        //    {
        //        Console.WriteLine("Cannot connect to USB2Dynamixel");
        //    }
        //    // close device
        //    exportfunc.dxl_terminate();
        //}

        public void backHome(int motorID, int speed) // specificID
        {
            if (exportfunc.dxl_initialize(defaultPort, defaultBaudrate) == 1)
            {
                //ledControl(1);
                exportfunc.dxl_set_txpacket_id(motorID);
                exportfunc.dxl_set_txpacket_length(7);
                exportfunc.dxl_set_txpacket_instruction(3);
                exportfunc.dxl_set_txpacket_parameter(0, 30);
                exportfunc.dxl_set_txpacket_parameter(1, 512 % 256);
                exportfunc.dxl_set_txpacket_parameter(2, 512 / 256);
                exportfunc.dxl_set_txpacket_parameter(3, speed % 256); // it was 512 % 256 
                exportfunc.dxl_set_txpacket_parameter(4, speed / 256); // changed to be 360 to reduce speed
                exportfunc.dxl_tx_packet();
                //ledControl(0);
            }
            else
            {
                Console.WriteLine("Cannot connect to USB2Dynamixel");
            }
            // close device
            exportfunc.dxl_terminate();
        }

        public void drivePosition(int position) // broadcastID
            {
                if (exportfunc.dxl_initialize(defaultPort, defaultBaudrate) == 1)
                {
                    exportfunc.dxl_set_txpacket_id(defaultMotorID);
                    exportfunc.dxl_set_txpacket_length(7);
                    exportfunc.dxl_set_txpacket_instruction(3);
                    exportfunc.dxl_set_txpacket_parameter(0, 30);
                    exportfunc.dxl_set_txpacket_parameter(1, position % 256);
                    exportfunc.dxl_set_txpacket_parameter(2, position / 256);
                    exportfunc.dxl_set_txpacket_parameter(3, defaultSpeed % 256);
                    exportfunc.dxl_set_txpacket_parameter(4, defaultSpeed / 256);
                    exportfunc.dxl_tx_packet();
                }
                else
                {
                    Console.WriteLine("Cannot connect to USB2Dynamixel");
                }
                // close device
                exportfunc.dxl_terminate();
            }

        public void drivePosition(int motorID, int position) // specificID, defaultSpeed
        {
            if (exportfunc.dxl_initialize(defaultPort, defaultBaudrate) == 1)
            {
                exportfunc.dxl_set_txpacket_id(motorID);
                exportfunc.dxl_set_txpacket_length(7);
                exportfunc.dxl_set_txpacket_instruction(3);
                exportfunc.dxl_set_txpacket_parameter(0, 30);
                exportfunc.dxl_set_txpacket_parameter(1, position % 256);
                exportfunc.dxl_set_txpacket_parameter(2, position / 256);
                exportfunc.dxl_set_txpacket_parameter(3, defaultSpeed % 256);
                exportfunc.dxl_set_txpacket_parameter(4, defaultSpeed / 256);
                exportfunc.dxl_tx_packet();
            }
            else
            {
                Console.WriteLine("Cannot connect to USB2Dynamixel");
            }
            // close device
            exportfunc.dxl_terminate();
        }      

        public void drivePosition(int motorID, int position, int speed) // specificID
        {
            if (exportfunc.dxl_initialize(defaultPort, defaultBaudrate) == 1)
            {
                //ledControl(1);
                exportfunc.dxl_set_txpacket_id(motorID);
                //Console.WriteLine(motorID);
                exportfunc.dxl_set_txpacket_length(7);
                exportfunc.dxl_set_txpacket_instruction(3);
                exportfunc.dxl_set_txpacket_parameter(0, 30);
                exportfunc.dxl_set_txpacket_parameter(1, position % 256);
                exportfunc.dxl_set_txpacket_parameter(2, position / 256);
                exportfunc.dxl_set_txpacket_parameter(3, speed % 256);
                exportfunc.dxl_set_txpacket_parameter(4, speed / 256);
                exportfunc.dxl_tx_packet();

                //ledControl(0);
            }
            else
            {
                Console.WriteLine("Cannot connect to USB2Dynamixel");
            }
            // close device
            exportfunc.dxl_terminate();
        }

        public int readPosition(int motorID) // specificID
        {
            if (exportfunc.dxl_initialize(defaultPort, defaultBaudrate) == 1)
            {
                int low = exportfunc.dxl_read_byte(motorID, 36);
                int high = exportfunc.dxl_read_byte(motorID, 37);
                //Console.WriteLine("low byte = {0} high byte = {1}", low, high);
                int presentPos = (high * 256) + low;
                //Console.WriteLine("Present position is {0} ",presentPos);
                return presentPos;
            }
            else
            {
                Console.WriteLine("Cannot connect to USB2Dynamixel..");
            }
            // close device
            exportfunc.dxl_terminate();
            return 0;
        }

        public void regWrite(int position, int speed) // broadcastID
        {
            if (exportfunc.dxl_initialize(defaultPort, defaultBaudrate) == 1)
            {
                exportfunc.dxl_set_txpacket_id(broadcastID);
                exportfunc.dxl_set_txpacket_length(7);
                exportfunc.dxl_set_txpacket_instruction(4);
                exportfunc.dxl_set_txpacket_parameter(0, 30);
                exportfunc.dxl_set_txpacket_parameter(1, position % 256);
                exportfunc.dxl_set_txpacket_parameter(2, position / 256);
                exportfunc.dxl_set_txpacket_parameter(3, speed % 256);
                exportfunc.dxl_set_txpacket_parameter(4, speed / 256);
                exportfunc.dxl_tx_packet();
                Console.WriteLine("Successfully write REG packet!");
            }
            else
            {
                Console.WriteLine("Cannot connect to USB2Dynamixel");
            }
            // close device
            exportfunc.dxl_terminate();
        }

        public void regWrite(int motorID, int position, int speed) // specificID
        {
            if (exportfunc.dxl_initialize(defaultPort, defaultBaudrate) == 1)
            {
                exportfunc.dxl_set_txpacket_id(motorID);
                exportfunc.dxl_set_txpacket_length(7);
                exportfunc.dxl_set_txpacket_instruction(4);
                exportfunc.dxl_set_txpacket_parameter(0, 30);
                exportfunc.dxl_set_txpacket_parameter(1, position % 256);
                exportfunc.dxl_set_txpacket_parameter(2, position / 256);
                exportfunc.dxl_set_txpacket_parameter(3, speed % 256);
                exportfunc.dxl_set_txpacket_parameter(4, speed / 256);
                exportfunc.dxl_tx_packet();
                Console.WriteLine("Successfully write REG packet!");
            }
            else
            {
                Console.WriteLine("Cannot connect to USB2Dynamixel");
            }
            // close device
            exportfunc.dxl_terminate();
        }

        public void regAction() // broadcastID
        {
            if (exportfunc.dxl_initialize(defaultPort, defaultBaudrate) == 1)
            {
                exportfunc.dxl_set_txpacket_id(broadcastID);
                exportfunc.dxl_set_txpacket_length(2);
                exportfunc.dxl_set_txpacket_instruction(5);
                exportfunc.dxl_tx_packet();
                Console.WriteLine("REG packet is started!");
            }
            else
            {
                Console.WriteLine("Cannot REGAction, please connect to USB2Dynamixel..");
            }
            // close device
            exportfunc.dxl_terminate();
        }

        public void regAction(int motorID) // specificID
        {
            if (exportfunc.dxl_initialize(defaultPort, defaultBaudrate) == 1)
            {
                exportfunc.dxl_set_txpacket_id(motorID);
                exportfunc.dxl_set_txpacket_length(2);
                exportfunc.dxl_set_txpacket_instruction(5);
                exportfunc.dxl_tx_packet();
                Console.WriteLine("REG packet is started!");
            }
            else
            {
                Console.WriteLine("Cannot REGAction, please connect to USB2Dynamixel..");
            }
            // close device
            exportfunc.dxl_terminate();
        }
    }
}

