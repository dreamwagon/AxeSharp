using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace com.dreamwagon.axe
{

    /// <summary>
    /// An interface all game entities implement. When a game entity expires
    /// it is automatically removed from a list to cease any updates and
    /// draw methods.
    /// </summary>
    public interface Entity
    {
        /// <summary>
        /// Determines if the entity is expired. The entity may be expired
        /// from itself or forcefully expired.
        /// </summary>
        bool Expired { get; set; }

        /// <summary>
        /// Method invoked on the entity once it is removed from the enitty 
        /// list. This should not be invoked directly.
        /// </summary>
        void OnExpired();

        /// <summary>
        /// Updates the entity with the elapsed time.
        /// </summary>
        /// <param name="dt">The elapsed time in seconds since the last update.</param>
        void Update(float dt);

        /// <summary>
        /// Draws the entity with the given SpriteBatch.
        /// </summary>
        /// <param name="spriteBatch"></param>
        void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// Whether the entity is updated.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Whether the entity is drawn.
        /// </summary>
        bool Visible { get; set; }
    }

    /// <summary>
    /// An abstract implementation of the Entity.
    /// </summary>
    public abstract class AbstractEntity : Entity
    {
        protected bool expired = false;
        protected bool enabled = true;
        protected bool visible = true;

        protected abstract void OnUpdate(float dt);
        protected abstract void OnDraw(SpriteBatch spriteBatch);

        public virtual void OnExpired()
        {
        }

        public void Update(float dt)
        {
            if (enabled)
            {
                OnUpdate(dt);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (visible)
            {
                OnDraw(spriteBatch);
            }
        }

        public bool Expired
        {
            get { return expired; }
            set { expired = value; }
        }

        public bool Enabled
        {
            get { return enabled; }
            set { expired = value; }
        }

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }
    }

    /// <summary>
    /// A list of entities. This list is also an entity, and also handles 
    /// automatically removing expired entities.
    /// </summary>
    public class EntityList : AbstractEntity, System.Collections.IEnumerable
    {
        const int DEFAULT_CAPACITY = 16;

        protected Entity[] entities;
        protected int size;

        public EntityList()
            : this( DEFAULT_CAPACITY ) { }

        public EntityList(int initialCapacity)
        {
            entities = new Entity[initialCapacity];
        }

        protected override void OnUpdate(float dt)
        {
            int alive = 0;

            for (int i = 0; i < size; i++)
            {
                Entity e = entities[i];

                e.Update(dt);

                entities[alive] = e;

                if (e.Expired)
                {
                    e.OnExpired();
                }
                else
                {
                    alive++;
                }
            }

            while (size > alive)
            {
                entities[--size] = null;
            }
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < size; i++)
            {
                entities[i].Draw(spriteBatch);
            }
        }

        public override void OnExpired()
        {
            for (int i = 0; i < size; i++)
            {
                entities[i].OnExpired();
                entities[i] = null;
            }
            size = 0;
        }

        public E Add<E>(E e) where E : Entity
        {
            Allocate(1);

            entities[size++] = e;

            return e;
        }

        public void Add(params Entity[] entityArray)
        {
            Allocate(entityArray.Length);

            for (int i = 0; i < entityArray.Length; i++)
            {
                entities[size++] = entityArray[i];
            }
        }

        public void Add(List<Entity> entityList)
        {
            Allocate(entityList.Count);

            for (int i=0; i<entityList.Count; i++)
            {
                entities[size++] = entityList[i];
            }
        }

        public void Allocate(int spaces)
        {
            if (spaces + size > entities.Length)
            {
                Resize( Math.Max( entities.Length << 1, spaces + size) );
            }
        }

        public void Deallocate(int spaces)
        {
            Resize(Math.Max(size, entities.Length - spaces));
        }

        public void Resize(int capacity)
        {
            System.Array.Resize<Entity>(ref entities, capacity);
        }

        public void Clear(bool invokeExpire)
        {
            if (invokeExpire)
            {
                for (int i = 0; i < size; i++)
                {
                    entities[i].OnExpired();
                    entities[i] = null;
                }
            }
            else
            {
                for (int i = 0; i < size; i++)
                {
                    entities[i] = null;
                }
            }

            size = 0;
        }

        public int Size
        {
            get { return size; }
        }

        public Entity this[int i]
        {
            get { return entities[i]; }
            set { entities[i] = value; }
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            for (int i = 0; i < size; i++)
            {
                yield return entities[i];
            }
        }

    }


    /// <summary>
    /// An attribute is a wrapper around some primitive. This enables things
    /// like vectors, colors, and rectangles to be animated, watched, and 
    /// synced.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface Attribute<T> 
    {
        bool IsEqual(T value);
        T Get();
        void Set(T value);
        void Update(T target);
        void Copy(T from, T to);
        void Interpolate(T start, T end, float delta);
        void Add(T value, float delta);
        void Mul(T value);
        void Scale(float d);
        float Distance(T to);
        T Clone();
        Attribute<T> Create();
    }

    /// <summary>
    /// An attribute for XNA's Vector2 class.
    /// </summary>
    public class Vec2f : Attribute<Vector2>
    {
        public Vector2 v = new Vector2();

        public Vec2f()
        {
        }

        public Vec2f(float v)
        {
            this.v = new Vector2(v);
        }

        public Vec2f(float x, float y)
        {
            this.v = new Vector2(x, y);
        }

        public Vec2f(Vector2 v)
        {
            this.v = v;
        }

        public Vec2f(Vec2f v)
        {
            this.v = new Vector2(v.v.X, v.v.Y);
        }

        public bool IsEqual(Vector2 value)
        {
            return (value.X == v.X && value.Y == v.Y);
        }

        public Vector2 Get()
        {
            return v;
        }

        public void Set(Vec2f value)
        {
            v.X = value.v.X;
            v.Y = value.v.Y;
        }

        public void Set(float x, float y)
        {
            v.X = x;
            v.Y = y;
        }

        public void Set(Vector2 value)
        {
            v.X = value.X;
            v.Y = value.Y;
        }

        public void Update(Vector2 target)
        {
            target.X = v.X;
            target.Y = v.Y;
        }

        public void Copy(Vector2 from, Vector2 to)
        {
            to.X = from.X;
            to.Y = from.Y;
        }

        public void Interpolate(Vector2 start, Vector2 end, float delta)
        {
            v.X = (end.X - start.X) * delta + start.X;
            v.Y = (end.Y - start.Y) * delta + start.Y;
        }

        public void Add(Vector2 value, float delta)
        {
            v.X += value.X * delta;
            v.Y += value.Y * delta;
        }

        public void Mul(Vector2 value)
        {
            v.X *= value.X;
            v.Y *= value.Y;
        }

        public void Scale(float d)
        {
            v.X *= d;
            v.Y *= d;
        }

        public float Distance(Vector2 to)
        {
            float dx = (v.X - to.X);
            float dy = (v.Y - to.Y);

            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public Vector2 Clone()
        {
            return new Vector2(v.X, v.Y);
        }

        public Attribute<Vector2> Create()
        {
            return new Vec2f();
        }

        public void Length(float d)
        {
            float sq = (v.X * v.X + v.Y * v.Y);
            if (sq != 0 && sq != d * d)
            {
                float w = (float)(1.0 / d * Math.Sqrt(sq));
                v.X *= w;
                v.Y *= w;
            }
        }

        public float Angle()
        {
            double a = Math.Atan2(v.Y, v.X);

            if (a < 0)
            {
                a += Math.PI * 2;
            }

            return (float)a;
        }

        public void Angle(float radians, float magnitude)
        {
            v.X = (float)Math.Cos(radians) * magnitude;
            v.Y = (float)Math.Sin(radians) * magnitude;
        }

        public void Norm()
        {
            Norm(v.X, v.Y);
        }

        public void Norm(float x, float y)
        {
            float sq = (x * x + y * y);
            if (sq != 1 && sq != 0)
            {
                float w = (float)(1.0 / Math.Sqrt(sq));
                x *= w;
                y *= w;
            }
            v.X = x;
            v.Y = y;
        }
    }

    /// <summary>
    /// An attribute for XNA's Rectangle class.
    /// </summary>
    public class Rect2i : Attribute<Rectangle>
    {
        public Rectangle r = new Rectangle();

        public Rect2i()
        {
        }

        public Rect2i(int x, int y, int w, int h)
        {
            r.X = x;
            r.Y = y;
            r.Width = w;
            r.Height = h;
        }

        public Rect2i(Rectangle r)
        {
            this.r = r;
        }

        public Rect2i(Rect2i r)
        {
            this.r = new Rectangle(r.r.X, r.r.Y, r.r.Width, r.r.Height);
        }

        public bool IsEqual(Rectangle value)
        {
            return (value.X == r.X && value.Y == r.Y && value.Width == r.Width && value.Height == r.Height);
        }

        public Rectangle Get()
        {
            return r;
        }

        public void Set(Rectangle value)
        {
            r.X = value.X;
            r.Y = value.Y;
            r.Width = value.Width;
            r.Height = value.Height;
        }

        public void Update(Rectangle target)
        {
            target.X = r.X;
            target.Y = r.Y;
            target.Width = r.Width;
            target.Height = r.Height;
        }

        public void Copy(Rectangle from, Rectangle to)
        {
            to.X = from.X;
            to.Y = from.Y;
            to.Width = from.Width;
            to.Height = from.Height;
        }

        public void Interpolate(Rectangle start, Rectangle end, float delta)
        {
            r.X = (int)((end.X - start.X) * delta) + start.X;
            r.Y = (int)((end.Y - start.Y) * delta) + start.Y;
            r.Width = (int)((end.Width - start.Width) * delta) + start.Width;
            r.Height = (int)((end.Height - start.Height) * delta) + start.Height;
        }

        public void Add(Rectangle value, float delta)
        {
            r.X += (int)(value.X * delta);
            r.Y += (int)(value.Y * delta);
            r.Width += (int)(value.Width * delta);
            r.Height += (int)(value.Height * delta);
        }

        public void Mul(Rectangle value)
        {
            r.X *= value.X;
            r.Y *= value.Y;
            r.Width *= value.Width;
            r.Height *= value.Height;
        }

        public void Scale(float d)
        {
            r.X = (int)(r.X * d);
            r.Y = (int)(r.Y * d);
            r.Width = (int)(r.Width * d);
            r.Height = (int)(r.Height * d);
        }

        public float Distance(Rectangle to)
        {
            float dx = (r.X - to.X);
            float dy = (r.Y - to.Y);
            float dw = (r.Width - to.Width);
            float dh = (r.Height - to.Height);

            return (float)Math.Sqrt(dx * dx + dy * dy + dw * dw + dh * dh);
        }

        public Rectangle Clone()
        {
            return new Rectangle(r.X, r.Y, r.Width, r.Height);
        }

        public Attribute<Rectangle> Create()
        {
            return new Rect2i();
        }
    }

    /// <summary>
    /// An attribute for XNA's Color class.
    /// </summary>
    public class Color4b : Attribute<Color>
    {
        public const byte MIN = 0x00;
        public const byte MAX = 0xFF;

        public Color c = new Color();

        public Color4b()
        {
        }

        public Color4b(int r, int g, int b)
        {
            c = new Color(r, g, b);
        }

        public Color4b(int r, int g, int b, int a)
        {
            c = new Color(r, g, b, a);
        }

        public Color4b(float r, float g, float b)
        {
            c = new Color(r, g, b);
        }

        public Color4b(float r, float g, float b, float a)
        {
            c = new Color(r, g, b, a);
        }

        public Color4b(Color color)
        {
            c = color;
        }

        public bool IsEqual(Color value)
        {
            return (value.R == c.R && value.G == c.G && value.B == c.B && value.A == c.A);
        }

        public Color Get()
        {
            return c;
        }

        public void Set(Color value)
        {
            c.R = value.R;
            c.G = value.G;
            c.B = value.B;
            c.A = value.A;
        }

        public void Update(Color target)
        {
            target.R = c.R;
            target.G = c.G;
            target.B = c.B;
            target.A = c.A;
        }

        public void Copy(Color from, Color to)
        {
            to.R = from.R;
            to.G = from.G;
            to.B = from.B;
            to.A = from.A;
        }

        public void Interpolate(Color start, Color end, float delta)
        {
            c.R = Numbers.Clamp((byte)((end.R - start.R) * delta + start.R), MIN, MAX);
            c.G = Numbers.Clamp((byte)((end.G - start.G) * delta + start.G), MIN, MAX);
            c.B = Numbers.Clamp((byte)((end.B - start.B) * delta + start.B), MIN, MAX);
            c.A = Numbers.Clamp((byte)((end.A - start.A) * delta + start.A), MIN, MAX);
        }

        public void Add(Color value, float delta)
        {
            c.R = Numbers.Clamp((byte)(c.R + value.R * delta), MIN, MAX);
            c.G = Numbers.Clamp((byte)(c.G + value.G * delta), MIN, MAX);
            c.B = Numbers.Clamp((byte)(c.B + value.B * delta), MIN, MAX);
            c.A = Numbers.Clamp((byte)(c.A + value.A * delta), MIN, MAX);
        }

        public void Mul(Color value)
        {
            c.R = Numbers.Clamp((byte)(c.R * value.R), MIN, MAX);
            c.G = Numbers.Clamp((byte)(c.G * value.G), MIN, MAX);
            c.B = Numbers.Clamp((byte)(c.B * value.B), MIN, MAX);
            c.A = Numbers.Clamp((byte)(c.A * value.A), MIN, MAX);
        }

        public void Scale(float d)
        {
            c.R = Numbers.Clamp((byte)(c.R * d), MIN, MAX);
            c.G = Numbers.Clamp((byte)(c.G * d), MIN, MAX);
            c.B = Numbers.Clamp((byte)(c.B * d), MIN, MAX);
            c.A = Numbers.Clamp((byte)(c.A * d), MIN, MAX);
        }

        public float Distance(Color to)
        {
            int dr = (int)c.R - to.R;
            int dg = (int)c.G - to.G;
            int db = (int)c.B - to.B;
            int da = (int)c.A - to.A;

            return (float)Math.Sqrt(dr * dr + dg * dg + db * db + da * da);
        }

        public Color Clone()
        {
            return new Color(c.R, c.G, c.B, c.A);
        }

        public Attribute<Color> Create()
        {
            return new Color4b();
        }

        public static Color4b[] Array(params Color[] colors)
        {
            Color4b[] c = new Color4b[colors.Length];
            for (int i = 0; i < colors.Length; i++)
            {
                c[i] = new Color4b(colors[i]);
            }
            return c;
        }
    }

    /// <summary>
    /// An attribute for XNA's Matrix class.
    /// </summary>
    public class Matrixf : Attribute<Matrix>
    {
         public Matrix m = new Matrix();

        public Matrixf()
        {
        }

        public Matrixf(Matrix m)
        {
            Set(m);
        }

        public bool IsEqual(Matrix value)
        {
            return (value == m);
        }

        public Matrix Get()
        {
            return m;
        }

        public void Set(Matrix value)
        {
            Copy(value, m);
        }

        public void Update(Matrix target)
        {
            Copy(m, target);
        }

        public void Copy(Matrix from, Matrix to)
        {
            to.M11 = from.M11;
            to.M12 = from.M12;
            to.M13 = from.M13;
            to.M14 = from.M14;
            to.M21 = from.M21;
            to.M22 = from.M22;
            to.M23 = from.M23;
            to.M24 = from.M24;
            to.M31 = from.M31;
            to.M32 = from.M32;
            to.M33 = from.M33;
            to.M34 = from.M34;
            to.M41 = from.M41;
            to.M42 = from.M42;
            to.M43 = from.M43;
            to.M44 = from.M44;
        }

        public void Interpolate(Matrix start, Matrix end, float delta)
        {
            m = (end - start) * delta + start;
        }

        public void Add(Matrix value, float delta)
        {
            m += value * delta;
        }

        public void Mul(Matrix value)
        {
            m *= value;
        }

        public void Scale(float d)
        {
            m *= d;
        }

        public float Distance(Matrix to)
        {
            // Just use the distance between the translations
            float dx = (m.M14 - to.M14);
            float dy = (m.M24 - to.M24);
            float dz = (m.M34 - to.M34);

            return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public Matrix Clone()
        {
            return new Matrix(m.M11, m.M12, m.M13, m.M14, m.M21, m.M22, m.M23, m.M24, m.M31, m.M32, m.M33, m.M34, m.M41, m.M42, m.M43, m.M44);
        }

        public Attribute<Matrix> Create()
        {
            return new Matrixf();
        }
    }

    /// <summary>
    /// A tile is a source rectangle on a texture.
    /// </summary>
    public class Tile : Attribute<Tile>
    {
        private Rectangle source;
        private Texture2D texture;

        public Tile() { }

        public Tile(Texture2D texture, int w, int h)
            : this( texture, 0, 0, w, h ) { }
        
        public Tile(Texture2D texture, int x, int y, int w, int h)
        {
            this.texture = texture;
            this.source = new Rectangle(x, y, w, h);
        }

        public Tile(Texture2D texture, Rectangle source)
        {
            this.texture = texture;
            this.source = source;
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Rectangle Source
        {
            get { return source; }
            set { source = value; }
        }

        public bool IsEqual(Tile value)
        {
            return (texture == value.texture && source == value.source);
        }

        public Tile Get()
        {
            return this;
        }

        public void Set(Tile value)
        {
            Copy(value, this);
        }

        public void Update(Tile target)
        {
            Copy(this, target);
        }

        public void Copy(Tile from, Tile to)
        {
            to.texture = from.texture;
            to.source = from.source;
        }

        public void Interpolate(Tile start, Tile end, float delta)
        {
            source.X = (int)((end.source.X - start.source.X) * delta + start.source.X);
            source.Y = (int)((end.source.Y - start.source.Y) * delta + start.source.Y);
            source.Width = (int)((end.source.Width - start.source.Width) * delta + start.source.Width);
            source.Height = (int)((end.source.Height - start.source.Height) * delta + start.source.Height);
        }

        public void Add(Tile value, float delta)
        {
            source.X += (int)(value.source.X * delta);
            source.Y += (int)(value.source.X * delta);
            source.Width += (int)(value.source.X * delta);
            source.Height += (int)(value.source.X * delta);
        }

        public void Mul(Tile value)
        {
            source.X *= value.source.X;
            source.Y *= value.source.Y;
            source.Width *= value.source.Width;
            source.Height *= value.source.Height;
        }

        public void Scale(float d)
        {
            source.X = (int)(source.X * d);
            source.Y = (int)(source.Y * d);
            source.Width = (int)(source.Width * d);
            source.Height = (int)(source.Height * d);
        }

        public float Distance(Tile to)
        {
            return Math.Abs(source.X - to.source.X) +
                Math.Abs(source.Y - to.source.Y);
        }

        public Tile Clone()
        {
            return new Tile(texture, source);
        }

        public Attribute<Tile> Create()
        {
            return new Tile();
        }
    }

    /// <summary>
    /// A composite Attribute which is the range between two attributes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Range<T> : Attribute<Range<T>>
    {
        private Attribute<T> min;
        private Attribute<T> max;

        public Range()
        {
        }

        public Range(Attribute<T> x)
        {
            this.min = this.max = x;
        }

        public Range(Attribute<T> min, Attribute<T> max)
        {
            this.min = min;
            this.max = max;
        }

        public Attribute<T> Min
        {
            get { return min; }
            set { min = value; }
        }

        public Attribute<T> Max
        {
            get { return max; }
            set { max = value; }
        }

        public void Random(Attribute<T> target)
        {
            target.Interpolate(min.Get(), max.Get(), Numbers.Random(1f));
        }

        public T Random()
        {
            Attribute<T> random = min.Create();
            random.Interpolate(min.Get(), max.Get(), Numbers.Random(1f));
            return random.Get();
        }

        public bool IsEqual(Range<T> value)
        {
            return (value.min.IsEqual(min.Get()) && value.max.IsEqual(max.Get()));
        }

        public Range<T> Get()
        {
            return this;
        }

        public void Set(Range<T> value)
        {
            min.Set(value.min.Get());
            max.Set(value.max.Get());
        }

        public void Set(Attribute<T> min, Attribute<T> max)
        {
            this.min = min;
            this.max = max;
        }

        public void Set(Attribute<T> x)
        {
            this.min = x;
            this.max = x;
        }

        public void Set(T min, T max)
        {
            this.min.Set(min);
            this.max.Set(max);
        }

        public void Set(T x)
        {
            this.min.Set(x);
            this.max.Set(x);
        }

        public void Update(Range<T> target)
        {
            min.Update(target.min.Get());
            max.Update(target.max.Get());
        }

        public void Copy(Range<T> from, Range<T> to)
        {
            min.Copy(from.min.Get(), to.min.Get());
            max.Copy(from.max.Get(), to.max.Get());
        }

        public void Interpolate(Range<T> start, Range<T> end, float delta)
        {
            min.Interpolate(start.min.Get(), end.min.Get(), delta);
            max.Interpolate(start.max.Get(), end.max.Get(), delta);
        }

        public void Add(Range<T> value, float delta)            
        {
            min.Add(value.min.Get(), delta);
            max.Add(value.max.Get(), delta);
        }

        public void Mul(Range<T> value)
        {
            min.Mul(value.min.Get());
            max.Mul(value.max.Get());
        }

        public void Scale(float d)
        {
            min.Scale(d);
            max.Scale(d);
        }

        public float Distance(Range<T> to)
        {
            return min.Distance(to.min.Get()) + max.Distance(to.max.Get());
        }

        public Range<T> Clone()
        {
            Attribute<T> cloneMin = min.Create();
            cloneMin.Set(min.Get());
            Attribute<T> cloneMax = max.Create();
            cloneMax.Set(max.Get());

            return new Range<T>(cloneMin, cloneMax);
        }

        public Attribute<Range<T>> Create()
        {
            return new Range<T>();
        }

    }

    /// <summary>
    /// A range between two integers.
    /// </summary>
    public class Rangei
    {
        private int min;
        private int max;

        public Rangei()
            : this(0, 0)
        {
        }

        public Rangei(int x)
            : this(x, x)
        {
        }

        public Rangei(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public void Set(int x)
        {
            this.min = x;
            this.max = x;
        }

        public void Set(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public int Random()
        {
            return Numbers.Random(min, max);
        }

        public int Min
        {
            get { return min; }
            set { min = value; }
        }

        public int Max
        {
            get { return max; }
            set { max = value; }
        }
    }

    /// <summary>
    /// A range between two floats.
    /// </summary>
    public class Rangef
    {
        private float min;
        private float max;

        public Rangef()
            : this(0, 0)
        {
        }

        public Rangef(float x)
            : this(x, x)
        {
        }

        public Rangef(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public void Set(Rangef range)
        {
            this.min = range.min;
            this.max = range.max;
        }

        public void Set(float x)
        {
            this.min = x;
            this.max = x;
        }

        public void Set(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public float Random()
        {
            return Numbers.Random(min, max);
        }

        public float Min
        {
            get { return min; }
            set { min = value; }
        }

        public float Max
        {
            get { return max; }
            set { max = value; }
        }
    }

    /// <summary>
    /// A factory for creating objects of type T.
    /// </summary>
    /// <typeparam name="T">The type of object to create</typeparam>
    public delegate T Factory<T>();

    /// <summary>
    /// A path will set an attribute's value given a delta value between 0 
    /// and 1. A path is typically used in Event which actually animates
    /// an attribute along the path.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface Path<T>
    {
        void Set(Attribute<T> subject, float delta);
    }

    /// <summary>
    /// An N-order generic Bezier path.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BezierPath<T> : Path<T>
    {
        private Attribute<T>[] points;
        private Attribute<T> temp;
        private int[] weights;

        public BezierPath( params Attribute<T>[] points) 
        {
            this.points = points;
            this.weights = computeWeights(points.Length);
            this.temp = points[0].Create();
        }

        private int[] computeWeights(int n)
        {
            int[] w = new int[n--];

            for (int i = 0; i <= n; i++)
            {
                w[i] = Numbers.Choose(n, i);
            }

            return w;
        }

        public void Set(Attribute<T> subject, float delta)
        {
            int n = points.Length;
		    float[] inv = inverses( n, delta );
		    float x = 1;
		
		    temp.Scale( 0 );
    		
		    for (int i = 0; i < n; i++) 
		    {
    			temp.Add( points[i].Get(), weights[i] * inv[i] * x );
    
			    x *= delta;
    		}
		
		    subject.Set( temp.Get() );
        }

        private float[] inverses(int n, float d)
        {
            float[] inv = new float[n];

            inv[--n] = 1;

            for (int i = --n; i >= 0; i--)
            {
                inv[i] = inv[i + 1] * (1 - d);
            }

            return inv;
        }

        public int Order()
        {
            return points.Length;
        }

        public T Get(int i)
        {
            return points[i].Get();
        }

        public Attribute<T>[] Points()
        {
            return points;
        }

    }

    /// <summary>
    /// A path that has been compiled from another path. This can be used at 
    /// the expensive of a little bit more memory in the case where a path
    /// is very CPU intensive (like the Bezier Path). The more attributes that 
    /// are passed into the CompiledPath the smoother the path will be.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CompiledPath<T> : Path<T>
    {
        private Attribute<T>[] points;

        public CompiledPath(Path<T> path, Attribute<T>[] allocated)
        {
            this.points = allocated;

            int n = points.Length - 1;

            for (int i = 0; i <= n; i++)
            {
                path.Set(points[i], (float)i / n);
            }
        }

        public CompiledPath(Path<T> path, Attribute<T>[] allocated, Attribute<T> factory)
        {
            this.points = allocated;

            int n = points.Length - 1;

            for (int i = 0; i <= n; i++)
            {
                path.Set(points[i] = factory.Create(), (float)i / n);
            }
        }

        public void Set(Attribute<T> subject, float delta)
        {
            int n = points.Length;
            int i = Numbers.Clamp((int)(delta * n), 0, n - 1);

            subject.Set(points[i].Get());
        }

        public int Length()
        {
            return points.Length;
        }

        public T Get(int i)
        {
            return points[i].Get();
        }

        public Attribute<T>[] Points()
        {
            return points;
        }
	
    }

    /// <summary>
    /// A cubic curve between 4 attributes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CubicPath<T> : Path<T>
    {
        private Attribute<T> p0;
        private Attribute<T> p1;
        private Attribute<T> p2;
        private Attribute<T> p3;

        private Attribute<T> temp;

        public CubicPath(Attribute<T> p0, Attribute<T> p1, Attribute<T> p2, Attribute<T> p3)
        {
            this.p0 = p0;
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            this.temp = p0.Create();
        }

        public void Set(Attribute<T> subject, float d1)
        {
            float d2 = d1 * d1;
            float d3 = d1 * d2;
            float i1 = 1 - d1;
            float i2 = i1 * i1;
            float i3 = i1 * i2;

            temp.Set(p0.Get());
            temp.Scale(i3);
            temp.Add(p1.Get(), 3 * i2 * d1);
            temp.Add(p2.Get(), 3 * i1 * d2);
            temp.Add(p3.Get(), d3);

            subject.Set(temp.Get());
        }

        public T P0()
        {
            return p0.Get();
        }

        public T P1()
        {
            return p1.Get();
        }

        public T P2()
        {
            return p2.Get();
        }

        public T P3()
        {
            return p3.Get();
        }
    }

    /// <summary>
    /// An interpolated path of evenly separated attributes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IntegralPath<T> : Path<T>
    {
        private Attribute<T>[] points;

        public IntegralPath(params Attribute<T>[] points ) 
	    {
    		this.points = points;
    	}

        public void Set(Attribute<T> subject, float delta)
        {
            float a = delta * (points.Length - 1);
            int index = Numbers.Clamp((int)a, 0, points.Length - 2);
            float q = a - index;

            subject.Interpolate(points[index].Get(), points[index + 1].Get(), q);
        }

        public Attribute<T>[] Points()
        {
            return points;
        }
    }

    /// <summary>
    /// A path of evenly separated attributes where the value is not interpolated.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JumpPath<T> : Path<T>
    {
        private Attribute<T>[] points;

        public JumpPath(params Attribute<T>[] points) 
	    {
    		this.points = points;
    	}

        public void Set(Attribute<T> subject, float delta)
        {
            float a = delta * points.Length;
            int index = Numbers.Clamp((int)a, 0, points.Length - 1);

            subject.Set(points[index].Get());
        }

        public Attribute<T>[] Points()
        {
            return points;
        }
    }

    /// <summary>
    /// A interpolated path of attributes separated by their distance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LinearPath<T> : Path<T>
    {
        private Attribute<T>[] points;
        private float[] distances;
        private float length;

        public LinearPath(params Attribute<T>[] points)
        {
            this.points = points;
            this.distances = new float[points.Length - 1];
            this.length = reset();
        }

        private float reset()
        {
            float length = 0;

            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = points[i].Distance(points[i + 1].Get());
                length += distances[i];
            }

            return length;
        }

        public void Set(Attribute<T> subject, float delta)
        {
            float ld = Numbers.Clamp(delta * length, 0, length);
            float total = distances[0];
            int i = 0;

            while (total < ld)
            {
                total += distances[++i];
            }

            float before = total - distances[i];
            float q = (ld - before) / (total - before);

            subject.Interpolate(points[i].Get(), points[i + 1].Get(), q);
        }

        public int Length()
        {
            return points.Length;
        }

        public T Get(int i)
        {
            return points[i].Get();
        }

        public Attribute<T>[] Points()
        {
            return points;
        }

        public float PathLength()
        {
            return length;
        }

        public float Distance(int i)
        {
            return distances[i];
        }

    }

    /// <summary>
    /// A quadtratic curve between 3 attributes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QuadraticPath<T> : Path<T>
    {
        private Attribute<T> p0;
        private Attribute<T> p1;
        private Attribute<T> p2;
        private Attribute<T> temp;

        public QuadraticPath(Attribute<T> p0, Attribute<T> p1, Attribute<T> p2)
        {
            this.p0 = p0;
            this.p1 = p1;
            this.p2 = p2;
            this.temp = p0.Create();
        }

        public void Set(Attribute<T> subject, float d1)
        {
            float d2 = d1 * d1;
            float i1 = 1 - d1;
            float i2 = i1 * i1;

            temp.Set(p0.Get());
            temp.Scale(i2);
            temp.Add(p1.Get(), 2 * i1 * d1);
            temp.Add(p2.Get(), d2);

            subject.Set(temp.Get());
        }

        public T P0()
        {
            return p0.Get();
        }

        public T P1()
        {
            return p1.Get();
        }

        public T P2()
        {
            return p2.Get();
        }
    }

    /// <summary>
    /// An interpolated path of attributes and a timeline for when those 
    /// points are reached.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TimedPath<T> : Path<T>
    {
        private float[] times;
        private Attribute<T>[] points;

        public TimedPath(Attribute<T>[] points, float[] times)
        {
            this.points = points;
            this.times = normalize(times);
        }

        private float[] normalize(float[] times)
        {
            float max = times[times.Length - 1];

            for (int i = 0; i < times.Length; i++)
            {
                times[i] /= max;
            }

            return times;
        }

        public void Set(Attribute<T> subject, float delta)
        {
            int n = times.Length;
            int i = 0;

            while (times[i] < delta && i < n)
            {
                i++;
            }

            if (i == 0)
            {
                subject.Set(points[0].Get());
            }
            else if (i == n)
            {
                subject.Set(points[n - 1].Get());
            }
            else
            {
                float start = times[i - 1];
                float interval = times[i] - start;
                float difference = delta - start;

                subject.Interpolate(points[i - 1].Get(), points[i].Get(), difference / interval);
            }
        }

        public int Length()
        {
            return points.Length;
        }

        public T Get(int i)
        {
            return points[i].Get();
        }

        public Attribute<T>[] Points()
        {
            return points;
        }
    }

    /// <summary>
    /// An interpolated path between two attributes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Tween<T> : Path<T>
    {
        private Attribute<T> start;
        private Attribute<T> end;

        public Tween(Attribute<T> start, Attribute<T> end)
        {
            this.start = start;
            this.end = end;
        }

        public void Set(Attribute<T> subject, float delta)
        {
            subject.Interpolate(start.Get(), end.Get(), delta);
        }

        public T Start()
        {
            return start.Get();
        }

        public T End()
        {
            return end.Get();
        }
    }

    /// <summary>
    /// An entity which adds a given attribute to another.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Mover<T> : AbstractEntity
    {
        private Attribute<T> subject;
        private Attribute<T> velocity;
        private Entity dependent;

        public Mover(Attribute<T> subject, Attribute<T> velocity)
            : this(subject, velocity, null) { }

        public Mover(Attribute<T> subject, Attribute<T> velocity, Entity dependent)
        {
            this.subject = subject;
            this.velocity = velocity;
            this.dependent = dependent;
        }

        protected override void OnUpdate(float dt)
        {
            subject.Add(velocity.Get(), dt);

            if (dependent != null && dependent.Expired)
            {
                expired = true;
            }
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
        }

        public Attribute<T> Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        public Attribute<T> Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public Entity Dependent
        {
            get { return dependent; }
            set { dependent = value; }
        }

    }

    /// <summary>
    /// An entity which ensures an attribute has the same value as another.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Pair<T> : AbstractEntity
    {
        private Attribute<T> follower, leader;
        private Entity dependent;

        public Pair(Attribute<T> follower, Attribute<T> leader)
            : this(follower, leader, null) { }

        public Pair(Attribute<T> follower, Attribute<T> leader, Entity dependent)
        {
            this.follower = follower;
            this.leader = leader;
            this.dependent = dependent;
        }

        protected override void OnUpdate(float dt)
        {
            follower.Set( leader.Get() );

            if (dependent != null && dependent.Expired)
            {
                expired = true;
            }
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
        }
    }

    /// <summary>
    /// An entity which applies a spring force to an attribute to a resting
    /// attribute state.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Spring<T> : AbstractEntity
    {
        private Attribute<T> stiffness;
        private Attribute<T> damping;
        private Attribute<T> rest;
        private Attribute<T> velocity;
        private Attribute<T> position;

        private Attribute<T> temp0;
        private Attribute<T> temp1;

        public Spring()
        {
        }

        public Spring(Attribute<T> position, Attribute<T> damping, Attribute<T> stiffness)
            : this(position, clone(position), damping, stiffness) { }

        public Spring(Attribute<T> position, Attribute<T> rest, Attribute<T> damping, Attribute<T> stiffness)
        {
            this.position = position;
            this.rest = rest;
            this.damping = damping;
            this.stiffness = stiffness;
            this.velocity = position.Create();
            this.temp0 = position.Create();
            this.temp1 = position.Create();
        }

        protected override void OnUpdate(float dt)
        {
            // velocity += ((stiffness * (position - rest)) - (damping * velocity)) * elapsed.seconds;
            // position += velocity * elapsed.seconds;

            temp0.Set(position.Get());
            temp0.Add(rest.Get(), -1);
            temp0.Mul(stiffness.Get());
            temp1.Set(damping.Get());
            temp1.Mul(velocity.Get());
            temp0.Add(temp1.Get(), -1);

            velocity.Add(temp0.Get(), dt);
            position.Add(velocity.Get(), dt);
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
        }

        public T Stiffness()
        {
            return stiffness.Get();
        }

        public T Damping()
        {
            return damping.Get();
        }

        public T Rest()
        {
            return rest.Get();
        }

        public T Velocity()
        {
            return velocity.Get();
        }

        public T Position()
        {
            return position.Get();
        }

        private static Attribute<T> clone(Attribute<T> a)
        {
            Attribute<T> c = a.Create();
            c.Set(a.Get());
            return c;
        }
    }

    /// <summary>
    /// The possible states for an Event.
    /// </summary>
    public enum EventState
    {
        Waiting, Running, Resting, Stopped
    }

    /// <summary>
    /// An entity that moves an Attribute along a given Path based on a Function.
    /// </summary>
    /// <example> 
    /// Move the camera position (Vector2) from {5,5} to {100,200} in 5 seconds:
    /// <code>
    /// Event&lt;Vec2f&gt; e = new Event&lt;Vec2f&gt;( 
    ///     new Vec2f(camera.Position), // Attribute to move, this is referenced based so camera.Position shouldn't be renewed.
    ///     new LinearPath&lt;Vec2f&gt;( new Vec2f(5,5), new Vec2f(100,200)), // Path to move along
    ///     1f, // Delay before attribute is moved, in seconds
    ///     5f, // Time it takes attribute to reach end of path.
    ///     0.5f, // Time between moving attribute from start to end of path.
    ///     2,  // Times to move attribute between start and end
    ///     Function.PingPong, // A function which bounces the attribute between the start and end in one iteration.
    ///     Function.Quartic) // A function to move the attribute in a non-linear manner.
    /// </code>
    /// Move a sprite color (Color4b) indefinitely between two values using with methods:
    /// <code>
    /// Event&lt;Color&gt; e = new Event&lt;Color&gt;()
    ///     .WithTarget( sprite.Color )
    ///     .WithPath( new Tween&lt;Color&gt;(new Color4b(255,0,0), new Color4b(0,0,255)) )
    ///     .WithDuration( 1f )
    ///     .WithLoops( Event.INFINITE );
    /// </code>
    /// </example>
    /// <typeparam name="T"></typeparam>
    public class Event<T> : AbstractEntity
    {
        public const int INFINITE = -1;

        protected Function function = new Function();
        protected Attribute<T> target;
        protected Path<T> path;
        protected EventState state = EventState.Waiting;
        protected float duration, time, delay, rest;
        protected int iteration, loops;

        public Event() { }

        public Event(Attribute<T> target, Path<T> path, float duration)
            : this(target, path, 0, duration, 0, 1) { }

        public Event(Attribute<T> target, Path<T> path, float delay, float duration)
            : this(target, path, delay, duration, 0, 1) { }

        public Event(Attribute<T> target, Path<T> path, float delay, float duration, float rest)
            : this(target, path, delay, duration, rest, 1) { }

        public Event(Attribute<T> target, Path<T> path, float delay, float duration, float rest, int loops)
            : this(target, path, delay, duration, rest, loops, Function.In, Function.Linear) { }

        public Event(Attribute<T> target, Path<T> path, float delay, float duration, float rest, int loops, Function.TypeDelegate type, Function.MethodDelegate method)
        {
            this.target = target;
            this.path = path;
            this.delay = delay;
            this.duration = duration;
            this.rest = rest;
            this.loops = loops;
            this.function.Method = method;
            this.function.Type = type;
        }

        public void Reset()
        {
            iteration = 0;
            time = 0;
            state = EventState.Waiting;
            enabled = true;
            expired = false;
        }

        public void Stop()
        {
            state = EventState.Stopped;
            enabled = false;
        }

        public void Finish()
        {
            path.Set(target, function.Delta(1));
            state = EventState.Stopped;
            enabled = false;
        }

        protected void OnStateChange(EventState previous, EventState current)
        {
        }

        protected override void OnUpdate(float dt)
        {
            time += dt;

            if (state == EventState.Waiting)
            {
                if (time > delay)
                {
                    time -= delay;
                    state = EventState.Running;
                    OnStateChange(EventState.Waiting, EventState.Running);
                }
            }
            if (state == EventState.Running)
            {
                float x = Numbers.Clamp(time / duration, 0f, 1f);
                float fx = function.Delta(x);

                if (time > duration)
                {
                    time -= duration;
                    iteration++;

                    if (iteration < loops || loops == INFINITE)
                    {
                        state = EventState.Resting;
                        OnStateChange(EventState.Running, EventState.Resting);
                    }
                    else
                    {
                        state = EventState.Stopped;
                        enabled = false;
                        expired = true;
                        OnStateChange(EventState.Running, EventState.Stopped);
                    }
                }

                path.Set(target, fx);
            }
            if (state == EventState.Resting)
            {
                if (time > rest)
                {
                    time -= rest;
                    state = EventState.Running;
                    OnStateChange(EventState.Resting, EventState.Running);
                }
            }
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
        }

        public EventState State
        {
            get { return state; }
        }

        public float Time
        {
            get { return time; }
        }

        public int Iterations
        {
            get { return iteration; }
        }

        public T Target
        {
            get { return target.Get(); }
        }

        public Path<T> Path
        {
            get { return path; }
        }

        public float Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        public float Delay
        {
            get { return delay; }
            set { delay = value; }
        }

        public float Rest
        {
            get { return rest; }
            set { rest = value; }
        }

        public int Loops
        {
            get { return loops; }
            set { loops = value; }
        }

        public Function Function
        {
            get { return function; }
            set { function = value; }
        }

        public Event<T> WithState(EventState state)
        {
            this.state = state;
            return this;
        }

        public Event<T> WithTime(float time)
        {
            this.time = time;
            return this;
        }

        public Event<T> WithIteration(int iteration)
        {
            this.iteration = iteration;
            return this;
        }

        public Event<T> WithTarget(Attribute<T> target)
        {
            this.target = target;
            return this;
        }

        public Event<T> WithPath(Path<T> path)
        {
            this.path = path;
            return this;
        }

        public Event<T> WithDuration(float duration)
        {
            this.duration = duration;
            return this;
        }

        public Event<T> WithDelay(float delay)
        {
            this.delay = delay;
            return this;
        }

        public Event<T> WithRest(float rest)
        {
            this.rest = rest;
            return this;
        }

        public Event<T> WithLoops(int loops)
        {
            this.loops = loops;
            return this;
        }

        public Event<T> WithFunction(Function function)
        {
            this.function = function;
            return this;
        }

    }

    /// <summary>
    /// Watches an attribute and keeps track of how many changes
    /// have been made to it since the changes counter has been cleared.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Watcher<T> : AbstractEntity
    {
        private Attribute<T> current;
        private T previous;
        private int changes;

        public Watcher(Attribute<T> current)
        {
            this.Set(current);
        }

        public void Set(Attribute<T> current)
        {
            this.current = current;
            this.previous = current.Clone();
        }

        protected override void OnUpdate(float dt)
        {
            bool changed = !current.IsEqual(previous);

            if (changed)
            {
                changes++;
            }

            current.Update(previous);
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
        }

        public bool HasChanged()
        {
            return (changes > 0);
        }

        public bool HasChangedAndReset()
        {
            bool hasChanged = (changes > 0);
            changes = 0;
            return hasChanged;
        }

        public void Reset()
        {
            changes = 0;
        }

        public int Changes
        {
            get { return changes; }
        }

        public T Current
        {
            get { return current.Get(); }
        }

        public T Previous
        {
            get { return previous; }
        }
    }

    /// <summary>
    /// An interface for all 2d particles to implement. This does not require
    /// an implementation to have all the functionality suggested. At the very
    /// least Location and Size need to be non-null.
    /// </summary>
    public interface Particle
    {
        void Update(float dt);
        void Reset(float lifetime);

        Vec2f Location { get; }
        Vec2f Velocity { get; }
        Vec2f Acceleration { get; }

        Vec2f Size { get; }

        Vec2f Scale { get; }
        Vec2f ScaleVelocity { get; }
        Vec2f ScaleAcceleration { get; }

        Scalarf Angle { get; }
        Scalarf AngleVelocity { get; }
        Scalarf AngleAcceleration { get; }

        Color4b Shade { get; }

        Tile Tile { get; }

        float Lifetime { get; }
        float Age { get; }
    }

    /// <summary>
    /// An emitter which has the information to created ParticleEffects.
    /// </summary>
    public class ParticleEmitter
    {
        private Rangef burstDelay = new Rangef();
        private Rangei burstAmount = new Rangei();
        private Rangei burstCapacity = new Rangei();
        private Rangei capacity = new Rangei(512);
        private Rangef delay = new Rangef();
        private Rangef angle = new Rangef();
        private Range<Vector2> scale = new Range<Vector2>(new Vec2f(1));
        private Rangef effectLife = new Rangef();
        private Rangef particleLife = new Rangef();
        private Rangei initialParticles = new Rangei();
        private Range<Vector2> offset = new Range<Vector2>(new Vec2f());
        private Range<Vector2> particleAnchor = new Range<Vector2>(new Vec2f(0.5f));
        private Tile tile;
        private BlendState blend = BlendState.Additive;
        private Factory<Particle> factory;
        private ParticleRenderer renderer;
        private ParticleVolume volume = new VolumeDefault();
        private ParticleVelocity velocity = new VelocityDefault();
        private ParticleInfluence[] influences = { };
        private ParticleInitialize[] initializers = { };
        private ParticleListener listeners;

        public Pool<Particle> pool;

        public ParticleEffect Create()
        {
            //Default pool
            pool = new Pool<Particle>(new Particle[512], FullParticle.Factory);

            return new ParticleEffect(this);
        }

        //public Particle NewParticle()
        //{
        //    return factory();
        //} 

        public void Reset( Particle p, ParticleEffect effect  )
	    {
            p.Reset(NewParticleLife());

		    Vec2f direction = volume.NewVolume( p, effect );
		
    		velocity.NewVelocity( p, direction, effect );

            p.Location.Add(effect.Offset.Get(), 1);

    		for ( int i = 0; i< initializers.Length; i++ )
		    {
                initializers[i].Initialize(p, effect);
		    }
	    }
        public Particle Emit()
        {
            return (pool != null ? pool.alloc() : factory());
        }

        public void free(Particle p)
        {
            if (pool != null)
            {
                pool.free(p);
            }
        }

        //public Particle Emit(ParticleEffect effect)
        //{
        //    Particle p = factory();

        //    Reset(p, effect);

        //    return p;
        //}

        public float NewBurstDelay()
        {
            return burstDelay.Random();
        }

        public int NewBurstAmount()
        {
            return burstAmount.Random();
        }

        public int NewBurstCapacity()
        {
            return burstCapacity.Random();
        }

        public int NewCapacity()
        {
            return capacity.Random();
        }

        public float NewDelay()
        {
            return delay.Random();
        }

        public float NewEffectLife()
        {
            return effectLife.Random();
        }

        public float NewParticleLife()
        {
            return particleLife.Random();
        }

        public float NewAngle()
        {
            return angle.Random();
        }

        public int NewInitialParticles()
        {
            return initialParticles.Random();
        }

        public Vector2 NewScale()
        {
            return scale.Random();
        }

        public Vector2 NewOffset()
        {
            return offset.Random();
        }

        public Vector2 NewParticleAnchor()
        {
            return particleAnchor.Random();
        }

        public ParticleRenderer Renderer
        {
            get { return renderer; }
            set { renderer = value; }
        }

        public ParticleInfluence[] Influences
        {
            get { return influences; }
            set { influences = value; }
        }

        public void AddInfluence(ParticleInfluence influence)
        {
            this.influences = Arrays.Add(influence, influences);
        }

        public ParticleInitialize[] Initializers
        {
            get { return initializers; }
            set { initializers = value; }
        }

        public void AddInitializer(ParticleInitialize initializer)
        {
            this.initializers = Arrays.Add(initializer, initializers);
        }

        public ParticleListener Listener
        {
            get { return listeners; }
            set { listeners = value; }
        }

        public ParticleVelocity Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public ParticleVolume Volume
        {
            get { return volume; }
            set { volume = value; }
        }

        public Factory<Particle> Factory
        {
            get { return factory; }
            set { factory = value; }
        }

        public Tile Tile
        {
            get { return tile; }
            set { tile = value; }
        }

        public BlendState Blend
        {
            get { return blend; }
            set { blend = value; }
        }

        public Rangef BurstDelay
        {
            get { return burstDelay; }
            set { burstDelay = value; }
        }

        public Rangei BurstAmount
        {
            get { return burstAmount; }
            set { burstAmount = value; }
        }

        public Rangei BurstCapacity
        {
            get { return burstCapacity; }
            set { burstCapacity = value; }
        }

        public Rangei Capacity
        {
            get { return capacity; }
            set { capacity = value; }
        }

        public Rangef Delay
        {
            get { return delay; }
            set { delay = value; }
        }

        public Rangef Angle
        {
            get { return angle; }
            set { angle = value; }
        }

        public Range<Vector2> Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public Range<Vector2> Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        public Rangef EffectLife
        {
            get { return effectLife; }
            set { effectLife = value; }
        }

        public Rangef ParticleLife
        {
            get { return particleLife; }
            set { particleLife = value; }
        }

        public Rangei InitialParticles
        {
            get { return initialParticles; }
            set { initialParticles = value; }
        }

        public Range<Vector2> ParticleAnchor
        {
            get { return particleAnchor; }
            set { particleAnchor = value; }
        }
    }

    /// <summary>
    /// A delegate for particles to be drawn.
    /// </summary>
    /// <param name="particles">An array of particles to draw.</param>
    /// <param name="particleCount">The number of particles in the array.</param>
    /// <param name="spriteBatch">A sprite batch to draw with.</param>
    /// <param name="effect">The effect rendering the particles.</param>
    public delegate void ParticleRenderer(Particle[] particles, int particleCount, SpriteBatch spriteBatch, ParticleEffect effect);

    /// <summary>
    /// A volume sets the initial position of a particle with respect to its effect.
    /// </summary>
    public interface ParticleVolume
    {
        /// <summary>
        /// Sets the particles position and returns a suggested direction by the volume.
        /// </summary>
        /// <param name="p">The particle to set the position of.</param>
        /// <param name="effect">The effect the particle exists in.</param>
        /// <returns>A suggested direction of the particle.</returns>
        Vec2f NewVolume(Particle p, ParticleEffect effect);
    }

    /// <summary>
    /// A velocity sets the initial velocity of a particle.
    /// </summary>
    public interface ParticleVelocity
    {
        /// <summary>
        /// Sets the particles velocity.
        /// </summary>
        /// <param name="p">The particle to set the velocity of.</param>
        /// <param name="direction">A suggested direction from the volume.</param>
        /// <param name="effect">The effect the particle exists in.</param>
        void NewVelocity(Particle p, Vec2f direction, ParticleEffect effect);
    }

    /// <summary>
    /// Initializes the particle before its added to the effect.
    /// </summary>
    public interface ParticleInitialize
    {
        /// <summary>
        /// Initializes the particle before its added to the effect.
        /// </summary>
        /// <param name="p">The particle to initialize.</param>
        /// <param name="effect">The effect the particle is being added to.</param>
        void Initialize(Particle p, ParticleEffect effect);
    }

    /// <summary>
    /// An influence affects all particles in an effect during their entire life.
    /// </summary>
    public interface ParticleInfluence
    {
        /// <summary>
        /// Influences the set of particles.
        /// </summary>
        /// <param name="particles">The internal array of particles.</param>
        /// <param name="particleCount">The number of particles that exist in the array.</param>
        /// <param name="elapsed">The number of seconds that have elapsed since the last influence (approximately).</param>
        void Influence(Particle[] particles, int particleCount, float elapsed);
    }

    /// <summary>
    /// An interface to notify when every particle is emitted, updated, and has died.
    /// </summary>
    public interface ParticleListener
    {
        void OnParticleUpdate(Particle p, ParticleEffect effect);
        void OnParticleEmit(Particle p, ParticleEffect effect);
        void OnParticleDeath(Particle p, ParticleEffect effect);
    }

    /// <summary>
    /// Possible states a ParticleEffect can have.
    /// </summary>
    public enum ParticleEffectState
    {   
	    Delayed, Alive, Dying, Dead
    }

    /// <summary>
    /// A collection of particles.
    /// </summary>
    public class ParticleEffect : AbstractEntity
    {
        public const int DEFAULT_CAPACITY = 64;

        protected Vector2 particleAnchor = new Vector2();
        protected ParticleEmitter emitter;
        protected Particle[] particles;
        protected ParticleListener listener;
        protected ParticleRenderer renderer;
        protected Tile tile;
        protected int particleCount;
        protected float emitterTime;
        protected float burstDelay;
        protected int burstAmount;
        protected int burstCapacity;
        protected int capacity;
        protected float time;
	    protected float delay;
	    protected float lifetime;
        protected BlendState blend;
        protected bool emitting = true;
	    protected ParticleEffectState state = ParticleEffectState.Delayed;
	    protected Vec2f scale = new Vec2f( 1f );
	    protected Scalarf angle = new Scalarf( 0f );
	    protected Vec2f position = new Vec2f( 0f, 0f );
	    protected Vec2f offset = new Vec2f( 0f, 0f );

        public ParticleEffect(ParticleEmitter emitter)
            : this( emitter, DEFAULT_CAPACITY )
        {
        }

        public ParticleEffect(ParticleEmitter emitter, int initialCapacity)
        {
            this.emitter = emitter;
            this.particles = new Particle[initialCapacity];
            this.emitterTime = 0f;
            this.tile = emitter.Tile;
            this.blend = emitter.Blend;
            this.delay = emitter.NewDelay();
            this.lifetime = emitter.NewEffectLife();
            this.capacity = emitter.NewCapacity();
            this.listener = emitter.Listener;
            this.renderer = emitter.Renderer;
            this.angle.Set(emitter.NewAngle());
            this.scale.Set(emitter.NewScale());
            this.offset.Set(emitter.NewOffset());
            this.particleAnchor = emitter.NewParticleAnchor();
            this.burstCapacity = emitter.NewBurstCapacity();
            this.burstAmount = emitter.NewInitialParticles();
            this.burstDelay = emitter.NewBurstDelay();
            this.emitterTime = burstDelay;
        }

        protected override void OnUpdate(float dt)
        {
            if (enabled)
            {
                time += dt;

                if (state == ParticleEffectState.Delayed && time > delay)
                {
                    state = ParticleEffectState.Alive;
                    time -= delay;
                }
                if (lifetime > 0 && state == ParticleEffectState.Alive && time > lifetime)
                {
                    state = ParticleEffectState.Dying;
                }
                if (state == ParticleEffectState.Dying && IsDead())
                {
                    Expired = true;
                }
                else
                {
                    Update(dt, state);
                }
            }
        }

        private void Update(float dt, ParticleEffectState state)
        {
            if ( state == ParticleEffectState.Alive && emitting )
		    {
			    emitterTime += dt;
			
			    if ( emitterTime > burstDelay ) 
			    {
				    Burst();
			    }
		    }
		
		    Step( dt );

            for (int i = 0; i < emitter.Influences.Length; i++ )
            {
                emitter.Influences[i].Influence(particles, particleCount, dt);
            }
		
		    if ( listener != null ) 
		    {
			    for (int i = 0; i < particleCount; i++) 
			    {
				    listener.OnParticleUpdate( particles[ i ], this );
			    }
		    }
        }

        private void Step(float dt)
        {
		    int alive = 0;

            for (int i = 0; i < particleCount; i++)
            {
                Particle p = particles[i];

                p.Update(dt);

                if (p.Age < p.Lifetime)
                {
                    particles[alive++] = p;
                }
                else
                {
                    if (listener != null)
                    {
                        listener.OnParticleDeath(p, this);
                    }

                    emitter.free(p);

                    particles[i] = null;
                }
            }
            particleCount = alive;
        }

        public void Burst()
        {
            //burstAmount = Math.Min(burstAmount, capacity - particleCount);

            //if (burstAmount > 0)
            //{
            //    Allocate(burstAmount);

            //    while (--burstAmount >= 0)
            //    {
            //        Particle p = particles[particleCount];

            //        if (p == null)
            //        {
            //            particles[particleCount] = p = emitter.Emit();
            //        }

            //        emitter.Reset(p, this);

            //        if (listener != null)
            //        {
            //            listener.OnParticleEmit(p, this);
            //        }

            //        particleCount++;
            //    }

            //    if (burstCapacity > 0 && --burstCapacity == 0)
            //    {
            //        state = ParticleEffectState.Dying;
            //    }
            //}

            //emitterTime -= Math.Min(burstDelay, emitterTime);

            //burstDelay = emitter.NewBurstDelay();
            //burstAmount = emitter.NewBurstAmount();



            /************************************/
            burstAmount = Math.Min(burstAmount, capacity - particleCount);

            if (burstAmount > 0)
            {
                Allocate(burstAmount);

                while (--burstAmount >= 0)
                {
                    Particle p = emitter.Emit();

                    particles[particleCount++] = p;

                    emitter.Reset(p, this);

                    if (listener != null)
                    {
                        listener.OnParticleEmit(p, this);
                    }
                }

                if (burstCapacity > 0 && --burstCapacity == 0)
                {
                    state = ParticleEffectState.Dying;
                }
            }

            emitterTime -= Math.Min(burstDelay, emitterTime);

            burstDelay = emitter.NewBurstDelay();
            burstAmount = emitter.NewBurstAmount();
        }

        private void Allocate(int amount)
        {
            if (amount + particleCount > particles.Length)
            {
                System.Array.Resize<Particle>(ref particles, Math.Max(particles.Length << 1, amount + particleCount));
            }
        }

        protected bool IsDead()
        {
            return (particleCount == 0);
        }

        protected void Clear()
        {
            for (int i = 0; i < particleCount; i++)
            {
                emitter.free(particles[i]);
                particles[i] = null;
            }

            particleCount = 0;
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            renderer(particles, particleCount, spriteBatch, this);
        }

        public void Start()
        {
            Reset();
        }

        public void Reset()
        {
            time = 0;
            state = (delay > 0 ? ParticleEffectState.Delayed : ParticleEffectState.Alive);
            expired = false;
        }

        public override void OnExpired()
        {
            //state = ParticleEffectState.Dead;
            clear();
        }

        public void clear()
        {
            for (int i = 0; i < particleCount; i++)
            {
              //  particles[i].expire();

                emitter.free(particles[i]);

                particles[i] = null;
            }

            particleCount = 0;
        }

        public Particle[] ParticleArray
        {
            get { return particles; }
        }

        public int Particles
        {
            get { return particleCount; }
        }

        public int Capacity
        {
            get { return capacity; }
            set { capacity = value; }
        }

        public bool Emitting
        {
            get { return emitting; }
            set { emitting = value; }
        }

        public ParticleEmitter Emitter
        {
            get { return emitter; }
        }

        public ParticleListener Listener
        {
            get { return listener; }
            set { listener = value; }
        }

        public Vector2 ParticleAnchor
        {
            get { return particleAnchor; }
            set { particleAnchor = value; }
        }

        public Tile Tile
        {
            get { return tile; }
            set { tile = value; }
        }

        public ParticleRenderer Renderer 
        {
		    get { return renderer; }
            set { renderer = value; }
	    }

    	public BlendState Blend
        {
		    get { return blend; }
            set { blend = value; }
	    }

	    public float Lifetime
        {
    		get { return lifetime; }
            set { lifetime = value; }
	    }   

	    public float Delay
        {
    		get { return delay; }
            set { delay = value; }
    	}

	    public float Age
        {
    		get { return time; }
    	}
	
	    public ParticleEffectState State 
        {
    		get { return state; }
	    }   

	    public Vec2f Scale 
        {
    		get { return scale; }
    	}

    	public Scalarf Angle
        {
		    get { return angle; }
    	}

	    public Vec2f Position
        {
    		get { return position; }
            set { position = value; }
	    }   

	    public Vec2f Offset 
        {
            get { return offset; }
            set { offset = value; }
    	}

    }

    public abstract class AbstractInfluence : ParticleInfluence
    {
	    protected abstract void onInfluence( Particle p, float elapsed );
	
    	protected virtual void onInfluenceStart( Particle[] particles, int particleCount, float elapsed )
    	{
    		
    	}

        protected virtual void onInfluenceEnd(Particle[] particles, int particleCount, float elapsed)
    	{
    		
    	}
	
    	public void Influence( Particle[] particles, int particleCount, float elapsed )
    	{
		    onInfluenceStart( particles, particleCount, elapsed );	
		    for ( int i = 0; i < particleCount; i++ )
		    {
    			onInfluence( particles[ i ], elapsed );
		    }
		    onInfluenceEnd( particles, particleCount, elapsed );
	    }
    }

    public abstract class InfluencePath<T> : AbstractInfluence
    {
    	protected Path<T> path;
    	
    	public InfluencePath()
    	{
    	}
    	
    	public InfluencePath( Path<T> path )
    	{
    		this.path = path;
	    }
    }

    public class InfluenceAcceleration : AbstractInfluence
    {
	    private Vec2f acceleration = new Vec2f();
	
	    public InfluenceAcceleration( Vec2f acceleration )
	    {
		    this.acceleration.Set( acceleration.Get() );
	    }
	
	    public InfluenceAcceleration( float accelerationX, float accelerationY )
	    {
		    this.acceleration.Set( accelerationX, accelerationY );
	    }

	    protected override void onInfluence( Particle p, float elapsed )
	    {
		    p.Velocity.Add( acceleration.Get(), elapsed );
	    }
    }

    public class InfluenceAlpha : InfluencePath<Scalarf>
    {
	
	    private Scalarf alpha = new Scalarf();
	
    	public InfluenceAlpha()
	    {
	    }
    	
    	public InfluenceAlpha( Path<Scalarf> path )
            : base( path )
    	{
    	}
	
    	public InfluenceAlpha( float start, float end )
            : base(new Tween<Scalarf>( new Scalarf(start), new Scalarf(end) ))
    	{
	    }
    	
    	public InfluenceAlpha( params float[] alphas )
            : base(new LinearPath<Scalarf>( Scalarf.Array( alphas ) ))
    	{
	    }
    
    	protected override void onInfluence( Particle p, float elapsed )
    	{
            if (p!= null)
		    path.Set( alpha, p.Age / p.Lifetime );
    
		    p.Shade.c.A = Numbers.Clamp((byte)(alpha.v * 255), (byte)0, (byte)255);
	    }
    }

    public class InfluenceAngle : InfluencePath<Scalarf>
    {
	
    	public InfluenceAngle()
	    {
	    }
	
	    public InfluenceAngle( Path<Scalarf> path )
            : base( path )
	    {
    	}

	    protected override void onInfluence( Particle p, float elapsed )
	    {
    		path.Set( p.Angle, p.Age / p.Lifetime );
    	}
    }

    public class InfluenceColor : InfluencePath<Color>
    {
	
	    public InfluenceColor() { }

        public InfluenceColor(Path<Color> path)
            : base( path ) { }
    	
    	public InfluenceColor( Color4b start, Color4b end )
            : base(new Tween<Color>(start, end)) { }

        public InfluenceColor(params Color4b[] colors)
            : base(new LinearPath<Color>(colors)) { }

        public InfluenceColor(params Color[] colors)
            : base(new LinearPath<Color>(Color4b.Array(colors))) { }
    	
    	protected override void onInfluence( Particle p, float elapsed )
    	{
		    path.Set( p.Shade, p.Age / p.Lifetime );
    	}
    }

    public class InfluenceDamping : AbstractInfluence
    {
        private Vec2f damping = new Vec2f();

        public InfluenceDamping()
        {
        }

        public InfluenceDamping(float damping)
        {
            this.damping.Set(damping, damping);
        }

        public InfluenceDamping(Vec2f damping)
        {
            this.damping.Set(damping);
        }

        public InfluenceDamping(float dampingX, float dampingY)
        {
            this.damping.Set(dampingX, dampingY);
        }

        protected override void onInfluence(Particle p, float elapsed)
        {
            p.Velocity.Mul( damping.Get() );
        }
    }
  
    public class InfluenceScale : InfluencePath<Vector2>
    {
	    
	    public InfluenceScale() { }
    	
    	public InfluenceScale( Path<Vector2> path )
            : base (path) { }
    
    	protected override void onInfluence( Particle p, float elapsed )
    	{
		    path.Set( p.Scale, p.Age / p.Lifetime );
	    }
    
    }

    public class InfluenceSize : InfluencePath<Vector2>
    {
	    public InfluenceSize() { }
	
	    public InfluenceSize( Path<Vector2> path )
            : base( path ) { }

        public InfluenceSize(Vec2f start, Vec2f end)
            : base( new Tween<Vector2>( start, end ) ) { }
	
	    public InfluenceSize( params Vec2f[] sizes )
            : base( new LinearPath<Vector2>( sizes ) ) { }

	    protected override void onInfluence( Particle p, float elapsed )
	    {
		    path.Set( p.Size, p.Age / p.Lifetime );
	    }

    }

    public class InfluenceTile : InfluencePath<Tile>
    {
	    public InfluenceTile() { }

        public InfluenceTile(params Tile[] tiles)
            : base(new JumpPath<Tile>(tiles)) { }
	
	    protected override void onInfluence( Particle p, float elapsed )
	    {
		    path.Set( p.Tile, p.Age / p.Lifetime );
	    }
    }

    public class InfluenceVelocity : InfluencePath<Vector2>
    {
        public InfluenceVelocity() { }

        public InfluenceVelocity(Path<Vector2> path)
            : base (path ) { }

        protected override void onInfluence(Particle p, float elapsed)
        {
            path.Set(p.Velocity, p.Age / p.Lifetime);
        }
    }

    public class InfluenceAlign : AbstractInfluence
    {
        private Scalarf offset = new Scalarf();

        public InfluenceAlign()
        {
        }

        public InfluenceAlign(float offset)
        {
            this.offset.Set(offset);
        }

        protected override void onInfluence(Particle p, float elapsed)
        {
            p.Angle.v = p.Velocity.Angle() + offset.v;
        }
    }

    public class InitializeAcceleration : ParticleInitialize
    {
        private Range<Vector2> acceleration = new Range<Vector2>();
	
	    public InitializeAcceleration()
	    {
	    }
	
	    public InitializeAcceleration( float min, float max )
	    {
		    acceleration.Set( new Vec2f(min), new Vec2f(max) );
	    }
	
	    public InitializeAcceleration( Vec2f min, Vec2f max )
	    {
		    acceleration.Set( min, max );
	    }
	
	    public InitializeAcceleration( float minX, float minY, float maxX, float maxY )
	    {
		    acceleration.Set( new Vec2f(minX, minY), new Vec2f(maxX, maxY) );
	    }

	    public void Initialize( Particle particle, ParticleEffect effect )
	    {
		    acceleration.Random( particle.Acceleration );
	    }
    }

    public class InitializeAccelerationScalar : ParticleInitialize
    {
	
	    private Rangef acceleration = new Rangef();
	    private bool relative;
	
	    public InitializeAccelerationScalar()
            : this( 0, true ) { }
	
	    public InitializeAccelerationScalar( float x, bool relative )
	    {
		    this.acceleration.Set( x );
		    this.relative = relative;
	    }
	
	    public InitializeAccelerationScalar( float min, float max, bool relative )
	    {
		    this.acceleration.Set( min, max );
		    this.relative = relative;
	    }
	
	    public void Initialize( Particle particle, ParticleEffect effect )
	    {
		    Vec2f v = particle.Velocity;
		    Vec2f a = particle.Acceleration;
		
		    a.Set( v );
		
		    if ( relative )
		    {
			    a.Scale( acceleration.Random() );
		    }
		    else
		    {
			    a.Length( acceleration.Random() );	
		    }
	    }
    }

    public class InitializeAngle : ParticleInitialize
    {
	    private Rangef angle = new Rangef();
	
	    public InitializeAngle()
	    {
	    }
	
	    public InitializeAngle( bool random )
	    {
		    if ( random )
		    {
			    angle.Set( 0, (float)(Math.PI * 2.0) );
		    }
	    }

	    public InitializeAngle( float x )
	    {
		    angle.Set( x );
	    }
	
	    public InitializeAngle( float min, float max )
	    {
		    angle.Set( min, max );
	    }
	
	    public void Initialize( Particle particle, ParticleEffect effect )
	    {
		    particle.Angle.Set( angle.Random() );
	    }
    }

    public class InitializeAngleAcceleration : ParticleInitialize
    {
    	private Rangef angleAcceleration = new Rangef();
    	
    	public InitializeAngleAcceleration()
    	{
    	}
    
    	public InitializeAngleAcceleration( float x )
    	{
		    angleAcceleration.Set( x );
	    }
    	
    	public InitializeAngleAcceleration( float min, float max )
    	{
		    angleAcceleration.Set( min, max );
	    }
    	
    	public void Initialize( Particle particle, ParticleEffect effect )
	    {   
       		particle.AngleAcceleration.Set( angleAcceleration.Random() );
    	}
    }

    public class InitializeAngleVelocity : ParticleInitialize
    {
	    private Rangef angleVelocity = new Rangef();
	
	    public InitializeAngleVelocity()
	    {
	    }

	    public InitializeAngleVelocity( float x )
	    {
		    angleVelocity.Set( x );
	    }
	
	    public InitializeAngleVelocity( float min, float max )
	    {
		    angleVelocity.Set( min, max );
	    }
	
	    public void Initialize( Particle particle, ParticleEffect effect )
	    {
		    particle.AngleVelocity.Set( angleVelocity.Random() );
	    }
    }

    public class InitializeColor : ParticleInitialize
    {
	    private Color[] colors;
	
	    public InitializeColor( params Color[] colors )
	    {
		    this.colors = colors;
	    }
	
	    public void Initialize( Particle particle, ParticleEffect effect )
	    {
		    particle.Shade.Set( Numbers.Random( colors, Color.White ) );
	    }
    }

    public class InitializeScale : ParticleInitialize
    {
	    private Range<Vector2> scale = new Range<Vector2>();
	
	    public InitializeScale()
	    {
	    }
	
	    public InitializeScale( float min, float max )
	    {
		    scale.Set( new Vec2f(min), new Vec2f(max) );
	    }
	
	    public InitializeScale( Vec2f min, Vec2f max )
	    {
		    scale.Set( min, max );
	    }
	
	    public InitializeScale( float minX, float minY, float maxX, float maxY )
	    {
		    scale.Set( new Vec2f(minX, minY), new Vec2f(maxX, maxY) );
	    }

	    public void Initialize( Particle particle, ParticleEffect effect )
	    {
		    scale.Random( particle.Scale );
	    }
    }

    public class InitializeScaleAcceleration : ParticleInitialize
    {
        private Range<Vector2> scale = new Range<Vector2>();

        public InitializeScaleAcceleration()
        {
        }

        public InitializeScaleAcceleration(float min, float max)
        {
            scale.Set(new Vec2f(min), new Vec2f(max));
        }

        public InitializeScaleAcceleration(Vec2f min, Vec2f max)
        {
            scale.Set(min, max);
        }

        public InitializeScaleAcceleration(float minX, float minY, float maxX, float maxY)
        {
            scale.Set(new Vec2f(minX, minY), new Vec2f(maxX, maxY));
        }

        public void Initialize(Particle particle, ParticleEffect effect)
        {
            scale.Random(particle.ScaleAcceleration);
        }
    }

    public class InitializeScaleVelocity : ParticleInitialize
    {
        private Range<Vector2> scale = new Range<Vector2>();

        public InitializeScaleVelocity()
        {
        }

        public InitializeScaleVelocity(float min, float max)
        {
            scale.Set(new Vec2f(min), new Vec2f(max));
        }

        public InitializeScaleVelocity(Vec2f min, Vec2f max)
        {
            scale.Set(min, max);
        }

        public InitializeScaleVelocity(float minX, float minY, float maxX, float maxY)
        {
            scale.Set(new Vec2f(minX, minY), new Vec2f(maxX, maxY));
        }

        public void Initialize(Particle particle, ParticleEffect effect)
        {
            scale.Random(particle.ScaleVelocity);
        }
    }

    public class InitializeSize : ParticleInitialize
    {
        private Range<Vector2> size = new Range<Vector2>();

        public InitializeSize()
        {
        }

        public InitializeSize(float x)
        {
            size.Set(new Vec2f(x), new Vec2f(x));
        }

        public InitializeSize(float min, float max)
        {
            size.Set(new Vec2f(min), new Vec2f(max));
        }

        public InitializeSize(Vec2f x)
        {
            size.Set(x);
        }

        public InitializeSize(Vec2f min, Vec2f max)
        {
            size.Set(min, max);
        }

        public InitializeSize(float minX, float minY, float maxX, float maxY)
        {
            size.Set(new Vec2f(minX, minY), new Vec2f(maxX, maxY));
        }

        public void Initialize(Particle particle, ParticleEffect effect)
        {
            size.Random(particle.Size);
        }
    }

    public class InitializeTile : ParticleInitialize
    {
        private Tile[] tiles;

        public InitializeTile(params Tile[] tiles)
	    {
		    this.tiles = tiles;
	    }
	
	    public void Initialize( Particle particle, ParticleEffect effect )
	    {
		    particle.Tile.Set( Numbers.Random( tiles, null ) );
	    }
    }

    public class VelocityDefault : ParticleVelocity 
    {
	    public VelocityDefault()
	    {
	    }
	    public void NewVelocity( Particle p, Vec2f direction, ParticleEffect effect ) 
	    {
		    p.Velocity.Set(0, 0);
	    }
    }

    public class VelocityDirectional : ParticleVelocity 
    {
	    private Rangef angles = new Rangef( 0 );
	    private Rangef speeds = new Rangef( 0 );

	    public VelocityDirectional()
	    {
	    }
	
	    public VelocityDirectional( Rangef angles, Rangef speeds )
	    {
		    this.angles.Set( angles );
		    this.speeds.Set( speeds );
	    }

	    public VelocityDirectional( float angle, float speed )
	    {
		    this.angles.Set( angle );
		    this.speeds.Set( speed );
	    }
	
	    public VelocityDirectional( float angleMin, float angleMax, float speedMin, float speedMax )
	    {
		    this.angles.Set( angleMin, angleMax );
		    this.speeds.Set( speedMin, speedMax );
	    }
	
	    public void NewVelocity( Particle p, Vec2f direction, ParticleEffect effect ) 
	    {
		    float angle = angles.Random();
		    float speed = speeds.Random();
		
		    p.Velocity.Angle( angle, speed );
	    }
    }

    public class VelocityOrtho : ParticleVelocity 
    {
	    private Range<Vector2> velocity = new Range<Vector2>();

	    public VelocityOrtho()
	    {
	    }
	
	    public VelocityOrtho( Range<Vector2> velocity )
	    {
		    this.velocity = velocity;
	    }
	
	    public VelocityOrtho( Vec2f min, Vec2f max )
	    {
		    this.velocity.Set( min, max );
	    }
	
	    public VelocityOrtho( Vec2f velocity )
	    {
		    this.velocity.Set( velocity, velocity );
	    }
	
	    public VelocityOrtho( float x, float y )
	    {
		    this.velocity.Set( new Vec2f(x, y), new Vec2f(x, y) );
	    }
	
	    public VelocityOrtho( float minX, float maxX, float minY, float maxY )
	    {
		    this.velocity.Set( new Vec2f(minX, minY), new Vec2f(maxX, maxY) );
	    }
	
	    public void NewVelocity( Particle p, Vec2f direction, ParticleEffect effect ) 
	    {
		    velocity.Random( p.Velocity );
	    }
    }

    public class VelocityOutward : ParticleVelocity 
    {
	    private Rangef speeds = new Rangef( 0 );

	    public VelocityOutward()
	    {
	    }
	
	    public VelocityOutward( Rangef speeds )
	    {
		    this.speeds.Set( speeds );
	    }

	    public VelocityOutward( float speed )
	    {
		    this.speeds.Set( speed );
	    }
	
	    public VelocityOutward( float speedMin, float speedMax )
	    {
		    this.speeds.Set( speedMin, speedMax );
	    }
	
	    public void NewVelocity( Particle p, Vec2f direction, ParticleEffect effect ) 
	    {
		    p.Velocity.Set(direction);
            p.Velocity.Scale(speeds.Random());
	    }
    }

    public class VelocityTowards : ParticleVelocity 
    {
	    private Rangef speeds = new Rangef( 0 );
	    private Vec2f target = new Vec2f();

	    public VelocityTowards()
	    {
	    }
	
	    public VelocityTowards( Rangef speeds, Vec2f target )
	    {
		    this.speeds.Set( speeds );
		    this.target.Set( target );
	    }
	
	    public VelocityTowards( float speed, float x, float y )
	    {
		    this.speeds.Set( speed );
		    this.target.Set( x, y );
	    }
	
	    public VelocityTowards( float speedMin, float speedMax, float x, float y )
	    {
		    this.speeds.Set( speedMin, speedMax );
		    this.target.Set( x, y );
	    }
	
	    public void NewVelocity( Particle p, Vec2f direction, ParticleEffect effect ) 
	    {
		    Vec2f pos = p.Location;
		    Vec2f vel = p.Velocity;
		
		    vel.Set( pos );
		    vel.Add( target.Get(), -1 );
		    vel.Length( speeds.Random() );
	    }
    }

    public class VolumeBounds : ParticleVolume 
    {
	    private Rangef width = new Rangef(0);
	    private Rangef height = new Rangef(0);
	    private Vec2f direction = new Vec2f();
	
	    public VolumeBounds()
	    {
	    }

	    public VolumeBounds( Rangef width, Rangef height )
	    {
		    this.width.Set( width );
		    this.height.Set( height );
	    }

	    public VolumeBounds( float width, float height )
	    {
		    this.width.Set( width );
		    this.height.Set( height );
	    }

	    public VolumeBounds( float widthMin, float widthMax, float heightMin, float heightMax )
	    {
		    this.width.Set( widthMin, widthMax );
		    this.height.Set( heightMin, heightMax );
	    }
	
	    public Vec2f NewVolume( Particle p, ParticleEffect effect ) 
	    {
		    float w = width.Random() * Numbers.RandomSign();
		    float h = height.Random() * Numbers.RandomSign();

            if (Numbers.Random(2) == 1)
            {
                w = Numbers.Random(width.Min) * Numbers.RandomSign();
            }
            else
            {
                h = Numbers.Random(height.Min) * Numbers.RandomSign();
            }

		    Vec2f pos = p.Location;
		
		    pos.Set( w, h );
		    direction.Norm( w, h );

		    return direction;
	    }
    }

    public class VolumeDefault : ParticleVolume 
    {
	    private Rangef angles = new Rangef(0, (float)(Math.PI * 2));
	    private Vec2f direction = new Vec2f();

	    public VolumeDefault()
	    {
	    }

	    public VolumeDefault( Rangef angles )
	    {
		    this.angles.Set( angles );
	    }

	    public VolumeDefault( float angle )
	    {
		    this.angles.Set( angle );
	    }

	    public VolumeDefault( float angleMin, float angleMax )
	    {
		    this.angles.Set( angleMin, angleMax );
	    }
	
	    public Vec2f NewVolume( Particle p, ParticleEffect effect ) 
	    {
            p.Location.Set(0, 0);

            direction.Angle( angles.Random(), 1 );

		    return direction;
	    }
    }

    public class VolumeEllipse : ParticleVolume 
    {
	    private Rangef radius = new Rangef(0);
	    private Rangef angles = new Rangef(0, (float)(Math.PI * 2));
	    private Vec2f direction = new Vec2f();

	    public VolumeEllipse()
	    {
	    }
	
	    public VolumeEllipse( float radius )
	    {
		    this.radius.Set( radius );
	    }
	
	    public VolumeEllipse( Rangef radius )
	    {
		    this.radius.Set( radius );
	    }
	
	    public VolumeEllipse( float radiusMin, float radiusMax )
	    {
		    this.radius.Set( radiusMin, radiusMax );
	    }

	    public VolumeEllipse( Rangef radius, Rangef angles )
	    {
		    this.radius.Set( radius );
		    this.angles.Set( angles );
	    }

	    public VolumeEllipse( float radiusMin, float radiusMax, float angleMin, float angleMax )
	    {
		    this.radius.Set( radiusMin, radiusMax );
		    this.angles.Set( angleMin, angleMax );
	    }

	    public Vec2f NewVolume( Particle p, ParticleEffect effect ) 
	    {
		    Vec2f pos = p.Location;
		
		    direction.Angle(angles.Random(), 1);
		
            pos.Set( direction.Get() );
            pos.Scale( radius.Random() );
		
		    return direction;
	    }
    }

    public class VolumePath : ParticleVolume 
    {
        private Path<Vector2> path;
        private Vec2f direction = new Vec2f();
        private Vec2f temp0 = new Vec2f();
        private Vec2f temp1 = new Vec2f();

        public VolumePath()
        { 
        }

        public VolumePath(Path<Vector2> path)
        {
            this.path = path;
        }

        public Vec2f NewVolume(Particle p, ParticleEffect effect)
        {
            float x = Numbers.Random(1 - 0.01f);
            float y = x + 0.01f;

            path.Set(temp0, x);
            path.Set(temp1, y);

            direction.Norm(temp0.v.Y - temp1.v.Y, temp1.v.X - temp0.v.X);

            p.Location.Set(temp0);

            return direction;
        }
    }

    public class BaseParticle : Particle
    {
        protected Vec2f location = new Vec2f();
        protected Vec2f velocity = new Vec2f();
        protected Vec2f size = new Vec2f();
        protected float age;
        protected float lifetime;

        public virtual void Update(float dt)
        {
            age += dt;
            location.Add(velocity.Get(), dt);
        }

        public virtual void Reset(float lifetime)
        {
            this.age = 0;
            this.lifetime = lifetime;
        }

        public Vec2f Location { get { return location; } }
        public Vec2f Velocity { get { return velocity; } }
        public Vec2f Size { get { return size; } }
        public float Lifetime { get { return lifetime; } }
        public float Age { get { return age; } }
        public virtual Vec2f Acceleration { get { return null; } }
        public virtual Vec2f Scale { get { return null; } }
        public virtual Vec2f ScaleVelocity { get { return null; } }
        public virtual Vec2f ScaleAcceleration { get { return null; } }
        public virtual Scalarf Angle { get { return null; } }
        public virtual Scalarf AngleVelocity { get { return null; } }
        public virtual Scalarf AngleAcceleration { get { return null; } }
        public virtual Color4b Shade { get { return null; } }
        public virtual Tile Tile { get { return null; } }

        public static void Renderer(Particle[] particles, int particleCount, SpriteBatch spriteBatch, ParticleEffect effect)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, effect.Blend);
            Rectangle d = new Rectangle();

            for (int i = 0; i < particleCount; i++)
            {
                Particle p = particles[i];
                d.X = (int)(p.Location.v.X - (p.Size.v.X * effect.ParticleAnchor.X));
                d.Y = (int)(p.Location.v.Y - (p.Size.v.Y * effect.ParticleAnchor.Y));
                d.Width = (int)p.Size.v.X;
                d.Height = (int)p.Size.v.Y;

                spriteBatch.Draw(effect.Tile.Texture, d, effect.Tile.Source, Color.White);
            }
            spriteBatch.End();
        }

        public static BaseParticle Factory()
        {
            return new BaseParticle();
        }
    }

    public class ColorParticle : BaseParticle
    {
        protected Color4b shade = new Color4b(new Color());

        public override Color4b Shade { get { return shade; } }

        new public static void Renderer(Particle[] particles, int particleCount, SpriteBatch spriteBatch, ParticleEffect effect)
        {
            Camera camera = Camera.Get();
            camera.End(spriteBatch);
            camera.Begin(spriteBatch, SpriteSortMode.Immediate, effect.Blend);

            Rectangle d = new Rectangle();

            for (int i = 0; i < particleCount; i++)
            {
                Particle p = particles[i];
                d.X = (int)(p.Location.v.X - (p.Size.v.X * effect.ParticleAnchor.X));
                d.Y = (int)(p.Location.v.Y - (p.Size.v.Y * effect.ParticleAnchor.Y));
                d.Width = (int)p.Size.v.X;
                d.Height = (int)p.Size.v.Y;

                spriteBatch.Draw(effect.Tile.Texture, d, effect.Tile.Source, p.Shade.c);
            }

            camera.End(spriteBatch);
            camera.Begin(spriteBatch);
        }

        new public static ColorParticle Factory()
        {
            return new ColorParticle();
        }
    }

    public class FullParticle : BaseParticle
    {
        protected Vec2f acceleration = new Vec2f();
        protected Vec2f scale = new Vec2f();
        protected Vec2f scaleVelocity = new Vec2f();
        protected Vec2f scaleAcceleration = new Vec2f();
        protected Scalarf angle = new Scalarf();
        protected Scalarf angleVelocity = new Scalarf();
        protected Scalarf angleAcceleration = new Scalarf();
        protected Color4b shade = new Color4b(new Color());
        protected Tile tile = new Tile();

        public override void Update(float dt)
        {
            age += dt;

            velocity.Add(acceleration.Get(), dt);
            location.Add(velocity.Get(), dt);

            scaleVelocity.Add(scaleAcceleration.Get(), dt);
            scale.Add(scaleVelocity.Get(), dt);

            angleVelocity.Add(angleAcceleration.Get(), dt);
            angle.Add(angleVelocity.Get(), dt);
        }

        public override void Reset(float lifetime)
        {
            this.age = 0;
            this.lifetime = lifetime;
            this.velocity.Set(0, 0);
            this.acceleration.Set(0, 0);
            this.scale.Set(1, 1);
            this.scaleVelocity.Set(0, 0);
            this.scaleAcceleration.Set(0, 0);
            this.angle.Set(0);
            this.angleVelocity.Set(0);
            this.angleAcceleration.Set(0);
            this.shade.Set(new Color());
        }

        public override Vec2f Acceleration { get { return acceleration; } }
        public override Vec2f Scale { get { return scale; } }
        public override Vec2f ScaleVelocity { get { return scaleVelocity; } }
        public override Vec2f ScaleAcceleration { get { return scaleAcceleration; } }
        public override Scalarf Angle { get { return angle; } }
        public override Scalarf AngleVelocity { get { return angleVelocity; } }
        public override Scalarf AngleAcceleration { get { return angleAcceleration; } }
        public override Color4b Shade { get { return shade; } }
        public override Tile Tile { get { return tile; } }

        new public static void Renderer(Particle[] particles, int particleCount, SpriteBatch spriteBatch, ParticleEffect effect)
        {
            Camera camera = Camera.Get();
            camera.End(spriteBatch);
            camera.Begin(spriteBatch, SpriteSortMode.Immediate, effect.Blend);

            Vector2 offset = new Vector2();
            Vector2 scale = new Vector2();
            Vector2 position = effect.Position.v;

            for (int i = 0; i < particleCount; i++)
            {
                Particle p = particles[i];
                scale.X = (p.Size.v.X / p.Tile.Source.Width) * p.Scale.v.X;
                scale.Y = (p.Size.v.Y / p.Tile.Source.Height) * p.Scale.v.Y;
                offset.X = (effect.ParticleAnchor.X * p.Tile.Source.Width);
                offset.Y = (effect.ParticleAnchor.Y * p.Tile.Source.Height);

                spriteBatch.Draw(p.Tile.Texture, p.Location.v + position, p.Tile.Source, p.Shade.c, p.Angle.v, offset, scale, SpriteEffects.None, 0);
            }

            camera.End(spriteBatch);
            camera.Begin(spriteBatch);
        }

        new public static FullParticle Factory()
        {
            return new FullParticle();
        }
    }

    /// <summary>
    /// Utility class for array functions.
    /// </summary>
    public class Arrays
    {
        public static T[] Add<T>(T item, T[] array)
        {
            System.Array.Resize<T>(ref array, array.Length + 1);
            array[array.Length - 1] = item;
            return array;
        }
    }

    public class Pool<T>
    {

        public int size;
        public T[] stack;
        public Factory<T> factory;

        public Pool(T[] stack, Factory<T> factory)
        {
            this.stack = stack;
            this.factory = factory;
        }

        public T alloc()
        {
            return (size == 0 ? factory() : stack[--size]);
        }

        public void free(T item)
        {
            if (size < stack.Length)
            {
                stack[size++] = item;
            }
        }
    }

    /* SANDBOX * /

    public interface AxeController<T>
    {
        bool IsEqual(T a, T b);
        bool Copy(T from, T to);
        void Interpolate(T target, T start, T end, float delta);
        void Add(T target, T amount, float delta);
        void Mul(T target, T by);
        void Scale(T target, float amount);
        float Distance(T from, T to);
        T Clone(T subject);
        T Create();
    }
    public interface AxePath<T>
    {
        AxeController<T> Controller { get; }
        void Set(ref T subject, float delta);
    }
    
    public unsafe class AxeEvent<T> where T : struct
    {
        protected AxePath<T> path;
        protected float* subject;
        public AxeEvent(float* subject, AxePath<T> path)
        {
            this.subject = subject;
            this.path = path;
        }
    }

     /**/
    public interface Cloned<T>
    {
        T clone();
    }

}
