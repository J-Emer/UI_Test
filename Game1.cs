using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UI.Controls;
using UI.Desktops;
using UI.Util;

namespace UI;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private TestDesktop _desktop;



    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        int w = _graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
        int h = _graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
        Window.AllowUserResizing = true;

        _graphics.PreferredBackBufferWidth = w;
        _graphics.PreferredBackBufferHeight = h;
        _graphics.ApplyChanges();

        AssetLoader.Init(Content, _graphics.GraphicsDevice, "font");

        base.Initialize();
    }




    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _desktop = new TestDesktop(this, "font");
        _desktop.Load();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _desktop.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _desktop.Draw();

        _spriteBatch.Begin();

        _spriteBatch.DrawString(AssetLoader.DefaultFont, Input.MousePos.ToString(), Input.MousePos + new Vector2(10, 0), Color.Black);

        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
}
