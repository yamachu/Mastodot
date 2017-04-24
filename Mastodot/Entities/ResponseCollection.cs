using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Mastodot.Entities
{
    public class ResponseCollection<T> : IEnumerable<T>
        where T : IBaseMastodonEntity
    {
        public LinkHeader Links { get; set; }
        public string RawJson { get; set; }
        private IEnumerable<T> Source;

        public ResponseCollection(IEnumerable<T> source)
        {
            Source = source;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var val in Source)
            {
                yield return val;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Source.GetEnumerator();
        }
    }
}
