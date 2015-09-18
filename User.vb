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
Imports System.Runtime.Serialization
Imports System.ComponentModel

''' <summary>
''' Manages user related API requests. 
''' For reference on user calls see http://gamejolt.com/api/doc/game/users
''' </summary>
''' <remarks></remarks>
Public Class UserManager
    Inherits GameJolt

    Private _isLoggedin As Boolean
    Private userResponses As New List(Of UserResponse)
    Private _lastResponse As UserResponse

    Public session As SessionManager

    ''' <summary>
    ''' ' The current user types
    ''' </summary>
    ''' <remarks></remarks>
    Enum Types
        User
        Developer
        Moderator
        Administrator
    End Enum

    ''' <summary>
    ''' The user status types
    ''' </summary>
    ''' <remarks></remarks>
    Enum Status
        Active
        Banned
    End Enum

    ''' <summary>
    ''' Gets the current login status
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property LoggedIn As Boolean
        Get
            Return _isLoggedin
        End Get
    End Property

    ''' <summary>
    ''' Gets the list of all deserialized responses returned from API
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Responses As List(Of UserResponse)
        Get
            Return userResponses
        End Get
    End Property

    ''' <summary>
    ''' Returns the last response object created from last user API call
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property LastUserResponse As UserResponse
        Get
            Return _lastResponse
        End Get
    End Property

    ''' <summary>
    ''' Authenticates the user's information and starts a new session. 
    ''' This should be done before you make any calls for the user, to make sure that the user's credentials (username/token) are valid.
    ''' </summary>
    ''' <remarks></remarks>
    Public Async Function AuthenticateUser(Optional format As Formats = Formats.Xml) As Task(Of MemoryStream)
        Dim authUrl As String
        Dim signature As String
        Dim result As MemoryStream

        authUrl = url + "/users/auth/?format=" + strFormat(format) + "&game_id=" + CStr(GameJolt.GameID) _
            + "&username=" + CStr(GameJolt.Username) + "&user_token=" + CStr(GameJolt.Token)

        signature = GameJolt.getSignature(authUrl + PrivateKey)
        authUrl += "&signature=" + signature

        result = Await GameJolt.GJRequest(authUrl)

        _lastResponse = UserResponseStatus(result)

        ' If the user was successfully authenticated
        If (LastUserResponse.Success = "true") Then
            ' Open a new session
            session = New SessionManager()
            _isLoggedin = True
        End If

        Return result
    End Function

    ''' <summary>
    ''' Outputs user data
    ''' Fetch a list of users, pass an integer list of the user IDs
    ''' </summary>
    ''' <remarks></remarks>
    Public Async Function FetchUsers(users As List(Of Integer), Optional format As Formats = Formats.Xml) As Task(Of MemoryStream)
        Dim strUsers As String
        Dim signature As String
        Dim result As MemoryStream

        strUsers = url + "/users/fetch/?" + "format=" + strFormat(format) + "&game_id=" + CStr(GameJolt.GameID) + "&user_id="

        ' Separate each value with comma for URL
        For Each u In users
            strUsers += u.ToString()
            strUsers += ","
        Next
        strUsers = strUsers.Substring(0, strUsers.Length - 1)

        signature = GameJolt.getSignature(strUsers + GameJolt.PrivateKey)
        strUsers += "&signature=" + signature

        result = Await GameJolt.GJRequest(strUsers)

        _lastResponse = UserResponseStatus(result)

        Return result
    End Function

    ''' <summary>
    ''' Outputs user data
    ''' Fetch a single user, pass user ID 
    ''' </summary>
    ''' <remarks></remarks>
    Public Async Function FetchUsers(user As Integer, Optional format As Formats = Formats.Xml) As Task(Of MemoryStream)
        Dim strUsers As String
        Dim signature As String
        Dim result As MemoryStream

        strUsers = url + "/users/?format=" + strFormat(format) + "&game_id=" + CStr(GameJolt.GameID) _
            + "&user_id=" + CStr(user)

        signature = GameJolt.getSignature(strUsers + GameJolt.PrivateKey)
        strUsers += "&signature=" + signature

        result = Await GameJolt.GJRequest(strUsers)

        _lastResponse = UserResponseStatus(result)

        Return result
    End Function

    ''' <summary>
    ''' Outputs user data
    ''' Fetch a single user, pass username
    ''' </summary>
    ''' <remarks></remarks>
    Public Async Function FetchUsers(user As String, Optional format As Formats = Formats.Xml) As Task(Of MemoryStream)
        Dim strUsers As String
        Dim signature As String
        Dim result As MemoryStream

        strUsers = url + "/users/?format=" + strFormat(format) + "&game_id=" + CStr(GameJolt.GameID) _
            + "&username=" + user

        signature = GameJolt.getSignature(strUsers + GameJolt.PrivateKey)
        strUsers += "&signature=" + signature

        result = Await GameJolt.GJRequest(strUsers)

        _lastResponse = UserResponseStatus(result)

        Return result
    End Function

    ''' <summary>
    ''' Deserializes user call responses and stores returned fields
    ''' </summary>
    ''' <param name="response"></param>
    ''' <remarks></remarks>
    Public Function UserResponseStatus(response As MemoryStream) As UserResponse
        Dim uResponse As New UserResponse
        Dim res() As Byte = response.ToArray()
        Dim sw As New StringReader(System.Text.Encoding.UTF8.GetString(res))
        ' Attempt to deserialize the output from one of the returned API formats
        Try
            ' Deserialize as XML
            Dim xml As New XmlSerializer(GetType(UserResponse))
            uResponse = xml.Deserialize(sw)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
        userResponses.Add(uResponse)

        Return uResponse
    End Function

    ''' <summary>
    ''' Logout the user from game
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub GJLogout()
        GameJolt.Username = ""
        GameJolt.Token = ""
        session.Close()
    End Sub

End Class

''' <summary>
''' Stores all user response output returned from user API calls
''' </summary>
''' <remarks></remarks>
<Serializable()> _
<XmlRoot("response")> _
Public Class UserResponse
    ''' <summary>
    ''' The returned response
    ''' </summary>
    <XmlElementAttribute(ElementName:="success")> _
    Public Property Success() As String

    ''' <summary>
    ''' The returned message
    ''' </summary>
    <XmlElementAttribute(ElementName:="message")> _
    Public Property Message() As String

    ''' <summary>
    ''' List of fetched users
    ''' </summary>
    <XmlElementAttribute(ElementName:="users")> _
    Public Property Users As Users
End Class

''' <summary>
''' Stores the list of user items returned from fetch call
''' </summary>
''' <remarks></remarks>
Public Class Users
    ' List of fetched users
    <XmlElementAttribute(ElementName:="user")> _
    Public Property User As List(Of User)
End Class

''' <summary>
''' Represents an individual user and stores all returned fields from fetch requests
''' </summary>
''' <remarks></remarks>
<Serializable()> _
Public Class User
    ''' <summary>
    ''' The ID of the user
    ''' </summary>
    <XmlElementAttribute(ElementName:="id")> _
    Public Property userID() As String

    ''' <summary>
    ''' The user's type
    ''' </summary>
    <XmlElementAttribute(ElementName:="type")> _
    Public Property type As String

    ''' <summary>
    ''' The user's username
    ''' </summary>
    <XmlElementAttribute(ElementName:="username")> _
    Public Property username As String

    ''' <summary>
    ''' The URL of the user's avatar
    ''' </summary>
    <XmlElementAttribute(ElementName:="avatar_url")> _
    Public Property avatarUrl As String

    ''' <summary>
    ''' How long ago the user signed up
    ''' </summary>
    <XmlElementAttribute(ElementName:="signed_up")> _
    Public Property signedUp As String

    ''' <summary>
    ''' How long ago the user was last logged in. Will be "Online Now" if the user is currently online
    ''' </summary>
    <XmlElementAttribute(ElementName:="last_logged_in")> _
    Public Property lastLoggedIn As String

    ''' <summary>
    ''' "Active" if the user is still a member on the site. "Banned" if they've been banned.
    ''' </summary>
    <XmlElementAttribute(ElementName:="status")> _
    Public Property status As String

    ' **** Developer properties ****

    ''' <summary>
    ''' The developer's name
    ''' </summary>
    <XmlElementAttribute(ElementName:="developer_name")> _
    Public Property developerName As String

    ''' <summary>
    ''' The developer's website
    ''' </summary>
    <XmlElementAttribute(ElementName:="developer_website")> _
    Public Property developerWebsite As String

    ''' <summary>
    ''' The description that the developer put in for themselves
    ''' </summary>
    <XmlElementAttribute(ElementName:="developer_description")> _
    Public Property developerDescription As String
End Class
