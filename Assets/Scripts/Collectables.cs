using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AddLifeMatch3
{
	[System.Serializable, InlineProperty()]
	public struct Collectables
	{
		[ShowInInspector, HorizontalGroup]
		public Vector2 position { get; private set; }

		public GameObject gameObject { get; private set; }

		public bool selected { get; private set; }

		public Collectables(Vector2 p, GameObject g) {
			position = p;
			gameObject = g;
			selected = false;
		}
		public void SetGameObject(GameObject g) {
			gameObject = g;
		}

		public void SetSelected(bool s) {
			selected = s;
		}

		public void SetPosition(Vector2 p) {
			position = p;
		}

		public GameObject GetGameObject() => gameObject;
		public Vector2 GetPosition() => position;
		public override bool Equals(object other)
		{
			if (other == null) return false;
			// Optimization for a common success case.
			if (System.Object.ReferenceEquals(this, other))
			{
				return true;
			}
			if (this.GetType() != other.GetType()) return false;

			var obj = (Collectables)other;

			return (position == obj.position) && 
				   (gameObject == obj.gameObject);
		}


		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			string output = string.Format("{0}, {1}, {2}", gameObject.name, position, selected);
			return output;
		}

		public static bool operator ==(Collectables rhs, Collectables lhs) => rhs.Equals(lhs);
		public static bool operator !=(Collectables rhs, Collectables lhs) => !(rhs == lhs);

		public static bool operator >(Collectables rhs, Collectables lhs) => (rhs.position.x > lhs.position.x && rhs.position.y >= lhs.position.y);
		public static bool operator <(Collectables rhs, Collectables lhs) => (rhs.position.x < lhs.position.x && rhs.position.y <= lhs.position.y);

	}
}
