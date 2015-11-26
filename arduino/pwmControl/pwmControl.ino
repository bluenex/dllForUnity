#define PIN_DC_MOTOR 11
#define PIN_LED 13

char buffer;
int speedMotor = 0;
int timeDelay = 10;

void setup()
{
  Serial.begin(115200);
  pinMode(PIN_DC_MOTOR, OUTPUT);
  pinMode(PIN_LED, OUTPUT);
}

void loop()
{
  digitalWrite(PIN_LED, LOW);
  delay(timeDelay);
  if(Serial.available() > 0)
  {
      buffer = Serial.read();
      /*Serial.print("Incoming buffer = ");
      Serial.println(buffer);*/
      
      /*if(buffer == 'T')
      {
          timeDelay = 0;
          timeDelay += (Serial.read()-'0')*1000;
          timeDelay += (Serial.read()-'0')*100;
          timeDelay += (Serial.read()-'0')*10;
          timeDelay += (Serial.read()-'0');
          Serial.print("time delay is set as ");
          Serial.println(timeDelay);
      }*/
      
      if(buffer == 'M')
      {
          speedMotor = 0;
          speedMotor += (Serial.read()-'0')*100;
          speedMotor += (Serial.read()-'0')*10;
          speedMotor += (Serial.read()-'0');
          /*Serial.print("motor speed = ");
          Serial.println(speedMotor);*/
          digitalWrite(PIN_LED, HIGH);
          if(speedMotor >= 0)
          {
            if(speedMotor < 256)
            {
              analogWrite(PIN_DC_MOTOR, speedMotor);
              /*Serial.print("motor speed is set at ");*/
              Serial.println(speedMotor);  
            }
            /*else
            {
              Serial.println("motor speed is not set.");
            }*/  
          }
          /*else
          {
            Serial.println("motor speed is not set.");
          }*/ 
      }
  }
}
