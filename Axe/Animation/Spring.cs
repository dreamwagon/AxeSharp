using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace com.dreamwagon.axe
{
    public class Spring
    {
        private float stiffness = -10f;
        private float damping = 1.2f;
        private float rest = 0f;
        private float velocity = 0f;
        private float position = 0f;

        public Spring()
        {
        }

        public Spring(float stiffness, float damping)
        {
            this.stiffness = stiffness;
            this.damping = damping;
        }

        public void Update(GameTime gameTime)
        {
            float dt = gameTime.ElapsedGameTime.Milliseconds * 0.001f;

            velocity += (stiffness * (position - rest) - damping * velocity) * dt;
            position += velocity * dt;
        }

        public float Stiffness
        {
            get { return stiffness; }
            set { stiffness = value; }
        }

        public float Damping
        {
            get { return damping; }
            set { damping = value; }
        }

        public float Rest
        {
            get { return rest; }
            set { rest = value; }
        }

        public float Position
        {
            get { return position; }
            set { position = value; }
        }

        public float Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

    }
}
