



'use strict';
const Net = require('net');
const spi = require('spi-device');
const config = require('./config.json');


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
    Promise.all([getAZposition(), getELposition()])
        .then(function (values) {//wait for all the promises to resolve
            clearTimeout(encodertimeout);
            //console.log(values)
            let msg = { uuid: uuid, type: "position", AZ: values[0], EL: values[1] };
            socket.write(JSON.stringify(msg));
        }).catch((reason) => {
            console.log("promise rejected: " + reason)
            clearTimeout(encodertimeout);
            socket.write(JSON.stringify({error:reason}));
        });

    /*
    socket.on('data', function (chunk) {console.log(`Data received from client: ${chunk.toString()}`);});
    socket.on('end', function () {console.log('Closing connection with the client');});
    //*/
    socket.on('error', function (err) {
        console.log(`Error: ${err}`);
    });

});

var azEncoderMessage = [{
    sendBuffer: Buffer.from([0x00, 0x00, 0x00, 0x00, 0x00, 0x00]),
    receiveBuffer: Buffer.alloc(6),
    byteLength: 6,
    speedHz: 200000,
    microSecondDelay: 0xf

}];
//[{ threeWire: true, noChipSelect: true, }]
function getAZposition() {
    return new Promise((resolve, reject) => {
        let AZencoder = spi.open(2, 0, (err) => {
            if (err) reject(err);
            AZencoder.transfer(azEncoderMessage, (err, message) => {
                if (err) reject(err);
                var read = message[0].receiveBuffer;
                var datagood = (read[2] >> 7);
                if (datagood == 0) {
                    //reject("bad_data")
                    //console.log("data bad");
                    //return;
                }
                var chcksum = read[5] & 0x7f;
                var val = (read[2] << 16)+ (read[3] << 8) + read[4];
                var vallow = (read[3] << 16) + (read[4] << 8) + read[5];
               //console.log((message[0].receiveBuffer));//, " checksum: " + chcksum);
                //console.log(val.toString(2).padStart(21, '0') ,val)
                console.log((read[0].toString(2).padStart(8, '0')+" "+read[1].toString(2).padStart(8, '0')+" "+read[2].toString(2).padStart(8, '0')+" "+read[3].toString(2).padStart(8, '0')+" "+read[4].toString(2).padStart(8, '0')+" "+read[5].toString(2).padStart(8, '0')), (message[0].receiveBuffer) )
                //console.log(val, val.toString(2));
                resolve(val);
                AZencoder.close(nothing);
            });
        });
    });
}

function getELposition() {
    return new Promise((resolve, reject) => {
       /* let AZencoder = spi.open(config.azencoderSPIbus, config.azencoderSPIdeviceNumber, (err) => {
            if (err) reject(err);
            AZencoder.transfer(azEncoderMessage, (err, message) => {
                if (err) reject(err);
                // Convert raw value from sensor to celcius and log to console
                let rawValue = ((message[0].receiveBuffer & (0x1FFFFF << 8)) >> 8);//21 bit mask over the position value
                //console.log(rawValue);
                resolve(rawValue);
            });
        });
        /*/
        resolve(0)
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

function nothing(){}

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