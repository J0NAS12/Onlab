# Onlab/Szakdolgozat
This is my thesis work for BSc.
An online game client created in Unity and a server in node.js.

To run the application, you can build it from Unity Editor.
Open the project, and build it to an executable:
File -> Build Settings -> Build
You can start the game by running the created game.exe file.

For the server, you will need node.js and npm installed.
Used versions:
node v18.14.2
npm 9.5.0
To run the server, you need need to install the node packages first:
`npm install`
Then start the Server from the Server folder:
`node server.js`

The client connects to the default port, localhost:8081 in default, if there is not a server there, you can connect to a different one.
You need to give the ip and the port, and then try reconnecting.
Example: `127.0.0.1:8081`
