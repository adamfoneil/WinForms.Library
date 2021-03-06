﻿using System;
using System.Collections.Generic;

namespace WinForms.SampleApp.Models
{
    public enum BeltOptions
    {
        White,
        Gold,
        Green,
        Purple,
        Blue,
        Red,
        Brown,
        Black
    }

    public enum ItemKeys
    {
        Binghamton,
        Plopthee,
        Galvatron
    }

    /// <summary>
    /// This is just a bogus model class demonstrating various types for use with DocumentManager
    /// </summary>
    public class AppDocument
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public BeltOptions Belt { get; set; } // combobox from enum
        public DateTime EffectiveDate { get; set; }
        public int LookupValue { get; set; } // combo box of int values
        public decimal Level { get; set; } // numeric updown
        public string[] Items { get; set; } // not sure about this yet
        public string BuilderText { get; set; } // plain text used with builder textbox
        public DocumentItem Item { get; set; }
        public ItemKeys Key { get; set; } // For cases when we bind to a scalar value in the document, but we use corresponding reference types (e.g. SMM SourceType)

        // toolstrip values
        public string ToolStripText { get; set; }
        public int DocumentItemValue { get; set; }

        public static IEnumerable<DocumentItem> SelectableItems
        {
            get
            {
                return new DocumentItem[]
                {
                    new DocumentItem() { Name = "Whatever", Value = 100 },
                    new DocumentItem() { Name = "Blather", Value = 110 },
                    new DocumentItem() { Name = "Hopscotch", Value = 632 },
                    new DocumentItem() { Name = "Branzenfiller", Value = 211 }
                };
            }
        }

        public static Dictionary<ItemKeys, DocumentItem> KeyedItems
        {
            get
            {
                return new Dictionary<ItemKeys, DocumentItem>()
                {
                    { ItemKeys.Binghamton, new DocumentItem() { Name = "Binghamton", Value = 219 } },
                    { ItemKeys.Plopthee, new DocumentItem() { Name = "Plopthee", Value = 198 } },
                    { ItemKeys.Galvatron, new DocumentItem() { Name = "Galvatron", Value = 82 } }
                };
            }
        }
    }

    public class DocumentItem
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            var test = obj as DocumentItem;
            return (test != null) ? test.Value.Equals(Value) : false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}