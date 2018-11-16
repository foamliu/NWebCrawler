using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using NWebCrawlerLib;
using NWebCrawlerLib.Common;
using NWebCrawlerLib.Interface;

namespace NWebCrawler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        private PerformanceCounter m_cpuCounter;
        private PerformanceCounter m_ramCounter;
        private Downloader m_downloader;
        private DispatcherTimer m_dispatcherTimer;

        #endregion

        #region Delegates

        delegate void NewErrorCallback(string message);
        delegate void NewLogCallback(string message);
        delegate void UpdateDataGridCallback();
        delegate void UpdateStatusStripCallback();
        delegate void UpdateToolStripCallback();

        #endregion

        #region Properties

        // number of bytes downloaded
        private int nByteCount;
        private int ByteCount
        {
            get { return nByteCount; }
            set
            {
                nByteCount = value;
                this.statusBarPanelByteCount.Text = Commas(nByteCount / 1024 + 1) + " KB";
            }
        }

        // number of Uri's found
        private int nURLCount;
        private int URLCount
        {
            get { return nURLCount; }
            set
            {
                nURLCount = value;
                this.statusBarPanelURLs.Text = Commas(nURLCount) + " URL found";
            }
        }

        // available memory
        private float nFreeMemory;
        private float FreeMemory
        {
            get { return nFreeMemory; }
            set
            {
                nFreeMemory = value;
                this.statusBarPanelMem.Text = nFreeMemory + " MB Available";
            }
        }

        // CPU usage
        private int nCPUUsage;
        private int CPUUsage
        {
            get { return nCPUUsage; }
            set
            {
                nCPUUsage = value;
                this.statusBarPanelCPU.Text = "CPU usage " + nCPUUsage + "%";
            }
        }

        // number of files downloaded
        private int nFileCount;
        private int FileCount
        {
            get { return nFileCount; }
            set
            {
                nFileCount = value;
                this.statusBarPanelFiles.Text = Commas(nFileCount) + " file(s) downloaded";
            }
        }

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                m_cpuCounter = new PerformanceCounter();
                m_cpuCounter.CategoryName = "Processor";
                m_cpuCounter.CounterName = "% Processor Time";
                m_cpuCounter.InstanceName = "_Total";

                m_ramCounter = new PerformanceCounter();
                m_ramCounter.CategoryName = "Memory";
                m_ramCounter.CounterName = "Available MBytes";
            }
            catch (FormatException fe)
            {
                Logger.Error(fe.Message);
                Logger.Error(fe.StackTrace);
            }

            m_downloader = new Downloader();
            m_downloader.StatusChanged += new DownloaderStatusChangedEventHandler(DownloaderStatusChanged);

            Logger.NewLogEvent += new NewLogEventHandler(Logger_NewLogEvent);
            Logger.NewErrorEvent += new NewErrorEventHandler(Logger_NewErrorEvent);

            m_dispatcherTimer = new DispatcherTimer();
            m_dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            m_dispatcherTimer.Interval = TimeSpan.FromSeconds(10);
            m_dispatcherTimer.Start();

            this.buttonResume.IsEnabled = false;
            this.buttonStop.IsEnabled = false;
            this.buttonSuspend.IsEnabled = false;
        }

        #region UI Events

        private void Logger_NewErrorEvent(object sender, NewErrorEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(
                    DispatcherPriority.Background,
                    new Action<string>((m) =>
                        {
                            this.listViewErrors.Items.Add(m);
                        }),
                    e.ErrorMessage);
            }
            catch (NullReferenceException) { }
        }

        private void Logger_NewLogEvent(object sender, NewLogEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Background,
                new Action<string>((m) => this.listViewLogs.Items.Add(m)),
                e.LogMessage);
        }

        private void miExit_Click(object sender, RoutedEventArgs e)
        {
            if (m_downloader != null)
                m_downloader.Abort();

            Application.Current.Shutdown();
        }

        private void miSettings_Click(object sender, RoutedEventArgs e)
        {
            ShowSettingsDialog();
        }

        private void miAbout_Click(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (m_downloader != null)
                m_downloader.Abort();

            Application.Current.Shutdown();

            base.OnClosing(e);
        }

        private void DownloaderStatusChanged(object sender, DownloaderStatusChangedEventArgs e)
        {
            UpdateToolStrip();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            this.UpdateStatusStrip();
            if (this.m_downloader.Dirty)
            {
                this.UpdateDataGrid();
                this.m_downloader.Dirty = false;
            }
        }

        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void buttonResume_Click(object sender, RoutedEventArgs e)
        {
            m_downloader.Resume();
        }

        private void buttonSuspend_Click(object sender, RoutedEventArgs e)
        {
            m_downloader.Suspend();
        }

        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            m_downloader.Abort();

            buttonGo.IsEnabled = true;
        }

        private void buttonSettings_Click(object sender, RoutedEventArgs e)
        {
            ShowSettingsDialog();
        }

        private void buttonGo_Click(object sender, RoutedEventArgs e)
        {
            m_downloader.InitSeeds(new string[] { txtSeed.Text });
            m_downloader.Start();
        }

        #endregion

        #region Helpers

        private string Commas(int nNum)
        {
            string str = nNum.ToString();
            int nIndex = str.Length;
            while (nIndex > 3)
            {
                str = str.Insert(nIndex - 3, ",");
                nIndex -= 3;
            }
            return str;
        }

        private void ShowSettingsDialog()
        {
            string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Utility.ExecuteCommandSync("notepad.exe", Path.Combine(folder, "config.ini"));
        }

        private void UpdateDataGrid()
        {
            if (null == m_downloader || null == m_downloader.Crawlers) return;

            Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Background,
                new Action<IEnumerable<CrawlerThread>>((data) =>
                {
                    try
                    {
                        dataGridThreads.ItemsSource = data;
                        dataGridThreads.Items.Refresh();
                    }
                    catch (Exception ex)
                    {
                        File.WriteAllText("err.log", ex.ToString());
                    }
                }),
                m_downloader.Crawlers);
        }

        private void UpdateStatusStrip()
        {
            if (null != m_cpuCounter)
            {
                Application.Current.Dispatcher.Invoke(
                    DispatcherPriority.Background,
                    new Action<string>((v) => this.statusBarPanelCPU.Text = v),
                    string.Format("CPU: {0:0.00}%", m_cpuCounter.NextValue()));
            }
            if (null != m_cpuCounter)
            {
                Application.Current.Dispatcher.Invoke(
                    DispatcherPriority.Background,
                    new Action<string>((v) => this.statusBarPanelMem.Text = v),
                    string.Format("Mem: {0:0.00}MB", m_ramCounter.NextValue()));
            }
            Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Background,
                new Action<string>((v) => this.statusBarPanelURLs.Text = v),
                string.Format("URLs: {0}", m_downloader.UrlsQueueFrontier.Count.ToString()));
            Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Background,
                new Action<string>((v) => this.statusBarPanelFiles.Text = v),
                string.Format("Files: {0}", m_downloader.CrawleHistroy.Count.ToString()));
            Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Background,
                new Action<string>((v) => this.statusBarPanelSpeed.Text = v),
                string.Format("Speed: {0:0.00}KB/sec", m_downloader.GetDownloadSpeed()));
            Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Background,
                new Action<string>((v) => this.statusBarPanelByteCount.Text = v),
                string.Format("Total size: {0:0.00}MB", 1.0 * m_downloader.TotalSize / 1024 / 1024));
        }

        private void UpdateToolStrip()
        {
            Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Background,
                new Action<bool>((b) => this.buttonResume.IsEnabled = b),
                (m_downloader.Status == DownloaderStatusType.Suspended));

            Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Background,
                new Action<bool>((b) => this.buttonGo.IsEnabled = b),
                (m_downloader.Status == DownloaderStatusType.NotStarted));

            Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Background,
                new Action<bool>((b) => this.buttonSuspend.IsEnabled = this.buttonStop.IsEnabled = b),
                (m_downloader.Status == DownloaderStatusType.Running));
        }

        #endregion

    }
}
