﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Fase1_MapaAlturas
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Map mapa;

        Vector3 cameraDirection = Vector3.Forward;
        Vector3 cameraPosition = Vector3.Up*10;

        private Point lastMousePosition;
        private Vector2 _mouseSensitivity = new Vector2(.01f, .005f);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            mapa = new Map(GraphicsDevice, Content);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var mouseDelta = (Mouse.GetState().Position - lastMousePosition).ToVector2() * _mouseSensitivity;
            var cameraRight = Vector3.Cross(cameraDirection, Vector3.Up);

            cameraDirection = Vector3.Transform(cameraDirection, Matrix.CreateFromAxisAngle(Vector3.Up, -mouseDelta.X));
            cameraDirection = Vector3.Transform(cameraDirection, Matrix.CreateFromAxisAngle(cameraRight, -mouseDelta.Y));
            cameraPosition += ((Keyboard.GetState().IsKeyDown(Keys.Right) ? 1 : 0) -
                               (Keyboard.GetState().IsKeyDown(Keys.Left) ? 1 : 0)) * cameraRight;
            cameraPosition += ((Keyboard.GetState().IsKeyDown(Keys.Up) ? 1 : 0) -
                               (Keyboard.GetState().IsKeyDown(Keys.Down) ? 1 : 0)) * cameraDirection;

            // TODO: Add your update logic here

            lastMousePosition = Mouse.GetState().Position;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            mapa.Draw(GraphicsDevice, Matrix.CreateLookAt(cameraPosition,cameraPosition+cameraDirection,Vector3.Up));
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
