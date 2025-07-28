using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ki_ADAS.VEPBench
{
    public class VEPBenchSynchroZone
    {
        private static VEPBenchSynchroZone _instance;
        private static readonly object _lock = new object();

        public const int SYNCHRO_SIZE_PART1 = 123; // 125 / 65 크기로 잘라서 사용
        public const int SYNCHRO_SIZE_PART2 = 67;
        public const int DEFAULT_SYNCHRO_SIZE = SYNCHRO_SIZE_PART1 + SYNCHRO_SIZE_PART2;

        // 디바이스 타입 인덱스
        public const int DEVICE_TYPE_FRONT_CAMERA_INDEX = 3;
        public const int DEVICE_TYPE_REAR_RIGHT_RADAR_INDEX = 51;
        public const int DEVICE_TYPE_REAR_LEFT_RADAR_INDEX = 53;

        // 동기화 명령 인덱스
        public const int SYNC_COMMAND_FRONT_CAMERA_INDEX = 4;
        public const int SYNC_COMMAND_REAR_RIGHT_RADAR_INDEX = 52;
        public const int SYNC_COMMAND_REAR_LEFT_RADAR_INDEX = 54;

        // 각도값 인덱스 상수
        public const int FRONT_CAMERA_ANGLE1_INDEX = 110; // Roll
        public const int FRONT_CAMERA_ANGLE2_INDEX = 111; // Azimuth
        public const int FRONT_CAMERA_ANGLE3_INDEX = 112; // Elevation
        public const int REAR_RIGHT_RADAR_ANGLE_INDEX = 115;
        public const int REAR_LEFT_RADAR_ANGLE_INDEX = 116;

        // Try / Retry 여부 인덱스 상수
        public const int TRY_FRONT_CAMERA_INDEX = 89;
        public const int TRY_REAR_RIGHT_RADAR_INDEX = 83;
        public const int TRY_REAR_LEFT_RADAR_INDEX = 82;

        public static VEPBenchSynchroZone Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new VEPBenchSynchroZone();
                    }
                }

                return _instance;
            }
        }

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

        // 실제에서는 각도 값을 Read만 하고, Write 하지 않으므로 getter/setter가 필요 없음 (시뮬레이션을 위해 잠시 삭제 보류)
        public double FrontCameraAngle1
        {
            get => _values.Length > FRONT_CAMERA_ANGLE1_INDEX ? _values[FRONT_CAMERA_ANGLE1_INDEX] / 100.0 : 0.0;
            set => _values[FRONT_CAMERA_ANGLE1_INDEX] = (ushort)(value * 100);
        }

        public double FrontCameraAngle2
        {
            get => _values.Length > FRONT_CAMERA_ANGLE2_INDEX ? _values[FRONT_CAMERA_ANGLE2_INDEX] / 100.0 : 0.0;
            set => _values[FRONT_CAMERA_ANGLE2_INDEX] = (ushort)(value * 100);
        }

        public double FrontCameraAngle3
        {
            get => _values.Length > FRONT_CAMERA_ANGLE3_INDEX ? _values[FRONT_CAMERA_ANGLE3_INDEX] / 100.0 : 0.0;
            set => _values[FRONT_CAMERA_ANGLE3_INDEX] = (ushort)(value * 100);
        }

        public double RearRightRadarAngle
        {
            get => _values.Length > REAR_RIGHT_RADAR_ANGLE_INDEX ? _values[REAR_RIGHT_RADAR_ANGLE_INDEX] / 100.0 : 0.0;
            set => _values[REAR_RIGHT_RADAR_ANGLE_INDEX] = (ushort)(value * 100);
        }

        public double RearLeftRadarAngle
        {
            get => _values.Length > REAR_LEFT_RADAR_ANGLE_INDEX ? _values[REAR_LEFT_RADAR_ANGLE_INDEX] / 100.0 : 0.0;
            set => _values[REAR_LEFT_RADAR_ANGLE_INDEX] = (ushort)(value * 100);
        }

        public VEPBenchSynchroZone(int size = DEFAULT_SYNCHRO_SIZE)
        {
            _values = new ushort[size];
            ResetAllValues();
        }

        public static VEPBenchSynchroZone ReadFromVEP(Func<int, int, ushort[]> readFunc)
        {
            ushort[] part1 = readFunc(0, SYNCHRO_SIZE_PART1);
            ushort[] part2 = readFunc(SYNCHRO_SIZE_PART1, SYNCHRO_SIZE_PART2);

            if (part1.Length != SYNCHRO_SIZE_PART1 || part2.Length != SYNCHRO_SIZE_PART2)
                throw new ArgumentException("VEP에서 읽은 Synchro 데이터 크기가 올바르지 않습니다.");

            ushort[] all = new ushort[DEFAULT_SYNCHRO_SIZE];
            Array.Copy(part1, 0, all, 0, SYNCHRO_SIZE_PART1);
            Array.Copy(part2, 0, all, SYNCHRO_SIZE_PART1, SYNCHRO_SIZE_PART2);

            return FormUshortArray(all);
        }

        public static VEPBenchSynchroZone FormUshortArray(ushort[] arr)
        {
            if (arr == null)
                throw new ArgumentNullException(nameof(arr));

            var synchro = Instance;

            if (synchro._values.Length != arr.Length)
                synchro._values = new ushort[arr.Length];

            Array.Copy(arr, synchro._values, arr.Length);

            return synchro;
        }

        public ushort[] ToUshortArray()
        {
            ushort[] result = new ushort[_values.Length];
            Array.Copy(_values, result, _values.Length);

            return result;
        }

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

        public int GetDetectedDeviceType()
        {
            if (GetValue(DEVICE_TYPE_FRONT_CAMERA_INDEX) == 1)
                return DEVICE_TYPE_FRONT_CAMERA_INDEX;
            else if (GetValue(DEVICE_TYPE_REAR_RIGHT_RADAR_INDEX) == 1)
                return DEVICE_TYPE_REAR_RIGHT_RADAR_INDEX;
            else if (GetValue(DEVICE_TYPE_REAR_LEFT_RADAR_INDEX) == 1)
                return DEVICE_TYPE_REAR_LEFT_RADAR_INDEX;
            else
                return 0;
        }
    }
}