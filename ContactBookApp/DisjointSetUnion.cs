namespace ContactBookApp;

public sealed class DisjointSetUnion
{
    private readonly Dictionary<int, int> _parent = new();
    private readonly Dictionary<int, int> _rank = new();

    public void MakeSet(int value)
    {
        if (_parent.ContainsKey(value))
        {
            return;
        }

        _parent[value] = value;
        _rank[value] = 0;
    }

    public int Find(int value)
    {
        if (!_parent.ContainsKey(value))
        {
            throw new InvalidOperationException($"No set exists for id {value}.");
        }

        if (_parent[value] != value)
        {
            _parent[value] = Find(_parent[value]);
        }

        return _parent[value];
    }

    public bool Union(int first, int second)
    {
        var firstRoot = Find(first);
        var secondRoot = Find(second);

        if (firstRoot == secondRoot)
        {
            return false;
        }

        if (_rank[firstRoot] < _rank[secondRoot])
        {
            _parent[firstRoot] = secondRoot;
        }
        else if (_rank[firstRoot] > _rank[secondRoot])
        {
            _parent[secondRoot] = firstRoot;
        }
        else
        {
            _parent[secondRoot] = firstRoot;
            _rank[firstRoot]++;
        }

        return true;
    }

    public void Remove(int value)
    {
        if (!_parent.Remove(value))
        {
            return;
        }

        _rank.Remove(value);

        foreach (var item in _parent.Keys.ToList())
        {
            if (_parent[item] == value)
            {
                _parent[item] = item;
                _rank[item] = 0;
            }
        }
    }
}
