using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace com.dreamwagon.axe
{
    public class GraphicsHelper
    {
        public static void drawBound(GraphicsDevice graphicsDevice, Bound3f b, BasicEffect basicEffect, Color color)
        {
            basicEffect.CurrentTechnique.Passes[0].Apply();
            var vertices = new VertexPositionColor[24];

            vertices[0].Position = new Vector3(b.l, b.t, b.n);
            vertices[1].Position = new Vector3(b.l, b.b, b.n);
            vertices[2].Position = new Vector3(b.r, b.b, b.n);
            vertices[3].Position = new Vector3(b.r, b.t, b.n);

            vertices[4].Position = new Vector3(b.l, b.t, b.f);
            vertices[5].Position = new Vector3(b.l, b.b, b.f);
            vertices[6].Position = new Vector3(b.r, b.b, b.f);
            vertices[7].Position = new Vector3(b.r, b.t, b.f);

            vertices[8].Position = new Vector3(b.l, b.t, b.n);
            vertices[9].Position = new Vector3(b.l, b.t, b.f);
            vertices[10].Position = new Vector3(b.l, b.b, b.n);
            vertices[11].Position = new Vector3(b.l, b.b, b.f);

            vertices[12].Position = new Vector3(b.r, b.b, b.n);
            vertices[13].Position = new Vector3(b.r, b.b, b.f);
            vertices[14].Position = new Vector3(b.r, b.t, b.n);
            vertices[15].Position = new Vector3(b.r, b.t, b.f);

            vertices[16].Position = new Vector3(b.l, b.t, b.n);
            vertices[17].Position = new Vector3(b.r, b.t, b.n);
            vertices[18].Position = new Vector3(b.l, b.b, b.n);
            vertices[19].Position = new Vector3(b.r, b.b, b.n);

            vertices[20].Position = new Vector3(b.l, b.t, b.f);
            vertices[21].Position = new Vector3(b.r, b.t, b.f);
            vertices[22].Position = new Vector3(b.l, b.b, b.f);
            vertices[23].Position = new Vector3(b.r, b.b, b.f);

            for (int i = 0; i < vertices.Length; i++ )
            {
                vertices[i].Color = color;
            }

            graphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, 12);
        }
    }
}
