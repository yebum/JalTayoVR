# 잘타요 VR (JalTayoVR)

버스 이용 과정을 VR로 체험하는 Unity 프로젝트입니다. 사용자는 가상 도시에서 버스에 탑승하고, 교통카드를 태그하고, 하차벨을 누른 뒤 목적지에서 내리는 과정을 연습할 수 있습니다.

## 주요 기능

- XR 컨트롤러 기반 이동, 회전, 오브젝트 상호작용
- 컨트롤러 입력에 반응하는 가상 손 애니메이션
- 버스 정류장과 경유지를 따라 이동하는 버스 경로 시스템
- 승차·하차 단말기의 교통카드 태그
- 하차벨 입력에 따른 선택 정류장 정차 또는 통과
- 정차 시 버스 문 자동 개폐
- 버스 이동 중 플레이어 위치 동기화
- 튜토리얼, 미션 안내, 상황 메시지 및 완료 UI
- 스냅 회전과 연속 회전 방식 전환

## 체험 흐름

1. 시작 화면에서 체험을 시작합니다.
2. 튜토리얼을 통해 이동과 상호작용 방법을 확인합니다.
3. 정류장에서 버스를 기다린 뒤 교통카드를 태그합니다.
4. 버스에 탑승하면 플레이어가 버스의 이동을 따라갑니다.
5. 내릴 정류장 전에 하차벨을 누릅니다.
6. 버스가 다음 선택 정류장에 정차하고 문이 열리면 하차합니다.
7. 목적지에 도착하면 완료 UI가 표시됩니다.

## 개발 환경

| 항목 | 버전 / 구성 |
| --- | --- |
| Unity | 2022.3.62f3 (LTS) |
| 렌더 파이프라인 | Universal Render Pipeline 14.0.12 |
| XR Interaction Toolkit | 2.6.5 |
| XR Plug-in Management | 4.5.4 |
| OpenXR Plugin | 1.14.3 |
| UI | Unity UI, TextMesh Pro 3.0.7 |

## 실행 방법

1. 저장소를 복제합니다.

   ```bash
   git clone https://github.com/yebum/JalTayoVR.git
   ```

2. Unity Hub에서 저장소 폴더를 프로젝트로 추가합니다.
3. Unity Editor `2022.3.62f3`으로 프로젝트를 엽니다.
4. Package Manager가 의존성 설치를 마칠 때까지 기다립니다.
5. `File > Build Settings`에서 등록된 씬과 순서를 확인합니다.
6. OpenXR를 사용할 대상 플랫폼과 XR 기기 설정을 확인한 뒤 에디터에서 Play하거나 빌드합니다.

현재 Build Settings에 활성화된 씬은 다음과 같습니다.

1. `Assets/Scenes/Yebum.unity`
2. `Assets/Scenes/Tutorial.unity`
3. `Assets/Scenes/TutorialHyejin.unity`
4. `Assets/Scenes/Play02.unity`

> XR 기기 없이 일부 상호작용을 확인할 때는 XR Interaction Toolkit의 Device Simulator 샘플을 활용할 수 있습니다. 카드 단말기는 Inspector의 테스트 옵션으로 태그 입력을 확인할 수 있습니다.

## 프로젝트 구조

```text
JalTayoVR/
├─ Assets/
│  ├─ Scenes/          # 시작, 튜토리얼, 플레이 씬
│  ├─ Scripts/         # 버스 운행, 카드 태그, UI 등 게임 로직
│  ├─ XRI/             # XR Interaction Toolkit 설정
│  ├─ XR/              # XR Plug-in 설정
│  ├─ Settings/        # URP 렌더링 설정
│  ├─ Images/          # 교통카드 등 이미지 리소스
│  ├─ Sprites/         # 로고와 시작 화면 리소스
│  └─ Simple city plain/ # 도시 환경 모델과 프리팹
├─ Packages/           # Unity 패키지 의존성
└─ ProjectSettings/    # Unity 프로젝트 및 빌드 설정
```

## 주요 스크립트

| 스크립트 | 역할 |
| --- | --- |
| `BusRoute.cs` | 경유지 이동, 정류장 유형별 정차, 승차 대기와 운행 상태 관리 |
| `BellController.cs` | 하차벨 상호작용, 색상·소리 피드백, 다음 정류장 정차 요청 |
| `CardReader.cs` | 승차·하차 교통카드 태그 처리 및 테스트 입력 |
| `DoorController.cs` | 정차 시 버스 출입문 개폐 애니메이션 |
| `testscript.cs` | 탑승 중 플레이어를 버스 이동량에 맞춰 동기화 |
| `TutorialController.cs` | 튜토리얼 페이지 전환과 미션 UI 활성화 |
| `BusMessageController.cs` | 상황 안내 메시지의 페이드 인·아웃 표시 |
| `VRFollowUI.cs` | 안내 UI가 HMD 카메라를 부드럽게 따라가도록 제어 |
| `AnimateHandOnInput.cs` | 트리거·그립 입력을 가상 손 애니메이션에 반영 |
| `SceneMover.cs` | UI 이벤트를 통한 씬 전환 |
| `SetTurnType.cs` | 연속 회전과 스냅 회전 설정 전환 |

## 버스 정차 규칙

`BusRoute`의 각 경유지는 다음 세 유형 중 하나로 설정됩니다.

- `Boarding`: 승차 정류장. 벨 입력과 관계없이 정차하며 카드 태그를 기다립니다.
- `Optional`: 일반 정류장. 하차벨이 눌린 경우에만 정차합니다.
- `Final`: 최종 목적지. 벨 입력과 관계없이 정차합니다.

## 참고 사항

- `Library`, `Logs`, `obj`, 사용자별 IDE 설정 파일은 저장소에 포함하지 않습니다.
- 씬의 컴포넌트 참조와 태그(`Player`, `Bus`, `BusCard`, `Bell`)가 빠지면 관련 상호작용이 동작하지 않을 수 있습니다.
- 일부 기존 C# 주석은 문자 인코딩이 깨져 있으므로 수정 시 UTF-8로 정리하는 것을 권장합니다.

## 라이선스

현재 저장소에 별도의 라이선스 파일이 없습니다. 코드나 에셋을 외부에서 사용하려면 프로젝트 소유자와 각 에셋의 이용 조건을 먼저 확인하세요.
