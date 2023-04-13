var uuid = require('uuid-random');
const WebSocket = require('ws');

const wss = new WebSocket.WebSocketServer({ port: 8080 }, () => {
    console.log('server started')
})


let lobbies = [];

wss.on('connection', function connection(client) {

    //Create Unique User ID for player
    client.id = uuid();

    console.log(`Client ${client.id} Connected!`)

    //Method retrieves message from client
    client.on('message', (data) => {
        let dataJSON = JSON.parse(data);
        console.log("Player Message");
        console.log(dataJSON);

        switch (dataJSON.method) {
            case "getLobbies":
                let listlobbies = {};
                listlobbies.method = "getLobbies";
                listlobbies.lobbies = lobbies;
                //listlobbies.lobbies.forEach((l) => l.clients = undefined);
                let lobbiesJSon = JSON.stringify(listlobbies);
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

                lobbies.push(dataJSON);
                client.send(JSON.stringify(response));
                break;
            case "joinLobby":
                {
                    let joinLobby = lobbies.find((e) => dataJSON.lobbyID = e.lobbyID);
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
                        console.log("client "+i++);
                        e.send(JSON.stringify(response));
                    });
                    break;
                }
            case "leaveLobby":
                {
                    let leaveLobby = lobbies.find((e) => dataJSON.lobbyID = e.lobbyID);
                    leaveLobby.players.splice(leaveLobby.clients.indexOf(client), 1);
                    leaveLobby.clients.splice(leaveLobby.clients.indexOf(client), 1);
                    if (leaveLobby.players.length === 0) {
                        lobbies.splice(lobbies.indexOf(leaveLobby), 1);
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
        }

    });

    //Method notifies when client disconnects
    client.on('close', () => {
        console.log('This Connection Closed!')
        console.log("Removing Client: " + client.id)
        let index = lobbies.findIndex((x)=> x.clients.find((x)=>x === client));
        console.log(index);
        if(index<=0){
            console.log("Client was in lobby: " + lobbies[index].lobbyID);
        }
    })
    var id = {};
    id.method = "id";
    id.id = client.id;

    client.send(JSON.stringify(id));
    console.log("Sent back");
    console.log(JSON.stringify(id));

})

wss.on('listening', () => {
    console.log('listening on 8080')
})