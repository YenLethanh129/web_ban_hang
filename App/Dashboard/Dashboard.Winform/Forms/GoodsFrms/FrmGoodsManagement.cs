using Dashboard.Winform.Presenters;
using Dashboard.Winform.ViewModels;
using Microsoft.Extensions.Logging;

namespace Dashboard.Winform.Forms.GoodsFrms
{
    public partial class FrmGoodsManagement : FrmBaseManagement<GoodsManagementModel, GoodManagementPresenter>
    {
        #region Fields
        private readonly GoodsManagementModel _model;
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Constructor
        public FrmGoodsManagement(
            IServiceProvider serviceProvider,
            ILogger<FrmGoodsManagement> logger,
            GoodManagementPresenter goodPresenter
        ) : base(logger, goodPresenter)
        {
            _model = _presenter.Model;
            _serviceProvider = serviceProvider;

            InitializeBaseComponents();
            // Setup event handler cho OnDataLoaded
        }
        #endregion
    }
}
