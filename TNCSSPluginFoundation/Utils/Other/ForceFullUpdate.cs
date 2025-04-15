using System.Runtime.InteropServices;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory;

namespace TNCSSPluginFoundation.Utils.Other;


/// <summary>
///
/// This class provides  xxxx
///
/// The original snippet from CSSharp's discord thread, and authored by Gold KingZ and rc.
/// https://discord.com/channels/1160907911501991946/1345830591882330152/1345830591882330152
///
/// And optimized by uru
/// 
/// </summary>
public static class ForceFullUpdate
{
    private delegate IntPtr GetAddonNameDelegate(IntPtr thisPtr);
    private static readonly INetworkServerService NetworkServerService = new();

    [StructLayout(LayoutKind.Sequential)]
    struct CUtlMemory
    {
        public unsafe nint* m_pMemory;
        public int m_nAllocationCount;
        public int m_nGrowSize;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct CUtlVector
    {
        public unsafe nint this[int index]
        {
            get => this.m_Memory.m_pMemory[index];
            set => this.m_Memory.m_pMemory[index] = value;
        }

        public int m_iSize;
        public CUtlMemory m_Memory;

        public nint Element(int index) => this[index];
    }

    /// <summary>
    /// See CSSharp's discord thread for details
    /// https://discord.com/channels/1160907911501991946/1345830591882330152/1345830591882330152
    /// </summary>
    public class INetworkServerService : NativeObject
    {
        private readonly VirtualFunctionWithReturn<nint, nint> GetIGameServerFunc;
        
        /// <summary>
        /// See CSSharp's discord thread for details
        /// https://discord.com/channels/1160907911501991946/1345830591882330152/1345830591882330152
        /// </summary>
        public INetworkServerService() : base(NativeAPI.GetValveInterface(0, "NetworkServerService_001"))
        {
            this.GetIGameServerFunc = new VirtualFunctionWithReturn<nint, nint>(this.Handle, GameData.GetOffset("INetworkServerService_GetIGameServer"));
        }
        
        /// <summary>
        /// See CSSharp's discord thread for details
        /// https://discord.com/channels/1160907911501991946/1345830591882330152/1345830591882330152
        /// </summary>
        /// <returns></returns>
        public INetworkGameServer GetIGameServer()
        {
            return new INetworkGameServer(this.GetIGameServerFunc.Invoke(this.Handle));
        }
    }

    
    /// <summary>
    /// See CSSharp's discord thread for details
    /// https://discord.com/channels/1160907911501991946/1345830591882330152/1345830591882330152
    /// </summary>
    public class INetworkGameServer : NativeObject
    {
        private static int SlotsOffset = GameData.GetOffset("INetworkGameServer_Slots");

        private CUtlVector Slots;
    
        /// <summary>
        /// See CSSharp's discord thread for details
        /// https://discord.com/channels/1160907911501991946/1345830591882330152/1345830591882330152
        /// </summary>
        /// <param name="ptr"></param>
        public INetworkGameServer(nint ptr) : base(ptr)
        {
            this.Slots = Marshal.PtrToStructure<CUtlVector>(base.Handle + SlotsOffset);
        }
    }

    /// <summary>
    /// Obtain workshop ID from server.
    /// </summary>
    /// <returns>Returns current workshop map ID</returns>
    public static string GetWorkshopId()
    {
        IntPtr networkGameServer = NetworkServerService.GetIGameServer().Handle;
        IntPtr vtablePtr = Marshal.ReadIntPtr(networkGameServer);
        IntPtr functionPtr = Marshal.ReadIntPtr(vtablePtr + (25 * IntPtr.Size));
        var getAddonName = Marshal.GetDelegateForFunctionPointer<GetAddonNameDelegate>(functionPtr);
        IntPtr result = getAddonName(networkGameServer);
        return Marshal.PtrToStringAnsi(result)!.Split(',')[0];
    }
}