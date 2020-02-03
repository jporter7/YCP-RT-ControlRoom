'use strict';
//const Net = require('net');
//const spi = require('spi-device');
//const config = require('./config.json');
console.log("Start\n")
var telescopePos = 5;
var overlap = 0;
var upperOverlap = 0;
var lowerOverlap = 0;
var incr = 0;
var testArr = [
    4, 3, 2, 1, 0, 359, 358, 359, 0, 1, 2, 3, 4, 5, 4, 3, 2, 1, 359, 358, 359, 360, 0, 1, 2, 4,6, 8, 10, 8, 6, 4, 2, 1, 0, 359, 358, 359, 3, 2, 1, 0, 359, 358, 359, 0, 1, 2];

//For testing only
function getAZposition()
{
    let pos = testArr[incr];
    incr++;
    return pos;
    
}

function checkPos()
{
    //Initializes newPos and currentPos to same value
    let newAzPos = getAZposition();
    let currentAzPos = newAzPos;

    while(incr < testArr.length)
    {
        newAzPos = getAZposition();
        console.log("Current: " + currentAzPos + " New: " + newAzPos + " Upper: " + upperOverlap + " Lower: " + lowerOverlap + " Tele: " + telescopePos);
        
        //console.log("newAzPos: " + newAzPos + " currentAzPos: " + currentAzPos);
        
        //Use difference + overlap bit to determine when encoder crosses 0 and from which side
        //359 to 361
        if(currentAzPos > 359 && newAzPos < 5 && telescopePos > 300)
        {
            console.log("359 to 361");
            upperOverlap = 1;
        }
        //361 to 359
        if(currentAzPos < 5 && newAzPos > 350 && telescopePos > 300)
        {
            console.log("361 to 359");
            upperOverlap = 0;
        }

        //-1 to 1
        if(currentAzPos > 359 && newAzPos < 5 && telescopePos < 10)
        {
            console.log("-1 to 1");
            lowerOverlap = 1;
        }
        //1 to -1
        if(currentAzPos < 5 && newAzPos > 350 && telescopePos < 10)
        {
            console.log("1 to -1");
            lowerOverlap = 0;
        }


        //Upper and Lower Overlap Logic
        if(upperOverlap == 1 && lowerOverlap == 0)
        {
            telescopePos = 360 + newAzPos;
            console.log("Telescope Pos1: " + telescopePos);
        }
        else if(upperOverlap == 0 && lowerOverlap == 1)
        {
            telescopePos = newAzPos - 360;
            console.log("Telescope Pos2: " + telescopePos);
        }
        else if(lowerOverlap == 0 && upperOverlap == 0)
        {
            telescopePos = currentAzPos;
            console.log("Telescope Pos3: " + telescopePos);
        }
        else
        {
            console.log("Error: Upper and Lower overlap are both 1");
        }

       

        currentAzPos = newAzPos;
    }



}


checkPos();