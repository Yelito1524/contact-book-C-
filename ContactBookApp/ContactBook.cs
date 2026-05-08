namespace ContactBookApp;

public sealed class ContactBook
{
    private readonly List<Contact> _contacts = new();
    private readonly Dictionary<int, Contact> _contactsById = new();
    private readonly Dictionary<string, int> _idsByPhone = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, int> _idsByEmail = new(StringComparer.OrdinalIgnoreCase);
    private readonly DisjointSetUnion _groups = new();
    private int _nextId = 1;

    public Contact Add(string name, string phone, string email, bool allowDuplicateContactData = false)
    {
        EnsureRequired(name, "name");
        EnsureRequired(phone, "phone");
        EnsureRequired(email, "email");

        if (!allowDuplicateContactData)
        {
            EnsureUnique(phone, email);
        }

        var contact = new Contact(_nextId++, name.Trim(), phone.Trim(), email.Trim());
        _contacts.Add(contact);
        _contactsById[contact.Id] = contact;
        _idsByPhone.TryAdd(contact.Phone, contact.Id);
        _idsByEmail.TryAdd(contact.Email, contact.Id);
        _groups.MakeSet(contact.Id);

        return contact;
    }

    public bool Update(int id, string name, string phone, string email)
    {
        if (!_contactsById.TryGetValue(id, out var contact))
        {
            return false;
        }

        EnsureRequired(name, "name");
        EnsureRequired(phone, "phone");
        EnsureRequired(email, "email");

        var normalizedPhone = phone.Trim();
        var normalizedEmail = email.Trim();

        EnsureUnique(normalizedPhone, normalizedEmail, id);

        _idsByPhone.Remove(contact.Phone);
        _idsByEmail.Remove(contact.Email);

        contact.Update(name.Trim(), normalizedPhone, normalizedEmail);
        _idsByPhone[contact.Phone] = contact.Id;
        _idsByEmail[contact.Email] = contact.Id;

        return true;
    }

    public bool Delete(int id)
    {
        if (!_contactsById.TryGetValue(id, out var contact))
        {
            return false;
        }

        _contacts.Remove(contact);
        _contactsById.Remove(id);
        _idsByPhone.Remove(contact.Phone);
        _idsByEmail.Remove(contact.Email);
        _groups.Remove(id);

        return true;
    }

    public Contact? FindById(int id)
    {
        return _contactsById.GetValueOrDefault(id);
    }

    public Contact? FindByPhone(string phone)
    {
        return _idsByPhone.TryGetValue(phone.Trim(), out var id)
            ? _contactsById[id]
            : null;
    }

    public IReadOnlyList<Contact> GetContacts()
    {
        return _contacts.ToList();
    }

    public IReadOnlyList<Contact> FindContacts(string query)
    {
        var normalizedQuery = query.Trim();

        return _contacts
            .Where(contact =>
                contact.Name.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase)
                || contact.Phone.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase)
                || contact.Email.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase))
            .OrderBy(contact => contact.Name)
            .ToList();
    }

    public IReadOnlyList<Contact> SearchByName(string query)
    {
        return _contacts
            .Where(contact => contact.Name.Contains(query.Trim(), StringComparison.OrdinalIgnoreCase))
            .OrderBy(contact => contact.Name)
            .ToList();
    }

    public IReadOnlyList<Contact> ListSortedByName()
    {
        return _contacts
            .OrderBy(contact => contact.Name)
            .ThenBy(contact => contact.Id)
            .ToList();
    }

    public bool LinkContacts(int firstId, int secondId)
    {
        if (!_contactsById.ContainsKey(firstId) || !_contactsById.ContainsKey(secondId))
        {
            return false;
        }

        return _groups.Union(firstId, secondId);
    }

    public IReadOnlyList<Contact> GetGroup(int id)
    {
        if (!_contactsById.ContainsKey(id))
        {
            return Array.Empty<Contact>();
        }

        var root = _groups.Find(id);

        return _contacts
            .Where(contact => _groups.Find(contact.Id) == root)
            .OrderBy(contact => contact.Name)
            .ToList();
    }

    public IReadOnlyDictionary<int, IReadOnlyList<Contact>> GetAllGroups()
    {
        return _contacts
            .GroupBy(contact => _groups.Find(contact.Id))
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyList<Contact>)group.OrderBy(contact => contact.Name).ToList());
    }

    public IReadOnlyList<DuplicateContactGroup> FindDuplicateGroups()
    {
        return _contacts
            .GroupBy(contact => contact.Phone)
            .Where(group => group.Count() > 1)
            .Select(group => new DuplicateContactGroup(group.Key, group.OrderBy(contact => contact.Name).ToList()))
            .ToList();
    }

    private static void EnsureRequired(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"The {fieldName} cannot be empty.");
        }
    }

    private void EnsureUnique(string phone, string email, int? currentId = null)
    {
        if (_idsByPhone.TryGetValue(phone.Trim(), out var phoneOwner) && phoneOwner != currentId)
        {
            throw new InvalidOperationException("Another contact already uses that phone number.");
        }

        if (_idsByEmail.TryGetValue(email.Trim(), out var emailOwner) && emailOwner != currentId)
        {
            throw new InvalidOperationException("Another contact already uses that email.");
        }
    }
}
