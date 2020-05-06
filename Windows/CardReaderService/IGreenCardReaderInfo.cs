using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReaderService
{
    public delegate void GreenCardReaderEventHandler(object sender, GreenCardReaderEventArgs e);
    public class GreenCardReaderEventArgs : EventArgs
    {
        /// <summary>
        /// The card reader that hold card id
        /// </summary>
        public IGreenCardReader CardReader { get; set; }

        /// <summary>
        /// Get or set card id
        /// </summary>
        public string CardID { get; set; }
        public DateTime Time { get; set; }
        public string TimeRide { get; set; }
        public string Door { get; set; }
        public string Reader { get; set; }
        public Exception ex { get; set; }
    }
    public delegate void GreenHandButtonEventHandler(object sender, GreenHandButtonEventArgs e);
    public class GreenHandButtonEventArgs : EventArgs
    {
        public string Ip { get; set; }
        public ushort Port { get; set; }
        public DateTime Time { get; set; }
        public byte EventType { get; set; }
        public Exception ex { get; set; }
    }

    public enum CardState
    {
        IsDisable,
        IsReady,
        IsPause,
        IsStop,
        IsReading
    }
    public interface IGreenCardReaderInfo
    {
        /// <summary>
        /// Type can be 'ModWinsCard', 'Tcp Ip Client', 'Tcp Ip Server'
        /// </summary>
        string Type { get; set; }
        string CallName { get; set; }
        /// <summary>
        /// Using when Type ="ModWinsCard"
        /// </summary>
        string DeviceName { get; set; }
        string SerialNumber { get; set; }
        /// <summary>
        /// Using when Type in ['Tcp Ip Client', 'Tcp Ip Server']
        /// </summary>
        string TcpIp { get; set; }
        ushort Port { get; set; }
        byte ActiveCode { get; set; }
        byte InactiveCode { get; set; }

    }
    public interface IGreenCardReader
    {
        IGreenCardReaderInfo Info { get; set; }
        CardState State { get; set; }
        bool Connect();
        void DisConnect();
        Object GetController();
        event GreenCardReaderEventHandler ReadingCompleted;
        event GreenCardReaderEventHandler TakingOffCompleted;
    }
    public interface IGreenCardReaderService
    {
        Dictionary<string, IGreenCardReader> Devices { get; }
        void LoadCarDevice();
        void Connect();
        void Disconnect();
        void Connect(int index);
        void Disconnect(int index);
    }
}
