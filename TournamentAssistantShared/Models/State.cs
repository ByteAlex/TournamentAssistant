// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: state.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace TournamentAssistantShared.Models {

  /// <summary>Holder for reflection information generated from state.proto</summary>
  public static partial class StateReflection {

    #region Descriptor
    /// <summary>File descriptor for state.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static StateReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CgtzdGF0ZS5wcm90bxIgVG91cm5hbWVudEFzc2lzdGFudFNoYXJlZC5Nb2Rl",
            "bHMaFXNlcnZlcl9zZXR0aW5ncy5wcm90bxoMcGxheWVyLnByb3RvGhFjb29y",
            "ZGluYXRvci5wcm90bxoLbWF0Y2gucHJvdG8aFXF1YWxpZmllcl9ldmVudC5w",
            "cm90bxoRY29yZV9zZXJ2ZXIucHJvdG8ikQMKBVN0YXRlEkkKD3NlcnZlcl9z",
            "ZXR0aW5ncxgBIAEoCzIwLlRvdXJuYW1lbnRBc3Npc3RhbnRTaGFyZWQuTW9k",
            "ZWxzLlNlcnZlclNldHRpbmdzEjkKB3BsYXllcnMYAiADKAsyKC5Ub3VybmFt",
            "ZW50QXNzaXN0YW50U2hhcmVkLk1vZGVscy5QbGF5ZXISQwoMY29vcmRpbmF0",
            "b3JzGAMgAygLMi0uVG91cm5hbWVudEFzc2lzdGFudFNoYXJlZC5Nb2RlbHMu",
            "Q29vcmRpbmF0b3ISOAoHbWF0Y2hlcxgEIAMoCzInLlRvdXJuYW1lbnRBc3Np",
            "c3RhbnRTaGFyZWQuTW9kZWxzLk1hdGNoEkAKBmV2ZW50cxgFIAMoCzIwLlRv",
            "dXJuYW1lbnRBc3Npc3RhbnRTaGFyZWQuTW9kZWxzLlF1YWxpZmllckV2ZW50",
            "EkEKC2tub3duX2hvc3RzGAYgAygLMiwuVG91cm5hbWVudEFzc2lzdGFudFNo",
            "YXJlZC5Nb2RlbHMuQ29yZVNlcnZlckIjqgIgVG91cm5hbWVudEFzc2lzdGFu",
            "dFNoYXJlZC5Nb2RlbHNiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::TournamentAssistantShared.Models.ServerSettingsReflection.Descriptor, global::TournamentAssistantShared.Models.PlayerReflection.Descriptor, global::TournamentAssistantShared.Models.CoordinatorReflection.Descriptor, global::TournamentAssistantShared.Models.MatchReflection.Descriptor, global::TournamentAssistantShared.Models.QualifierEventReflection.Descriptor, global::TournamentAssistantShared.Models.CoreServerReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::TournamentAssistantShared.Models.State), global::TournamentAssistantShared.Models.State.Parser, new[]{ "ServerSettings", "Players", "Coordinators", "Matches", "Events", "KnownHosts" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class State : pb::IMessage<State>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<State> _parser = new pb::MessageParser<State>(() => new State());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<State> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::TournamentAssistantShared.Models.StateReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public State() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public State(State other) : this() {
      serverSettings_ = other.serverSettings_ != null ? other.serverSettings_.Clone() : null;
      players_ = other.players_.Clone();
      coordinators_ = other.coordinators_.Clone();
      matches_ = other.matches_.Clone();
      events_ = other.events_.Clone();
      knownHosts_ = other.knownHosts_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public State Clone() {
      return new State(this);
    }

    /// <summary>Field number for the "server_settings" field.</summary>
    public const int ServerSettingsFieldNumber = 1;
    private global::TournamentAssistantShared.Models.ServerSettings serverSettings_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::TournamentAssistantShared.Models.ServerSettings ServerSettings {
      get { return serverSettings_; }
      set {
        serverSettings_ = value;
      }
    }

    /// <summary>Field number for the "players" field.</summary>
    public const int PlayersFieldNumber = 2;
    private static readonly pb::FieldCodec<global::TournamentAssistantShared.Models.Player> _repeated_players_codec
        = pb::FieldCodec.ForMessage(18, global::TournamentAssistantShared.Models.Player.Parser);
    private readonly pbc::RepeatedField<global::TournamentAssistantShared.Models.Player> players_ = new pbc::RepeatedField<global::TournamentAssistantShared.Models.Player>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<global::TournamentAssistantShared.Models.Player> Players {
      get { return players_; }
    }

    /// <summary>Field number for the "coordinators" field.</summary>
    public const int CoordinatorsFieldNumber = 3;
    private static readonly pb::FieldCodec<global::TournamentAssistantShared.Models.Coordinator> _repeated_coordinators_codec
        = pb::FieldCodec.ForMessage(26, global::TournamentAssistantShared.Models.Coordinator.Parser);
    private readonly pbc::RepeatedField<global::TournamentAssistantShared.Models.Coordinator> coordinators_ = new pbc::RepeatedField<global::TournamentAssistantShared.Models.Coordinator>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<global::TournamentAssistantShared.Models.Coordinator> Coordinators {
      get { return coordinators_; }
    }

    /// <summary>Field number for the "matches" field.</summary>
    public const int MatchesFieldNumber = 4;
    private static readonly pb::FieldCodec<global::TournamentAssistantShared.Models.Match> _repeated_matches_codec
        = pb::FieldCodec.ForMessage(34, global::TournamentAssistantShared.Models.Match.Parser);
    private readonly pbc::RepeatedField<global::TournamentAssistantShared.Models.Match> matches_ = new pbc::RepeatedField<global::TournamentAssistantShared.Models.Match>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<global::TournamentAssistantShared.Models.Match> Matches {
      get { return matches_; }
    }

    /// <summary>Field number for the "events" field.</summary>
    public const int EventsFieldNumber = 5;
    private static readonly pb::FieldCodec<global::TournamentAssistantShared.Models.QualifierEvent> _repeated_events_codec
        = pb::FieldCodec.ForMessage(42, global::TournamentAssistantShared.Models.QualifierEvent.Parser);
    private readonly pbc::RepeatedField<global::TournamentAssistantShared.Models.QualifierEvent> events_ = new pbc::RepeatedField<global::TournamentAssistantShared.Models.QualifierEvent>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<global::TournamentAssistantShared.Models.QualifierEvent> Events {
      get { return events_; }
    }

    /// <summary>Field number for the "known_hosts" field.</summary>
    public const int KnownHostsFieldNumber = 6;
    private static readonly pb::FieldCodec<global::TournamentAssistantShared.Models.CoreServer> _repeated_knownHosts_codec
        = pb::FieldCodec.ForMessage(50, global::TournamentAssistantShared.Models.CoreServer.Parser);
    private readonly pbc::RepeatedField<global::TournamentAssistantShared.Models.CoreServer> knownHosts_ = new pbc::RepeatedField<global::TournamentAssistantShared.Models.CoreServer>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<global::TournamentAssistantShared.Models.CoreServer> KnownHosts {
      get { return knownHosts_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as State);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(State other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(ServerSettings, other.ServerSettings)) return false;
      if(!players_.Equals(other.players_)) return false;
      if(!coordinators_.Equals(other.coordinators_)) return false;
      if(!matches_.Equals(other.matches_)) return false;
      if(!events_.Equals(other.events_)) return false;
      if(!knownHosts_.Equals(other.knownHosts_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (serverSettings_ != null) hash ^= ServerSettings.GetHashCode();
      hash ^= players_.GetHashCode();
      hash ^= coordinators_.GetHashCode();
      hash ^= matches_.GetHashCode();
      hash ^= events_.GetHashCode();
      hash ^= knownHosts_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (serverSettings_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(ServerSettings);
      }
      players_.WriteTo(output, _repeated_players_codec);
      coordinators_.WriteTo(output, _repeated_coordinators_codec);
      matches_.WriteTo(output, _repeated_matches_codec);
      events_.WriteTo(output, _repeated_events_codec);
      knownHosts_.WriteTo(output, _repeated_knownHosts_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (serverSettings_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(ServerSettings);
      }
      players_.WriteTo(ref output, _repeated_players_codec);
      coordinators_.WriteTo(ref output, _repeated_coordinators_codec);
      matches_.WriteTo(ref output, _repeated_matches_codec);
      events_.WriteTo(ref output, _repeated_events_codec);
      knownHosts_.WriteTo(ref output, _repeated_knownHosts_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (serverSettings_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(ServerSettings);
      }
      size += players_.CalculateSize(_repeated_players_codec);
      size += coordinators_.CalculateSize(_repeated_coordinators_codec);
      size += matches_.CalculateSize(_repeated_matches_codec);
      size += events_.CalculateSize(_repeated_events_codec);
      size += knownHosts_.CalculateSize(_repeated_knownHosts_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(State other) {
      if (other == null) {
        return;
      }
      if (other.serverSettings_ != null) {
        if (serverSettings_ == null) {
          ServerSettings = new global::TournamentAssistantShared.Models.ServerSettings();
        }
        ServerSettings.MergeFrom(other.ServerSettings);
      }
      players_.Add(other.players_);
      coordinators_.Add(other.coordinators_);
      matches_.Add(other.matches_);
      events_.Add(other.events_);
      knownHosts_.Add(other.knownHosts_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            if (serverSettings_ == null) {
              ServerSettings = new global::TournamentAssistantShared.Models.ServerSettings();
            }
            input.ReadMessage(ServerSettings);
            break;
          }
          case 18: {
            players_.AddEntriesFrom(input, _repeated_players_codec);
            break;
          }
          case 26: {
            coordinators_.AddEntriesFrom(input, _repeated_coordinators_codec);
            break;
          }
          case 34: {
            matches_.AddEntriesFrom(input, _repeated_matches_codec);
            break;
          }
          case 42: {
            events_.AddEntriesFrom(input, _repeated_events_codec);
            break;
          }
          case 50: {
            knownHosts_.AddEntriesFrom(input, _repeated_knownHosts_codec);
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            if (serverSettings_ == null) {
              ServerSettings = new global::TournamentAssistantShared.Models.ServerSettings();
            }
            input.ReadMessage(ServerSettings);
            break;
          }
          case 18: {
            players_.AddEntriesFrom(ref input, _repeated_players_codec);
            break;
          }
          case 26: {
            coordinators_.AddEntriesFrom(ref input, _repeated_coordinators_codec);
            break;
          }
          case 34: {
            matches_.AddEntriesFrom(ref input, _repeated_matches_codec);
            break;
          }
          case 42: {
            events_.AddEntriesFrom(ref input, _repeated_events_codec);
            break;
          }
          case 50: {
            knownHosts_.AddEntriesFrom(ref input, _repeated_knownHosts_codec);
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code
