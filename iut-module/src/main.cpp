#include <Arduino.h>
#include <IUTConnect.h>

//VIVO-52C0|Td9VYk7m3W 
IUTConnect IUTConn;
unsigned int multicastPort = 5000;
char* ssid = "Aquario";
char* password = "aquario-virtual";
char* tokenID = "AQUARIUM_01";

void server(String identifier, String content) {
  if (content && !content.equals("")) {    
    Serial.println("Mensagem: " + content);
    if (content.equals("TESTE")) {
      Serial.println("Received TESTE message");
      IUTConn.sendMessage("TESTE", "Received message!");      
    }
  }
}

void setup() {
  Serial.begin(9600);  
  IUTConn.setSocketCallback(server);
  IUTConn.start(ssid, password, tokenID);
}

void loop() {  
  IUTConn.listenSocket();
}