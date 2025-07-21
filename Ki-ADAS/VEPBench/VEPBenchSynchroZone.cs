using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ki_ADAS.VEPBench
{
    public class VEPBenchSynchroZone
    {
        private const int DEFAULT_SYNCHRO_SIZE = 40;

        private ushort[] _values;
        public int Size => _values.Length;

        public ushort this[int index]
        {
            get
            {
                if (index < 0 || index >= _values.Length)
                    throw new IndexOutOfRangeException("Index out of range for VEPBenchSynchro values.");
                
                return _values[index];
            }
            set
            {
                if (index < 0 || index >= _values.Length)
                    throw new IndexOutOfRangeException("Index out of range for VEPBenchSynchro values.");

                _values[index] = value;
            }
        }

        public double Angle1
        {
            get => _values.Length > 0 ? _values[0] / 100.0 : 0.0;
            set => _values[0] = (ushort)(value * 100);
        }

        public double Angle2
        {
            get => _values.Length > 1 ? _values[1] / 100.0 : 0.0;
            set => _values[1] = (ushort)(value * 100);
        }

        public double Angle3
        {
            get => _values.Length > 2 ? _values[2] / 100.0 : 0.0;
            set => _values[2] = (ushort)(value * 100);
        }

        public VEPBenchSynchroZone(int size = DEFAULT_SYNCHRO_SIZE)
        {
            _values = new ushort[size];
            ResetAllValues();
        }

        public static VEPBenchSynchroZone FormUshortArray(ushort[] arr)
        {
            if (arr == null)
                throw new ArgumentNullException(nameof(arr));

            var synchro = new VEPBenchSynchroZone(arr.Length);
            Array.Copy(arr, synchro._values, arr.Length);

            return synchro;
        }

        public ushort[] ToUshortArray()
        {
            ushort[] result = new ushort[_values.Length];
            Array.Copy(_values, result, _values.Length);

            return result;
        }

        // 새 사이클 시작하기 전 모든 동기화 값 0으로 초기화
        public void ResetAllValues()
        {
            for (int i = 0; i < _values.Length; i++)
            {
                _values[i] = 0;
            }
        }

        public void SetValue(int index, ushort value)
        {
            this[index] = value;
        }

        public ushort GetValue(int index)
        {
            return this[index];
        }
    }
}
