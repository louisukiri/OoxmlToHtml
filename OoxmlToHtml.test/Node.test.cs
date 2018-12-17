using System;
using System.Collections.Generic;
using NUnit.Framework;
using OoxmlToHtml.test.Helpers;

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
        public void ShouldAddChildToTree()
        {
            var child = new Node(KeywordToken.PreviousParagraph);
            node.AddChild(child);

            Assert.AreEqual(node.Child, child);
        }

        [Test]
        public void ShouldChildToMainChildNextWhenChildExists()
        {
            var child = new Node(KeywordToken.PreviousParagraph);
            var child2 = new Node(KeywordToken.Run);
            
            node.AddChild(child);
            node.AddChild(child2);

            Assert.AreEqual(child.Next, child2);
            Assert.AreEqual(child2.Previous, child);
        }

        [Test]
        public void ShouldAddParentToNonMainChildToo()
        {
            var child = new Node(KeywordToken.PreviousParagraph);
            var child2 = new Node(KeywordToken.Run);

            node.AddChild(child);
            node.AddChild(child2);
            
            Assert.AreEqual(child2.Parent, node);
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
        public void ShouldRemoveOnlyChild()
        {
            var child = new Node(KeywordToken.ParagraphStyle);
            node.AddChild(child);

            int afterAdd = node.Children.Count;

            node.RemoveChild(child);

            Assert.AreEqual(1, afterAdd);
            Assert.AreEqual(0, node.Children.Count);
        }


        [Test]
        public void ShouldRemoveFirstChild()
        {
            var child = new Node(KeywordToken.ParagraphStyle);
            var child2 = new Node(KeywordToken.Run);
            node.AddChild(child);
            node.AddChild(child2);

            int afterAdd = node.Children.Count;

            node.RemoveChild(child);

            Assert.AreEqual(2, afterAdd);
            Assert.AreEqual(1, node.Children.Count);
        }

        [Test]
        public void ShouldRemoveNonFirstChild()
        {
            var child = new Node(KeywordToken.ParagraphStyle);
            var child2 = new Node(KeywordToken.Run);
            node.AddChild(child);
            node.AddChild(child2);

            int afterAdd = node.Children.Count;

            node.RemoveChild(child2);

            Assert.AreEqual(2, afterAdd);
            Assert.AreEqual(1, node.Children.Count);
        }

        [Test]
        public void ShouldRemoveMiddleChild()
        {
            var child = new Node(KeywordToken.ParagraphStyle);
            var child2 = new Node(KeywordToken.Run);
            var child3 = new Node(KeywordToken.Text);
            node.AddChild(child);
            node.AddChild(child2);
            node.AddChild(child3);

            int afterAdd = node.Children.Count;

            node.RemoveChild(child2);

            Assert.AreEqual(3, afterAdd);
            Assert.AreEqual(2, node.Children.Count);
        }

        [Test]
        public void ShouldPrintExpectedToString()
        {
            var testNode = TestHelper.ParseString(@"<w:body>
                                                    <w:p>
                                                        <w:pPr><w:pStyle w:val = ""Title""/></w:pPr>
                                                        <w:p>
                                                            <w:t>TestTItle</w:t>
                                                         </w:p>
                                                    </w:p>
                                                </w:body>");
            var expected = @"<body><paragraph><previousparagraph><paragraphstyle value='Title' /></previousparagraph><paragraph><text><stringliteral text='TestTItle' /></text></paragraph></paragraph></body>";
            Console.WriteLine(testNode.ToString());
            Assert.AreEqual(expected, testNode.ToString());
        }

        [Test]
        public void ShouldIndicateNodeNotEqualGivenDifferentAttributes()
        {
            var testNode = TestHelper.ParseString(@"<w:body>
	                <w:p w:rsidR=""00644911"" w:rsidRPr=""00C63EA3"" w:rsidRDefault=""00644911"" w:rsidP=""00644911"">
                        <w:r w:val=""test"">
                            <w:rPr>
                                <w:rStyle w:val=""TitleChar""/>
                            </w:rPr>
                            <w:t>Te</w:t>
                        </w:r>
                        <w:r w:rsidRPr=""00C63EA3""><w:rPr><w:rStyle w:val=""TitleChar""/></w:rPr>
                            <w:t>sting this testing thing</w:t>
                        </w:r>
                    </w:p>
                                                </w:body>");
            Assert.IsFalse(testNode.Child.Child.Equals(testNode.Child.Child.Next));
        }

        [Test]
        public void ShouldIndicateNodeEqualGivenTypeAndAttribMatch()
        {
            var testNode = TestHelper.ParseString(@"<w:body>
	                <w:p w:rsidR=""00644911"" w:rsidRPr=""00C63EA3"" w:rsidRDefault=""00644911"" w:rsidP=""00644911"">
                        <w:r>
                            <w:rPr>
                                <w:rStyle w:val=""TitleChar""/>
                            </w:rPr>
                            <w:t>Te</w:t>
                        </w:r>
                        <w:r><w:rPr><w:rStyle w:val=""TitleChar""/></w:rPr>
                            <w:t>sting this testing thing</w:t>
                        </w:r>
                    </w:p>
                                                </w:body>");
            Assert.IsTrue(testNode.Child.Child.Equals(testNode.Child.Child.Next));
        }

        [Test]
        public void ShouldHaveExpectedChildren()
        {
            var testNode = TestHelper.ParseString(@"<w:body>
	                <w:p>
                    </w:p>
                    <w:p></w:p>
                 </w:body>");
            Assert.AreEqual(2, testNode.Children.Count);
            Assert.IsNotNull(testNode.Child.Next);
        }

        [Test]
        public void ShouldIndicateNodeNotEqualGivenTypeMisMatch()
        {
            var testNode = TestHelper.ParseString(@"<w:body>
	                <w:p w:rsidR=""00644911"" w:rsidRPr=""00C63EA3"" w:rsidRDefault=""00644911"" w:rsidP=""00644911"">
                        <w:p>
                            <w:rPr>
                                <w:rStyle w:val=""TitleChar""/>
                            </w:rPr>
                            <w:t>Te</w:t>
                        </w:p>
                        <w:r w:rsidRPr=""00C63EA3""><w:rPr><w:rStyle w:val=""TitleChar""/></w:rPr>
                            <w:t>sting this testing thing</w:t>
                        </w:r>
                    </w:p>
                                                </w:body>");
            Assert.IsFalse(testNode.Child.Child.Equals(testNode.Child.Child.Next));
        }

        //[Test]
        //public void ShouldMoveChildrenDuringMerge()
        //{
        //    var testNode = TestHelper.ParseString(@"<w:body>
	       //         <w:p w:rsidR=""00644911"" w:rsidRPr=""00C63EA3"" w:rsidRDefault=""00644911"" w:rsidP=""00644911"">
        //                <w:p>
        //                    <w:rPr>
        //                        <w:rStyle w:val=""TitleChar""/>
        //                    </w:rPr>
        //                    <w:t>Te</w:t>
        //                </w:p>
        //                <w:r w:rsidRPr=""00C63EA3""><w:rPr><w:rStyle w:val=""TitleChar""/></w:rPr>
        //                    <w:t>sting this testing thing</w:t>
        //                </w:r>
        //            </w:p>
        //                                        </w:body>");
        //    Assert.IsFalse(testNode.child.child.Equals(testNode.child.child.Next));
        //}
    }
}