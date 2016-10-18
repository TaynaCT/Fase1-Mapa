using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace Fase1_MapaAlturas
{
    class Map
    {
        BasicEffect effect;
        Matrix worldMatrix;
        Color[] texels;
        Texture2D texture;
        VertexPositionColor[] vertices;
        short[] index;
        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;

        
        public Map(GraphicsDevice device, ContentManager content)
        {
            //matriz identidade do mundo 
            worldMatrix = Matrix.Identity;
            effect = new BasicEffect(device);

            //camera
            float aspectRatio = (float)device.Viewport.Width / device.Viewport.Height;
            effect.View = Matrix.CreateLookAt(
                new Vector3(3f, 3.0f, 3f),
                Vector3.Zero, Vector3.Up);

            effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45f),
                aspectRatio, 1f, 10f);

            effect.LightingEnabled = false;
            effect.VertexColorEnabled = true;

            //Indexação dos vertices do mapa, a partir dos valores rgb da textura
            texture = content.Load<Texture2D>("lh3d1");
            texels = new Color[texture.Height*texture.Width]; // tamanho do array = a autura * a largura da img
            texture.GetData(texels);
            vertices = new VertexPositionColor[texels.Length];

            //Gerar vertices
            for (int z = 0; z < texture.Height; z++)
            {
                for (int x = 0; x < texture.Width; x++)
                {
                    float y = (texels[z * texture.Width + x].R);

                    vertices[z * texture.Width + x] = new VertexPositionColor(new Vector3(x, y * 0.5f, z), texels[z * texture.Width + x]);
                                        
                }
            }

            vertexBuffer = new VertexBuffer(device, typeof(VertexPositionColor),
                vertices.Length,
                BufferUsage.None);
            vertexBuffer.SetData<VertexPositionColor>(vertices);

            indexBuffer = new IndexBuffer(device,
                typeof(short),
                index.Length,
                BufferUsage.None);
            indexBuffer.SetData(index);

            index = new short[texels.Length]; // index.Length = texels.Length

            //organização dos indices
            for (int i = 0; i < index.Length+1; i++)
            {

            }
        }

        public void Draw(GraphicsDevice device)
        {
            effect.World = worldMatrix;

            effect.CurrentTechnique.Passes[0].Apply();
            device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, vertices, 0,(texture.Height * texture.Width)-2);
        }

    }
}
