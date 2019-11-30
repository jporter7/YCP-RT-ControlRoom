const i2c = require('i2c-bus');

const DS1621_ADDR = 0x53;
const CMD_ACCESS_CONFIG = 0xac;
const CMD_READ_TEMP = 0xA7;
const CMD_START_CONVERT = 0xee;
const ADXL345_POWER_CTL = 0x2D;
const toCelsius = (rawTemp) => {
    const halfDegrees = ((rawTemp & 0xff) << 1) + (rawTemp >> 15);

    if ((halfDegrees & 0x100) === 0) {
        return halfDegrees / 2; // Temp +ve
    }

    return -((~halfDegrees & 0xff) / 2); // Temp -ve
};

const displayTemperature = () => {
    const i2c1 = i2c.openSync(1);
    console.log(i2c1);
    // Enter one shot mode (this is a non volatile setting)
    //i2c1.writeByteSync(DS1621_ADDR, CMD_ACCESS_CONFIG, 0x01);
    console.log(i2c1.readByteSync(DS1621_ADDR, CMD_ACCESS_CONFIG))
    // Wait while non volatile memory busy
    while (i2c1.readByteSync(DS1621_ADDR, CMD_ACCESS_CONFIG) & 0x10) {
    }

    // Start temperature conversion
    i2c1.sendByteSync(DS1621_ADDR, CMD_START_CONVERT);

    // Wait for temperature conversion to complete
    while ((i2c1.readByteSync(DS1621_ADDR, CMD_ACCESS_CONFIG) & 0x80) === 0) {
    }

    // Display temperature
    const rawTemp = i2c1.readWordSync(DS1621_ADDR, CMD_READ_TEMP);
    console.log('temp: ' + toCelsius(rawTemp));

    i2c1.closeSync();
};

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

//displayTemperature();
let completed=0;
async function run() {
    const i2c1 = i2c.openSync(2);
    console.log("start");
    i2c1.writeByteSync(DS1621_ADDR, ADXL345_POWER_CTL, 0);
    i2c1.writeByteSync(DS1621_ADDR, ADXL345_POWER_CTL, 16);
    i2c1.writeByteSync(DS1621_ADDR, ADXL345_POWER_CTL, 8);
    console.log("configd");
    console.time();
    for(var i=0;i<50;i++) {
        for(var j=0;j<20;j++) {
            let data = Buffer.alloc(6);
            i2c1.readI2cBlock(DS1621_ADDR, 0x32, 6, data, I2CCallbac);
        }
        /*
        try {
            //Buffer[] data;//[6];// =new Buffer[6];
            let data = Buffer.alloc(6);
            i2c1.readI2cBlockSync(DS1621_ADDR, 0x32, 6, data);
            // console.log(data[1].toString(2).padStart(8, '0')+""+data[0].toString(2).padStart(8, '0')+" "+data[3].toString(2).padStart(8, '0')+""+data[2].toString(2).padStart(8, '0')+" "+data[5].toString(2).padStart(8, '0')+""+data[4].toString(2).padStart(8, '0') );
            let x = Twos_compliment_16(data[1], data[0]);
            let y = Twos_compliment_16(data[3], data[2]);
            let z = Twos_compliment_16(data[5], data[4]);
            //console.log(` ${x/256} ${y/256} ${z/256}`);
        } catch (err) {
            console.log(err);
            await sleep(200);
        }
        */
    }
}

run();

function I2CCallbac (error, bytesRead, buffer){
    completed++;
    if (completed==1000){
        console.timeEnd()
    }
}

function Twos_compliment_16(msw, lsw) {
    let unsignedValue = (msw << 8) + (lsw);
    if ((unsignedValue & 0x8000)) {
        return (unsignedValue | 0xffff0000);
    } else {
        return unsignedValue;
    }
}