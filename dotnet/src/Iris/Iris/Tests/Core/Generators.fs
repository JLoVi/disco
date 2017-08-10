namespace Iris.Tests

#nowarn "21"
#nowarn "40"

open FsCheck
open FsCheck.GenBuilder
open Iris.Core
open Iris.Net
open Iris.Raft
open Iris.Client
open Iris.Service
open System
open System.Text

module Generators =

  //  ___         _       _     _
  // |_ _|_ __   / \   __| | __| |_ __ ___  ___ ___
  //  | || '_ \ / _ \ / _` |/ _` | '__/ _ \/ __/ __|
  //  | || |_) / ___ \ (_| | (_| | | |  __/\__ \__ \
  // |___| .__/_/   \_\__,_|\__,_|_|  \___||___/___/
  //     |_|
  let ipv4 = gen {
      let! bts = Gen.listOfLength 4 Arb.generate<byte>
      let addr =
        bts
        |> List.map (int >> string)
        |> fun parts -> String.Join(".", parts)
      return addr
    }

  let ipv6 = gen {
      let! bts = Gen.listOfLength 16 Arb.generate<byte>
      let addr =
        bts
        |> List.map (fun bte -> String.Format("{0:X2}",bte))
        |> List.chunkBySize 2
        |> List.fold
          (fun m lst ->
            match lst with
            | [ fst; snd ] -> fst + snd :: m
            | _ -> m)
          []
        |> fun lst -> String.Join(":", lst)
      return addr
    }

  let ipGen =
    Gen.oneof [ Gen.map IPv4Address ipv4
                Gen.map IPv6Address ipv6 ]

  let maybeEncode (str: string) =
    match str with
    | null -> null
    | _ -> str |> Encoding.UTF8.GetBytes |> Convert.ToBase64String

  let stringGen = Arb.generate<string> |> Gen.map maybeEncode
  let stringsGen = Gen.arrayOfLength 2 stringGen
  let intGen = Arb.generate<int>
  let intsGen = Gen.arrayOfLength 4 intGen
  let boolGen = Arb.generate<bool>
  let boolsGen = Gen.arrayOfLength 4 boolGen
  let byteGen = Arb.generate<byte>
  let bytesGen = Gen.arrayOfLength 4 byteGen
  let doubleGen = Arb.generate<double>
  let doublesGen = Gen.arrayOfLength 4 doubleGen
  let uint8Gen = Arb.generate<uint8>
  let uint16Gen = Arb.generate<uint16>
  let uint32Gen = Arb.generate<uint32>

  let indexGen = Gen.map index intGen
  let termGen = Gen.map term intGen
  let nameGen = Gen.map name stringGen
  let emailGen = Gen.map email stringGen
  let hashGen = Gen.map checksum stringGen
  let pathGen = Gen.map filepath stringGen
  let portGen = Gen.map port uint16Gen
  let versionGen = Gen.map version stringGen
  let tsGen = Arb.generate<TimeStamp>
  let tagGen = Gen.map astag stringGen
  let timeoutGen = Gen.map ((*) 1<ms>) intGen

  let inline maybeGen g = Gen.oneof [ Gen.constant None
                                      Gen.map Some g ]

  let inline mapGen g = Gen.arrayOfLength 2 g |> Gen.map (Array.map toPair >> Map.ofArray)

  //  ___    _
  // |_ _|__| |
  //  | |/ _` |
  //  | | (_| |
  // |___\__,_|

  let idGen = gen {
      let! value = Arb.generate<Guid>
      return Id (string value)
    }

  //   ____       _     _
  //  / ___|_   _(_) __| |
  // | |  _| | | | |/ _` |
  // | |_| | |_| | | (_| |
  //  \____|\__,_|_|\__,_|

  let guidGen = gen {
      let guid = Guid.NewGuid()
      return guid
    }

  //  __  __            _     _
  // |  \/  | __ _  ___| |__ (_)_ __   ___
  // | |\/| |/ _` |/ __| '_ \| | '_ \ / _ \
  // | |  | | (_| | (__| | | | | | | |  __/
  // |_|  |_|\__,_|\___|_| |_|_|_| |_|\___|

  let machineGen = gen {
      let! id = idGen
      let! hn = nameGen
      let! wrksp = pathGen
      let! logpth = pathGen
      let! ba = ipGen
      let! wp = portGen
      let! rp = portGen
      let! wsp = portGen
      let! gp = portGen
      let! ap = portGen
      let! vs = versionGen
      return
        { MachineId = id
          HostName = hn
          WorkSpace = wrksp
          LogDirectory = logpth
          BindAddress = ba
          WebPort = wp
          RaftPort = rp
          WsPort = wsp
          GitPort = gp
          ApiPort = ap
          Version = vs }
    }

  //  ____        __ _   ____  _        _
  // |  _ \ __ _ / _| |_/ ___|| |_ __ _| |_ ___
  // | |_) / _` | |_| __\___ \| __/ _` | __/ _ \
  // |  _ < (_| |  _| |_ ___) | || (_| | ||  __/
  // |_| \_\__,_|_|  \__|____/ \__\__,_|\__\___|

  let raftStateGen = Gen.oneof [ Gen.constant Joining
                                 Gen.constant Running
                                 Gen.constant Failed ]

  //  ____        __ _   __  __                _
  // |  _ \ __ _ / _| |_|  \/  | ___ _ __ ___ | |__   ___ _ __
  // | |_) / _` | |_| __| |\/| |/ _ \ '_ ` _ \| '_ \ / _ \ '__|
  // |  _ < (_| |  _| |_| |  | |  __/ | | | | | |_) |  __/ |
  // |_| \_\__,_|_|  \__|_|  |_|\___|_| |_| |_|_.__/ \___|_|

  let raftMemberGen = gen {
      let! id = idGen
      let! n = nameGen
      let! ip = ipGen
      let! p = portGen
      let! wp = portGen
      let! ap = portGen
      let! gp = portGen
      let! voting = boolGen
      let! vfm = boolGen
      let! state = raftStateGen
      let! nidx = indexGen
      let! midx = indexGen
      return
        { Id         = id
          HostName   = n
          IpAddr     = ip
          Port       = p
          WsPort     = wp
          GitPort    = gp
          ApiPort    = ap
          Voting     = voting
          VotedForMe = vfm
          State      = state
          NextIndex  = nidx
          MatchIndex = midx }
    }

  let raftMemArr = Gen.arrayOfLength 2 raftMemberGen

  //  _                _                   _
  // | |    ___   __ _| |    _____   _____| |
  // | |   / _ \ / _` | |   / _ \ \ / / _ \ |
  // | |__| (_) | (_| | |__|  __/\ V /  __/ |
  // |_____\___/ \__, |_____\___| \_/ \___|_|
  //             |___/

  let logLevelGen =
    [ LogLevel.Debug
      LogLevel.Info
      LogLevel.Warn
      LogLevel.Err
      LogLevel.Trace ]
    |> List.map Gen.constant
    |> Gen.oneof

  //  ___      _      ____             __ _
  // |_ _|_ __(_)___ / ___|___  _ __  / _(_) __ _
  //  | || '__| / __| |   / _ \| '_ \| |_| |/ _` |
  //  | || |  | \__ \ |__| (_) | | | |  _| | (_| |
  // |___|_|  |_|___/\____\___/|_| |_|_| |_|\__, |
  //                                        |___/

  let audioGen = gen {
      let! sr = Arb.generate<uint32>
      return { SampleRate = sr }
    }

  let exeGen = gen {
      let! pth = pathGen
      let! vs = versionGen
      let! req = boolGen
      return { Executable = pth
               Version = vs
               Required = req }
    }

  let plugGen = gen {
      let! nm = nameGen
      let! pth = pathGen
      return { Name = nm; Path = pth }
    }

  let vvvvGen = gen {
      let! exes = Gen.arrayOfLength 2 exeGen
      let! plugs = Gen.arrayOfLength 2 plugGen
      return { Executables = exes; Plugins = plugs }
    }

  let raftConfigGen = gen {
      let! reqto = timeoutGen
      let! elto = timeoutGen
      let! mld = intGen
      let! level = logLevelGen
      let! dd = pathGen
      let! mrtr = intGen
      let! prdc = timeoutGen
      return
        { RequestTimeout = reqto
          ElectionTimeout = elto
          MaxLogDepth = mld
          LogLevel = level
          DataDir = dd
          MaxRetries = mrtr
          PeriodicInterval = prdc }
    }

  let timingGen = gen {
      let! fb = Arb.generate<uint32>
      let! input = stringGen
      let! srvs = Gen.arrayOf ipGen
      let! udp = Arb.generate<uint32>
      let! tcp = Arb.generate<uint32>
      return
        { Framebase = fb
          Input = input
          Servers = srvs
          UDPPort = udp
          TCPPort = tcp }
    }

  let hostgroupGen = gen {
      let! nm = nameGen
      let! mems = Gen.arrayOf idGen
      return { Name = nm; Members = mems }
    }

  let clusterGen = gen {
      let! id = idGen
      let! nm = nameGen
      let! mems = mapGen raftMemberGen
      let! grps = Gen.arrayOf hostgroupGen
      return
        { Id = id
          Name = nm
          Members = mems
          Groups = grps }
    }

  let rectGen = gen {
      let! w = intGen
      let! h = intGen
      return Rect (w, h)
    }

  let coordinateGen = gen {
      let! x = intGen
      let! y = intGen
      return Coordinate (x, y)
    }

  let signalGen = gen {
      let! sz = rectGen
      let! pos = coordinateGen
      return { Size = sz; Position = pos }
    }

  let regionGen = gen {
      let! id = idGen
      let! nm = nameGen
      let! srcpos = coordinateGen
      let! srcsz = rectGen
      let! outpos = coordinateGen
      let! outsz = rectGen
      return
        { Id = id
          Name = nm
          SrcPosition = srcpos
          SrcSize = srcsz
          OutputPosition = outpos
          OutputSize = outsz }
    }

  let regionmapGen = gen {
      let! id = idGen
      let! regs = Gen.arrayOf regionGen
      return { SrcViewportId = id; Regions = regs }
    }

  let displayGen = gen {
      let! id = idGen
      let! nm = nameGen
      let! sz = rectGen
      let! sigs = Gen.arrayOf signalGen
      let! rm = regionmapGen
      return
        { Id = id
          Name = nm
          Size = sz
          Signals = sigs
          RegionMap = rm }
    }

  let viewportGen = gen {
      let! id = idGen
      let! nm = nameGen
      let! pos = coordinateGen
      let! sz = rectGen
      let! outpos = coordinateGen
      let! outsz = rectGen
      let! ovlp = rectGen
      let! desc = stringGen
      return
        { Id = id
          Name = nm
          Position = pos
          Size = sz
          OutputPosition = outpos
          OutputSize = outsz
          Overlap = ovlp
          Description = desc }
    }

  let taskGen = gen {
      let! id = idGen
      let! desc = stringGen
      let! did = idGen
      let! aus = stringGen
      let! args = Gen.arrayOf (Gen.zip stringGen
                                       stringGen)
      return
        { Id = id
          Description = desc
          DisplayId = did
          AudioStream = aus
          Arguments = args }
    }

  let configGen = gen {
      let! machine = machineGen
      let! site = maybeGen idGen
      let! vs = stringGen
      let! ac = audioGen
      let! vvvv = vvvvGen
      let! raft = raftConfigGen
      let! timing = timingGen
      let! sites = Gen.arrayOf clusterGen
      let! vps = Gen.arrayOf viewportGen
      let! disps = Gen.arrayOf displayGen
      let! tks = Gen.arrayOf taskGen
      return
        { Machine = machine
          ActiveSite = site
          Version = vs
          Audio = ac
          Vvvv = vvvv
          Raft = raft
          Timing = timing
          Sites = sites
          ViewPorts = vps
          Displays = disps
          Tasks = tks }
    }

  //  ____            _           _
  // |  _ \ _ __ ___ (_) ___  ___| |_
  // | |_) | '__/ _ \| |/ _ \/ __| __|
  // |  __/| | | (_) | |  __/ (__| |_
  // |_|   |_|  \___// |\___|\___|\__|
  //               |__/

  let projectGen = gen {
      let! id  = idGen
      let! nme = nameGen
      let! pth = pathGen
      let! crt = tsGen
      let! lst = maybeGen tsGen
      let! cr  = maybeGen stringGen
      let! ath = maybeGen stringGen
      let! cfg = configGen
      return
        { Id = id
          Name = nme
          Path = pth
          CreatedOn = crt
          LastSaved = lst
          Copyright = cr
          Author = ath
          Config = cfg }
    }

  //  ___      _     _____
  // |_ _|_ __(_)___| ____|_ __ _ __ ___  _ __
  //  | || '__| / __|  _| | '__| '__/ _ \| '__|
  //  | || |  | \__ \ |___| |  | | | (_) | |
  // |___|_|  |_|___/_____|_|  |_|  \___/|_|

  let errGen =
    [ Gen.constant IrisError.OK
      Gen.map      IrisError.GitError     (Gen.two stringGen)
      Gen.map      IrisError.ProjectError (Gen.two stringGen)
      Gen.map      IrisError.SocketError  (Gen.two stringGen)
      Gen.map      IrisError.ParseError   (Gen.two stringGen)
      Gen.map      IrisError.IOError      (Gen.two stringGen)
      Gen.map      IrisError.AssetError   (Gen.two stringGen)
      Gen.map      IrisError.RaftError    (Gen.two stringGen)
      Gen.map      IrisError.ClientError  (Gen.two stringGen)
      Gen.map      IrisError.Other        (Gen.two stringGen) ]
    |> Gen.oneof

  //  ____                  _          ____  _        _
  // / ___|  ___ _ ____   _(_) ___ ___/ ___|| |_ __ _| |_ _   _ ___
  // \___ \ / _ \ '__\ \ / / |/ __/ _ \___ \| __/ _` | __| | | / __|
  //  ___) |  __/ |   \ V /| | (_|  __/___) | || (_| | |_| |_| \__ \
  // |____/ \___|_|    \_/ |_|\___\___|____/ \__\__,_|\__|\__,_|___/

  let servicestatusGen =
    [ Gen.constant ServiceStatus.Starting
      Gen.constant ServiceStatus.Running
      Gen.constant ServiceStatus.Stopping
      Gen.constant ServiceStatus.Stopped
      Gen.map ServiceStatus.Degraded errGen
      Gen.map ServiceStatus.Failed errGen
      Gen.constant ServiceStatus.Disposed ]
    |> Gen.oneof

  //  ___      _      ____ _ _            _
  // |_ _|_ __(_)___ / ___| (_) ___ _ __ | |_
  //  | || '__| / __| |   | | |/ _ \ '_ \| __|
  //  | || |  | \__ \ |___| | |  __/ | | | |_
  // |___|_|  |_|___/\____|_|_|\___|_| |_|\__|


  let clientGen = gen {
      let! id = idGen
      let! nm = stringGen
      let! sts = servicestatusGen
      let! ip = ipGen
      let! prt = portGen
      return
        { Id = id
          Role = Role.Renderer
          Name = nm
          Status = sts
          IpAddress = ip
          Port = prt }
    }

  //  ____  _
  // |  _ \(_)_ __
  // | |_) | | '_ \
  // |  __/| | | | |
  // |_|   |_|_| |_|

  let directionGen =
    [ ConnectionDirection.Input
      ConnectionDirection.Output ]
    |> List.map Gen.constant
    |> Gen.oneof

  let behaviorGen =
    [ Behavior.Simple
      Behavior.MultiLine
      Behavior.FileName
      Behavior.Directory
      Behavior.Url
      Behavior.IP ]
    |> List.map Gen.constant
    |> Gen.oneof

  let vecsizeGen =
    [ Gen.constant VecSize.Dynamic
      Gen.map VecSize.Fixed uint16Gen ]
    |> Gen.oneof

  let stringpinGen = gen {
      let! id = idGen
      let! nm = stringGen
      let! grp = idGen
      let! tgs = Gen.arrayOf tagGen
      let! dir = directionGen
      let! bh = behaviorGen
      let! mx = Gen.map ((*) 1<chars>) intGen
      let! vs = vecsizeGen
      let! lbs = Gen.arrayOf stringGen
      let! vls = Gen.arrayOf stringGen
      return
        { Id = id
          Name = nm
          PinGroup = grp
          Tags = tgs
          Direction = dir
          Behavior = bh
          MaxChars = mx
          VecSize = vs
          Labels = lbs
          Values = vls }
    }

  let numberpinGen = gen {
      let! id = idGen
      let! nm = stringGen
      let! grp = idGen
      let! tgs = Gen.arrayOf tagGen
      let! dir = directionGen
      let! vs = vecsizeGen
      let! min = intGen
      let! max = intGen
      let! unit = stringGen
      let! prec = Arb.generate<uint32>
      let! lbs = Gen.arrayOf stringGen
      let! vls = Arb.generate<double[]>
      return
        { Id = id
          Name = nm
          PinGroup = grp
          Tags = tgs
          Min = min
          Max = max
          Unit = unit
          Direction = dir
          Precision = prec
          VecSize = vs
          Labels = lbs
          Values = vls }
    }

  let boolpinGen = gen {
      let! id = idGen
      let! nm = stringGen
      let! grp = idGen
      let! tgs = Gen.arrayOf tagGen
      let! dir = directionGen
      let! vs = vecsizeGen
      let! trig = boolGen
      let! lbs = Gen.arrayOf stringGen
      let! vls = Arb.generate<bool[]>
      return
        { Id = id
          Name = nm
          PinGroup = grp
          Tags = tgs
          IsTrigger = trig
          Direction = dir
          VecSize = vs
          Labels = lbs
          Values = vls }
    }

  let bytepinGen : Gen<BytePinD> = gen {
      let! id = idGen
      let! nm = stringGen
      let! grp = idGen
      let! tgs = Gen.arrayOf tagGen
      let! dir = directionGen
      let! vs = vecsizeGen
      let! lbs = Gen.arrayOf stringGen
      let! vls = Gen.arrayOf Arb.generate<byte[]>
      return
        { Id = id
          Name = nm
          PinGroup = grp
          Tags = tgs
          VecSize = vs
          Direction = dir
          Labels = lbs
          Values = vls }
    }

  let propertyGen = gen {
      let! key = stringGen
      let! value = stringGen
      return { Key = key; Value = value }
    }

  let enumpinGen = gen {
      let! id = idGen
      let! nm = stringGen
      let! grp = idGen
      let! vs = vecsizeGen
      let! tgs = Gen.arrayOf tagGen
      let! dir = directionGen
      let! lbs = Gen.arrayOf stringGen
      let! vls = Gen.arrayOf propertyGen
      let! props = Gen.arrayOf propertyGen
      return
        { Id = id
          Name = nm
          PinGroup = grp
          Tags = tgs
          VecSize = vs
          Direction = dir
          Properties = props
          Labels = lbs
          Values = vls }
    }

  let rgbaGen = gen {
      let! red = Arb.generate<uint8>
      let! green = Arb.generate<uint8>
      let! blue = Arb.generate<uint8>
      let! alpha = Arb.generate<uint8>
      return
        { Red = red
          Green = green
          Blue = blue
          Alpha = alpha }
    }

  let hslaGen = gen {
      let! hue = Arb.generate<uint8>
      let! saturation = Arb.generate<uint8>
      let! lightness = Arb.generate<uint8>
      let! alpha = Arb.generate<uint8>
      return
        { Hue = hue
          Saturation = saturation
          Lightness = lightness
          Alpha = alpha }
    }

  let colorGen =
    Gen.oneof [ Gen.map RGBA rgbaGen
                Gen.map HSLA hslaGen ]

  let colorpinGen = gen {
      let! id = idGen
      let! nm = stringGen
      let! grp = idGen
      let! vs = vecsizeGen
      let! tgs = Gen.arrayOf tagGen
      let! dir = directionGen
      let! lbs = Gen.arrayOf stringGen
      let! vls = Gen.arrayOf colorGen
      return
        { Id = id
          Name = nm
          PinGroup = grp
          Direction = dir
          Tags = tgs
          VecSize = vs
          Labels = lbs
          Values = vls }
    }

  let pinGen =
    [ Gen.map StringPin stringpinGen
      Gen.map NumberPin numberpinGen
      Gen.map BoolPin   boolpinGen
      Gen.map BytePin   bytepinGen
      Gen.map EnumPin   enumpinGen
      Gen.map ColorPin  colorpinGen ]
    |> Gen.oneof

  let sliceGen =
    [ Gen.map StringSlice (Gen.zip indexGen stringGen)
      Gen.map NumberSlice (Gen.zip indexGen doubleGen)
      Gen.map BoolSlice   (Gen.zip indexGen boolGen)
      Gen.map ByteSlice   (Gen.zip indexGen bytesGen)
      Gen.map EnumSlice   (Gen.zip indexGen propertyGen)
      Gen.map ColorSlice  (Gen.zip indexGen colorGen) ]
    |> Gen.oneof

  let slicesGen =
    [ Gen.map StringSlices (Gen.zip idGen stringsGen)
      Gen.map NumberSlices (Gen.zip idGen doublesGen)
      Gen.map BoolSlices   (Gen.zip idGen boolsGen)
      Gen.map ByteSlices   (Gen.zip idGen (Gen.arrayOfLength 2 bytesGen))
      Gen.map EnumSlices   (Gen.zip idGen (Gen.arrayOfLength 2 propertyGen))
      Gen.map ColorSlices  (Gen.zip idGen (Gen.arrayOfLength 2 colorGen)) ]
    |> Gen.oneof

  //   ____
  //  / ___|   _  ___
  // | |  | | | |/ _ \
  // | |__| |_| |  __/
  //  \____\__,_|\___|

  let cueGen = gen {
      let! id = idGen
      let! nm = nameGen
      let! slcs = Gen.arrayOfLength 2 slicesGen
      return
        { Id = id
          Name = nm
          Slices = slcs }
    }

  //   ____           _     _     _
  //  / ___|   _  ___| |   (_)___| |_
  // | |  | | | |/ _ \ |   | / __| __|
  // | |__| |_| |  __/ |___| \__ \ |_
  //  \____\__,_|\___|_____|_|___/\__|

  let cueReferenceGen = gen {
      let! id = idGen
      let! cue = idGen
      let! af = intGen
      let! dur = intGen
      let! pw = intGen
      return
        { Id = id
          CueId = cue
          AutoFollow = af
          Duration = dur
          Prewait = pw }
    }

  let cueGroupGen = gen {
      let! id = idGen
      let! nm = nameGen
      let! refs = Gen.arrayOf cueReferenceGen
      return
        { Id = id
          Name = nm
          CueRefs = refs }
    }

  let cuelistGen = gen {
      let! id = idGen
      let! nm = nameGen
      let! groups = Gen.arrayOf cueGroupGen
      return
        { Id = id
          Name = nm
          Groups = groups }
    }

  //   ____           ____  _
  //  / ___|   _  ___|  _ \| | __ _ _   _  ___ _ __
  // | |  | | | |/ _ \ |_) | |/ _` | | | |/ _ \ '__|
  // | |__| |_| |  __/  __/| | (_| | |_| |  __/ |
  //  \____\__,_|\___|_|   |_|\__,_|\__, |\___|_|
  //                                |___/

  let cuePlayerGen = gen {
      let! id = idGen
      let! nm = nameGen
      let! cl = maybeGen idGen
      let! sel = indexGen
      let! call = pinGen
      let! next = pinGen
      let! prev = pinGen
      let! rmw = intGen
      let! lcd = maybeGen idGen
      let! lcr = maybeGen idGen
      return
        { Id = id
          Name = nm
          CueList = cl
          Selected = sel
          Call = call
          Next = next
          Previous = prev
          RemainingWait = rmw
          LastCalled = lcd
          LastCaller = lcr }
    }

  //  ____  _        ____
  // |  _ \(_)_ __  / ___|_ __ ___  _   _ _ __
  // | |_) | | '_ \| |  _| '__/ _ \| | | | '_ \
  // |  __/| | | | | |_| | | | (_) | |_| | |_) |
  // |_|   |_|_| |_|\____|_|  \___/ \__,_| .__/
  //                                     |_|

  let pingroupGen = gen {
      let! id = idGen
      let! nm = nameGen
      let! clnt = idGen
      let! pins = mapGen pinGen
      return
        { Id = id
          Name = nm
          Client = clnt
          Pins = pins }
    }

  //  _   _
  // | | | |___  ___ _ __
  // | | | / __|/ _ \ '__|
  // | |_| \__ \  __/ |
  //  \___/|___/\___|_|

  let userGen = gen {
      let! id = idGen
      let! un = nameGen
      let! fn = nameGen
      let! ln = nameGen
      let! em = emailGen
      let! pw = hashGen
      let! slt = hashGen
      let! jnd = Arb.generate<DateTime>
      let! crt = Arb.generate<DateTime>
      return
        { Id = id
          UserName = un
          FirstName = fn
          LastName = ln
          Email = em
          Password = pw
          Salt = slt
          Joined = jnd
          Created = crt }
    }

  //  ____                _
  // / ___|  ___  ___ ___(_) ___  _ __
  // \___ \ / _ \/ __/ __| |/ _ \| '_ \
  //  ___) |  __/\__ \__ \ | (_) | | | |
  // |____/ \___||___/___/_|\___/|_| |_|

  let sessionGen = gen {
      let! id = idGen
      let! ip = ipGen
      let! ua = stringGen
      return
        { Id = id
          IpAddress = ip
          UserAgent = ua }
    }

  //  ____  _                                     _
  // |  _ \(_)___  ___ _____   _____ _ __ ___  __| |
  // | | | | / __|/ __/ _ \ \ / / _ \ '__/ _ \/ _` |
  // | |_| | \__ \ (_| (_) \ V /  __/ | |  __/ (_| |
  // |____/|_|___/\___\___/ \_/ \___|_|  \___|\__,_|

  let machinestatusGen =
    [ Gen.constant Idle
      Gen.map Busy (Gen.zip idGen nameGen) ]
    |> Gen.oneof

  let protocolGen =
    [ IPv4; IPv6 ]
    |> List.map Gen.constant
    |> Gen.oneof

  let servicetypeGen =
    [ ServiceType.Git
      ServiceType.Raft
      ServiceType.Http
      ServiceType.WebSocket
      ServiceType.Api ]
    |> List.map Gen.constant
    |> Gen.oneof

  let exposedserviceGen = gen {
      let! tpe = servicetypeGen
      let! prt = portGen
      return { ServiceType = tpe; Port = prt }
    }

  let discoveredGen = gen {
      let! id = idGen
      let! nm = stringGen
      let! fn = stringGen
      let! hn = stringGen
      let! ht = stringGen
      let! mst = machinestatusGen
      let! als = Gen.arrayOf stringGen
      let! prt = protocolGen
      let! addrs = Gen.arrayOf ipGen
      let! srvcs = Gen.arrayOf exposedserviceGen
      let! meta = Gen.arrayOf propertyGen
      return
        { Id = id
          Name = nm
          FullName = fn
          HostName = hn
          HostTarget = ht
          Status = mst
          Aliases = als
          Protocol = prt
          AddressList = addrs
          Services = srvcs
          ExtraMetadata = meta }
    }

  //  _                _____                 _
  // | |    ___   __ _| ____|_   _____ _ __ | |_
  // | |   / _ \ / _` |  _| \ \ / / _ \ '_ \| __|
  // | |__| (_) | (_| | |___ \ V /  __/ | | | |_
  // |_____\___/ \__, |_____| \_/ \___|_| |_|\__|
  //             |___/

  let tierGen =
    [ Tier.FrontEnd
      Tier.Client
      Tier.Service ]
    |> List.map Gen.constant
    |> Gen.oneof

  let logeventGen = gen {
      let! time = Arb.generate<uint32>
      let! thread = intGen
      let! tier = tierGen
      let! id = idGen
      let! tag = stringGen
      let! level = logLevelGen
      let! msg = stringGen
      return
        { Time = time
          Thread = thread
          Tier = tier
          Id = id
          Tag = tag
          LogLevel = level
          Message = msg }
    }

  //     _                 ____                                          _
  //    / \   _ __  _ __  / ___|___  _ __ ___  _ __ ___   __ _ _ __   __| |
  //   / _ \ | '_ \| '_ \| |   / _ \| '_ ` _ \| '_ ` _ \ / _` | '_ \ / _` |
  //  / ___ \| |_) | |_) | |__| (_) | | | | | | | | | | | (_| | | | | (_| |
  // /_/   \_\ .__/| .__/ \____\___/|_| |_| |_|_| |_| |_|\__,_|_| |_|\__,_|
  //         |_|   |_|

  let appcommandGen =
    [ AppCommand.Undo
      AppCommand.Redo
      AppCommand.Reset ]
    |> List.map Gen.constant
    |> Gen.oneof

  //  ____  _        _
  // / ___|| |_ __ _| |_ ___
  // \___ \| __/ _` | __/ _ \
  //  ___) | || (_| | ||  __/
  // |____/ \__\__,_|\__\___|

  let stateGen = gen {
    let! project = projectGen
    let! groups = mapGen pingroupGen
    let! cues = mapGen cueGen
    let! cuelists = mapGen cuelistGen
    let! sessions = mapGen sessionGen
    let! users = mapGen userGen
    let! clients = mapGen clientGen
    let! players = mapGen cuePlayerGen
    let! discovered = mapGen discoveredGen
    return
      { Project            = project
        PinGroups          = groups
        Cues               = cues
        CueLists           = cuelists
        Sessions           = sessions
        Users              = users
        Clients            = clients
        CuePlayers         = players
        DiscoveredServices = discovered }
    }

  //  ____  _        _       __  __            _     _
  // / ___|| |_ __ _| |_ ___|  \/  | __ _  ___| |__ (_)_ __   ___
  // \___ \| __/ _` | __/ _ \ |\/| |/ _` |/ __| '_ \| | '_ \ / _ \
  //  ___) | || (_| | ||  __/ |  | | (_| | (__| | | | | | | |  __/
  // |____/ \__\__,_|\__\___|_|  |_|\__,_|\___|_| |_|_|_| |_|\___|

  let simpleStateMachineGen =
    [ Gen.map UpdateProject           projectGen
      Gen.constant UnloadProject
      Gen.map AddMember               raftMemberGen
      Gen.map UpdateMember            raftMemberGen
      Gen.map RemoveMember            raftMemberGen
      Gen.map AddClient               clientGen
      Gen.map UpdateClient            clientGen
      Gen.map RemoveClient            clientGen
      Gen.map AddPinGroup             pingroupGen
      Gen.map UpdatePinGroup          pingroupGen
      Gen.map RemovePinGroup          pingroupGen
      Gen.map AddPin                  pinGen
      Gen.map UpdatePin               pinGen
      Gen.map RemovePin               pinGen
      Gen.map UpdateSlices            slicesGen
      Gen.map AddCue                  cueGen
      Gen.map UpdateCue               cueGen
      Gen.map RemoveCue               cueGen
      Gen.map CallCue                 cueGen
      Gen.map AddCueList              cuelistGen
      Gen.map UpdateCueList           cuelistGen
      Gen.map RemoveCueList           cuelistGen
      Gen.map AddCuePlayer            cuePlayerGen
      Gen.map UpdateCuePlayer         cuePlayerGen
      Gen.map RemoveCuePlayer         cuePlayerGen
      Gen.map AddUser                 userGen
      Gen.map UpdateUser              userGen
      Gen.map RemoveUser              userGen
      Gen.map AddSession              sessionGen
      Gen.map UpdateSession           sessionGen
      Gen.map RemoveSession           sessionGen
      Gen.map AddDiscoveredService    discoveredGen
      Gen.map UpdateDiscoveredService discoveredGen
      Gen.map RemoveDiscoveredService discoveredGen
      Gen.map UpdateClock             uint32Gen
      Gen.map Command                 appcommandGen
      Gen.map DataSnapshot            stateGen
      Gen.map SetLogLevel             logLevelGen
      Gen.map LogMsg                  logeventGen ]

  let stateMachineBatchGen =
    Gen.map StateMachineBatch (simpleStateMachineGen |> Gen.oneof |> Gen.listOf)

  let private commandBatchGen =
    Gen.map CommandBatch stateMachineBatchGen

  let stateMachineGen =
    commandBatchGen :: simpleStateMachineGen |> Gen.oneof

  //   ____             __ _        ____ _
  //  / ___|___  _ __  / _(_) __ _ / ___| |__   __ _ _ __   __ _  ___
  // | |   / _ \| '_ \| |_| |/ _` | |   | '_ \ / _` | '_ \ / _` |/ _ \
  // | |__| (_) | | | |  _| | (_| | |___| | | | (_| | | | | (_| |  __/
  //  \____\___/|_| |_|_| |_|\__, |\____|_| |_|\__,_|_| |_|\__, |\___|
  //                         |___/                         |___/

  let changesGen = Gen.oneof [ Gen.map MemberAdded raftMemberGen
                               Gen.map MemberRemoved raftMemberGen ]

  let changeArr = Gen.arrayOf changesGen

  //  _                _____       _
  // | |    ___   __ _| ____|_ __ | |_ _ __ _   _
  // | |   / _ \ / _` |  _| | '_ \| __| '__| | | |
  // | |__| (_) | (_| | |___| | | | |_| |  | |_| |
  // |_____\___/ \__, |_____|_| |_|\__|_|   \__, |
  //             |___/                      |___/

  let rec logEntryGen = gen {
      let prev = Gen.oneof [ Gen.constant None; Gen.map Some logEntryGen ]

      let! id = idGen
      let! idx = indexGen
      let! trm = termGen
      let! lidx = indexGen
      let! ltrm = termGen
      let! entry =
        Gen.oneof [ Gen.map (fun (mems, prev) -> RaftLogEntry.Configuration(id, idx, trm, mems, prev))
                            (Gen.zip raftMemArr prev)

                    Gen.map (fun (chs, prev) -> RaftLogEntry.JointConsensus(id, idx, trm, chs, prev))
                            (Gen.zip changeArr prev)

                    Gen.map (fun (data, prev) -> RaftLogEntry.LogEntry(id, idx, trm, data, prev))
                            (Gen.zip stateMachineGen prev)

                    Gen.map (fun (mems, data) -> RaftLogEntry.Snapshot(id, idx, trm, lidx, ltrm, mems, data))
                            (Gen.zip raftMemArr stateMachineGen) ]
      return entry
    }

  //  ____        __ _   ____                            _
  // |  _ \ __ _ / _| |_|  _ \ ___  __ _ _   _  ___  ___| |_
  // | |_) / _` | |_| __| |_) / _ \/ _` | | | |/ _ \/ __| __|
  // |  _ < (_| |  _| |_|  _ <  __/ (_| | |_| |  __/\__ \ |_
  // |_| \_\__,_|_|  \__|_| \_\___|\__, |\__,_|\___||___/\__|
  //                                  |_|

  let voteRequestGen = gen {
      let! mem = raftMemberGen
      let! trm = termGen
      let! lidx = indexGen
      let! ltrm = termGen
      return
        { Term = trm
          Candidate = mem
          LastLogIndex = lidx
          LastLogTerm = ltrm }
    }

  let appendEntriesGen = gen {
      let! trm = termGen
      let! plidx = indexGen
      let! pltrm = termGen
      let! lcmt = indexGen
      let! entries = Gen.oneof [ Gen.map Some logEntryGen
                                 Gen.constant None ]
      return
        { Term         = trm
          PrevLogIdx   = plidx
          PrevLogTerm  = pltrm
          LeaderCommit = lcmt
          Entries      = entries }
    }

  let installSnapshotGen = gen {
      let! trm = termGen
      let! id = idGen
      let! lidx = indexGen
      let! ltrm = termGen
      let! data = logEntryGen
      return
        { Term = trm
          LeaderId = id
          LastIndex = lidx
          LastTerm = ltrm
          Data = data }
    }

  let raftRequestGen =
    [ Gen.map RequestVote     (Gen.zip idGen voteRequestGen)
      Gen.map AppendEntries   (Gen.zip idGen appendEntriesGen)
      Gen.map InstallSnapshot (Gen.zip idGen installSnapshotGen)
      Gen.map AppendEntry     stateMachineGen ]
    |> Gen.oneof

  //  ____        __ _   ____
  // |  _ \ __ _ / _| |_|  _ \ ___  ___ _ __   ___  _ __  ___  ___
  // | |_) / _` | |_| __| |_) / _ \/ __| '_ \ / _ \| '_ \/ __|/ _ \
  // |  _ < (_| |  _| |_|  _ <  __/\__ \ |_) | (_) | | | \__ \  __/
  // |_| \_\__,_|_|  \__|_| \_\___||___/ .__/ \___/|_| |_|___/\___|
  //                                   |_|

  let voteResponseGen = gen {
      let! trm = termGen
      let! granted = boolGen
      let! reason = maybeGen errGen
      return
        { Term = trm
          Granted = granted
          Reason = reason }
    }

  let appendResponseGen = gen {
      let! trm = termGen
      let! success = boolGen
      let! cidx = indexGen
      let! fidx = indexGen
      return
        { Term = trm
          Success = success
          CurrentIndex = cidx
          FirstIndex = fidx }
    }

  let entryResponseGen = gen {
      let! id = idGen
      let! trm = termGen
      let! idx = indexGen
      return
        { Id = id
          Term = trm
          Index = idx }
    }

  let raftResponseGen =
    [ Gen.map RequestVoteResponse     (Gen.zip idGen voteResponseGen)
      Gen.map AppendEntriesResponse   (Gen.zip idGen appendResponseGen)
      Gen.map InstallSnapshotResponse (Gen.zip idGen appendResponseGen)
      Gen.map ErrorResponse           (Gen.zip idGen errGen)
      Gen.map AppendEntryResponse     entryResponseGen
      Gen.map Redirect                raftMemberGen ]
    |> Gen.oneof

  //     _          _ ____                            _
  //    / \   _ __ (_)  _ \ ___  __ _ _   _  ___  ___| |_
  //   / _ \ | '_ \| | |_) / _ \/ _` | | | |/ _ \/ __| __|
  //  / ___ \| |_) | |  _ <  __/ (_| | |_| |  __/\__ \ |_
  // /_/   \_\ .__/|_|_| \_\___|\__, |\__,_|\___||___/\__|
  //         |_|                   |_|

  let apiRequestGen =
    [ Gen.map      ApiRequest.Snapshot   stateGen
      Gen.map      ApiRequest.Register   clientGen
      Gen.map      ApiRequest.UnRegister clientGen
      Gen.map      ApiRequest.Update     stateMachineGen ]
    |> Gen.oneof

  //     _          _ _____
  //    / \   _ __ (_) ____|_ __ _ __ ___  _ __
  //   / _ \ | '_ \| |  _| | '__| '__/ _ \| '__|
  //  / ___ \| |_) | | |___| |  | | | (_) | |
  // /_/   \_\ .__/|_|_____|_|  |_|  \___/|_|
  //         |_|

  let apiErrorGen =
    [ Gen.map ApiError.Internal         stringGen
      Gen.map ApiError.UnknownCommand   stringGen
      Gen.map ApiError.MalformedRequest stringGen ]
    |> Gen.oneof

  //     _          _ ____
  //    / \   _ __ (_)  _ \ ___  ___ _ __   ___  _ __  ___  ___
  //   / _ \ | '_ \| | |_) / _ \/ __| '_ \ / _ \| '_ \/ __|/ _ \
  //  / ___ \| |_) | |  _ <  __/\__ \ |_) | (_) | | | \__ \  __/
  // /_/   \_\ .__/|_|_| \_\___||___/ .__/ \___/|_| |_|___/\___|
  //         |_|                    |_|

  let apiResponseGen =
    [ Gen.constant OK
      Gen.constant Registered
      Gen.constant Unregistered
      Gen.map NOK apiErrorGen ]
    |> Gen.oneof

  //  ____                            _
  // |  _ \ ___  __ _ _   _  ___  ___| |_
  // | |_) / _ \/ _` | | | |/ _ \/ __| __|
  // |  _ <  __/ (_| | |_| |  __/\__ \ |_
  // |_| \_\___|\__, |\__,_|\___||___/\__|
  //               |_|

  let requestGen = gen {
      let! peerId = guidGen
      let! data = Gen.arrayOf Arb.generate<byte>
      return Request.create peerId data
    }

  //     _         _     _ _
  //    / \   _ __| |__ (_) |_ _ __ __ _ _ __ _   _
  //   / _ \ | '__| '_ \| | __| '__/ _` | '__| | | |
  //  / ___ \| |  | |_) | | |_| | | (_| | |  | |_| |
  // /_/   \_\_|  |_.__/|_|\__|_|  \__,_|_|   \__, |
  //                                          |___/

  let changeArb = Arb.fromGen changesGen
  let raftRequestArb = Arb.fromGen raftRequestGen
  let raftResponseArb = Arb.fromGen raftResponseGen
  let projectArb = Arb.fromGen projectGen
  let cueArb = Arb.fromGen cueGen
  let cuelistArb = Arb.fromGen cuelistGen
  let pingroupArb = Arb.fromGen pingroupGen
  let sessionArb = Arb.fromGen sessionGen
  let userArb = Arb.fromGen userGen
  let sliceArb = Arb.fromGen sliceGen
  let slicesArb = Arb.fromGen slicesGen
  let pinArb = Arb.fromGen pinGen
  let clientArb = Arb.fromGen clientGen
  let discoveredArb = Arb.fromGen discoveredGen
  let apiRequestArb = Arb.fromGen apiRequestGen
  let apiResponseArb = Arb.fromGen apiResponseGen
  let cuePlayerArb = Arb.fromGen cuePlayerGen
  let stateMachineArb = Arb.fromGen stateMachineGen
  let stateArb = Arb.fromGen stateGen
  let requestArb = Arb.fromGen requestGen
  let commandBatchArb = Arb.fromGen stateMachineBatchGen
