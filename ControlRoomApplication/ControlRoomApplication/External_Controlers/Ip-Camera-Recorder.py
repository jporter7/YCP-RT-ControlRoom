hires = True
camIP ="169.254.113.233"





import numpy as np
import cv2
import time
import imutils

width = 1920
height = 1080
start = time.time()
streamId=1
if hires:
    streamId=0
cap = cv2.VideoCapture('rtsp://'+camIP+':554/user=admin_password=6QNMIQGe_channel=0_stream='+str(streamId)+'.sdp/trackID=4')
fourcc = cv2.VideoWriter_fourcc(*'XVID')
#out = cv2.VideoWriter('output.avi', fourcc, 20.0, (704,480))
out = cv2.VideoWriter('output.avi', fourcc, 20.0, (width,height))

ret, frame = cap.read()
end = time.time()
print("time to aquire capture "+str(end - start))

cv2.imshow('image',frame)
ret, firstFrame = cap.read()

firstFrame = cv2.cvtColor(firstFrame, cv2.COLOR_BGR2GRAY)
firstFrame = cv2.GaussianBlur(firstFrame, (21, 21), 0)

height1, width1 = frame.shape[:2]
if (height1 != height) or (width1 != width):
    height = height1
    width = width1
    out = cv2.VideoWriter('output.avi', fourcc, 20.0, (width,height))



frameCount =0
while(True):
    frameCount+=1
    ret, frame = cap.read()
    #cv2.imshow('frame',frame)
    #imshow( "Display window", image ); # Show our image inside it.
    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
    gray = cv2.GaussianBlur(gray, (21, 21), 0)
    frameDelta = cv2.absdiff(firstFrame, gray)
    thresh = cv2.threshold(frameDelta, 25, 255, cv2.THRESH_BINARY)[1]


    thresh = cv2.dilate(thresh, None, iterations=2)
    cnts = cv2.findContours(thresh.copy(), cv2.RETR_EXTERNAL,
        cv2.CHAIN_APPROX_SIMPLE)
    cnts = imutils.grab_contours(cnts)


    for c in cnts:
        # if the contour is too small, ignore it
        if cv2.contourArea(c) < 200:
            continue

        # compute the bounding box for the contour, draw it on the frame,
        # and update the text
        (x, y, w, h) = cv2.boundingRect(c)
        cv2.rectangle(frame, (x, y), (x + w, y + h), (0, 255, 0), 2)
    

    out.write(frame)
    cv2.imshow('image',frame)

    if frameCount > 500:
        break
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

cap.release()
out.release()
cv2.destroyAllWindows()

print("fin")

