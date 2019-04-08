/*
  WiFiConnect.h - Library for connect ESP8266 to WiFi.
  Created by Flávio Omar Losada, March 30, 2019.
  Released into the public domain.
*/
#ifndef WiFiConnect_h
#define WiFiConnect_h

#include <Arduino.h>

class WiFiConnect {
  public:
    WiFiConnect();
    void start();
    void createWebServer(void (*callback)(String request));
    String getConnectionType();
  private:
    void loadHTML();
    String readFile(String file);
    void writeFile(String content, String file);
    String getConfigFromPage(String parameter);
    void connectAccessPoint();
    boolean connectWiFi(String wifiConfig);
};

#endif