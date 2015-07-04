using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Timers;

namespace dcDLLforUnity
{
    public class dcAction
    {
        public string arduinoPort = "COM4";
        public int baudRate = 115200;
        public SerialPort arduino;

        public dcAction(string port)
        {
            arduinoPort = port;
            baudRate = 115200;
        }
        public dcAction(string port, int baud)
        {
            arduinoPort = port;
            baudRate = baud;
        }

        public void Open()
        {
            arduino = new SerialPort(arduinoPort, baudRate);
            arduino.Open();
        }

        public void Write(string messages)
        {
            arduino.Write(messages);
        }

        public void WritePWM(int pwmValues)
        {
            string pwmString;
            if (pwmValues < 100 && pwmValues >= 10) { pwmString = "0" + pwmValues.ToString(); }
            else if (pwmValues < 10 && pwmValues >= 0) { pwmString = "00" + pwmValues.ToString(); }
            else { pwmString = pwmValues.ToString(); }
            arduino.Write("M" + pwmString);
        }

        public void Close()
        {
            WritePWM(0); // make sure to set PWM = 0
            arduino.Close();
        }

        public bool IsOpen()
        {
            return arduino.IsOpen;
        }

        public string ReadLine()
        {
            return arduino.ReadLine();
        }

        public string ReadTo(string values)
        {
            return arduino.ReadTo(values);
        }
    }
}
