using UnityEngine;
using Sirenix.OdinInspector;

namespace AddLifeMatch3
{
	[System.Serializable, InlineProperty()]
	public struct Vector2I
	{
		[HorizontalGroup, LabelText("width:"), LabelWidth(40)]
		public int x;

		[HorizontalGroup, LabelText("hieght:"), LabelWidth(40)]
		public int y;
       

        public Vector2I(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
		//convert to Vector2
		public static implicit operator Vector2(Vector2I v)
		{
			return new Vector2(v.x, v.y);
		}
		//convert to Vector2Int
		public static implicit operator Vector2Int(Vector2I v)
		{
			return new Vector2Int(v.x, v.y);
		}
		public static implicit operator Vector2I(Vector2Int v)
		{
			return new Vector2I(v.x, v.y);
		}
		public static implicit operator Vector2I(Vector2 v)
		{
			return new Vector2I((int)v.x, (int)v.y);
		}
		// magnitude of vector
		public readonly float magnitude { 
			get { 
				Vector2 v2 = this;
				return v2.magnitude;
			} 
		}
	}
}
