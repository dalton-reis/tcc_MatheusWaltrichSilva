'use strict'

const dgram = require('dgram');
const process = require('process');

const PORT = 5000;
const IP_ADDRESS = '227.55.77.99';

const socket = dgram.createSocket({type: 'udp4', reuseAddr: true});

socket.bind(PORT);

const sendMessage = () => {  
  const message = Buffer.from(`Message from PC: ${process.pid}`);
  socket.send(message, 0, message.length, PORT, IP_ADDRESS, () => console.log(`Sending message: ${message}`));
}

socket.on('listening', () => {
  socket.addMembership(IP_ADDRESS);
  setInterval(sendMessage, 2500);
  const address = socket.address();
  console.log(`UDP socket listening on ${address.address}:${address.port} pid: ${process.pid}`);
});

socket.on('message', (message, rinfo) => {
  console.info(`Message from: ${rinfo.address}:${rinfo.port} - ${message}`);
});