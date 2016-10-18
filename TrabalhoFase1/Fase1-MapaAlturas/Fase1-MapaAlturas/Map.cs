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
        VertexPositionColor[] vertex;
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
            vertex = new VertexPositionColor[texels.Length];

            //Gerar vertices
            int y;
            for (int z = 0; z < texture.Height; z++)
            {
                for (int x = 0; x < texture.Width; x++)
                {
                    y = (texels[z * texture.Width + x].R);
                    vertex[z * texture.Width + x] = new VertexPositionColor(new Vector3(x, y * 0.5f, z), texels[z * texture.Width + x]);                                        
                }
            }

            //organização dos index
            index = new short[6 * (texture.Height - 1) * (texture.Width - 1)]; // index.Length = texels.Length

            for (int z = 0; z < texture.Height; z++)
            {
                for (int x = 0; x < texture.Width; x++)
                {
                    index[(z * texture.Width + x) * 2] = (short)(z * texture.Width + x);
                    index[(z * texture.Width + x) * 2 + 1] = (short)(z * texture.Width + x + 1);                    
                }
                 
            }

            vertexBuffer = new VertexBuffer(device,
                typeof(VertexPositionColor),
                vertex.Length,
                BufferUsage.None);
            vertexBuffer.SetData<VertexPositionColor>(vertex);

            indexBuffer = new IndexBuffer(device,
                typeof(short),
                index.Length,
                BufferUsage.None);
            indexBuffer.SetData(index);
        }

        public void Draw(GraphicsDevice device)
        {
            effect.World = worldMatrix;
            effect.CurrentTechnique.Passes[0].Apply();
            device.SetVertexBuffer(vertexBuffer);
            device.Indices = indexBuffer;

            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, texels.Length/3);
        }

    }
}
