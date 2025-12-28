using System.Runtime.InteropServices;
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

        // 1. Key Down dla wszystkich (np. Ctrl dół, potem C dół)
        foreach (var vk in keys)
            inputs.Add(CreateKeyInput(vk, dwFlags: 0));

        // 2. Key Up dla wszystkich w odwrotnej kolejności (np. C góra, potem Ctrl góra)
        for (int i = keys.Count - 1; i >= 0; i--)
            inputs.Add(CreateKeyInput(keys[i], dwFlags: 0x0002)); // 0x0002 = KEYEVENTF_KEYUP

        SendInput((uint)inputs.Count, inputs.ToArray(), Marshal.SizeOf(typeof(INPUT)));
    }

    private INPUT CreateKeyInput(int vk, uint dwFlags) => new INPUT
    {
        Type = 1, // INPUT_KEYBOARD
        U = new InputUnion { ki = new KEYBDINPUT { wVk = (ushort)vk, dwFlags = dwFlags } }
    };

    [StructLayout(LayoutKind.Sequential)]
    struct INPUT { public uint Type; public InputUnion U; }
    [StructLayout(LayoutKind.Explicit)]
    struct InputUnion { [FieldOffset(0)] public KEYBDINPUT ki; }
    [StructLayout(LayoutKind.Sequential)]
    struct KEYBDINPUT { public ushort wVk; public ushort wScan; public uint dwFlags; public uint time; public IntPtr dwExtraInfo; }
}