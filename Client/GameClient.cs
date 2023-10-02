using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceTrucker.Client.Handlers;
using SpaceTrucker.Shared.UI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace SpaceTrucker.Client;

public class GameClient : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private readonly UIEventHandler UIEventHandler = new UIEventHandler();

    private readonly ConcurrentBag<IElement> _UIElements;

    public GameClient()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _UIElements = new ConcurrentBag<IElement>();
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        UIEventHandler.RegisterNewEvent("Test button clicked", (sender, EventArgs) => DisplayPopUp(EventArgs as ButtonClickedArgs));



        var panel = new Panel("MainPanel", new Rectangle(0, 0, 200, 200), this);
        //Create solid white texture to fill panel
        var solidTexture = new Texture2D(GraphicsDevice, 1, 1);
        solidTexture.SetData(new Color[] { Color.White });
        panel.SetTexture(solidTexture);
        panel.HasBorder = true;
        panel.BorderSize = 2;
        panel.BorderColor = Color.Black;

        var solidGreenTexture = new Texture2D(GraphicsDevice, 1, 1);
        solidGreenTexture.SetData(new Color[] { Color.Green });
        var panelHeader = new PanelHeader("MainPanelHeader",new Rectangle(0, 0, 100, 20), this);
        panelHeader.SetTexture(solidGreenTexture);
        panelHeader.HeaderSize = 20;
        panelHeader.Parent = panel;
        panelHeader.HasBorder = true;
        panelHeader.BorderSize = 2;
        panelHeader.BorderColor = Color.Black;
        panelHeader.HeaderText = "Test Header";
        panelHeader.HeaderTextColor = Color.Black;
        panel.AddElement(panelHeader);

        //Add Button to Center of Panel
        var button = new Button("Text Button 1","Test Button", panel);
        button.SetTexture(solidTexture);
        button.Bounds = new Rectangle((panel.Bounds.Width / 2) - 50, (panel.Bounds.Height / 2) - 10, 100, 20);
        button.HasBorder = true;
        button.BorderSize = 2;
        button.BorderColor = Color.Black;
        button.Parent = panel;
        button.ButtonClicked += (sender, EventArgs) => UIEventHandler.Connect("Test button clicked", sender, EventArgs);

        button.IsVisable = true;
        panel.AddElement(button);
        _UIElements.Add(panel);
        base.Initialize();
    }

    private void DisplayPopUp(ButtonClickedArgs buttonClickedArgs)
    {
        //Create Popup Panel in center of screen 
        var popupPanel = new Panel("PopupPanel",new Rectangle((GraphicsDevice.Viewport.Width / 2) - 100, (GraphicsDevice.Viewport.Height / 2) - 100, 200, 200), this);
        //Create solid white texture to fill panel
        var solidTexture = new Texture2D(GraphicsDevice, 1, 1);
        solidTexture.SetData(new Color[] { Color.White });
        popupPanel.SetTexture(solidTexture);
        popupPanel.HasBorder = true;
        popupPanel.BorderSize = 2;
        popupPanel.BorderColor = Color.Black;
        popupPanel.IsVisable = true;

        var solidGreenTexture = new Texture2D(GraphicsDevice, 1, 1);
        solidGreenTexture.SetData(new Color[] { Color.Green });
        var panelHeader = new PanelHeader("PopupPanelHeader",new Rectangle(0, 0, 100, 20), this);
        panelHeader.SetTexture(solidGreenTexture);
        panelHeader.HeaderSize = 20;
        panelHeader.Parent = popupPanel;
        panelHeader.HasBorder = true;
        panelHeader.BorderSize = 2;
        panelHeader.BorderColor = Color.Black;
        panelHeader.HeaderText = "Popup";
        panelHeader.HeaderTextColor = Color.Black;
        popupPanel.AddElement(panelHeader);

        //Draw Text to center of Panel
        var font = Content.Load<SpriteFont>("Fonts/Default");
        var textSize = font.MeasureString(buttonClickedArgs.Button.Text);
        var textPosition = new Vector2(popupPanel.Bounds.X + (popupPanel.Bounds.Width / 2) - (textSize.X / 2), popupPanel.Bounds.Y + (popupPanel.Bounds.Height / 2) - (textSize.Y / 2));
        var textElement = new TextElement("PopupPanelText",buttonClickedArgs.Button.Text, new Rectangle((int)textPosition.X, (int)textPosition.Y, (int)textSize.X, (int)textSize.Y), popupPanel, this, font, Color.Black);
        popupPanel.AddElement(textElement);

        _UIElements.Add(popupPanel);
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
        foreach (var element in _UIElements)
        {
            element.Update(gameTime);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        foreach(var element in _UIElements)
        {
            element.Draw(_spriteBatch);
        }

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
