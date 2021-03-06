using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

/// <summary>
/// Represents a rocket enemy that flies horizontally through the screen.
/// </summary>
class Rocket : AnimatedGameObject
{
    Level level;
    Vector2 startPosition;
    const float speed = 500;
    bool collision;

    public Rocket(Level level, Vector2 startPosition, bool facingLeft) 
        : base(TickTick.Depth_LevelObjects)
    {
        this.level = level;

        LoadAnimation("Sprites/LevelObjects/Rocket/spr_rocket@3", "rocket", true, 0.1f);
        LoadAnimation("Sprites/LevelObjects/Player/spr_die@5", "die", true, 0.1f);
        LoadAnimation("Sprites/LevelObjects/Player/spr_explode@5x5", "explode", false, 0.04f);

        if (IsActive)
        {
            Reset();
            sprite.Mirror = facingLeft;
            if (sprite.Mirror)
            {
                velocity.X = -speed;
                this.startPosition = startPosition + new Vector2(2 * speed, 0);
            }
            else
            {
                velocity.X = speed;
                this.startPosition = startPosition - new Vector2(2 * speed, 0);
            }
        }
    }

    public override void Reset()
    {
        // go back to the starting position
        LocalPosition = startPosition;
        IsActive = true;
        collision = true;
        PlayAnimation("rocket");
        SetOriginToCenter();
    }

    //Create a bounding box that checks collision with player, to check whether the player is stepping on the rocket
    Rectangle BboxForCollisions
    {
        get
        {
            Rectangle bboxRocket = BoundingBox;
            bboxRocket.X += 1;
            bboxRocket.Width -= 2;
            bboxRocket.Height = 1;
            return bboxRocket;
        }
    }

    public bool IsActive { get; private set; } = true;

    public void RocketDie()
    {
        // rocket gets removed from the game
        IsActive = false;
        collision = false;
        PlayAnimation("explode");
        ExtendedGame.AssetManager.PlaySoundEffect("Sounds/snd_player_explode");
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        // if the rocket has left the screen, reset it
        if (sprite.Mirror && BoundingBox.Right < level.BoundingBox.Left)
            Reset();
        else if (!sprite.Mirror && BoundingBox.Left > level.BoundingBox.Right)
            Reset();

        // if the rocket touches the player, the player dies
        if (collision)
        {
            if (level.Player.CanCollideWithObjects && HasPixelPreciseCollision(level.Player))
                level.Player.Die();

            // if the player's boundingbox intersects with the rocket's boundingbox, the rocket dies
            else if (level.Player.CanCollideWithObjects && CollisionDetection.ShapesIntersect(BboxForCollisions, level.Player.BoundingBoxForCollisions))
                RocketDie();
        }
    }
}