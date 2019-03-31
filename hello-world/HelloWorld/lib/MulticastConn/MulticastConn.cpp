#include <Arduino.h>
#include <ESP8266WiFi.h>
#include <WiFiUdp.h>

#define BUFFER_LENGTH 256

WiFiUDP UDP;

char incomingPacket[BUFFER_LENGTH]; 
String remoteIP = "";

void startMulticast(int ipPart01, int ipPart02, int ipPart03, int ipPart04, unsigned int multicastPort) {  
  IPAddress multicastAddress(ipPart01, ipPart02, ipPart03, ipPart04);  
  UDP.beginMulticast(WiFi.localIP(), multicastAddress, multicastPort);
}

char* getPacket() {
  return incomingPacket;
}

String getRemoteIP() {
  return remoteIP;
}

void listeningMulticast() {
  int packetLength = UDP.parsePacket(); 
  if (packetLength > 0) {
    int len = UDP.read(incomingPacket, BUFFER_LENGTH);
    if (len > 0) {
      incomingPacket[len] = 0;
      remoteIP = UDP.remoteIP().toString();
      Serial.printf("%s\n", incomingPacket);
      Serial.println("IP: " + remoteIP);
    }
  }
}