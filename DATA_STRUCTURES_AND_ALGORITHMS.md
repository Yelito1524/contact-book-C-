# General Data Structures and Algorithms

## Data Structures

`List<T>` stores items in order. In this app, `List<Contact>` keeps the full contact collection.

`Dictionary<TKey, TValue>` stores key-value pairs. In this app, dictionaries support fast lookup by id, phone number, and email.

An object or class groups related fields and behavior. The `Contact` class stores id, name, phone, and email. The `ContactBook` class manages the full set of operations.

## Algorithms

Insertion adds a new contact to the list and lookup dictionaries.

Deletion removes the contact from all data structures so there is no stale data.

Update changes a contact and refreshes the phone and email indexes.

Linear search checks contacts one by one when searching by part of a name.

Sorting orders contacts by name before printing them.

Hash lookup uses dictionaries to find contacts quickly by exact keys such as id or phone number.

## Complexity Summary

Adding a contact is average `O(1)` for dictionary inserts and amortized `O(1)` for list append.

Searching by name is `O(n)` because every contact may need to be checked.

Finding by id or phone number is average `O(1)` because it uses a dictionary.

Listing contacts sorted by name is `O(n log n)` because sorting is required.

Deleting a contact is `O(n)` because removing from the list may require scanning and shifting elements.
