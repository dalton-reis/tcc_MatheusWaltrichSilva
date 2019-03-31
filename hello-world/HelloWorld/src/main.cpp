#include <Arduino.h>
#include <WiFiConnect.h>
#include <MulticastConn.h>

//VIVO-52C0|Td9VYk7m3W

unsigned int multicastPort = 5000;

void setup() {
  Serial.begin(9600);
  start();
  if (getConnectionType() == "STA") {
    startMulticast(227, 55, 77, 99, multicastPort);
  }
}

void loop() {
  createWebServer();
  if (getConnectionType() == "STA") {
    listeningMulticast();
  }
}