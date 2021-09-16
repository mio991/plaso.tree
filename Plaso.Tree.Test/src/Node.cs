using NUnit.Framework;
using Plaso.Tree;

namespace Tests
{
    public class Node
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void RootEqualsRoot()
        {
            Assert.That(NodeReference.Root, Is.EqualTo(NodeReference.Root));

            {
                var child = NodeReference.Root.FirstChild.Value;
                var root = child.Parent.Value;
                Assert.That(root, Is.EqualTo(NodeReference.Root));
            }

            {
                var child = NodeReference.Root.LastChild.Value;
                var root = child.Parent.Value;
                Assert.That(root, Is.EqualTo(NodeReference.Root));
            }

            {
                var child = NodeReference.Root.FirstChild.Value;
                var sibling = child.NextSibling.Value;
                var root = sibling.Parent.Value;
                Assert.That(root, Is.EqualTo(NodeReference.Root));
            }

            {
                var child = NodeReference.Root.LastChild.Value;
                var sibling = child.PreviousSibling.Value;
                var root = sibling.Parent.Value;
                Assert.That(root, Is.EqualTo(NodeReference.Root));
            }
        }

        [Test]
        public void RootHasNoParrent()
        {
            Assert.That(NodeReference.Root.Parent, Is.EqualTo(null));
        }
    }
}