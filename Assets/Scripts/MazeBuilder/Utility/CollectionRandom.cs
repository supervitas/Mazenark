using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeBuilder.Utility {
	public class CollectionRandom {
		private HashSet<Element> objects = new HashSet<Element>();
		private HashSet<CollectionRandom> anotherCollectionRandoms = new HashSet<CollectionRandom>();
		private Random random = new Random();
		public Object GetRandom(Type thing, bool canBrowseChildrenIfEmpty = false, bool alwaysBrowseChildren = false) {
			var collectionOfThisType = GetAllOfType(thing);
			if (alwaysBrowseChildren || canBrowseChildrenIfEmpty && collectionOfThisType.Count == 0) {
				foreach (CollectionRandom collection in anotherCollectionRandoms) {
					collectionOfThisType.AddRange(collection.GetAllOfType(thing));
				}
			}

			if (collectionOfThisType.Count == 0)
				return null;

			float totalWeight = collectionOfThisType.Sum(x => x.Weight);
			float randomWeight = (float) random.NextDouble() * totalWeight; // random.NextDouble() returns something between 0 and 0.999..9997

			collectionOfThisType = collectionOfThisType.OrderBy(x => x.Weight).ToList();    // Ascending


			float weightsWereBefore = 0;
			Element previous = collectionOfThisType[0];
			foreach (Element el in collectionOfThisType) {
				if (weightsWereBefore > randomWeight) {
					break;
				}
				previous = el;
				weightsWereBefore += el.Weight;
			}

			return previous.Data;
		}

		public List<Element> GetAllOfType(Type type) {
			return objects.Where(x => x.Class.Equals(type)).ToList();
		}

		public override bool Equals(object obj) {
			CollectionRandom another = obj as CollectionRandom;

			return objects == another.objects && anotherCollectionRandoms == another.anotherCollectionRandoms;
		}

		public override int GetHashCode() {
			int hash = 37;

			hash = hash * 13 + anotherCollectionRandoms.GetHashCode();
			hash = hash * 13 + objects.GetHashCode();

			return hash;
		}

		public bool Add(Element thing) {
			if (objects.Contains(thing))
				return false;

			objects.Add(thing);
			return true;
		}

		public bool Add(CollectionRandom set) {
			if (anotherCollectionRandoms.Contains(set))
				return false;

			anotherCollectionRandoms.Add(set);
			return true;
		}

		public class Element {
			public string Name { get; private set; }
			public float Weight { get; private set; }
			public Type Class { get; private set; }
			public Object Data { get; private set; }
			public Element(Object data, string name, Type type, float weight = 1.0f) {
				Name = name;
				Class = type;
				Weight = weight;
				Data = data;
			}

			public override bool Equals(object obj) {
				Element other = obj as Element;

				if (other == null)
					return false;

				return Data == other.Data;
			}

			public override int GetHashCode() {
				int hash = 37;

				/*hash = hash * 13 + Name.GetHashCode();
				hash = hash * 13 + (int) Weight;
				hash = hash * 13 + Class.GetHashCode();*/
				hash = hash * 13 + Data.GetHashCode();

				return hash;
			}
		}
	}
}
