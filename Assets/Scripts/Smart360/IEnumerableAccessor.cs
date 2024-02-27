using System.Collections.Generic;

public interface IEnumerableAccessor<T> : IAccessor<IEnumerable<T>>, IEnumerable<T> { }
