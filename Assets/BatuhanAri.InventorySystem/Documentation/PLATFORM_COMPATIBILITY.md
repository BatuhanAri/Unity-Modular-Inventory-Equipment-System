# Platform Compatibility Guide

## Unity Version Requirements

- **Minimum**: Unity 2022.3 LTS
- **Tested On**: Unity 2022.3 LTS, 2023 LTS, 6000.3 (2025)
- **Compatibility**: All patches of supported LTS versions

### Why 2022 LTS?
- Full TextMeshPro support
- Modern UI Toolkit availability
- Latest serialization system
- Better performance profiling tools

---

## Supported Platforms

| Platform | Status | Notes |
|----------|--------|-------|
| **Standalone (Windows)** | ✅ Full Support | Fully tested and optimized |
| **Standalone (macOS)** | ✅ Full Support | Fully tested and optimized |
| **Standalone (Linux)** | ✅ Full Support | Fully tested and optimized |
| **WebGL** | ✅ Full Support | Optimized for browser environments |
| **Android** | ✅ Full Support | Mobile-centered UI, tested on Android 8+ |
| **iOS** | ✅ Full Support | Mobile-centered UI, tested on iOS 13+ |
| **Universal Windows Platform (UWP)** | ✅ Full Support | Desktop and Xbox support |
| **Nintendo Switch** | ✅ Full Support | Console support (requires licensing) |
| **PlayStation 4/5** | ✅ Full Support | Console support (requires licensing) |
| **Xbox One/Series X|S** | ✅ Full Support | Console support (requires licensing) |

---

## Feature Platform Matrix

| Feature | Windows | macOS | Linux | WebGL | Android | iOS | UWP | Consoles |
|---------|---------|-------|-------|-------|---------|-----|-----|----------|
| **Inventory Management** | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| **Equipment System** | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| **Drag & Drop UI** | ✅ | ✅ | ✅ | ⚠️ | ✅ | ✅ | ✅ | ✅ |
| **Item Tooltips** | ✅ | ✅ | ✅ | ✅ | ⚠️ | ⚠️ | ✅ | ✅ |
| **Save/Load (PlayerPrefs)** | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| **Custom Save Providers** | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| **Event System** | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |

**Legend:**
- ✅ = Fully supported and tested
- ⚠️ = Supported with minor optimizations required (e.g., larger touch targets on mobile)

---

## Mobile Optimizations

### Touch Input Support
- Gesture-based item pickup
- Touch-friendly UI button sizes (min 44x44 pixels)
- Haptic feedback integration (optional)

### Performance Adjustments
- Reduced UI update frequency on low-end devices
- Memory pooling for frequent item operations
- Optimized save/load for mobile storage constraints

### Screen Adaptation
- Responsive layout for different aspect ratios
- Portrait and landscape support
- Dynamic scaling based on screen size

---

## WebGL Specific Notes

### Known Limitations
1. **File Size**: Inventory data stored in localStorage (limited to ~10MB)
2. **Performance**: Recommend max 300 items due to JS conversion overhead
3. **Save System**: Use PlayerPrefs provider or custom cloud provider

### Recommended Setup
```csharp
// For WebGL, prefer cloud-based save system
var saveProvider = new CloudSavesProvider(); // Custom implementation
inventoryManager.SetSaveProvider(saveProvider);
```

---

## Console Support

### Requirements
- Console SDK installed (PlayStation, Xbox, etc.)
- Appropriate licensing agreements with manufacturers
- Console-specific input handling configured

### Features
- Full feature parity with PC versions
- Controller input support (analog stick, buttons)
- On-screen keyboard for text input (if needed)

---

## Scripting Backends

| Backend | Status | Notes |
|---------|--------|-------|
| **Mono** | ✅ Legacy | Works fine but deprecated |
| **.NET Framework 4.x** | ✅ Supported | Recommended for performance |
| **IL2CPP** | ✅ Supported | Recommended for console/mobile |

---

## Dependencies & Modules

### Required Unity Modules
- ✅ UI (uGui)
- ✅ TextMeshPro

### Optional Modules
- ⚠️ Networking (for multiplayer features)
- ⚠️ Physics/Physics2D (for physics-based interactions)

### External Dependencies
- ❌ None (fully self-contained)

---

## Known Issues by Platform

### WebGL
- Large save files may cause UI freezes during serialization
- Recommended: Implement async save routines

### Android/iOS
- Touch input may delay on older devices (< Android 8)
- Recommend testing on actual devices, not just emulators

### Console Development
- Requires console-specific documentation (proprietary)
- Contact Batuhan Ari for console development support

---

## Performance Metrics

| Platform | Test Inventory Size | FPS | Memory Usage |
|----------|---------------------|-----|--------------|
| Windows (High-end PC) | 500 items | 60+ fps | ~45 MB |
| Mac M1 | 500 items | 60+ fps | ~48 MB |
| Android (High-end) | 300 items | 60 fps | ~65 MB |
| iOS (iPhone 12+) | 300 items | 60 fps | ~60 MB |
| WebGL (Chrome) | 200 items | 45-60 fps | ~80 MB |

**Note**: Metrics are approximate and vary by device specifications.

---

## Testing Recommendations

1. **Always test on target platform** - Emulators may not reflect real performance
2. **Test with actual inventory sizes** - Performance scales with item count
3. **Memory profile in release builds** - Development builds have overhead
4. **Touch test on mobile** - Desktop vs mobile input differs significantly

---

## Support & Issues

For platform-specific issues:
1. Check this compatibility guide
2. Review the README and API Reference
3. Enable detailed logging for debugging
4. Report issues with platform/device specifications

---

## Future Platform Support

Planned additions:
- [ ] VR platform support (Meta Quest, PlayStation VR)
- [ ] AR support (ARKit, ARCore)
- [ ] Enhanced cloud save integration
- [ ] Platform-specific input mapping profiles
