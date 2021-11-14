using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Engine
{
    public abstract class ExtendedGame : Game
    {
        // standard MonoGame objects for graphics and sprites
        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;

        // object for handling keyboard and mouse input
        protected InputHelper inputHelper;

        /// <summary>
        /// The width and height of the game world, in game units.
        /// </summary>
        public Point WorldSize { get; set; }

        /// <summary>
        /// The width and height of the window, in pixels.
        /// </summary>
        protected Point windowSize;

        private Camera camera;
        public Camera Camera
        {
            get
            {
                if (camera == null)
                {
                    return new Camera(WorldSize, Rectangle.Empty, GraphicsDevice);
                }
                return camera;
            }
            set { camera = value; }
        }

        public static bool Paused { get; set; } = false;

        /// <summary>
        /// An object for generating random numbers throughout the game.
        /// </summary>
        public static Random Random { get; private set; }

        /// <summary>
        /// An object for loading assets throughout the game.
        /// </summary>
        public static AssetManager AssetManager { get; private set; }

        /// <summary>
        /// The object that manages all game states, one of which is the active state.
        /// </summary>
        public static GameStateManager GameStateManager { get; private set; }

        public static string ContentRootDirectory { get { return "Content"; } }

        /// <summary>
        /// Creates a new ExtendedGame object.
        /// </summary>
        protected ExtendedGame()
        {
            // MonoGame preparations
            Content.RootDirectory = ContentRootDirectory;
            graphics = new GraphicsDeviceManager(this);

            // create the input helper and random number generator
            inputHelper = new InputHelper(this);
            Random = new Random();

            // default window and world size
            WorldSize = new Point(1440, 825);
            windowSize = new Point(1024, 586);
        }

        /// <summary>
        /// Does the initialization tasks that involve assets, and then prepares the game world.
        /// Override this method to do your own specific things when your game starts.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // store a static reference to the AssetManager
            AssetManager = new AssetManager(Content);

            // prepare an empty game state manager
            GameStateManager = new GameStateManager();

            // by default, we're not running in full-screen mode
            FullScreen = false;
        }

        /// <summary>
        /// Updates all objects in the game world, by first calling HandleInput and then Update.
        /// </summary>
        /// <param name="gameTime">An object containing information about the time that has passed.</param>
        protected override void Update(GameTime gameTime)
        {
            HandleInput();
            TimedActionManager.Update(gameTime);
            Camera.Update(gameTime);
            FullScreen = FullScreen;
            if (!Paused) GameStateManager.Update(gameTime);
        }

        /// <summary>
        /// Performs basic input handling and then calls HandleInput for the entire game world.
        /// </summary>
        protected void HandleInput()
        {
            inputHelper.Update();

            // quit the game when the player presses ESC
            if (inputHelper.KeyPressed(Keys.Escape))
                Exit();

            // toggle full-screen mode when the player presses F5
            if (inputHelper.KeyPressed(Keys.F5))
                FullScreen = !FullScreen;

            GameStateManager.HandleInput(inputHelper);
        }

        /// <summary>
        /// Draws the game world.
        /// </summary>
        /// <param name="gameTime">An object containing information about the time that has passed.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // start drawing sprites, applying the scaling and translation matrix
            Matrix transform = Camera.TranslationMatrix * Camera.ScalingMatrix;
            spriteBatch.Begin(SpriteSortMode.FrontToBack, null, null, null, null, null, transform);
            spriteBatch.Name = "Default";
            // let the game world draw itself
            GameStateManager.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, null, null, null, null, null, Matrix.CreateScale(
                    (float)GraphicsDevice.Viewport.Width / Camera.CameraViewPortSize.X,
                    (float)GraphicsDevice.Viewport.Height / Camera.CameraViewPortSize.Y, 1
                    ));
            spriteBatch.Name = "UI";
            // let the UI draw itself
            GameStateManager.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

        /// <summary>
        /// Scales the window to the desired size, and calculates how the game world should be scaled to fit inside that window.
        /// </summary>
        void ApplyResolutionSettings(bool fullScreen)
        {
            // make the game full-screen or not
            graphics.IsFullScreen = fullScreen;

            // get the size of the screen to use: either the window size or the full screen size
            Point screenSize;
            if (fullScreen)
                screenSize = new Point(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            else
                screenSize = windowSize;

            // scale the window to the desired size
            graphics.PreferredBackBufferWidth = screenSize.X;
            graphics.PreferredBackBufferHeight = screenSize.Y;

            graphics.ApplyChanges();

            // calculate and set the viewport to use
            GraphicsDevice.Viewport = CalculateViewport(screenSize);

        }

        /// <summary>
        /// Calculates and returns the viewport to use, so that the game world fits on the screen while preserving its aspect ratio.
        /// </summary>
        /// <param name="windowSize">The size of the screen on which the world should be drawn.</param>
        /// <returns>A Viewport object that will show the game world as large as possible while preserving its aspect ratio.</returns>
        Viewport CalculateViewport(Point windowSize)
        {
            // create a Viewport object
            Viewport viewport = new Viewport();

            // calculate the two aspect ratios
            float gameAspectRatio = (float)Camera.CameraViewPortSize.X / Camera.CameraViewPortSize.Y;
            float windowAspectRatio = (float)windowSize.X / windowSize.Y;

            // if the window is relatively wide, use the full window height
            if (windowAspectRatio > gameAspectRatio)
            {
                viewport.Width = (int)(windowSize.Y * gameAspectRatio);
                viewport.Height = windowSize.Y;
            }
            // if the window is relatively high, use the full window width
            else
            {
                viewport.Width = windowSize.X;
                viewport.Height = (int)(windowSize.X / gameAspectRatio);
            }

            // calculate and store the top-left corner of the viewport
            viewport.X = (windowSize.X - viewport.Width) / 2;
            viewport.Y = (windowSize.Y - viewport.Height) / 2;

            return viewport;
        }

        /// <summary>
        /// Gets or sets whether the game is running in full-screen mode.
        /// </summary>
        public bool FullScreen
        {
            get { return graphics.IsFullScreen; }
            set { ApplyResolutionSettings(value); }
        }

        /// <summary>
        /// Converts a position in screen coordinates to a position in world coordinates.
        /// </summary>
        /// <param name="screenPosition">A position in screen coordinates.</param>
        /// <returns>The corresponding position in world coordinates.</returns>
        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            Vector2 viewportTopLeft = new Vector2(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y);
            float screenToWorldScale = Camera.CameraViewPortSize.X / (float)GraphicsDevice.Viewport.Width;
            return (screenPosition - viewportTopLeft) * screenToWorldScale + Camera.GlobalPosition;
        }
    }
}