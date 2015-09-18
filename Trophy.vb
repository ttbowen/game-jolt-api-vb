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
''' Manages trophy related API requests. 
''' For reference on user calls see http://gamejolt.com/api/doc/game/trophies
''' </summary>
''' <remarks></remarks>
Public Class TrophyManager
    Inherits GameJolt

    Private trophyResponses As New List(Of TrophyResponse)
    Private _lastResponse As TrophyResponse

    ''' <summary>
    ''' Gets the list of all deserialized responses returned from API
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Responses As List(Of TrophyResponse)
        Get
            Return trophyResponses
        End Get
    End Property

    ''' <summary>
    ''' Returns the last response object created from last trophy API call
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property LastResponse As TrophyResponse
        Get
            Return _lastResponse
        End Get
    End Property

    ''' <summary>
    ''' For fetching a single trophy from a game. Pass the ID of the trophy to return it
    ''' Pass True to only return achieved trophies. Leave "achieved" and "id" blank to return all trophies.
    ''' If "id" is specified then "achieved" will be ignored
    ''' </summary>
    ''' <remarks></remarks>
    Public Async Function fetchTrophy(Optional achieved As Boolean = Nothing, Optional id As UInteger = Nothing,
                                      Optional format As Formats = Formats.Xml) As Task(Of MemoryStream)
        Dim strTrophyFetch As String
        Dim signature As String
        Dim result As MemoryStream

        strTrophyFetch = url + "/trophies/?" + "format=" + strFormat(format) + "&game_id=" + CStr(GameJolt.GameID) _
            + "&username=" + GameJolt.Username + "&user_token=" + GameJolt.Token

        If achieved Then
            strTrophyFetch += "&achieved=true"
        ElseIf achieved = False Then
            strTrophyFetch += "&achieved=false"
        End If

        If id <> Nothing Then
            strTrophyFetch += "&trophy_id=" + CStr(id)
        End If

        signature = GameJolt.getSignature(strTrophyFetch + GameJolt.PrivateKey)

        strTrophyFetch += "&signature=" + signature

        result = Await GameJolt.GJRequest(strTrophyFetch)

        _lastResponse = TrophyResponseStatus(result)

        Return result
    End Function

    ''' <summary>
    ''' For fetching a list of trophies. Pass in an integer list to return the trophies  
    ''' </summary>
    ''' <remarks></remarks>
    Public Async Function fetchTrophy(trophyList As List(Of Integer), Optional format As Formats = Formats.Xml) As Task(Of MemoryStream)
        Dim strFetchTrophyList As String
        Dim signature As String
        Dim result As MemoryStream

        strFetchTrophyList = url + "/trophies/?format=" + strFormat(format) + "&game_id=" + CStr(GameJolt.GameID) _
            + "&username=" + GameJolt.Username + "&user_token=" + GameJolt.Token

        ' Separate each value with comma for URL
        For Each t In trophyList
            strFetchTrophyList += t.ToString()
            strFetchTrophyList += ","
        Next
        strFetchTrophyList = strFetchTrophyList.Substring(0, strFetchTrophyList.Length - 1)

        signature = GameJolt.getSignature(strFetchTrophyList + GameJolt.PrivateKey)
        strFetchTrophyList += "&signature=" + signature

        result = Await GameJolt.GJRequest(strFetchTrophyList)

        _lastResponse = TrophyResponseStatus(result)

        Return result
    End Function

    ''' <summary>
    ''' Achieve a game trophy with the specified trophy ID
    ''' </summary>
    ''' <remarks></remarks>
    Public Async Function achieveTrophy(id As UInteger, Optional formats As Formats = Formats.Xml) As Task(Of MemoryStream)
        Dim strTrophyAchieve As String
        Dim signature As String
        Dim result As MemoryStream

        strTrophyAchieve = url + "/trophies/add-achieved/?formatl" + strFormat(Format) + "&game_id=" + CStr(GameJolt.GameID) _
            + "&username=" + GameJolt.Username + "&user_token=" + GameJolt.Token + "&trophy_id=" + CStr(id)

        signature = GameJolt.getSignature(strTrophyAchieve + GameJolt.PrivateKey)

        strTrophyAchieve += "&signature=" + signature

        result = Await GameJolt.GJRequest(strTrophyAchieve)

        _lastResponse = TrophyResponseStatus(result)

        Return result
    End Function

    ''' <summary>
    ''' Deserializes score API responses and stores returned fields into a response object
    ''' </summary>
    ''' <param name="response"></param>
    ''' <remarks></remarks>
    ''' <returns>Deserialized API response data</returns>
    Public Function TrophyResponseStatus(response As MemoryStream) As TrophyResponse
        Dim uResponse As New TrophyResponse
        Dim res() As Byte = response.ToArray()
        Dim sw As New StringReader(System.Text.Encoding.UTF8.GetString(res))
        ' Attempt to deserialize the output from returned format
        Try
            Dim xml As New XmlSerializer(GetType(TrophyResponse))
            uResponse = xml.Deserialize(sw)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
        TrophyResponses.Add(uResponse)

        Return uResponse
    End Function
End Class

''' <summary>
''' Stores all trophy responses returned from the score API calls
''' </summary>
''' <remarks></remarks>
<Serializable()> _
<XmlRoot("response")>
Public Class TrophyResponse
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

    <XmlElementAttribute(ElementName:="trophies")> _
    Public Property Trophies As Trophies
End Class

''' <summary>
''' Stores the list of trophy items returned from fetch call
''' </summary>
''' <remarks></remarks>
Public Class Trophies
    <XmlElementAttribute(ElementName:="trophy")> _
    Public Property Trophy As List(Of Trophy)
End Class

''' <summary>
''' Data returned about a trophy is stored in an object from this type
''' Trophies come in four materials: Bronze, Silver, Gold and Platinum. This is to reflect how difficult it is to achieve a trophy.
''' </summary>
''' <remarks></remarks>
<Serializable()> _
Public Class Trophy
    ''' <summary>
    ''' The ID of the trophy
    ''' </summary>
    <XmlElementAttribute(ElementName:="id")> _
    Public Property trophyID As String

    ''' <summary>
    ''' The title of the trophy on the site
    ''' </summary>
    <XmlElementAttribute(ElementName:="title")> _
    Public Property title As String

    ''' <summary>
    ''' The trophy description text
    ''' </summary>
    <XmlElementAttribute(ElementName:="description")> _
    Public Property trophyDescription As String

    ''' <summary>
    ''' Level of difficulty "Bronze", "Silver", "Gold" or "Platinum"
    ''' </summary>
    <XmlElementAttribute(ElementName:="difficulty")> _
    Public Property difficulty As String

    ''' <summary>
    ''' The URL to the trophy's thumbnail
    ''' </summary>
    <XmlElementAttribute(ElementName:="image_url")> _
    Public Property imageURL As String

    ''' <summary>
    ''' Returns when the trophy was achieved by the user, or "false" if they haven't achieved it yet
    ''' </summary>
    <XmlElementAttribute(ElementName:="achieved")> _
    Public Property achieved As String

End Class
