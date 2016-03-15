﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PeterPiper.Hl7.V2.Model;
using PeterPiper.Hl7.V2.Support;
using System.Collections.ObjectModel;


namespace TestHl7V2
{
  [TestFixture]
  public class TestComponent
  {
    public MessageDelimiters CustomDelimiters;
    string ComponentTestString = "Cat\\T\\Mice&Dogs\\T\\Cats";

    [SetUp]
    public void MyTestInitialize()
    {
      CustomDelimiters = new MessageDelimiters('#', '@', '*', '!', '%');
    }

    /// <summary>
    ///A test for Component Constructor
    ///</summary>
    [Test]
    public void ComponentConstructorTest()
    {
      var target = Creator.Component(ComponentTestString);
      Assert.AreEqual("Cat&Mice&Dogs&Cats", target.AsString, "A test for Component Constructor");
      Assert.AreEqual("Cat\\T\\Mice&Dogs\\T\\Cats", target.AsStringRaw, "A test for Component Constructor");
    }

    /// <summary>
    ///A test for Component Constructor
    ///</summary>
    [Test]
    public void ComponentConstructorTest1()
    {
      string StringRaw = "Cat%T%Mice!Dogs%T%Cats";
      var target = Creator.Component(StringRaw, CustomDelimiters);
      Assert.AreEqual("Cat!Mice!Dogs!Cats", target.AsString, "A test for Component Constructor 1");
      Assert.AreEqual("Cat%T%Mice!Dogs%T%Cats", target.AsStringRaw, "A test for Component Constructor 1");
    }

    /// <summary>
    ///A test for Component Constructor
    ///</summary>
    [Test]
    public void ComponentConstructorTest2()
    {
      var target = Creator.Component();
      Assert.AreEqual(String.Empty, target.AsString, "A test for Component Constructor 2");
      Assert.AreEqual(true, target.IsEmpty, "A test for Component Constructor 2");
      Assert.AreEqual(String.Empty, target.SubComponent(1).AsStringRaw, "A test for Component Constructor 2");
      Assert.AreEqual(true, target.SubComponent(1).IsEmpty, "A test for Component Constructor 2");

    }    

    /// <summary>
    ///A test for Add
    ///</summary>
    [Test]
    public void AddTest1()
    {
      var target = Creator.Component("Comp1");
      var item = Creator.SubComponent("Sub1");
      target.Add(item);
      Assert.AreEqual("Comp1&Sub1", target.AsStringRaw, "A test for Add 1");
    }

    /// <summary>
    ///A test for Add
    ///</summary>
    [Test]
    public void AddTest2()
    {
      var target = Creator.Component("Comp1");
      var item = Creator.Content(PeterPiper.Hl7.V2.Support.Standard.EscapeType.NewLine);
      target.Add(item);
      Assert.AreEqual("Comp1\\.br\\", target.AsStringRaw, "A test for Add 2");
    }

    /// <summary>
    ///A test for ClearAll
    ///</summary>
    [Test]
    public void ClearAllTest()
    {
      var target = Creator.Component("Comp1&Sub1&Sub2\\T\\2");
      target.ClearAll();
      Assert.AreEqual(String.Empty, target.AsStringRaw, "A test for ClearAll");
      Assert.AreEqual(true, target.IsEmpty, "A test for ClearAll");
    }

    /// <summary>
    ///A test for Clone
    ///</summary>
    [Test]
    public void CloneTest()
    {      
      var target = Creator.Component("Comp1&Sub1&Sub2\\T\\2");
      var expected = Creator.Component("Comp1&Sub1&Sub2\\T\\2");      
      var actual = target.Clone();
      Assert.AreEqual(expected.AsStringRaw, actual.AsStringRaw, "A test for Clone");
    }

    /// <summary>
    ///A test for Content
    ///</summary>
    [Test]
    public void ContentTest()
    {
      var target = Creator.Component("Hello \\T\\ World \\F\\ Bye");
      int index = 3;
      var expected = Creator.Content(PeterPiper.Hl7.V2.Support.Standard.EscapeType.Field);      
      var actual = target.Content(index);
      Assert.AreEqual(expected.AsStringRaw, actual.AsStringRaw, "A test for Content");

    }

    /// <summary>
    ///A test for Insert
    ///</summary>
    [Test]
    public void InsertTest()
    {
      var target = Creator.Component("Hello&World&Bye");
      int index = 2;
      var item = Creator.SubComponent("Earth");
      target.Insert(index, item);
      Assert.AreEqual("Hello&Earth&World&Bye", target.AsStringRaw, "A test for Insert");
    }

    /// <summary>
    ///A test for Insert
    ///</summary>
    [Test]
    public void InsertTest1()
    {
      var target = Creator.Component("Hello\\R\\World\\R\\Bye");
      int index = 3;
      var item = Creator.Content(PeterPiper.Hl7.V2.Support.Standard.EscapeType.Field);
      target.Insert(index, item);
      Assert.AreEqual("Hello~World|~Bye", target.AsString, "A test for Insert");
    }

    /// <summary>
    ///A test for RemoveContentAt
    ///</summary>
    [Test]
    public void RemoveContentAtTest()
    {
      var target = Creator.Component("Hello\\R\\World\\R\\Bye");
      int index = 3; // TODO: Initialize to an appropriate value
      target.RemoveContentAt(index);
      Assert.AreEqual("Hello~WorldBye", target.AsString, "A test for RemoveContentAt");
    }

    /// <summary>
    ///A test for RemoveSubComponentAt
    ///</summary>
    [Test]
    public void RemoveSubComponentAtTest()
    {
      var target = Creator.Component("Hello!World!Bye", CustomDelimiters);
      int index = 2; // TODO: Initialize to an appropriate value
      target.RemoveSubComponentAt(index);
      Assert.AreEqual("Hello!Bye", target.AsString, "A test for RemoveSubComponentAt");
    }

    /// <summary>
    ///A test for Set
    ///</summary>
    [Test]
    public void SetTest()
    {
      var target = Creator.Component("Hello\\R\\World\\R\\Bye&SecondComponent");
      int index = 1;
      var item1 = Creator.Content(PeterPiper.Hl7.V2.Support.Standard.EscapeType.NewLine);
      var item2 = Creator.Content(PeterPiper.Hl7.V2.Support.Standard.EscapeType.Indent);
      target.Set(index, item1);
      Assert.AreEqual("HelloWorld~Bye&SecondComponent", target.AsString, "A test for Set");
      Assert.AreEqual(".br", target.Content(1).AsStringRaw, "A test for Set");
      target.Set(10, item2);
      Assert.AreEqual("Hello\\.br\\World\\R\\Bye\\.in\\&SecondComponent", target.AsStringRaw, "A test for Set");
    }

    /// <summary>
    ///A test for Set
    ///</summary>
    [Test]
    public void SetTest1()
    {
      var target = Creator.Component("Hello\\R\\World\\R\\Bye&SecondComponent&ThirdComponent");
      int index = 2;
      var item = Creator.SubComponent("NewSub");
      var item2 = Creator.SubComponent("NewSub2");
      target.Set(index, item);
      Assert.AreEqual("NewSub", target.SubComponent(2).AsStringRaw, "A test for Set 2");
      target.Set(10, item2);
      Assert.AreEqual("NewSub2", target.SubComponent(10).AsStringRaw, "A test for Set 2");

    }

    /// <summary>
    ///A test for SubComponent
    ///</summary>
    [Test]
    public void SubComponentTest()
    {
      var target = Creator.Component("Hello\\R\\World\\R\\Bye&SecondComponent");
      int index = 2;
      var expected = Creator.SubComponent("SecondComponent");      
      var actual = target.SubComponent(index);
      Assert.AreEqual(expected.AsStringRaw, actual.AsStringRaw, "A test for SubComponent");
    }

    /// <summary>
    ///A test for ToString
    ///</summary>
    [Test]
    public void ToStringTest()
    {
      var target = Creator.Component("Hello\\R\\World\\R\\Bye&SecondComponent");
      string expected = "Hello~World~Bye&SecondComponent";
      string actual;
      actual = target.ToString();
      Assert.AreEqual(expected, actual, "A test for ToString");
    }

    /// <summary>
    ///A test for AsString
    ///</summary>
    [Test]
    public void AsStringTest()
    {
      var target = Creator.Component("Hello\\R\\World\\R\\Bye&SecondComponent");
      string expected = "Hello~World~Bye&SecondComponent"; // TODO: Initialize to an appropriate value
      string actual;
      target.AsString = expected;
      actual = target.AsString;
      Assert.AreEqual(expected, actual, "A test for ToString");

      target.AsString = "left^right";
      expected = "left\\S\\right";
      Assert.AreEqual(expected, target.AsStringRaw, "A test for AsString escaping");

    }

    /// <summary>
    ///A test for AsStringRaw
    ///</summary>
    [Test]
    public void AsStringRawTest()
    {
      var target = Creator.Component("Hello\\R\\World\\R\\Bye&SecondComponent");
      string expected = "Hello\\R\\World\\R\\Bye&SecondComponent";
      string actual;
      target.AsStringRaw = expected;
      actual = target.AsStringRaw;
      Assert.AreEqual(expected, actual, "A test for ToStringRaw");

    }

    /// <summary>
    ///A test for ContentCount
    ///</summary>
    [Test]
    public void ContentCountTest()
    {
      var target = Creator.Component("Hello\\R\\World\\R\\Bye&SecondComponent");
      int actual;
      actual = target.ContentCount;
      Assert.AreEqual(5, target.ContentCount, "A test for ContentCount");
    }

    /// <summary>
    ///A test for IsEmpty
    ///</summary>
    [Test]
    public void IsEmptyTest()
    {
      var target = Creator.Component(); // TODO: Initialize to an appropriate value
      bool actual;
      actual = target.IsEmpty;
      Assert.AreEqual(true, target.IsEmpty, "A test for IsEmpty");
      target.AsString = "Hello";
      Assert.AreEqual(false, target.IsEmpty, "A test for IsEmpty");
    }

    /// <summary>
    ///A test for IsHL7Null
    ///</summary>
    [Test]
    public void IsHL7NullTest()
    {
      var target = Creator.Component("\"\"");
      bool actual;
      actual = target.IsHL7Null;
      Assert.AreEqual(true, target.IsHL7Null, "A test for IsHL7Null");
      Assert.AreEqual(PeterPiper.Hl7.V2.Support.Standard.Null.HL7Null, target.AsString, "A test for IsHL7Null");
      target.AsString = "Hello";
      Assert.AreEqual(false, target.IsHL7Null, "A test for IsHL7Null");

    }

    /// <summary>
    ///A test for SubComponentCount
    ///</summary>
    [Test]
    public void SubComponentCountTest()
    {
      var target = Creator.Component("one&two&three&four");
      int actual;
      actual = target.SubComponentCount;
      Assert.AreEqual(4, actual, "A test for SubComponentCount");
    }

    /// <summary>
    ///A test for SubComponentList
    ///</summary>
    [Test]
    public void SubComponentListTest()
    {
      var target = Creator.Component("one&two&three&four");
      ReadOnlyCollection<ISubComponent> actual;
      actual = target.SubComponentList;
      Assert.AreEqual(4, actual.Count, "A test for SubComponentList");
      Assert.AreEqual("one", actual[0].AsString, "A test for SubComponentList");
      Assert.AreEqual("two", actual[1].AsString, "A test for SubComponentList");
      Assert.AreEqual("three", actual[2].AsString, "A test for SubComponentList");
      Assert.AreEqual("four", actual[3].AsString, "A test for SubComponentList");
    }

    /// <summary>
    ///A test for PathInformation
    ///</summary>
    [Test]
    public void PathInformationTest()
    {      
      var target = Creator.Component("one&two&three&four");
      Assert.AreEqual("<unk>-?", target.PathDetail.PathBrief, "A test for SubComponentList");
      Assert.AreEqual("<unk>-?.?.2", target.SubComponent(2).PathDetail.PathBrief, "A test for SubComponentList");
      Assert.AreEqual("<unk>-?.?.2 [0]", target.SubComponent(2).Content(0).PathDetail.PathBrief, "A test for SubComponentList");
    }

    /// <summary>
    ///A test for MessageDelimiters
    ///</summary>
    [Test]
    public void DelimterAccessTest()
    {
      var target = Creator.Component("o\\R\\ne&two&three&four");
      Assert.AreEqual('~', target.MessageDelimiters.Repeat, "A test for MessageDelimiters");
      Assert.AreEqual('|', target.MessageDelimiters.Field, "A test for MessageDelimiters");
      Assert.AreEqual('^', target.MessageDelimiters.Component, "A test for MessageDelimiters");
      Assert.AreEqual('&', target.MessageDelimiters.SubComponent, "A test for MessageDelimiters");
      Assert.AreEqual('\\', target.MessageDelimiters.Escape, "A test for MessageDelimiters");
    }

  }
}
