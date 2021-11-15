using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;

partial class Level : GameObjectList
{
    void LoadLevelFromFile(string filename)
    {
        // open the file
        StreamReader reader = new StreamReader(filename);

        // read the description
        string description = reader.ReadLine();

        //read the time the player has to reach the goal and add the timer
        int levelTime = int.Parse(reader.ReadLine());
        timer = new BombTimer(levelTime);
        AddChild(timer);

        // read the rows of the grid; keep track of the longest row
        int gridWidth = 0;

        List<string> gridRows = new List<string>();
        string line = reader.ReadLine();
        while (line != null)
        {
            if (line.Length > gridWidth)
                gridWidth = line.Length;

            gridRows.Add(line);
            line = reader.ReadLine();
        }

        // stop reading the file
        reader.Close();

        // create all game objects for the grid
        AddPlayingField(gridRows, gridWidth, gridRows.Count);

        // add game objects to show that general level info
        AddLevelInfoObjects(description);
    }

    void AddLevelInfoObjects(string description)
    {
        // - background box
        SpriteGameObject frame = new SpriteGameObject("Sprites/UI/spr_frame_hint", TickTick.Depth_UIBackground, "UI");
        frame.SetOriginToCenter();
        frame.LocalPosition = new Vector2(720, 50);
        AddChild(frame);

        // - text
        TextGameObject hintText = new TextGameObject("Fonts/HintFont", TickTick.Depth_UIForeground, Color.Black, "UI", TextGameObject.Alignment.Left);
        hintText.Text = description;
        hintText.LocalPosition = new Vector2(510, 40);
        AddChild(hintText);
    }

    void AddPlayingField(List<string> gridRows, int gridWidth, int gridHeight)
    {
        // create a parent object for everything
        GameObjectList playingField = new GameObjectList();

        // initialize the list of water drops and items
        waterDrops = new List<WaterDrop>();
        fastItems = new List<FastItem>();
        slowItems = new List<SlowItem>();

        // prepare the grid arrays
        tiles = new Tile[gridWidth, gridHeight];

        // load the tiles
        for (int y = 0; y < gridHeight; y++)
        {
            string row = gridRows[y];
            for (int x = 0; x < gridWidth; x++)
            {
                // the row could be too short; if so, pretend there is an empty tile
                char symbol = '.';
                if (x < row.Length)
                    symbol = row[x];

                // load the tile
                AddTile(x, y, symbol);
            }
        }
    }

    void AddTile(int x, int y, char symbol)
    {
        // load the static part of the tile
        Tile tile = CharToStaticTile(symbol);
        tile.LocalPosition = GetCellPosition(x, y);
        AddChild(tile);

        // store a reference to that tile in the grid
        tiles[x, y] = tile;

        // load the dynamic part of the tile
        if (symbol == '1')
            LoadCharacter(x, y);
        else if (symbol == 'X')
            LoadGoal(x, y);
        else if (symbol == 'W')
            LoadWaterDrop(x, y);
        else if (symbol == 'R')
            LoadRocketEnemy(x, y);
        else if (symbol == 'T')
            LoadTurtleEnemy(x, y);
        else if (symbol == 'S')
            LoadSparkyEnemy(x, y);
        else if (symbol == 'A' || symbol == 'B' || symbol == 'C')
            LoadFlameEnemy(x, y, symbol);
        else if (symbol == 'F')
            LoadFastItem(x, y);
        else if (symbol == 'E')
            LoadSlowItem(x, y);
    }

    Tile CharToStaticTile(char symbol)
    {
        switch (symbol)
        {
            case '-':
                return new Tile(Tile.Type.Platform, Tile.SurfaceType.Normal);
            case '#':
                return new Tile(Tile.Type.Wall, Tile.SurfaceType.Normal);
            case 'h':
                return new Tile(Tile.Type.Platform, Tile.SurfaceType.Hot);
            case 'H':
                return new Tile(Tile.Type.Wall, Tile.SurfaceType.Hot);
            case 'i':
                return new Tile(Tile.Type.Platform, Tile.SurfaceType.Ice);
            case 'I':
                return new Tile(Tile.Type.Wall, Tile.SurfaceType.Ice);
            case 'l':
                return new Tile(Tile.Type.Platform, Tile.SurfaceType.Slow);
            case 'L':
                return new Tile(Tile.Type.Wall, Tile.SurfaceType.Slow);
            case 'v':
                return new Tile(Tile.Type.Platform, Tile.SurfaceType.Fast);
            case 'V':
                return new Tile(Tile.Type.Wall, Tile.SurfaceType.Fast);
            default:
                return new Tile(Tile.Type.Empty, Tile.SurfaceType.Normal);
        }
    }

    void LoadCharacter(int x, int y)
    {
        // create the bomb character
        Player = new Player(this, GetCellBottomCenter(x, y));
        AddChild(Player);
    }

    void LoadGoal(int x, int y)
    {
        // create the exit object
        goal = new SpriteGameObject("Sprites/LevelObjects/spr_goal", TickTick.Depth_LevelObjects);
        // make sure it's standing exactly on the tile below
        goal.LocalPosition = GetCellPosition(x, y + 1);
        goal.Origin = new Vector2(0, goal.Height);
        AddChild(goal);
    }

    void LoadWaterDrop(int x, int y)
    {
        // create the water drop object;  place it around the center of the tile
        Vector2 pos = GetCellPosition(x, y) + new Vector2(TileWidth / 2, TileHeight / 3);
        WaterDrop w = new WaterDrop(this, pos);
        // add it to the game world
        AddChild(w);
        // store an extra reference to it
        waterDrops.Add(w);
    }

    void LoadRocketEnemy(int x, int y)
    {
        Rocket r = new Rocket(this, GetCellPosition(x, y), x != 0);
        AddChild(r);
    }

    void LoadTurtleEnemy(int x, int y)
    {
        Turtle t = new Turtle(this);
        t.LocalPosition = GetCellBottomCenter(x, y);
        AddChild(t);
    }

    void LoadSparkyEnemy(int x, int y)
    {
        Sparky s = new Sparky(this, GetCellBottomCenter(x, y));
        AddChild(s);
    }

    void LoadFlameEnemy(int x, int y, char symbol)
    {
        Vector2 pos = GetCellBottomCenter(x, y);

        PatrollingEnemy enemy;
        if (symbol == 'A')
            enemy = new PatrollingEnemy(this, pos);
        else if (symbol == 'B')
            enemy = new PlayerFollowingEnemy(this, pos);
        else
            enemy = new UnpredictableEnemy(this, pos);

        AddChild(enemy);
    }
    void LoadFastItem(int x, int y)
    {
        // create the item object;  place it around the center of the tile
        Vector2 pos = GetCellPosition(x, y) + new Vector2(TileWidth / 2, TileHeight / 3);
        FastItem f = new FastItem(this, pos);
        // add it to the game world
        AddChild(f);
        // store an extra reference to it
        fastItems.Add(f);
    }
    void LoadSlowItem(int x, int y)
    {
        // create the item object;  place it around the center of the tile
        Vector2 pos = GetCellPosition(x, y) + new Vector2(TileWidth / 2, TileHeight / 3);
        SlowItem s = new SlowItem(this, pos);
        // add it to the game world
        AddChild(s);
        // store an extra reference to it
        slowItems.Add(s);
    }

    void LoadCamera()
    {
        // initializes a camera that can move around the level, and start animating it, so it shows the level.
        Point cameraViewPort = new Point(1440, 825);
        Point cameraLimitsSize = BoundingBox.Size - cameraViewPort;
        Rectangle cameraLimits = new Rectangle(Point.Zero, cameraLimitsSize);
        TickTick.Game.Camera = new Camera(cameraViewPort, cameraLimits, TickTick.Game.GraphicsDevice, Player, false, 1.2F);
        ShowLevelCameraAnimation();
    }

    void ShowLevelCameraAnimation()
    {
        //Pause the game, because we dont want the game to start while we are still showing the level.
        ExtendedGame.Paused = true;

        Camera camera = TickTick.Game.Camera;

        //calculate the startingposition, targetposition and zoom for the camera, depending on how big the level is
        //(the camera starts zoomed out as much as possible, so it does not show anything outside of the level.
        //In other words, the camera zooms out until one of the camera viewport edges hit the edges of the level).
        float zoomWidth = 1.0F * camera.CameraViewPortSize.X / BoundingBox.Width;
        float zoomHeight = 1.0F * camera.CameraViewPortSize.Y / BoundingBox.Height;
        Vector2 startingPosition, targetPosition;
        if (zoomWidth < zoomHeight)
        {
            camera.Zoom = zoomHeight;
            startingPosition = new Vector2(BoundingBox.Size.X - camera.CameraViewPortSize.X / zoomHeight, 0);
            targetPosition = Vector2.Zero;
        }
        else
        {
            camera.Zoom = zoomWidth;
            startingPosition = Vector2.Zero;
            targetPosition = new Vector2(0, BoundingBox.Size.Y - camera.CameraViewPortSize.Y / zoomHeight);
        }

        //calculate the scalar for the duration of the animations, so the speed at which the camera moves is independent from the level size and level ratio.
        float durationScale1 = Vector2.Distance(startingPosition, targetPosition);
        float durationScale2 = MathF.Abs((BoundingBox.X * BoundingBox.Y) / (camera.CameraViewPortSize.X * camera.CameraViewPortSize.Y) - 1);

        //Scroll through the level, and then zoom in to the player.
        camera.Animation.AddZoomAnimation(0.005F * durationScale1, camera.Zoom, camera.Zoom);
        camera.Animation.AddZoomAnimation(0.5F * durationScale2, camera.Zoom, 1);
        Vector2 playerPositionClamped = Extensions.PositionClampedToRectangle(camera.CenteredCameraPosition(Player.GlobalPosition), camera.CameraLimits);
        camera.Animation.AddMoveAnimation(0.005F * durationScale1, startingPosition, targetPosition);
        camera.Animation.AddMoveAnimation(0.5F * durationScale2, targetPosition, playerPositionClamped);

        //After the animation is done, start the game and make the camera follow the player.
        new TimedAction(0.005F * durationScale1 + 0.5F * durationScale2, () => { ExtendedGame.Paused = false; camera.IsFollowing = true; });
    }

    Vector2 GetCellBottomCenter(int x, int y)
    {
        return GetCellPosition(x, y + 1) + new Vector2(TileWidth / 2, 0);
    }
}