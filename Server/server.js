var uuid = require('uuid-random');
const WebSocket = require('ws');

const wss = new WebSocket.WebSocketServer({ port: 8080 }, () => {
    console.log('server started')
})


let games = [];
let clients = [];


function getGames() {
    let gameList = {};
    gameList.method = "getLobbies";
    gameList.lobbies = [];
    games.forEach((lobby) => {
        let newlobby = {};
        newlobby.lobbyID = lobby.lobbyID;
        newlobby.lobbyName = lobby.lobbyName;
        gameList.lobbies.push(newlobby);
    });
    return gameList;
}

function updateAllClients(){
    let lobbiesJSon = JSON.stringify(getGames());
        clients.forEach((c)=>{
            c.send(lobbiesJSon);
        });
}

function startGame(game, input){
    let gameSettings = {};
    gameSettings.method = "startGame";
    gameSettings.maze = input.maze;
    gameSettings.players = game.players;
    for(let i = 0; i< gameSettings.players.length; i++){
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
                myLobby.clients.forEach(c => c.send(data));
                break;
            case "getLobbies":
                let lobbiesJSon = JSON.stringify(getGames());
                client.send(lobbiesJSon);
                console.log("Response");
                console.log(lobbiesJSon);
                break;
            case "createLobby":
                dataJSON.lobbyID = uuid();
                dataJSON.clients = [];
                dataJSON.clients.push(client);
                let response = {
                    lobbyID: dataJSON.lobbyID,
                    players: dataJSON.players,
                    method: "createLobby"
                };
                games.push(dataJSON);
                client.send(JSON.stringify(response));
                updateAllClients();
                break;
            case "joinLobby":
                {
                    let joinLobby = games.find((e) => dataJSON.lobbyID === e.lobbyID);
                    console.log(joinLobby);
                    joinLobby.players.push(dataJSON);
                    joinLobby.clients.push(client);
                    let response = {
                        lobbyID: dataJSON.lobbyID,
                        players: joinLobby.players,
                        method: "updateLobby"
                    };
                    console.log("Update for lobby");
                    console.log(response);
                    let i=0
                    joinLobby.clients.forEach((e) => {
                        console.log("client "+ i++);
                        e.send(JSON.stringify(response));
                    });
                    break;
                }
            case "leaveLobby":
                {
                    let leaveLobby = games.find((e) => dataJSON.lobbyID === e.lobbyID);
                    leaveLobby.players.splice(leaveLobby.clients.indexOf(client), 1);
                    leaveLobby.clients.splice(leaveLobby.clients.indexOf(client), 1);
                    if (leaveLobby.players.length === 0) {
                        games.splice(games.indexOf(leaveLobby), 1);
                        updateAllClients();
                    }
                    let response = {
                        lobbyID: dataJSON.lobbyID,
                        players: leaveLobby.players,
                        method: "updateLobby"
                    };
                    leaveLobby.clients.forEach((e) => {
                        e.send(JSON.stringify(response));
                    });

                }
                break;
                case "startGame":
                    let game = games.find((e) => dataJSON.lobbyID === e.lobbyID);
                    game.clients.forEach((e) => {
                        console.log(startGame(game, dataJSON));
                        e.send(JSON.stringify(startGame(game, dataJSON)));
                    });


        }

    });

    //Method notifies when client disconnects
    client.on('close', () => {
        console.log('This Connection Closed!')
        console.log("Removing Client: " + client.id)
        clients.splice(clients.indexOf(client), 1);
        let index = games.findIndex((x)=> x.clients.find((x)=>x === client));
        console.log(index);
        if(index>=0){
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