using System;
using System.Collections.Generic;

namespace Ki_ADAS
{
    public enum Language
    {
        English,
        Korean,
        Portuguese
    }

    public static class LanguageResource
    {
        private static Language _currentLanguage = Language.English;

        // 언어별 메시지 딕셔너리
        private static readonly Dictionary<string, Dictionary<Language, string>> _resources = new Dictionary<string, Dictionary<Language, string>>
        {
            // 폼 및 컨트롤 타이틀
            {"MainTitle", new Dictionary<Language, string>
                {
                    {Language.English, "Main"},
                    {Language.Korean, "메인"},
                    {Language.Portuguese, "Principal"}
                }
            },
            {"ConfigTitle", new Dictionary<Language, string>
                {
                    {Language.English, "Configuration"},
                    {Language.Korean, "환경설정"},
                    {Language.Portuguese, "Configuração"}
                }
            },
            {"CalibrationTitle", new Dictionary<Language, string>
                {
                    {Language.English, "Calibration"},
                    {Language.Korean, "보정"},
                    {Language.Portuguese, "Calibração"}
                }
            },
            {"ManualTitle", new Dictionary<Language, string>
                {
                    {Language.English, "Manual"},
                    {Language.Korean, "매뉴얼"},
                    {Language.Portuguese, "Manual"}
                }
            },
            {"ResultTitle", new Dictionary<Language, string>
                {
                    {Language.English, "Result"},
                    {Language.Korean, "결과"},
                    {Language.Portuguese, "Resultado"}
                }
            },
            {"VEPTitle", new Dictionary<Language, string>
                {
                    {Language.English, "VEP"},
                    {Language.Korean, "VEP"},
                    {Language.Portuguese, "VEP"}
                }
            },
            
            // 버튼
            {"MainButton", new Dictionary<Language, string>
                {
                    {Language.English, "Main"},
                    {Language.Korean, "메인"},
                    {Language.Portuguese, "Principal"}
                }
            },
            {"ConfigButton", new Dictionary<Language, string>
                {
                    {Language.English, "Config"},
                    {Language.Korean, "설정"},
                    {Language.Portuguese, "Config"}
                }
            },
            {"CalibrationButton", new Dictionary<Language, string>
                {
                    {Language.English, "Calibration"},
                    {Language.Korean, "보정"},
                    {Language.Portuguese, "Calibração"}
                }
            },
            {"ManualButton", new Dictionary<Language, string>
                {
                    {Language.English, "Manual"},
                    {Language.Korean, "매뉴얼"},
                    {Language.Portuguese, "Manual"}
                }
            },
            {"ResultButton", new Dictionary<Language, string>
                {
                    {Language.English, "Result"},
                    {Language.Korean, "결과"},
                    {Language.Portuguese, "Resultado"}
                }
            },
            {"VEPButton", new Dictionary<Language, string>
                {
                    {Language.English, "VEP"},
                    {Language.Korean, "VEP"},
                    {Language.Portuguese, "VEP"}
                }
            },
            {"SaveButton", new Dictionary<Language, string>
                {
                    {Language.English, "Save"},
                    {Language.Korean, "저장"},
                    {Language.Portuguese, "Salvar"}
                }
            },
            {"StartButton", new Dictionary<Language, string>
                {
                    {Language.English, "Start"},
                    {Language.Korean, "시작"},
                    {Language.Portuguese, "Iniciar"}
                }
            },
            {"StopButton", new Dictionary<Language, string>
                {
                    {Language.English, "Stop"},
                    {Language.Korean, "중지"},
                    {Language.Portuguese, "Parar"}
                }
            },
            {"AddButton", new Dictionary<Language, string>
                {
                    {Language.English, "ADD"},
                    {Language.Korean, "추가"},
                    {Language.Portuguese, "Adicionar"}
                }
            },
            {"TestModbusButton", new Dictionary<Language, string>
                {
                    {Language.English, "Test Modbus"},
                    {Language.Korean, "Modbus 테스트"},
                    {Language.Portuguese, "Testar Modbus"}
                }
            },
            
            // 레이블 및 그룹
            {"VepIpLabel", new Dictionary<Language, string>
                {
                    {Language.English, "VEP IP Address:"},
                    {Language.Korean, "VEP IP 주소:"},
                    {Language.Portuguese, "Endereço IP do VEP:"}
                }
            },
            {"VepPortLabel", new Dictionary<Language, string>
                {
                    {Language.English, "VEP Port:"},
                    {Language.Korean, "VEP 포트:"},
                    {Language.Portuguese, "Porta do VEP:"}
                }
            },
            {"PlcIpLabel", new Dictionary<Language, string>
                {
                    {Language.English, "PLC IP Address:"},
                    {Language.Korean, "PLC IP 주소:"},
                    {Language.Portuguese, "Endereço IP do PLC:"}
                }
            },
            {"PlcPortLabel", new Dictionary<Language, string>
                {
                    {Language.English, "PLC Port:"},
                    {Language.Korean, "PLC 포트:"},
                    {Language.Portuguese, "Porta do PLC:"}
                }
            },
            {"BarcodeLabel", new Dictionary<Language, string>
                {
                    {Language.English, "Barcode:"},
                    {Language.Korean, "바코드:"},
                    {Language.Portuguese, "código de barras"}
                }
            },
            {"WheelbaseLabel", new Dictionary<Language, string>
                {
                    {Language.English, "WheelBase:"},
                    {Language.Korean, "휠베이스:"},
                    {Language.Portuguese, "base de rodas"}
                }
            },
            {"LanguageLabel", new Dictionary<Language, string>
                {
                    {Language.English, "Language:"},
                    {Language.Korean, "언어:"},
                    {Language.Portuguese, "Idioma:"}
                }
            },
            {"NetworkGroup", new Dictionary<Language, string>
                {
                    {Language.English, "Network Settings"},
                    {Language.Korean, "네트워크 설정"},
                    {Language.Portuguese, "Configurações de Rede"}
                }
            },
            {"SystemGroup", new Dictionary<Language, string>
                {
                    {Language.English, "System Settings"},
                    {Language.Korean, "시스템 설정"},
                    {Language.Portuguese, "Configurações do Sistema"}
                }
            },
            {"StatusLabel", new Dictionary<Language, string>
                {
                    {Language.English, "Status:"},
                    {Language.Korean, "상태:"},
                    {Language.Portuguese, "Estado:"}
                }
            },
            {"ProgressLabel", new Dictionary<Language, string>
                {
                    {Language.English, "Progress:"},
                    {Language.Korean, "진행률:"},
                    {Language.Portuguese, "Progresso:"}
                }
            },
            {"LogLabel", new Dictionary<Language, string>
                {
                    {Language.English, "Log:"},
                    {Language.Korean, "로그:"},
                    {Language.Portuguese, "Registro:"}
                }
            },
            
            // ListView
            {"ModelName", new Dictionary<Language, string>
                {
                    {Language.English, "ModelName"},
                    {Language.Korean, "ModelName"},
                    {Language.Portuguese, "Nome do modelo"}
                }
            },
            
            // 프로세스 단계 설명
            {"Step_213_1", new Dictionary<Language, string>
                {
                    {Language.English, "Camera Calibration Request"},
                    {Language.Korean, "카메라 캘리브레이션 요청"},
                    {Language.Portuguese, "Solicitação de Calibração de Câmera"}
                }
            },
            {"Step_214", new Dictionary<Language, string>
                {
                    {Language.English, "Check Front Camera Target Position"},
                    {Language.Korean, "전면 카메라 타겟 위치 확인"},
                    {Language.Portuguese, "Verificar Posição do Alvo da Câmera Frontal"}
                }
            },
            {"Step_213_2", new Dictionary<Language, string>
                {
                    {Language.English, "Move Target to Home Position"},
                    {Language.Korean, "타켓을 홈 위치로 이동"},
                    {Language.Portuguese, "Mover Alvo para Posição Inicial"}
                }
            },
            {"Step_213_3", new Dictionary<Language, string>
                {
                    {Language.English, "Move Target to Home Position Step 2"},
                    {Language.Korean, "타켓을 홈 위치로 이동 2단계"},
                    {Language.Portuguese, "Mover Alvo para Posição Inicial Passo 2"}
                }
            },
            {"Step_320", new Dictionary<Language, string>
                {
                    {Language.English, "Send Angle 1 Measurement"},
                    {Language.Korean, "각도 1 측정값 전송"},
                    {Language.Portuguese, "Enviar Medição do Ângulo 1"}
                }
            },
            {"Step_321", new Dictionary<Language, string>
                {
                    {Language.English, "Send Angle 2 Measurement"},
                    {Language.Korean, "각도 2 측정값 전송"},
                    {Language.Portuguese, "Enviar Medição do Ângulo 2"}
                }
            },
            {"Step_322", new Dictionary<Language, string>
                {
                    {Language.English, "Send Angle 3 Measurement"},
                    {Language.Korean, "각도 3 측정값 전송"},
                    {Language.Portuguese, "Enviar Medição do Ângulo 3"}
                }
            },
            {"Step_299_1", new Dictionary<Language, string>
                {
                    {Language.English, "Synchro 89 First Attempt"},
                    {Language.Korean, "Synchro 89 첫번째 시도"},
                    {Language.Portuguese, "Primeira Tentativa de Synchro 89"}
                }
            },
            {"Step_299_2", new Dictionary<Language, string>
                {
                    {Language.English, "Synchro 89 Second Attempt"},
                    {Language.Korean, "Synchro 89 두번째 시도"},
                    {Language.Portuguese, "Segunda Tentativa de Synchro 89"}
                }
            },
            
            // 메시지
            {"CameraAngleValidationPass", new Dictionary<Language, string>
                {
                    {Language.English, "Front camera angle validation passed"},
                    {Language.Korean, "전면 카메라 각도 검증 통과"},
                    {Language.Portuguese, "Validação do ângulo da câmera frontal aprovada"}
                }
            },
            {"VEPBenchNotInitialized", new Dictionary<Language, string>
                {
                    {Language.English, "VEPBenchClient is not initialized"},
                    {Language.Korean, "VEPBenchClient가 초기화되지 않았습니다"},
                    {Language.Portuguese, "VEPBenchClient não foi inicializado"}
                }
            },
            {"AngleReadComplete", new Dictionary<Language, string>
                {
                    {Language.English, "Angle Read Complete"},
                    {Language.Korean, "각도 읽기 완료"},
                    {Language.Portuguese, "Angle Read Complete"}
                }
            },
            {"SynchroReadComplete", new Dictionary<Language, string>
                {
                    {Language.English, "Synchro {0} = {1} reading completed"},
                    {Language.Korean, "Synchro {0} = {1} 읽기 완료"},
                    {Language.Portuguese, "Sincro {0} = {1} leitura completa"}
                }
            },
            {"SynchroReadFail", new Dictionary<Language, string>
                {
                    {Language.English, "Failed to read Synchro value: {0}"},
                    {Language.Korean, "Synchro 값 읽기 실패: {0}"},
                    {Language.Portuguese, "Sincro {0} = {1} leitura falhada"}
                }
            },
            {"SensorTypeDetectionFail", new Dictionary<Language, string>
                {
                    {Language.English, "SensorType Detection Fail"},
                    {Language.Korean, "센서타입 발견 실패"},
                    {Language.Portuguese, "Falha de detecção de tipo sensor"}
                }
            },
            {"ProcessStart", new Dictionary<Language, string>
                {
                    {Language.English, "Process started"},
                    {Language.Korean, "프로세스 시작"},
                    {Language.Portuguese, "Processo iniciado"}
                }
            },
            {"ProcessStartFail", new Dictionary<Language, string>
                {
                    {Language.English, "Failed to start process: {0}"},
                    {Language.Korean, "프로세스 시작 실패: {0}"},
                    {Language.Portuguese, "Falha ao iniciar processo: {0}"}
                }
            },
            {"ProcessStop", new Dictionary<Language, string>
                {
                    {Language.English, "Process stopped"},
                    {Language.Korean, "프로세스 중지"},
                    {Language.Portuguese, "Processo parado"}
                }
            },
            {"ProcessComplete", new Dictionary<Language, string>
                {
                    {Language.English, "Process completed"},
                    {Language.Korean, "프로세스 완료"},
                    {Language.Portuguese, "Processo concluído"}
                }
            },
            {"ProcessError", new Dictionary<Language, string>
                {
                    {Language.English, "Error during process execution: {0}"},
                    {Language.Korean, "프로세스 실행 중 오류: {0}"},
                    {Language.Portuguese, "Erro durante execução do processo: {0}"}
                }
            },
            {"StepProgress", new Dictionary<Language, string>
                {
                    {Language.English, "Step {0} / {1}: {2}"},
                    {Language.Korean, "단계 {0} / {1}: {2}"},
                    {Language.Portuguese, "Passo {0} / {1}: {2}"}
                }
            },
            {"StepFail", new Dictionary<Language, string>
                {
                    {Language.English, "Step {0} failed: {1}"},
                    {Language.Korean, "단계 {0} 실패: {1}"},
                    {Language.Portuguese, "Passo {0} falhou: {1}"}
                }
            },
            {"StepError", new Dictionary<Language, string>
                {
                    {Language.English, "Error in step {0}: {1}"},
                    {Language.Korean, "단계 {0} 오류 발생: {1}"},
                    {Language.Portuguese, "Erro no passo {0}: {1}"}
                }
            },
            {"Roll", new Dictionary<Language, string>
                {
                    {Language.English, "Roll"},
                    {Language.Korean, "Roll"},
                    {Language.Portuguese, "Roll"}
                }
            },
            {"Azimuth", new Dictionary<Language, string>
                {
                    {Language.English, "Azimuth"},
                    {Language.Korean, "Azimuth"},
                    {Language.Portuguese, "Azimute"}
                }
            },
            {"Elevation", new Dictionary<Language, string>
                {
                    {Language.English, "Elevation"},
                    {Language.Korean, "Elevation"},
                    {Language.Portuguese, "Elevação"}
                }
            },
            {"SettingsSaved", new Dictionary<Language, string>
                {
                    {Language.English, "Settings have been saved."},
                    {Language.Korean, "설정이 저장되었습니다."},
                    {Language.Portuguese, "Configurações foram salvas."}
                }
            },
            {"SaveComplete", new Dictionary<Language, string>
                {
                    {Language.English, "Save Complete"},
                    {Language.Korean, "저장 완료"},
                    {Language.Portuguese, "Salvamento Concluído"}
                }
            },
            {"ModelSaveSuccess", new Dictionary<Language, string>
                {
                    {Language.English, "Save Model Settings Completed"},
                    {Language.Korean, "모델 설정 저장 완료"},
                    {Language.Portuguese, "Salvar as configurações do modelo completo"}
                }
            },
            {"ModelSaveFailed", new Dictionary<Language, string>
                {
                    {Language.English, "Save Model Settings Failed"},
                    {Language.Korean, "모델 설정 저장 실패"},
                    {Language.Portuguese, "Salvar as configurações do modelo falhou"}
                }
            },
            {"LanguageChangeSuccess", new Dictionary<Language, string>
                {
                    {Language.English, "Saving Language Settings Complete"},
                    {Language.Korean, "언어 설정 저장 완료"},
                    {Language.Portuguese, "Salvar as configurações de linguagem completa"}
                }
            },
            {"ConfigSaveSuccess", new Dictionary<Language, string>
                {
                    {Language.English, "Save Settings complete"},
                    {Language.Korean, "설정 저장 완료"},
                    {Language.Portuguese, "Ajustes de salvação completados"}
                }
            },
            {"ModelNameRequired", new Dictionary<Language, string>
                {
                    {Language.English, "Model Name Required"},
                    {Language.Korean, "모델 이름 필요"},
                    {Language.Portuguese, "Nome do modelo necessário"}
                }
            },
            {"ModelNameAlreadyExists", new Dictionary<Language, string>
                {
                    {Language.English, "Model Name Already Exists"},
                    {Language.Korean, "모델 이름 이미 존재"},
                    {Language.Portuguese, "O nome do modelo já existe"}
                }
            },
            {"PleaseSelectModel", new Dictionary<Language, string>
                {
                    {Language.English, "Please Select Model"},
                    {Language.Korean, "삭제할 모델을 선택"},
                    {Language.Portuguese, "Por favor, selecione o modelo"}
                }
            },
            {"ConfirmDeleteModel", new Dictionary<Language, string>
                {
                    {Language.English, "Confirm Delete Model"},
                    {Language.Korean, "모델 삭제 확인"},
                    {Language.Portuguese, "Confirme Eliminar modelo"}
                }
            },
            {"ModelAddSuccess", new Dictionary<Language, string>
                {
                    {Language.English, "Model Add Success"},
                    {Language.Korean, "모델 추가 성공"},
                    {Language.Portuguese, "Modelo adicionar sucesso"}
                }
            },
            {"ModelAddFailed", new Dictionary<Language, string>
                {
                    {Language.English, "Model Add Failed"},
                    {Language.Korean, "모델 추가 실패"},
                    {Language.Portuguese, "Adicionar modelo falhou"}
                }
            },
            {"ModelUpdateSuccess", new Dictionary<Language, string>
                {
                    {Language.English, "Model Update Success"},
                    {Language.Korean, "모델 편집 성공"},
                    {Language.Portuguese, "Atualização do modelo sucesso"}
                }
            },
            {"ModelUpdateFailed", new Dictionary<Language, string>
                {
                    {Language.English, "Model Update Failed"},
                    {Language.Korean, "모델 편집 실패"},
                    {Language.Portuguese, "Atualização do modelo falhou"}
                }
            },
            {"ModelDeleteSuccess", new Dictionary<Language, string>
                {
                    {Language.English, "Model Delete Success"},
                    {Language.Korean, "모델 삭제 성공"},
                    {Language.Portuguese, "Modelo Eliminar o sucesso"}
                }
            },
            {"ModelDeleteFailed", new Dictionary<Language, string>
                {
                    {Language.English, "Model Delete Failed"},
                    {Language.Korean, "모델 삭제 실패"},
                    {Language.Portuguese, "Modelo Eliminar falhou"}
                }
            },
            {"DatabaseError", new Dictionary<Language, string>
                {
                    {Language.English, "Database Error"},
                    {Language.Korean, "데이터베이스 에러"},
                    {Language.Portuguese, "Erro na base de dados"}
                }
            },
            {"NoModelDetailsFound", new Dictionary<Language, string>
                {
                    {Language.English, "No Model Details Found"},
                    {Language.Korean, "데이터 없음"},
                    {Language.Portuguese, "Nenhum detalhe do modelo encontrado"}
                }
            },

            // 캡션
            {"Information", new Dictionary<Language, string>
                {
                    {Language.English, "Information"},
                    {Language.Korean, "정보"},
                    {Language.Portuguese, "Informação"}
                }
            },
            {"Warning", new Dictionary<Language, string>
                {
                    {Language.English, "Warning"},
                    {Language.Korean, "경고"},
                    {Language.Portuguese, "Atenção"}
                }
            },
            {"Error", new Dictionary<Language, string>
                {
                    {Language.English, "Error"},
                    {Language.Korean, "오류"},
                    {Language.Portuguese, "Erro"}
                }
            },
        };

        // 현재 언어 설정
        public static Language CurrentLanguage
        {
            get { return _currentLanguage; }
            set { _currentLanguage = value; }
        }

        // 키에 해당하는 메시지 가져오기
        public static string GetMessage(string key)
        {
            if (_resources.ContainsKey(key) && _resources[key].ContainsKey(_currentLanguage))
            {
                return _resources[key][_currentLanguage];
            }

            // 리소스를 찾을 수 없는 경우 영어 리소스로 폴백
            if (_resources.ContainsKey(key) && _resources[key].ContainsKey(Language.English))
            {
                return _resources[key][Language.English];
            }

            return key; // 리소스를 찾을 수 없는 경우 키를 반환
        }

        // 포맷 문자열이 있는 메시지 가져오기
        public static string GetFormattedMessage(string key, params object[] args)
        {
            string message = GetMessage(key);
            return string.Format(message, args);
        }

        public static string GetMessageOrDefault(string key, string defaultText = null)
        {
            if (_resources.ContainsKey(key) && _resources[key].ContainsKey(CurrentLanguage))
            {
                return _resources[key][CurrentLanguage];
            }

            if (_resources.ContainsKey(key) && _resources[key].ContainsKey(Language.English))
            {
                return _resources[key][Language.English];
            }

            return defaultText ?? key;
        }
    }
}