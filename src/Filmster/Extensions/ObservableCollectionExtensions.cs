using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Search;

namespace Filmster.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void AddRange<T>(this ObservableCollection<T> observableCollection, IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                observableCollection.Add(item);
            }
        }

        public static void Reset<T>(this ObservableCollection<T> observableCollection, IEnumerable<T> collection)
        {
            observableCollection.Clear();
            observableCollection.AddRange(collection);
        }

        public static void Keep<T>(this ObservableCollection<T> oldList, int keepCount)
        {
            var diff = oldList.Count - keepCount;

            if (diff <= 0)
            {
                return;
            }

            for (int i = 0; i < diff; i++)
            {
                oldList.RemoveAt(keepCount);
            }
        }

        // Fisher-Yates shuffle
        public static IEnumerable<T> Shuffle<T>(this IList<T> list)
        {
            Random random = new Random();
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
            return list;
        }

        public static void Refresh(this ObservableCollection<SearchMovie> oldList, IEnumerable<SearchMovie> newList)
        {
            for (int i = 0; i < oldList.Count; i++)
            {
                if (!newList.Any(x => x.Id == oldList[i].Id))
                {
                    oldList.RemoveAt(i--);
                }
            }

            foreach (var (item, index) in newList.Select((x, i) => (item: x, index: i)))
            {
                if (!oldList.Any(x => x.Id == item.Id))
                {
                    oldList.Insert(index, item);
                }
                else
                {
                    var i = oldList.IndexOf(oldList.FirstOrDefault(x => x.Id == item.Id));
                    if (i != index)
                    {
                        oldList.Move(i, index);
                    }
                }
            }
        }

        public static void Refresh(this ObservableCollection<MovieRole> oldList, IEnumerable<MovieRole> newList)
        {
            for (int i = 0; i < oldList.Count; i++)
            {
                if (!newList.Any(x => x.CreditId == oldList[i].CreditId))
                {
                    oldList.RemoveAt(i--);
                }
            }

            foreach (var (item, index) in newList.Select((x, i) => (item: x, index: i)))
            {
                if (!oldList.Any(x => x.CreditId == item.CreditId))
                {
                    oldList.Insert(index, item);
                }
                else
                {
                    var i = oldList.IndexOf(oldList.FirstOrDefault(x => x.CreditId == item.CreditId));
                    if (i != index)
                    {
                        oldList.Move(i, index);
                    }
                }
            }
        }

        public static void Refresh(this ObservableCollection<TvRole> oldList, IEnumerable<TvRole> newList)
        {
            for (int i = 0; i < oldList.Count; i++)
            {
                if (!newList.Any(x => x.CreditId == oldList[i].CreditId))
                {
                    oldList.RemoveAt(i--);
                }
            }

            foreach (var (item, index) in newList.Select((x, i) => (item: x, index: i)))
            {
                if (!oldList.Any(x => x.CreditId == item.CreditId))
                {
                    oldList.Insert(index, item);
                }
                else
                {
                    var i = oldList.IndexOf(oldList.FirstOrDefault(x => x.CreditId == item.CreditId));
                    if (i != index)
                    {
                        oldList.Move(i, index);
                    }
                }
            }
        }

        public static void Refresh(this ObservableCollection<MovieJob> oldList, IEnumerable<MovieJob> newList)
        {
            for (int i = 0; i < oldList.Count; i++)
            {
                if (!newList.Any(x => x.CreditId == oldList[i].CreditId))
                {
                    oldList.RemoveAt(i--);
                }
            }

            foreach (var (item, index) in newList.Select((x, i) => (item: x, index: i)))
            {
                if (!oldList.Any(x => x.CreditId == item.CreditId))
                {
                    oldList.Insert(index, item);
                }
                else
                {
                    var i = oldList.IndexOf(oldList.FirstOrDefault(x => x.CreditId == item.CreditId));
                    if (i != index)
                    {
                        oldList.Move(i, index);
                    }
                }
            }
        }

        public static void Refresh(this ObservableCollection<TvJob> oldList, IEnumerable<TvJob> newList)
        {
            for (int i = 0; i < oldList.Count; i++)
            {
                if (!newList.Any(x => x.CreditId == oldList[i].CreditId))
                {
                    oldList.RemoveAt(i--);
                }
            }

            foreach (var (item, index) in newList.Select((x, i) => (item: x, index: i)))
            {
                if (!oldList.Any(x => x.CreditId == item.CreditId))
                {
                    oldList.Insert(index, item);
                }
                else
                {
                    var i = oldList.IndexOf(oldList.FirstOrDefault(x => x.CreditId == item.CreditId));
                    if (i != index)
                    {
                        oldList.Move(i, index);
                    }
                }
            }
        }
    }
}
