using System;
using Microsoft.Xna.Framework;
using Engine;


class FastItem : SpriteGameObject
    {
    Level level;
    Player player;
    protected float bounce;
    Vector2 startPosition;
    private double elapsedTime = 0;

    public FastItem(Level level, Vector2 startPosition) : base("Sprites/LevelObjects/fastitem", TickTick.Depth_LevelObjects)
    {
        this.level = level;
        this.startPosition = startPosition;

        SetOriginToCenter();

        Reset();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        double t = gameTime.TotalGameTime.TotalSeconds * 3.0f + LocalPosition.X;
        bounce = (float)Math.Sin(t) * 0.2f;
        localPosition.Y += bounce;

        // check if the player collects this item
        if (Visible && level.Player.CanCollideWithObjects && HasPixelPreciseCollision(level.Player))
        {
            Visible = false;
            ExtendedGame.AssetManager.PlaySoundEffect("Sounds/snd_itemcollected");

            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsedTime < 500)
                player.walkingSpeed = 600;
            else if (elapsedTime > 500)
                player.walkingSpeed = 400;
        }

    }

    public override void Reset()
    {
        localPosition = startPosition;
        Visible = true;
    }
}
