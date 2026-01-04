using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace UI.Util
{
    public static class AssetLoader
    {
        private static ContentManager _content;
        private static Texture2D _pixel;
        private static readonly Dictionary<string, Texture2D> _textures = new();
        private static readonly Dictionary<string, SpriteFont> _fonts = new();
        public static SpriteFont DefaultFont { get; private set; }
        public static readonly RasterizerState RasterizerState = new RasterizerState
                                                         {
                                                             ScissorTestEnable = true
                                                         };
        private static GraphicsDevice _graphicsDevice;





        /// <summary>
        /// Initialize the AssetLoader with content manager, graphics device, and default font
        /// </summary>
        public static void Init(ContentManager content, GraphicsDevice graphicsDevice, string defaultFontName)
        {
            _content = content;

            _pixel = new Texture2D(graphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            DefaultFont = GetFont(defaultFontName);

            _graphicsDevice = graphicsDevice;
        }

        /// <summary>
        /// Returns a 1x1 Texture2D
        /// </summary>
        /// <returns>Texture2D</returns>
        public static Texture2D GetPixel() => _pixel;

        /// <summary>
        /// Returns a Texture2D from Game.Content
        /// </summary>
        /// <param name="name">Name of the Texture</param>
        /// <returns>Texture2D</returns>
        public static Texture2D GetTexture(string name)
        {
            if (!_textures.ContainsKey(name))
                _textures[name] = _content.Load<Texture2D>(name);

            return _textures[name];
        }

        /// <summary>
        /// Returns a SpriteFont from Game.Content
        /// </summary>
        /// <param name="name">Name of the Spritefont</param>
        /// <returns>SpriteFont</returns>
        public static SpriteFont GetFont(string name)
        {
            if (!_fonts.ContainsKey(name))
                _fonts[name] = _content.Load<SpriteFont>(name);

            return _fonts[name];
        }

        public static Rectangle GetDefaultRect()
        {
            return new Rectangle(
                0,
                0,
                _graphicsDevice.Viewport.Width,
                _graphicsDevice.Viewport.Height
            );
        }
    }
}
