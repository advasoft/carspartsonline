
namespace StoreAppTest.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using Event;
    using Microsoft.Practices.Prism.PubSubEvents;
    using Microsoft.Practices.ServiceLocation;

    public partial class ClosableTabItem
    {
        private Button PART_btnClose;

        private IEventAggregator _agregator;
        private RequestCloseTabEvent _requestCloseTabEvent;

        public ClosableTabItem()
        {
            _agregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _requestCloseTabEvent = _agregator.GetEvent<RequestCloseTabEvent>();

            InitializeComponent();
        }

        #region Overrides of TabItem

        /// <summary>
        /// При переопределении в производном классе вызывается каждый раз, когда код приложения или внутренние процессы (например, повторное построение прохода макета) вызывают метод <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>. Вкратце это означает, что данный метод вызывается непосредственно перед отображением элемента пользовательского интерфейса в приложении. Дополнительные сведения см. в разделе "Примечания".
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_btnClose = GetTemplateChild("PART_btnClose") as Button;

            if (PART_btnClose != null)
            {
                PART_btnClose.Click += new RoutedEventHandler(OnCloseButtonClick);
            }
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            _requestCloseTabEvent.Publish(this);
        }

        #endregion
    }
}
