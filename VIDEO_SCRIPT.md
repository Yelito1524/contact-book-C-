# 10-Minute Video Script

## 0:00-1:00 Introduction

Hello, my name is [your name]. This is my Contact Book App made in C#. The app lets a user add, list, search, update, and delete contacts. It also includes a relationship-group feature that uses the Disjoint-Set-Union data structure and the Find-Union algorithm.

## 1:00-2:30 App Demo

Run the application with:

```powershell
dotnet run --project ContactBookApp
```

Show the menu. List the sample contacts. Add a new contact with a name, phone number, and email. Search by name and find by phone number. Explain that this is a console application, so all interaction happens through menu options and keyboard input.

## 2:30-4:00 General Data Structures

Open `ContactBook.cs`.

The app uses a `List<Contact>` to store the complete collection of contacts. A list is useful because I can add contacts, loop through all contacts, and sort them for display.

The app also uses dictionaries. `_contactsById` maps an integer id to a contact. `_idsByPhone` maps a phone number to a contact id. `_idsByEmail` maps an email to a contact id. A dictionary is useful because lookup is very fast on average.

## 4:00-5:30 General Algorithms

The app uses search, sorting, insertion, update, and delete algorithms.

For search by name, it checks each contact and compares the name with the query. This is a linear search because it may need to inspect every contact.

For listing contacts, it sorts them alphabetically by name. Sorting makes the output easier to read.

For finding by phone number, it uses a dictionary lookup. This is faster than checking every contact one by one.

## 5:30-7:30 Disjoint-Set-Union

Open `DisjointSetUnion.cs`.

Disjoint-Set-Union, also called Union-Find, is a data structure for managing groups. In this project, each contact starts in its own group. When two contacts are related, the app links them into the same group.

The `MakeSet` method creates a new group. The `Find` method returns the representative or root of a group. The `Union` method joins two groups together.

This implementation uses path compression in `Find`. When a node points to another parent, the method updates it to point directly to the root. That makes future searches faster.

It also uses union by rank. The smaller tree is attached under the larger tree so the structure stays efficient.

## 7:30-9:00 Union-Find Demo

Go back to the app. Choose the option to link contacts with Union-Find. Enter two contact ids. Then choose the option to show that contact group.

Explain that both contacts now have the same group root, so the app knows they belong together. This could represent family members, coworkers, classmates, or emergency contacts.

## 9:00-10:00 Conclusion

To summarize, this Contact Book App uses lists for storing contacts, dictionaries for fast lookup, sorting for readable output, linear search for name matching, and Disjoint-Set-Union for grouping related contacts. These data structures and algorithms make the app organized, efficient, and easy to extend.
