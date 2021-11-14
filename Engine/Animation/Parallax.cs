using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class Parallax : GameObject
    {
        Dictionary<SpriteGameObject, float> depthDictionary = new Dictionary<SpriteGameObject, float>();
        GameObject perspectiveObject;

        public Parallax(GameObject perspectiveObject)
        {
            this.perspectiveObject = perspectiveObject;
        }

        public override void Update(GameTime gameTime){
            foreach (KeyValuePair<SpriteGameObject, float> keyValuePair in depthDictionary)
            {
                Vector2 velocity = perspectiveObject.velocity * keyValuePair.Value;
                keyValuePair.Key.velocity = new Vector2(velocity.X, 0);
                keyValuePair.Key.Update(gameTime);
            }
        }

        public void AddObject(SpriteGameObject spriteGameObject, float depth)
        {
            depthDictionary.Add(spriteGameObject, depth);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach(KeyValuePair<SpriteGameObject, float> keyValuePair in depthDictionary)
            {
                keyValuePair.Key.Draw(gameTime, spriteBatch);
            }
        }
    }
}
