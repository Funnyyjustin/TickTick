using System;
using Microsoft.Xna.Framework;
using Engine;


class FastItem : SpriteGameObject
    {
    Level level;
    protected float bounce;
    Vector2 startPosition;
    private float elapsedTime = 0;
    private float duration = 5000f;

    public FastItem(Level level, Vector2 startPosition) : base("Sprites/LevelObjects/fastitem", TickTick.Depth_LevelObjects)
    {
        this.level = level;
        this.startPosition = startPosition;

        SetOriginToCenter();

        Reset();
    }

    // player walks twice as fast for five seconds
    public bool pickUp
    {
        get { return pickUp = true; }
        set
        {
            level.Player.walkingSpeed = level.Player.normalWalkingSpeed * 2;
            if (elapsedTime > duration)
                pickUp = false;
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        double t = gameTime.TotalGameTime.TotalSeconds * 3.0f + LocalPosition.X;
        bounce = (float)Math.Sin(t) * 0.2f;
        localPosition.Y += bounce;
        elapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

        // check if the player collects this item
        if (Visible && level.Player.CanCollideWithObjects && HasPixelPreciseCollision(level.Player))
        {
            elapsedTime = 0;
            Visible = false;
            ExtendedGame.AssetManager.PlaySoundEffect("Sounds/snd_watercollected");
            pickUp = true;
        }
    }

    public override void Reset()
    {
        localPosition = startPosition;
        Visible = true;
    }
}
