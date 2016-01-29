namespace Iris.Core.Types.Config

/// Configuration type and logic for the Iris service.
[<AutoOpen>]
module Vsync =

  /// internal model wrap multiple option types
  type private vsyncopt =
    | VsInt  of uint32      option
    | VsStr  of string      option
    | VsBool of bool        option
    | VsList of string list option

  /// check whether the wrapped inner type is a Some
  let inline private isSome (v : vsyncopt) : bool =
    match v with
      | VsInt(value)  -> Option.isSome value
      | VsStr(value)  -> Option.isSome value
      | VsBool(value) -> Option.isSome value
      | VsList(value) -> Option.isSome value

  /// shortcut to set an environment variable
  let inline private setVar var value : unit =
    //printfn "var=%s value=%s" var value
    System.Environment.SetEnvironmentVariable(var, value)

  /// Vsync engine-specific configuration options record
  type VsyncConfig =
    { AesKey                : string      option /// AesKey yeah!
    ; DefaultTimeout        : uint32      option
    ; DontCompress          : bool        option
    ; FastEthernet          : bool        option 
    ; GracefulShutdown      : bool        option 
    ; Hosts                 : string list option
    ; IgnorePartitions      : bool        option 
    ; IgnoreSmallPartitions : bool        option 
    ; InfiniBand            : bool        option
    ; Large                 : bool        option
    ; LogDir                : string      option
    ; Logged                : bool        option
    ; MCMDReportRate        : uint32      option
    ; MCRangeHigh           : string      option
    ; MCRangeLow            : string      option
    ; MaxAsyncMTotal        : uint32      option
    ; MaxIPMCAddrs          : uint32      option
    ; MaxMsgLen             : uint32      option
    ; Mute                  : bool        option
    ; Netmask               : string      option
    ; NetworkInterfaces     : string list option
    ; OOBViaTCP             : bool        option
    ; Port                  : uint32      option
    ; PortP2P               : uint32      option
    ; RateLim               : uint32      option
    ; Sigs                  : bool        option
    ; SkipFirstInterface    : bool        option
    ; Subnet                : string      option
    ; TTL                   : uint32      option
    ; TokenDelay            : uint32      option
    ; UDPChkSum             : bool        option 
    ; UnicastOnly           : bool        option
    ; UseIPv4               : bool        option
    ; UseIPv6               : bool        option
    ; UserDMA               : bool        option
    }
    with

      override self.ToString() =
        sprintf "
                 AesKey                : %A
                 DefaultTimeout        : %A
                 DontCompress          : %A
                 FastEthernet          : %A
                 GracefulShutdown      : %A
                 Hosts                 : %A
                 IgnorePartitions      : %A
                 IgnoreSmallPartitions : %A
                 InfiniBand            : %A
                 Large                 : %A
                 LogDir                : %A
                 Logged                : %A
                 MCMDReportRate        : %A
                 MCRangeHigh           : %A
                 MCRangeLow            : %A
                 MaxAsyncMTotal        : %A
                 MaxIPMCAddrs          : %A
                 MaxMsgLen             : %A
                 Mute                  : %A
                 Netmask               : %A
                 NetworkInterfaces     : %A
                 OOBViaTCP             : %A
                 Port                  : %A
                 PortP2P               : %A
                 RateLim               : %A
                 Sigs                  : %A
                 SkipFirstInterface    : %A
                 Subnet                : %A
                 TTL                   : %A
                 TokenDelay            : %A
                 UDPChkSum             : %A
                 UnicastOnly           : %A
                 UseIPv4               : %A
                 UseIPv6               : %A
                 UserDMA               : %A"
                 self.AesKey
                 self.DefaultTimeout
                 self.DontCompress
                 self.FastEthernet
                 self.GracefulShutdown
                 self.Hosts
                 self.IgnorePartitions
                 self.IgnoreSmallPartitions
                 self.InfiniBand
                 self.Large
                 self.LogDir
                 self.Logged
                 self.MCMDReportRate
                 self.MCRangeHigh
                 self.MCRangeLow
                 self.MaxAsyncMTotal
                 self.MaxIPMCAddrs
                 self.MaxMsgLen
                 self.Mute
                 self.Netmask
                 self.NetworkInterfaces
                 self.OOBViaTCP
                 self.Port
                 self.PortP2P
                 self.RateLim
                 self.Sigs
                 self.SkipFirstInterface
                 self.Subnet
                 self.TTL
                 self.TokenDelay
                 self.UDPChkSum
                 self.UnicastOnly
                 self.UseIPv4
                 self.UseIPv6
                 self.UserDMA


      /// Default configuration options
      static member Default =
        { AesKey                = None
        ; DefaultTimeout        = None
        ; DontCompress          = Some false
        ; FastEthernet          = Some false
        ; GracefulShutdown      = Some true
        ; Hosts                 = None
        ; IgnorePartitions      = Some true
        ; IgnoreSmallPartitions = Some true
        ; InfiniBand            = Some false
        ; Large                 = Some false
        ; LogDir                = None 
        ; Logged                = Some true
        ; MCMDReportRate        = None
        ; MCRangeHigh           = None
        ; MCRangeLow            = None
        ; MaxAsyncMTotal        = None
        ; MaxIPMCAddrs          = None
        ; MaxMsgLen             = None
        ; Mute                  = Some false
        ; Netmask               = None
        ; NetworkInterfaces     = None
        ; OOBViaTCP             = Some true
        ; Port                  = None
        ; PortP2P               = None
        ; RateLim               = None
        ; Sigs                  = Some false
        ; SkipFirstInterface    = Some false
        ; Subnet                = None
        ; TTL                   = None
        ; TokenDelay            = None
        ; UDPChkSum             = Some false
        ; UnicastOnly           = Some false
        ; UseIPv4               = Some true
        ; UseIPv6               = Some false
        ; UserDMA               = Some false
        }

      /// Apply all configuration options to current environment
      member self.Apply() =
        [ ("VSYNC_DEFAULTTIMEOUT"        , VsInt(self.DefaultTimeout))
        ; ("VSYNC_MAXASYNCMTOTAL"        , VsInt(self.MaxAsyncMTotal))
        ; ("VSYNC_MAXIPMCADDRS"          , VsInt(self.MaxIPMCAddrs))
        ; ("VSYNC_MAXMSGLEN"             , VsInt(self.MaxMsgLen))
        ; ("VSYNC_MCMDREPORTRATE"        , VsInt(self.MCMDReportRate))
        ; ("VSYNC_MCRANGE_HIGH"          , VsStr(self.MCRangeHigh))
        ; ("VSYNC_MCRANGE_LOW"           , VsStr(self.MCRangeLow))
        ; ("VSYNC_NETMASK"               , VsStr(self.Netmask))
        ; ("VSYNC_PORTNO"                , VsInt(self.Port))
        ; ("VSYNC_PORTNOp"               , VsInt(self.PortP2P))
        ; ("VSYNC_RATELIM"               , VsInt(self.RateLim))
        ; ("VSYNC_SUBNET"                , VsStr(self.Subnet))
        ; ("VSYNC_TOKEN_DELAY"           , VsInt(self.TokenDelay))
        ; ("VSYNC_AESKEY"                , VsStr(self.AesKey))
        ; ("VSYNC_HOSTS"                 , VsList(self.Hosts))
        ; ("VSYNC_NETWORK_INTERFACES"    , VsList(self.NetworkInterfaces))
        ; ("VSYNC_DONT_COMPRESS"         , VsBool(self.DontCompress))
        ; ("VSYNC_FASTETHERNET"          , VsBool(self.FastEthernet))
        ; ("VSYNC_GRACEFULSHUTDOWN"      , VsBool(self.GracefulShutdown))
        ; ("VSYNC_IGNOREPARTITIONS"      , VsBool(self.IgnorePartitions))
        ; ("VSYNC_IGNORESMALLPARTITIONS" , VsBool(self.IgnoreSmallPartitions))
        ; ("VSYNC_INFINIBAND"            , VsBool(self.InfiniBand))
        ; ("VSYNC_LARGE"                 , VsBool(self.Large))
        ; ("VSYNC_LOGDIR"                , VsStr(self.LogDir))
        ; ("VSYNC_LOGGED"                , VsBool(self.Logged))
        ; ("VSYNC_MUTE"                  , VsBool(self.Mute))
        ; ("VSYNC_OOBVIATCP"             , VsBool(self.OOBViaTCP))
        ; ("VSYNC_SIGS"                  , VsBool(self.Sigs))
        ; ("VSYNC_SKIPFIRSTINTERFACE"    , VsBool(self.SkipFirstInterface))
        ; ("VSYNC_TTL"                   , VsInt(self.TTL))
        ; ("VSYNC_UDPCHKSUM"             , VsBool(self.UDPChkSum))
        ; ("VSYNC_UNICAST_ONLY"          , VsBool(self.UnicastOnly))
        ; ("VSYNC_USEIPv4"               , VsBool(self.UseIPv4))
        ; ("VSYNC_USEIPv6"               , VsBool(self.UseIPv6))
        ; ("VSYNC_USERDMA"               , VsBool(self.UserDMA))
        ]
        |> List.iter (fun (name, value) ->
          if isSome value
          then match value with
                 | VsInt(value')  -> setVar name (string (Option.get value'))
                 | VsStr(value')  -> setVar name (Option.get value')
                 | VsBool(value') -> setVar name (string (Option.get value'))
                 | VsList(value') ->
                   let folded =
                     Option.get value'
                     |> List.fold (fun (m : string) s ->
                                   match m.Length with
                                     | 0 -> s 
                                     | _ -> m + " " + s ) ""
                   setVar name folded)