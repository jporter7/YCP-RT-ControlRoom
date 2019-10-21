/*
npm install --global --production windows-build-tools
npm install spi-device
npm install onoff

UNDEFINED = -1,
AZ_MOTOR = 0,
EL_MOTOR = 1,
END_OF_COUNTER_BALLENCE = 2,
MICRO_CTRL = 3,
MICRO_CTRL_BOX = 4
*/
//192.168.1.39
//192.168.43.86
//169.254.28.40
//192.168.7.1
'use strict';

// An SPI message is an array of one or more read+write transfers
var mesage_readCH5 = [{
    sendBuffer: Buffer.from([0x01, 0xd0, 0x00]), // Sent to read channel 5
    receiveBuffer: Buffer.alloc(3),              // Raw data read from channel 5
    byteLength: 3,
    speedHz: 20000
}];


var runMode = "";

const net = require('net');
const spi = require('spi-device');
const config = require('./config.json');
/*
var hrTime = process.hrtime();
let acc = { x: 0, y: 0, z: 0, time: (hrTime[0] * 1000 + hrTime[1] / 1000000), netAcc: 0 }
console.log((hrTime[1]))
date=new Date().getTime()
date.getSeconds()+(date.getMinutes()*60)+(date.getHours()*60*60)
console.log(process.hrtime()[0],(Math.floor(new Date().getTime() / 1000)*1000)+(process.hrtime()[1]/1000000))
(Math.floor(new Date().getTime() / 1000)*1000)+(process.hrtime()[1]/1000000);
*/
var temperatureInterval, vibrationInterval, vibrationSendInterval/*, positionInterval*/;
function startup() {
    const client = new net.Socket();
    try {
        let conectTimeout = setTimeout(() => { client.destroy(); startup(); }, 20000);
        client.connect(config.controlRoom.TCPportrecive, config.controlRoom.IP, function () {
            client.write("getMode<EOF>");
            //client.destroy();
        });
        client.on('data', function (data) {
            console.log('Received: ' + data);
            runMode = "run";
            run();
            client.destroy(); // kill client after server's response
            clearTimeout(conectTimeout);
        });
    } catch (err) { console.log(err); client.destroy(); startup(); }
}

/* spawn child process to colect position data on seprate process
var cp = require('child_process');
var child = cp.spawn('node', ['server.js'], { detached: true, stdio: ['ignore'] });
child.unref();
*/
startup();

function cleanup() {
    clearInterval(temperatureInterval);
    clearInterval(vibrationInterval);
    clearInterval(vibrationSendInterval);
    //clearInterval(positionInterval);

    if (runMode == "run") {
        run();
    }
}



process.on('uncaughtException', (err, origin) => {
    console.log(err);
    console.log(origin);
    console.log(err.stack)
    console.log("ERR     starting cleanup");
    cleanup();
});



class SendBuffer {//in the event that coms are down this will acumulate data to send
    stored = {
        //xxxxxxxxxxxxxxx:{type: "", data: [{ val: 0, time: 0 }]} 
    };
    order = [];
    inTransit = {};
    addData = function (data = { type: "", data: [{ val: 0, time: 0, loc: 0 }] }) {
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
        //console.log("^^^^^^^^^^^^")
        if (this.stored[nextuuid] == undefined) { delete this.stored[nextuuid]; return null; }
        let tosend = this.stored[nextuuid];
        //console.log("&&&&&&&&&&&&&&&&&&&")
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
        let date = new Date().getTime();
        let uuid = 'xxxxxxxxxxxxxxx'.replace(/[x]/g, function (c) {
            let r = (date + Math.random() * 16) % 16 | 0;
            date = Math.floor(date / 16);
            return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
        });
        return uuid;
    }
}
function run() {
    let buffer = new SendBuffer();
    function trysend(data = { type: "", data: [{ val: 0, time: 0, loc: 0 }] }) {
        buffer.addData(data);
        let nextdata = buffer.GetNextSend();
        if (nextdata != null) {
            //console.log("sending"+JSON.stringify(nextdata))
            sendData(nextdata);
        } else { console.log("norywtvr " + buffer.CanSend()) }
    }

    function sendData(data = { type: "", data: [{ val: 0, time: 0, loc: 0 }], uuid: "xxxxxxxxxxxxxxx" }) {
        let client = new net.Socket();
        let conectTimeout;
        client.connect(config.controlRoom.TCPportrecive, config.controlRoom.IP, function () {
            client.write(JSON.stringify(data) + "<EOF>"); //send first data
            conectTimeout = setTimeout(function (dat) { client.destroy(); sendData(dat); }.bind(data), 20000);//if the socket times out destroy it and resend the data 20 seconds
        });
        client.on('data', function (resp) {
            clearTimeout(conectTimeout);//remove the timeout on sucessfull responce
            if (resp.indexOf("200") != -1) {
                // (?:200-)([0-9a-zA-Z]{15})//alternative regex
                let uuid = /(?:200-)(.*)/.exec(resp);// find uuid of last packet in responce
                buffer.ConfirmSend(uuid[1]);
                let nextdata = buffer.GetNextSend();
                if (nextdata == null) { client.destroy(); }
                else {//there is moar data to send so just reuse this existing conection
                    client.write(JSON.stringify(nextdata) + "<EOF>");
                    conectTimeout = setTimeout(function (dat) { client.destroy(); sendData(dat); }.bind(nextdata), 20000);
                }
            }
            else { client.write(JSON.stringify(data) + "<EOF>"); }
        });
    }

    function sendalert(message = "") {//used to send one time string message if necicary
        let client = new net.Socket();
        client.connect(port, ip, function () {
            client.write(message + "<EOF>");
        });
        client.on('data', function (data) {
            if (data == "200") {
                client.destroy(); // kill client after server's response
            } else { }
        });
    }

    temperatureInterval = setInterval(checkTemperatuer, config.checkTemperatuerEvery);
    vibrationInterval = setInterval(recordVibration, 1);
    vibrationSendInterval = setInterval(sendVipration, 1000);
    //positionInterval = setInterval(getposition, config.getpositionEvery);

    function checkTemperatuer() {
        let ADconverter = spi.open(config.AdcSPIbus, config.AdcSPIdeviceNumber, (err) => {
            if (err) { console.log(err); return; }
            ADconverter.transfer(mesage_readCH5, (err, message) => {
                if (err) { console.log(err); throw err; }
                let rawValue = ((message[0].receiveBuffer[1] & 0x03) << 8) + message[0].receiveBuffer[2];
                let voltage = rawValue * 3.3 / 1023;
                let celcius = (voltage - 0.5) * 100;
                if (celcius > config.motorfanOnTemperature) {
                    setfanrunning(true)
                } else if (celcius < config.motorfanOnTemperature - 3) { setfanrunning(false) }//hysteris for fan
                let now = new Date().getTime();
                trysend({ type: "temp", data: [{ val: celcius, time: now, loc: 0 }] });
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
        let acc = { x: 0, y: 0, z: 0, time: new Date().getTime(), val: 0, loc: 0 }
        //TODO:get the acc
        acc.x = Math.random();
        acc.z = Math.random();
        acc.y = Math.random();
        acc.val = Math.sqrt((acc.x * acc.x) + (acc.y * acc.y) + (acc.z * acc.z));

        vibrationBuffer.push(acc);
    }

    function sendVipration() {
        trysend({ type: "acc", data: vibrationBuffer });
        vibrationBuffer = [];
    }
}


