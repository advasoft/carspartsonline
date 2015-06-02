
namespace StoreAppTest.Controls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using DevExpress.Data.Utils.ServiceModel;
    using DevExpress.Xpf.Grid;
    using DevExpress.Xpf.Printing;
    using DevExpress.Xpf.Printing.BrickCollection;
    using DevExpress.Xpf.Printing.Native;
    using DevExpress.XtraPrinting;
    using DevExpress.XtraPrinting.DataNodes;

    public class ExtendedGrid : DataGrid//, IPrintableControl
    {
        public event EventHandler OnEnterKeyPresses = (sender, args) => { };

        #region Overrides of Control

        /// <summary>
        /// Вызывается перед возникновением события <see cref="E:System.Windows.UIElement.KeyDown"/>.
        /// </summary>
        /// <param name="e">The data for the event. </param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            // Stop "Enter" selecting the next row in the grid
            if (e.Key == Key.Enter)
            {
                OnEnterKeyPresses(this, new EventArgs());
                e.Handled = true;
            }
            base.OnKeyDown(e);
        }

        #endregion

        //#region Implementation of IPrintableControl

        ///// <summary>
        ///// <para>
        ///// [To be supplied] 
        ///// </para>
        ///// </summary>
        ///// <param name="usablePageSize">A <see cref="T:System.Drawing.Size"/> structure.
        /////             </param><param name="reportHeaderSize">A <see cref="T:System.Drawing.Size"/> structure.
        /////             </param><param name="reportFooterSize">A <see cref="T:System.Drawing.Size"/> structure.
        /////             </param><param name="pageHeaderSize">A <see cref="T:System.Drawing.Size"/> structure.
        /////             </param><param name="pageFooterSize">A <see cref="T:System.Drawing.Size"/> structure.
        /////             </param>
        //void IPrintableControl.CreateRootNodeAsync(Size usablePageSize, Size reportHeaderSize, Size reportFooterSize, Size pageHeaderSize,
        //    Size pageFooterSize)
        //{
        //    CreatePrintingTreeNodeAsync(this, usablePageSize, (MasterDetailPrintInfo)null);
        //}


        ///// <summary>
        ///// <para>
        ///// [To be supplied] 
        ///// </para>
        ///// </summary>
        ///// <param name="usablePageSize">A <see cref="T:System.Drawing.Size"/> structure.
        /////             </param><param name="reportHeaderSize">A <see cref="T:System.Drawing.Size"/> structure.
        /////             </param><param name="reportFooterSize">A <see cref="T:System.Drawing.Size"/> structure.
        /////             </param><param name="pageHeaderSize">A <see cref="T:System.Drawing.Size"/> structure.
        /////             </param><param name="pageFooterSize">A <see cref="T:System.Drawing.Size"/> structure.
        /////             </param>
        ///// <returns>
        ///// An object implementing the <b>DevExpress.XtraPrinting.DataNodes.IRootDataNode</b> interface.
        ///// </returns>
        //public IRootDataNode CreateRootNode(Size usablePageSize, Size reportHeaderSize, Size reportFooterSize, Size pageHeaderSize,
        //    Size pageFooterSize)
        //{
            
        //}

        ///// <summary>
        ///// <para>
        ///// [To be supplied] 
        ///// </para>
        ///// </summary>
        ///// <returns>
        ///// An object implementing the <b>DevExpress.Xpf.Printing.BrickCollection.IVisualTreeWalker</b> interface.
        ///// </returns>
        //public IVisualTreeWalker GetCustomVisualTreeWalker()
        //{
            
        //}

        //public void PagePrintedCallback(IEnumerator pageBrickEnumerator, Dictionary<IVisualBrick, IOnPageUpdater> brickUpdaters)
        //{
            
        //}

        ///// <summary>
        ///// <para>
        ///// [To be supplied] 
        ///// </para>
        ///// </summary>
        ///// <value>
        ///// [To be supplied] 
        ///// </value>
        //public bool CanCreateRootNodeAsync
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //public event EventHandler<ScalarOperationCompletedEventArgs<IRootDataNode>> CreateRootNodeCompleted;

        //#endregion


        //private void CreatePrintingTreeNodeAsync(ExtendedGrid extendedGrid, Size usablePageSize, MasterDetailPrintInfo masterDetailPrintInfo)
        //{
        //    //GridPrintingHelper.ClearMasterDetailCache(masterDetailPrintInfo);
        //    ItemsGenerationAsyncServerModeStrategyAsync itemsGenerationStrategy = new ItemsGenerationAsyncServerModeStrategyAsync((DataViewBase)view);
        //    itemsGenerationStrategy.StartFetchingAllFilteredAndSortedRows((Action)(() => view.RaiseCreateRootNodeCompleted(GridPrintingHelper.CreatePrintingTreeNode((ITableView)view, usablePageSize, masterDetailPrintInfo, (ItemsGenerationStrategyBase)itemsGenerationStrategy))));
        //}

    }


    //public class ItemsGenerationAsyncServerModeStrategyAsync : ItemsGenerationServerStrategy
    //{
    //    private IList allFilteredAndSortedRows = (IList)new List<object>();
    //    private DispatcherTimer fetchingTimer;

    //    public ItemsGenerationAsyncServerModeStrategyAsync(DataViewBase view)
    //        : base(view)
    //    {
    //    }

    //    protected override IList FetchAllFilteredAndSortedRows()
    //    {
    //        return this.allFilteredAndSortedRows;
    //    }

    //    public void StartFetchingAllFilteredAndSortedRows(Action createPrintingNodeAction)
    //    {
    //        Dispatcher uiDispatcher = this.View.Dispatcher;
    //        AsyncServerModeDataController asyncDataController = (AsyncServerModeDataController)this.DataProvider.DataController;
    //        CommandGetAllFilteredAndSortedRows commandGetRows = asyncDataController.Server.GetAllFilteredAndSortedRows();
    //        DXWindow progressWindow = ProgressControl.CreateProgressWindow((ManualResetEvent)null, false, this.View.GetLocalizedString(GridControlStringId.ProgressWindowTitle), this.View.GetLocalizedString(GridControlStringId.ProgressWindowCancel));
    //        progressWindow.Closed += (EventHandler)((s, e) => asyncDataController.Server.Cancel((Command)commandGetRows));
    //        progressWindow.Show();
    //        this.fetchingTimer = new DispatcherTimer();
    //        this.fetchingTimer.Interval = TimeSpan.FromMilliseconds(100.0);
    //        this.fetchingTimer.Tick += (EventHandler)((s, e) =>
    //        {
    //            if (!this.fetchingTimer.IsEnabled || !asyncDataController.Server.WaitFor((Command)commandGetRows))
    //                return;
    //            this.fetchingTimer.Stop();
    //            this.allFilteredAndSortedRows = commandGetRows.IsCanceled ? (IList)new List<object>() : commandGetRows.Rows;
    //            progressWindow.Close();
    //            uiDispatcher.BeginInvoke(createPrintingNodeAction);
    //        });
    //        this.fetchingTimer.Start();
    //    }
    //}


    
    //public abstract class ItemsGenerationServerStrategy : ItemsGenerationStrategyBase
    //{
    //private IList allFilteredAndSortedRows;

    //protected IList AllFilteredAndSortedRows
    //{
    //    get
    //    {
    //    if (this.allFilteredAndSortedRows == null)
    //        this.allFilteredAndSortedRows = this.GetAllFilteredAndSortedRows();
    //    return this.allFilteredAndSortedRows;
    //    }
    //}

    //public override bool RequireFullExpand
    //{
    //    get
    //    {
    //    return true;
    //    }
    //}

    //    public ItemsGenerationServerStrategy(ExtendedGrid view)
    //        : base(view)
    //    {
    //        //DataGrid gd = new DataGrid();
    //    }
    

    //protected virtual IList GetAllFilteredAndSortedRows()
    //{
    //    //if (!((TableView) this.View).PrintSelectedRowsOnly)
    //    //  return this.FetchAllFilteredAndSortedRows();
    //    return GetSelectedRows(this.View);
    //}

    //protected IList GetSelectedRows(ExtendedGrid view)
    //{
    //    return view.ItemsSource as IList;
    //}

    //protected abstract IList FetchAllFilteredAndSortedRows();

    //public override void PrepareDataControllerAndPerformPrintingAction(Action printingAction)
    //{
    //    //TODO: Maybe here
    //    //SubstituteDataControllerForPrintingAndPerformPrintingAction(this.AllFilteredAndSortedRows, this.View.PrintAllGroupsCore, printingAction);
    //}

    //public override object GetRowValue(int rowData)
    //{
    //    return this.AllFilteredAndSortedRows[rowData];
    //}

    //public override object GetCellValue(int cellData)
    //{
    //    DataGridColumn actualColumnInfo = View.Columns[cellData];
    //    if (actualColumnInfo == null)
    //    return (object) null;
    //    return actualColumnInfo.PropertyDescriptor.GetValue(this.AllFilteredAndSortedRows[cellData.RowData.DataRowNode.PrintInfo.ListIndex]);
    //}
    //}


    //public abstract class ItemsGenerationStrategyBase
    //{
    //    private ExtendedGrid view;

    //    protected ExtendedGrid View
    //    {
    //        get
    //        {
    //        return this.view;
    //        }
    //    }

    //    //protected DataProviderBase DataProvider
    //    //{
    //    //  get
    //    //  {
    //    //    return this.view.DataProviderBase;
    //    //  }
    //    //}

    //    public abstract bool RequireFullExpand { get; }

    //    public ItemsGenerationStrategyBase(ExtendedGrid view)
    //    {
    //        this.view = view;
        
    //    }

    //    public abstract void PrepareDataControllerAndPerformPrintingAction(Action printingAction);

    //    public abstract object GetRowValue(int rowData);

    //    public abstract object GetCellValue(int cellData);
    //}
}
