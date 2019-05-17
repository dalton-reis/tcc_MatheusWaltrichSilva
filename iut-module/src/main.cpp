#include <Arduino.h>
#include <IUTConnect.h>
#include <MCP3008.h>

#define CS_PIN D1
#define CLOCK_PIN D5
#define MOSI_PIN D7
#define MISO_PIN D6

IUTConnect IUTConn;
unsigned int multicastPort = 5000;
char* ssid = "Aquario";
char* password = "aquario-virtual";
char* tokenID = "AQUARIUM_01";
float temperatura;
float temperaturaAnterior = 0;

MCP3008 adc(CLOCK_PIN, MOSI_PIN, MISO_PIN, CS_PIN);

void server(WiFiClient client, String content) {
  temperatura = adc.readADC(0) * 14 / 1023 + 19;
  if (temperatura != temperaturaAnterior) {
    client.println(temperatura);
    client.flush();  
    Serial.println(temperatura);
    temperaturaAnterior = temperatura;
  }
  if (content && !content.equals("")) {    
    Serial.println("Mensagem: " + content);    
    if (content.equals("TESTE")) {
      Serial.println("Received TESTE message");
      client.println("Received message!");
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