var uuid = require('uuid-random');
const WebSocket = require('ws');

const wss = new WebSocket.WebSocketServer({ port: 8080 }, () => {
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



wss.on('connection', function connection(client) {

    //Create Unique User ID for player
    client.id = uuid();
    clients.push(client);
    console.log(`Client ${client.id} Connected!`)

    //Method retrieves message from client
    client.on('message', (data) => {
        let dataJSON = JSON.parse(data);
        console.log("Player Message");
        console.log(dataJSON);
        switch (dataJSON.method) {
            case "game":
                let myLobby = games.find((e) => dataJSON.lobbyID === e.lobbyID);
                console.log("hello here" + myLobby.clients.length);
                myLobby.clients.forEach(c => {c.send(JSON.stringify(dataJSON));});
                break;
            case "getGames":
                let lobbiesJSon = JSON.stringify(getGames());
                client.send(lobbiesJSon);
                break;
            case "createGame":
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
                break;
            case "joinGame":
                {
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
                    break;
                }
            case "leaveGame":
                {
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
                break;
            case "startGame":
                {
                    let game = games.find((e) => dataJSON.lobbyID === e.lobbyID);
                    game.clients.forEach((e) => {
                        console.log(startGame(game, dataJSON));
                        e.send(JSON.stringify(startGame(game, dataJSON)));
                    });
                }
                break;
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
        console.log(index);
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
    var id = {};
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
    console.log('listening on 8080')
})