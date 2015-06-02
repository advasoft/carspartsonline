
namespace StoreAppTest.Views.Tasks
{
    using System.Linq;
    using System.Windows.Controls;
    using System.Windows.Input;
    using ViewModels.Tasks;

    public partial class RemaindersWarehouseAmountView
    {
        private RemaindersWarehouseAmountViewModel _vm;

        public RemaindersWarehouseAmountView()
        {
            InitializeComponent();
            Loaded += RemaindersWarehouseAmountView_Loaded;
        }

        void RemaindersWarehouseAmountView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _vm = (RemaindersWarehouseAmountViewModel) DataContext;
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_vm.PriceItems != null)
            {
                var groups = from g in _vm.PriceItems
                    group g by new {PriceList = g.PriceList_Name}
                    into grps
                    select new
                    {
                        PriceList = grps.Key.PriceList,
                        Amount = grps.Sum(s => (int)(s.Remainders*s.WholesalePrice))
                    };

                StoresStackPanel.Children.Clear();

                foreach (var @group in groups){
                    var stack = new StackPanel();
                    stack.Orientation = Orientation.Horizontal;

                    var label = new TextBlock();
                    label.Width = 150;
                    label.FontSize = 20;
                    label.Text = @group.PriceList;
                    stack.Children.Add(label);

                    var value = new TextBlock();
                    value.FontSize = 20;
                    value.Text = @group.Amount.ToString("###,###");
                    stack.Children.Add(value);

                    StoresStackPanel.Children.Add(stack);
                }

            }
        }
    }
}
