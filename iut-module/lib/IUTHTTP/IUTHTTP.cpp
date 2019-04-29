#include <Arduino.h>
#include "IUTHTTP.h"
#include <WiFiConnect.h>
#include <ESPAsyncUDP.h>
#include <sstream>
#include <ESP8266WiFi.h>

WiFiServer wifiServer(8080);

void IUTHTTP::setAccessPointSSID(char* SSID) {
  this->accessPointSSID = SSID;
}

char* IUTHTTP::getAccessPointSSID() {
  return this->accessPointSSID;
}

void IUTHTTP::setAccessPointPassword(char* password) {
  this->accessPointPassword = password;
}

char* IUTHTTP::getAccessPointPassword() {
  return this->accessPointPassword;
}

void IUTHTTP::setTokenID(char* tokenID) {
  this->tokenID = tokenID;
}

char* IUTHTTP::getTokenID() {
  return this->tokenID;
}

void IUTHTTP::setMulticastIP(char* multicastIP) {
  this->multicastIP = multicastIP;
}

char* IUTHTTP::getMulticastIP() {
  return this->multicastIP;
}

void IUTHTTP::setMulticastPort(unsigned int multicastPort) {
  this->multicastPort = multicastPort;
}

unsigned int IUTHTTP::getMulticastPort() {
  return this->multicastPort;
}

IPAddress IUTHTTP::getDeviceIP() {
  return this->deviceIP;
}

void IUTHTTP::setSocketCallback(void (*callback)(WiFiClient client, String message)) {
  this->socketCallback = callback;
}

void IUTHTTP::listenSocket() {
  this->wifiConn.handleWebServer();
  WiFiClient client = wifiServer.available();  
  if (client) {
    String data = "";    
    while (client.connected()) {      
      data = "";
      this->wifiConn.handleWebServer();            
      while (client.available() > 0) {        
        char c = client.read();
        data += c;
        Serial.write(c);
      }
      delay(10);      
      this->socketCallback(client, data);      
    }
    client.stop();
  }
}

void IUTHTTP::initializeWiFiConn() {
  this->wifiConn.start(this->accessPointSSID, this->accessPointPassword);
  wifiServer.begin();
}

void IUTHTTP::handlePacket(AsyncUDPPacket packet) {
  if (packet.remoteIP() != WiFi.localIP() && packet.isMulticast()) {
    char* data = (char *) packet.data();
    data[strlen(data)-1] = 0;
    Serial.write(packet.data(), packet.length());
    Serial.println(data);
    if (data && strcmp(data, this->tokenID)) {
      deviceIP = packet.remoteIP();
      packet.printf(this->tokenID);
    }
  }
}

void IUTHTTP::initializeMulticast() {
  IPAddress multicastAddress;
  multicastAddress.fromString(this->multicastIP);
  if (this->udpConn.listenMulticast(multicastAddress, this->multicastPort)) {
    this->udpConn.onPacket([this](AsyncUDPPacket packet) {
      handlePacket(packet);
    });
  }
}

void IUTHTTP::start(char* accessPointSSID, char* accessPointPassword, char* tokenID) {
  this->setAccessPointSSID(accessPointSSID);
  this->setAccessPointPassword(accessPointPassword);
  this->setTokenID(tokenID);
  this->start();
}

void IUTHTTP::start(char* tokenID) {
  this->setTokenID(tokenID);
  this->start();
}

void IUTHTTP::start() {
  if (!this->accessPointSSID || strlen(this->accessPointSSID) <= 0) {
    Serial.println("Access Point SSID is mandatory!");
    return;
  }
  if (!this->tokenID || strlen(this->tokenID) <= 0) {
    Serial.println("Token Identification is mandatory!");
    return;
  }
  if (!this->multicastIP || strlen(this->multicastIP) <= 0) {
    Serial.println("Multicast IP is mandatory!");
    return;
  }
  if (!this->multicastPort || this->multicastPort <= 0) {
    Serial.println("Multicast Port must be greater than zero!");
    return;
  }
  this->initializeWiFiConn();
  if (this->wifiConn.getConnectionType().equals("STA")) {
    this->initializeMulticast();
  }
}