# VR Controller

Contents:
This file contains a Unity VR (2020.3.30f1) project. You should be able to download and open the project from the Unity Hub.

NOTE:
I have only tested this with the Oculus Quest 2. If you are using a different device you will need to make some changes. I'll detail where below.
This controller was developed for a stationary VR game where the player character does not move. I have made some changes to the script to allow for the player to be controlled or a different game object to be controlled. 

Scene: VR Controller Demo
In this scene there is a Directional Light, Plane, XR Interaction Manager, XR Origin and a Cube.

The XR Interaction Manager Has the XR Interaction Manager and Input Action Manager scripts attached. The Input Action Manager should have one action asset, the XRI Default Input Actions. NOTE: I added a custom action map to this input action asset and is required for the controller to work properly.

The XR Origin has the VR Controller script on it. NOTE: This script must stay on the XR Origin for the controller to work properly.

The cube is there for perspective.

VR Controller:
The gameObjectToMove GameObject is the game object that will be controlled by the script.

The rightControllerGrip ande leftControllerGrip Input Actions are the buttons used to initiate movement and rotation. NOTE: This must set two Oculus Touch Controller (Right Hand/Left Hand) for the controller to work properly. If you are not using the Oculus Quest 2 you can try using the XR Controller (Right Hand/Left Hand) or one of the other controllers.

The rightControllerPositionProperty, leftControllerPositionProperty, rightControllerVelocityProperty and leftControllerVelocityProperty Input Action Properties get the position and velocity of the right and left controllers. NOTE: They must be set to Use Reference and they must be set to OculusControllerPositionVelocity/RightPosition, OculusControllerPositionVelocity/LeftPosition, OculusControllerPositionVelocity/RightVelocity and OculusControllerPositionVelocity/LeftVelocity for the controller to work properly. Again, if you are not using the Oculus Quest 2 you can try changing the binding to your controller in the XRI Default Input Actions.

The moveSpeed, rotateSpeed, floats set how fast the object will move and rotate. 

The moveVelocityMinimumThreshold and rotateVelocityMinimumThreshold floats sets the minimun velocity required for the movment and rotation to initiate.

The invertControls bool will do just that. It will invert the move and rotation directions. I added this so that you can quickly and easily change them depending on if the gameObjectToMove is the player or a different object.
