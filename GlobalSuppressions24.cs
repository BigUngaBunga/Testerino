﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>", Scope = "member", Target = "~F:kcp2k.KcpConnection.CHANNEL_HEADER_SIZE")]
[assembly: SuppressMessage("Major Code Smell", "S2933:Fields that are only assigned in the constructor should be \"readonly\"", Justification = "<Pending>", Scope = "member", Target = "~F:kcp2k.KcpConnection.kcpMessageBuffer")]
[assembly: SuppressMessage("Major Code Smell", "S2933:Fields that are only assigned in the constructor should be \"readonly\"", Justification = "<Pending>", Scope = "member", Target = "~F:kcp2k.KcpConnection.kcpSendBuffer")]
[assembly: SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>", Scope = "member", Target = "~F:kcp2k.KcpConnection.QueueDisconnectThreshold")]
[assembly: SuppressMessage("Major Code Smell", "S2933:Fields that are only assigned in the constructor should be \"readonly\"", Justification = "<Pending>", Scope = "member", Target = "~F:kcp2k.KcpConnection.rawSendBuffer")]
[assembly: SuppressMessage("Major Code Smell", "S2933:Fields that are only assigned in the constructor should be \"readonly\"", Justification = "<Pending>", Scope = "member", Target = "~F:kcp2k.KcpServer.connectionsToRemove")]
[assembly: SuppressMessage("Style", "IDE0022:Use expression body for methods", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.Kcp.AckPush(System.UInt32,System.UInt32)")]
[assembly: SuppressMessage("Major Code Smell", "S1854:Unused assignments should be removed", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.Kcp.Check(System.UInt32)~System.UInt32")]
[assembly: SuppressMessage("Major Code Smell", "S1066:Collapsible \"if\" statements should be merged", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.Kcp.Flush")]
[assembly: SuppressMessage("Major Code Smell", "S1066:Collapsible \"if\" statements should be merged", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.Kcp.Input(System.Byte[],System.Int32,System.Int32)~System.Int32")]
[assembly: SuppressMessage("Major Code Smell", "S1172:Unused method parameters should be removed", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.Kcp.ParseFastack(System.UInt32,System.UInt32)")]
[assembly: SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.Kcp.Receive(System.Byte[],System.Int32)~System.Int32")]
[assembly: SuppressMessage("Style", "IDE0022:Use expression body for methods", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.Kcp.SegmentDelete(kcp2k.Segment)")]
[assembly: SuppressMessage("Style", "IDE0022:Use expression body for methods", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.Kcp.SegmentNew~kcp2k.Segment")]
[assembly: SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.Kcp.SetNoDelay(System.UInt32,System.UInt32,System.Int32,System.Boolean)")]
[assembly: SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.KcpClient.#ctor(System.Action,System.Action{System.ArraySegment{System.Byte}},System.Action)")]
[assembly: SuppressMessage("Style", "IDE0017:Simplify object initialization", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.KcpClient.Connect(System.String,System.UInt16,System.Boolean,System.UInt32,System.Int32,System.Boolean,System.UInt32,System.UInt32)")]
[assembly: SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.KcpClient.Connect(System.String,System.UInt16,System.Boolean,System.UInt32,System.Int32,System.Boolean,System.UInt32,System.UInt32)")]
[assembly: SuppressMessage("Style", "IDE0022:Use expression body for methods", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.KcpClient.TickOutgoing")]
[assembly: SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.KcpClientConnection.RawReceive")]
[assembly: SuppressMessage("Major Code Smell", "S108:Nested blocks of code should not be left empty", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.KcpClientConnection.RawReceive")]
[assembly: SuppressMessage("Style", "IDE0022:Use expression body for methods", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.KcpClientConnection.RawSend(System.Byte[],System.Int32)")]
[assembly: SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.KcpConnection.HandlePing(System.UInt32)")]
[assembly: SuppressMessage("Info Code Smell", "S1135:Track uses of \"TODO\" tags", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.KcpConnection.SendReliable(kcp2k.KcpHeader,System.ArraySegment{System.Byte})")]
[assembly: SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.KcpConnection.TickIncoming_Authenticated(System.UInt32)")]
[assembly: SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.KcpServer.#ctor(System.Action{System.Int32},System.Action{System.Int32,System.ArraySegment{System.Byte}},System.Action{System.Int32},System.Boolean,System.UInt32,System.Int32,System.Boolean,System.UInt32,System.UInt32)")]
[assembly: SuppressMessage("Style", "IDE0017:Simplify object initialization", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.KcpServer.Start(System.UInt16)")]
[assembly: SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.KcpServer.TickIncoming")]
[assembly: SuppressMessage("Major Code Smell", "S108:Nested blocks of code should not be left empty", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.KcpServer.TickIncoming")]
[assembly: SuppressMessage("Info Code Smell", "S1135:Track uses of \"TODO\" tags", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.KcpServer.TickIncoming")]
[assembly: SuppressMessage("Style", "IDE0022:Use expression body for methods", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.KcpServerConnection.RawSend(System.Byte[],System.Int32)")]
[assembly: SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>", Scope = "member", Target = "~F:kcp2k.Segment.Pool")]
[assembly: SuppressMessage("Style", "IDE0022:Use expression body for methods", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.Utils.TimeDiff(System.UInt32,System.UInt32)~System.Int32")]
[assembly: SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "<Pending>", Scope = "member", Target = "~F:kcp2k.KcpConnection.kcpMessageBuffer")]
[assembly: SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "<Pending>", Scope = "member", Target = "~F:kcp2k.KcpConnection.kcpSendBuffer")]
[assembly: SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "<Pending>", Scope = "member", Target = "~F:kcp2k.KcpConnection.rawSendBuffer")]
[assembly: SuppressMessage("Style", "IDE0048:Add parentheses for clarity", Justification = "<Pending>", Scope = "member", Target = "~F:kcp2k.KcpConnection.ReliableMaxMessageSize")]
[assembly: SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "<Pending>", Scope = "member", Target = "~F:kcp2k.KcpServer.connectionsToRemove")]
[assembly: SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.Kcp.Check(System.UInt32)~System.UInt32")]
[assembly: SuppressMessage("Style", "IDE0047:Remove unnecessary parentheses", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.Kcp.Input(System.Byte[],System.Int32,System.Int32)~System.Int32")]
[assembly: SuppressMessage("Style", "IDE0048:Add parentheses for clarity", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.Kcp.Input(System.Byte[],System.Int32,System.Int32)~System.Int32")]
[assembly: SuppressMessage("Style", "IDE0045:Convert to conditional expression", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.Kcp.Send(System.Byte[],System.Int32,System.Int32)~System.Int32")]
[assembly: SuppressMessage("Style", "IDE0045:Convert to conditional expression", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.Kcp.SetNoDelay(System.UInt32,System.UInt32,System.Int32,System.Boolean)")]
[assembly: SuppressMessage("Style", "IDE0048:Add parentheses for clarity", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.Kcp.UpdateAck(System.Int32)")]
[assembly: SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.Kcp.WndUnused~System.UInt32")]
[assembly: SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.KcpServer.GetClientAddress(System.Int32)~System.String")]
[assembly: SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "<Pending>", Scope = "member", Target = "~M:kcp2k.Utils.Clamp(System.Int32,System.Int32,System.Int32)~System.Int32")]
