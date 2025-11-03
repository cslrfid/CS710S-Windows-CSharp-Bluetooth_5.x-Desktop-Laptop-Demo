# CS710S Windows C# Bluetooth 5.x Desktop/Laptop Demo

A Windows Forms (.NET Framework 4.8) application that discovers, connects to, and inventories UHF RFID tags with the CSL CS710S handheld reader over Bluetooth 5.x. The app is backward compatible with CS108 devices.

## Project structure

- `CS710SDesktopDemo` (WinForms app)
  - UI for scanning/connecting to readers, starting/stopping tag inventory, and viewing tags/battery.
  - Main form: `FormMain`.
- `Library/CSLibrary2024` (class library, .NET Framework 4.8)
  - CSL Unified API implementation for Bluetooth, RFID, barcode, and device services.
  - Key classes used by the app: `HighLevelInterface`, `DeviceFinder`, `ClassRFID`, `Notification`.

## Design overview

- Discovery
  - Uses `DeviceFinder.SearchDevice()` to scan for nearby readers. Results are raised through `DeviceFinder.OnSearchCompleted` and added to the readers list.
  - Optional MAC address filtering can be toggled.
- Connection lifecycle
  - `HighLevelInterface.ConnectAsync()` establishes a Bluetooth connection to the selected device.
  - The app listens for `rfid.OnStateChanged`. When `RFState.INITIALIZATION_COMPLETE` is received, the UI enables inventory controls and loads the reader’s active RF link profiles into a dropdown.
  - `Disconnect` cleanly tears down the connection via `HighLevelInterface.DisconnectAsync()` and disables inventory controls. The app enforces disconnect before exit.
- Inventory flow
  - On `Start Inventory`, the app:
    - Enables antenna port `0` (`AntennaPortSetState`), sets RF power with `SetPowerLevel(300)`, and applies the selected link profile via `SetCurrentLinkProfile`.
    - Starts tag ranging with `StartOperation(Operation.TAG_RANGING)`.
    - Subscribes to `rfid.OnAsyncCallback` and updates the tag grid with EPC and latest RSSI, de-duplicating on EPC.
  - On `Stop Inventory`, the app stops the RFID operation and unsubscribes from callbacks.
- Battery monitoring
  - Subscribes to `notification.OnVoltageEvent` and displays battery `% / volts`.
  - A local `ClassBattery` converts voltage to percentage with mode-dependent curves for `INVENTORY` vs `IDLE`.

## UI walkthrough

- `Scan`: searches for nearby readers (status shows progress). Results appear in the list (device name, MAC, model).
- `Connect`: connects to the selected reader and enables inventory controls when ready.
- `RF Modes / Link Profile`: list of supported link profiles read from the device. Choose one before starting inventory.
- `Start Inventory`: begins continuous tag ranging, showing EPC and current RSSI (dBuV). Re-seen EPCs update RSSI instead of adding duplicates.
- `Stop Inventory`: stops tag ranging.
- `Clear Tag records`: clears the EPC table.
- `Disconnect`: cleanly disconnects the reader.
- `Exit`: the app will prompt you to disconnect first if still connected.
- Battery indicator: shows remaining percentage and voltage in real time when connected.

## Build and run

Prerequisites

- Windows with Bluetooth 5.x support
- Visual Studio 2019 or 2022 with .NET Framework 4.8 developer pack

Steps

- Open the solution in Visual Studio.
- Set startup project to `CS710SDesktopDemo`.
- Restore/build the solution.
- Deploy/run on a PC with Bluetooth enabled.

## Using the application

1. Turn on the CS710S and ensure it is in Bluetooth advertising mode.
2. Launch the app and click `Scan`. Wait for discovery to complete.
3. Select your reader in the list and click `Connect`.
4. After connected, choose an RF link profile from the dropdown.
5. Click `Start Inventory` to begin reading tags. EPCs and RSSI will appear in the grid.
6. Click `Stop Inventory` to stop. Use `Clear Tag records` to clear the grid as needed.
7. Click `Disconnect` when finished. Exit the app afterward.

## Extensibility points

- Power level: adjust in `FormMain` before starting inventory via `rfid.SetPowerLevel`.
- Antenna selection: uses port `0` by default; change via `rfid.AntennaPortSetState`.
- Link profiles: populated from `rfid.GetActiveLinkProfileName`. The selected profile is applied through `rfid.SetCurrentLinkProfile`.
- Tag handling: customize the `OnAsyncCallback` handler to add filtering, persistence, or business logic.

## Troubleshooting

- No devices found
  - Ensure Windows Bluetooth is enabled and the reader is powered and advertising.
  - Toggle `Mac Address Filtering` off and rescan if your device is filtered out.
- Connect fails or never completes
  - Move closer to the reader and retry.
  - Power-cycle the reader and/or disable/enable Bluetooth on the PC.
- Inventory starts but no tags are shown
  - Verify the chosen RF profile is valid for your region.
  - Increase power level if needed and ensure tags are within range.
- Unable to exit
  - Click `Disconnect` first. The app requires a clean disconnect before closing.

## Notes

- The app window title includes the version and indicates backward compatibility with CS108.
- The library provides additional features (e.g., barcode, engineering APIs) not exercised in this simple demo but available for integration.
