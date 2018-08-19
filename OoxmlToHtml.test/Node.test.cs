using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace OoxmlToHtml.test
{
    [TestFixture]
    public class NodeTest
    {
        private Node node;

        [SetUp]
        public void Setup()
        {
            node = new Node(KeywordToken.Paragraph);
        }

        [Test]
        public void ShouldAddChild()
        {
            var child = new Node(KeywordToken.PreviousParagraph);

            var oldCount = node.Children.Count;
            node.AddChild(child);

            Assert.AreEqual(0, oldCount);
            Assert.AreEqual(1, node.Children.Count);
        }

        [Test]
        public void ShouldAddChildsParentOnAdd()
        {
            var child = new Node(KeywordToken.PreviousParagraph);

            var oldCount = node.Children.Count;
            node.AddChild(child);

            Assert.AreEqual(node, child.Parent);
        }

        [Test]
        public void ShouldGetAndSetAttribute()
        {
            Assert.Throws<KeyNotFoundException>(() =>
            {
                var c = node["test"];
            });

            node.SetAttribute("test", "testValue");

            Assert.AreEqual("testValue", node["test"]);
            Assert.AreEqual("testValue", node.GetAttribute("test"));
        }

        [Test]
        public void ShouldRenameAttrWhenSettingAttrThatExistsWithRename()
        {
            node.SetAttribute("test", "testValue");
            node.SetAttribute("test", "testValue2");

            Assert.AreEqual("testValue2", node["test_2"]);
            Assert.AreEqual("testValue2", node.GetAttribute("test_2"));
        }

        [Test]
        public void ShouldAppendAttrWhenSettingAttrThatExistsWithAppend()
        {
            node.SetAttribute("test", "testValue");
            node.SetAttribute("test", "testValue2", AttributeMergeStrategy.Append);

            Assert.AreEqual("testValuetestValue2", node["test"]);
            Assert.AreEqual("testValuetestValue2", node.GetAttribute("test"));
        }

        [Test]
        public void ShouldAppendAttrWhenSettingAttrNamedText()
        {
            node.SetAttribute("Text", "testValue");
            node.SetAttribute("Text", "testValue2");

            Assert.AreEqual("testValuetestValue2", node["Text"]);
            Assert.AreEqual("testValuetestValue2", node.GetAttribute("Text"));
        }

        [Test]
        public void ShouldOverwriteAttrWhenSettingAttrThatExistsWithOverwrite()
        {
            node.SetAttribute("test", "testValue");
            node.SetAttribute("test", "testValue2", AttributeMergeStrategy.Overwrite);

            Assert.AreEqual("testValue2", node["test"]);
            Assert.AreEqual("testValue2", node.GetAttribute("test"));
        }

        [Test]
        public void ShouldNotAddAttrWhenSettingAttrThatExistsWithDiffValueUsingMerge()
        {
            node.SetAttribute("test", "testValue");
            var oldAttributeCount = node.Count;
            var result = node.SetAttribute("test", "testValue2", AttributeMergeStrategy.Merge);

            Assert.AreEqual("testValue", node["test"]);
            Assert.AreEqual("testValue", node.GetAttribute("test"));
            Assert.AreEqual(oldAttributeCount, node.Count);
            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldNotAddAttrWhenSettingAttrThatExistsWithSameValueUsingMerge()
        {
            node.SetAttribute("test", "testValue");
            var oldAttributeCount = node.Count;
            var result = node.SetAttribute("test", "testValue", AttributeMergeStrategy.Merge);

            Assert.AreEqual("testValue", node["test"]);
            Assert.AreEqual("testValue", node.GetAttribute("test"));
            Assert.AreEqual(oldAttributeCount, node.Count);
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldAddAttrWhenSettingAttrThatDoesNotExistUsingMerge()
        {
            var result = node.SetAttribute("test", "testValue", AttributeMergeStrategy.Merge);

            Assert.AreEqual("testValue", node["test"]);
            Assert.AreEqual("testValue", node.GetAttribute("test"));
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldRemoveAttribute()
        {
            node.SetAttribute("test", "testValue");

            Assert.AreEqual("testValue", node["test"]);
            node.RemoveAttribute("test");

            Assert.Throws<KeyNotFoundException>(() =>
            {
                var c = node["test"];
            });
        }

        [Test]
        public void ShouldGetAllAttributes()
        {
            node.SetAttribute("test1", "testValue1");
            node.SetAttribute("test2", "testValue2");
            node.SetAttribute("test3", "testValue3");
            node.SetAttribute("test4", "testValue4");
            node.SetAttribute("test5", "testValue5");

            var expected = node.GetAllAttributes;

            Assert.AreEqual(5, expected.Count);
        }

        [Test]
        public void ShouldSetParent()
        {
            var child = new Node(KeywordToken.PreviousParagraph);
            child.SetParent(node);

            Assert.AreEqual(child.Parent, node);
        }

        [Test]
        public void ShouldCopyChildren()
        {
            var sourceNode = new Node(KeywordToken.PreviousParagraph);
            var child1 = new Node(KeywordToken.ParagraphStyle);
            var child1Child1 = new Node(KeywordToken.Color);
            sourceNode.AddChild(child1);
            child1.AddChild(child1Child1);

            var before = node.Children.Count;
            node.CopyChildren(sourceNode);

            Assert.AreEqual(0, before);
            Assert.AreEqual(1, node.Children.Count);
        }

        [Test]
        public void ShouldNotDuplicateChildren()
        {
            var child1 = new Node(KeywordToken.ParagraphStyle);
            node.AddChild(child1);
            node.AddChild(child1);
            node.AddChild(child1);

            Assert.AreEqual(1, node.Children.Count);
        }

        [Test]
        public void ShouldCopyAttributes()
        {
            var sourceNode = new Node(KeywordToken.Color);
            sourceNode.SetAttribute("value", "CC0000");

            Assert.Throws<KeyNotFoundException>(() => { node.GetAttribute("value"); });
            node.CopyAttributes(sourceNode);
            Assert.AreEqual("CC0000", node.GetAttribute("value"));
        }

        [Test]
        public void ShouldNotCopyAttributesWithReservedNames()
        {
            var sourceNode = new Node(KeywordToken.Color);
            sourceNode.SetAttribute("__value", "CC0000");

            node.CopyAttributes(sourceNode);
            Assert.Throws<KeyNotFoundException>(() => node.GetAttribute("__value"));
        }

        [Test]
        public void ShouldRemoveChild()
        {
            var child = new Node(KeywordToken.ParagraphStyle);
            node.AddChild(child);

            int afterAdd = node.Children.Count;

            node.RemoveChild(child);

            Assert.AreEqual(1, afterAdd);
            Assert.AreEqual(0, node.Children.Count);
        }
    }
}