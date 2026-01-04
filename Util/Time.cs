using Microsoft.Xna.Framework;

namespace UI.Util
{
    public static class Time
    {
        public static float DeltaTime { get; private set; }
        public static float FPS { get; private set; }
        public static GameTime Current { get; private set; }

        public static void Update(GameTime gameTime)
        {
            Current = gameTime;
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            FPS = DeltaTime > 0f ? 1f / DeltaTime : 0f;
        }

    }
}
