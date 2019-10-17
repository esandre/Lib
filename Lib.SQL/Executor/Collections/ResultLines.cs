using System.Collections;
using System.Collections.Generic;

namespace Lib.SQL.Executor.Collections
{
    public sealed class ResultLines : IList<ResultLine>
    {
        private readonly List<ResultLine> _internal = new List<ResultLine>(); 

        public ResultLines(IEnumerable<IEnumerable<KeyValuePair<string, object>>> otherCollection)
        {
            if(otherCollection == null) return;
            foreach (var line in otherCollection) _internal.Add(new ResultLine(line));
        }

        #region impl

        public IEnumerator<ResultLine> GetEnumerator()
        {
            return _internal.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(ResultLine item)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public bool Contains(ResultLine item)
        {
            return _internal.Contains(item);
        }

        public void CopyTo(ResultLine[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(ResultLine item)
        {
            throw new System.NotImplementedException();
        }

        public int Count => _internal.Count;
        public bool IsReadOnly => true;
        public int IndexOf(ResultLine item)
        {
            return _internal.IndexOf(item);
        }

        public void Insert(int index, ResultLine item)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new System.NotImplementedException();
        }

        public ResultLine this[int index]
        {
            get { return _internal[index]; }
            set { throw new System.NotImplementedException(); }
        }

        #endregion
    }
}
