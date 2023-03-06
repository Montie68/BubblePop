using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AddLifeMatch3
{
	public static class Extentions
	{

		/// <summary>
		///  Get attched Collectable Componet on the game object.
		///  WARNING: make sure the GameObject has a Collectable Componet attached
		/// </summary>
		/// <param name="gameObject"></param>
		/// <returns></returns>
		public static Collectable Collected(this GameObject gameObject)
		{
			try
			{
				return gameObject.GetComponent<Collectable>();
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				throw new ArgumentNullException();
			}
		}

		public static List<Collectables> SortCollectables(this List<Collectables> collectables)
		{
			// bubble sort the list by the position x first and then by the position y
			for (int i = 0; i < collectables.Count; i++)
			{
				for (int j = 0; j < collectables.Count - 1; j++)
				{
					if (collectables[j].position.x > collectables[j + 1].position.x)
					{
						var temp = collectables[j];
						collectables[j] = collectables[j + 1];
						collectables[j + 1] = temp;
					}
					else if (collectables[j].position.x == collectables[j + 1].position.x)
					{
						if (collectables[j].position.y > collectables[j + 1].position.y)
						{
							var temp = collectables[j];
							collectables[j] = collectables[j + 1];
							collectables[j + 1] = temp;
						}
					}
				}
			}

			return collectables;
		}
		public static T GC<T>(this GameObject gameObject)
		{
			try
			{
				return gameObject.GetComponent<T>();
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				throw new ArgumentNullException();
			}
		}
		public static List<T> GCsC<T>(this GameObject gameObject)
		{
			try
			{
				var componets = gameObject.GetComponentsInChildren<T>().ToList();
				if (gameObject.GetComponent<T>() != null)
				{
					componets.RemoveFirst();
				}
				return componets;
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				throw new ArgumentNullException();
			}
		}
		public static T GCC<T>(this GameObject gameObject)
		{
			return gameObject.GCsC<T>().FirstElement();
		}
		public static T FirstElement<T>(this List<T> list)
		{
			try
			{
				return list[0];
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				throw new ArgumentNullException();
			}
		}
		public static T LastElement<T>(this List<T> list)
		{
			try
			{
				return list[list.Count - 1];
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				throw new ArgumentNullException();
			}
		}
		public static T SecondLastElement<T>(this List<T> list)
		{
			try
			{
				return list[list.Count - 2];
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				throw new ArgumentNullException();
			}
		}
		public static void RemoveLast<T>(this List<T> list)
		{
			try
			{
				list.RemoveAt(list.LastIndex());
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				throw new ArgumentNullException();
			}
		}
		public static void RemoveFirst<T>(this List<T> list)
		{
			try
			{
				list.RemoveAt(0);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				throw new ArgumentNullException();
			}
		}
		public static int LastIndex<T>(this List<T> list)
		{
			try
			{
				return list.Count - 1;
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				throw new ArgumentNullException();
			}
		}


		public static object LastElement(this object[] list)
		{
			try
			{
				return list[list.Length - 1];
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				throw new ArgumentNullException();
			}
		}
		public static object FirstElement(this object[] list)

		{
			try
			{
				return list[0];
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				throw new ArgumentNullException();
			}
		}
		public static int LastIndex(this object[] list)
		{
			try
			{
				return list.Length - 1;
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				throw new ArgumentNullException();
			}
		}

		public static object[] RemoveFirst(this object[] list)
		{
			try
			{
				var newList = list.ToList();
				 newList.RemoveFirst();
				return newList.ToArray();
				//
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				throw new ArgumentNullException();
			}
		}
		public static object[] RemoveLast(this object[] list)
		{
			try
			{
				var newList = list.ToList();
				newList.RemoveLast();
				return newList.ToArray();
				//
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				throw new ArgumentNullException();
			}
		}

		public static int LastIndex<T>(this T[] list)
		{
			try
			{
				return list.Length - 1;
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				throw new ArgumentNullException();
			}
		}
		public static object LastElement<T>(this T[] list)
		{
			try
			{
				return list[list.Length - 1];
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				throw new ArgumentNullException();
			}
		}
		public static object FirstElement<T>(this T[] list)

		{
			try
			{
				return list[0];
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				throw new ArgumentNullException();
			}
		}

		public static T[] RemoveFirst<T>(this T[] list)
		{
			try
			{
				var newList = list.ToList();
				 newList.RemoveFirst();
				return newList.ToArray();
				//
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				throw new ArgumentNullException();
			}
		}
		public static T[] RemoveLast<T>(this T[] list)
		{
			try
			{
				var newList = list.ToList();
				newList.RemoveLast();
				return newList.ToArray();
				//
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				throw new ArgumentNullException();
			}
		}

		public static T FromObject<T>(this object obj)
		{
			
			throw new NotImplementedException();
		}
	}

}




