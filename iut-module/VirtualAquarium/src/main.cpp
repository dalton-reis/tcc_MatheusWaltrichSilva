#include <Arduino.h>
#include <IUTConnect.h>
#include <MCP3008.h>

#define CS_PIN D1
#define CLOCK_PIN D4
#define MOSI_PIN D2
#define MISO_PIN D3
#define LIGADO_PIN D5
#define WIFI_PIN D6
#define CONECTADO_PIN D7

IUTConnect IUTConn;
int temperatura = 25;
int potenciometroValue = 0;
int temperaturaAnterior = 0;
int ldrValue = 0;
int luz = 0;
int luzAnterior = 0;
int botaoValue = 0;
int comida = 0;
int comidaAnterior = 0;
bool conectado = false;

MCP3008 adc(CLOCK_PIN, MOSI_PIN, MISO_PIN, CS_PIN);

void server(WiFiClient client, String content) {
  potenciometroValue = adc.readADC(7); 
  Serial.println(potenciometroValue);
  ldrValue = adc.readADC(6);
  Serial.println(ldrValue);
  botaoValue = adc.readADC(5);
  Serial.println(botaoValue);
  if (conectado)   {
    digitalWrite(CONECTADO_PIN, HIGH);
    temperatura = potenciometroValue * 16 / 1023 + 18;  
    if (temperatura != temperaturaAnterior) {
      client.printf("TEMP|%d\r\n", temperatura);    
      client.println();
      client.flush();  
      Serial.println(temperatura);
      temperaturaAnterior = temperatura; 
      delay(500);   
    }
    luz = ldrValue * 50 / 1023;
    if (luz != luzAnterior) {      
      client.printf("LIGHT|%d\r\n", luz);
      client.println();
      client.flush();  
      Serial.println(luz);    
      luzAnterior = luz;    
      delay(500);
    }
    comida = botaoValue;
    if (comida == 1023 && comida != comidaAnterior) {
      client.printf("FOOD|1\r\n");
      client.println();
      client.flush();        
      comidaAnterior = comida;
      delay(500);
    } else if (comida == 0) {
      comidaAnterior = comida;
    }
  } else if (content.equals("CONNECTED")) {
    conectado = true;
    client.println("CONNECTED_TOO\r\n");
    client.flush();
  }
}

void setup() {
  pinMode(LIGADO_PIN, OUTPUT);
  pinMode(CONECTADO_PIN, OUTPUT);
  pinMode(WIFI_PIN, OUTPUT);
  char* ssid = "Aquario";
  char* password = "aquario-virtual";
  char* tokenID = "AQUARIUM_01";
  Serial.begin(9600);  
  IUTConn.setSocketCallback(server);
  IUTConn.start(ssid, password, tokenID);  
}

void loop() {
  digitalWrite(LIGADO_PIN, HIGH);
  if (IUTConn.isWiFi()) {
    digitalWrite(WIFI_PIN, HIGH);
  } else {
    digitalWrite(WIFI_PIN, LOW);
  }
  digitalWrite(CONECTADO_PIN, LOW);
  IUTConn.listenSocket();  
}