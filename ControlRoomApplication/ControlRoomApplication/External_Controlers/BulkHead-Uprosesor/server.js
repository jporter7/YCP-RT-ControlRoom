



'use strict';
const Net = require('net');
const spi = require('spi-device');
const config=require('./config.json');
let azEncoderMessage = [{
    sendBuffer: Buffer.from([0x01, 0xd0, 0x30, 0x40, 0x35, 0x04]),
    receiveBuffer: Buffer.alloc(6),
    byteLength: 6,
    speedHz: 100000
}];

//alternative eventlistner that can be passed as a callback for createServer
/*function (c) { //'connection' e listener //console.log('server connected');c.on('end', function () {//console.log('server disconnected');});}//*/
// Create a new TCP server. 
const server = new Net.createServer();
server.listen(config.controlRoom.TCPportsend, function () {//start listining for conections
    console.log(`Server listening for connection requests on socket localhost:${config.controlRoom.TCPportsend}`);
});
server.on('connection', function (socket) {
    //console.log('A new connection has been established.');
    //xxxxxxxxxxxxxxx:{type: "", data: [{ val: 0, time: 0 }]} 
    let uuid = makeUUid();
    let encodertimeout = setTimeout(() => { socket.write("null"); }, 5000);
    let az, el, total = 2, recived = 0;
    getAZposition().then(val => { az = val; recived++; both(); });
    getELposition().then(val => { el = val; recived++; both(); });

    var msg = {};
    function both() {
        if (recived == total) {
            console.log(az,el)
            msg = {uuid:uuid, type: "position", AZ: az, EL: el };
            socket.write(JSON.stringify(msg));
            clearTimeout(encodertimeout);
        }
    }
    /*
    socket.on('data', function (chunk) {console.log(`Data received from client: ${chunk.toString()}`);});
    socket.on('end', function () {console.log('Closing connection with the client');});
    //*/
    socket.on('error', function (err) {
        console.log(`Error: ${err}`);
    });

});

function getAZposition() {
    return new Promise((resolve, reject) => {
        let AZencoder = spi.open(config.azencoderSPIbus, config.azencoderSPIdeviceNumber, (err) => {
            if (err) reject(err);
            AZencoder.transfer(azEncoderMessage, (err, message) => {
                if (err) reject(err);
                // Convert raw value from sensor to celcius and log to console
                let rawValue = ((message[0].receiveBuffer & (0x1FFFFF << 8)) >> 8);//21 bit mask over the position value
                //console.log(rawValue);
                resolve(rawValue);
            });
        });
    });
}

function getELposition() {
    return new Promise((resolve, reject) => {
        let AZencoder = spi.open(config.azencoderSPIbus, config.azencoderSPIdeviceNumber, (err) => {
            if (err) reject(err);
            AZencoder.transfer(azEncoderMessage, (err, message) => {
                if (err) reject(err);
                // Convert raw value from sensor to celcius and log to console
                let rawValue = ((message[0].receiveBuffer & (0x1FFFFF << 8)) >> 8);//21 bit mask over the position value
                //console.log(rawValue);
                resolve(rawValue);
            });
        });
    });
}



function makeUUid() {
    let date = new Date().getTime();
    let uuid = 'xxxxxxxxxxxxxxx'.replace(/[x]/g, function (c) {
        let r = (date + Math.random() * 16) % 16 | 0;
        date = Math.floor(date / 16);
        return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    });
    return uuid;
}

/*
    return new Promise((resolve, reject) => {
        const xhr = new XMLHttpRequest();
        xhr.open("GET", url);
        xhr.onload = () => resolve(xhr.responseText);
        xhr.onerror = () => reject(xhr.statusText);
        xhr.send();
        resolve(231);
    });
*/