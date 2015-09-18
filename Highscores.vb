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

''' <summary>
''' Manages user related API requests. 
''' For reference on user calls see http://gamejolt.com/api/doc/game/scores
''' </summary>
''' <remarks></remarks>
Public Class HighscoresManager
    Inherits GameJolt

    Private highscoreResponses As New List(Of HighscoreResponse)
    Private _lastResponse As HighscoreResponse

    Private _guestname As String

    Sub New()
        _guestname = ""
    End Sub

    ''' <summary>
    ''' Get and set the guest's username, when not logged in
    ''' </summary>
    Public Property GuestName() As String
        Get
            Return _guestname
        End Get
        Set(value As String)
            If _guestname <> value Then
                _guestname = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets the list of all deserialized responses returned from API
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Responses() As List(Of HighscoreResponse)
        Get
            Return highscoreResponses
        End Get
    End Property

    ''' <summary>
    ''' Returns the last response object created from last user API call
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property LastHighscoreResponse() As HighscoreResponse
        Get
            Return _lastResponse
        End Get
    End Property

    ''' <summary>
    ''' Returns a list of scores either for a user or globally for a game.
    ''' </summary>
    ''' <param name="tableid"></param>
    ''' <param name="limit"></param>
    ''' <remarks></remarks>
    Public Async Function fetchScores(user As Boolean, Optional tableid As UInteger = Nothing, Optional limit As Integer = Nothing,
                                      Optional format As Formats = Formats.Xml) As Task(Of MemoryStream)
        Dim strFetchScores As String
        Dim signature As String
        Dim result As MemoryStream

        strFetchScores = url + "/scores/?format=" + strFormat(format) + "&game_id=" + CStr(GameJolt.GameID)

        ' If fetching own scores
        If user Then
            strFetchScores += +"&username=" + GameJolt.Username + "&user_token=" + GameJolt.Token
        End If

        ' Fetch from a specific table
        If tableid <> Nothing Then
            strFetchScores += "&table_id=" + CStr(tableid)
        End If

        ' Limit the number of scores
        If limit <> Nothing Then
            ' Make sure the limit doesn't exceed 100 (API Maximum limit)
            If limit > 100 Then
                limit = 100
            End If
            strFetchScores += "&limit=" + CStr(limit)
        End If
        signature = GameJolt.getSignature(strFetchScores + PrivateKey)
        strFetchScores += "&signature=" + signature

        result = Await GameJolt.GJRequest(strFetchScores)

        _lastResponse = HighscoreResponseStatus(result)

        Return result
    End Function

    ''' <summary>
    ''' Adds a score for a user or guest.
    ''' </summary>
    ''' <remarks></remarks>
    Public Async Function addScore(score As String, sort As String, Optional guest As Boolean = False, Optional extra As String = "", _
        Optional tableid As UInteger = Nothing, Optional format As Formats = Formats.Xml) As Task(Of MemoryStream)

        Dim strAddScores As String
        Dim signature As String
        Dim result As MemoryStream

        strAddScores = url + "/scores/add/?" + "format=" + strFormat(format) + "&game_id=" + CStr(GameJolt.GameID) _
            + "&score=" + score + "&sort=" + sort

        ' Send as user if not guest, otherwise use set guest name
        If Not guest Then
            strAddScores += "&username=" + GameJolt.Username + "&user_token=" + GameJolt.Token
        Else
            strAddScores += "&guest=" + GuestName
        End If

        ' Set extra data if any
        If extra <> "" Then
            strAddScores += "&extra_data=" + extra
        End If

        ' Send score to specific table, if not send to primary
        If tableid <> Nothing Then
            strAddScores += "&table_id=" + CStr(tableid)
        End If

        signature = GameJolt.getSignature(strAddScores + PrivateKey)
        strAddScores += "&signature=" + signature

        result = Await GameJolt.GJRequest(strAddScores)

        _lastResponse = HighscoreResponseStatus(result)

        Return result
    End Function

    ''' <summary>
    ''' Returns a list of high score tables added for the game.
    ''' </summary>
    ''' <remarks></remarks>
    Public Async Function fetchTables(Optional format As Formats = Formats.Xml) As Task(Of MemoryStream)
        Dim strFetchTables As String
        Dim signature As String
        Dim result As MemoryStream

        strFetchTables = url + "/scores/tables/?" + "format=" + strFormat(format) + "&game_id=" + CStr(GameJolt.GameID)

        signature = GameJolt.getSignature(strFetchTables + PrivateKey)
        strFetchTables += "&signature=" + signature

        result = Await GameJolt.GJRequest(strFetchTables)
        _lastResponse = HighscoreResponseStatus(result)

        Return result
    End Function

    ''' <summary>
    ''' Deserializes score API responses and stores returned fields into a response object
    ''' </summary>
    ''' <param name="response"></param>
    ''' <remarks></remarks>
    ''' <returns>Deserialized API response data</returns>
    Public Function HighscoreResponseStatus(response As MemoryStream) As HighscoreResponse
        Dim uResponse As New HighscoreResponse
        Dim res() As Byte = response.ToArray()
        Dim sw As New StringReader(System.Text.Encoding.UTF8.GetString(res))
        ' Attempt to deserialize the output from returned format
        Try
            Dim xml As New XmlSerializer(GetType(HighscoreResponse))
            uResponse = xml.Deserialize(sw)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
        highscoreResponses.Add(uResponse)

        Return uResponse
    End Function
End Class

''' <summary>
''' Stores all score responses returned from the score API calls
''' </summary>
''' <remarks></remarks>
<Serializable()> _
<XmlRoot("response")>
Public Class HighscoreResponse
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

    <XmlElementAttribute(ElementName:="scores")> _
    Public Property Highscores As Scores

    <XmlElementAttribute(ElementName:="tables")> _
    Public Property Tables As Tables
End Class

''' <summary>
''' Stores the list of score items returned from fetch call
''' </summary>
''' <remarks></remarks>
Public Class Scores
    <XmlElementAttribute(ElementName:="score")> _
    Public Property Score As List(Of Score)
End Class

''' <summary>
''' Stores the list of tables returned from fetch call
''' </summary>
''' <remarks></remarks>
Public Class Tables
    <XmlElementAttribute(ElementName:="table")> _
    Public Property Table As List(Of Table)
End Class

''' <summary>
''' Data returned about a highscore is stored in an object from this type
''' Game Jolt supports multiple online high score tables per game. With this you are able to, for example, have a score board for each level in your game.
''' </summary>
''' <remarks></remarks>
<Serializable()> _
Public Class Score
    ''' <summary>
    ''' The score string
    ''' </summary>
    <XmlElementAttribute("score")>
    Public Property score As String

    ''' <summary>
    ''' The score's numerical sort value
    ''' </summary>
    <XmlElementAttribute(ElementName:="sort")> _
    Public Property sort As String

    ''' <summary>
    ''' Any extra data associated with the score
    ''' </summary>
    <XmlElementAttribute(ElementName:="extra_data")> _
    Public Property extraData As String

    ''' <summary>
    ''' If this is a user score, this is the display name for the user
    ''' </summary>
    <XmlElementAttribute(ElementName:="user")> _
    Public Property userScoreName As String

    ''' <summary>
    ''' If this is a user score, this is the user's ID
    ''' </summary>
    <XmlElementAttribute(ElementName:="user_id")> _
    Public Property userScoreID As String

    ''' <summary>
    ''' If this is a guest score, this is the guest's submitted name
    ''' </summary>
    <XmlElementAttribute(ElementName:="guest")> _
    Public Property guest As String

    ''' <summary>
    ''' Returns when the score was logged by the user
    ''' </summary>
    <XmlElementAttribute(ElementName:="stored")> _
    Public Property stored As String
End Class

''' <summary>
''' Data returned about a highscore table is stored in an object from this type
''' </summary>
''' <remarks></remarks>
<Serializable()> _
Public Class Table
    ''' <summary>
    ''' The high score table identifier
    ''' </summary>
    <XmlElementAttribute(ElementName:="id")> _
    Public Property tableID As String

    ''' <summary>
    ''' The developer-defined high score table name
    ''' </summary>
    <XmlElementAttribute(ElementName:="name")> _
    Public Property name As String

    ''' <summary>
    ''' The developer-defined high score table description
    ''' </summary>
    <XmlElementAttribute(ElementName:="description")> _
    Public Property tableDescription As String

    ''' <summary>
    ''' Whether or not this is the default high score table. High scores are submitted to the primary table by default
    ''' </summary>
    <XmlElementAttribute(ElementName:="primary")> _
    Public Property primary As String
End Class
