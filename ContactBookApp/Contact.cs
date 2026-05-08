namespace ContactBookApp;

public sealed class Contact
{
    public Contact(int id, string name, string phone, string email)
    {
        Id = id;
        Name = name;
        Phone = phone;
        Email = email;
    }

    public int Id { get; }
    public string Name { get; private set; }
    public string Phone { get; private set; }
    public string Email { get; private set; }

    public void Update(string name, string phone, string email)
    {
        Name = name;
        Phone = phone;
        Email = email;
    }

    public override string ToString()
    {
        return $"{Id,3} | {Name,-24} | {Phone,-14} | {Email}";
    }
}
