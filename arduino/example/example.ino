#define PIN_LED 13

char buffer;
int time_delay = 1000;

void setup()
{
    Serial.begin(115200);
    pinMode(PIN_LED, OUTPUT);
}

void loop()
{ 
    digitalWrite(PIN_LED, HIGH);
    Serial.println("ON");
    delay(time_delay);
    digitalWrite(PIN_LED, LOW);
    Serial.println("OFF");
    delay(time_delay);
    if(Serial.available() > 0){
        buffer = Serial.read();
        Serial.print("Incoming = ");
        Serial.println(buffer);
        if(buffer =='D'){
            time_delay = 0;
            time_delay += (Serial.read()-48)*1000;
            time_delay += (Serial.read()-'0')*100;
            time_delay += (Serial.read()-'0')*10;
            time_delay += (Serial.read()-'0');
            Serial.println(time_delay);
        }
    }
}

/*
millis();
delay();
delayMicroseconds();

// pinMode();
// digitalWrite();

// analogWrite(8, 255);


bool HIGH;
digitalRead(PIN);

int a;
analogRead(PIN);

*/
