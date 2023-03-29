# WheelchairSimulator

This is a lightweight unity game. The player continuously moves forward, a variable _direction_ is modified to steer the player left or right.
This simulator is intended to test out how steering with gaze would feel like in a first-person view.

There are currently 2 ways that control the player's steering (changes the player's direction).

1. **KeyboardInputController** (move with left and right key). This is attached to the GameManager obj, it is currently disabled. For debug/fun use only.
2. **ConnectionsHandler** and **PlayerController** (a UDP Listener class attached to the GameManager obj) listens to UDP messages. There is an obj called UDPSenderTesterObj with a UDPSenderTester.cs script attached, which continuously broadcasts a float direction variable through UDP. You can change the direction variable in runtime to test out how it changes the direction of the movement.

These 3 component

For the _direction_ variable, -45 means turning 45 degrees left, 0 goes straight, +45 turns 45 degrees towards right per second.
If you can get **your own gazetracking code** to send a float value that represents direction over via UDP to port 8051, you should be able to control the player's steering via gaze.

Credit: Josh, Lam
