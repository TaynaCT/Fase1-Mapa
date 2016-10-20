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

        Vector3 direction;
        Vector3 position;

        private Point lastMousePosition;
        private Vector2 mouseSensitivity;

        public Camera()
        {
            direction = Vector3.Forward;
            position = Vector3.Up * 10;
            mouseSensitivity = new Vector2(.01f, .005f);
        }

        public void Update()
        {
            var mouseDelta = (Mouse.GetState().Position - lastMousePosition).ToVector2() * mouseSensitivity;
            var cameraRight = Vector3.Cross(direction, Vector3.Up);// O cross dos dois vetores devolve o vetor direção para a qual a camera deve se mover

            direction = Vector3.Transform(direction, Matrix.CreateFromAxisAngle(Vector3.Up, -mouseDelta.X));
            direction = Vector3.Transform(direction, Matrix.CreateFromAxisAngle(cameraRight, -mouseDelta.Y));

            //movimentação da camera 
            position += ((Keyboard.GetState().IsKeyDown(Keys.NumPad6) ? 1 : 0) -
                         (Keyboard.GetState().IsKeyDown(Keys.NumPad4) ? 1 : 0)) * cameraRight;

            position += ((Keyboard.GetState().IsKeyDown(Keys.NumPad8) ? 1 : 0) -
                         (Keyboard.GetState().IsKeyDown(Keys.NumPad5) ? 1 : 0)) * direction;

            lastMousePosition = Mouse.GetState().Position;

            lastMousePosition = Mouse.GetState().Position;
            
        }

        public Matrix View()
        {
            Matrix view = Matrix.CreateLookAt(position, position + direction, Vector3.Up);
            return view;
        }
    }
}
