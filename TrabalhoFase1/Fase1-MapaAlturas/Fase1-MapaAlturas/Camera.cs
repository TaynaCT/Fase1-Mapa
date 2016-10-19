using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase1_MapaAlturas
{
    class Camera
    {
        //vetor de direção da camera
        Vector3 cameraDirection = Vector3.Forward;
        Vector3 cameraPosition = Vector3.Up * 10;

        private Point lastMousePosition;
        private Vector2 _mouseSensitivity = new Vector2(.01f, .005f);

        public Camera()
        {

        }

        public void Update(GameTime gameTime)
        {
            var mouseDelta = (Mouse.GetState().Position - lastMousePosition).ToVector2() * _mouseSensitivity;
            var cameraRight = Vector3.Cross(cameraDirection, Vector3.Up);

            cameraDirection = Vector3.Transform(cameraDirection, Matrix.CreateFromAxisAngle(Vector3.Up, -mouseDelta.X));
            cameraDirection = Vector3.Transform(cameraDirection, Matrix.CreateFromAxisAngle(cameraRight, -mouseDelta.Y));
            cameraPosition += ((Keyboard.GetState().IsKeyDown(Keys.Right) ? 1 : 0) -
                               (Keyboard.GetState().IsKeyDown(Keys.Left) ? 1 : 0)) * cameraRight;
            cameraPosition += ((Keyboard.GetState().IsKeyDown(Keys.Up) ? 1 : 0) -
                               (Keyboard.GetState().IsKeyDown(Keys.Down) ? 1 : 0)) * cameraDirection;
            
            lastMousePosition = Mouse.GetState().Position;
            
        }

        public Matrix View()
        {
            Matrix view = Matrix.CreateLookAt(cameraPosition, cameraPosition + cameraDirection, Vector3.Up);
            return view;
        }
    }
}
