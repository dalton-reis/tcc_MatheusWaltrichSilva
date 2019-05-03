/*
  IUTConnect.h - Library for communicate ESP8266 by HTTP protocol.
  Created by Flávio Omar Losada, April 26, 2019.
  Released into the public domain.
*/
#ifndef IUTConnect_h
#define IUTConnect_h

#include <Arduino.h>
#include <WiFiConnect.h>
#include <ESPAsyncUDP.h>

class IUTConnect {
  public:
    void setAccessPointSSID(char* SSID);
    char* getAccessPointSSID();
    void setAccessPointPassword(char* password);
    char* getAccessPointPassword();
    void setTokenID(char* tokenID);
    char* getTokenID();
    void setMulticastIP(char* multicastIP);
    char* getMulticastIP();
    void setMulticastPort(unsigned int multicastPort);    
    unsigned int getMulticastPort();
    IPAddress getDeviceIP();
    void setSocketCallback(void (*callback)(String identifier, String content));
    void start();
    void start(char* tokenID);
    void start(char* accessPointSSID, char* accessPointPassword, char* tokenID);    
    void listenSocket();
    void sendMessage(String identifier, String value);
  private:
    void (*socketCallback)(String);
    char* accessPointSSID = "IUTConnect";
    char* accessPointPassword;
    char* tokenID;
    WiFiConnect wifiConn;
    AsyncUDP udpConn;
    char* multicastIP = "227.55.77.99";
    IPAddress deviceIP;
    unsigned int multicastPort = 5000;
    void initializeWiFiConn();
    void initializeMulticast();    
    void handlePacket(AsyncUDPPacket packet);
};

#endif