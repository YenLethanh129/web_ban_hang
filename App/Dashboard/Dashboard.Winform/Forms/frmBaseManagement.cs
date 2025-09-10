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
    public interface IFrmBaseManagement
    {
        Task WaitForDataLoadingComplete();
        void InitializeEvents();
        void OnDataLoaded(object? sender, EventArgs e);
        void OnPresenterError(object? sender, string errorMessage);


    }
    public partial class FrmBaseManagement : Form, IFrmBaseManagement
    {
        public FrmBaseManagement()
        {
            InitializeComponent();
        }

        private void InitializeEvents()
        {
            //btnOrderBy.Click += BtnOrderBy_Click;
            //btnfilterByGoodsStatus.Click += BtnOrderBy_Click;
            //cbxOrderBy.SelectedIndexChanged += cbxOrderBy_SelectedIndexChanged;
            //cbxFilterStatus.SelectedIndexChanged += cbxFilterStatus_SelectedIndexChanged;
            //cbxFilterByGoodStatus.SelectedIndexChanged += cbxFilterByGoodStatus_SelectedIndexChanged;
            //cbxNumbRecordsPerPage.SelectedIndexChanged += cbxNumbRecordsPerPage_SelectedIndexChanged;
        }

        private void BtnOrderBy_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void cbxOrderBy_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbxFilterStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbxFilterByGoodStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbxNumbRecordsPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public Task WaitForDataLoadingComplete()
        {
            throw new NotImplementedException();
        }

        void IFrmBaseManagement.InitializeEvents()
        {
            InitializeEvents();
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
