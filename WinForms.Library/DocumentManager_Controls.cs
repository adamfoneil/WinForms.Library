using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;
using WinForms.Library.Extensions;
using WinForms.Library.Interfaces;
using WinForms.Library.Models;

namespace WinForms.Library
{
	public partial class JsonSDI<TDocument>
	{
		#region TextBox

		public void AddControl(TextBox control, Action<TDocument> setProperty, Action<TDocument> setControl)
		{
			_setControls.Add(setControl);

			control.TextChanged += delegate (object sender, EventArgs e)
			{
				if (_suspend) return;
				setProperty.Invoke(Document);
				IsDirty = true;
			};
		}

		public void AddControl(TextBox control, Expression<Func<TDocument, object>> property)
		{
			PropertyInfo pi = GetProperty(property);
			Action<TDocument> setProperty = (doc) =>
			{
				pi.SetValue(doc, control.Text);
			};

			var func = property.Compile();
			Action<TDocument> setControl = (doc) =>
			{
				control.Text = func.Invoke(doc)?.ToString();
			};

			AddControl(control, setProperty, setControl);
		}

		#endregion TextBox

		#region CheckBox

		public void AddControl(CheckBox control, Action<TDocument> setProperty, Action<TDocument> setControl)
		{
			_setControls.Add(setControl);

			control.CheckedChanged += delegate (object sender, EventArgs e)
			{
				if (_suspend) return;
				setProperty.Invoke(Document);
				IsDirty = true;
			};
		}

		public void AddControl(CheckBox control, Expression<Func<TDocument, bool>> property)
		{
			PropertyInfo pi = GetProperty(property);
			Action<TDocument> setProperty = (doc) =>
			{
				pi.SetValue(doc, control.Checked);
			};

			var func = property.Compile();
			Action<TDocument> setControl = (doc) =>
			{
				control.Checked = func.Invoke(doc);
			};

			AddControl(control, setProperty, setControl);
		}

		#endregion CheckBox

		#region ComboBox

		public void AddControl<TEnum>(ComboBox control, Action<TDocument> setProperty, Action<TDocument> setControl)
		{
			control.Fill<TEnum>();

			_setControls.Add(setControl);

			control.SelectedIndexChanged += delegate (object sender, EventArgs e)
			{
				if (_suspend) return;
				setProperty.Invoke(Document);
				IsDirty = true;
			};
		}

		public void AddControl<TEnum>(ComboBox control, Expression<Func<TDocument, TEnum>> property)
		{
			PropertyInfo pi = GetProperty(property);
			Action<TDocument> setProperty = (doc) =>
			{
				pi.SetValue(doc, (control.SelectedItem as EnumValue<TEnum>).Value);
			};

			var func = property.Compile();
			Action<TDocument> setControl = (doc) =>
			{
				ComboBoxExtensions.SetValue(control, func.Invoke(doc));
			};

			AddControl<TEnum>(control, setProperty, setControl);
		}

		public void AddControl<TItem>(ComboBox control, Expression<Func<TDocument, TItem>> property, IEnumerable<TItem> items) where TItem : class
		{
			PropertyInfo pi = GetProperty(property);
			Action<TDocument> setProperty = (doc) =>
			{
				pi.SetValue(doc, (control.SelectedItem as TItem));
			};

			var func = property.Compile();
			Action<TDocument> setControl = (doc) =>
			{
				ComboBoxExtensions.SetItem(control, func.Invoke(doc));
			};

			AddControl(control, setProperty, setControl, items);
		}

		public void AddControl<TItem>(ComboBox control, Action<TDocument> setProperty, Action<TDocument> setControl, IEnumerable<TItem> items)
		{
			control.Fill(items);

			_setControls.Add(setControl);

			control.SelectedIndexChanged += delegate (object sender, EventArgs e)
			{
				if (_suspend) return;
				setProperty.Invoke(Document);
				IsDirty = true;
			};
		}

		#endregion

		#region DateTimePicker

		public void AddControl(DateTimePicker control, Action<TDocument> setProperty, Action<TDocument> setControl)
		{
			_setControls.Add(setControl);

			control.ValueChanged += delegate (object sender, EventArgs e)
			{
				if (_suspend) return;
				setProperty.Invoke(Document);
				IsDirty = true;
			};
		}

		public void AddControl(DateTimePicker control, Expression<Func<TDocument, DateTime>> property)
		{
			PropertyInfo pi = GetProperty(property);
			Action<TDocument> setProperty = (doc) =>
			{
				pi.SetValue(doc, control.Value);
			};

			var func = property.Compile();
			Action<TDocument> setControl = (doc) =>
			{
				DateTime value = func.Invoke(doc);
				if (value < DateTimePicker.MinimumDateTime) value = DateTimePicker.MinimumDateTime;
				control.Value = value;
			};

			AddControl(control, setProperty, setControl);
		}

		#endregion

		#region NumericUpDate

		public void AddControl(NumericUpDown control, Action<TDocument> setProperty, Action<TDocument> setControl)
		{
			_setControls.Add(setControl);

			control.ValueChanged += delegate (object sender, EventArgs e)
			{
				if (_suspend) return;
				setProperty.Invoke(Document);
				IsDirty = true;
			};
		}

		public void AddControl(NumericUpDown control, Expression<Func<TDocument, decimal>> property)
		{
			PropertyInfo pi = GetProperty(property);
			Action<TDocument> setProperty = (doc) =>
			{
				pi.SetValue(doc, control.Value);
			};

			var func = property.Compile();
			Action<TDocument> setControl = (doc) =>
			{				
				control.Value = func.Invoke(doc);
			};

			AddControl(control, setProperty, setControl);
		}

		#endregion

		#region IBoundControl

		public void AddControl<TValue>(IBoundControl<TValue> control, Action<TDocument> setProperty, Action<TDocument> setControl)
		{
			_setControls.Add(setControl);

			control.ValueChanged += delegate (object sender, EventArgs e)
			{
				if (_suspend) return;
				setProperty.Invoke(Document);
				_dirty = true;
			};
		}

		public void AddControl<TValue>(IBoundControl<TValue> control, Expression<Func<TDocument, TValue>> property)
		{
			PropertyInfo pi = GetProperty(property);
			Action<TDocument> setProperty = (doc) =>
			{
				pi.SetValue(doc, control.Value);
			};

			var func = property.Compile();
			Action<TDocument> setControl = (doc) =>
			{
				control.Value = func.Invoke(doc);
			};

			AddControl(control, setProperty, setControl);
		}

		#endregion IBoundControl

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