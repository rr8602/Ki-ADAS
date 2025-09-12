using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ki_ADAS.VEPBench
{
    public class VEPBenchTransmissionZone : IVEPBenchZone
    {
        private static VEPBenchTransmissionZone _instance;
        private static readonly object _lock = new object();

        // 주소값
        public const ushort Offset_Reserved1 = 0;
        public const ushort Offset_Reserved2 = 1;
        public const ushort Offset_AddTSize = 2;
        public const ushort Offset_ExchStatus = 3;
        public const ushort Offset_Reserved3 = 4;
        public const ushort Offset_Reserved4 = 5;
        public const ushort Offset_FctAndPCNum = 6;
        public const ushort Offset_Reserver5 = 7;
        public const ushort Offset_ProcessAndSubFct = 8;
        public const ushort Offset_Reserved6 = 9;
        public const ushort Offset_Reserved7 = 10;
        public const ushort Offset_Reserved8 = 11;
        public const ushort Offset_DataStart = 12;

        // 기능 코드
        public const int FctCode_PJI = 6;
        public const int FctCode_ReportPrint = 15;
        public const int FctCode_WorkInfo = 20;

        // 교환 상태
        public const ushort ExchStatus_Response = 1; // 요청 없음
        public const ushort ExchStatus_Request = 2; // 요청

        private bool _isChanged;
        public bool IsChanged => _isChanged;

        private ushort[] _values;

        public ushort AddTzSize
        {
            get => _values.Length > Offset_AddTSize ? _values[Offset_AddTSize] : (ushort)0;
            set
            {
                if (_values[Offset_AddTSize] != value)
                {
                    _values[Offset_AddTSize] = value;
                    _isChanged = true;
                }
            }
        }

        public ushort ExchStatus
        {
            get => _values.Length > Offset_ExchStatus ? _values[Offset_ExchStatus] : (ushort)0;
            set
            {
                if (_values[Offset_ExchStatus] != value)
                {
                    _values[Offset_ExchStatus] = value;
                    _isChanged = true;
                }
            }
        }

        public byte FctCode
        {
            get => (byte)(_values.Length > Offset_FctAndPCNum ? _values[Offset_FctAndPCNum] & 0xFF : 0);
            set
            {
                ushort current = _values.Length > Offset_FctAndPCNum ? _values[Offset_FctAndPCNum] : (ushort)0;
                ushort next = (ushort)((current & ~0xFF) | value);
                if (current != next)
                {
                    _values[Offset_FctAndPCNum] = next;
                    _isChanged = true;
                }
            }
        }

        public byte PCNum
        {
            get => (byte)(_values.Length > Offset_FctAndPCNum ? (_values[Offset_FctAndPCNum] >> 8) & 0xFF : 0);
            set
            {
                ushort current = _values.Length > Offset_FctAndPCNum ? _values[Offset_FctAndPCNum] : (ushort)0;
                ushort next = (ushort)((current & ~0xFF00) | (value << 8));
                if (current != next)
                {
                    _values[Offset_FctAndPCNum] = next;
                    _isChanged = true;
                }
            }
        }

        public byte ProcessCode
        {
            get => (byte)(_values.Length > Offset_ProcessAndSubFct ? _values[Offset_ProcessAndSubFct] & 0xFF : 0);
            set
            {
                ushort current = _values.Length > Offset_ProcessAndSubFct ? _values[Offset_ProcessAndSubFct] : (ushort)0;
                ushort next = (ushort)((current & ~0xFF) | value);
                if (current != next)
                {
                    _values[Offset_ProcessAndSubFct] = next;
                    _isChanged = true;
                }
            }
        }

        public byte SubFctCode
        {
            get => (byte)(_values.Length > Offset_ProcessAndSubFct ? (_values[Offset_ProcessAndSubFct] >> 8) & 0xFF : 0);
            set
            {
                ushort current = _values.Length > Offset_ProcessAndSubFct ? _values[Offset_ProcessAndSubFct] : (ushort)0;
                ushort next = (ushort)((current & ~0xFF00) | (value << 8));
                if (current != next)
                {
                    _values[Offset_ProcessAndSubFct] = next;
                    _isChanged = true;
                }
            }
        }

        public ushort[] Data
        {
            get
            {
                if (_values == null || _values.Length <= Offset_DataStart) return new ushort[0];

                ushort[] data = new ushort[_values.Length - Offset_DataStart];
                Array.Copy(_values, Offset_DataStart, data, 0, data.Length);

                return data;
            }
            set
            {
                int dataSize = value?.Length ?? 0;
                ushort[] newValues = new ushort[Offset_DataStart + dataSize];

                if (_values != null)
                {
                    Array.Copy(_values, 0, newValues, 0, Math.Min(Offset_DataStart, _values.Length));
                }

                if (value != null)
                {
                    Array.Copy(value, 0, newValues, Offset_DataStart, dataSize);
                }

                if (_values == null || !newValues.SequenceEqual(_values))
                {
                    _values = newValues;
                    _isChanged = true;
                }
            }
        }

        public int TotalSize => _values?.Length ?? 0;

        public void ResetChangedState()
        {
            _isChanged = false;
        }

        public ushort this[int index]
        {
            get
            {
                if (index < 0 || _values == null || index >= _values.Length)
                    throw new IndexOutOfRangeException("Index out of range for VEPBenchTransmissionZone values.");
                return _values[index];
            }
            set
            {
                if (index < 0 || _values == null || index >= _values.Length)
                    throw new IndexOutOfRangeException("Index out of range for VEPBenchTransmissionZone values.");
                if (_values[index] != value)
                {
                    _values[index] = value;
                    _isChanged = true;
                }
            }
        }

        public static VEPBenchTransmissionZone Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new VEPBenchTransmissionZone();
                    }
                }
                return _instance;
            }
        }

        private VEPBenchTransmissionZone(int dataSize = 48)
        {
            _values = new ushort[Offset_DataStart + dataSize];
            ExchStatus = ExchStatus_Response;
            AddTzSize = 0;
            FctCode = 0;
            PCNum = 1; // 고정값
            ProcessCode = 0;
            SubFctCode = 0;
            _isChanged = false;
        }

        public void FromRegisters(ushort[] registers)
        {
            if (registers == null)
                throw new ArgumentException("Invalid register array.");

            if (_values == null || !registers.SequenceEqual(_values))
            {
                _values = (ushort[])registers.Clone(); // only for value type (no reference type)
                _isChanged = true;
            }
        }

        public ushort[] ToRegisters()
        {
            ushort[] result = new ushort[_values.Length];
            Array.Copy(_values, result, _values.Length);
            return result;
        }

        public void SetRequestMode()
        {
            ExchStatus = ExchStatus_Request;
        }

        public void SetResponseMode()
        {
            ExchStatus = ExchStatus_Response;
        }

        public void SetValue(int index, ushort value)
        {
            this[index] = value;
        }

        public int GetValue(int index)
        {
            return this[index];
        }

        public bool IsRequest => ExchStatus == ExchStatus_Request;
        public bool IsResponse => ExchStatus == ExchStatus_Response;

        public string GetFctCodeString()
        {
            switch (FctCode)
            {
                case FctCode_PJI:
                    return "PJI";
                case FctCode_ReportPrint:
                    return "Report Print";
                case FctCode_WorkInfo:
                    return "Work Info";
                default:
                    return "Unknown Function Code";
            }
        }
    }
}