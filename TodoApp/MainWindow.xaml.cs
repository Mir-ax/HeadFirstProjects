using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TodoApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private BindingList<TaskModel> _taskList;
    private readonly string PATH = $"{Environment.CurrentDirectory}\\TodoTaskList.json";
    private FileIOService _fileService;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        _fileService = new FileIOService(PATH);

        try
        {
           _taskList = _fileService.LoadData();
        }
        catch (Exception ex)
        {

            MessageBox.Show(ex.Message);
        }

        taskGrid.ItemsSource = _taskList;
        _taskList.ListChanged += _taskList_ListChanged;
    }

    private void _taskList_ListChanged(object? sender, ListChangedEventArgs e)
    {
        if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted || e.ListChangedType == ListChangedType.ItemChanged)
        {
            try
            {
                _fileService.SaveData(sender);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}