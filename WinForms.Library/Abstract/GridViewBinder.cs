using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms.Library.Abstract
{
    public abstract class GridViewBinder<TModel> where TModel : class, new()
    {
        private readonly DataGridView _dataGridView;

        private TModel _model = null;
        private bool _modified = false;

        public GridViewBinder(DataGridView dataGridView)
        {
            _dataGridView = dataGridView;
            _dataGridView.AutoGenerateColumns = false;
            _dataGridView.CurrentCellDirtyStateChanged += CellDirtyStateChanged;
            _dataGridView.RowValidated += RowValidated;
            _dataGridView.UserDeletingRow += RowDeleting;
            _dataGridView.UserDeletedRow += RowDeleted;
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

        protected abstract bool SupportsAsync { get; }

        protected abstract void OnSave(TModel model);
        protected abstract void OnDelete(TModel model);

        protected abstract Task OnSaveAsync(TModel model);
        protected abstract Task OnDeleteAsync(TModel model);

        private async void RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (_modified)
            {
                if (_model != null)
                {
                    if (SupportsAsync)
                    {
                        await OnSaveAsync(_model);
                    }
                    else
                    {
                        OnSave(_model);
                    }                    
                    
                    _model = null;
                }
                _modified = false;
            }
        }

        private void CellDirtyStateChanged(object sender, EventArgs e)
        {
            if (_dataGridView.IsCurrentCellDirty)
            {
                _modified = true;
                _model = _dataGridView.CurrentRow.DataBoundItem as TModel;
            }
        }

        private async void RowDeleted(object sender, DataGridViewRowEventArgs e)
        {
            if (_model != null)
            {
                if (SupportsAsync)
                {
                    await OnDeleteAsync(_model);
                }
                else
                {
                    OnDelete(_model);
                }                
                
                _model = null;
            }
        }

        private void RowDeleting(object sender, DataGridViewRowCancelEventArgs e)
        {
            _model = e.Row.DataBoundItem as TModel;
        }
    }
}
