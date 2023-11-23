let uuid = require('uuid-random');
const WebSocket = require('ws');

const wss = new WebSocket.WebSocketServer({ port: 8081 }, () => {
    console.log('server started')
})


let games = [];
let clients = [];


function getRooms() {
    let gameList = {};
    gameList.method = "getRooms";
    gameList.rooms = [];
    games.forEach((room) => {
        let newroom = {};
        newroom.roomID = room.roomID;
        newroom.roomName = room.roomName;
        gameList.rooms.push(newroom);
    });
    return gameList;
}

function updateAllClients() {
    let roomsJSON = JSON.stringify(getRooms());
    clients.forEach((c) => {
        c.send(roomsJSON);
    });
}

function startGame(game, input) {
    let gameSettings = {};
    gameSettings.method = "startGame";
    gameSettings.maze = input.maze;
    gameSettings.players = game.players;
    for (let i = 0; i < gameSettings.players.length; i++) {
        gameSettings.players[i].index = i;
    }
    return gameSettings;
}

function synchronizeClock(client){
        let timestamp = {};
        timestamp.method = "clock";
        timestamp.timestamp = new Date().getTime() / 1000.0;
        client.send(JSON.stringify(timestamp));
}

function sendToAllClients(client, dataJSON){
    let game = games.find((e) => dataJSON.roomID === e.roomID);
    game.clients.forEach(c => {c.send(JSON.stringify(dataJSON));});
}

function sendBullet(client, dataJSON){
    let game = games.find((e) => dataJSON.shooter.roomID === e.roomID);
    game.clients.forEach(c => {c.send(JSON.stringify(dataJSON));});
}

function updateClient(client){
    let roomsJSON = JSON.stringify(getRooms());
    client.send(roomsJSON);
}

function createRoom(client, dataJSON){
    dataJSON.roomID = uuid();
    dataJSON.clients = [];
    dataJSON.clients.push(client);
    let response = {
        roomID: dataJSON.roomID,
        players: dataJSON.players,
        method: "createGame"
    };
    games.push(dataJSON);
    client.send(JSON.stringify(response));
    updateAllClients();
}

function joinRoom(client, dataJSON){
        let joinroom = games.find((e) => dataJSON.roomID === e.roomID);
        joinroom.players.push(dataJSON);
        joinroom.clients.push(client);
        let response = {
            roomID: dataJSON.roomID,
            players: joinroom.players,
            method: "updateGame"
        };
        joinroom.clients.forEach((e) => {
            e.send(JSON.stringify(response));
        });
}

function leaveRoom(client, dataJSON){
    let room = games.find((e) => dataJSON.roomID === e.roomID);
    room.players.splice(room.clients.indexOf(client), 1);
    room.clients.splice(room.clients.indexOf(client), 1);
    if (room.players.length === 0) {
        games.splice(games.indexOf(room), 1);
        updateAllClients();
    }
    let response = {
        roomID: dataJSON.roomID,
        players: room.players,
        method: "updateGame"
    };
    room.clients.forEach((e) => {
        e.send(JSON.stringify(response));
    });
}

function startGameforAll(dataJSON){
    {
        let game = games.find((e) => dataJSON.roomID === e.roomID);
        game.clients.forEach((e) => {
            e.send(JSON.stringify(startGame(game, dataJSON)));
        });
    }
}

wss.on('connection', function connection(client) {

    //Create Unique User ID for player
    client.id = uuid();
    clients.push(client);
    console.log(`Client ${client.id} Connected!`)

    //Method retrieves message from client
    client.on('message', (data) => {
        let dataObject = JSON.parse(data);
        switch (dataObject.method) {
            case "hit":
            case "game":
                sendToAllClients(client, dataObject); break;
            case "clock":
                synchronizeClock(client); break;
            case "bullet":
                sendBullet(client, dataObject); break;
            case "getRooms":
                updateClient(client, dataObject); break;
            case "createRoom":
                createRoom(client, dataObject); break;
            case "joinRoom":
                joinRoom(client, dataObject); break;
            case "leaveRoom":
                leaveRoom(client, dataObject); break;
            case "startGame":
                startGameforAll(dataObject); break;
            default:
                console.log(`wrong method: ${dataObject.method}`);
        }
    });

    //Method notifies when client disconnects
    client.on('close', () => {
        console.log('This Connection Closed!')
        console.log("Removing Client: " + client.id)
        clients.splice(clients.indexOf(client), 1);
        let index = games.findIndex((x) => x.clients.find((x) => x === client));
        if (index >= 0) {
            console.log("Client was in room: " + games[index].roomID);
            games[index].players.splice(games[index].clients.indexOf(client), 1);
            games[index].clients.splice(games[index].clients.indexOf(client), 1);
            if (games[index].players.length === 0) {
                games.splice(index, 1);
                updateAllClients();
            }
        }
    })
    const id = {};
    id.method = "id";
    id.id = client.id;

    client.send(JSON.stringify(id));
    console.log("Sent back");
    console.log(JSON.stringify(id));

    let roomsJSON = JSON.stringify(getRooms());
    client.send(roomsJSON);
    console.log("Response");
    console.log(roomsJSON);

})

wss.on('listening', () => {
    console.log('listening on 8081')
})