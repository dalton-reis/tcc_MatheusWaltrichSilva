#include <Arduino.h>
#include <IUTHTTP.h>

//VIVO-52C0|Td9VYk7m3W 
IUTHTTP IUTConn;
unsigned int multicastPort = 5000;
char* ssid = "Aquario";
char* password = "aquario-virtual";
char* tokenID = "AQUARIUM_01";

void server(WiFiClient client, String message) {
  if (message && !message.equals("")) {
    Serial.println("Callback...");
    Serial.println("Mensagem: " + message);
    if (message.equals("TESTE")) {
      Serial.println("Received TESTE message");
      client.printf("Received message!");
      client.flush();
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