using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using com.dreamwagon;
using Microsoft.Xna.Framework.Graphics;

namespace com.dreamwagon.axe
{
    /// <summary>
    /// A 2-dimensional camera with size, center, scale, and rotation.
    /// Size is a constant property, but center, scale, and rotation all have
    /// movement queues and jitter. The jitter is too add shaking effects 
    /// (center jitter), wobble effects (rotational jitter) and smacked effects
    /// (scale jitter).
    /// </summary>
    public class Camera
    {
        private static Camera instance = new Camera();

        /// <summary>
        /// Gets the Camera in the Game.
        /// </summary>
        /// <returns></returns>
        public static Camera Get()
        {
            return instance;
        }

        public const double EPSILON = 0.001;

        // Bounds to restrict the camera to. If this is non-null the 
        // axis-aligned bounds of the camera will always remain in this
        // rectangle, even if the camera has to be scaled down or translated
        // into the region.
        private Nullable<Rectangle> bounds = new Nullable<Rectangle>();

        // The point at which the camera rotates, by default the center.
        private Vector2 anchor = new Vector2(0.5f, 0.5f);

        // The size of the camera.
        private Vector2 size;

        // The center of the camera, the motion queues, and jitter.
        private Vector2 center = new Vector2();
        private Queue<Tween> centerXTarget = new Queue<Tween>();
        private Queue<Tween> centerYTarget = new Queue<Tween>();
        private Spring centerXJitter;
        private Spring centerYJitter;

        // The scale of the camera, the scaling queues, and jitter.
        private Vector2 scale = new Vector2(1f);
        private Queue<Tween> scaleXTarget = new Queue<Tween>();
        private Queue<Tween> scaleYTarget = new Queue<Tween>();
        private Spring scaleXJitter;
        private Spring scaleYJitter;

        // The rotation of the camera in radians, the rotating queue, and jitter.
        private float rotation = 0;
        private Queue<Tween> rotationTarget = new Queue<Tween>();
        private Spring rotationJitter;

        // The transform of the camera which is dictated by the computed
        // center, scaling, and rotation.
        private Matrix transform = Matrix.Identity;

        // The camera can be disabled from updating, perhaps to do pausing for
        // some undetermined period of time.
        private bool enabled = true;

        // The camera can pause for a given number of milliseconds before resuming update.
        private int pauseTime = 0;

        /// <summary>
        /// Secretly instantiates a new Camera.
        /// </summary>
        private Camera()
        { 
        }

        /// <summary>
        /// Updates the Camera class if its enabled.
        /// </summary>
        /// <param name="gameTime">The GameTime object.</param>
        public void Update(GameTime gameTime)
        {
            if (enabled)
            {
                // Pause?
                if (pauseTime > 0)
                {
                    pauseTime = Math.Max(0, pauseTime - gameTime.ElapsedGameTime.Milliseconds);

                    return;
                }

                // Adjust center, scale, and rotation according to any tweens on the queue.
                HandleQueue(centerXTarget, gameTime, ref center.X);
                HandleQueue(centerYTarget, gameTime, ref center.Y);
                HandleQueue(scaleXTarget, gameTime, ref scale.X);
                HandleQueue(scaleYTarget, gameTime, ref scale.Y);
                HandleQueue(rotationTarget, gameTime, ref rotation);

                // The offsets to the computed orientation.
                float offsetCX = 0f, offsetCY = 0f, offsetSX = 0f, offsetSY = 0f, offsetRotation = 0f;

                // Adjust the offsets, not the actual variables.
                HandleJitter(centerXJitter, gameTime, ref offsetCX);
                HandleJitter(centerYJitter, gameTime, ref offsetCY);
                HandleJitter(scaleXJitter, gameTime, ref offsetSX);
                HandleJitter(scaleYJitter, gameTime, ref offsetSY);
                HandleJitter(rotationJitter, gameTime, ref offsetRotation);

                // Compute the actual values.
                float actualCX = center.X + offsetCX;
                float actualCY = center.Y + offsetCY;
                float actualSX = scale.X + offsetSX;
                float actualSY = scale.Y + offsetSY;
                float actualRotation = rotation + offsetRotation;

                // This ensures the corners and sides of the camera stays
                // inside the bounds rectangle.
                if (bounds.HasValue)
                {
                    // The non rotated boundaries on the camera.
                    float hw = size.X * 0.5f;
                    float hh = size.Y * 0.5f;
                    float T = actualCY - hh;
                    float B = actualCY + hh;
                    float R = actualCX + hw;
                    float L = actualCX - hw;

                    // The adjusted boundaries on the camera.
                    if (actualRotation != 0f)
                    {
                        float cos = (float)Math.Abs(Math.Cos(actualRotation));
                        float sin = (float)Math.Abs(Math.Sin(actualRotation));
                        float aw = (cos * size.X + sin * size.Y) * 0.5f;
                        float ah = (cos * size.Y + sin * size.X) * 0.5f;
                        T = actualCY - ah;
                        B = actualCY + ah;
                        R = actualCX + aw;
                        L = actualCX - aw;
                    }

                    // Compute the view bounds, and scale the camera down
                    // if it extends beyound the bounds.
                    float viewHeight = B - T;
                    float viewWidth = R - L;
                    Rectangle b = bounds.Value;

                    if (viewHeight > b.Height)
                    {
                        actualSY *= b.Height / viewHeight;
                    }
                    if (viewWidth > b.Width)
                    {
                        actualSX *= b.Width / viewWidth;
                    }

                    // Translate the camera if it exists outside the bounds.
                    if (T < b.Top)
                    {
                        actualCY += (b.Top - T);
                    }
                    if (B > b.Bottom)
                    {
                        actualCY -= (B - b.Bottom);
                    }
                    if (L < b.Left)
                    {
                        actualCX += (b.Left - L);
                    }
                    if (R > b.Right)
                    {
                        actualCX -= (R - b.Right);
                    }
                }

                // Compute the transform for the camera.
                transform = Matrix.CreateTranslation(-size.X * anchor.X, -size.Y * anchor.Y, 0) *
                            Matrix.CreateRotationZ(actualRotation) *
                            Matrix.CreateScale(actualSX, actualSY, 1) *
                            Matrix.CreateTranslation(actualCX, actualCY, 0);
            }
        }

        /// <summary>
        /// Updates the queue and the referenced value.
        /// </summary>
        private void HandleQueue(Queue<Tween> queue, GameTime gameTime, ref float value)
        {
            if (queue.Count > 0)
            {
                Tween next = queue.Peek();

                next.Update(gameTime);
                value = next.Value;

                if (next.IsComplete())
                {
                    queue.Dequeue();
                }
            }
        }

        /// <summary>
        /// Updates the jitter and the referenced value.
        /// </summary>
        private void HandleJitter(Spring jitter, GameTime gameTime, ref float value)
        {
            if (jitter != null)
            {
                jitter.Update(gameTime);

                value += jitter.Position;

                if (Math.Abs(jitter.Position - jitter.Rest) < EPSILON && Math.Abs(jitter.Velocity) < EPSILON)
                {
                    jitter.Position = jitter.Rest;
                    jitter.Velocity = 0;
                }
            }
        }

        /// <summary>
        /// Queues a camera move.
        /// </summary>
        /// <param name="x">The move on the x-axis to queue.</param>
        /// <param name="y">The move on the y-axis to queue.</param>
        /// <param name="setStart">If the starting variables of the tweens 
        /// should be the center of the camera.</param>
        public void QueueCenter(Tween x, Tween y, bool setStart)
        {
            Queue(centerXTarget, x, center.X, setStart);
            Queue(centerYTarget, y, center.Y, setStart);
        }

        /// <summary>
        /// Queues a camera move on the x-axis.
        /// </summary>
        /// <param name="x">The move on the x-axis to queue.</param>
        /// <param name="setStart">If the starting variables of the tweens 
        /// should be the center of the camera.</param>
        public void QueueCenterX(Tween x, bool setStart)
        {
            Queue(centerXTarget, x, center.X, setStart);
        }

        /// <summary>
        /// Queues a camera move on the y-axis.
        /// </summary>
        /// <param name="x">The move on the y-axis to queue.</param>
        /// <param name="setStart">If the starting variables of the tweens 
        /// should be the center of the camera.</param>
        public void QueueCenterY(Tween y, bool setStart)
        {
            Queue(centerYTarget, y, center.Y, setStart);
        }

        /// <summary>
        /// Queues a camera to scale.
        /// </summary>
        /// <param name="x">The scaling on the x-axis to queue.</param>
        /// <param name="y">The scaling on the y-axis to queue.</param>
        /// <param name="setStart">If the starting variables of the tweens 
        /// should be the center of the camera.</param>
        public void QueueScale(Tween x, Tween y, bool setStart)
        {
            Queue(scaleXTarget, x, scale.X, setStart);
            Queue(scaleYTarget, y, scale.Y, setStart);
        }

        /// <summary>
        /// Queues a camera to scale on the x-axis.
        /// </summary>
        /// <param name="x">The scaling on the x-axis to queue.</param>
        /// <param name="setStart">If the starting variables of the tweens 
        /// should be the center of the camera.</param>
        public void QueueScaleX(Tween x, bool setStart)
        {
            Queue(scaleXTarget, x, scale.X, setStart);
        }

        /// <summary>
        /// Queues a camera to scale on the y-axis.
        /// </summary>n
        /// <param name="y">The scaling on the y-axis to queue.</param>
        /// <param name="setStart">If the starting variables of the tweens 
        /// should be the center of the camera.</param>
        public void QueueScaleY(Tween y, bool setStart)
        {
            Queue(scaleYTarget, y, scale.Y, setStart);
        }

        public void QueueRotation(Tween radians, bool setStart)
        {
            Queue(rotationTarget, radians, rotation, setStart);
        }

        /// <summary>
        /// Generically Queues the tween on the queue and optionally sets its start.
        /// </summary>
        /// <param name="queue">The queue to add to.</param>
        /// <param name="x">The tween to enqueue.</param>
        /// <param name="start">The starting value if nothing exists on the end of the queue</param>
        /// <param name="setStart">If the start of the tween should be set</param>
        private void Queue(Queue<Tween> queue, Tween x, float start, bool setStart)
        {
            if (setStart)
            {
                if (queue.Count > 0)
                {
                    x.Start = queue.ElementAt(queue.Count - 1).End;
                }
                else
                {
                    x.Start = start;
                }
            }

            queue.Enqueue(x);
        }

        public void JitterCenter(float x, float y)
        {
            CreateSpring(ref centerXJitter).Velocity = x;
            CreateSpring(ref centerYJitter).Velocity = y;
        }

        public void JitterScale(float x, float y)
        {
            CreateSpring(ref scaleXJitter).Velocity = x;
            CreateSpring(ref scaleYJitter).Velocity = y;
        }

        public void JitterRotation(float radians)
        {
            CreateSpring(ref rotationJitter).Velocity = radians;
        }

        private Spring CreateSpring(ref Spring s)
        {
            if ( s == null )
            {
                s = new Spring();
            }

            return s;
        }

        public void Begin(SpriteBatch spriteBatch)
        {
           Begin( spriteBatch, null);
        }

        public void Begin(SpriteBatch spriteBatch, Effect effect)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, effect, transform);
        }

        public void Begin(SpriteBatch spriteBatch, SpriteSortMode sort, BlendState blend)
        {
            spriteBatch.Begin(sort, blend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, transform);
        }

        public void End(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
        }

        public void Clear()
        {
            centerXTarget.Clear();
            centerYTarget.Clear();
            scaleXTarget.Clear();
            scaleYTarget.Clear();
            rotationTarget.Clear();

            centerXJitter = null;
            centerYJitter = null;
            scaleXJitter = null;
            scaleYJitter = null;
            rotationJitter = null;
        }

        public void Reset()
        {
            Clear();
            Scale = new Vector2(1);
            Rotation = 0f;
        }

        public Matrix Transform
        {
            get { return transform; }
        }

        public Vector2 Center
        {
            get { return center; }
            set { center = value; }
        }

        public Queue<Tween> CenterXTarget
        {
            get { return centerXTarget; }
        }

        public Queue<Tween> CenterYTarget
        {
            get { return centerYTarget; }
        }

        public Spring CenterXJitter
        {
            get { return centerXJitter; }
            set { centerYJitter = value; }
        }

        public Spring CenterYJitter
        {
            get { return centerYJitter; }
            set { centerYJitter = value; }
        }

        public Vector2 Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public Queue<Tween> ScaleXTarget
        {
            get { return scaleXTarget; }
        }

        public Queue<Tween> ScaleYTarget
        {
            get { return scaleYTarget; }
        }

        public Spring ScaleXJitter
        {
            get { return scaleXJitter; }
            set { scaleXJitter = value; }
        }

        public Spring ScaleYJitter
        {
            get { return scaleYJitter; }
            set { scaleYJitter = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Queue<Tween> RotationTarget
        {
            get { return rotationTarget; }
        }

        public Spring RotationJitter
        {
            get { return rotationJitter; }
            set { rotationJitter = value; }
        }

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public int PauseTime
        {
            get { return pauseTime; }
            set { pauseTime = value; }
        }

        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
        }

        public Rectangle Bounds
        {
            get { return bounds.Value; }
            set { bounds = value; }
        }

    }
}
