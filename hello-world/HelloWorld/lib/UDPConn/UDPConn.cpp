#include <Arduino.h>
#include "UDPConn.h"
#include <ESP8266WiFi.h>
#include <WiFiUdp.h>

#define BUFFER_LENGTH 256

WiFiUDP UDP;

char incomingPacket[BUFFER_LENGTH]; 
String remoteIP = "";

UDPConn::UDPConn() {

}

void UDPConn::startMulticast(int ipPart01, int ipPart02, int ipPart03, int ipPart04, unsigned int multicastPort) {  
  IPAddress multicastAddress(ipPart01, ipPart02, ipPart03, ipPart04);  
  UDP.beginMulticast(WiFi.localIP(), multicastAddress, multicastPort);
}

char* UDPConn::getMulticastPacket() {
  return incomingPacket;
}

String UDPConn::getRemoteIP() {
  return remoteIP;
}

void UDPConn::listeningMulticast() {
  this->listeningMulticast(false);
}

void UDPConn::listeningMulticast(bool log) {
  int packetLength = UDP.parsePacket(); 
  if (packetLength > 0) {
    int len = UDP.read(incomingPacket, BUFFER_LENGTH);
    if (len > 0) {
      incomingPacket[len] = 0;
      remoteIP = UDP.remoteIP().toString();
      if (log) {
        Serial.printf("%s\n", incomingPacket);
        Serial.println("IP: " + remoteIP);
      }
    }
  }
}