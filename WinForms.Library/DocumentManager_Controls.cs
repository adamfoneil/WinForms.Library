using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;

namespace WinForms.Library
{
	public partial class DocumentManager<TDocument>
	{
		private bool _suspend = false;
		private List<Action<TDocument>> _setControls = new List<Action<TDocument>>();

		public void AddControl(TextBox control, Action<TDocument> setProperty, Action<TDocument> setControl)
		{
			_setControls.Add(setControl);

			control.TextChanged += delegate (object sender, EventArgs e) 
			{
				if (_suspend) return;
				setProperty.Invoke(Document);
				_isDirty = true;
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
	}
}