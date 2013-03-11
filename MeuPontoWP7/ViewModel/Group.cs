using System.Collections;
using System.Collections.Generic;

namespace MeuPontoWP7.ViewModel
{
    public class Group<T> : IEnumerable<T>
    {
        public Group(string name, IEnumerable<T> items)
        {
            Title = name;
            Items = new List<T>(items);
        }

        public override bool Equals(object obj)
        {
            var that = obj as Group<T>;

            return (that != null) && (Title.Equals(that.Title));
        }

        public string Title { get; set; }

        public IList<T> Items { get; set; }

        public IEnumerator<T> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }
    }
}
