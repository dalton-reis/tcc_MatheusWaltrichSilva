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
int temperatura;
float temperaturaAnterior = 0;

MCP3008 adc(CLOCK_PIN, MOSI_PIN, MISO_PIN, CS_PIN);

void server(WiFiClient client, String content) {
  temperatura = adc.readADC(0) * 16 / 1023 + 18;
  if (temperatura != temperaturaAnterior) {
    client.println("TEMP|" + temperatura);
    client.flush();  
    Serial.println("TEMP|" + temperatura);
    temperaturaAnterior = temperatura;    
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