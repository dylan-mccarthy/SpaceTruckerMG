using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceTrucker.Shared.UI;
using System.Collections.Generic;

namespace SpaceTrucker.Client;

public class GameClient : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private List<IElement> _UIElements;

    public GameClient()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _UIElements = new List<IElement>();
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        var panel = new Panel(new Rectangle(0, 0, 100, 100), this);
        //Create solid white texture to fill panel
        var solidTexture = new Texture2D(GraphicsDevice, 1, 1);
        solidTexture.SetData(new Color[] { Color.White });
        panel.SetTexture(solidTexture);
        panel.HasBorder = true;
        panel.BorderSize = 2;
        panel.BorderColor = Color.Black;

        var solidGreenTexture = new Texture2D(GraphicsDevice, 1, 1);
        solidGreenTexture.SetData(new Color[] { Color.Green });
        var panelHeader = new PanelHeader(new Rectangle(0, 0, 100, 20), this);
        panelHeader.SetTexture(solidGreenTexture);
        panelHeader.HeaderSize = 20;
        panelHeader.Parent = panel;
        panelHeader.HasBorder = true;
        panelHeader.BorderSize = 2;
        panelHeader.BorderColor = Color.Black;
        panelHeader.HeaderText = "Test Header";
        panelHeader.HeaderTextColor = Color.Black;

        panel.AddElement(panelHeader);
        _UIElements.Add(panel);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        _UIElements.ForEach(e => e.Update(gameTime));

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _UIElements.ForEach(e => e.Draw(_spriteBatch));

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
