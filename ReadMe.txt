Justin Timmer (1126458) en Jens Steenmetz (6403948)

CHANGES
-----------------------------------------------------------------------------
- Added Extensions.cs
- Sidescrolling
	- Created a Camera class.
	- Created a FollowingCamera class.
	- Added LoadCamera and GetLevelSize methods to the Level Class (in LevelLoading.cs)
	- Overwritten the GlobalPosition property in the Level Class, so it returns the negated position of the Camera.

- Jumping onto rockets
	- Added a method that gives the Rocket object a BoundingBox.
	- Added a method that makes the Rocket 'explode'.
	- Extended the Update method, so it now checks whether the Rocket BB collides with the Player BB.
	- When a player jumps onto the rocket, the rocket will now explode and not respawn until the player has completed the level.


- Speed Behavior
	- Slow and Fast tiles
		- Created slow and fast tiles and platforms (added to the Tile and Player classes respectively).
		- When the player walks on slow tiles, they move at 0.5x normal speed, and when they walk on fast tiles, they move at 2x normal speed.
	- Pickup items
		- Created an Item class, which is similar to the Waterdrop class.
		- When the player picks up an item, they either move at 0.5x OR 2x normal speed, for a duration of 5 seconds.


TODO
-----------------------------------------------------------------------------
- Sidescrolling
	- 26.6.1b
	- 26.6.1c (excl. outside game world)
	- 26.6.1d (?)
	- 26.6.1e
	- 26.6.1f

- Speed Behavior (26.6.2b)
	- Sprites
		- Fast tiles and items (Tile class, surfaceExtension = _fast AND FastItem class)
		- Slow tiles and items (Tile class, surfaceExtension = _slow AND SlowItem class)
		(NOTE): added placeholder sprites for now