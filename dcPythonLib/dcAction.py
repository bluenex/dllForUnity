import serial
import threading


class dcAction(threading.Thread):
    def __init__(self, port="COM4", baudRate=115200):
        threading.Thread.__init__(self)
        # self.port = port
        # self.baudRate = baudRate
        self.arduino = serial.Serial(port=port, baudrate=baudRate)

    def Open(self):
        # arduino = serial.Serial(port=self.port, baudrate=self.baudRate)
        # arduino.open()
        self.arduino.open()

    def Write(self, messages):
        self.arduino.write(messages)

    def WritePWM(self, pwmValues):
        if (pwmValues < 100 and pwmValues >= 10):
            pwmString = "0" + str(pwmValues)
        elif (pwmValues < 10 and pwmValues >= 0):
            pwmString = "00" + str(pwmValues)
        else:
            pwmString = str(pwmValues)

        self.arduino.write("M" + pwmString)

    def Close(self):
        self.WritePWM(0)
        self.arduino.close()

    def IsOpen(self):
        return self.arduino._isOpen

    def ReadLine(self):
        return self.arduino.readline()

# # connect to arduino on mac
# macDuino = dcAction(port="/dev/cu.usbmodem1411")
# # open arduino connection
# # macDuino.Open()
# driveSpeed = 0
#
# while (driveSpeed != 256):
#     driveSpeed = input("speed you want?: ")
#     macDuino.WritePWM(driveSpeed)
#
# macDuino.Close()
