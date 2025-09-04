# Rovio Test
First of all, say thank you for this opportunity. This test has been quite interesting.

https://github.com/user-attachments/assets/8901add6-a7a3-4e2e-a4b9-b66b602f8088

## Steps
1. First of all, I play the game and take into account some details about game flow, game interactions, controls, and more.
2. Second, I created a **Trello** Board to handle the task. [Trello](https://trello.com/b/SmkQAxU5/roviotest).
3. I begin to do the project. In the next list, I write down some of the stimations and how it was at the end.

## Ability design
For the ability, I choose to follow the game without being crazy. The ability is a fast ball simulating the baseball pitch launch. It makes the ball smaller and faster until it hits the opponent or the opponent hits the ball.

## Architecture overview

The architecture follows a **Service Locator**. This Service locator, like some of the services and some utilities, is taking from my own public code located in [github](https://github.com/antoniojesusnc/UrdLib). You will be able to differentiate if it is from this source by checking the namespace "Urd" instead of "RovioTest"

You can find the service locator configuration in the folder "Configs/ServicesLocatorConfig".
In this file, you can find all the main services of the project and the service responsible for the GamePlay ("GamePlayService"). This service contains submodules that handle each part of the gameplay play such as the Level itself, Enemy, Player, or Ball.

This entity has its own **Views** ("MonoBehavior"), **Model**, and **Configs** ("ScriptableObjects").
The views can be found in the scenes and are responsible for showing how the model acts.

To handle the **communications** between the different parts of the game, there is an "EventBusService" that handles **services**. Any class can subscribe to certain events using an interface, and then it will receive when an event is sent. Thanks to this system, it's easy to react when something happens without connecting different parts of the game.

The Enemy has two different behaviors based on some classes: Movement and Hitter. As it is now, in the config, there are both of those configurations, allowing for combining and creating different enemies quite easily.

## Code Steps a.k.a. Time Spent
1. Project configurations (Unity and Plugins): Here, I downloaded Unity, created a Trello, and added the necessary plugins to begin to work.
- Estimated Time: 0:30 min.
- Real Time aprox: 0:30 min.
3. Set Up of the project: He, re I set up the main architecture of the project, and made the navigation between scenes.
- Estimated Time: 1:00 hours
- RealTime  approximately 0:45 hours
4. Main Scene set up: Here, I begin to create the main court and add some extra logic to handle it.
- Estimated Time: 0:45 min
- Real Time aprox: 0:45 min
5. Character Movement and detech release to try to  hit the ball.
- Estimated Time: 1:30 h
- Real Time aprox: 2:00 h
6. Ball Movement: ball movement in a straight line.
- Estimated Time: 1:00 h
- Real Time aprox: 1:00 h
7. Enemy Movement behavior randomly straight for ara  certain time.
- Estimated Time: 0:45 h
- Real Time aprox: 1:30 h
8. Enemy Hit behavior by proximity.
- Estimated Time: 0:45 h
- Real Time aprox: 1:15 h
9. Enemy and Player being hit by the ball, and making the service.
- Estimated Time: 0:45 h
- Real Time aprox: 0:30 h
10. Game Loop.
- Estimated Time: 0:45 h
- Real Time aprox: 0:45 h
11. Camera and game loop improvements.
- Estimated Time: 0:45 h
- Real Time aprox: 0:30 h
12. Ball Reflecting in the walls.
- Estimated Time: 1:00 h
- Real Time aprox: 1:00 h
13. Hero Skill.
- Estimated Time: 3:00 h
- Real Time aprox: 2:00 h
14. Smash actions.
- Estimated Time: 1:00 h
- Real Time aprox: 1:00 h
15. Serve Counter.
- Estimated Time: 1:30 h
- Real Time aprox: 0:30 h
16. Improving the court.
- Estimated Time: 1:00 h
- Real Time aprox: 1:00 h
17. Character Animations.
- Estimated Time: 2:00 h
- Real Time aprox: 1:45 h
18. Ball Feedback.
- Estimated Time: 1:00 h
- Real Time aprox: 1:00 h
19. Character Feedback.
- Estimated Time: 1:00 h
- Real Time aprox: 0:45 h
20. Mix Improvements.
- Estimated Time: 2:00 h
- Real Time aprox: 2:00 h
21. UI.
- Estimated Time: 2:00 h
- Real Time aprox: 1:00 h
22. Audio & feedback.
- Estimated Time: 1:00 h
- Real Time aprox: 1:00 h
21. General polish and bug fixing.
- Estimated Time: 2:00 h
- Real Time aprox: 1:30 h	

## Plugins added:
- My Box: Tools and easy access from the editor
	[Link](https://github.com/Deadcows/MyBox.git).
- Safe Area: help with the notch in the mobiles
	[Link](https://github.com/gilzoide/unity-safe-area-layout.git).
- Serialize reference: Allow to set up a reference in the editor, useful to make generic configurations.
	[Link](https://github.com/mackysoft/Unity-SerializeReferenceExtensions.git?path=Assets/MackySoft/).
- ToolBar Extender: Allow to extend the editor with custom buttons and functionalities
	[Link](https://github.com/marijnz/unity-toolbar-extender.git).
- UI Effect: Provide a lot of effects for UI
	[Link](https://github.com/mob-sakai/UIEffect.git).
- Dotween Pro comes with tween to make easy movements and animations. Purchase in the asset store, but you can find the soft version here.
	[Link](https://dotween.demigiant.com/).
- HapticFeedback: Provides an easy way to configure Haptics
	[Link](https://github.com/CandyCoded/HapticFeedback.git#v1.0.3).
- Mobile Console Kit: A very useful console to check errors in the device. It allows adding a  custom debug command, but in this project, I didn't use this functionality
	[Link](https://github.com/pixeption/MobileConsoleKit.git).
- Joystick: Virtual Joystick used in the project.
	[Link](https://github.com/Bian-Sh/UniJoystick.git?path=Packages/Joystick).


## Assets used:
- (URP)Simple Toon Shader
	[Link](https://assetstore.unity.com/packages/vfx/shaders/urp-simple-toon-shader-243515).
- Free General Ambience Sounds
	[Link](https://assetstore.unity.com/packages/audio/ambient/urban/free-general-ambience-sounds-246000).
- Free Deadly Kombat
	[Link](https://assetstore.unity.com/packages/audio/sound-fx/free-deadly-kombat-228835).
- FREE Casual Game SFX Pack
	[Link](https://assetstore.unity.com/packages/audio/sound-fx/free-casual-game-sfx-pack-54116).
- (URP)Simple Toon Shader
	[Link](https://assetstore.unity.com/packages/vfx/shaders/urp-simple-toon-shader-243515).
- Free - Casual & Relaxing Game Music Pack
	[Link](https://assetstore.unity.com/packages/audio/music/free-casual-relaxing-game-music-pack-262740).
- RPG Essentials Sound Effects - FREE!
[Link](https://assetstore.unity.com/packages/audio/sound-fx/rpg-essentials-sound-effects-free-227708).
- Free Sport Balls
[Link](https://assetstore.unity.com/packages/3d/props/free-sport-balls-293937).
- Baseball Bats – Pack
[Link](https://assetstore.unity.com/packages/3d/props/weapons/baseball-bats-pack-102171).
- Cartoon Race Track - Oval
[Link](https://assetstore.unity.com/packages/3d/environments/roadways/cartoon-race-track-oval-175061).
- Robot Kyle | URP
[Link](https://assetstore.unity.com/packages/3d/characters/robots/robot-kyle-urp-4696).
- FreLow-Poly Nature Forest
[Link](https://assetstore.unity.com/packages/3d/environments/landscapes/free-low-poly-nature-forest-205742).
- Some Mixamo animations
	[Link](https://www.mixamo.com/).
- Some Particles from Hovl 
[Link](https://assetstore.unity.com/publishers/28391?srsltid=AfmBOorRRngH0zdAdK1_3Mao9a0oQCG-zSpnXDaZln5DvVEcjpI14KYZ).
	
## Known Issues
Some known issues need to be tested.
- The Stella of the ball is sometimes visible moving to the initial position after smashing a character.
- Sometimes the player goes off the court.
- Sometimes the ball goes out of the court.

## Missing
- It is missing a progression in the match; right now, the enemy is random.
- All characters have the same skills; I will add some different skills.


## Next
- I would add more characters, balls, and courts.
- Also,o I will add more skills.
- Right now, the main menu does not have functionality., I would add something to check the characters and their skills.
- I will do some small changes in the court to make it more attractive, like adding public or better dynamic music.
