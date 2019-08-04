using System.Windows.Forms;

namespace WinForms.Library
{
    /// <summary>
    /// adapted from https://www.c-sharpcorner.com/UploadFile/c5c6e2/datagridview-autocomplete-textbox/
    /// </summary>
    public class GridCellAutoComplete
    {
        private readonly DataGridView _dataGridView;
        private readonly int _colIndex;
        private readonly AutoCompleteStringCollection _items;

        private bool _showControl = false;

        public GridCellAutoComplete(DataGridViewTextBoxColumn column, string[] items)
        {
            _colIndex = column.Index;
            _dataGridView = column.DataGridView;
            _dataGridView.CellEnter += CellEnter;
            _dataGridView.EditingControlShowing += EditControlShowing;

            // tried to do this originally with items queried dynamically via callback, but ran into access violations
            // see https://stackoverflow.com/questions/8779557/dynamically-changing-textboxs-autocomplete-list-causes-accessviolationexception
            // this was good enough for my purposes

            _items = new AutoCompleteStringCollection();
            _items.AddRange(items);
        }

        private void CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            _showControl = (e.ColumnIndex == _colIndex);
        }

        private void EditControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (_showControl)
            {
                var textBox = e.Control as TextBox;
                textBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                textBox.AutoCompleteCustomSource = _items;
            }
        }
    }
}
