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

		private List<Body> bodies = new List<Body>();

		private List<Pair> pairs = new List<Pair>();
		private List<Pair> uniquePairs = new List<Pair>();

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

			if (Vector2.DistanceSquared(a.Position, b.Position) > r) {
				return false;
			}

			Vector2 n = b.Position - a.Position;

			float d = Vector2.Distance(a.Position, b.Position);


			if (d != 0) {
				m.Penetration = r - d;
				m.Normal = n / d;

				return true;
			} else {
				m.Penetration = r - d;
				m.Normal = new Vector2(1, 0);

				return true;
			}
		}

		bool Collide(ref Manifold m, AABB a, AABB b)
		{
			Vector2 n = b.Position - a.Position;

			float aExtent = (a.Max.X - a.Min.X) / 2;
			float bExtent = (b.Max.X - b.Min.X) / 2;

			float xOverlap = aExtent + bExtent - Math.Abs(n.X);

			if (xOverlap > 0) {
				aExtent = (a.Max.Y - a.Min.Y) / 2;
				bExtent = (b.Max.Y - b.Min.Y) / 2;

				float yOverlap = aExtent + bExtent - Math.Abs(n.Y);

				if (yOverlap > 0) {
					if (xOverlap > yOverlap) {
						if (n.X < 0) {
							m.Normal = new Vector2(-1, 0);
						} else {
							m.Normal = new Vector2(0, 0);
						}
						m.Penetration = xOverlap;
						return true;
					} else {
						// Point toward B knowing that n points from A to B
						if (n.Y < 0) {
							m.Normal = new Vector2(0, -1);
						} else {
							m.Normal = new Vector2(0, 1);
						}
						m.Penetration = yOverlap;
						return true;
					}
				}
			}
			return false;
		}

		bool Collide(Manifold m, AABB a, Circle b)
		{

			// Vector from A to B
			Vector2 n = b.Position - a.Position;

			// Closest point on A to center of B
			Vector2 closest = n;

			// Calculate half extents along each axis
			float x_extent = (a.Max.X - a.Min.X) / 2f;
			float y_extent = (a.Max.Y - a.Min.Y) / 2f;

			// Clamp point to edges of the AABB

			closest = Vector2.Clamp(closest, new Vector2(-x_extent, -y_extent), new Vector2(x_extent, y_extent));

			bool inside = false;

			// Circle is inside the AABB, so we need to clamp the circle's center
			// to the closest edge
			if (n == closest) {
				inside = true;

				// Find closest axis
				if (Math.Abs(n.X) > Math.Abs(n.Y)) {
					// Clamp to closest extent
					if (closest.X > 0)
						closest.X = x_extent;
					else
						closest.X = -x_extent;
				}

				// y axis is shorter
				else {
					// Clamp to closest extent
					if (closest.Y > 0)
						closest.Y = y_extent;
					else
						closest.Y = -y_extent;
				}
			}

			Vector2 normal = n - closest;
			double d = normal.LengthSquared();
			float r = b.Radius;

			// Early out of the radius is shorter than distance to closest point and
			// Circle not inside the AABB
			if (d > r * r && !inside)
				return false;

			// Avoided sqrt until we needed
			d = Math.Sqrt(d);

			// Collision normal needs to be flipped to point outside if circle was
			// inside the AABB
			if (inside) {
				m.Normal = -n;
				m.Penetration = r + (float)d;
			} else {
				m.Normal = n;
				m.Penetration = r + (float)d;
			}

			return true;
		}

		public void Update(GameTime gameTime)
		{
			
		}
		private void BroadPhase()
		{
			AABB aAabb;
			AABB bAabb;

			foreach (Body i in bodies) {
				foreach (Body j in bodies) {
					if (i.Equals(j))
						continue;

					i.ComputeAabb(aAabb);
					j.ComputeAabb(bAabb);

					if (AABBtoAABB(aAabb, bAabb)) {
						pairs.Add(new Pair(i, j));

					}
				}
			}

			pairs.Sort();
			int i = 0;
			while (i < pairs.Count) {
				Pair p = pairs[i];

			}
		}
	}
}
