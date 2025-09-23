using Keyence.AutoID.SDK;

using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Ki_ADAS.ThreadADAS
{
    public class BarcodeReader
    {
        public class BarcodeEventArgs : EventArgs
        {
            public string BarcodeData { get; }

            public BarcodeEventArgs(string barcodeData)
            {
                BarcodeData = barcodeData;
            }
        }

        private ReaderAccessor m_reader = new ReaderAccessor();
        private Thread _readThread;
        private volatile bool _run = false;

        public event EventHandler<BarcodeEventArgs> OnBarcodeReceived;
        public event EventHandler<Exception> OnError;

        public BarcodeReader(string ipAddress)
        {
            m_reader.IpAddress = ipAddress;
        }

        public void ConnectBarcodeReader()
        {
            m_reader.Connect();

            if (_readThread == null || !_readThread.IsAlive)
            {
                _run = true;
                _readThread = new Thread(ReadLoop);
                _readThread.IsBackground = true;
                _readThread.Start();
            }
        }

        private void ReadLoop()
        {
            while (_run)
            {
                try
                {
                    string data = m_reader.ExecCommand("LON");

                    if (!_run) break;

                    if (!string.IsNullOrEmpty(data))
                    {
                        OnBarcodeReceived?.Invoke(this, new BarcodeEventArgs(data));
                    }
                }
                catch (Exception ex)
                {
                    if (_run)
                    {
                        _run = false;
                        OnError?.Invoke(this, ex);
                    }
                }

                Thread.Sleep(100); 
            }
        }

        public void Disconnect()
        {
            _run = false;

            m_reader.Disconnect();

            if (_readThread != null && _readThread.IsAlive)
            {
                _readThread.Join(500);
            }
        }
    }
}
