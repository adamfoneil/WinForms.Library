using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;
using WinForms.Library.Interfaces;

namespace WinForms.Library
{
	public partial class DocumentManager<TDocument>
	{
		#region TextBox
		public void AddControl(TextBox control, Action<TDocument> setProperty, Action<TDocument> setControl)
		{
			_setControls.Add(setControl);

			control.TextChanged += delegate (object sender, EventArgs e) 
			{
				if (_suspend) return;
				setProperty.Invoke(Document);
				_dirty = true;
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
		#endregion

		#region CheckBox
		public void AddControl(CheckBox control, Action<TDocument> setProperty, Action<TDocument> setControl)
		{
			_setControls.Add(setControl);

			control.CheckedChanged += delegate (object sender, EventArgs e)
			{
				if (_suspend) return;
				setProperty.Invoke(Document);
				_dirty = true;
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
		#endregion

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
		#endregion
	}
}