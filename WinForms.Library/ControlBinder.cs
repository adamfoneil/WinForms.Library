using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;
using WinForms.Library.Extensions;
using WinForms.Library.Extensions.ComboBoxes;
using WinForms.Library.Extensions.ToolStrip;
using WinForms.Library.Interfaces;
using WinForms.Library.Models;

namespace WinForms.Library
{
    public delegate void PropertyUpdatedHandler<T>(object sender, T document, string propertyName);

    public class ControlBinder<TDocument>
    {
        private TDocument _document;
        private bool _dirty = false;
        private bool _suspend = false;
        private List<Action<TDocument>> _setControls = new List<Action<TDocument>>();
        private List<Action> _clearControls = new List<Action>();
        private Dictionary<Control, bool> _textChanged = new Dictionary<Control, bool>();        

        public ControlBinder()
        {
        }

        public ControlBinder(TDocument document)
        {
            Document = document;
        }

        public TDocument Document
        {
            get { return _document; }
            set
            {
                _document = value;
                LoadValues();
            }
        }

        public event EventHandler IsDirtyChanged;
        public event EventHandler ClearingValues;
        public event PropertyUpdatedHandler<TDocument> PropertyUpdated;
        public event EventHandler<TDocument> LoadingValues;

        public bool IsDirty
        {
            get { return _dirty; }
            set
            {
                if (_dirty != value)
                {
                    _dirty = value;
                    IsDirtyChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        public void ClearValues()
        {
            _suspend = true;
            foreach (var action in _clearControls) action.Invoke();
            ClearingValues?.Invoke(this, new EventArgs());
            _suspend = false;
            IsDirty = false;
        }

        public void LoadValues()
        {
            _suspend = true;
            foreach (var setter in _setControls) setter.Invoke(Document);
            LoadingValues?.Invoke(this, Document);
            _suspend = false;
            IsDirty = false;
        }

        #region TextBox

        public void Add(TextBoxBase control, Func<TDocument, string> setProperty, Action<TDocument> setControl)
        {
            _setControls.Add(setControl);

            string propertyName = null;

            control.TextChanged += delegate (object sender, EventArgs e)
            {
                if (_suspend) return;
                propertyName = setProperty.Invoke(Document);
                IsDirty = true;
                _textChanged[control] = true;
            };

            control.Leave += delegate (object sender, EventArgs e)
            {
                if (_textChanged.ContainsKey(control) && _textChanged[control])
                {
                    PropertyUpdated?.Invoke(this, Document, propertyName);
                    _textChanged[control] = false;
                }
            };
        }

        public void Add(TextBoxBase control, Expression<Func<TDocument, object>> property)
        {
            PropertyInfo pi = GetProperty(property);
            Func<TDocument, string> setProperty = (doc) =>
            {
                pi.SetValue(doc, control.Text);
                return pi.Name;
            };

            var func = property.Compile();
            Action<TDocument> setControl = (doc) =>
            {
                control.Text = func.Invoke(doc)?.ToString();
            };

            Add(control, setProperty, setControl);
        }

        #endregion TextBox

        #region CheckBox

        public void Add(CheckBox control, Func<TDocument, string> setProperty, Action<TDocument> setControl)
        {
            _setControls.Add(setControl);

            control.CheckedChanged += delegate (object sender, EventArgs e)
            {
                if (_suspend) return;
                var propertyName = setProperty.Invoke(Document);
                IsDirty = true;
                PropertyUpdated?.Invoke(this, Document, propertyName);
            };
        }

        public void Add(CheckBox control, Expression<Func<TDocument, bool>> property)
        {
            PropertyInfo pi = GetProperty(property);
            Func<TDocument, string> setProperty = (doc) =>
            {
                pi.SetValue(doc, control.Checked);
                return pi.Name;
            };

            var func = property.Compile();
            Action<TDocument> setControl = (doc) =>
            {
                control.Checked = func.Invoke(doc);
            };

            Add(control, setProperty, setControl);
        }

        #endregion CheckBox

        #region ComboBox

        public void AddEnum<TEnum>(ComboBox control, Func<TDocument, string> setProperty, Action<TDocument> setControl)
        {
            control.Fill<TEnum>();

            _clearControls.Add(() => control.SelectedIndex = -1);
            _setControls.Add(setControl);

            control.SelectedIndexChanged += delegate (object sender, EventArgs e)
            {
                if (_suspend) return;
                var propertyName = setProperty.Invoke(Document);
                IsDirty = true;
                PropertyUpdated?.Invoke(this, Document, propertyName);
            };
        }

        public void AddEnum<TEnum>(ComboBox control, Expression<Func<TDocument, TEnum>> property)
        {
            PropertyInfo pi = GetProperty(property);
            Func<TDocument, string> setProperty = (doc) =>
            {
                pi.SetValue(doc, (control.SelectedItem as ListItem<TEnum>).Value);
                return pi.Name;
            };

            var func = property.Compile();
            Action<TDocument> setControl = (doc) =>
            {
                ComboBoxExtensions.SetValue(control, func.Invoke(doc));
            };

            AddEnum<TEnum>(control, setProperty, setControl);
        }

        public void AddItems<TItem>(ComboBox control, Expression<Func<TDocument, TItem>> property, IEnumerable<TItem> items) where TItem : class
        {
            PropertyInfo pi = GetProperty(property);
            Func<TDocument, string> setProperty = (doc) =>
            {
                pi.SetValue(doc, (control.SelectedItem as TItem));
                return pi.Name;
            };

            var func = property.Compile();
            Action<TDocument> setControl = (doc) =>
            {
                ComboBoxExtensions.SetItem(control, func.Invoke(doc));
            };

            AddItems(control, setProperty, setControl, items);
        }

        public void AddItems<TItem>(ComboBox control, Func<TDocument, string> setProperty, Action<TDocument> setControl, IEnumerable<TItem> items)
        {
            control.Fill(items);

            _clearControls.Add(() => control.SelectedIndex = -1);
            _setControls.Add(setControl);

            control.SelectedIndexChanged += delegate (object sender, EventArgs e)
            {
                if (_suspend) return;
                var propertyName = setProperty.Invoke(Document);
                IsDirty = true;
                PropertyUpdated?.Invoke(this, Document, propertyName);
            };
        }

        /// <summary>
        /// Fills a combo box from TItem keys of a dictionary, mapping to corresponding TValues that are bound to the document		
        /// </summary>
        public void AddItems<TValue, TItem>(ComboBox control, Expression<Func<TDocument, TValue>> property, Dictionary<TValue, TItem> itemDictionary) where TItem : class
        {
            var pi = GetProperty(property);
            Func<TDocument, string> setProperty = (doc) =>
            {
                var reverseDictionary = itemDictionary.ToDictionary(kp => kp.Value, kp => kp.Key);
                pi.SetValue(doc, reverseDictionary[control.GetItem<TItem>()]);
                return pi.Name;
            };

            var func = property.Compile();
            Action<TDocument> setControl = (doc) =>
            {
                var value = func.Invoke(doc);
                ComboBoxExtensions.SetItem(control, itemDictionary[value]);
            };

            AddItems(control, setProperty, setControl, itemDictionary.Select(kp => kp.Value));
        }

        #endregion

        #region DateTimePicker

        public void Add(DateTimePicker control, Func<TDocument, string> setProperty, Action<TDocument> setControl)
        {
            _setControls.Add(setControl);

            control.ValueChanged += delegate (object sender, EventArgs e)
            {
                if (_suspend) return;
                var propertyName = setProperty.Invoke(Document);
                IsDirty = true;
                PropertyUpdated?.Invoke(this, Document, propertyName);
            };
        }

        public void Add(DateTimePicker control, Expression<Func<TDocument, DateTime>> property)
        {
            PropertyInfo pi = GetProperty(property);
            Func<TDocument, string> setProperty = (doc) =>
            {
                pi.SetValue(doc, control.Value);
                return pi.Name;
            };

            var func = property.Compile();
            Action<TDocument> setControl = (doc) =>
            {
                DateTime value = func.Invoke(doc);
                if (value < DateTimePicker.MinimumDateTime) value = DateTimePicker.MinimumDateTime;
                control.Value = value;
            };

            Add(control, setProperty, setControl);
        }

        #endregion

        #region NumericUpDown

        public void Add(NumericUpDown control, Func<TDocument, string> setProperty, Action<TDocument> setControl)
        {
            _setControls.Add(setControl);

            control.ValueChanged += delegate (object sender, EventArgs e)
            {
                if (_suspend) return;
                var propertyName = setProperty.Invoke(Document);
                IsDirty = true;
                PropertyUpdated?.Invoke(this, Document, propertyName);
            };
        }

        public void Add(NumericUpDown control, Expression<Func<TDocument, decimal>> property)
        {
            PropertyInfo pi = GetProperty(property);
            Func<TDocument, string> setProperty = (doc) =>
            {
                pi.SetValue(doc, control.Value);
                return pi.Name;
            };

            var func = property.Compile();
            Action<TDocument> setControl = (doc) =>
            {
                control.Value = func.Invoke(doc);
            };

            Add(control, setProperty, setControl);
        }

        #endregion

        #region IBoundControl

        public void Add<TValue>(IBoundControl<TValue> control, Func<TDocument, string> setProperty, Action<TDocument> setControl)
        {
            _setControls.Add(setControl);

            control.ValueChanged += delegate (object sender, EventArgs e)
            {
                if (_suspend) return;
                var propertyName = setProperty.Invoke(Document);
                IsDirty = true;
                PropertyUpdated?.Invoke(this, Document, propertyName);
            };
        }

        public void Add<TValue>(IBoundControl<TValue> control, Expression<Func<TDocument, TValue>> property)
        {
            PropertyInfo pi = GetProperty(property);
            Func<TDocument, string> setProperty = (doc) =>
            {
                pi.SetValue(doc, control.Value);
                return pi.Name;
            };

            var func = property.Compile();
            Action<TDocument> setControl = (doc) =>
            {
                control.Value = func.Invoke(doc);
            };

            Add(control, setProperty, setControl);
        }

        #endregion IBoundControl

        #region ToolStripTextBox

        public void Add(ToolStripTextBox control, Func<TDocument, string> setProperty, Action<TDocument> setControl)
        {
            _setControls.Add(setControl);

            control.TextChanged += delegate (object sender, EventArgs e)
            {
                if (_suspend) return;
                var propertyName = setProperty.Invoke(Document);
                IsDirty = true;
                PropertyUpdated?.Invoke(this, Document, propertyName);
            };
        }

        public void Add(ToolStripTextBox control, Expression<Func<TDocument, object>> property)
        {
            PropertyInfo pi = GetProperty(property);
            Func<TDocument, string> setProperty = (doc) =>
            {
                pi.SetValue(doc, control.Text);
                return pi.Name;
            };

            var func = property.Compile();
            Action<TDocument> setControl = (doc) =>
            {
                control.Text = func.Invoke(doc)?.ToString();
            };

            Add(control, setProperty, setControl);
        }

        #endregion

        #region ToolStripComboBox

        public void AddItems<TItem>(ToolStripComboBox control, Func<TDocument, string> setProperty, Action<TDocument> setControl, IEnumerable<TItem> items)
        {
            ToolStripComboBoxExtensions.Fill(control, items);

            _clearControls.Add(() => control.SelectedIndex = -1);
            _setControls.Add(setControl);

            control.SelectedIndexChanged += delegate (object sender, EventArgs e)
            {
                if (_suspend) return;
                var propertyName = setProperty.Invoke(Document);
                IsDirty = true;
                PropertyUpdated?.Invoke(this, Document, propertyName);
            };
        }

        public void AddItems<TItem>(ToolStripComboBox control, Expression<Func<TDocument, TItem>> property, IEnumerable<TItem> items) where TItem : class
        {
            PropertyInfo pi = GetProperty(property);
            Func<TDocument, string> setProperty = (doc) =>
            {
                pi.SetValue(doc, (control.SelectedItem as TItem));
                return pi.Name;
            };

            var func = property.Compile();
            Action<TDocument> setControl = (doc) =>
            {
                ToolStripComboBoxExtensions.SetItem(control, func.Invoke(doc));
            };

            AddItems(control, setProperty, setControl, items);
        }

        #endregion

        public BindingSource Add<TItem>(DataGridView dataGridView, Func<TDocument, IList<TItem>> items)
        {
            _setControls.Add((doc) => InitBinding());

            _clearControls.Add(() =>
            {
                dataGridView.DataSource = null;
                dataGridView.Enabled = false;
            });

            InitBinding();

            dataGridView.CellValueChanged += delegate (object sender, DataGridViewCellEventArgs args) { SetDirty(); };
            DataGridViewRowEventHandler setIsDirty = delegate (object sender, DataGridViewRowEventArgs args) { SetDirty(); };
            dataGridView.UserDeletedRow += setIsDirty;
            dataGridView.UserAddedRow += setIsDirty;

            return dataGridView.DataSource as BindingSource;

            void SetDirty()
            {
                if (_suspend) return;
                IsDirty = true;
            }

            void InitBinding()
            {
                dataGridView.Enabled = true;
                var boundList = items.Invoke(_document);
                if (!boundList?.Any() ?? true) boundList = Enumerable.Empty<TItem>().ToList();
                var bs = new BindingSource();
                bs.DataSource = new BindingList<TItem>(boundList);
                dataGridView.DataSource = bs;                
            }
        }

        #region support methods

        private PropertyInfo GetProperty<TValue>(Expression<Func<TDocument, TValue>> property)
        {
            string propName = PropertyNameFromLambda(property);
            PropertyInfo pi = typeof(TDocument).GetProperty(propName);
            return pi;
        }

        private string PropertyNameFromLambda(Expression expression)
        {
            // thanks to http://odetocode.com/blogs/scott/archive/2012/11/26/why-all-the-lambdas.aspx
            // thanks to http://stackoverflow.com/questions/671968/retrieving-property-name-from-lambda-expression

            LambdaExpression le = expression as LambdaExpression;
            if (le == null) throw new ArgumentException("expression");

            MemberExpression me = null;
            if (le.Body.NodeType == ExpressionType.Convert)
            {
                me = ((UnaryExpression)le.Body).Operand as MemberExpression;
            }
            else if (le.Body.NodeType == ExpressionType.MemberAccess)
            {
                me = le.Body as MemberExpression;
            }

            if (me == null) throw new ArgumentException("expression");

            return me.Member.Name;
        }
        #endregion support methods
    }
}