let uuid = require('uuid-random');
const WebSocket = require('ws');

const wss = new WebSocket.WebSocketServer({ port: 8081 }, () => {
    console.log('server started')
})


let games = [];
let clients = [];


function getGames() {
    let gameList = {};
    gameList.method = "getGames";
    gameList.lobbies = [];
    games.forEach((lobby) => {
        let newlobby = {};
        newlobby.lobbyID = lobby.lobbyID;
        newlobby.lobbyName = lobby.lobbyName;
        gameList.lobbies.push(newlobby);
    });
    return gameList;
}

function updateAllClients() {
    let lobbiesJSon = JSON.stringify(getGames());
    clients.forEach((c) => {
        c.send(lobbiesJSon);
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
    let game = games.find((e) => dataJSON.lobbyID === e.lobbyID);
    game.clients.forEach(c => {c.send(JSON.stringify(dataJSON));});
}

function sendBullet(client, dataJSON){
    let game = games.find((e) => dataJSON.shooter.lobbyID === e.lobbyID);
    game.clients.forEach(c => {c.send(JSON.stringify(dataJSON));});
}

function updateClient(client){
    let lobbiesJSon = JSON.stringify(getGames());
    client.send(lobbiesJSon);
}

function createGame(client, dataJSON){
    dataJSON.lobbyID = uuid();
    dataJSON.clients = [];
    dataJSON.clients.push(client);
    let response = {
        lobbyID: dataJSON.lobbyID,
        players: dataJSON.players,
        method: "createGame"
    };
    games.push(dataJSON);
    client.send(JSON.stringify(response));
    updateAllClients();
}

function joinGame(client, dataJSON){
        let joinLobby = games.find((e) => dataJSON.lobbyID === e.lobbyID);
        joinLobby.players.push(dataJSON);
        joinLobby.clients.push(client);
        let response = {
            lobbyID: dataJSON.lobbyID,
            players: joinLobby.players,
            method: "updateGame"
        };
        joinLobby.clients.forEach((e) => {
            e.send(JSON.stringify(response));
        });
}

function leaveGame(client, dataJSON){
    let game = games.find((e) => dataJSON.lobbyID === e.lobbyID);
    game.players.splice(game.clients.indexOf(client), 1);
    game.clients.splice(game.clients.indexOf(client), 1);
    if (game.players.length === 0) {
        games.splice(games.indexOf(game), 1);
        updateAllClients();
    }
    let response = {
        lobbyID: dataJSON.lobbyID,
        players: game.players,
        method: "updateGame"
    };
    game.clients.forEach((e) => {
        e.send(JSON.stringify(response));
    });
}

function startGameforAll(dataJSON){
    {
        let game = games.find((e) => dataJSON.lobbyID === e.lobbyID);
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
            case "getGames":
                updateClient(client, dataObject); break;
            case "createGame":
                createGame(client, dataObject); break;
            case "joinGame":
                joinGame(client, dataObject); break;
            case "leaveGame":
                leaveGame(client, dataObject); break;
            case "startGame":
                startGameforAll(dataObject); break;
            default:
                console.log("wrong method");
        }
    });

    //Method notifies when client disconnects
    client.on('close', () => {
        console.log('This Connection Closed!')
        console.log("Removing Client: " + client.id)
        clients.splice(clients.indexOf(client), 1);
        let index = games.findIndex((x) => x.clients.find((x) => x === client));
        if (index >= 0) {
            console.log("Client was in lobby: " + games[index].lobbyID);
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

    let lobbiesJSon = JSON.stringify(getGames());
    client.send(lobbiesJSon);
    console.log("Response");
    console.log(lobbiesJSon);

})

wss.on('listening', () => {
    console.log('listening on 8081')
})