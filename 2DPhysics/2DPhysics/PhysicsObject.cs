using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
namespace _2DPhysics
{
	public abstract class PhysicsObject
	{
		public Vector2 Velocity
		{
			get;
			set;
		}

		private float mass;
		public float Mass
		{
			get
			{
				return mass;
			}
			private set
			{
				mass = value;
				if (value == 0) {
					InvMass = 0;
				} else {
					InvMass = 1f / value;
				}
			}
		}

		public float InvMass
		{
			get;
			private set;
		}

		public float Restitution
		{
			get;
			private set;
		}

		public abstract Vector2 Position
		{
			get;
			set;
		}

		public PhysicsObject(Vector2 velocity, float mass, float restitution)
		{
			Velocity = velocity;
			Restitution = restitution;
			Mass = mass;
		}
	}
}
