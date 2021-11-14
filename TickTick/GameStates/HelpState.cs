using Engine;
using Engine.UI;
using Microsoft.Xna.Framework;

class HelpState : GameState
{
    Button backButton;

    public HelpState()
    {
        // add a background
        gameObjects.AddChild(new SpriteGameObject("Sprites/Backgrounds/spr_help", 1));

        // add a back button
        backButton = new Button("Sprites/UI/spr_button_back", 1);
        backButton.LocalPosition = new Vector2(1180, 750);
        gameObjects.AddChild(backButton);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (backButton.Pressed)
            ExtendedGame.GameStateManager.SwitchTo(ExtendedGameWithLevels.StateName_Title);
    }

    public override void Initialize()
    {
        // initializes a camera
        Camera camera = new Camera(new Point(1440, 825), Rectangle.Empty, TickTick.Game.GraphicsDevice);
        TickTick.Game.Camera = camera;
    }
}
