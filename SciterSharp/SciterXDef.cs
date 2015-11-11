﻿#pragma warning disable 0169

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SciterSharp
{
	public static class SciterXDef
	{
		public enum SciterResourceType : uint
		{
			RT_DATA_HTML = 0,
			RT_DATA_IMAGE = 1,
			RT_DATA_STYLE = 2,
			RT_DATA_CURSOR = 3,
			RT_DATA_SCRIPT = 4,
			RT_DATA_RAW = 5,
		}

		public enum LoadResult : uint
		{
			LOAD_OK = 0,	  // do default loading if data not set
			LOAD_DISCARD = 1, // discard request completely
			LOAD_DELAYED = 2, // data will be delivered later by the host
							  // Host application must call SciterDataReadyAsync(,,, requestId) on each LOAD_DELAYED request to avoid memory leaks.
		}

		public const uint SC_LOAD_DATA = 0x01;
		public const uint SC_DATA_LOADED = 0x02;
		public const uint SC_ATTACH_BEHAVIOR = 0x04;
		public const uint SC_ENGINE_DESTROYED = 0x05;
		public const uint SC_POSTED_NOTIFICATION = 0x06;

		[StructLayout(LayoutKind.Sequential)]
		public struct SCITER_CALLBACK_NOTIFICATION
		{
			public uint code;// SC_LOAD_DATA or SC_DATA_LOADED or ..
			public IntPtr hwnd;
		}

		public delegate uint FPTR_SciterHostCallback(IntPtr ns /*SCITER_CALLBACK_NOTIFICATION*/, IntPtr callbackParam);

		[StructLayout(LayoutKind.Sequential)]
		public struct SCN_LOAD_DATA
		{
			public uint code;				// UINT - [in] one of the codes above.
			public IntPtr hwnd;				// HWINDOW - [in] HWINDOW of the window this callback was attached to.

			[MarshalAs(UnmanagedType.LPWStr)]
			public string uri;				// LPCWSTR - [in] Zero terminated string, fully qualified uri, for example "http://server/folder/file.ext".

			public byte[] outData;			// LPCBYTE - [in,out] pointer to loaded data to return. if data exists in the cache then this field contain pointer to it
			public uint outDataSize;		// UINT - [in,out] loaded data size to return.
			public uint dataType;			// UINT - [in] SciterResourceType

			public IntPtr requestId;		// LPVOID - [in] request id that needs to be passed as is to the SciterDataReadyAsync call

			public IntPtr principal;		// HELEMENT
			public IntPtr initiator;		// HELEMENT
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct SCN_DATA_LOADED
		{
			public uint code;	// UINT - [in] one of the codes above.
			public IntPtr hwnd;	// HWINDOW - [in] HWINDOW of the window this callback was attached to.
			public IntPtr uri;	// LPCWSTR - [in] Zero terminated string, fully qualified uri, for example "http://server/folder/file.ext".

			public IntPtr data;		// LPCBYTE - [in] pointer to loaded data.
			public uint dataSize;	// UINT - [in] loaded data size (in bytes).
			public uint dataType;	// UINT - [in] SciterResourceType
			public uint status;		// UINT - [in] 
									// status = 0 (dataSize == 0) - unknown error. 
									// status = 100..505 - http response status, Note: 200 - OK! 
									// status > 12000 - wininet error code, see ERROR_INTERNET_*** in wininet.h
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct SCN_ATTACH_BEHAVIOR
		{
			public uint code;			// UINT - [in] one of the codes above.
			public IntPtr hwnd;			// HWINDOW - [in] HWINDOW of the window this callback was attached to.

			public IntPtr elem;			// HELEMENT - [in] target DOM element handle
			public IntPtr behaviorName;	// LPCSTR - [in] zero terminated string, string appears as value of CSS behavior:"???" attribute.

			public SciterXBehaviors.FPTR_ElementEventProc elementProc;	// ElementEventProc - [out] pointer to ElementEventProc function.
			public IntPtr elementTag;	// LPVOID - [out] tag value, passed as is into pointer ElementEventProc function.
		}
		
		[StructLayout(LayoutKind.Sequential)]
		public struct SCN_ENGINE_DESTROYED
		{
			public uint code;	// UINT [in] one of the codes above.
			public IntPtr hwnd;	// HWINDOW - [in] HWINDOW of the window this callback was attached to.
		}

		public struct SCN_POSTED_NOTIFICATION
		{
			public uint	code;		// UINT - [in] one of the codes above.*/
			public IntPtr	hwnd;	// HWINDOW - [in] HWINDOW of the window this callback was attached to.*/
			public IntPtr	wparam;	// UINT_PTR
			public IntPtr	lparam;	// UINT_PTR
			public IntPtr	lreturn;// UINT_PTR
		}


		public enum SCRIPT_RUNTIME_FEATURES : uint
		{
			ALLOW_FILE_IO = 0x00000001,
			ALLOW_SOCKET_IO = 0x00000002,
			ALLOW_EVAL = 0x00000004,
			ALLOW_SYSINFO = 0x00000008
		}

		public enum GFX_LAYER : uint
		{
			GFX_LAYER_GDI      = 1,
			GFX_LAYER_WARP     = 2,
			GFX_LAYER_D2D      = 3,
			GFX_LAYER_AUTO     = 0xFFFF,
		}

		public enum SCITER_RT_OPTIONS : uint
		{
			SCITER_SMOOTH_SCROLL = 1,		 // value:TRUE - enable, value:FALSE - disable, enabled by default
			SCITER_CONNECTION_TIMEOUT = 2,	 // value: milliseconds, connection timeout of http client
			SCITER_HTTPS_ERROR = 3,			 // value: 0 - drop connection, 1 - use builtin dialog, 2 - accept connection silently
			SCITER_FONT_SMOOTHING = 4,		 // value: 0 - system default, 1 - no smoothing, 2 - std smoothing, 3 - clear type

			SCITER_TRANSPARENT_WINDOW = 6,	// Windows Aero support, value: 
											// 0 - normal drawing, 
											// 1 - window has transparent background after calls DwmExtendFrameIntoClientArea() or DwmEnableBlurBehindWindow().
			SCITER_SET_GPU_BLACKLIST  = 7,	// hWnd = NULL,
											// value = LPCBYTE, json - GPU black list, see: gpu-blacklist.json resource.
			SCITER_SET_SCRIPT_RUNTIME_FEATURES = 8, // value - combination of SCRIPT_RUNTIME_FEATURES flags.
			SCITER_SET_GFX_LAYER = 9,		// hWnd = NULL, value - GFX_LAYER
			SCITER_SET_DEBUG_MODE = 10,		// hWnd, value - TRUE/FALSE
			SCITER_SET_UX_THEMING = 11,		// hWnd = NULL, value - BOOL, TRUE - the engine will use "unisex" theme that is common for all platforms. 
											// That UX theme is not using OS primitives for rendering input elements. Use it if you want exactly
											// the same (modulo fonts) look-n-feel on all platforms.

			SCITER_ALPHA_WINDOW  = 12,		// hWnd, value - TRUE/FALSE - window uses per pixel alpha (e.g. WS_EX_LAYERED/UpdateLayeredWindow() window)
		}

#if WIN32
		public delegate IntPtr FPTR_SciterWindowDelegate(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam, IntPtr pParam, ref bool handled);
#elif OSX
		// SciterWindowDelegate = void*		Obj-C id, NSWindowDelegate and NSResponder
#elif LINUX
		// SciterWindowDelegate = void*
#endif


		public enum SCITER_CREATE_WINDOW_FLAGS : uint
		{
			SW_CHILD      = (1 << 0), // child window only, if this flag is set all other flags ignored
			SW_TITLEBAR   = (1 << 1), // toplevel window, has titlebar
			SW_RESIZEABLE = (1 << 2), // has resizeable frame
			SW_TOOL       = (1 << 3), // is tool window
			SW_CONTROLS   = (1 << 4), // has minimize / maximize buttons
			SW_GLASSY     = (1 << 5), // glassy window ( DwmExtendFrameIntoClientArea on windows )
			SW_ALPHA      = (1 << 6), // transparent window ( e.g. WS_EX_LAYERED on Windows )
			SW_MAIN       = (1 << 7), // main window of the app, will terminate app on close
			SW_POPUP      = (1 << 8), // the window is created as topmost.
			SW_ENABLE_DEBUG = (1 << 9), // make this window inspector ready
			SW_OWNS_VM      = (1 << 10), // it has its own script VM
		}

		public enum OUTPUT_SUBSYTEM : uint
		{
			OT_DOM = 0,       // html parser & runtime
			OT_CSSS,          // csss! parser & runtime
			OT_CSS,           // css parser
			OT_TIS,           // TIS parser & runtime
		}
		public enum OUTPUT_SEVERITY : uint
		{
			OS_INFO,
			OS_WARNING,
			OS_ERROR,
		}


		// alias DEBUG_OUTPUT_PROC = VOID function(LPVOID param, UINT subsystem /*OUTPUT_SUBSYTEMS*/, UINT severity, LPCWSTR text, UINT text_length);
		public delegate IntPtr FPTR_DEBUG_OUTPUT_PROC(IntPtr param, uint subsystem /*OUTPUT_SUBSYTEMS*/, uint severity /*OUTPUT_SEVERITY*/, string text, uint text_length);// TODO this string

	}
}

#pragma warning restore 0169