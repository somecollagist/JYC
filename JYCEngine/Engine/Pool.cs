namespace JYCEngine;

public class Pool<T> : IPool
{
    const int DefaultCapacity = 128;

    private int _itemCount;
    private int _freeCount;
    private T[] _items;
    private int[] _free;

    public Pool()
    {
        _items = new T[DefaultCapacity];
        _free = new int[DefaultCapacity];
    }

    public ref T Get(int id)
    {
        return ref _items[id];
    }

    public void Set(int id, T item)
    {
        _items[id] = item;
    }

    public int Reserve()
    {
        int id;
        if (_freeCount > 0)
        {
            _freeCount--;
            id = _free[_freeCount];
        }
        else
        {
            if (_itemCount >= _items.Length)
                Array.Resize(ref _items, _items.Length << 1); // Double capacity
            id = _itemCount;
            _itemCount++;
        }
        return id;
    }

    public void Recycle(int id)
    {
        _items[id] = default;
        if (_freeCount >= _free.Length)
            Array.Resize(ref _free, _freeCount << 1); // Double capacity
        _free[_freeCount] = id;
        _freeCount++;
    }

    object IPool.Get(int id)
    {
        return _items[id];
    }

    void IPool.Set(int id, object item)
    {
        _items[id] = (T)item;
    }
}

