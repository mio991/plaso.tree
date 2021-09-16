using System;
using System.Linq;
using System.Numerics;

namespace Plaso.Tree
{
    /// <summary>
    /// Represents a Node in a Tree using a bit pattern in 64-bits.
    /// Every 8 bits representing one Level for a total of 8 posible levels.
    ///
    /// The Nodes have a Total Order.
    /// </summary>
    public struct NodeReference
    {
        /// <summary>The Root Node or Level 0.</summary>
        public static readonly NodeReference Root = new NodeReference(0x00ul);
        private static ulong maskForLevel(int index) => 0xfful << ((8 - index) * 8);
        private static ulong incrementForLevel(int index) => 0x01ul << ((8 - index) * 8);

        private readonly ulong reference;



        /// <summary>The Level or Depth of the Node.</summary>
        public int Level => 8 - (BitOperations.TrailingZeroCount(reference) / 8);

        /// <summary>The Parrent Node</summary>
        public NodeReference? Parrent
        {
            get
            {
                if (this != NodeReference.Root)
                {
                    ulong parrentAddressMask = ~maskForLevel(Level);
                    ulong parrentAddress = reference & parrentAddressMask;

                    return new NodeReference(parrentAddress);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>The next Sibling</summary>
        public NodeReference? NextSibling
        {
            get
            {
                ulong currentIndexMask = maskForLevel(Level);
                ulong currentIndexIncrement = incrementForLevel(Level);

                if ((reference & currentIndexMask) == currentIndexMask)
                {
                    return null; // Index 255
                }
                else
                {
                    return new NodeReference(reference + currentIndexIncrement);
                }

            }
        }

        /// <summary>The previous Sibling</summary>
        public NodeReference? PreviousSibling
        {
            get
            {
                ulong currentIndexMask = maskForLevel(Level);
                ulong currentIndexIncrement = incrementForLevel(Level);

                if ((reference & currentIndexMask) == currentIndexIncrement)
                {
                    return null; // Index 1
                }
                else
                {
                    return new NodeReference(reference - currentIndexIncrement);
                }
            }
        }

        public NodeReference? FirstChild
        {
            get
            {
                if (Level < 8)
                {
                    ulong childAddress = reference | incrementForLevel(Level + 1);
                    return new NodeReference(childAddress);
                }
                else
                {
                    return null;
                }
            }
        }
        public NodeReference? LastChild
        {
            get
            {
                if (Level < 8)
                {
                    ulong childAddress = reference | maskForLevel(Level + 1);
                    return new NodeReference(childAddress);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Create a NodeReference from ist binary representation
        /// </summary>
        public NodeReference(ulong reference)
        {
            this.reference = reference;
        }


        public bool HasDescendent(NodeReference child)
        {
            return (reference & child.reference) == reference;
        }

        public bool HasChild(NodeReference child) => HasDescendent(child) && child.Level - 1 == Level;

        public bool HasAncestor(NodeReference ancestor) => ancestor.HasDescendent(this);

        public bool HasParent(NodeReference parrent) => parrent.HasChild(this);

        public override bool Equals(object other)
        {
            if (other is NodeReference otherNode)
            {
                return this == otherNode;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return reference.GetHashCode();
        }

        public static bool operator ==(NodeReference lhs, NodeReference rhs) => lhs.reference == rhs.reference;
        public static bool operator !=(NodeReference lhs, NodeReference rhs) => lhs.reference != rhs.reference;

        public static bool operator <(NodeReference lhs, NodeReference rhs) => lhs.reference < rhs.reference;
        public static bool operator >(NodeReference lhs, NodeReference rhs) => lhs.reference > rhs.reference;
    }
}