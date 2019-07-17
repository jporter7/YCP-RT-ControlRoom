'use strict';
const Net = require('net');
const spi = require('spi-device');
const config = require('./config.json');
let azEncoderMessage = [{
    sendBuffer: Buffer.from([0x00, 0x00, 0x00, 0x00, 0x00, 0x00]),
    receiveBuffer: Buffer.alloc(6),
    byteLength: 6,
    speedHz: 200000,
    microSecondDelay: 0xafff

}];


setInterval(loop, 1000)

function loop() {
    /*
    //,get11(),get20(),get21()
    Promise.all([get20()])
        .then(function (values) {//wait for all the promises to resolve
            console.log(values)
        }).catch((reason) => {
            console.log("promise rejected: " + reason)
        });
//*/

let AZencoder = spi.openSync(2,1,[{
    mode: 0,
    chipSelectHigh: false,
    lsbFirst: false,
    threeWire: true,
    loopback: false,
    noChipSelect: true,
    bitsPerWord: 8,
  }]);
//console.log(AZencoder.getOptionsSync());
AZencoder.transfer(azEncoderMessage, (err, message) => {
    if (err) throw(err);
    //console.log(AZencoder.getOptionsSync());
    // Convert raw value from sensor to celcius and log to console
    //let rawValue = ((message[0].receiveBuffer & (0x1FFFFF << 8)) >> 8);//21 bit mask over the position value
    var read=message[0].receiveBuffer;
    var datagood=(read[2]>>7);
    if (datagood==0){
        console.log("data bad");
        return
    }
    var chcksum=read[5]&0x7f;
    console.log((message[0].receiveBuffer) ," checksum: "+chcksum);
    var val=(read[3]<<8)+read[4];
    //(read[1]<<32)+(read[2]<<24)+
    var vallow=(read[3]<<16)+(read[4]<<8)+read[5];
    

    console.log(vallow.toString(2))
    console.log(val,val.toString(2),);
    AZencoder.close(nothing);
});
}

/**
 * 
{
  mode: 0,
  chipSelectHigh: false,
  lsbFirst: false,
  threeWire: false,
  loopback: false,
  noChipSelect: false,
  ready: false,
  bitsPerWord: 8,
  maxSpeedHz: 16000000
}
 */




function get10() {
    return new Promise((resolve, reject) => {
        let AZencoder = spi.open(1, 0, (err) => {

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

function get11() {
    return new Promise((resolve, reject) => {
        let AZencoder = spi.open(1, 1, (err) => {
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

var options = {
    threeWire: true,
    noChipSelect: true,
};

function get20() {
    return new Promise((resolve, reject) => {
        let AZencoder = spi.open(2, 0, options, (err) => {
            if (err) { reject(err); console.log(err); return; }
            //AZencoder.setOptionsSync(0,options);
            console.log(AZencoder.getOptionsSync());
            AZencoder.transfer(azEncoderMessage, (err, message) => {
                if (err) reject(err);
                // Convert raw value from sensor to celcius and log to console
                let rawValue = ((message[0].receiveBuffer & (0x1FFFFF << 8)) >> 8);//21 bit mask over the position value
                console.log(message);
                resolve(rawValue);
                AZencoder.close(nothing);
            });
        });
    });
}

function nothing() {

}

function get21() {
    return new Promise((resolve, reject) => {
        let AZencoder = spi.open(2, 1, (err) => {
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