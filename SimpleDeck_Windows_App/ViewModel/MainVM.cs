using SimpleDeck_Windows_App.Utilities;

namespace SimpleDeck_Windows_App.ViewModel;

public class MainVM : ObservableObject
{

    public RelayCommand ConnectionViewCommand { get; set; }
    public RelayCommand BindingsViewCommand { get; set; }
    public RelayCommand SettingsViewCommand { get; set; }
    public RelayCommand AboutViewCommand { get; set; }
    
    public ConnectionVM connectionVM { get; set; }
    public BindingsVM bindingsVM { get; set; }
    public SettingsVM settingsVM { get; set; }
    public AboutVM aboutVM { get; set; }
    
    private object _currentView;

    public object CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value ?? throw new ArgumentNullException(nameof(value)); 
            OnPropertyChanged();
        }
    }
    
    public MainVM()
    {
        connectionVM = new ConnectionVM();
        bindingsVM = new BindingsVM();
        settingsVM = new SettingsVM();
        aboutVM = new AboutVM();
        
        
        CurrentView = connectionVM;

        bindCommandsToViews();
        
    }

    private void bindCommandsToViews()
    {
        ConnectionViewCommand = new RelayCommand(o =>
        {
            CurrentView = connectionVM;
        });

        SettingsViewCommand = new RelayCommand(o =>
        {
            CurrentView = settingsVM;
        });

        BindingsViewCommand = new RelayCommand(o =>
        {
            CurrentView = bindingsVM;
        });
        
        AboutViewCommand = new RelayCommand(o =>
        {
            CurrentView = aboutVM;
        });
    }
}