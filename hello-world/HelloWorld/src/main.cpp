#include <Arduino.h>
#include <WiFiConnect.h>

//VIVO-52C0|Td9VYk7m3W

void setup() {
  Serial.begin(9600);
  start();
}

void loop() {
  createWebPage();
}