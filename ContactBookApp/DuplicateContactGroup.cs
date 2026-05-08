namespace ContactBookApp;

public sealed class DuplicateContactGroup
{
    public DuplicateContactGroup(string key, IReadOnlyList<Contact> contacts)
    {
        Key = key;
        Contacts = contacts;
    }

    public string Key { get; }
    public IReadOnlyList<Contact> Contacts { get; }
}
