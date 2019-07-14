/*
npm install --global --production windows-build-tools
npm install spi-device
npm install onoff
*/
'use strict';
const config = {
    azencoderSPIbus: 1,
    azencoderSPIdeviceNumber: 0,
    AdcSPIbus: 1,
    AdcSPIdeviceNumber: 1,
    checkTemperatuerEvery: 2000,//ms
    getpositionEvery: 2000,//ms
    motorfanOnTemperature: 50,//C
    //192.168.1.39
    //192.168.43.86
    //169.254.28.40
    //192.168.7.1
    controlRoom: {
        IP: "192.168.7.1",
        TCPport: 11000
    },
    // An SPI message is an array of one or more read+write transfers
    mesage_readCH5: [{
        sendBuffer: Buffer.from([0x01, 0xd0, 0x00]), // Sent to read channel 5
        receiveBuffer: Buffer.alloc(3),              // Raw data read from channel 5
        byteLength: 3,
        speedHz: 20000 
    }],
    azEncoderMessage:[{
        sendBuffer: Buffer.from([0x01, 0xd0, 0x30, 0x40, 0x35, 0x04]),
        receiveBuffer: Buffer.alloc(6),
        byteLength: 6,
        speedHz: 100000 
    }]
}



const net = require('net');
//const spi = require('spi-node');
//*

const spi = require('spi-device');

/*
var hrTime = process.hrtime();
let acc = { x: 0, y: 0, z: 0, time: (hrTime[0] * 1000 + hrTime[1] / 1000000), netAcc: 0 }
console.log((hrTime[1]))
date=new Date().getTime()
date.getSeconds()+(date.getMinutes()*60)+(date.getHours()*60*60)
console.log(process.hrtime()[0],(Math.floor(new Date().getTime() / 1000)*1000)+(process.hrtime()[1]/1000000))
(Math.floor(new Date().getTime() / 1000)*1000)+(process.hrtime()[1]/1000000);
*/
var temperatureInterval, vibrationInterval, vibrationSendInterval, positionInterval;
function startup() {
    const client = new net.Socket();
    try {
        client.connect(config.controlRoom.TCPport, config.controlRoom.IP, function () {
            client.write("getMode<EOF>");
            //client.destroy();
        });
        client.on('data', function (data) {
            console.log('Received: ' + data);

            run();
            client.destroy(); // kill client after server's response
        });
    } catch (err) { console.log(err); client.destroy(); startup(); }
}

startup();





process.on('uncaughtException', (err, origin) => {
    console.log(err);
    console.log(origin);
    console.log(err.stack)
    console.log("ERR     starting cleanup");
    //clearInterval(temperatureInterval);
    //clearInterval(vibrationInterval);
    //clearInterval(vibrationSendInterval);
    //clearInterval(positionInterval);
    //startup();
});



class SendBuffer {//in the event that coms are down this will acumulate data to send
    stored = {
        //xxxxxxxxxxxxxxx:{type: "", data: [{ val: 0, time: 0 }]} 
    };
    order = [];
    inTransit = {};
    addData = function (data = { type: "", data: [{ val: 0, time: 0 }] }) {
        let combined = false;
        //console.log(Object.keys(this.stored));
        //console.log(Object.keys(data))
        for (let key in this.stored) {
            if (this.stored[key] == undefined) { console.log(this.stored) }
            if (this.stored[key].type == data.type && this.stored[key].data.length < 800) {//if data types match and stored data lengthe is less than 800 add the data
                this.stored[key].data = data.data.concat(this.stored[key].data);
                combined = true;
            }
        }
        if (!combined) {
            let uuid = this.makeUUid();
            this.stored[uuid] = data;
            this.order.push(uuid);
        }
    }
    GetNextSend = function () {
        if (!this.CanSend()) { return null; }
        let nextuuid = this.order.shift();
        if (this.stored[nextuuid] == undefined) { delete this.stored[nextuuid]; return null; }
        let tosend = this.stored[nextuuid];

        this.inTransit[nextuuid] = tosend;
        tosend.uuid = nextuuid;
        delete this.stored[nextuuid];
        return tosend;

    }
    ConfirmSend = function (uuid = "xxxxxxxxxxxxxxx") {
        delete this.inTransit[uuid];
    }
    CanSend = function () {
        //console.log(Object.keys(this.inTransit).length)
        return Object.keys(this.inTransit).length == 0
    }
    makeUUid = function () {
        var date = new Date().getTime();
        var uuid = 'xxxxxxxxxxxxxxx'.replace(/[x]/g, function (c) {
            var r = (date + Math.random() * 16) % 16 | 0;
            date = Math.floor(date / 16);
            return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
        });
        return uuid;
    }
}
function run() {
    var buffer = new SendBuffer();
    function trysend(data = { type: "", data: [{ val: 0, time: 0 }] }) {
        buffer.addData(data);
        let nextdata = buffer.GetNextSend();
        if (nextdata != null) {
            //console.log("sending")
            sendData(nextdata);
        }else{console.log("norywtvr")}

    }


    function sendData(data = { type: "", data: [{ val: 0, time: 0 }], uuid: "xxxxxxxxxxxxxxx" }) {
        const client = new net.Socket();
        let nexxtuuid=data.uuid;
        client.connect(config.controlRoom.TCPport, config.controlRoom.IP, function () {
            client.write(JSON.stringify(data) + "<EOF>"); //send first data
        });
        client.on('data', function (resp) {
            if (resp.indexOf("200") != -1) {
                console.log("responce " + resp);
                buffer.ConfirmSend(nexxtuuid);
                let nextdata = buffer.GetNextSend();
                if (nextdata == null) { client.destroy(); }
                else {nexxtuuid=nextdata.uuid; client.write(JSON.stringify(nextdata) + "<EOF>"); }
            }
            else { client.write(JSON.stringify(data) + "<EOF>"); }
        });
    }

    function sendalert(message = "") {
        const client = new net.Socket();
        client.connect(port, ip, function () {
            client.write(message + "<EOF>");
        });
        client.on('data', function (data) {
            if (data == "200") {
                client.destroy(); // kill client after server's response
            } else { }
        });
    }

    const AZencoder = spi.open(config.azencoderSPIbus, config.azencoderSPIdeviceNumber, (err) => {
        if (err) throw err;
        AZencoder.transfer(config.mesage_readCH5, (err, message) => {
            if (err) throw err;
            // Convert raw value from sensor to celcius and log to console
            const rawValue = ((message[0].receiveBuffer[1] & 0x03) << 8) + message[0].receiveBuffer[2];
            const voltage = rawValue * 3.3 / 1023;
            const celcius = (voltage - 0.5) * 100;
            console.log(celcius);
        });
    });




    temperatureInterval = setInterval(checkTemperatuer, config.checkTemperatuerEvery);
    vibrationInterval = setInterval(recordVibration, 1);
    vibrationSendInterval = setInterval(sendVipration, 1000);
    positionInterval = setInterval(getposition, config.getpositionEvery);

    function checkTemperatuer() {
        const ADconverter = spi.open(config.AdcSPIbus, config.AdcSPIdeviceNumber, (err) => {
            if (err) {console.log(err); return;}
            ADconverter.transfer(config.mesage_readCH5, (err, message) => {
                if (err) {console.log(err); throw err;}
                let rawValue = ((message[0].receiveBuffer[1] & 0x03) << 8) + message[0].receiveBuffer[2];
                let voltage = rawValue * 3.3 / 1023;
                let celcius = (voltage - 0.5) * 100;
                if (celcius > config.motorfanOnTemperature) {
                    setfanrunning(true)
                } else if (celcius < config.motorfanOnTemperature - 3) { setfanrunning(false) }//hysteris for fan
                var now = new Date().getTime();
                trysend({ type: "temp", data: [{ val: celcius, time: now }] });
            });
        });
    }

    function setfanrunning(shouldrun) {
        let isrunning = false;
        //TODO:check if fan is running
        if (shouldrun != isrunning) {//make it run or stop

        }
    }

    var vibrationBuffer = [];
    function recordVibration() {
        //var hrTime = process.hrtime();
        //(hrTime[0] * 1000 + hrTime[1] / 1000000)
        //Math.floor(time / 1000)*1000;
        let acc = { x: 0, y: 0, z: 0, time: new Date().getTime(), val: 0 }
        //TODO:get the acc
        acc.x = Math.random();
        acc.z = Math.random();
        acc.y = Math.random();
        acc.val = Math.sqrt((acc.x * acc.x) + (acc.y * acc.y) + (acc.z * acc.z));

        vibrationBuffer.push(acc);
    }

    function sendVipration() {
        trysend({ type: "vibration", data: vibrationBuffer });
        vibrationBuffer = [];
    }

    function getposition() { }

    /**
     * get the azumith position via the spi bus for the encoder
     */
    function getAZposition() {

    }

    /**
     * get elevation position via the adc on the spi bus 
     * the elevation encoder has an analog voltage ranging from .5 to 4.5 volts
     */
    function getELposition() {


    }



    /**
     * remove all entries in table from befor the specified time
     * @param {*} table 
     * @param {*} time 
     */
    function cleandatabase(table, time) {

    }
}


