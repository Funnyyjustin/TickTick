Justin Timmer (1126458) en Jens Steenmetz (6403948)

CHANGES
-----------------------------------------------------------------------------
- Sidescrolling
	- Created a Camera class.
	- Added a Camera variable to the Level class.
	- Overwritten the GlobalPosition property in the Level Class, so it returns the negated position of the Camera.

- Speed Behavior
   - Slow and Fast tiles
	- Created slow and fast tiles and platforms (added to the Tile and Player classes respectively).
	- (NEEDS TESTING) When the player walks on slow tiles, they move at 0.5x normal speed, and when they walk on fast tiles, they move at 2x normal speed.
	- (TODO) Incorporated the slow and fast tiles into various levels (level 4 and higher).
   - Pickup items
	- Created a Item class, which is similar to the Waterdrop class.
	- (NEEDS TESTING) When the player picks up an item, they either move at 0.5x OR 2x normal speed, for a duration of 5 seconds.
	- (TODO) Placed pickups throughout various levels (level 4 and higher).





TO ADD
-----------------------------------------------------------------------------
- Sidescrolling
	- 26.6.1b
	- 26.6.1c (excl. outside game world)
	- 26.6.1d (?)
	- 26.6.1e
	- 26.6.1f
- Jumping onto rockets (26.6.2a)
- Speed Behavior (26.6.2b)
   - Sprites
	- Fast tiles and pickups (Tile class, surfaceExtension = _fast)
	- Slow tiles and pickups (Tile class, surfaceExtension = _slow)
   - Sound
	- ItemCollected (Item class)