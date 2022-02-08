﻿using System;
using System.Diagnostics;

using MicaForEveryone.Win32;

namespace MicaForEveryone.Models
{
    public class TargetWindow
    {
        //public static TargetWindow FromAutomationElement(IUIAutomationElement element)
        //{
        //    return new TargetWindow
        //    {
        //        WindowHandle = element.CurrentNativeWindowHandle,
        //        Title = element.CurrentName,
        //        ClassName = element.CurrentClassName,
        //        ProcessName = Process.GetProcessById(element.CurrentProcessId).ProcessName,
        //    };
        //}

        public static TargetWindow FromWindow(Window window)
        {
            return new TargetWindow
            {
                WindowHandle = window.Handle,
                Title = window.GetText(),
                ClassName = window.Class.Name,
                ProcessId = window.GetProcessId(),
                ProcessName = Process.GetProcessById((int)window.GetProcessId()).ProcessName,
            };
        }

        private TargetWindow() { }

        public IntPtr WindowHandle { get; private set; }
        public string Title { get; private set; }
        public string ClassName { get; private set; }
        public string ProcessName { get; private set; }
        public uint ProcessId { get; private set; }

        public void ApplyBackdropRule(BackdropType type)
        {
            if (DesktopWindowManager.IsBackdropTypeSupported)
            {
                if (type == BackdropType.Default)
                    return;

                DesktopWindowManager.SetBackdropType(WindowHandle, type);
            }
            else if (DesktopWindowManager.IsUndocumentedMicaSupported)
            {
                switch (type)
                {
                    case BackdropType.Default:
                        return;
                    case BackdropType.None:
                        DesktopWindowManager.SetMica(WindowHandle, false);
                        return;
                    case BackdropType.Mica:
                        DesktopWindowManager.SetMica(WindowHandle, true);
                        return;
                    case BackdropType.Acrylic:
                        throw new PlatformNotSupportedException("Acrylic backdrop requires at least Windows build 22523 or higher.");
                    case BackdropType.Tabbed:
                        throw new PlatformNotSupportedException("Tabbed backdrop requires at least Windows build 22523 or higher.");
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else if (type != BackdropType.Default)
            {
                throw new PlatformNotSupportedException("Customizing backdrop requires at least Windows build 22000 or higher.");
            }
        }

        public void ApplyTitlebarColorRule(TitlebarColorMode targetMode, TitlebarColorMode systemMode)
        {
            if (DesktopWindowManager.IsImmersiveDarkModeSupported)
            {
                switch (targetMode)
                {
                    case TitlebarColorMode.Default:
                        break;
                    case TitlebarColorMode.System:
                        if (systemMode == TitlebarColorMode.System) break;
                        ApplyTitlebarColorRule(systemMode, TitlebarColorMode.Default);
                        break;
                    case TitlebarColorMode.Light:
                        DesktopWindowManager.SetImmersiveDarkMode(WindowHandle, false);
                        break;
                    case TitlebarColorMode.Dark:
                        DesktopWindowManager.SetImmersiveDarkMode(WindowHandle, true);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else if (targetMode != TitlebarColorMode.Default)
            {
                throw new PlatformNotSupportedException("Customizing Titlebar color requires at least Windows build 19041.");
            }
        }
    }
}
