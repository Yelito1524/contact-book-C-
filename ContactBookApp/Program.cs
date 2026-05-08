using ContactBookApp;

var contactBook = new ContactBook();
Seed(contactBook);

while (true)
{
    ClearScreen();
    Console.WriteLine("Contact Book App");
    Console.WriteLine("================");
    Console.WriteLine("1. Add contact");
    Console.WriteLine("2. List contacts sorted by name");
    Console.WriteLine("3. Search contacts by name");
    Console.WriteLine("4. Find contact by phone");
    Console.WriteLine("5. Update contact");
    Console.WriteLine("6. Delete contact");
    Console.WriteLine("7. Link contacts with Union-Find");
    Console.WriteLine("8. Show contact group");
    Console.WriteLine("9. Show all groups");
    Console.WriteLine("0. Exit");
    Console.Write("Choose an option: ");

    var option = Console.ReadLine();
    Console.WriteLine();

    try
    {
        switch (option)
        {
            case "1":
                AddContact(contactBook);
                break;
            case "2":
                PrintContacts(contactBook.ListSortedByName());
                break;
            case "3":
                SearchByName(contactBook);
                break;
            case "4":
                FindByPhone(contactBook);
                break;
            case "5":
                UpdateContact(contactBook);
                break;
            case "6":
                DeleteContact(contactBook);
                break;
            case "7":
                LinkContacts(contactBook);
                break;
            case "8":
                ShowGroup(contactBook);
                break;
            case "9":
                ShowAllGroups(contactBook);
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Invalid option.");
                break;
        }
    }
    catch (Exception error) when (error is ArgumentException or InvalidOperationException)
    {
        Console.WriteLine(error.Message);
    }

    Pause();
}

static void Seed(ContactBook contactBook)
{
    var ana = contactBook.Add("Ana Garcia", "555-0101", "ana@example.com");
    var luis = contactBook.Add("Luis Perez", "555-0110", "luis@example.com");
    var maria = contactBook.Add("Maria Lopez", "555-0120", "maria@example.com");
    var carlos = contactBook.Add("Carlos Ruiz", "555-0130", "carlos@example.com");

    contactBook.LinkContacts(ana.Id, maria.Id);
    contactBook.LinkContacts(luis.Id, carlos.Id);
}

static void AddContact(ContactBook contactBook)
{
    var contact = contactBook.Add(
        ReadRequired("Name: "),
        ReadRequired("Phone: "),
        ReadRequired("Email: "));

    Console.WriteLine("Contact added:");
    PrintContacts(new[] { contact });
}

static void SearchByName(ContactBook contactBook)
{
    var query = ReadRequired("Name search: ");
    PrintContacts(contactBook.SearchByName(query));
}

static void FindByPhone(ContactBook contactBook)
{
    var phone = ReadRequired("Phone: ");
    var contact = contactBook.FindByPhone(phone);
    PrintContacts(contact is null ? Array.Empty<Contact>() : new[] { contact });
}

static void UpdateContact(ContactBook contactBook)
{
    var id = ReadId("Contact id to update: ");
    var updated = contactBook.Update(
        id,
        ReadRequired("New name: "),
        ReadRequired("New phone: "),
        ReadRequired("New email: "));

    Console.WriteLine(updated ? "Contact updated." : "Contact not found.");
}

static void DeleteContact(ContactBook contactBook)
{
    var id = ReadId("Contact id to delete: ");
    Console.WriteLine(contactBook.Delete(id) ? "Contact deleted." : "Contact not found.");
}

static void LinkContacts(ContactBook contactBook)
{
    var firstId = ReadId("First contact id: ");
    var secondId = ReadId("Second contact id: ");
    var linked = contactBook.LinkContacts(firstId, secondId);

    Console.WriteLine(linked
        ? "Contacts linked in the same group."
        : "Contacts were already linked or one id was not found.");
}

static void ShowGroup(ContactBook contactBook)
{
    var id = ReadId("Contact id: ");
    PrintContacts(contactBook.GetGroup(id));
}

static void ShowAllGroups(ContactBook contactBook)
{
    var groups = contactBook.GetAllGroups();

    foreach (var group in groups)
    {
        Console.WriteLine($"Group root id: {group.Key}");
        PrintContacts(group.Value);
        Console.WriteLine();
    }
}

static void PrintContacts(IReadOnlyCollection<Contact> contacts)
{
    if (contacts.Count == 0)
    {
        Console.WriteLine("No contacts found.");
        return;
    }

    Console.WriteLine(" ID | Name                     | Phone          | Email");
    Console.WriteLine("----+--------------------------+----------------+----------------------");

    foreach (var contact in contacts)
    {
        Console.WriteLine(contact);
    }
}

static string ReadRequired(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        var value = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        Console.WriteLine("Value required.");
    }
}

static int ReadId(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out var id) && id > 0)
        {
            return id;
        }

        Console.WriteLine("Enter a positive whole number.");
    }
}

static void Pause()
{
    Console.WriteLine();
    Console.Write("Press Enter to continue...");
    Console.ReadLine();
}

static void ClearScreen()
{
    if (Console.IsInputRedirected || Console.IsOutputRedirected)
    {
        Console.WriteLine();
        return;
    }

    Console.Clear();
}
