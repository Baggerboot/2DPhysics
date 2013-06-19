using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace _2DPhysics
{
	class World
	{
		const float posCorrPercent = 0.2f; // usually 20% to 80%
		const float posCorrSlop = 0.01f; // usually 0.01 to 0.1

		private List<Circle> circles = new List<Circle>();

		private void ResolveCollision(PhysicsObject a, PhysicsObject b, Vector2 normal, float pDepth)
		{
			Vector2 rv = b.Velocity - a.Velocity;

			float velocityAlongNormal = Vector2.Dot(rv, normal);
			if (velocityAlongNormal > 0)
				return;


			float e = Math.Min(a.Restitution, b.Restitution);

			float j = -(1 + e) * velocityAlongNormal;
			j /= (1 / a.Mass + 1 / b.Mass);

			Vector2 impulse = j * normal;
			a.Velocity -= a.InvMass * impulse;
			b.Velocity += b.InvMass * impulse;
		}

		private void PositionalCorrection(PhysicsObject A, PhysicsObject B, float pDepth, Vector2 normal)
		{
		  Vector2 correction = Vector2.Multiply(normal, (Math.Max(pDepth - posCorrSlop, 0.0f) / (A.InvMass + B.InvMass)) * posCorrPercent);

		  A.Position -= A.InvMass * correction;
		  B.Position += B.InvMass * correction;
		}

		bool Collide(ref Manifold m, Circle a, Circle b)
		{
			float r = a.Radius + b.Radius;
			r *= r;

			if(Vector2.DistanceSquared(a.Position, b.Position) > r)
			{
				return false;
			}

			Vector2 n = b.Position - a.Position;

			float d = Vector2.Distance(a.Position, b.Position);


			if(d != 0)
			{
				m.Penetration = r - d;
				m.Normal = n / d;

				return true;
			}else{
				m.Penetration = r - d;
				m.Normal = new Vector2(1,0);

				return true;
			}
		}

		bool Collide(ref Manifold m, AABB a, AABB b)
		{
			Vector2 n = b.Position - a.Position;

			float aExtent = (a.Max.X - a.Min.X) / 2;
			float bExtent = (b.Max.X - b.Min.X) / 2;

			float xOverlap = aExtent + bExtent - Math.Abs(n.X);

			return true;
		}

		public void Update()
		{


			foreach (Circle c in circles) {
				c.Position += c.Velocity;
			}
		}
	}
}
