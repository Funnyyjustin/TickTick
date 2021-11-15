using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class Parallax : GameObject
    {
        //a dictionary storing all parallax objects and their depths.
        Dictionary<SpriteGameObject, float> depthDictionary = new Dictionary<SpriteGameObject, float>();

        //the object from which the parallax should be calculated
        GameObject perspectiveObject;

        public Parallax(GameObject perspectiveObject)
        {
            this.perspectiveObject = perspectiveObject;
        }

        public override void Update(GameTime gameTime){
            //When the perspectiveObject moves, all parallax objects move a fraction of that amount, depending on the depth
            foreach (KeyValuePair<SpriteGameObject, float> keyValuePair in depthDictionary)
            {
                Vector2 velocity = perspectiveObject.velocity * keyValuePair.Value;
                keyValuePair.Key.velocity = new Vector2(velocity.X, 0);
                keyValuePair.Key.Update(gameTime);
            }
        }

        //Add a parallax object
        public void AddObject(SpriteGameObject spriteGameObject, float depth)
        {
            depthDictionary.Add(spriteGameObject, depth);
        }

        //draw all parallax objects
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach(KeyValuePair<SpriteGameObject, float> keyValuePair in depthDictionary)
            {
                keyValuePair.Key.Draw(gameTime, spriteBatch);
            }
        }
    }
}
