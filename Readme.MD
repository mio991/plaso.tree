# Idea

The idea for this came when I had to wrestle a tree structure in a database implemented as self references to the parent node.

This library provides a `NodeReference` implementation using a `ulong` as a simple bitmap.

The bits are equaly distributed on 8 levels à 8 bits.