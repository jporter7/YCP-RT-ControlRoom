'use strict';
//const Net = require('net');
//const spi = require('spi-device');
//const config = require('./config.json');
console.log("Start\n")
var telescopePos = 357;
var overlap = 0;
var upperOverlap = 0;
var lowerOverlap = 0;
var incr = 0;
var testArr = [];
//Test Lower overlap
//testArr = [3, 2, 1, 0, 359, 358, 357, 358, 359, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 3, 2, 1, 0, 359, 358, 357, 358, 359, 0, 1, 2, 3];
//Test Upper overlap
testArr = [357, 358, 359, 0, 1, 2, 3, 2, 1, 0, 359, 358, 359, 0, 1, 0, 359, 358];

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
        
        //Use difference + overlap bit to determine when encoder crosses 0 and from which side
        //359 to 361
        if(currentAzPos > 355 && newAzPos < 5 && telescopePos > 300)
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
        if(currentAzPos > 350 && newAzPos < 5 && telescopePos < 10)
        {
            console.log("-1 to 1");
            lowerOverlap = 0;
        }
        //1 to -1
        if(currentAzPos < 5 && newAzPos > 350 && telescopePos < 10)
        {
            console.log("1 to -1");
            lowerOverlap = 1;
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
            telescopePos = newAzPos;
            console.log("Telescope Pos3: " + telescopePos);
        }
        else
        {
            //console.log("Error: Upper and Lower overlap are both 1");
        }

       

        currentAzPos = newAzPos;
    }



}


checkPos();