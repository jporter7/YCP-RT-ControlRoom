/*
npm install --global --production windows-build-tools
npm install spi-device
npm install onoff
*/
'use strict';
const config = {
    azencoderSPIbus: 0,
    azencoderSPIdeviceNumber: 0,


    checkTemperatuerEvery: 2000,//ms
    getpositionEvery: 2000,//ms

}

var net = require('net');
function sendTo(port, ip, data) {
    const client = new net.Socket();
    client.connect(port, ip, function () {
        console.log('Connected');
        //client.write('Hello, server! Love, Client.');
        setInterval(function (data) { client.write(data + "<EOF>"); }.bind(null, data), 1000);

    });

    //

    client.on('data', function (data) {
        console.log('Received: ' + data);
        // client.destroy(); // kill client after server's response
    });

    client.on('close', function () {
        console.log('Connection closed');
    });


    //return client;
}

/*
const spi = require('bindings')('spi');
const AZencoder = spi.open(config.azencoderSPIbus, config.azencoderSPIdeviceNumber, (err) => {
    // An SPI message is an array of one or more read+write transfers
    const message = [{
        sendBuffer: Buffer.from([0x01, 0xd0, 0x00]), // Sent to read channel 5
        receiveBuffer: Buffer.alloc(3),              // Raw data read from channel 5
        byteLength: 3,
        speedHz: 20000 // Use a low bus speed to get a good reading from the TMP36
    }];

    if (err) throw err;

    AZencoder.transfer(message, (err, message) => {
        if (err) throw err;

        // Convert raw value from sensor to celcius and log to console
        const rawValue = ((message[0].receiveBuffer[1] & 0x03) << 8) +
            message[0].receiveBuffer[2];
        const voltage = rawValue * 3.3 / 1023;
        const celcius = (voltage - 0.5) * 100;

        console.log(celcius);
    });
    
});
*/



function IoLoop() {
    while (true) {

    }
}
var temperatureInterval = setInterval(checkTemperatuer, config.checkTemperatuerEvery);
//var vibrationInterval = setInterval(checkTemperatuer,1000);
var positionInterval = setInterval(getposition, config.getpositionEvery);

sendTo(11000, '192.168.43.86', "{temp: 100, acc:50}");
function checkTemperatuer() {


}
var data={task:"send temp buffer" , temp:[]}
for (var i=0;i<1000;i++){
    data.temp.push(Math.random())
}


sendTo(11000, '192.168.43.86', JSON.stringify(data))
function setfanSpeed() { }

function checkVibration() { }

function getposition() { }

/**
 * get the azumith position via the spi bus for the encoder
 */
function getAZposition() { }

/**
 * get elevation position via the adc on the spi bus 
 * the elevation encoder has an analog voltage ranging from .5 to 4.5 volts
 */
function getELposition() { }




