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
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();

        AssetLoader.Init(Content, _graphics.GraphicsDevice, "font");

        base.Initialize();
    }




    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _desktop = new TestDesktop(this);
        _desktop.Load();
    }

    protected override void Update(GameTime gameTime)
    {
        Time.Update(gameTime);
        Input.Update();


        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        _desktop.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _desktop.Draw();

        _spriteBatch.Begin();

        _spriteBatch.DrawString(AssetLoader.DefaultFont, Input.MousePos.ToString(), Input.MousePos, Color.Black);

        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
}
