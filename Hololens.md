# Instructions for Hololens
The [MixedRealityToolkit for Unity](https://github.com/Microsoft/MixedRealityToolkit-Unity) includes alot of good documentation and out of the box functionality for the Hololens.

Follow the [Quick Start](https://github.com/Microsoft/MixedRealityToolkit-Unity/blob/master/GettingStarted.md) guide to get the project set up to deploy to Hololens.

Add a hololens camera and cursor prefab.
Scale the maze to be the correct real world size. 1 unit in unity is equivalent to one meter. 

## Placing the maze
Add the SpatialPerception prefab into the scene. This will allow you to see a mesh around the real world environment.

Create a "HoloLens Managers" empty object and attach the scripts:
- Gaze Manager
- Input Manager
- Gaze Stabilizer
- Foxus Manager
- And any other managers that might be needed

On the parent object for the maze, attach a TapToPlace script with `isBeingPlaced` enabled. This should allow the player to place the maze in the place they want to play, for example, on top of a table. Play around with the size because the smaller it is the harder it will be to be precise about which walls you want to interact with.

## Moving the walls
Create a script that implements the `IInputClickHandler.cs` on the wall objects.
The `OnInputClicked` function should contain the functionality that is in MouseDown on the `WallPathfinding` script.

## Setting up the Game
Use [VRDevice.isPresent](https://docs.unity3d.com/530/Documentation/ScriptReference/VR.VRDevice-isPresent.html) to determine if the game is being played on the Hololens (aka is in defender mode or attack mode).  Certain functionalities and game objects can be enabled/disabled depending if you are in attack or defender mode.


