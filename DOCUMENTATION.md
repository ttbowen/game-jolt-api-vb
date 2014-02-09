Documentation
=============
This class will allow you to implement trophies, highscores, data storage and game sessions into your games with the 
Gamejolt API with VB.net. This document will help you to use all of the functions contained within this class and give you an insight of their uses. The documentation here will only provide information on the functions/subs/properties you need to call in your game, the rest contained in the class you do not call. The comments will explain the reason for the function/sub but do not modify it unless really needed.

Contents
=============
1. Game sessions/Users
  - fetchUser(ByVal users As List(Of String), ByVal user As Integer)
  - authenticateUser()
  - GJ_Logout()
  - pingSession(ByVal status As Boolean)
  - openSession()
  - closeSession
  - setUserType(ByVal user As user_type)
  - getUserType()

2. Getters/Setters
  - SetUserName()
  - SetToken()  
  - getUserName()
  - getToken()
  - getGameID()
  - isLoggedIn()

3. Trophies
  - fetchTrophy()
  - fetchTrophy(ByVal value As Object)
  - add_TrophyAchieve(ByVal trophyID As Integer)

4. Highscores
  - fetchScores(ByVal userInfo As Boolean, ByVal limit As Integer, ByVal tableID As Integer)
  - addScores(ByVal score As Integer, ByVal sort As Integer, ByVal Guest As Boolean, ByVal extraData As String, ByVal     tableID As Integer)
  - fetchTables()
  - setDataStorage(ByVal key As String, ByVal userInfo As Boolean, ByVal data As String)

5. Data storage
  - fetchDataStorage(ByVal key As String, ByVal userInfo As Boolean)
  - setDataStorage(ByVal key As String, ByVal userInfo As Boolean, ByVal data As String)
  - removeDataStorage(ByVal key As String, ByVal userInfo As Boolean)
  - getKeysDataStorage(ByVal userInfo As Boolean) As Boolean

6. miscellaneous 
  - New(ByVal private_key As String, ByVal gameID As Integer)

Game sessions/Users
=============

Getters/Setters
