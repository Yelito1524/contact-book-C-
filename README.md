# Contact Book App

A C# console application for managing contacts. The solution demonstrates general data structures and algorithms, plus the Disjoint-Set-Union data structure using the Find-Union algorithm.

## Features

- Add, list, update, and delete contacts.
- Search contacts by name.
- Find a contact quickly by phone number.
- Keep phone numbers and emails unique.
- Sort contacts alphabetically.
- Link related contacts into groups using Union-Find.
- Show one contact's group or all connected groups.

## Run the App

```powershell
dotnet run --project ContactBookApp
```

## Data Structures Used

- `List<Contact>` stores all contacts and supports full listing, sorting, and name search.
- `Dictionary<int, Contact>` maps contact ids to contacts for fast id lookup.
- `Dictionary<string, int>` maps phone numbers and emails to contact ids for fast duplicate checks and phone search.
- `DisjointSetUnion` stores groups of linked contacts.

## Algorithms Used

- Linear search filters contacts by name using `Where`.
- Sorting orders contacts alphabetically using `OrderBy`.
- Hash lookup finds contacts by id, phone, or email in average constant time.
- Union-Find links contacts into relationship groups.
- Path compression makes future `Find` operations faster.
- Union by rank keeps the DSU tree shallow.

## Suggested Video Submission

Record a 10-minute walkthrough showing:

1. The program running from the terminal.
2. The `Contact`, `ContactBook`, and `DisjointSetUnion` classes.
3. How lists, dictionaries, sorting, searching, and Union-Find are used.
4. A demo linking two contacts and showing their group.

Submit the YouTube URL after uploading the recording.
