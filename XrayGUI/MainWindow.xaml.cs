using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace XrayGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _vlessUri = string.Empty;
        private bool _isConnected = false;
        private VlessProfile _profile = new();
        private XrayProcess _xray;

        private readonly XrayDownloader _xrayDownloader = new();
        private readonly WindowsProxyService _proxyService = new();

        private readonly Brush _connectedBrush = Brushes.Green;
        private readonly Brush _disconnectedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8A8A8A"));

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Window Control Events
        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion

        #region Core Logic Methods
        private bool LoadVlessUri()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Imagens|*.png;*.jpg;*.jpeg;*.bmp",
                Multiselect = false
            };

            if (dialog.ShowDialog() != true)
                return false;

            var reader = new QRCodeReader();
            string uri = reader.ReadFromFile(dialog.FileName);

            if (string.IsNullOrEmpty(uri))
            {
                MessageBox.Show("Não foi possível ler o QR Code.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            _vlessUri = uri;
            return true;
        }

        private void MountVlessProfile()
        {
            _profile = VlessParser.Parse(_vlessUri);
            var json = XrayConfigBuilder.Build(_profile);

            string configPath = Path.Combine(AppContext.BaseDirectory, "Core", "config.json");
            File.WriteAllText(configPath, json);
        }

        private void ConnectXray()
        {
            StatusText.Text = "Conectando...";

            _xray?.Start();
            _proxyService.EnableProxy();
            _isConnected = true;

            BtOpenQr.Content = "Desconectar";
            StatusSymbol.Foreground = _connectedBrush;
            StatusText.Text = "Conectado";
        }

        private void DisconnectXray()
        {
            _proxyService.DisableProxy();
            _xray?.Stop();
            _isConnected = false;

            BtOpenQr.Content = "Conectar";
            StatusSymbol.Foreground = _disconnectedBrush;
            StatusText.Text = "Desconectado";
        }

        #endregion

        #region UI Interaction Events
        public async void BtOpenQr_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_vlessUri))
            {
                if (!LoadVlessUri())
                    return;

                MountVlessProfile();
                await _xrayDownloader.EnsureInstalled();

                _xray = new XrayProcess();
                BtOpenQr.Content = "Conectar";
                return;
            }

            if (!_isConnected)
            {
                ConnectXray();
            }
            else
            {
                DisconnectXray();
            }
        }

        #endregion
    }
}