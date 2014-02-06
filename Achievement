' Game Jolt API for Visual Basic - For implementing game achievements 

' Copyright (C) 2014  Thomas Bowen

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

' Import libraries 
Imports System
Imports System.Security.Cryptography
Imports System.Text
Imports System.Net
Imports System.IO

Public Class GJ_Achievement
    Private GJ_private_key As String
    Private GJ_gameID As Integer
    Private GJ_UserName As String
    Private GJ_Token As String
    Private GJ_URL As String = "http://gamejolt.com/api/game/v1"
    Private GJ_Signature As String
    Private GJ_isActive As Boolean
    Private GJ_isLoggedIn As Boolean
    Private GJ_userType As String
    Private GJ_achieved As String

    ' *** Constructor ***
    Sub New(ByVal private_key As String, ByVal gameID As Integer) ' Use this to set the private key and the game's ID
        GJ_private_key = private_key
        GJ_gameID = gameID
        GJ_UserName = ""
        GJ_Token = ""
        GJ_isActive = False
        GJ_isLoggedIn = False
        GJ_achieved = False
    End Sub

    ' *** Get the signature ***
    Public Function GetSignature() As String

        Using MD5Hash As MD5 = MD5.Create()

            Dim signature As String = GenerateMD5Sig(MD5Hash, GJ_Signature)
            Return signature

        End Using

    End Function

    ' *** Generate 32 character signature using an MD5 Hash ***
    Public Shared Function GenerateMD5Sig(ByVal md5Hash As MD5, ByVal hash As String) As String

        Dim dataConvert As Byte() = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(hash))

        Dim stringBuilder As New StringBuilder()

        Dim i As Integer

        For i = 0 To dataConvert.Length - 1
            stringBuilder.Append(dataConvert(i).ToString("x2"))
        Next i

        Return stringBuilder.ToString()
    End Function

    ' *** Getters and setters **************************
    ' Main get and set functions are found below 
    ' **************************************************
    ' *** Set the username ***
    Public WriteOnly Property SetUserName() As String
        Set(ByVal value As String)
            GJ_UserName = value
        End Set
    End Property

    ' *** Set the token ***
    Public WriteOnly Property SetToken() As String
        Set(ByVal value As String)
            GJ_Token = value
        End Set
    End Property

    ' *** Get the username ***
    Public ReadOnly Property getUserName() As String
        Get
            Return GJ_UserName
        End Get
    End Property

    ' *** Get the user token ***
    Public ReadOnly Property getToken() As String
        Get
            Return GJ_Token
        End Get
    End Property

    ' *** Get the game ID ***
    Public ReadOnly Property getGameID() As Integer
        Get
            Return GJ_gameID
        End Get
    End Property

    ' *** Users ****************************************
    ' All the functions related to users are found below
    ' **************************************************
    Public Enum user_status ' Define the current user status 
        Active
        Banned
    End Enum

    Public Enum user_type ' Define the user types
        User = 1 ' User = 1, Developer = 2, Moderator = 3, Admin = 4
        Developer
        Moderator
        Admin
    End Enum
    ' Set the user type
    Public Sub setUserType(ByVal user As user_type)
        GJ_userType = "type" & user
    End Sub
    ' Get the user type 
    Public ReadOnly Property getUserType()
        Get
            Return GJ_userType
        End Get
    End Property

    ' *** Get the URL call to fetch users ***
    ' Note: You do not need to call this function, this only returns the URL for "fetchUser"
    Public Function fetchUserURL(ByVal usersID As List(Of String), ByVal userID As Integer)
        Dim user_URL_temp As String
        Dim user_URL As String
        Dim Signature As String
        Dim user_part As String
        Dim usrs As String

        If userID <> 0 And usersID Is Nothing Then
            user_URL_temp = GJ_URL & "/users/?game_id=" & CStr(GJ_gameID) & "&" & "user_id=" & CStr(userID) & GJ_private_key

            GJ_Signature = user_URL_temp

            Signature = GetSignature()

            user_URL = GJ_URL & "/users/?game_id=" & CStr(GJ_gameID) & "&" & "user_id=" & CStr(userID) & "&" & "signature=" _
                & Signature
        Else
            user_URL_temp = GJ_URL & "/users/?game_id=" & CStr(GJ_gameID)
            user_part = "&" & "user_id="
            ' If there is more than one user to be returned
            For Each usrs In usersID
                user_part += usrs & ","
            Next
            usrs = user_part.Substring(0, user_part.Count() - 1)
            user_part += GJ_private_key
            user_URL_temp += user_part
            GJ_Signature = user_URL_temp

            Signature = GetSignature()

            user_URL = GJ_URL & "/users/?game_id=" & CStr(GJ_gameID) & "&" & "user_id=" & userID & "&" & "signature=" _
                & Signature
        End If

        Return user_URL
    End Function

    ' *** Use this function to fetch a user ***
    Public Function fetchUser(ByVal users As List(Of String), ByVal user As Integer)
        Dim handleRequest As WebRequest
        handleRequest = WebRequest.Create(fetchUserURL(users, user)) ' The default values, change if needed

        Dim stream As Stream
        stream = handleRequest.GetResponse.GetResponseStream()

        Dim streamRd As New StreamReader(stream)

        Dim response As String = streamRd.ReadToEnd()

        Return response
    End Function

    ' *** Get the URL call to authenticate the user ***
    ' Note: You do not need to call this function, this only returns the URL for "authenticateUser"
    Public Function authenticateUserURL()
        Dim user_URL_temp As String
        Dim user_URL As String
        Dim Signature As String
        user_URL_temp = GJ_URL & "/users/auth/?game_id=" & CStr(GJ_gameID) & "&" & "username=" & GJ_UserName _
            & "&" & "user_token=" & GJ_Token & GJ_private_key
			
		GJ_Signature = user_URL_temp
		
        Signature = GetSignature()

        user_URL = GJ_URL & "/users/auth/?game_id=" & CStr(GJ_gameID) & "&" & "username=" & GJ_UserName _
            & "&" & "user_token=" & GJ_Token & "&" & "signature=" & Signature

        Return user_URL

    End Function

    ' *** Authenticate the user using GJ credentials ***
    Public Function authenticateUser() As Boolean
        Dim handleRequest As WebRequest
        handleRequest = WebRequest.Create(authenticateUserURL())

        Dim stream As Stream
        stream = handleRequest.GetResponse.GetResponseStream()

        Dim streamRd As New StreamReader(stream)

        Dim response As String = streamRd.ReadToEnd()

        If InStr(response, "success:" & "true") = 1 Then
            openSession()
            Return True
        Else
            MsgBox("Could not authenticate user")
            Return False
        End If

    End Function

    ' *** Logout the user from the current session ***
    Public Sub GJ_Logout()
        GJ_UserName = ""
        GJ_Token = ""
        GJ_isLoggedIn = False
        closeSession()
    End Sub

    ' *** Sessions ************************************
    ' All functions related to sessions are found below
    ' *************************************************

    ' *** Ping the current session ***
    ' Note: The session needs to be pinged within a 120 seconds or the session will automatically close. 30 seconds is recommended
    Public Function pingSession(ByVal status As Boolean)
        Dim session_URL_temp As String
        Dim session_URL As String
        Dim Signature As String
        Dim statusResult As String
        status = GJ_isActive

        If status = True Then
            statusResult = "active"
        Else
            statusResult = "idle"
        End If

        session_URL_temp = GJ_URL & "/sessions/ping/?game_id=" & CStr(GJ_gameID) & "&" & "username=" & GJ_UserName _
            & "&" & "user_token=" & GJ_Token & "&" & "status=" & statusResult & GJ_private_key
        GJ_Signature = session_URL_temp


        Signature = GetSignature()

        session_URL = GJ_URL & "/sessions/ping/?game_id=" & CStr(GJ_gameID) & "&" & "username=" & GJ_UserName _
            & "&" & "user_token=" & GJ_Token & "&" & "status=" & statusResult _
            & "&" & "signature=" & Signature

        ' Send the request and get the response
        Dim handleRequest As WebRequest
        handleRequest = WebRequest.Create(session_URL)

        Dim stream As Stream
        stream = handleRequest.GetResponse.GetResponseStream()

        Dim streamRd As New StreamReader(stream)

        Dim response As String = streamRd.ReadToEnd()

        Return response
    End Function

    ' *** Open the current session ***
    Public Function openSession()
        Dim session_URL_temp As String
        Dim session_URL As String
        Dim Signature As String
        session_URL_temp = GJ_URL & "/sessions/open/?game_id=" & CStr(GJ_gameID) & "&" & "username=" & GJ_UserName _
            & "&" & "user_token=" & GJ_Token & GJ_private_key
        GJ_Signature = session_URL_temp

        Signature = GetSignature()

        session_URL = GJ_URL & "/sessions/open/?game_id=" & CStr(GJ_gameID) & "&" & "username=" & GJ_UserName _
            & "&" & "user_token=" & GJ_Token & "&" & "signature=" & Signature

        ' Send the request and get the response
        Dim handleRequest As WebRequest
        handleRequest = WebRequest.Create(session_URL)

        Dim stream As Stream
        stream = handleRequest.GetResponse.GetResponseStream()

        Dim streamRd As New StreamReader(stream)

        Dim response As String = streamRd.ReadToEnd()

        If InStr(response, "success:" & "true") = 1 Then
            GJ_isActive = True
        Else
            GJ_Logout()
            MsgBox("error" & response)
        End If

        Return response
    End Function

    ' *** Close the current session ***
    Public Function closeSession()
        Dim session_URL_temp As String
        Dim session_URL As String
        Dim Signature As String
        session_URL_temp = GJ_URL & "/sessions/close/?game_id=" & CStr(GJ_gameID) & "&" & "username=" & GJ_UserName _
            & "&" & "user_token=" & GJ_Token & GJ_private_key
        GJ_Signature = session_URL_temp

        Signature = GetSignature()

        session_URL = GJ_URL & "/sessions/close/?game_id=" & CStr(GJ_gameID) & "&" & "username=" & GJ_UserName _
            & "&" & "user_token=" & GJ_Token & "&" & "signature=" & Signature

        ' Send the request and get the response
        Dim handleRequest As WebRequest
        handleRequest = WebRequest.Create(session_URL)

        Dim stream As Stream
        stream = handleRequest.GetResponse.GetResponseStream()

        Dim streamRd As New StreamReader(stream)

        Dim response As String = streamRd.ReadToEnd()

        If InStr(response, "success:" & "true") = 1 Then
            GJ_Logout()
            GJ_isActive = False
        End If
        Return response
    End Function

    ' *** Trophies *************************************
    ' All functions related to trophies are found below
    ' **************************************************
    ' Define trophy levels of difficulty
    Public Enum difficulty
        Bronze = 1 '1 = Gold, 2 = Silver, 3 = Gold, 4 = Platinum 
        Silver
        Gold
        Platinum
    End Enum

    ' This is an overloaded function that returns all achieved trophies, if true is passed to it
    ' If false is passed to it all unachieved trophies will be returned
    ' Note: You do not need to call this function, this only returns the URL for "fetchTrophy"
    Public Function fetchTrophyURL(ByVal achieved As Boolean) As String
        Dim trophy_URL_temp As String
        Dim trophy_URL As String
        Dim Signature As String

        If achieved = True Then
            GJ_achieved = "true"
        Else
            GJ_achieved = "false"
        End If

        trophy_URL_temp = GJ_URL & "/trophies/?game_id=" & CStr(GJ_gameID) & "&" & "username=" & GJ_UserName _
            & "&" & "user_token=" & GJ_Token & "&" & "achieved=" & GJ_achieved & GJ_private_key
        GJ_Signature = trophy_URL_temp

        Signature = GetSignature()

        trophy_URL = GJ_URL & "/trophies/?game_id=" & CStr(GJ_gameID) & "&" & "username=" & GJ_UserName _
            & "&" & "user_token=" & GJ_Token & "&" & "achieved=" & GJ_achieved _
            & "&" & "signature=" & Signature

        Return trophy_URL

    End Function

    ' *** Only use this fetchTrophyURL when you wish to fetch trophies by ID or all trophies ***
    ' To use this function just type the list of trophies you wish to return by ID or leave as nothing to return all trophies
    ' Note: You do not need to call this function, this returns the URL for "fetchTrophy"
    Public Function fetchTrophyURL(ByVal trophyID As List(Of String)) As String
        Dim trophy_URL_temp As String
        Dim trophy_URL As String
        Dim url_part As String
        Dim Signature As String
        Dim tr As String
        ' If there are no arguments, leave optional parameters out 
        If trophyID Is Nothing Then
            trophy_URL_temp = GJ_URL & "/trophies/?game_id=" & CStr(GJ_gameID) & "&" & "username=" & GJ_UserName _
                    & "&" & "user_token=" & GJ_Token & GJ_private_key
            GJ_Signature = trophy_URL_temp

            Signature = GetSignature()

            trophy_URL = GJ_URL & "/trophies/?game_id=" & CStr(GJ_gameID) & "&" & "username=" & GJ_UserName _
                 & "&" & "user_token=" & GJ_Token & "&" & "signature=" & Signature
        Else
            trophy_URL_temp = GJ_URL & "/trophies/?game_id=" & CStr(GJ_gameID) & "&" & "username=" & GJ_UserName _
                   & "&" & "user_token=" & GJ_Token
            ' If there is more than one ID to be returned
            url_part = "&" & "trophy_id="
            For Each tr In trophyID
                url_part += tr & ","
            Next
            tr = url_part.Substring(0, url_part.Count() - 1)

            url_part += GJ_private_key
            trophy_URL_temp += url_part
            GJ_Signature = trophy_URL_temp

            Signature = GetSignature()

            trophy_URL = GJ_URL & "/trophies/?game_id=" & CStr(GJ_gameID) & "&" & "username=" & GJ_UserName _
                   & "&" & "user_token=" & GJ_Token & "&" & "signature=" & Signature
        End If
        Return trophy_URL
    End Function

    ' *** Use this function to fetch a trophy from the game ***
    Public Function fetchTrophy(ByVal value As Object) 'Define the type, Object is the default. Fixed types are not recommended. 
        Dim handleRequest As WebRequest
        handleRequest = WebRequest.Create(fetchTrophyURL(value))

        Dim stream As Stream
        stream = handleRequest.GetResponse.GetResponseStream()

        Dim streamRd As New StreamReader(stream)

        Dim response As String = streamRd.ReadToEnd()

        Return response
    End Function

    ' *** Get the URL call to achieve trophies ***
    ' Note: You do not need to call this function, this returns the URL for "add_TrophyAchieve"
    Public Function add_TrophyAchieveURL(ByVal trophy_ID As Integer)
        Dim trophy_URL_temp As String
        Dim trophy_URL As String
        Dim Signature As String
        trophy_URL_temp = GJ_URL & "/trophies/add-achieved/?game_id=" & CStr(GJ_gameID) & "&" & "username=" & GJ_UserName _
            & "&" & "user_token=" & GJ_Token & "&" & trophy_ID & GJ_private_key
        GJ_Signature = trophy_URL_temp

        Signature = GetSignature()

        trophy_URL = GJ_URL & "/trophies/add-achieved/?game_id=" & CStr(GJ_gameID) & "&" & "username=" & GJ_UserName _
            & "&" & "user_token=" & GJ_Token & "&" & "signature=" & Signature
        Return trophy_URL
    End Function

    ' *** Use this function to achieve a trophy ***
    ' Just enter the ID of the trophy you wish be achieved by the user
    Public Function add_TrophyAchieve(ByVal trophyID As Integer) As Boolean ' Set the ID of the trophy that needs to be achieved here
        Dim handleRequest As WebRequest
        handleRequest = WebRequest.Create(add_TrophyAchieveURL(trophyID))

        Dim stream As Stream
        stream = handleRequest.GetResponse.GetResponseStream()

        Dim streamRd As New StreamReader(stream)

        Dim response As String = streamRd.ReadToEnd()

        If InStr(response, "success:" & "true") = 1 Then
            Return True
        Else
            Return False
            MsgBox("error:" & response)
        End If
    End Function

    ' *** Scores **************************************
    ' All functions related to scores are found below
    ' *************************************************

    ' *** Get the URl call to fetch scores ***
    ' Note: You do not need to call this function, this returns the URL for "fetchScoresURL"
    Public Function fetchScoresURL(ByVal userInfo As Boolean, ByVal limit As Integer, ByVal tableID As Integer) As String

        Dim score_URL_temp As String
        Dim score_URL As String
        Dim Signature As String

        score_URL_temp = GJ_URL & "/scores/?game_id=" & CStr(GJ_gameID)
        score_URL = GJ_URL & "/scores/?game_id=" & CStr(GJ_gameID)

        ' If you wish to retrieve score information for specific users
        If userInfo = True Then
            score_URL_temp += "&" & "username=" & GJ_UserName & "&" & "user_token=" & GJ_Token
            score_URL += "&" & "username=" & GJ_UserName & "&" & "user_token=" & GJ_Token
        End If

        ' Limit the amount of scores you need to fetch and add to URL
        If limit > 0 Then
            ' Do not allow the limit to surpass 100
            If limit > 100 Then
                limit = 100
            End If
            score_URL_temp += "&" & "limit=" & CStr(limit)
            score_URL += "&" & "limit=" & CStr(limit)
        End If

        ' Add the table ID to the URL request 
        If tableID > 0 Then
            score_URL_temp += "&" & "table_id=" & CStr(tableID)
            score_URL += "&" & "table_id=" & CStr(tableID)
        End If

        score_URL_temp += GJ_private_key
        GJ_Signature = score_URL_temp
        Signature = GetSignature()

        score_URL += "&" & "signature=" & Signature

        Return score_URL
    End Function

    ' *** Use this function to fetch scores from your game's leader boards ***
    ' Enter True to return user scores and false for non user scores
    ' Enter the limit or number of scores you wish to fetch. *Note: the limit is 100
    ' Enter the table ID to specify the table you wish to fetch scores from. *Note: 0 is the default/primary table
    Public Function fetchScores(ByVal userInfo As Boolean, ByVal limit As Integer, ByVal tableID As Integer)
        Dim handleRequest As WebRequest
        handleRequest = WebRequest.Create(fetchScoresURL(userInfo, limit, tableID))

        Dim stream As Stream
        stream = handleRequest.GetResponse.GetResponseStream()

        Dim streamRd As New StreamReader(stream)

        Dim response As String = streamRd.ReadToEnd()

        Return response
    End Function

    ' *** Get the URL call to add scores to leader boards ***
    ' Note: You do not need to call this function, this is used to return the URL for "addScores"
    Public Function addScoresURL(ByVal score As Integer, ByVal sort As Integer, ByVal Guest As Boolean, _
                                 ByVal extraData As String, ByVal tableID As Integer) As String

        Dim score_URL_temp As String
        Dim score_URL As String
        Dim Signature As String

        score_URL_temp = GJ_URL & "/trophies/?game_id=" & CStr(GJ_gameID) & "&" & "score=" & score & "&" _
            & "sort=" & sort

        score_URL = GJ_URL & "/trophies/?game_id=" & CStr(GJ_gameID) & "&" & "score=" & score & "&" _
           & "sort=" & sort

        If Guest = False Then ' If the user is not a guest, use GJ account information
            score_URL_temp += "&" & "username=" & GJ_UserName & "&" & "token=" & GJ_Token
        Else
            score_URL_temp += "&" & "guest=" & Guest
        End If
        ' Check for any extra data
        If extraData <> "" Then
            score_URL_temp += "&" & "extra_data=" & extraData
        End If
        ' Add scores to a specific table in your game
        If tableID <> 0 Then
            score_URL_temp += "&" & "table_id=" & CStr(tableID)
        End If
        score_URL_temp += GJ_private_key
        GJ_Signature = score_URL_temp
        Signature = GetSignature()

        score_URL += "&" & "signature=" & Signature

        Return score_URL
    End Function

    ' *** Use this function to add user scores to leader boards within your game page ***
    Public Function addScores(ByVal score As Integer, ByVal sort As Integer, ByVal Guest As Boolean, _
                              ByVal extraData As String, ByVal tableID As Integer)

        Dim handleRequest As WebRequest
        handleRequest = WebRequest.Create(addScoresURL(score, sort, Guest, extraData, tableID))

        Dim stream As Stream
        stream = handleRequest.GetResponse.GetResponseStream()

        Dim streamRd As New StreamReader(stream)

        Dim response As String = streamRd.ReadToEnd()

        Return response
    End Function

    ' *** Get the URL call to fetch game tables ***
    ' Note: You do not need to call this function, this is to return the URL for "fetchTables"
    Public Function fetchTablesURL() As String

        Dim tables_URL_temp As String
        Dim tables_URL As String
        Dim Signature As String

        tables_URL_temp = GJ_URL & "/scores/tables/?game_id=" & CStr(GJ_gameID) & GJ_private_key

        Signature = GetSignature()

        tables_URL = GJ_URL & "/scores/tables/?game_id=" & CStr(GJ_gameID) & "&" & "signature=" & Signature

        Return tables_URL
    End Function

    ' *** Use this function to download tables from your game ***
    Public Function fetchTables()

        Dim handleRequest As WebRequest
        handleRequest = WebRequest.Create(fetchTablesURL())

        Dim stream As Stream
        stream = handleRequest.GetResponse.GetResponseStream()

        Dim streamRd As New StreamReader(stream)

        Dim response As String = streamRd.ReadToEnd()

        Return response
    End Function

    ' *** Data store **************************************
    ' All functions related to data storing are found below
    '******************************************************

    ' *** Get the URL call for fetching data keys from the data store ***
    ' Note: You do not need to call this function, this is used to return the URL for "fetchDataStorage"
    Public Function fetchDataStorageURL(ByVal key As String, ByVal userInfo As Boolean) As String

        Dim data_URL_temp As String
        Dim data_URL As String
        Dim Signature As String

        data_URL_temp = GJ_URL & "/data-store/?format=dump" & "&" & "game_id=" & CStr(GJ_gameID) & "&" & _
            "key=" & key
        data_URL = GJ_URL & "/data-store/?format=dump" & "&" & "game_id=" & CStr(GJ_gameID) & "&" & _
            "key=" & key
        ' Fetch data items for users only
        If userInfo = True Then
            data_URL_temp += "&" & "username=" & GJ_UserName & "&" & "user_token=" & GJ_Token
            data_URL += "&" & "username=" & GJ_UserName & "&" & "user_token=" & GJ_Token
        End If
        data_URL_temp += GJ_private_key
        GJ_Signature = data_URL_temp
        Signature = GetSignature()

        data_URL += "&" & "signature=" & Signature

        Return data_URL
    End Function

    '*** Use this function to fetch data stored on GJ servers ***
    ' To fetch specific user data, then have userInfo as True
    Public Function fetchDataStorage(ByVal key As String, ByVal userInfo As Boolean)

        Dim handleRequest As WebRequest
        handleRequest = WebRequest.Create(fetchDataStorageURL(key, userInfo))

        Dim stream As Stream
        stream = handleRequest.GetResponse.GetResponseStream()

        Dim streamRd As New StreamReader(stream)

        Dim response As String = streamRd.ReadToEnd()

        Return response
    End Function

    ' *** Get the URL call to set data storage ***
    ' Note: You do not need to call this function, this returns the URL for "setDataStorage"
    Public Function setDataStorageURL(ByVal key As String, ByVal userInfo As Boolean, ByVal data As String) As String

        Dim data_URL_temp As String
        Dim data_URL As String
        Dim Signature As String

        data_URL_temp = GJ_URL & "/data-store/set/?game_id=" & CStr(GJ_gameID) & "&" & "key=" & key _
            & "&" & "data=" & data
        data_URL = GJ_URL & "/data-store/set/?game_id=" & CStr(GJ_gameID) & "&" & "key=" & key _
        & "&" & "data=" & data

        ' Store data items for users only
        If userInfo = True Then
            data_URL_temp += "&" & "username=" & GJ_UserName & "&" & "user_token=" & GJ_Token
            data_URL += "&" & "username=" & GJ_UserName & "&" & "user_token=" & GJ_Token
        End If

        data_URL_temp += GJ_private_key
        GJ_Signature = data_URL_temp
        Signature = GetSignature()

        data_URL += "&" & "signature=" & GJ_Signature

        Return data_URL
    End Function

    ' *** Set data in the data storage ***
    ' To set specific data within the data store, have userInfo as True
    Public Function setDataStorage(ByVal key As String, ByVal userInfo As Boolean, ByVal data As String)

        Dim handleRequest As WebRequest
        handleRequest = WebRequest.Create(setDataStorageURL(key, userInfo, data))

        Dim stream As Stream
        stream = handleRequest.GetResponse.GetResponseStream()

        Dim streamRd As New StreamReader(stream)

        Dim response As String = streamRd.ReadToEnd()

        Return response
    End Function

    ' *** Get the URL call to remove data storage keys ***
    ' Note: You do not need to call this function, this returns the URL for "removeDataStorage"
    Public Function removeDataStorageURL(ByVal key As String, ByVal userInfo As Boolean) As String

        Dim data_URL_temp As String
        Dim data_URL As String
        Dim Signature As String

        data_URL_temp = GJ_URL & "/data-store/remove/?game_id=" & CStr(GJ_gameID) & "&" & "key=" & key
        data_URL = GJ_URL & "/data-store/remove/?game_id=" & CStr(GJ_gameID) & "&" & "key=" & key

        ' Remove data items for users only
        If userInfo = True Then
            data_URL_temp += "&" & "username=" & GJ_UserName & "&" & "user_token=" & GJ_Token
            data_URL += "&" & "username=" & GJ_UserName & "&" & "user_token=" & GJ_Token
        End If

        data_URL_temp += GJ_private_key
        GJ_Signature = data_URL_temp
        Signature = GetSignature()

        data_URL += "&" & "signature=" & Signature

        Return data_URL
    End Function

    ' *** This function removes current data in the data store for your game ***
    ' To remove specific user data in the data store, have userInfo as True
    Public Function removeDataStorage(ByVal key As String, ByVal userInfo As Boolean)

        Dim handleRequest As WebRequest
        handleRequest = WebRequest.Create(removeDataStorageURL(key, userInfo))

        Dim stream As Stream
        stream = handleRequest.GetResponse.GetResponseStream()

        Dim streamRd As New StreamReader(stream)

        Dim response As String = streamRd.ReadToEnd()

        Return response
    End Function

    ' *** Gets the URL call to get data storage keys in the data store ***
    ' Note: You do not need to call this function, this returns the URL for "getKeysDataStorage"
    Public Function getKeysDataStorageURL(ByVal userInfo As Boolean) As String

        Dim data_URL_temp As String
        Dim data_URL As String
        Dim Signature As String

        data_URL_temp = GJ_URL & "/data-store/get-keys/?game_id=" & CStr(GJ_gameID)
        data_URL = GJ_URL & "/data-store/get-keys/?game_id=" & CStr(GJ_gameID)

        If userInfo = True Then
            data_URL_temp += "&" & "username=" & GJ_UserName & "&" & "user_token=" & GJ_Token
            data_URL += "&" & "username=" & GJ_UserName & "&" & "user_token=" & GJ_Token
        End If

        data_URL_temp += GJ_private_key
        GJ_Signature = data_URL_temp
        Signature = GetSignature()

        data_URL += "&" & "signature=" & Signature

        Return data_URL
    End Function

    ' *** This function returns all the data keys in your game's data storage  ***
    ' To return data keys for users, then have the userInfo argument as True
    Public Function getKeysDataStorage(ByVal userInfo As Boolean) As Boolean

        Dim handleRequest As WebRequest
        handleRequest = WebRequest.Create(getKeysDataStorageURL(userInfo))

        Dim stream As Stream
        stream = handleRequest.GetResponse.GetResponseStream()

        Dim streamRd As New StreamReader(stream)

        Dim response As String = streamRd.ReadToEnd()

        If InStr(response, "success:" & "true") = 1 Then
            Return True
        Else
            Return False
            MsgBox("error:" & response)
        End If
    End Function

End Class
