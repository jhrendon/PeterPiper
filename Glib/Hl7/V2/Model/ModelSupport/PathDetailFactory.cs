﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glib.Hl7.V2.Model;
using Glib.Hl7.V2.Support.Content;

namespace Glib.Hl7.V2.Model.ModelSupport
{
  internal static class PathDetailFactory
  {
    public static PathDetailBase GetPathDetail(ModelBase item)
    {
      return LoadPathDetailData(item);      
    }
    private static PathDetailBase LoadPathDetailData(ModelBase item)
    {
      PathDetailBase _Info = new PathDetailBase();  
      if (item is Glib.Hl7.V2.Model.Content)
      {
        GetContentInfo(item, _Info);
      }
      else if (item is Glib.Hl7.V2.Model.SubComponent)
      {
        GetSubComponentInfo(item, _Info);
      }
      else if (item is Glib.Hl7.V2.Model.Component)
      {
        GetComponentInfo(item, _Info);
      }
      else if (item is Glib.Hl7.V2.Model.Field)
      {
        GetFieldInfo(item, _Info);
      }
      else if (item is Glib.Hl7.V2.Model.Element)
      {
        GetElementInfo(item, _Info);
      }
      else if (item is Glib.Hl7.V2.Model.Segment)
      {
        GetSegmentInfo(item, _Info);
      }
      else if (item is Glib.Hl7.V2.Model.Message)
      {
        GetMessageInfo(item, _Info);
      }
      return _Info;
    }
    private static void GetContentInfo(Glib.Hl7.V2.Model.ModelBase item, PathDetailBase _Info)
    {
      var Content = item as Glib.Hl7.V2.Model.Content;
      _Info._ContentPosition = Content._Index;        
      if (item._Parent != null)
        GetSubComponentInfo(item._Parent,_Info);     
    }
    private static void GetSubComponentInfo(Glib.Hl7.V2.Model.ModelBase item, PathDetailBase _Info)
    {
      var SubComponent = item as Glib.Hl7.V2.Model.SubComponent;
      _Info._SubComponentPosition = SubComponent._Index;
      if (item._Parent != null)
        GetComponentInfo(item._Parent, _Info);
    }
    private static void GetComponentInfo(Glib.Hl7.V2.Model.ModelBase item, PathDetailBase _Info)
    {
      var Component = item as Glib.Hl7.V2.Model.Component;
      _Info._ComponentPosition = Component._Index;
      _Info._SubComponentCount = Component.SubComponentCount;
      if (item._Parent != null)
        GetFieldInfo(item._Parent, _Info);
    }
    private static void GetFieldInfo(Glib.Hl7.V2.Model.ModelBase item, PathDetailBase _Info)
    {
      var Field = item as Glib.Hl7.V2.Model.Field;
      _Info._RepeatPosition = Field._Index;
      _Info._ComponentCount = Field.ComponentCount;
      if (item._Parent != null)
        GetElementInfo(item._Parent, _Info);
    }
    private static void GetElementInfo(Glib.Hl7.V2.Model.ModelBase item, PathDetailBase _Info)
    {
      var Element = item as Glib.Hl7.V2.Model.Element;
      _Info._FieldPosition = Element._Index;
      _Info._RepeatCount = Element.RepeatCount;
      if (item._Parent != null)
        GetSegmentInfo(item._Parent, _Info);
    }
    private static void GetSegmentInfo(Glib.Hl7.V2.Model.ModelBase item, PathDetailBase _Info)
    {
      var Segment = item as Glib.Hl7.V2.Model.Segment;
      int SegmentCount = 0;
      if (Segment._Parent != null)
      {
        var Msg = Segment._Parent as Glib.Hl7.V2.Model.Message;
        SegmentCount = Msg.SegmentCount(Segment.Code); 
      }
      _Info._SegmentPosition = Segment._Index;
      _Info._SegmentTypePosition = SegmentCount;
      _Info._SegmentCode = Segment.Code;      
      if (item._Parent != null)
        GetMessageInfo(item._Parent, _Info);
    }
    private static void GetMessageInfo(Glib.Hl7.V2.Model.ModelBase item, PathDetailBase _Info)
    {
      var Message = item as Glib.Hl7.V2.Model.Message;
      _Info._MessageType = Message.MessageType;
      _Info._MessageEvent = Message.MessageTrigger;
    }
  }  
}
 
