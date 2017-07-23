## Current progress

### TODO
##### Player
* When the player is neutralized the player will get timed out.
* When the player is killed the screen will fade away and a sound effect is played.

##### Enemy
* Each enemy continuously chasing the closest player.
* When a enemy is killed the player is awarded X amount of points.

##### Camera
* The camera follows the player from above in a top-down fixed-camera view.

##### UI
* Before the game starts an “Are you ready? Press Space” text is shown, and the game starts when the player presses the Space key.
* When the game is ended a “Game Over! Press R to restart” text is shown, and the game restarts when the player presses the R key.
* The player score is shown in the top center of the screen.
* The kill-feed is shown on the top right of the screen.
* A player health bar is shown on the bottom center of the screen. The color of the bar changes from green to yellow to red depending on how much health the player has left.

##### Sector
* The further away from the player's starting point the harder the sectors become.
* When all the enemies in the sector is killed the doors will open to the previous and next sector.

##### Game World
* The game world spawns random sectors in a single line throughout the Z-axis.
* The player will enter the combat mode when walking into the first sector.

### ALL
##### Player
* ~~The player can move in any direction with WSAD.~~
* ~~The player can aim with the mouse.~~
* ~~The player can shoot bullets with the Left Mouse Key.~~
* ~~After X amount of bullets has been fired the player need to reload the gun before firing again (i.e. limited fire rate).~~
* ~~The bullets have ''unlimited'' range because they are destroyed on impact with enemies, players or sector walls.~~
* ~~Sound effects shall be played when firing the weapons.~~
* ~~The player starts with 100% health, which is reduced by X% each time a enemy hits the player, depending on the enemy damage.~~
* ~~When the player is hit a sound effect is played.~~
* When the player is neutralized the player will get timed out.
* When the player is killed the screen will fade away and a sound effect is played.

##### Weapons
* ~~The weapons that are available will be of the types assault rifle, shotgun and pistol.~~

##### Enemy
* ~~Enemies are spawned at random locations in a section.~~
* ~~Enemies cannot spawn at the some location or too close to each other.~~
* Each enemy continuously chasing the closest player.
* ~~The enemies has a short range where they can hit a player.~~
* ~~A enemy is killed if hit by X amount of bullets shot by a player.~~
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
* ~~The sector consist of randomly spawned enemies with some random variance of properties, e.g. damage and speed.~~
* ~~The sector also consist of randomly spawned objects, e.g. pillars.~~
* The further away from the player's starting point the harder the sectors become.
* When all the enemies in the sector is killed the doors will open to the previous and next sector.

##### Game World
* The game world spawns random sectors in a single line throughout the Z-axis.
* The player ~~is spawned in a waiting room at (0,0,0) and~~ will enter the combat mode when walking into the first sector.
* ~~The sectors has no height differences.~~
