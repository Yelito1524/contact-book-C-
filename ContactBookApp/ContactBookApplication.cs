namespace ContactBookApp;

public sealed class ContactBookApplication
{
    private const int PageSize = 3;
    private readonly ContactBook _contactBook = new();

    public ContactBookApplication()
    {
        CreateTestContacts();
    }

    public void Run()
    {
        ShowWelcomeScreen();

        var exit = false;
        while (!exit)
        {
            ShowContacts();
            ShowInputOptions();

            var option = Console.ReadLine();
            Console.WriteLine();

            try
            {
                switch (option)
                {
                    case "1":
                        CreateContact();
                        break;
                    case "2":
                        FindContacts();
                        break;
                    case "3":
                        ReviewContact();
                        break;
                    case "4":
                        OrderContacts();
                        break;
                    case "5":
                        DeduplicateContacts();
                        break;
                    case "6":
                        MergeContacts();
                        break;
                    case "7":
                        ShowContactGroups();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        Pause();
                        break;
                }
            }
            catch (Exception error) when (error is ArgumentException or InvalidOperationException)
            {
                Console.WriteLine(error.Message);
                Pause();
            }
        }

        ShowExitScreen();
    }

    private void ShowWelcomeScreen()
    {
        ClearScreen();
        Console.WriteLine("====================================");
        Console.WriteLine("        Contact Book Application     ");
        Console.WriteLine("====================================");
        Console.WriteLine("This app stores, searches, orders, and merges contacts.");
        Pause();
    }

    private void ShowContacts()
    {
        var contacts = _contactBook.GetContacts();
        var page = 0;

        while (true)
        {
            ClearScreen();
            Console.WriteLine("Contacts");
            Console.WriteLine("========");
            PrintContacts(contacts.Skip(page * PageSize).Take(PageSize).ToList());

            var totalPages = Math.Max(1, (int)Math.Ceiling(contacts.Count / (double)PageSize));
            Console.WriteLine();
            Console.WriteLine($"Page {page + 1} of {totalPages}");

            if (totalPages == 1)
            {
                return;
            }

            Console.Write("N = next page, P = previous page, Enter = menu: ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                return;
            }

            if (input.Equals("N", StringComparison.OrdinalIgnoreCase) && page < totalPages - 1)
            {
                page++;
            }
            else if (input.Equals("P", StringComparison.OrdinalIgnoreCase) && page > 0)
            {
                page--;
            }
        }
    }

    private static void ShowInputOptions()
    {
        Console.WriteLine();
        Console.WriteLine("Options");
        Console.WriteLine("=======");
        Console.WriteLine("1. Create contact");
        Console.WriteLine("2. Find contacts");
        Console.WriteLine("3. Review contact");
        Console.WriteLine("4. Order contacts");
        Console.WriteLine("5. Deduplicate contacts");
        Console.WriteLine("6. Merge contacts with Find-Union");
        Console.WriteLine("7. Show contact groups");
        Console.WriteLine("0. Exit");
        Console.Write("Select option: ");
    }

    private static void ShowExitScreen()
    {
        ClearScreen();
        Console.WriteLine("====================================");
        Console.WriteLine("        Thanks for using the app     ");
        Console.WriteLine("====================================");
    }

    private void CreateContact()
    {
        ClearScreen();
        Console.WriteLine("Create Contact");
        Console.WriteLine("==============");

        var contact = _contactBook.Add(
            ReadRequired("Name: "),
            ReadRequired("Phone: "),
            ReadRequired("Email: "));

        Console.WriteLine();
        Console.WriteLine("Contact created:");
        PrintContact(contact);
        Pause();
    }

    private void FindContacts()
    {
        ClearScreen();
        Console.WriteLine("Find Contacts");
        Console.WriteLine("=============");

        var query = ReadRequired("Search by name, phone, or email: ");
        var contacts = _contactBook.FindContacts(query);

        Console.WriteLine();
        PrintContacts(contacts);
        Pause();
    }

    private void ReviewContact()
    {
        ClearScreen();
        Console.WriteLine("Review Contact");
        Console.WriteLine("==============");

        var id = ReadId("Contact id: ");
        var contact = _contactBook.FindById(id);

        Console.WriteLine();
        if (contact is null)
        {
            Console.WriteLine("Contact not found.");
        }
        else
        {
            PrintContact(contact);
        }

        Pause();
    }

    private void OrderContacts()
    {
        ClearScreen();
        Console.WriteLine("Ordered Contacts");
        Console.WriteLine("================");
        PrintContacts(_contactBook.ListSortedByName());
        Pause();
    }

    private void DeduplicateContacts()
    {
        ClearScreen();
        Console.WriteLine("Deduplicate Contacts");
        Console.WriteLine("====================");

        var duplicateGroups = _contactBook.FindDuplicateGroups();
        if (duplicateGroups.Count == 0)
        {
            Console.WriteLine("No duplicate contacts found.");
            Pause();
            return;
        }

        foreach (var group in duplicateGroups)
        {
            Console.WriteLine($"Duplicate key: {group.Key}");
            PrintContacts(group.Contacts);
            Console.WriteLine();
        }

        Console.WriteLine("Use option 6 to merge contacts that belong together.");
        Pause();
    }

    private void MergeContacts()
    {
        ClearScreen();
        Console.WriteLine("Merge Contacts - Find-Union");
        Console.WriteLine("===========================");

        var firstId = ReadId("First contact id: ");
        var secondId = ReadId("Second contact id: ");
        var merged = _contactBook.LinkContacts(firstId, secondId);

        Console.WriteLine();
        Console.WriteLine(merged
            ? "Contacts merged into the same set."
            : "Contacts were already merged or one id was not found.");

        Pause();
    }

    private void ShowContactGroups()
    {
        ClearScreen();
        Console.WriteLine("Contact Groups");
        Console.WriteLine("==============");

        foreach (var group in _contactBook.GetAllGroups())
        {
            Console.WriteLine($"Set root: {group.Key}");
            PrintContacts(group.Value);
            Console.WriteLine();
        }

        Pause();
    }

    private void CreateTestContacts()
    {
        var ana = _contactBook.Add("Ana Garcia", "555-0101", "ana@example.com");
        var luis = _contactBook.Add("Luis Perez", "555-0110", "luis@example.com");
        var maria = _contactBook.Add("Maria Lopez", "555-0120", "maria@example.com");
        var carlos = _contactBook.Add("Carlos Ruiz", "555-0130", "carlos@example.com");
        _contactBook.Add("Ana Garcia Home", "555-0101", "ana.home@example.com", allowDuplicateContactData: true);

        _contactBook.LinkContacts(ana.Id, maria.Id);
        _contactBook.LinkContacts(luis.Id, carlos.Id);
    }

    private static void PrintContacts(IReadOnlyCollection<Contact> contacts)
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

    private static void PrintContact(Contact contact)
    {
        Console.WriteLine($"Id:    {contact.Id}");
        Console.WriteLine($"Name:  {contact.Name}");
        Console.WriteLine($"Phone: {contact.Phone}");
        Console.WriteLine($"Email: {contact.Email}");
    }

    private static string ReadRequired(string prompt)
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

    private static int ReadId(string prompt)
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

    private static void Pause()
    {
        Console.WriteLine();
        Console.Write("Press Enter to continue...");
        Console.ReadLine();
    }

    private static void ClearScreen()
    {
        if (Console.IsInputRedirected || Console.IsOutputRedirected)
        {
            Console.WriteLine();
            return;
        }

        Console.Clear();
    }
}
