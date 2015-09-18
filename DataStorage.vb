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
''' Manages data store related API requests. 
''' For reference on user calls see http://gamejolt.com/api/doc/game/data-store
''' </summary>
''' <remarks></remarks>
Public Class DataStorageManager
    Inherits GameJolt

    Private dataStoreResponses As New List(Of DataStoreResponse)

    Private _lastResponses As DataStoreResponse

    ''' <summary>
    ''' Mathematical and string operations for the update API call
    ''' </summary>
    ''' <remarks></remarks>
    Enum Operations
        Add
        Subtract
        Multiply
        Divide
        Append
        Prepend
    End Enum

    ''' <summary>
    ''' Gets the list of all deserialized responses returned from API
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Responses As List(Of DataStoreResponse)
        Get
            Return dataStoreResponses
        End Get
    End Property

    ''' <summary>
    ''' Returns the last response object created from last data store API call
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property LastResponse As DataStoreResponse
        Get
            Return _lastResponses
        End Get
    End Property

    ''' <summary>
    ''' Returns data from the Data Store. Specify the key of the data item you'd like to fetch
    ''' If user is set to true then this fetch user keys, otherwise global data is fetched 
    ''' </summary>
    ''' <remarks></remarks>
    Public Async Function fetchData(datakey As String, Optional user As Boolean = False,
                                    Optional format As Formats = Formats.Dump) As Task(Of MemoryStream)
        Dim strFetchData As String
        Dim signature As String
        Dim result As MemoryStream

        ' Dump format is recommended for this call
        strFetchData = url + "/data-store/?format=" + strFormat(format) + "&game_id=" + CStr(GameJolt.GameID) _
            + "&key=" + datakey

        ' Fetch user keys only
        If user Then
            strFetchData += "&username=" + GameJolt.Username + "&user_token=" + GameJolt.Token
        End If

        signature = GameJolt.getSignature(strFetchData + PrivateKey)
        strFetchData += "&signature=" + signature

        result = Await GameJolt.GJRequest(strFetchData)

        _lastResponses = DataStoreResponseStatus(result)

        Return result
    End Function

    ''' <summary>
    ''' Sets data in the Data Store
    ''' </summary>
    ''' <remarks></remarks>
    Public Async Function setData(dataKey As String, data As String, Optional user As Boolean = False,
                                  Optional format As Formats = Formats.Xml) As Task(Of MemoryStream)
        Dim strSetData As String
        Dim signature As String
        Dim result As MemoryStream

        strSetData = url + "/data-store/set/?format=" + strFormat(format) + "&game_id=" + CStr(GameJolt.GameID) _
            + "&key=" + dataKey + "&data=" + data

        ' Set user data only
        If user Then
            strSetData += "&username=" + GameJolt.Username + "&user_token=" + GameJolt.Token
        End If

        signature = GameJolt.getSignature(strSetData + PrivateKey)
        strSetData += "&signature=" + signature

        result = Await GameJolt.GJRequest(strSetData)

        _lastResponses = DataStoreResponseStatus(result)

        Return result
    End Function

    ''' <summary>
    ''' Updates data in the Data Store. Mathematical operations can be performed which are add,subtract,multiply and divide.
    ''' String operations can be performed with append and prepend
    ''' </summary>
    ''' <remarks></remarks>
    Public Async Function updateData(datakey As String, operation As Operations, dataval As String,
                                     Optional user As Boolean = False, Optional format As Formats = Formats.Xml) As Task(Of MemoryStream)
        Dim strUpdateData As String
        Dim signature As String
        Dim result As MemoryStream

        strUpdateData = url + "/data-store/update/?format=" + strFormat(format) + "&game_id=" + CStr(GameJolt.GameID) _
            + "&key=" + datakey

        ' Select operation
        Select Case operation
            Case 0
                strUpdateData += "&operation=add"
            Case 1
                strUpdateData += "&operation=subtract"
            Case 2
                strUpdateData += "&operation=multiply"
            Case 3
                strUpdateData += "&operation=divide"
            Case 4
                strUpdateData += "&operation=append"
            Case 5
                strUpdateData += "&operation=prepend"
        End Select
        strUpdateData += "&value=" + dataval

        ' Update user keys only
        If user Then
            strUpdateData += "&username=" + GameJolt.Username + "&user_token=" + GameJolt.Token
        End If

        signature = GameJolt.getSignature(strUpdateData + PrivateKey)
        strUpdateData += "&signature=" + signature

        result = Await GameJolt.GJRequest(strUpdateData)

        _lastResponses = DataStoreResponseStatus(result)

        Return result
    End Function

    ''' <summary>
    ''' Removes data from the Data Store
    ''' </summary>
    ''' <remarks></remarks>
    Public Async Function removeData(datakey As String, Optional user As Boolean = False,
                                     Optional formats As Formats = Formats.Xml) As Task(Of MemoryStream)
        Dim strRemoveData As String
        Dim signature As String
        Dim result As MemoryStream

        strRemoveData = url + "/data-store/remove/?format=" + strFormat(Format) + "&game_id=" + CStr(GameJolt.GameID) _
            + "&key=" + datakey

        ' Remove from user keys
        If user Then
            strRemoveData += "&username=" + GameJolt.Username + "&user_token=" + GameJolt.Token
        End If

        signature = GameJolt.getSignature(strRemoveData + PrivateKey)
        strRemoveData += "&signature=" + signature

        result = Await GameJolt.GJRequest(strRemoveData)

        _lastResponses = DataStoreResponseStatus(result)

        Return result
    End Function

    ''' <summary>
    ''' Returns all the keys in either the game's global data store, or all the keys in a user's data store
    ''' </summary>
    ''' <param name="user"></param>
    ''' <remarks></remarks>
    Public Async Function getKeys(Optional user As Boolean = False, Optional format As Formats = Formats.Xml) As Task(Of MemoryStream)
        Dim strGetKeysData As String
        Dim signature As String
        Dim result As MemoryStream

        strGetKeysData = url + "/data-store/get-keys/?format=" + strFormat(format) + "&game_id=" + CStr(GameJolt.GameID)

        ' Remove from user keys
        If user Then
            strGetKeysData += "&username=" + GameJolt.Username + "&user_token=" + GameJolt.Token
        End If

        signature = GameJolt.getSignature(strGetKeysData + PrivateKey)
        strGetKeysData += "&signature=" + signature

        result = Await GameJolt.GJRequest(strGetKeysData)

        _lastResponses = DataStoreResponseStatus(result)

        Return result
    End Function

    ''' <summary>
    ''' Deserializes data store call responses and stores returned fields
    ''' </summary>
    ''' <param name="response"></param>
    ''' <remarks></remarks>
    Public Function DataStoreResponseStatus(response As MemoryStream) As DataStoreResponse
        Dim uResponse As New DataStoreResponse
        Dim res() As Byte = response.ToArray()
        Dim sw As New StringReader(System.Text.Encoding.UTF8.GetString(res))

        ' Attempt to deserialize the output from returned format
        Try
            Dim xml As New XmlSerializer(GetType(DataStoreResponse))
            uResponse = xml.Deserialize(sw)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
        dataStoreResponses.Add(uResponse)

        Return uResponse
    End Function
End Class

''' <summary>
''' Stores all data store response output returned from data store API calls
''' </summary>
''' <remarks></remarks>
<Serializable()> _
<XmlRoot("response")>
Public Class DataStoreResponse
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
    ''' List of fetched keys
    ''' </summary>
    <XmlElementAttribute(ElementName:="keys")> _
    Public Property keys As DataStoreKeys
End Class

''' <summary>
''' Stores a list of fetched keys from the data store
''' </summary>
''' <remarks></remarks>
Public Class DataStoreKeys
    ' List of fetched keys 
    <XmlElementAttribute(ElementName:="key")>
    Public Property key As List(Of String)
End Class


