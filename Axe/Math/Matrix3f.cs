using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe
{
    public class Matrix3f
    {
        /*public static float EPSILON = 0.0000001f;
	
        public static FloatBuffer buffer = Buffers.floats( 
            1.0f, 0.0f, 0.0f, 0.0f, 
            0.0f, 1.0f, 0.0f, 0.0f,
            0.0f, 0.0f, 1.0f, 0.0f, 
            0.0f, 0.0f, 0.0f, 1.0f
        );
	
        public float m00, m01, m02;
        public float m10, m11, m12;
        public float m20, m21, m22;
        public float tx, ty, tz;

        public Matrix3f() 
        {
            reset();
        }
	
        public void reset() 
        {
            m00 = m11 = m22 = 1.0f;
            m01 = m02 = m10 = m12 = m20 = m21 = 0.0f;
            tx = ty = tz = 0.0f;
        }


        public void setModelView()
        {
            GL11.glGetFloat( GL11.GL_MODELVIEW_MATRIX, buffer );
            set( buffer );
            buffer.clear();
        }
	
        public void setProjection()
        {
            GL11.glGetFloat( GL11.GL_PROJECTION_MATRIX, buffer );
            set( buffer );
            buffer.clear();
        }
	
        public void set( FloatBuffer buffer )
        {
            m00 = buffer.get( 0 );
            m10 = buffer.get( 1 );
            m20 = buffer.get( 2 );
            m01 = buffer.get( 4 );
            m11 = buffer.get( 5 );
            m21 = buffer.get( 6 );
            m02 = buffer.get( 8 );
            m12 = buffer.get( 9 );
            m22 = buffer.get( 10 );
            tx = buffer.get( 12 );
            ty = buffer.get( 13 );
            tz = buffer.get( 14 );
        }
	
        public void set(Matrix3f m)
        {
            m00 = m.m00;
            m01 = m.m01;
            m02 = m.m02;
            m10 = m.m10;
            m11 = m.m11;
            m12 = m.m12;
            m20 = m.m20;
            m21 = m.m21;
            m22 = m.m22;
            tx = m.tx;
            ty = m.ty;
            tz = m.tz;
        }
	
        public Vec3f transform(Vec3f p, Vec3f out) 
        {
            out.x = (p.x * m00) + (p.y * m01) + (p.z * m02) + tx;
            out.y = (p.x * m10) + (p.y * m11) + (p.z * m12) + ty;
            out.z = (p.x * m20) + (p.y * m21) + (p.z * m22) + tz;
            return out;
        }
	
        public Vec3f transform(Vec3f p) 
        {
            float px = p.x;
            float py = p.y;
            float pz = p.z;
		
            p.x = (px * m00) + (py * m01) + (pz * m02) + tx;
            p.y = (px * m10) + (py * m11) + (pz * m12) + ty;
            p.z = (px * m20) + (py * m21) + (pz * m22) + tz;
		
            return p;
        }
	
        public void getX(Vec3f p) {
            p.x = m00;
            p.y = m10;
            p.z = m20;
        }
	
        public void getY(Vec3f p) {
            p.x = m01;
            p.y = m11;
            p.z = m21;
        }
	
        public void getZ(Vec3f p) {
            p.x = m02;
            p.y = m12;
            p.z = m22;
        }

        public void setEulerAngles(float yaw, float pitch, float roll) {
            float sinX = (float)Math.sin(pitch);
            float cosX = (float)Math.cos(pitch);
            float sinY = (float)Math.sin(yaw);
            float cosY = (float)Math.cos(yaw);
            float sinZ = (float)Math.sin(roll);
            float cosZ = (float)Math.cos(roll);
            m00 = (cosZ * cosY);
            m01 = (cosZ * -sinY * -sinX + sinZ * cosX);
            m02 = (cosZ * -sinY * cosX + sinZ * sinX);
            m10 = (-sinZ * cosY);
            m11 = (-sinZ * -sinY * -sinX + cosZ * cosX);
            m12 = (-sinZ * -sinY * cosX + cosZ * sinX);
            m20 = sinY;
            m21 = cosY * -sinX;
            m22 = cosY * cosX;
        }

        public void setEulerAngles(float yaw, float pitch) {
            float sinX = (float)Math.sin(pitch);
            float cosX = (float)Math.cos(pitch);
            float sinY = (float)Math.sin(yaw);
            float cosY = (float)Math.cos(yaw);
            m00 = cosY;
            m01 = sinY * sinX;
            m02 = -sinY * cosX;
            m10 = 0.0f;
            m11 = cosX;
            m12 = sinX;
            m20 = sinY;
            m21 = cosY * -sinX;
            m22 = cosY * cosX;
        }
	
        public Matrix3f rotate(double theta, Vec3f v) 
        {
            return rotate( theta, v.x, v.y, v.z );
        }
	
        public Matrix3f rotate(double theta, float vx, float vy, float vz) 
        {
            // Dot product of the rotation vector
            float dot = (vx * vx) + (vy * vy) + (vz * vz);
		
            // If the vector isn't normalized then normalize it.
            if (!equal(dot, 1.0f)) {
                float invlength = 1.0f / (float)Math.sqrt(dot);
                vx *= invlength;
                vy *= invlength;
                vz *= invlength;
            }
		
            // Cache values used in quaternion matrix calculation
            float cos, sin, mcos, vx_mcos, vy_mcos, vz_mcos, vx_sin, vy_sin, vz_sin;
            cos = (float)Math.cos(theta);
            sin = (float)Math.sin(theta);
            mcos = 1.0f - cos;
            vx_mcos = vx * mcos;
            vy_mcos = vy * mcos;
            vz_mcos = vz * mcos;
            vx_sin = vx * sin;
            vy_sin = vy * sin;
            vz_sin = vz * sin;

            // Copy current matrix state into A
            float a00, a01, a02, a10, a11, a12, a20, a21, a22;
            a00 = m00; a01 = m01; a02 = m02;
            a10 = m10; a11 = m11; a12 = m12;
            a20 = m20; a21 = m21; a22 = m22;
		
            // Derived quaternion matrix into B
            float b00, b01, b02, b10, b11, b12, b20, b21, b22;
            b00 = vx * vx_mcos + cos;
            b10 = vy * vx_mcos + vz_sin;
            b20 = vz * vx_mcos - vy_sin;
            b01 = vx * vy_mcos - vz_sin;
            b11 = vy * vy_mcos + cos;
            b21 = vz * vy_mcos + vx_sin;
            b02 = vx * vz_mcos + vy_sin;
            b12 = vy * vz_mcos + vx_sin;
            b22 = vz * vz_mcos + cos;
		
            // Matrix multiplication (A x B)
            m00 = (a00 * b00) + (a01 * b10) + (a02 * b20);
            m10 = (a10 * b00) + (a11 * b10) + (a12 * b20);
            m20 = (a20 * b00) + (a21 * b10) + (a22 * b20);
            m01 = (a00 * b01) + (a01 * b11) + (a02 * b21);
            m11 = (a10 * b01) + (a11 * b11) + (a12 * b21);
            m21 = (a20 * b01) + (a21 * b11) + (a22 * b21);
            m02 = (a00 * b02) + (a01 * b12) + (a02 * b22);
            m12 = (a10 * b02) + (a11 * b12) + (a12 * b22);
            m22 = (a20 * b02) + (a21 * b12) + (a22 * b22);
		
            return this;
        }

        public Matrix3f rotatex(double pitch) {
            if ( pitch != 0 ) {
                float c = (float)Math.cos(pitch);
                float s = (float)Math.sin(pitch);
                float a01 = m01, a11 = m11, a21 = m21;
                m01 = (a01 * c) + (m02 * s);
                m02 = (m02 * c) - (a01 * s);
                m11 = (a11 * c) + (m12 * s);
                m12 = (m12 * c) - (a11 * s);
                m21 = (a21 * c) + (m22 * s);
                m22 = (m22 * c) - (a21 * s);	
            }
            return this;
        }
	
        public Matrix3f rotatey(double yaw) {
            if ( yaw != 0 ) {
                float c = (float)Math.cos(yaw);
                float s = (float)Math.sin(yaw);
                float a00 = m00, a10 = m10, a20 = m20;
                m00 = (a00 * c) - (m02 * s);
                m02 = (m02 * c) + (a00 * s);
                m10 = (a10 * c) - (m12 * s);
                m12 = (m12 * c) + (a10 * s);
                m20 = (a20 * c) - (m22 * s);
                m22 = (m22 * c) + (a20 * s);
            }
            return this;
        }
	
        public Matrix3f rotatez(double roll) {
            if ( roll != 0 ) {
                float c = (float)Math.cos(roll);
                float s = (float)Math.sin(roll);
                float a00 = m00, a10 = m10, a20 = m20;
                m00 = (a00 * c) + (m01 * s);
                m01 = (m01 * c) - (a00 * s);
                m10 = (a10 * c) + (m11 * s);
                m11 = (m11 * c) - (a10 * s);
                m20 = (a20 * c) + (m21 * s);
                m21 = (m21 * c) - (a20 * s);	
            }
            return this;
        }

        public Matrix3f translate(Vec3f v) {
            return translate( v.x, v.y, v.z );
        }
	
        public Matrix3f translate(float dx, float dy, float dz) {
            tx += (m00 * dx) + (m01 * dy) + (m02 * dz);
            ty += (m10 * dx) + (m11 * dy) + (m12 * dz);
            tz += (m20 * dx) + (m21 * dy) + (m22 * dz);
            return this;
        }
	
        public Matrix3f translateBefore(Vec3f v) {
            return translateBefore( v.x, v.y, v.z );
        }
	
        public Matrix3f translateBefore(float dx, float dy, float dz) {
            tx += dx;
            ty += dy;
            tz += dz;
            return this;
        }
	
        public Matrix3f setTranslate(Vec3f v) {
            return setTranslate( v.x, v.y, v.z );
        }
	
        public Matrix3f setTranslate(float x, float y, float z) {
            reset();
            tx = x;
            ty = y;
            tz = z;
            return this;
        }
	
        public Matrix3f scale(Vec3f v) {
            return scale(v.x, v.y, v.z);
        }
	
        public Matrix3f scale(float sx, float sy, float sz) {
            m00 *= sx;  m01 *= sy;  m02 *= sz;
            m10 *= sx;  m11 *= sy;  m12 *= sz;
            m20 *= sx;  m21 *= sy;  m22 *= sz;
            return this;
        }
	
        public Matrix3f setScale(Vec3f v) {
            return setScale( v.x, v.y, v.z );
        }
	
        public Matrix3f setScale(float sx, float sy, float sz) {
            reset();
            m00 = sx;
            m11 = sy;
            m22 = sz;
            return this;
        }
	
        public Matrix3f shearxy(float hx, float hy) {
            m02 += (m00 * hx) + (m01 * hy);
            m12 += (m10 * hx) + (m11 * hy);
            m22 += (m20 * hx) + (m21 * hy);
            return this;
        }
	
        public Matrix3f shearxz(float hx, float hz) {
            m01 += (m00 * hx) + (m02 * hz);
            m11 += (m10 * hx) + (m12 * hz);
            m21 += (m20 * hx) + (m22 * hz);
            return this;
        }
	
        public Matrix3f shearyz(float hy, float hz) {
            m00 += (m01 * hy) + (m02 * hz);
            m10 += (m11 * hy) + (m12 * hz);
            m20 += (m21 * hy) + (m22 * hz);
            return this;
        }
	
        public void multiply(Matrix3f b) 
        {
            float am00 = m00;
            float am01 = m01;
            float am02 = m02;
            float am10 = m10;
            float am11 = m11;
            float am12 = m12;
            float am20 = m20;
            float am21 = m21;
            float am22 = m22;
            float atx = tx;
            float aty = ty;
            float atz = tz;
		
            m00 = (am00 * b.m00) + (am01 * b.m10) + (am02 * b.m20);
            m01 = (am00 * b.m01) + (am01 * b.m11) + (am02 * b.m21);
            m02 = (am00 * b.m02) + (am02 * b.m12) + (am02 * b.m22);
            tx = (am00 * b.tx) + (am01 * b.ty) + (am02 * b.tz) + atx;
		
            m10 = (am10 * b.m00) + (am11 * b.m10) + (am12 * b.m20);
            m11 = (am10 * b.m01) + (am11 * b.m11) + (am12 * b.m21);
            m12 = (am10 * b.m02) + (am11 * b.m12) + (am12 * b.m22);
            ty = (am10 * b.tx) + (am11 * b.ty) + (am12 * b.tz) + aty;
		
            m20 = (am20 * b.m00) + (am21 * b.m10) + (am22 * b.m20);
            m21 = (am20 * b.m01) + (am21 * b.m11) + (am22 * b.m21);
            m22 = (am20 * b.m02) + (am21 * b.m12) + (am22 * b.m22);
            tz = (am20 * b.tx) + (am21 * b.ty) + (am22 * b.tz) + atz;
        }
	
        private boolean equal(float a, float b) {
            return Math.abs(a - b) < EPSILON;
        }
	
        public static Matrix3f multiply(Matrix3f a, Matrix3f b, Matrix3f out) 
        {
            out.m00 = (a.m00 * b.m00) + (a.m01 * b.m10) + (a.m02 * b.m20);
            out.m01 = (a.m00 * b.m01) + (a.m01 * b.m11) + (a.m02 * b.m21);
            out.m02 = (a.m00 * b.m02) + (a.m02 * b.m12) + (a.m02 * b.m22);
            out.tx = (a.m00 * b.tx) + (a.m01 * b.ty) + (a.m02 * b.tz) + a.tx;
		
            out.m10 = (a.m10 * b.m00) + (a.m11 * b.m10) + (a.m12 * b.m20);
            out.m11 = (a.m10 * b.m01) + (a.m11 * b.m11) + (a.m12 * b.m21);
            out.m12 = (a.m10 * b.m02) + (a.m11 * b.m12) + (a.m12 * b.m22);
            out.ty = (a.m10 * b.tx) + (a.m11 * b.ty) + (a.m12 * b.tz) + a.ty;
		
            out.m20 = (a.m20 * b.m00) + (a.m21 * b.m10) + (a.m22 * b.m20);
            out.m21 = (a.m20 * b.m01) + (a.m21 * b.m11) + (a.m22 * b.m21);
            out.m22 = (a.m20 * b.m02) + (a.m21 * b.m12) + (a.m22 * b.m22);
            out.tz = (a.m20 * b.tx) + (a.m21 * b.ty) + (a.m22 * b.tz) + a.tz;
		
            return out;
        }
	
        public static Matrix3f multiply(Matrix3f a, Matrix3f b) 
        {
            Matrix3f out = new Matrix3f();
            multiply( a, b, out );
            return out;
        }*/

    }

}
