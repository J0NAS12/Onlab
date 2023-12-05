const WebSocket = require('ws');

const wss = new WebSocket.WebSocketServer({ port: 8082 }, () => {
    console.log('server started')
})


const fixLatency = 200
const latencyVariation = 100


wss.on('connection', (client) => {
    const socket = new WebSocket('ws://localhost:8081')
    socket.addEventListener('message', (event) => {
        const receivedMessage = event.data;
        createLatency(()=>client.send(receivedMessage))
      });
    client.on('message', (message) => {
      console.log(`Received message from client: ${message}`);
      createLatency(()=>socket.send(message))
    });
  });


  function createLatency(callback) {
    const latency = fixLatency + Math.random() * latencyVariation;
    console.log(latency)
    setTimeout(() => {
      if (callback) {
        callback();
      }
    }, latency);
  }