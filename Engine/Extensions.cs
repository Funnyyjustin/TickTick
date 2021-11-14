using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

public static class Extensions
{
    //normalize a vector2 while accounting for divisibility by zero.
    public static Vector2 Normalized(this Vector2 vector2)
    {
        Vector2 newVector2 = vector2;
        if (newVector2 != Vector2.Zero)
        {
            newVector2.Normalize();
        }
        return newVector2;
    }

    //takes the absolute value of a vector2
    public static Vector2 Abs(this Vector2 vector2)
    {
        return new Vector2(Math.Abs(vector2.X), Math.Abs(vector2.Y));
    }

    //return the signed version of vector2, but without it being able to become 0 (so it's either -1 or 1, depending on what it was before).
    public static Vector2 SignedWithoutZero(this Vector2 vector2)
    {
        Vector2 newVector2 = vector2;
        if (newVector2.X == 0)
        {
            newVector2 = new Vector2(1, newVector2.Y);
        }
        if (newVector2.Y == 0)
        {
            newVector2 = new Vector2(newVector2.X, 1);
        }
        return newVector2 / newVector2.Abs();
    }

    //return the signed version of vector2, but without it being able to become 0 (so it's either -1 or 1, depending on what it was before).
    public static Vector2 Signed(this Vector2 vector2)
    {
        Vector2 newVector2 = vector2;
        Vector2 zeroes = Vector2.One;
        if (newVector2.X == 0)
        {
            newVector2 = new Vector2(1, newVector2.Y);
            zeroes *= new Vector2(0, 1);
        }
        if (newVector2.Y == 0)
        {
            newVector2 = new Vector2(newVector2.X, 1);
            zeroes *= new Vector2(1, 0);
        }
        return (newVector2 / newVector2.Abs()) * zeroes;
    }

    //Check if the player started to press a key this frame.
    public static bool IsKeyDownFrame(this KeyboardState keyboardState, KeyboardState previousKeyboardState, Keys key)
    {
        return keyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key);
    }

    //Check if the player stopped to press a key this frame.
    public static bool IsKeyUpFrame(this KeyboardState keyboardState, KeyboardState previousKeyboardState, Keys key)
    {
        return keyboardState.IsKeyUp(key) && previousKeyboardState.IsKeyDown(key);
    }

    //Compares two vector2's, to see if a is greater than b.
    public static bool Vector2GreaterThan(this Vector2 a, Vector2 b)
    {
        if (a.X > b.X && a.Y > b.Y)
        {
            return true;
        }
        return false;
    }

    //lerps from float a to float b by x = 0 to 1
    public static float Lerp(float a, float b, float x)
    {
        return a * (1 - x) + b * x;
    }

    //lerps from vector a to vector b by x = 0 to 1
    public static Vector2 Lerp(Vector2 a, Vector2 b, float x)
    {
        return a * (1 - x) + b * x;
    }

    public static Vector2 PositionClampedToRectangle(Vector2 position, Rectangle rectangle)
    {
        return new Vector2(
            Math.Clamp(position.X, rectangle.Left, rectangle.Right),
            Math.Clamp(position.Y, rectangle.Top, rectangle.Bottom)
            );
    }
}
