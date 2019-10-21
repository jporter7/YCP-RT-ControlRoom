

String incoming = "";   // for incoming serial string data
int incomingByte = 0;
void setup() {
        Serial.begin(9600);     // opens serial port, sets data rate to 9600 bps
}

void loop() {
  // send data only when you receive data:
  /*
  if (Serial.available() > 0) {
    // read the incoming:
    incoming = Serial.readString();
    // say what you got:
    Serial.println(incoming);   
    if (incoming == 'demo') {
      //demo routine 
      Serial.println("you started the demo routine");
    }
    else if (incoming=='sort') {
      //sorteer routine
      Serial.println("you started the sort routine");
    }     
    else {
      //junk
      Serial.println("something else");
      incoming = "";
    }
    Serial.flush();
  } 
  */
  if (Serial.available() > 0) {
    int v=0;
    char byteArray[1000];
    for( int i = 0; i < sizeof(byteArray);  ++i )
      byteArray[i] = (char)0;
    //strcpy((char *)byteArray,"0123");
    
    while(true){
      incomingByte = Serial.read(); 
      if(incomingByte==-1||incomingByte==10){break;}
      byteArray[v]= incomingByte;
      v++;
    }
    String myString = String(byteArray);
    Serial.println(myString);

    // read the incoming byte:  

  }
}
