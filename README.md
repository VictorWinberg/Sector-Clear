# Sector-Clear

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/df06bdca16ce4e24ab024fc3dc927b89)](https://www.codacy.com/app/dat14vwi/Sector-Clear?utm_source=github.com&utm_medium=referral&utm_content=VictorWinberg/Sector-Clear&utm_campaign=badger)

*Course: 1DV437 Introduction to Game Programming*
## Game Description
You play as a soldier in an elite strike unit. The earth is at war unlike any war we've seen before. Sector clear is a program where soldiers compete against each other to improve and develop new combat skills. Sectors contains enemies spawned at random. You fight a variety of enemies and other soldiers with your weapons at your disposal.

### Prototype
![Prototype image](https://writelatex.s3.amazonaws.com/tcyfphxkhxwf/uploads/603/13737516/1.png "Prototype image")

### Game Objectives
The goal of game is to is to climb the leaderboard and achieve the highest score. Score is awarded for each enemy killed, player neutralized and sector cleared. Your task is to make it through as many sectors as possible. This multiplayer game is over either when the enemies kills you or if you get neutralized by another player.

### Feature list
This game will feature the features listed below. These features may have some changes, deletions, or additionals onto at any time during the development. These changes will be documented at the game release.

##### Player
* The player can move in any direction with WSAD.
* The player can aim with the mouse.
* The player can shoot bullets with the Left Mouse Key.
* After X amount of bullets has been fired the player need to reload the gun before firing again (i.e. limited fire rate).
* The bullets have ''unlimited'' range because they are destroyed on impact with enemies, players or sector walls.
* Sound effects shall be played when firing the weapons.
* The player starts with 100% health, which is reduced by X% each time a enemy hits the player, depending on the enemy damage.
* When the player is hit a sound effect is played.
* When the player is neutralized the player will get timed out.
* When the player is killed the screen will fade away and a sound effect is played.

##### Weapons
* The weapons that are available will be of the types assault rifle, shotgun and pistol.

##### Enemy
* Enemies are spawned at random locations in a section.
* Enemies cannot spawn at the some location or too close to each other.
* Each enemy continuously chasing the closest player.
* The enemies has a short range where they can hit a player.
* A enemy is killed if hit by X amount of bullets shot by a player.
* When a enemy is killed a sound effect and death animation is played, and the player is awarded X amount of points.

##### Camera
* The camera follows the player from above in a top-down fixed-camera view.

##### UI
* Before the game starts an “Are you ready? Press Space” text is shown, and the game starts when the player presses the Space key.
* When the game is ended a “Game Over! Press R to restart” text is shown, and the game restarts when the player presses the R key.
* The player score is shown in the top center of the screen.
* The kill-feed is shown on the top right of the screen.
* A player health bar is shown on the bottom center of the screen. The color of the bar changes from green to yellow to red depending on how much health the player has left.

##### Sector
* The sector consist of randomly spawned enemies with some random variance of properties, e.g. damage and speed.
* The sector also consist of randomly spawned objects, e.g. pillars.
* The further away from the player's starting point the harder the sectors become.
* When all the enemies in the sector is killed the doors will open to the previous and next sector.

##### Game World
* The game world spawns random sectors in a single line throughout the Z-axis.
* The player is spawned in a waiting room at (0,0,0) and will enter the combat mode when walking into the first sector.
* The sectors has no height differences.

### Asset list
I will use the following assets from Unity Asset Store in my game:
* [Standard Assets (mainly textures and effects)](https://www.assetstore.unity3d.com/en/#!/content/32351)
* [Post Processing Stack](https://www.assetstore.unity3d.com/en/#!/content/83912)
* [Survival Shooter tutorial (maybe using some models)](https://www.assetstore.unity3d.com/en/#!/content/40756)
* [Tanks! Tutorial (again, maybe using some models)](https://www.assetstore.unity3d.com/en/#!/content/46209)
* [Unity Particle Pack](https://www.assetstore.unity3d.com/en/#!/content/73777)
* [Low Poly: Free Pack](https://www.assetstore.unity3d.com/en/#!/content/58821)
* [Mega Fantasy Props Pack](https://www.assetstore.unity3d.com/en/#!/content/87811)
* [Unity Samples: UI](https://www.assetstore.unity3d.com/en/#!/content/25468)
* [Create a Game (Unity 5) - YouTube tutorial](https://www.youtube.com/playlist?list=PLFt_AvWsXl0ctd4dgE1F8g3uec4zKNRV0)
