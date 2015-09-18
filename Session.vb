' Game Jolt API for Visual Basic - For implementing game achievements 

' Copyright (C) 2015  Thomas Bowen

' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.

' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.

' You should have received a copy of the GNU General Public License
' along with this program.  If not, see <http://www.gnu.org/licenses/>.

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Text
Imports System.Security.Cryptography
Imports System.Threading.Tasks
Imports System.Net
Imports System.Net.Http
Imports System.IO
Imports System.Timers
Imports System.Xml.Serialization
Imports System.Web.Script.Serialization
Imports System.ComponentModel

''' <summary>
''' Manages user related API requests. 
''' For reference on session calls see http://gamejolt.com/api/doc/game/sessions
''' </summary>
''' <remarks></remarks>
Public Class SessionManager
    Inherits GameJolt

    ' Sessions must be pinged every 120 seconds, 30 seconds (30000 miliseconds) is recommended
    Public interval As Integer = 30000

    Private sessionTmr As Timer

    ' A flag to check if there is an active session
    Private isSession As Boolean

    Enum Status
        Active
        Idle
    End Enum

    Private _sessionStatus As Status

    Public Sub New()
        sessionTmr = New Timer()
        Me.Open()
        ' Set timer to start pinging the session
        sessionTmr.Interval = interval

        AddHandler sessionTmr.Elapsed, New ElapsedEventHandler(AddressOf sessionTimeHandler)
        sessionTmr.Start()
    End Sub

    ''' <summary>
    ''' Get and set the session status
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SessionStatus As Status
        Get
            Return _sessionStatus
        End Get
        Set(value As Status)
            If _sessionStatus <> value Then
                _sessionStatus = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Timer event handler to ping the session
    ''' </summary>
    Private Sub sessionTimeHandler(ByVal sender As Object, ByVal e As ElapsedEventArgs)
        ' Only ping is there is an active session
        If isSession Then
            Me.Ping()
        End If
    End Sub

    ''' <summary>
    ''' Opens a game session for a particular user. 
    ''' Allows you to tell Game Jolt that a user is playing your game
    ''' You can only have one session open at a time, if you attempt to open another it will close the current one
    ''' </summary>
    ''' <remarks></remarks>
    Public Async Sub Open()
        Dim strOpen As String
        Dim signature As String

        strOpen = url + "/sessions/open/?" + "format=xml" + "&game_id=" + CStr(GameJolt.GameID) _
            + "&username=" + GameJolt.Username + "&user_token=" + GameJolt.Token

        signature = GameJolt.getSignature(strOpen + GameJolt.PrivateKey)
        strOpen += "&signature=" + signature

        Await GameJolt.GJRequest(strOpen)

        isSession = True
        SessionStatus = Status.Active
    End Sub

    ''' <summary>
    ''' Pings an open session to tell the system that it's still active. 
    ''' If the session hasn't been pinged within 120 seconds, the system will close
    ''' </summary>
    ''' <remarks></remarks>
    Public Async Sub Ping()
        Dim strPing As String
        Dim signature As String

        strPing = url + "/sessions/ping/?" + "format=xml" + "&game_id=" + CStr(GameJolt.GameID) _
            + "&username=" + GameJolt.Username + "&user_token=" + GameJolt.Token

        If SessionStatus = Status.Active Then
            strPing += "&status=active"
        Else
            strPing += "&status=idle"
        End If

        signature = GameJolt.getSignature(strPing + GameJolt.PrivateKey)

        strPing += "&signature=" + signature

        Await GameJolt.GJRequest(strPing)

    End Sub

    ''' <summary>
    ''' Closes the active session
    ''' </summary>
    ''' <remarks></remarks>
    Public Async Sub Close()
        Dim strClose As String
        Dim signature As String

        strClose = url + "/sessions/close/?" + "format=xml" + "&game_id=" + CStr(GameJolt.GameID) _
            + "&username=" + GameJolt.Username + "&user_token=" + GameJolt.Token

        signature = GameJolt.getSignature(strClose + GameJolt.PrivateKey)

        strClose += "&signature=" + signature

        sessionTmr.Stop()

        Await GameJolt.GJRequest(strClose)
        isSession = False
    End Sub
End Class
