# Disjoint-Set-Union and Find-Union

Disjoint-Set-Union, also called DSU or Union-Find, is a data structure that keeps track of separate groups.

In the Contact Book App, every contact begins in a separate group. When the user links two contacts, the app combines their groups. This can represent relationships such as family, coworkers, classmates, or emergency contacts.

## Main Operations

`MakeSet(x)` creates a new set containing only `x`.

`Find(x)` returns the representative, or root, of the set containing `x`.

`Union(a, b)` joins the sets that contain `a` and `b`.

## Optimizations

Path compression makes `Find` faster. When finding the root, each visited item is updated to point directly to the root.

Union by rank keeps trees shallow. When joining two groups, the shorter tree is attached under the taller tree.

Together, these optimizations make DSU operations almost constant time in practice.

## Project Example

If contact 1 is linked with contact 3, the app calls:

```csharp
contactBook.LinkContacts(1, 3);
```

Inside that method, the DSU runs:

```csharp
_groups.Union(firstId, secondId);
```

Later, when showing a group, the app compares roots:

```csharp
_groups.Find(contact.Id) == root
```

Contacts with the same root belong to the same group.
