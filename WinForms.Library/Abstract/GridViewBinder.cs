﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms.Library.Abstract
{
    public delegate void GridViewActionHandler<TModel>(TModel model);
    public delegate void GridViewActionExceptionHandler<TModel>(TModel model, Exception exception);

    public abstract class GridViewBinder<TModel> where TModel : class, new()
    {
        private readonly DataGridView _dataGridView;

        private TModel _model = null;
        private bool _modified = false;
        private int _rowIndex = -1;

        public GridViewBinder(DataGridView dataGridView)
        {
            _dataGridView = dataGridView;
            _dataGridView.AutoGenerateColumns = false;
            _dataGridView.CurrentCellDirtyStateChanged += CellDirtyStateChanged;
            _dataGridView.RowValidated += RowValidated;
            _dataGridView.UserDeletingRow += RowDeleting;
            _dataGridView.UserDeletedRow += RowDeleted;
            _dataGridView.RowValidating += RowValidating;
        }

        public event GridViewActionHandler<TModel> ModelSaved;
        public event GridViewActionHandler<TModel> ModelDeleted;
        public event GridViewActionExceptionHandler<TModel> ModelSaveException;
        public event GridViewActionExceptionHandler<TModel> ModelDeleteException;

        private void RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            var row = _dataGridView.Rows[e.RowIndex];
            var record = row.DataBoundItem as TModel;
            if (record != null)
            {
                var valid = IsValid(record, out string message);
                row.ErrorText = (valid) ? null : message;
                e.Cancel = !valid;
            }
        }

        protected virtual bool IsValid(TModel record, out string message)
        {
            message = null;
            return true;
        }

        public void Fill(IEnumerable<TModel> rows)
        {
            BindingList<TModel> list = new BindingList<TModel>();
            foreach (var record in rows) list.Add(record);

            BindingSource bs = new BindingSource();
            bs.DataSource = list;
            _dataGridView.DataSource = bs;

            if (rows.Any()) _model = rows.First();
        }

        public IEnumerable<TModel> GetRows()
        {
            return (_dataGridView.DataSource as BindingSource).OfType<TModel>();
        }

        protected abstract bool SupportsAsync { get; }

        protected abstract void OnSave(TModel model);
        protected abstract void OnDelete(TModel model);

        protected abstract Task OnSaveAsync(TModel model);
        protected abstract Task OnDeleteAsync(TModel model);

        protected virtual bool DeleteCancelPrompt(TModel model) { return false; }

        private async void RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (_modified && _model != null && _rowIndex == e.RowIndex)
            {
                try
                {
                    if (SupportsAsync)
                    {
                        await OnSaveAsync(_model);
                    }
                    else
                    {
                        OnSave(_model);
                    }

                    ModelSaved?.Invoke(_model);
                    _dataGridView.Rows[e.RowIndex].ErrorText = null;
                    _modified = false;
                }
                catch (Exception exc)
                {
                    _dataGridView.Rows[e.RowIndex].ErrorText = exc.Message;
                    ModelSaveException?.Invoke(_model, exc);
                }

                _model = null;
            }
        }

        private void CellDirtyStateChanged(object sender, EventArgs e)
        {
            if (_dataGridView.IsCurrentCellDirty)
            {
                _rowIndex = _dataGridView.CurrentCell.RowIndex;
                _modified = true;
                _model = _dataGridView.CurrentRow.DataBoundItem as TModel;
            }
        }

        private async void RowDeleted(object sender, DataGridViewRowEventArgs e)
        {
            if (_model != null)
            {
                try
                {
                    if (SupportsAsync)
                    {
                        await OnDeleteAsync(_model);
                    }
                    else
                    {
                        OnDelete(_model);
                    }
                    ModelDeleted?.Invoke(_model);
                }
                catch (Exception exc)
                {
                    ModelDeleteException?.Invoke(_model, exc);
                    if (ModelDeleteException == null)
                    {
                        MessageBox.Show($"Error deleting record: " + exc.Message);
                    }
                }

                _model = null;
            }
        }

        private void RowDeleting(object sender, DataGridViewRowCancelEventArgs e)
        {
            _model = e.Row.DataBoundItem as TModel;
            if (DeleteCancelPrompt(_model)) e.Cancel = true;
        }
    }
}
