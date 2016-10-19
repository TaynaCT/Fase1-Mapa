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
                new Vector3(0f, 300.0f, 0f),
                new Vector3(1500, 0, 1500), Vector3.Up);

            effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(60f),
                aspectRatio, 1f, 1000f);

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

            for (int z = 0; z < texture.Height-1; z++)
            {
                for (int x = 0; x < texture.Width-1; x++)
                {                    
                    index[(z * (texture.Width-1) + x) * 6] = (short)(x + z * texture.Width);
                    index[(z * (texture.Width-1) + x) * 6 + 1] = (short)(x + 1 + (z + 1) * texture.Width);
                    index[(z * (texture.Width-1) + x) * 6 + 2] = (short)(x + (z + 1) * texture.Width);
                    index[(z * (texture.Width-1) + x) * 6 + 3] = (short)(x + z * texture.Width);
                    index[(z * (texture.Width-1) + x) * 6 + 4] = (short)(x + 1 + (z * texture.Width));
                    index[(z * (texture.Width-1) + x) * 6 + 5] = (short)(x + 1 + (z + 1) * texture.Width);
                    
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

        public void Draw(GraphicsDevice device, Matrix view)
        {
            effect.View = view;
            effect.World = worldMatrix;
            effect.CurrentTechnique.Passes[0].Apply();
            device.SetVertexBuffer(vertexBuffer);
            device.Indices = indexBuffer;

            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, texels.Length/3);
        }
                
    }
}
