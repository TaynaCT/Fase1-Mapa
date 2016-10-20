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
        public BasicEffect effect;
        Matrix worldMatrix;
        Color[] texels;
        Texture2D texture;
        Texture2D groundTexture;
        VertexPositionColorTexture[] vertex;
        short[] index;
        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;
    
        
        public Map(GraphicsDevice device, ContentManager content)
        {            
            //matriz identidade do mundo 
            worldMatrix = Matrix.Identity;
            effect = new BasicEffect(device);

            //textura do mapa
            groundTexture = content.Load<Texture2D>("grassTexture");
            effect.TextureEnabled = true;
            effect.Texture = groundTexture;

            float aspectRatio = (float)device.Viewport.Width / device.Viewport.Height;

            effect.View = Matrix.CreateLookAt(
                new Vector3(0f, 500, 0f),
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
            vertex = new VertexPositionColorTexture[texels.Length];           

            //Gerar vertices
            int y;
            for (int z = 0; z < texture.Height; z++)
            {
                for (int x = 0; x < texture.Width; x++)
                {
                    y = (texels[z * texture.Width + x].R);
                    vertex[z * texture.Width + x] = new VertexPositionColorTexture(new Vector3(x, y * 0.5f, z), texels[x], new Vector2(x%2, z%2));                   
                }
            }

            //organização dos index
            index = new short[6 * (texture.Height - 1) * (texture.Width - 1)]; // index.Length = texels.Length

            int a = 0;
            for (int z = 0; z < texture.Height - 1; z++)
            {
                int aux = texture.Width - 1;
                for (int x = 0; x < texture.Width; x++)
                {
                    //   if (z % 2 == 0)
                    //{
                    index[2 * a] = (short)(z * 128 + x + 128);
                    index[2 * a + 1] = (short)(z * 128 + x);
                    a++;
                    /*}
                    if(z % 2 !=0)
                    {
                       index[2 * a] = (short)(z * 128 + aux + 128);
                       index[2 * a + 1] = (short)(z * 128 + aux);
                       a++;
                       aux--;
                    }*/
                }
            }

            vertexBuffer = new VertexBuffer(device,
                typeof(VertexPositionColorTexture),
                vertex.Length,
                BufferUsage.None);
            vertexBuffer.SetData<VertexPositionColorTexture>(vertex);

            indexBuffer = new IndexBuffer(device,
                typeof(short),
                index.Length,
                BufferUsage.None);
            indexBuffer.SetData(index);
        }
                
        public void Draw(GraphicsDevice device, Matrix cameraView)
        {
            effect.View = cameraView;
            effect.World = worldMatrix;
            effect.CurrentTechnique.Passes[0].Apply();
            device.SetVertexBuffer(vertexBuffer);
            device.Indices = indexBuffer;

            //device.DrawIndexedPrimitives(PrimitiveType.TriangleStrip, 0, 0, texels.Length * 6);
                        
            for (int i = 0; i < texture.Height - 1; i++)
            {
                device.DrawIndexedPrimitives(PrimitiveType.TriangleStrip, 0, 0, texels.Length * 6, i * 256, (texture.Width - 1) * 2);
            }

        }
                
    }
}
