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
Imports System.Diagnostics
Imports System.Text
Imports System.Security.Cryptography
Imports System.Threading.Tasks
Imports System.Net
Imports System.Net.Http
Imports System.IO
Imports System.Xml.Serialization
Imports System.Web.Script.Serialization
Imports System.ComponentModel
Imports System.Runtime.CompilerServices

''' <summary>
''' Sets up main Game Jolt settings required by the Game Jolt API
''' For more info see http://gamejolt.com/api/doc/game
''' </summary>
''' <remarks></remarks>
Public Class GameJolt
    Protected url As String = "http://gamejolt.com/api/game/v1"

    Private Shared _privateKey As String
    Private Shared _ID As Integer
    Private Shared _username As String
    Private Shared _token As String

    Private Shared _format As Formats

    ' Events
    Public Shared Event usernameChanged As PropertyChangedEventHandler
    Public Shared Event tokenChanged As PropertyChangedEventHandler

    ''' <summary>
    ''' The API can return data in the following formats
    ''' Keypair = 0, Json = 1, Xml = 2, Dump = 3
    ''' </summary>
    ''' <remarks></remarks>
    Enum Formats
        keypair
        Json
        Xml
        Dump
    End Enum

    ''' <summary>
    ''' Returns the set format as a string
    ''' </summary>
    ''' <param name="formatStr"></param>
    ''' <returns>Returns the format string to be passed to URL parameters</returns>
    ''' <remarks></remarks>
    Public Shared Function strFormat(ByVal formatStr As Formats) As String
        Dim formatRet As String
        formatRet = ""

        Select Case formatStr
            Case Formats.keypair
                formatRet = "keypair"
            Case Formats.Json
                formatRet = "json"
            Case Formats.Xml
                formatRet = "xml"
            Case Formats.Dump
                formatRet = "dump"
        End Select

        Return formatRet
    End Function

    ' -------------------------
    ' -- Getters and setters --
    ' -------------------------

    ''' <summary>
    ''' Get and set the private key for appending to api calls
    ''' </summary>
    Public Shared Property PrivateKey As String
        Get
            Return _privateKey
        End Get
        Set(value As String)
            If _privateKey <> value Then
                _privateKey = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Get and set the game ID for your game
    ''' </summary>
    Public Shared Property GameID As Integer
        Get
            Return _ID
        End Get
        Set(value As Integer)
            If _ID <> value Then
                _ID = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Get and set the user's username
    ''' </summary>
    ''' <value></value>
    ''' <returns>Returns the user's username</returns>
    ''' <remarks></remarks>
    Public Shared Property Username As String
        Get
            Return _username
        End Get
        Set(value As String)
            If _username <> value Then
                _username = value
                RaiseEvent usernameChanged(Nothing, New PropertyChangedEventArgs("Username"))
            End If
        End Set
    End Property

    ''' <summary>
    ''' Get and set the user's token
    ''' </summary>
    ''' <value></value>
    ''' <returns>Returns the user's token</returns>
    ''' <remarks></remarks>
    Public Shared Property Token As String
        Get
            Return _token
        End Get
        Set(value As String)
            If _token <> value Then
                _token = value
                RaiseEvent tokenChanged(Nothing, New PropertyChangedEventArgs("Token"))
            End If
        End Set
    End Property

    ''' <summary>
    ''' Get and set the data format for API responses
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Property Format As Formats
        Get
            Return _format
        End Get
        Set(value As Formats)
            _format = value
        End Set
    End Property

    ''' <summary>
    ''' For generating a hash signature digest for authenticating API calls
    ''' </summary>
    ''' <param name="url">The next API call to be appended as call signature</param>
    Public Shared Function getSignature(ByVal url As String) As String
        Dim gj_md5 As MD5 = MD5.Create()
        Dim dataHash As Byte() = gj_md5.ComputeHash(Encoding.UTF8.GetBytes(url))

        Dim strBuild As New StringBuilder()

        For i As Integer = 0 To dataHash.Length - 1
            strBuild.Append(dataHash(i).ToString("x2"))
        Next

        Return strBuild.ToString()
    End Function

    ''' <summary>
    ''' Sends off a new web request to the API and gets the responses back
    ''' </summary>
    ''' <param name="request_url">The next API call</param>
    ''' <returns>Returns the API response</returns>
    ''' <remarks></remarks>
    Public Shared Async Function GJRequest(ByVal request_url As String) As Task(Of MemoryStream)
        Dim content = New MemoryStream()
        ' Send next call to API and get responses
        Try
            Dim handleRequest = CType(WebRequest.Create(request_url), HttpWebRequest)
            Using response As WebResponse = Await handleRequest.GetResponseAsync()
                Using stream As Stream = response.GetResponseStream()
                    Await stream.CopyToAsync(content)
                End Using
            End Using
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
        ' Return the response data
        Return content
    End Function

End Class
