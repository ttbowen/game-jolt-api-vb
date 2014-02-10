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

6. Miscellaneous 
  - New(ByVal private_key As String, ByVal gameID As Integer)

Game sessions/Users
=============
fetchUser(ByVal users As List(Of String), ByVal user As Integer)

1. **authenticateUser()** -
This function authenticates the user with the user's credentials and opens a new game session if successful. This function returns a  boolean value,  true if credentials are valid and false if they are not.


2. **GJ_Logout()** -
This sub logs the user out of the current session and resets the user's username and token. Call this at the point where you need to log the user out.

3. **pingSession(ByVal status As Boolean)**
This function pings an open session to test if the user is still active or not. This function takes 1 argument called "status" a boolean type, assign false to this "status" if the user is idle and true if the user is active.
If you do not call this function, within 120 seconds the session will automatically close, so it is recommended that you call this function every 30 seconds. A string type is returned from this function - the API response.

4. **openSession()** -
This function opens a new session once the user has been successfully authenticated. Note: This function will have been called at least once, if you have already called the "authenticateUser()" function. A string type is returned from this function - the API response.

5. **closeSession()** -
This function closes an open session with the user, which is also called when you call the "GJ_Logout()" subroutine. A string type is returned from this function - the API response. 

6. setUserType(ByVal user As user_type)


7. getUserType()


Getters/Setters
=============
Trophies
=============
Highscores
=============
Data storage
=============
Miscellaneous
=============
