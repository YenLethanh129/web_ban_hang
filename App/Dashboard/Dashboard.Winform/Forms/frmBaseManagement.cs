using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dashboard.Winform.Forms
{
    public partial class FrmBaseManagement : Form
    {
        public FrmBaseManagement()
        {
            InitializeComponent();
        }

        protected virtual void InitializeEvents()
        {
            cbxOrderBy.SelectedIndexChanged += (s, o) => CbxOrderBySelectedIndexChanged(s!, o);
            cbxFilterByGoodsStatus.SelectedIndexChanged += (s, o) => CbxFilterByGoodsStatusSelectedIndexChanged(s!, o);
            cbxFilterByStockStatus.SelectedIndexChanged += (s, o) => cbxFilterByStockStatusSelectedIndexChanged(s!, o);
            cbxNumbRecordsPerPage.SelectedIndexChanged += (s, o) => CbxNumbRecordsPerPageSelectedIndexChanged(s!, o);
        }

        protected virtual void CbxNumbRecordsPerPageSelectedIndexChanged(object v, EventArgs o)
        {
            if (cbxNumbRecordsPerPage.SelectedItem != null)
                btnNumbOfRecordShowing.Text = cbxNumbRecordsPerPage.Text;
        }

        protected virtual void cbxFilterByStockStatusSelectedIndexChanged(object v, EventArgs o)
        {
            if (cbxFilterByStockStatus.SelectedItem != null)
                btnFilterByStockStatus.Text = cbxFilterByStockStatus.Text;
        }

        protected virtual void CbxFilterByGoodsStatusSelectedIndexChanged(object s, EventArgs o)
        {
            if (cbxFilterByGoodsStatus.SelectedItem != null)
                btnfilterByGoodsStatus.Text = cbxFilterByGoodsStatus.Text;
        }

        protected virtual void CbxOrderBySelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxOrderBy.SelectedItem != null)
                btnOrderBy.Text = cbxOrderBy.Text;
        }

        public virtual Task WaitForDataLoadingComplete()
        {
            throw new NotImplementedException();
        }
        public void OnDataLoaded(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnPresenterError(object? sender, string errorMessage)
        {
            throw new NotImplementedException();
        }
    }
}
