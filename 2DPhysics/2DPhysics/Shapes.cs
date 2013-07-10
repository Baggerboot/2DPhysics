using Microsoft.Xna.Framework;

namespace _2DPhysics
{
	/// <summary>
	/// Defines an Axis-Aligned Bounding Box
	/// </summary>
	public class AABB : PhysicsObject
	{
		public Vector2 Min
		{
			get;
			private set;
		}
		public Vector2 Max
		{
			get;
			private set;
		}
		public override Vector2 Position
		{
			get
			{
				Vector2 center = Min + ((Min - Max) * 2);
				return center;
			}
			set
			{
				Vector2 prevCenter = Position;
				Vector2 delta = value - Position;
				Min += delta;
				Max += delta;
			}
		}

		public AABB(Vector2 min, Vector2 max, Vector2 velocity, float mass, float restitution)
			:base(velocity, mass, restitution)
		{
			Min = min;
			Max = max;
		}

		static bool Collide(AABB a, AABB b)
		{
			// If either bounding box is separated along any axis, we can be sure that there is no collision
			if (a.Max.X < b.Min.X || a.Min.X > b.Max.X) return false;
			if (a.Max.Y < b.Min.Y || a.Min.Y > b.Max.Y) return false;

			return true;
		}
	}

	public class Circle : PhysicsObject
	{
		public float Radius
		{
			get;
			private set;
		}
		public override Vector2 Position
		{
			get;
			set;
		}

		public Circle(float radius, Vector2 position, Vector2 velocity, float mass, float restitution)
			:base(velocity, mass, restitution)
		{
			Radius = radius;
			Position = position;
		}

		static bool Collide(Circle a, Circle b)
		{
			float r = a.Radius + b.Radius;
			r *= r;
			return r < (FastMath.Square(a.Position.X - b.Position.X) + FastMath.Square(a.Position.Y - b.Position.Y));
		}
	}

	/// <summary>
	/// Contains information about a collision between two objects
	/// </summary>
	public struct Manifold
	{
		public PhysicsObject A;
		public PhysicsObject B;
		public float Penetration;
		public Vector2 Normal;

		public Manifold(PhysicsObject a, PhysicsObject b, float penetration, Vector2 normal)
		{
			A = a;
			B = b;
			Penetration = penetration;
			Normal = normal;
		}
	}

	public struct Body
	{
		ref Shape Shape;
		Transform Tx;
		Material Material;
		MassData MassData;
		Vector2 Velocity;
		Vector2 Force;
		double GravityScale;
	}

	public struct MassData
	{
		float Mass;
		float InvMass;
		float Inertia;
		float InverseInertia;
	}

	public struct Material
	{
		float Density;
		float Restitution;

		public Material(float density, float restitution)
		{
			Density = density;
			Restitution = restitution;
		}

		static const Material ROCK = new Material(0.6f, 0.1f);
		static const Material WOOD = new Material(0.3f, 0.2f);
		static const Material METAL = new Material(1.2f, 0.05f);
		static const Material BOUNCY_BALL = new Material(0.3f, 0.8f);
		static const Material SUPER_BALL = new Material(0.3f, 0.95f);
		static const Material PILLOW = new Material(0.1f, 0.2f);
		static const Material STATIC = new Material(0.0f, 0.4f);
	}

	public struct Pair
	{
		object A;
		object B;

		public Pair(object a, object b)
		{
			A = a;
			B = b;
		}
	}
}