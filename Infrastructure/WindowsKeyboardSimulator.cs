using System.Runtime.InteropServices;
using Core;
using Core.Interfaces;

namespace Infrastructure;

public class WindowsKeyboardSimulator : IKeyboardSimulator
{
    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

    public void SendShortcut(IEnumerable<int> virtualKeys)
    {
        var keys = virtualKeys.ToList();
        var inputs = new List<INPUT>();

        // 1. Key Down dla wszystkich
        foreach (var vk in keys)
            inputs.Add(CreateKeyInput(vk, isKeyDown: true));

        // 2. Key Up dla wszystkich w odwrotnej kolejności
        for (int i = keys.Count - 1; i >= 0; i--)
            inputs.Add(CreateKeyInput(keys[i], isKeyDown: false));

        uint result = SendInput((uint)inputs.Count, inputs.ToArray(), Marshal.SizeOf(typeof(INPUT)));
        
        if (result == 0)
        {
            int error = Marshal.GetLastWin32Error();
            // Tutaj możesz dodać logowanie błędu: Log.Error("SendInput failed with error {Error}", error);
        }
    }

    private INPUT CreateKeyInput(int vk, bool isKeyDown)
    {
        uint dwFlags = isKeyDown ? 0u : 0x0002u; // 0x0002 = KEYEVENTF_KEYUP

        // Niektóre klawisze wymagają flagi EXTENDEDKEY (np. strzałki, prawy Alt, Ins, Del)
        if (IsExtendedKey(vk))
        {
            dwFlags |= 0x0001; // 0x0001 = KEYEVENTF_EXTENDEDKEY
        }

        return new INPUT
        {
            Type = 1, // INPUT_KEYBOARD
            U = new InputUnion
            {
                ki = new KEYBDINPUT
                {
                    wVk = (ushort)vk,
                    wScan = 0,
                    dwFlags = dwFlags,
                    time = 0,
                    dwExtraInfo = IntPtr.Zero
                }
            }
        };
    }

    private bool IsExtendedKey(int vk)
    {
        // Lista klawiszy rozszerzonych (strzałki, nawigacyjne, prawe modyfikatory)
        return vk is >= 33 and <= 46 or >= 91 and <= 93 or 163 or 165;
    }

    // --- STRUKTURY Z POPRAWIONYM WYRÓWNANIEM ---

    [StructLayout(LayoutKind.Sequential)]
    struct INPUT
    {
        public uint Type;
        public InputUnion U;
    }

    [StructLayout(LayoutKind.Explicit)]
    struct InputUnion
    {
        // FieldOffset(0) jest kluczowe dla Unii
        [FieldOffset(0)] public MOUSEINPUT mi;
        [FieldOffset(0)] public KEYBDINPUT ki;
        [FieldOffset(0)] public HARDWAREINPUT hi;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    // Puste struktury pomocnicze, aby Unia miała poprawny rozmiar
    [StructLayout(LayoutKind.Sequential)] struct MOUSEINPUT { public int dx; public int dy; public uint mouseData; public uint dwFlags; public uint time; public IntPtr dwExtraInfo; }
    [StructLayout(LayoutKind.Sequential)] struct HARDWAREINPUT { public uint uMsg; public ushort wParamL; public ushort wParamH; }
}