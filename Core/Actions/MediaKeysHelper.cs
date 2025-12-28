using System.Runtime.InteropServices;

namespace Core.Input;

public static class MediaKeysHelper
{
    // Importujemy funkcję z systemu Windows odpowiedzialną za symulowanie klawiszy
    [DllImport("user32.dll")]
    private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

    // Kody klawiszy multimedialnych
    private const byte VK_MEDIA_NEXT_TRACK = 0xB0;
    private const byte VK_MEDIA_PREV_TRACK = 0xB1;
    private const byte VK_MEDIA_PLAY_PAUSE = 0xB3;
    private const byte VK_MEDIA_STOP = 0xB2;

    private const uint KEYEVENTF_KEYUP = 0x0002;

    public static void PlayPause() => SendKey(VK_MEDIA_PLAY_PAUSE);
    public static void Next() => SendKey(VK_MEDIA_NEXT_TRACK);
    public static void Previous() => SendKey(VK_MEDIA_PREV_TRACK);

    private static void SendKey(byte keyCode)
    {
        // Wciśnij klawisz
        keybd_event(keyCode, 0, 0, 0);
        // Puść klawisz
        keybd_event(keyCode, 0, KEYEVENTF_KEYUP, 0);
    }
}