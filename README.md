### Using the EyeTribe eyetracker with Unity3D

Rich Stoner, 1/23/2014

![Screenshot](https://raw2.github.com/richstoner/unofficial-eyetribe-unity3d/master/screenshot.jpg)

This respository contains an initial client for the [EyeTribe eyetracker](https://theeyetribe.com/) in the Unity3D game engine (v4.x). 

### Important 

1. This will only work on Windows targets.
2. This is completely unofficial.
3. This requires the [EyeTribe developer SDK](http://dev.theeyetribe.com/general/) (not included).
4. This code requires an [EyeTribe eyetracker](https://theeyetribe.com/).
4. This code uses a game from the C# game examples released by M2h ([http://www.m2h.nl/unity/](http://www.m2h.nl/unity/))

### How to test

Download the beta release from [https://github.com/richstoner/unofficial-eyetribe-unity3d/releases](https://github.com/richstoner/unofficial-eyetribe-unity3d/releases). Follow the install instructions and post any issues if you come across them. *You will need to run the EyeTribe calibration prior to using the game.*

### For Developers

Clone this repository [https://github.com/richstoner/unofficial-eyetribe-unity3d.git](https://github.com/richstoner/unofficial-eyetribe-unity3d.git) and import it into Unity. The necessary scripts and required DLLs are in the plugins directory. You should be able to load the Shooting Gallery scene and run it without an issue. *You will need to run the EyeTribe calibration prior to using the game.*

#### Source overview

The key source files that make this work are highlighted in this [gist](https://gist.github.com/richstoner/8587990).

**ETListener.cs**

This file connects to the EyeTribe C# client. The ETlistener class implements the IGazeUpdateListener and handles updates from the tracker server. 

Source for the EyeTribe official client can be found at [https://github.com/EyeTribe/TETCSharpClient](https://github.com/EyeTribe/TETCSharpClient). This project uses the precompiled DLL rather than the client source. 

**EyeTribeClient.cs**

This file connects to the listener and provies a most recent gaze point for the Unity engine to pull from. Any other gameobjects in the Unity can reference this object and get the current gaze points.

**CursorImage.cs**

Example code to present the current gaze position as a sprite in the GUI.

**PlayerInput.cs**

Example code to handle the gaze input and use it as a projected ray with hit detection in the 3d environment. **Note: uses the inverted Y position!**


#### Notes

We're using an older version of the Newtonsoft.Json.dll that has been compiled with .NET 2.0 to make it compatible with Unity 4 (I believe we could have used a .NET 3.5 dll as well but this provides all of the JSON serialization functionality needed).



