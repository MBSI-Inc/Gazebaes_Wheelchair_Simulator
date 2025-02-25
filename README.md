# WheelchairSimulator
##Executable file 
Executable file can be found and downloaded here: https://drive.google.com/drive/folders/1KLMvJFP-mzzT8wL02fonObV2RuAe1vs8?usp=drive_link
Use your MBSI account to access

##Installation
Install unity if you want to make any changes/edits to the simulator
1. Install Unity Hub and Unity Editor (version 2022.3.14f1)
2. Clone this repository to your computer
3. Open this project in unity editor
4. Run the game, this opens up a TCP server to stream the game's video to the python-end; the unity-end also has a UDP listener that listens for movement commands from the python-end.


This is a lightweight unity game. The player continuously moves forward, a variable _direction_ is modified to steer the player left or right.
This simulator is intended to test out how steering with gaze would feel like in a first-person view.

There are currently 2 ways that control the player's steering (changes the player's direction). The two methods are toggled on and off via `Player` object -> `Eye Gaze Controller` component -> `Use Keyboard` checkbox.

1. **KeyboardInputController** (steer with A and D key) it is currently disabled. For debug/fun use only.
2. **ConnectionsHandler** and **PlayerController** (a UDP Listener class attached to the GameManager obj) listens to UDP messages. There is an obj called UDPSenderTesterObj with a UDPSenderTester.cs script attached, which continuously broadcasts a float direction variable through UDP. You can change the direction variable in runtime to test out how it changes the direction of the movement.

These 3 component

For the _direction_ variable, -45 means turning 45 degrees left, 0 goes straight, +45 turns 45 degrees towards right per second.
If you can get **your own gazetracking code** to send a float value that represents direction over via UDP to port 8051, you should be able to control the player's steering via gaze.

Credit: Josh, Lam
