'use strict';
const Net = require('net');
const spi = require('spi-device');
const config = require('./config.json');
const Gpio = require('onoff').Gpio;


var SPIdevice = spi.openSync(2, 1, [{/////////this throws an error if spi buss is not properly configured
    mode: 0,
    chipSelectHigh: false,
    lsbFirst: false,
    threeWire: true,
    loopback: false,
    noChipSelect: true,
    bitsPerWord: 8,
}]);

//10000010
configure();
function configure() {

    setInterval(loop, 1000)
}


function loop() {
    /*
    //,get11(),get20(),get21()
    Promise.all([get20()])
        .then(function (values) {//wait for all the promises to resolve
            console.log(values)
        }).catch((reason) => {
            console.log("promise rejected: " + reason)
        });
//*/            //chipselest only works for device 0
    //0x10, 0x01     , 
    /*    spi_transfer(Buffer.from([0x11,0x01]), 105, 200000, SPIdevice).then(ret=>{
             //SPIdevice.close(nothing);
           
            var datagood = (ret[2] >> 7);
            if (datagood == 0) {
                //console.log("data bad");
                //return;
            }
            var chcksum = ret[5] & 0x7f;
            console.log((ret), " checksum: " + chcksum);
            var val = (ret[3] << 8) + ret[4];
            //(read[1]<<32)+(read[2]<<24)+
            var vallow = (ret[3] << 16) + (ret[4] << 8) + ret[5];
        
            //console.log(vallow.toString(2))
            console.log(val, val.toString(2));
        }).catch(reason=>{
            console.log(reason)
        });//*/
    spi_transfer(Buffer.from([0x10, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00]), 105, 200000, SPIdevice).then(ret => {
        //SPIdevice.close(nothing);

        var datagood = (ret[2] >> 7);
        if (datagood == 0) {
            //console.log("data bad");
            //return;
        }
        var chcksum = ret[5] & 0x7f;
        console.log((ret), " checksum: " + chcksum);
        var val = (ret[3] << 8) + ret[4];
        //(read[1]<<32)+(read[2]<<24)+
        var vallow = (ret[3] << 16) + (ret[4] << 8) + ret[5];

        //console.log(vallow.toString(2))
        console.log(val, val.toString(2));
    }).catch(reason => {
        console.log(reason)
    });

}







function spi_transfer(message, devicePinNumber, speed, SPIdevice) {
    return new Promise((resolve, reject) => {
        let spimessage = [{
            sendBuffer: message,
            receiveBuffer: Buffer.alloc(Buffer.byteLength(message)),
            byteLength: Buffer.byteLength(message),
            speedHz: speed,
            microSecondDelay: 50

        }];
        SPIdevice.transfer(spimessage, (err, message) => {
            if (err) reject(err);
            //console.log(AZencoder.getOptionsSync());
            // Convert raw value from sensor to celcius and log to console
            //let rawValue = ((message[0].receiveBuffer & (0x1FFFFF << 8)) >> 8);//21 bit mask over the position value
            var read = message[0].receiveBuffer;
            resolve(read);
        });
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
