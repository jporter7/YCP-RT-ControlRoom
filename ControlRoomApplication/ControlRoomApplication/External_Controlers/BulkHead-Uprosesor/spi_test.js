'use strict';
const Net = require('net');
const spi = require('spi-device');
const config = require('./config.json');
const Gpio = require('onoff').Gpio;


var SPIdevice = spi.openSync(1, 0,);

//10000010
configure();
async function configure() {
    console.log(SPIdevice.getOptionsSync());
    SPIdevice.setOptionsSync({
        mode: spi.MODE1
    })
    console.log(SPIdevice.getOptionsSync());
    //.SetOptions(1,[{setMode:0b10}])
    //*
    spi_transfer(Buffer.from([0x0f]), 105, 1000000, SPIdevice).catch(reason => {//stop continuos recording cmd
        console.log(reason)
    });
    await sleep(1);
    writereg(0x03,0b10110000);//write drate register
    await sleep(1);
    spi_transfer(Buffer.from([0xf0]), 105, 1000000, SPIdevice).catch(reason => {//selfcal cmd
        console.log(reason)
    });

    await sleep(5000);
    //*/
    setInterval(loop, 1000)
}

function writereg(reg,val){
    spi_transfer(Buffer.from([0x5|reg, 0x00,val]), 105, 1000000, SPIdevice).catch(reason => {
        console.log(reason)
    });
}

function sleep(ms){
    return new Promise(resolve => setTimeout(resolve, ms));
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
        });
        

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
        
        //*/
        var time = process.hrtime();

    spi_transfer(Buffer.from([0x10, 0x02,]), 105, 1000000, SPIdevice).then(nothing => {
        var diff = process.hrtime(time);
        //console.log("first  "+ diff[0] * 1e9 + diff[1]);
    }).catch(reason => {
        console.log(reason)
    });
    spi_transfer(Buffer.from([0x00, 0x00]), 105, 1000000, SPIdevice).then(ret => {
        //SPIdevice.close(nothing);
        var diff = process.hrtime(time);
        console.log("second "+diff[0] * 1e9 + diff[1]);
        console.log(ret);

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
            microSecondDelay: 10,

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
