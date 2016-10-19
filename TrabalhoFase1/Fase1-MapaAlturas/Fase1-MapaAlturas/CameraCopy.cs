using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Fase1_MapaAlturas
{
    class CameraCopy
    {
       
        Matrix viewMatrix; // view da camera
        Matrix projectionMatrix; // projeção
        Matrix worldMatrix;
        Vector3 position;
        Vector3 direction;
        Vector3 movement;
        Vector3 rotation;

        private Point lastMousePosition;
        private Vector2 _mouseSensitivity = new Vector2(.01f, .005f);

        public CameraCopy(Vector3 position, Vector3 direction, Vector3 movement, Vector3 worldPosition)
        {
            this.position = position;
            this.direction = direction;
            this.movement = movement;
            rotation = movement * .05f;

            viewMatrix = Matrix.CreateLookAt(position, direction, Vector3.Up);
            projectionMatrix = Matrix.CreatePerspective(1.2f, 0.9f, 1.0f, 1000.0f);
            worldMatrix = Matrix.CreateTranslation(worldPosition);
        }

        public void Update()
        {
            var mouseDelta = (Mouse.GetState().Position - lastMousePosition).ToVector2() * _mouseSensitivity;
            Vector3 cameraRight = Vector3.Cross(direction, Vector3.Up);// O cross dos dois vetores devolve o vetor direção para a qual a camera deve se mover

            direction = Vector3.Transform(direction, Matrix.CreateFromAxisAngle(Vector3.Up, -mouseDelta.X));
            direction = Vector3.Transform(direction, Matrix.CreateFromAxisAngle(cameraRight, -mouseDelta.Y));                       

            //movimentação da camera 
            position += ((Keyboard.GetState().IsKeyDown(Keys.NumPad4) ? 1 : 0) -
                         (Keyboard.GetState().IsKeyDown(Keys.NumPad6) ? 1 : 0)) * cameraRight;

            position += ((Keyboard.GetState().IsKeyDown(Keys.NumPad8) ? 1 : 0) -
                         (Keyboard.GetState().IsKeyDown(Keys.NumPad5) ? 1 : 0)) * direction;
            
            lastMousePosition = Mouse.GetState().Position;
            
        }
                

        public void Draw(BasicEffect basicEffect, Map world)
        {
            basicEffect.View = viewMatrix;
            basicEffect.Projection = projectionMatrix;
            basicEffect.World = worldMatrix;


        }


    }
}
