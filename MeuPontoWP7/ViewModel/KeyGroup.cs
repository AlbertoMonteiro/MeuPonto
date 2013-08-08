using System;
using System.Collections.Generic;
using System.Linq;

namespace MeuPontoWP7.ViewModel
{
    public class KeyGroup<T> : List<T>
    {
        /// <summary>
        /// The Key of this group.
        /// </summary>
        public string Key { get; private set; }

        private KeyGroup(IGrouping<string, T> key)
        {
            Key = key.Key;
            AddRange(key);
        }

        public static List<KeyGroup<T>> CreateGroups(IEnumerable<T> items, Func<T, string> grouper)
        {
            return items.GroupBy(grouper).Select(x => new KeyGroup<T>(x)).ToList();
        }
    }
}