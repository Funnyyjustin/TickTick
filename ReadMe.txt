Justin Timmer (1126458) en Jens Steenmetz (6403948)

CHANGES
-----------------------------------------------------------------------------
- Sidescrolling
	- Created a Camera class.
	- Added a Camera variable to the Level class.
	- Overwritten the GlobalPosition property in the Level Class, so it returns the negated position of the Camera.

- Jumping onto rockets
	- Added a method that gives the Rocket object a BoundingBox.
	- Added a method that makes the Rocket 'explode'.
	- Extended the Update method, so it now checks whether the Rocket BB collides with the Player BB.
	- (NEEDS TESTING) When a player jumps onto the rocket, the rocket will now explode and reset.

- Speed Behavior
	- Slow and Fast tiles
		- Created slow and fast tiles and platforms (added to the Tile and Player classes respectively).
		- (NEEDS TESTING) When the player walks on slow tiles, they move at 0.5x normal speed, and when they walk on fast tiles, they move at 2x normal speed.
	- Pickup items
		- Created a Item class, which is similar to the Waterdrop class.
		- (NEEDS TESTING) When the player picks up an item, they either move at 0.5x OR 2x normal speed, for a duration of 5 seconds.

s
TODO
-----------------------------------------------------------------------------
- Sidescrolling
	- 26.6.1b
	- 26.6.1c (excl. outside game world)
	- 26.6.1d (?)
	- 26.6.1e
	- 26.6.1f

- Jumping onto rockets (26.6.2a)
	- Testing

- Speed Behavior (26.6.2b)
	- Sprites
		- Fast tiles (Tile class, surfaceExtension = _fast)
		- Slow tiles (Tile class, surfaceExtension = _slow)
		(NOTE): added placeholder sprites for the pickup items
	- Sound
		- ItemCollected (Item class)
	- Placing the tiles and items throughout various levels (level 4 and higher (level 1 to test playability))
	- Testing